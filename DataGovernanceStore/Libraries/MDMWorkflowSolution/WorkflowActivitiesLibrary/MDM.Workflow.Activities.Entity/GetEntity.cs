using System;
using System.Drawing;
using System.Activities;
using System.ComponentModel;
using System.Linq;
using System.Collections.ObjectModel;

namespace MDM.Workflow.Activities.Entity
{
    using MDM.Core;
    using MDMBO = MDM.BusinessObjects;
    using MDMBOW = MDM.BusinessObjects.Workflow;
    using MDM.EntityManager.Business;
    using MDM.Workflow.Activities.Core;
    using MDM.Workflow.Activities.Designer;

    ///<summary>
    /// Returns an instance of the MDM Entity object using Entity Id
    ///</summary>
    [Designer(typeof(GetEntityDesigner))]
    [ToolboxBitmap(typeof(GetEntityDesigner), "Images.GetEntity.bmp")]
	public class GetEntity : MDMCodeActivitiyBase<Boolean>
    {
		#region Fields

		private InArgument<String> _locale = "en_WW";

		#endregion

		#region Properties

		///<summary>
        ///</summary>
		[Category("Input Arguments")]
		[RequiredArgument]
		public InArgument<String> Locale
        {
			get { return _locale; }
			set { _locale = value; }
        }

		///<summary>
        ///</summary>
		[DisplayName(@"Entity Return Object")]
		[Category("Output Arguments")]
		public OutArgument<MDMBO.Entity> Entity { get; set; }

        /// <summary>
        /// Action Context in workflow client, provides action details for Activity
        /// </summary>
        [Browsable(false)]
        public new OutArgument<MDMBOW.WorkflowActionContext> MDMActionContext { get; set; }

		#endregion

		#region Methods
    	/// <summary>
    	/// When implemented in a derived class, performs the execution of the activity.
    	/// </summary>
    	/// <returns>
    	/// The result of the activity’s execution.
    	/// </returns>
    	/// <param name="context">The execution context under which the activity executes.</param>
    	protected override Boolean Execute(CodeActivityContext context)
		{
    		Object dataContext = MDMDataContext.Get(context);

            if (dataContext == null)
            {
                return false;
            }

			//Get the entity participating in the workflow
			MDMBOW.WorkflowDataContext wfDataContext = (MDMBOW.WorkflowDataContext) dataContext;
			MDMBOW.WorkflowMDMObject mdmObject = null;

            if (wfDataContext.MDMObjectCollection.Count > 0)
            {
                mdmObject = wfDataContext.MDMObjectCollection.First();
            }

            if (mdmObject == null)
            {
                return false;
            }

            EntityBL entityBL = new EntityBL();

            String strLocale = Locale.Get(context);
            LocaleEnum locale;
            Enum.TryParse(strLocale, out locale);

    		var entityContext = new MDMBO.EntityContext
    		{
    			ContainerId = 0,
    			CategoryId = 0,
    			EntityTypeId = 0,
    			Locale = locale,
    			DataLocales = new Collection<LocaleEnum>() {locale},
    			LoadEntityProperties = true,
    			LoadAttributes = false,
    			AttributeModelType = AttributeModelType.All,
    			AttributeIdList = null,
    			LoadRelationships = false,
    			LoadCreationAttributes = false,
    			LoadRequiredAttributes = false,
    			LoadHierarchyRelationships = false
    		};

			//TODO: Enhance get entity activity to pass entity context(what to load under an entity)
    		var entity = entityBL.Get(mdmObject.MDMObjectId, entityContext, wfDataContext.Application, wfDataContext.Module, false, false);

            if (entity == null)
            {
                return false;
            }

            Entity.Set(context, entity);

			return true;
		}

		#endregion
    }
}
