using System;
using System.Activities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Workflow.Activities.Entity
{
    using MDM.Core;
    using MDMBO = MDM.BusinessObjects;
    using MDMBOW = MDM.BusinessObjects.Workflow;
    using MDM.EntityManager.Business;
    using MDM.Workflow.Activities.Core;
    using MDM.Workflow.Activities.Designer;

    using MDM.Interfaces;
    using MDM.Utility;

    ///<summary>
    /// Returns an instance of the MDM Entity object using Entity Id
    ///</summary>
    [Designer(typeof (CheckforReApprovalDesigner))]
    [ToolboxBitmap(typeof (CheckforReApprovalDesigner), "Images.CheckforReapproval.bmp")]
    public class CheckforReApproval : MDMCodeActivitiyBase<Boolean>
    {

        ///<summary>
        ///</summary>
		[DisplayName(@"Re-Approval Status")]
        [Category("Output Arguments")]
        public OutArgument<Boolean> ReApprovalStatus { get; set; }

        #region Methods
        /// <summary>
        /// Fetch the entity reapproval status and resets the value to false.
        /// </summary>
        /// <returns>
        /// The result of the activity’s execution.
        /// </returns>
        /// <param name="context">The execution context under which the activity executes.</param>
        protected override Boolean Execute(CodeActivityContext context)
        {
            MDMBOW.WorkflowDataContext wfDataContext = MDMDataContext.Get(context);

            if (wfDataContext == null)
            {
                ReApprovalStatus.Set(context, false);
                return false;
            }

            //Get the entity participating in the workflow
            MDMBOW.WorkflowMDMObject mdmObject = null;

            if (wfDataContext.MDMObjectCollection.Count > 0)
            {
                mdmObject = wfDataContext.MDMObjectCollection.First();
            }

            if (mdmObject == null)
            {
                ReApprovalStatus.Set(context, false);
                return false;
            }

            EntityBL entityBL = new EntityBL();


            var entityContext = new MDMBO.EntityContext
            {
                ContainerId = 0,
                CategoryId = 0,
                EntityTypeId = 0,
                LoadEntityProperties = true,
                LoadAttributes = true,
                AttributeModelType = AttributeModelType.All,
                AttributeIdList = new Collection<int> { (Int32) SystemAttributes.EntityNeedsReApproval },
                LoadRelationships = false,
                LoadCreationAttributes = false,
                LoadRequiredAttributes = false,
                LoadHierarchyRelationships = false
            };

            var entity = entityBL.Get(mdmObject.MDMObjectId, entityContext, wfDataContext.Application, wfDataContext.Module, false, false);

            if (entity == null)
            {
                ReApprovalStatus.Set(context, false);
                return false;
            } 
            
            IAttribute reapprovalAttribute = entity.GetAttribute((Int32) SystemAttributes.EntityNeedsReApproval,GlobalizationHelper.GetSystemDataLocale());

            if (reapprovalAttribute == null)
            {
                ReApprovalStatus.Set(context, false);
                return false;
            }


            Object reApprovalStatusValue = reapprovalAttribute.GetCurrentValue();

            if (reApprovalStatusValue == null)
            {
                ReApprovalStatus.Set(context, false);
                return false;
            }

            Boolean entityReapprovalStatus = reApprovalStatusValue.ToString().Equals("Yes", StringComparison.InvariantCultureIgnoreCase); //entity.ReApprovalStatus;
            
            //Reset the 
            reapprovalAttribute.SetValue("No");

            if (entityReapprovalStatus)
            {
                entity.Action = MDM.Core.ObjectAction.Update;

                MDMBO.EntityProcessingOptions entityProcessingOptions = new MDMBO.EntityProcessingOptions
                {
                    ApplyAVS = false,
                    PublishEvents = false
                };

                ReApprovalStatus.Set(context, true);

                var entityOperationResult = entityBL.Update(entity, entityProcessingOptions, "CheckforReApproval", wfDataContext.Application, wfDataContext.Module);

                if (entityOperationResult != null && entityOperationResult.OperationResultStatus == OperationResultStatusEnum.Failed)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

    }
}
