using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Diagnostics;
using SysConf = System.Configuration;

namespace MDM.Workflow.TrackingManager.Business
{
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Workflow;
    using MDM.ConfigurationManager.Business;
    using MDM.Workflow.TrackingManager.Data;
    using MDM.Workflow.PersistenceManager.Business;
    using MDM.BusinessObjects.Diagnostics;

    using MDM.Core;
    using MDM.JigsawIntegrationManager;

    /// <summary>
    /// Business Logic for Instance Tracking Record
    /// </summary>
    public class InstanceTrackingBL : BusinessLogicBase
    {
        #region Fields

        private TraceSettings _currentTraceSettings = null;

        #endregion

        public InstanceTrackingBL()
        {
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
        }

        /// <summary>
        /// Adds new Instance Tracking Record.
        /// </summary>
        /// <param name="instanceTrackingRecord">Instance Tracking Record to be added</param>
        /// <param name="context">Context which tells which application/module called this API</param>
        /// <returns>Boolean result of operation completeness</returns>
        public Boolean Create(InstanceTracking instanceTrackingRecord, CallerContext context)
        {
            Boolean result = false;
            DiagnosticActivity diagnosticActivity = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "InstanceTrackingBL.Create");
                diagnosticActivity.Start();
            }

            try
            {
                instanceTrackingRecord.ProgramName = context.ProgramName;

                //Get Command
                DBCommandProperties command = DBCommandHelper.Get(context, Core.MDMCenterModuleAction.Create);
                
                InstanceTrackingDA instanceTrackingRecordDA = new InstanceTrackingDA();
                Int32 rowsAffected = instanceTrackingRecordDA.Create(instanceTrackingRecord, command);

                //if (JigsawConstants.IsJigsawIntegrationEnabled)
                //{
                //    JigsawIntegrationHelper.SendToJigsaw(instanceTrackingRecord.RuntimeInstanceId, instanceTrackingRecord.Status, context);
                //}

                if (rowsAffected > 0)
                {
                    result = true;
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

            return result;
        }

        /// <summary>
        /// Updates Instance Tracking Record
        /// </summary>
        /// <param name="instanceTrackingRecord">Instance Tracking Record to be updated</param>
        /// <param name="context">Context which tells which application/module called this API</param>
        /// <returns>Boolean result of operation completeness</returns>
        public Boolean Update(InstanceTracking instanceTrackingRecord, CallerContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes existing Instance Tracking Record
        /// </summary>
        /// <param name="instanceTrackingRecord">Instance Tracking Record to be deleted</param>
        /// <param name="context">Context which tells which application/module called this API</param>
        /// <returns>Boolean result of operation completeness</returns>
        public Boolean Delete(InstanceTracking instanceTrackingRecord, CallerContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get Instance Tracking Record by id
        /// </summary>
        /// <param name="id"> Id of the record to be fetched</param>
        /// <param name="context">Context which tells which application/module called this API</param>
        /// <returns>InstanceTrackingRecord</returns>
        public InstanceTracking GetById(int id, CallerContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets instances for which waiting timer has been elapsed
        /// </summary>
        /// <returns>Collection of Instance Ids for which waiting timer has been elapsed</returns>
        public Collection<String> GetElapsedTimerInstances()
        {
            Collection<String> elapsedTimerInstances = null;
            Boolean serverSplitEnabled = false;
            DBCommandProperties command = null;

            DiagnosticActivity diagnosticActivity = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "InstanceTrackingBL.GetElapsedTimerInstances");
                diagnosticActivity.Start();
            }


            try
            {
                #region Prepare DB Command


                //This operation needs to be performed on Workflow Database as the procedure runs on MS workflow tables.
                //Find the connection string..

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Preparing DB command..");
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Getting AppConfig 'MDMCenter.PhysicalServerSplit.Enabled'..");
                }
                try
                {
                    String strSplitEnabled = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.PhysicalServerSplit.Enabled");
                    serverSplitEnabled = ValueTypeHelper.ConvertToBoolean(strSplitEnabled);

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "AppConfig key get completed.");
                }
                catch (Exception ex)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Failed to get AppConfig. Error: {0} Considering PhysicalServerSplit as disabled.", ex.Message));
                }

                //If server split is enabled, take connection string from db else take it from separate WF connection string.
                if (serverSplitEnabled)
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Server Split is enabled. Preparing DB command from server split configuration..");

                    /*
                     * For DBCommanProperties, we are hard-coding application, module and action here.
                     * The reason behind this is, "SqlWorkflowInstanceStore" is used by WWF internals for their own tracking. So these tracking has to be offloaded to another server.
                     * The connection string for this has to be fetched from tb_ConnectionString table based on the configuration.
                     */

                    //Get Connection String from configuration.
                    command = DBCommandHelper.Get(MDMCenterApplication.WindowsWorkflow, MDMCenterModules.WindowsWorkflow, MDMCenterModuleAction.Read);
                }
                else
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Server Split is disabled. Preparing DB command from MSWorkflowConnectionString..");

                    command = new DBCommandProperties();
                    command.ConnectionString = SysConf.ConfigurationManager.AppSettings.Get("MSWorkflowConnectionString");
                }

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DB command preparation completed.");

                #endregion

                InstanceTrackingDA instanceTrackingDA = new InstanceTrackingDA();
                elapsedTimerInstances = instanceTrackingDA.GetElapsedTimerInstances(command);
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

            return elapsedTimerInstances;
        }
    }
}