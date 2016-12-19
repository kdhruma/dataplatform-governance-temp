using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Transactions;

namespace MDM.MonitoringManager.Business
{
    using MDM.BusinessObjects;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.MonitoringManager.Data.SqlClient;
    using MDM.Utility;

    public class MonitorBL : BusinessLogicBase
    {
        #region Methods

        /// <summary>
        /// Process service status. Updates record in database for given server and service / service sub type
        /// </summary>
        /// <param name="serverName">Name of machine calling the API</param>
        /// <param name="service">Type of service</param>
        /// <param name="serviceSubType">Sub type of service</param>
        /// <param name="serviceStatusXml">Xml representing service status</param>
        /// <param name="processorConfigXml"></param>
        /// <param name="callerContext">Indicates who called the API</param>
        /// <returns>true - if operation was successful else false.</returns>
        public Boolean ProcessServiceStatus(String serverName, MDMServiceType service, MDMServiceSubType serviceSubType, String serviceStatusXml, String processorConfigXml, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("MonitoringManager.Business.MonitorBL.Process", false);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Monitor Service status Process starting..");
            }

            #region validations

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "MonitoringManager.MonitorBL", String.Empty, "Process");
            }

            if (serviceStatusXml == null)
            {
                throw new MDMOperationException("111931", "Service status Xml cannot be null.", "MonitoringManager.MonitorBL", String.Empty, "Process");
            }

            #endregion validations

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Server name = " + serverName);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Service = " + service.ToString());
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Service sub type = " + serviceSubType.ToString());
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext = Application : " + callerContext.Application + " ");
            }
            Boolean returnResult = true;
            DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Update);
            if (!String.IsNullOrEmpty(processorConfigXml))
            {
                processorConfigXml = UpdateProcessorConfigXml(GetProcessorConfigurationCollection(serverName, service, serviceSubType, callerContext), processorConfigXml);
            }

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                MonitorDA monitorDA = new MonitorDA();
                returnResult = monitorDA.ProcessServiceStatus(serverName, service, serviceSubType, serviceStatusXml,
                                                              processorConfigXml, command);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information,
                                              "Service status update result = " + returnResult.ToString());

                if (returnResult == true)
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Committing Transaction");
                    transactionScope.Complete();
                }
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information,
                                              "Monitor Service status Process transaction committed.");
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information,
                                      "Monitor Service status Process completed successfully.");
                MDMTraceHelper.StopTraceActivity("MonitoringManager.Business.MonitorBL.Process");
            }
            return returnResult;
        }


        /// <summary>
        /// Process service status. Updates record in database for given server and service / service sub type
        /// </summary>
        /// <param name="serverName">Name of machine calling the API</param>
        /// <param name="service">Type of service</param>
        /// <param name="serviceSubType">Sub type of service</param>
        /// <param name="engineConfigXml"></param>
        /// <param name="callerContext">Indicates who called the API</param>
        /// <returns>true - if operation was successful else false.</returns>
        public Boolean ProcessEngineStatus(String serverName, MDMServiceType service, MDMServiceSubType serviceSubType, String engineConfigXml, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("MonitoringManager.Business.MonitorBL.ProcessEngineStatus", false);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Monitor Service status Process starting..");
            }
            #region validations

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "MonitoringManager.MonitorBL", String.Empty, "ProcessEngineStatus");
            }


            #endregion validations

            Boolean returnResult = true;
            DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Update);

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                MonitorDA monitorDA = new MonitorDA();
                returnResult = monitorDA.ProcessServiceStatus(serverName, service, serviceSubType, String.Empty,
                                                              engineConfigXml, command);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information,
                                              "Service status update result = " + returnResult.ToString());

                if (returnResult == true)
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Committing Transaction");
                    transactionScope.Complete();
                }
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information,
                                              "Monitor Service status Process transaction committed.");
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information,
                                      "Monitor Service status Process completed successfully.");
                MDMTraceHelper.StopTraceActivity("MonitoringManager.Business.MonitorBL.ProcessEngineStatus");
            }
            return returnResult;
        }

        /// <summary>
        /// Get server status 
        /// </summary>
        /// <param name="callerContext">Indicates who called the API</param>
        /// <returns>Different server's staus</returns>
        public ServiceStatusCollection GetServiceStatus(CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("MonitoringManager.Business.MonitorBL.GetServiceStatus", false);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Monitor Service status Get starting..");
            }
            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "MonitoringManager.MonitorBL",
                                                String.Empty, "GetServiceStatus");
            }

            MonitorDA monitorDA = new MonitorDA();
            ServiceStatusCollection serviceStatusCollection = new ServiceStatusCollection();

            DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module,
                                                              MDMCenterModuleAction.Read);

            serviceStatusCollection = monitorDA.GetServiceStatus(command);

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information,
                                      "Monitor Service status Get completed successfully.");
                MDMTraceHelper.StopTraceActivity("MonitoringManager.Business.MonitorBL.GetServiceStatus");
            }
            return serviceStatusCollection;
        }

        /// <summary>
        /// Get Data processor status Collection
        /// </summary>
        /// <param name="serverName">Name of the server</param>
        /// <param name="serviceType">Name of the service</param>
        /// <param name="serviceSubType">Name of the service sub type</param>
        /// <param name="callerContext">Indicates who called the API</param>
        /// <returns>the collection of data processor status</returns>
        public DataProcessorStatusCollection GetDataProcessorsStatus(String serverName, MDMServiceType serviceType, MDMServiceSubType serviceSubType, CallerContext callerContext)
        {
            ServiceStatusCollection serviceStatusCollection = GetServiceStatus(callerContext);
            var serviceStatus =
                serviceStatusCollection.FirstOrDefault(status => status.Server == serverName && status.Service == serviceType &&
                                                                 status.ServiceSubType == serviceSubType);
            if (serviceStatus != null)
            {
                ParallelizationEngineStatus parallelizationEngineStatus =
                    new ParallelizationEngineStatus(serviceStatus.ServiceStatusXML);
                return parallelizationEngineStatus.DataProcessorStatusCollection;
            }
            return null;
        }

        /// <summary>
        /// Gets the configuration collection for all the processors of a given server and service
        /// </summary>
        /// <param name="serverName">Name of the server</param>
        /// <param name="service">Name of the service</param>
        /// <param name="serviceSubType">Name of ther service subtytype</param>
        /// <param name="callerContext">Indicates who called the API</param>
        /// <returns>Data processor config collection</returns>
        public DataProcessorConfigCollection GetProcessorConfigurationCollection(String serverName, MDMServiceType service, MDMServiceSubType serviceSubType, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("MonitoringManager.Business.MonitorBL.GetProcessorConfigurationCollection", false);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Monitor Server processor config collection Get starting..");
            }
            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "MonitoringManager.MonitorBL", String.Empty, "GetProcessorConfigurationCollection");
            }

            if (serverName == null)
            {
                throw new MDMOperationException("112170", "Server Name cannot be null.", "MonitoringManager.MonitorBL", String.Empty, "GetProcessorConfigurationCollection");
            }

            if (service == MDMServiceType.UnKnown)
            {
                throw new MDMOperationException("112179", "Service cannot be null.", "MonitoringManager.MonitorBL", String.Empty, "GetProcessorConfigurationCollection");
            }

            ServiceStatusCollection serviceStatusCollection = GetServiceStatus(callerContext);

            var filteredServiceStatus = from serviceStatus in serviceStatusCollection
                                        where serviceStatus.Server == serverName && serviceStatus.Service == service && serviceStatus.ServiceSubType == serviceSubType
                                        select serviceStatus;

            if (filteredServiceStatus.Any())
            {
                ServiceStatus serviceStatus = filteredServiceStatus.FirstOrDefault();

                String processorConfigXMl = serviceStatus.ServiceConfigXML;

                DataProcessorConfigCollection dataProcessorConfigCollection = new DataProcessorConfigCollection(processorConfigXMl);

                return dataProcessorConfigCollection;
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Monitor GetProcessorConfigurationCollection Get completed successfully.");
                MDMTraceHelper.StopTraceActivity("MonitoringManager.Business.MonitorBL.GetProcessorConfigurationCollection");
            }
            return null;
        }

        /// <summary>
        /// Gets the configuration for a particular processor
        /// </summary>
        /// <param name="dataProcessor">Name of the processor</param>
        /// <param name="serverName">Name of the server</param>
        /// <param name="service">Name of the service</param>
        /// <param name="serviceSubType">Name of ther service subtytype</param>
        /// <param name="callerContext">Indicates who called the API</param>
        /// <returns>Data processor config object</returns>
        public DataProcessorConfig GetProcessorConfiguration(CoreDataProcessorList dataProcessor, String serverName, MDMServiceType service, MDMServiceSubType serviceSubType, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("MonitoringManager.Business.MonitorBL.GetProcessorConfiguration", false);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Monitor Server processor config Get starting..");
            }
            DataProcessorConfigCollection dataProcessorConfigCollection = GetProcessorConfigurationCollection(serverName, service, serviceSubType, callerContext);

            DataProcessorConfig dataProcessorConfig = null;
            if (dataProcessorConfigCollection != null)
                dataProcessorConfig = dataProcessorConfigCollection.GetDataProcessorConfig(dataProcessor.ToString());

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Monitor GetProcessorConfiguration completed successfully.");
                MDMTraceHelper.StopTraceActivity("MonitoringManager.Business.MonitorBL.GetProcessorConfiguration");
            }
            return dataProcessorConfig;
        }

        #endregion Methods

        #region Private Methods

        /// <summary>
        /// Updates the data processor config collection with all new values
        /// </summary>
        /// <param name="dataProcessorConfigCollection">data processor config collection which needs to be updated with new values</param>
        /// <param name="configXml">New values which has to be inserted in xml format.</param>
        /// <returns>String value which has updated data processor config collection values in xml format.</returns>
        private String UpdateProcessorConfigXml(DataProcessorConfigCollection dataProcessorConfigCollection, String configXml)
        {
            if (dataProcessorConfigCollection == null && String.IsNullOrEmpty(configXml))
                return String.Empty;

            DataProcessorConfig dataProcessorConfig = new DataProcessorConfig(configXml);
            if (dataProcessorConfigCollection == null)
                dataProcessorConfigCollection = new DataProcessorConfigCollection();
            return dataProcessorConfigCollection.UpdateProcessorConfigXml(dataProcessorConfig);
        }

        #endregion

    }

}
