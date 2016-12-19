using System;
using System.Linq;

namespace MDM.Workflow.AssignmentManager.Business
{
	using MDM.AdminManager.Business;
	using MDM.BusinessObjects;
	using MDM.BusinessObjects.Diagnostics;
	using MDM.Core;
	using MDM.EntityManager.Business;
	using MDM.Interfaces;
	using MDM.LookupManager.Business;
	using MDM.Utility;
	using MDM.ExpressionParser;
	using MDM.ExpressionParser.Data;
	
	public class AssignmentRuleManager : BusinessLogicBase
    { 
		#region Fields

        private const String WorkflowAsssignmentRule = "Workflow_AssignmentRule";

        private Lazy<LookupBL> LookupManager {get  {return new Lazy<LookupBL>(() => new LookupBL());} }

        private Lazy<CallerContext> CallContext { get { return new Lazy<CallerContext>(() => (CallerContext)MDMObjectFactory.GetICallerContext()); } }

		private readonly TraceSettings _traceSettings;

		private DiagnosticActivity _diagnosticActivity;

		#endregion Fields

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public AssignmentRuleManager()
		{
			_traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
			_diagnosticActivity = new DiagnosticActivity();
		}

		#endregion Constructors

		#region Properties

		#endregion Properties

		#region Methods

		#region Public Methods

		/// <summary>
		/// Get the Assigned User based on the rules in the Workflow_AssignmentRule table
		/// </summary>
		/// <param name="workflowDataContext"></param>
		/// <param name="activityDisplayName"></param>
		/// <param name="currentActivityName"></param>
		/// <returns></returns>
        public SecurityUser GetAssignedUser(IWorkflowDataContext workflowDataContext, String activityDisplayName, String currentActivityName)
        {
			if (_traceSettings.IsTracingEnabled)
			{
				_diagnosticActivity.ActivityName = "MDM.Workflow.AssignmentManager.Business.AssignmentRuleManager.GetAssignedUser";
				_diagnosticActivity.Start();
			}

            SecurityUser assignedUser = null;
            var workflowName = workflowDataContext.WorkflowName;
            var mdmObjectColection = workflowDataContext.GetWorkflowMDMObjectCollection();

			try
			{
				//Get the lookup data for the workflow 
				var assignmentLkpRows = LookupManager.Value.GetLookupRows(WorkflowAsssignmentRule, GlobalizationHelper.GetSystemDataLocale(), null, -1, CallContext.Value);

				if (assignmentLkpRows == null)
				{
					if (_traceSettings.IsTracingEnabled)
					{
						_diagnosticActivity.LogWarning(String.Format("Lookup table {0} for workflow assignment rules does npt exist.", WorkflowAsssignmentRule));
					}
					return null;
				}

				if (!assignmentLkpRows.Any())
				{
					if (_traceSettings.IsTracingEnabled)
					{
						_diagnosticActivity.LogWarning(String.Format("Lookup table {0} for workflow assignment rules does not have any assignments defined.", WorkflowAsssignmentRule));
					}
					return null;
				}

				var assignmentRows = (from ar in assignmentLkpRows
									  let lworkflowName = ar.GetValue("WorkflowName").ToString()
									  let lactivityName = ar.GetValue("WorkflowActivityName").ToString()
									  where lworkflowName.Equals(workflowName, StringComparison.InvariantCultureIgnoreCase)
									  && lactivityName.Equals(activityDisplayName, StringComparison.InvariantCultureIgnoreCase)
									  select ar).ToList();

				if (!assignmentRows.Any())
				{
					if (_traceSettings.IsTracingEnabled)
					{
						_diagnosticActivity.LogInformation(String.Format("Lookup table {0} for workflow assignment rules does not have any assignments defined for workflow {1}, Activity {2}.", WorkflowAsssignmentRule, workflowName, activityDisplayName));
					}
					return null;
				}

				Boolean assignmentConditionValidated;
				String conditionExpression;
				String assignToUser;

				var entityContext = new EntityContext
				{
					LoadEntityProperties = true,
					LoadAttributes = true,
					AttributeModelType = AttributeModelType.Common,
					LoadAttributeModels = true
				};

				var entityBL = new EntityBL();
				Int64 mdmObjectId = mdmObjectColection.First().MDMObjectId;
				var entity = entityBL.Get(mdmObjectId, entityContext, MDMCenterApplication.PIM, MDMCenterModules.Entity, false, false);

				ExpressionParserManager parserManager;
				var securityManager = new SecurityUserBL();

				foreach (var ar in assignmentRows.OrderBy(r => Int32.Parse(r.GetValue("Sequence").ToString())))
				{
					assignmentConditionValidated = true;
					conditionExpression = (String)ar.GetValue("Condition") ?? String.Empty;
					assignToUser = ar.GetValue("UserLogin").ToString();

					//Process Conditional expression if the condition is not false
					if (!String.IsNullOrEmpty(conditionExpression))
					{
						parserManager = new ExpressionParserManager();
						var expProcessor = parserManager.Get(conditionExpression);

						assignmentConditionValidated = expProcessor.Evaluate<Boolean>(new DefaultExpressionTokenDataProcessor(expProcessor, entity));
					}

					if (assignmentConditionValidated)
					{
						if (String.Compare(assignToUser, "##CreateUser##") != 0)
						{
							assignedUser = securityManager.GetUser(assignToUser);
						}
						else
						{
							assignedUser = securityManager.GetUser(workflowDataContext.ExtendedProperties);
						}

						if (assignedUser != null)
						{
							//Set the assigned user to null in case the assigned user is System.
							if (String.Compare(assignedUser.SecurityUserLogin, "System", StringComparison.InvariantCultureIgnoreCase) == 0)
							{
								assignedUser = null;
							}
							break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (_traceSettings.IsTracingEnabled)
				{
					_diagnosticActivity.LogError(String.Format("Error occurred while processing workflow assignment rules using the lookup table. Error: {0}", ex.Message));
				}

				throw;
			}
			finally
			{
				if (_traceSettings.IsTracingEnabled)
				{
					_diagnosticActivity.Stop();
				}
			}

            return assignedUser;
        }

		#endregion Public Methods

		#endregion Methods
	}
}
