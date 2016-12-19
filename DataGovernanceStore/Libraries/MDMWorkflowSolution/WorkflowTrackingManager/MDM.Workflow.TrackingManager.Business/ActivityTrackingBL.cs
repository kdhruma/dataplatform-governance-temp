using System;
using System.Activities.Tracking;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MDM.Workflow.TrackingManager.Business
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.BusinessObjects.Workflow;
    using MDM.ConfigurationManager.Business;
    using MDM.EntityManager.Business;
    using MDM.ExceptionManager;
    using MDM.Utility;

    using MDM.Workflow.TrackingManager.Data;
    using MDM.Workflow.Utility;
    using MDM.Workflow.PersistenceManager.Business;
    using MDM.Workflow.Designer.Business;
    
    using MessageBrokerManager;
    using MDM.JigsawIntegrationManager;
    using MDM.JigsawIntegrationManager.DataPackages;
    using MDM.Interfaces;

    /// <summary>
    /// Business Logic for Activity Tracking Record
    /// </summary>
    public class ActivityTrackingBL : BusinessLogicBase
    {
        #region Fields

        private TraceSettings _currentTraceSettings = null;

        #endregion

        #region Constructors

        public ActivityTrackingBL()
        {
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Adds Activity Tracking Record.
        /// </summary>
        /// <param name="activityTrackingRecord">Activity Tracking Record</param>
        /// <param name="context">Context which tells which application/module called this API</param>
        /// <param name="wfActionContext">Object having details of the performed action. This can be null.</param>
        /// <returns>Boolean result of operation completeness</returns>
        public Boolean Create(TrackedActivityInfo activityTrackingRecord, CallerContext context, WorkflowActionContext wfActionContext = null)
        {
            Boolean result = false;

            DiagnosticActivity diagnosticActivity = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "ActivityTrackingBL.Create");
                diagnosticActivity.Start();
            }

            try
            {
                #region Step: Save tracked activity details

                //Todo:: Add null check before assign program name.If activityTrackingRecord is null throw error.
                activityTrackingRecord.ProgramName = context.ProgramName;

                //Get command 
                DBCommandProperties command = DBCommandHelper.Get(context, Core.MDMCenterModuleAction.Create);

                ActivityTrackingDA activityTrackingRecordDA = new ActivityTrackingDA();
                int rowsAffected = activityTrackingRecordDA.Create(activityTrackingRecord, command);

                if (rowsAffected > 0)
                {
                    result = true;
                }

                #endregion

                #region Step: Publish Entity Transitioned Event

                //When Activity Record with state as Executing is received, this completes the activity transition 
                //from the previous activity to current activity. Since the transition happens in ASync manner, this is 
                //appropriate position to trigger Entity Transitioned event.

                //Check for HumanActivity and ActivityRecord state and if it is executing, trigger event
                //if (activityTrackingRecord.IsHumanActivity && activityTrackingRecord.Status == ActivityStates.Executing)
                //{
                //    PublishEntityTransitionedEvent(activityTrackingRecord, wfActionContext, context);
                //}

                #endregion
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    if (diagnosticActivity != null)
                    {
                        diagnosticActivity.Stop();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Updates Activity Tracking Record
        /// </summary>
        /// <param name="activityTrackingRecord">Activity Tracking Record to be updated</param>
        /// <param name="context">Context which tells which application/module called this API</param>
        /// <returns>Boolean result of operation completeness</returns>
        public Boolean Update(TrackedActivityInfo activityTrackingRecord, CallerContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes existing Activity Tracking Record
        /// </summary>
        /// <param name="activityTrackingRecord">Activity Tracking Record to be deleted</param>
        /// <param name="context">Context which tells which application/module called this API</param>
        /// <returns>Boolean result of operation completeness</returns>
        public Boolean Delete(TrackedActivityInfo activityTrackingRecord, CallerContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get Activity Tracking Record by id
        /// </summary>
        /// <param name="id"> Id of the record to be fetched</param>
        /// <param name="context">Context which tells which application/module called this API</param>
        /// <returns>ActivityTrackingRecord</returns>
        public TrackedActivityInfo GetById(Int32 id, CallerContext context)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods

        private void PublishEntityTransitionedEvent(TrackedActivityInfo activityTrackingRecord, WorkflowActionContext wfActionContext, CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = new DiagnosticActivity();
                diagnosticActivity.Start();
            }

            try
            {
                //Get entity details...
                WorkflowMDMObject mdmObject = null;
                WorkflowMDMObjectCollection mdmObjects = activityTrackingRecord.MDMObjectCollection;

                if (mdmObjects != null && mdmObjects.Count > 0)
                {
                    mdmObject = mdmObjects.First();
                }

                //Checking UserAction to avoid event publishing on First Activity of Workflow
                if (mdmObject != null && ((wfActionContext != null && !String.IsNullOrWhiteSpace(wfActionContext.UserAction))))
                {
                    //Create entityContext
                    EntityContext entityContext = new EntityContext();
                    entityContext.LoadWorkflowInformation = true;
                    entityContext.LoadEntityProperties = true;
                    entityContext.DataLocales.Add(GlobalizationHelper.GetSystemDataLocale());

                    EntityBL entityBL = new EntityBL();
                    var entity = entityBL.Get(mdmObject.MDMObjectId, entityContext, callerContext.Application, callerContext.Module, false, false);

                    if (entity != null)
                    {
                        if (wfActionContext != null)
                        {
                            entity.WorkflowActionContext = wfActionContext;

                            if (!String.IsNullOrWhiteSpace(wfActionContext.UserAction))
                            {
                                var entityOperationResult = new EntityOperationResult
                                {
                                    EntityId = entity.Id,
                                    Id = 1,
                                    ExternalId = entity.ExternalId,
                                    EntityLongName = entity.LongName,
                                    ReferenceId = entity.Id
                                };

                                EntityWorkflowOperationsCommonUtility.FireEntityWorkflowEvent(new EntityCollection() { entity }, new EntityOperationResultCollection { entityOperationResult },
                                    callerContext, EventSource.EntityTransitioned, entityBL, wfActionContext.ActingUserId);
                            }
                        }

                        if (JigsawConstants.IsJigsawIntegrationEnabled)
                        {
                            JigsawIntegrationHelper.SendToJigsaw(entity, activityTrackingRecord.WorkflowName, activityTrackingRecord.ActivityShortName, EventType.workflowTransitioned, callerContext);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Do not take any action on error...
                //Just log it.
                String exceptionMessage = String.Format("Unhandled exception occurred while publishing Entity Transitioned event. Error is:", ex.Message);
                ApplicationException appException = new ApplicationException(exceptionMessage, ex);
                new ExceptionHandler(appException);

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    String exceptionDetails = String.Format("Unhandled exception occurred while publishing Entity Transitioned event. Error is:", ex.ToString());
                    diagnosticActivity.LogError(exceptionDetails);
                }
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    if (diagnosticActivity != null)
                    {
                        diagnosticActivity.Stop();
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}
