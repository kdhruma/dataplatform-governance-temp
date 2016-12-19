using System;
using System.Diagnostics;

namespace MDM.MonitoringManager.Business
{
    using MDM.BufferManager;
    using MDM.BusinessObjects;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.MonitoringManager.Data.SqlClient;
    using MDM.Utility;
    using System.Collections.Generic;
    using System.Xml.Linq;
    using System.Linq;

    /// <summary>
    /// Business logic for server info
    /// </summary>
    public class ServerInfoBL : BusinessLogicBase
    {
        #region Fields

        private ServerInfoBufferManager _serverInfoBufferManager = new ServerInfoBufferManager();

        /// <summary>
        /// Indicates instance of server Info DA
        /// </summary>
        private ServerInfoDA _serverInfoDA = new ServerInfoDA();

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets server id based on server name.
        /// </summary>
        /// <param name="serverName"></param>
        /// <returns></returns>
        public Int32 GetServerId(String serverName)
        {
            ServerInfo serverInfo = GetServerInfo(serverName);

            if (serverInfo != null)
            {
                return serverInfo.Id;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Gets server info based on server name
        /// </summary>
        /// <param name="serverName"></param>
        /// <returns>ServerInfo object with one server's Information</returns>
        public ServerInfo GetServerInfo(String serverName)
        {
            ServerInfo serverInfo = new ServerInfo();

            if (String.IsNullOrWhiteSpace(serverName))
            {
                throw new Exception("Server name can not be null.");
            }

            ServerInfoCollection serverInfoCollection = GetServerInfoCollection(serverName);
            
            if (serverInfoCollection != null)
            {
                serverInfo = serverInfoCollection.GetServerInfo(serverName);
            }

            return serverInfo;
        }

        /// <summary>
        /// Gets all servers' information
        /// </summary>
        /// <returns>ServerInfoCollection object with all servers' information</returns>
        public String GetAllServerInfo()
        {
            var ss = GetServerInfoCollection(String.Empty);

            return ss.ToXml();
        }
        
        #endregion

        #region Private Methods

        private ServerInfoCollection GetServerInfoCollection(String serverName)
        {
            ServerInfoCollection serverInfos = new ServerInfoCollection();

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("IntegrationManager.ServerInfoBL.Get", MDMTraceSource.Integration, false);
            }

            try
            {
                CallerContext callerContext = new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Integration);
                
                // Get command
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Fetching info for server - " + serverName, MDMTraceSource.Entity);
                }

                #region Get Server info from cache

                //Get server info from cache
                serverInfos = _serverInfoBufferManager.FindServerInfo();

                #endregion

                #region As no info available in cache - Get Server info from DB

                if (serverInfos == null)
                {
                    //Get Server info from DB as no data for servers info is available in cache
                    serverInfos = _serverInfoDA.GetServerInfoColletion(String.Empty, command);

                    //If data returned from DB is not null then put it into cache
                    if (serverInfos != null && serverInfos.Count > 0)
                    {
                        ServerInfo serverInfo = serverInfos.GetServerInfo(serverName);

                        if (serverInfo == null)
                        {
                            //If server for which info is requested is not available in DB then send that server name for insertion.
                            ServerInfoCollection updatedServerInfoCollectionFromDB = _serverInfoDA.GetServerInfoColletion(serverName, command);

                            if (updatedServerInfoCollectionFromDB != null && updatedServerInfoCollectionFromDB.Count > 0)
                            {
                                ServerInfo updatedServerInfoFromDB = updatedServerInfoCollectionFromDB.GetServerInfo(serverName);
                                if (updatedServerInfoFromDB != null)
                                {
                                    serverInfos.Add(updatedServerInfoFromDB);
                                }
                            }
                        }
                        _serverInfoBufferManager.UpdateServerInfo(serverInfos, 3);
                    }
                }
                else
                {
                    ServerInfo serverInfo = serverInfos.GetServerInfo(serverName);

                    if (serverInfo == null)
                    {
                        // Get Server info from DB as info for this server is not available in cache
                        // Get command
                        ServerInfoCollection updatedServerInfoCollectionFromDB = _serverInfoDA.GetServerInfoColletion(serverName, command);

                        if (updatedServerInfoCollectionFromDB != null && updatedServerInfoCollectionFromDB.Count > 0)
                        {
                            ServerInfo updatedServerInfoFromDB = updatedServerInfoCollectionFromDB.GetServerInfo(serverName);
                            if (updatedServerInfoFromDB != null)
                            {
                                serverInfos.Add(updatedServerInfoFromDB);
                            }
                        }
                        _serverInfoBufferManager.UpdateServerInfo(serverInfos, 3);
                    }
                }

                String serverXml = MDM.Utility.AppConfigurationHelper.GetAppConfig<String>("MDMCenter.Diagnostics.ServerConfiguration");

                if (!String.IsNullOrWhiteSpace(serverXml))
                {
                    XElement xDoc = XElement.Parse(serverXml);

                    var wcfservers = xDoc.Element("WCFServers").Elements();

                    for (int i = 0; i < wcfservers.Count(); i++)
                    {
                        var wcfsrv = wcfservers.ElementAt(i);

                        var srvInfo = serverInfos.GetServerInfo(wcfsrv.Attribute("ServerName").Value);

                        if (srvInfo != null)
                        {
                            srvInfo.VirtualDirectory = wcfsrv.Attribute("VirtualDirectory").Value;
                            srvInfo.ServerType = MDMCenterSystem.WcfService;
                        }
                    }
                    _serverInfoBufferManager.UpdateServerInfo(serverInfos, 3);
                }

                #endregion

            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("IntegrationManager.ServerInfoBL.Get", MDMTraceSource.Entity);
                }
            }

            return serverInfos;
        }

        #endregion
    }
}
