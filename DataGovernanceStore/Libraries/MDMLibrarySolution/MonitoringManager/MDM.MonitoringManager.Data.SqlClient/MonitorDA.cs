using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using MDM.Core;

namespace MDM.MonitoringManager.Data.SqlClient
{
    using MDM.BusinessObjects;
    using MDM.Utility;

    public class MonitorDA : SqlClientDataAccessBase
    {
        public bool ProcessServiceStatus(string serverName, MDMServiceType service, MDMServiceSubType serviceSubType, String serviceStatusXml, String serviceConfigXml, DBCommandProperties command)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("MonitoringManager.MonitorDA.Process", false);

            Boolean returnResult = true;
            SqlParametersGenerator generator = new SqlParametersGenerator("MonitorManager_SqlParameters");
            SqlParameter[] parameters = generator.GetParameters("MonitoringManager_Monitor_Process_ParameterArray");

            try
            {
                parameters[0].Value = serverName;
                parameters[1].Value = service.ToString();
                parameters[2].Value = serviceSubType.ToString();
                parameters[3].Value = serviceStatusXml;
                if (String.IsNullOrEmpty(serviceConfigXml))
                    parameters[4].Value = String.Empty;
                else
                    parameters[4].Value = serviceConfigXml;

                String storedProcedureName = "usp_MonitoringManager_Monitor_ServiceStatus_Process";

                ExecuteProcedureNonQuery(command.ConnectionString, parameters, storedProcedureName);
            }
            finally
            {

            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("MonitoringManager.Monitor.ProcessServiceStatus");
            return returnResult;
        }

        public ServiceStatusCollection GetServiceStatus(DBCommandProperties command)
        {
            SqlDataReader reader = null;
            ServiceStatusCollection serviceStatusCollection = new ServiceStatusCollection();

            try
            {
                SqlParameter[] parameters = null;
                String storedProcedureName = String.Empty;

                storedProcedureName = "usp_MonitoringManager_Monitor_ServiceStatus_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        ServiceStatus serviceStatus = new ServiceStatus();

                        if (reader["Server"] != null)
                            serviceStatus.Server = reader["Server"].ToString();

                        if (reader["Service"] != null)
                        {
                            MDMServiceType serviceType = MDMServiceType.UnKnown;
                            Enum.TryParse<MDMServiceType>(reader["Service"].ToString(), out serviceType);
                            serviceStatus.Service = serviceType;
                        }

                        if (reader["ServiceSubType"] != null)
                        {
                            MDMServiceSubType serviceSubType = MDMServiceSubType.UnKnown;
                            Enum.TryParse<MDMServiceSubType>(reader["ServiceSubType"].ToString(), out serviceSubType);
                            serviceStatus.ServiceSubType = serviceSubType;
                        }

                        if (reader["ServiceStatus"] != null)
                            serviceStatus.ServiceStatusXML = reader["ServiceStatus"].ToString();

                        if (reader["ProcessorConfig"] != null)
                            serviceStatus.ServiceConfigXML = reader["ProcessorConfig"].ToString();

                        if (reader["ModDateTime"] != null)
                            serviceStatus.ModifiedDateTime = ValueTypeHelper.ConvertToDateTime(reader["ModDateTime"]);

                        serviceStatusCollection.Add(serviceStatus);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return serviceStatusCollection;
        }
    }
}
