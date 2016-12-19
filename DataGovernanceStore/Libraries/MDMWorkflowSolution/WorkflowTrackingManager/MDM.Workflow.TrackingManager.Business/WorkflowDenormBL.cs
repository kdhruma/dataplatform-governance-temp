//using System;

//namespace MDM.Workflow.TrackingManager.Business
//{
//    using MDM.Utility;
//    using MDM.BusinessObjects;
//    using MDM.BusinessObjects.Workflow;
//    using MDM.Core;
//    using MDM.DenormManager.Business;
//    using MDM.SearchManager.Business;
//    using System.Diagnostics;
//    using MDM.EntityManager.Business;

//    /// <summary>
//    /// 
//    /// </summary>
//    public class WorkflowDenormBL
//    {
//        #region Constructors
        
//        /// <summary>
//        /// 
//        /// </summary>
//        public WorkflowDenormBL()
//        {
//        }

//        #endregion

//        #region Methods

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="workflowMDMObjectCollection"></param>
//        public void Refresh(WorkflowMDMObjectCollection workflowMDMObjectCollection)
//        {
//            if (workflowMDMObjectCollection == null)
//                return;

//            DurationHelper durationHelper = new DurationHelper(DateTime.Now);

//            EntityBL entityBL = new EntityBL();
//            DenormEntityBL denormManager = new DenormEntityBL(entityBL);

//            foreach (WorkflowMDMObject workflowMDMObject in workflowMDMObjectCollection)
//            {
//                if (!workflowMDMObject.MDMObjectType.Equals("MDM.BusinessObjects.Entity"))
//                    continue;

//                if (workflowMDMObject.MDMObjectId < 1)
//                    continue;

//                Entity dniEntity = denormManager.RefreshWorkflowAttributes(workflowMDMObject.MDMObjectId, 0, new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Denorm));

//                //Disabled population of workflow attributes to dn_search table since search procedure takes data directly from core tables.
//                //if (dniEntity != null)
//                //{
//                //    EntitySearchDataBL searchManager = new EntitySearchDataBL(entityBL);
//                //    var result = searchManager.RefreshWorkflowAttributes(dniEntity, new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Denorm));
//                //}
//            }

//            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
//                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - ActivityTrackingBL-Method:UpdateDenorm", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.DenormProcess);
//        }

//        #endregion
//    }
//}
