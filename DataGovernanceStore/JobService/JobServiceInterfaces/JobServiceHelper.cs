using System;
using System.Configuration;
using System.Collections.ObjectModel;
using System.Xml;

namespace Riversand.JobService.Interfaces
{
    using MDM.Core;    
    using MDM.Utility;

	/// <summary>
	/// Summary description for JobServiceHelper.
	/// </summary>
	public class JobServiceHelper
	{
        #region Private and Protected Fields
        #endregion Private and Protected Fields

        #region Public Properties
        #endregion Public Properties

        #region Constructors
        public JobServiceHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #endregion Constructors

        #region Public Methods

        public static void Test()
        {
            IJobService service = JobServiceHelper.GetJobService();
            if (service != null)
            {
                service.OnTest();
            }
            else
            {
                throw new Exception("Cannot obtain a reference to the Job Service URI, make sure that the configuration file has the key 'JobServiceURI' configured correctly");
            }
        }

        /// <summary>
        /// Notifies the appropriate service that a change has occurred.
        /// </summary>
        public static void NotifyChange(Type pNotificationType,MDMCenterApplication application, MDMCenterModules module)
        {
            Boolean serverSplitEnabled = JobServiceHelper.IsServerSplitEnabled();

            IJobService service = null;
            if ( serverSplitEnabled == false )
            {
                service = JobServiceHelper.GetJobService();
            }
            else
            {
                service = JobServiceHelper.GetJobService(module);
            }

            if ( service != null )
            {
                service.OnNotifyChange(pNotificationType);
            }
            else
            {
                throw new Exception("Cannot obtain a reference to the Job Service URI, make sure that the configuration file has the key 'JobServiceURI' configured correctly");
            }
        }

        /// <summary>
        /// Notifies the appropriate service that a change has occurred.
        /// </summary>
        public static void NotifyJobRun(Type pNotificationType, Int32 jobId, MDMCenterApplication application, MDMCenterModules module)
        {
            Boolean serverSplitEnabled = JobServiceHelper.IsServerSplitEnabled();
            
            IJobService service = null;
            
            if (serverSplitEnabled == false)
            {
                service = JobServiceHelper.GetJobService();
            }
            else
            {
                service = JobServiceHelper.GetJobService(module);
            }

            if (service != null)
            {
                service.QueueJobForRun(pNotificationType, jobId);
            }
            else
            {
                throw new Exception("Cannot obtain a reference to the Job Service URI, make sure that the configuration file has the key 'JobServiceURI' configured correctly");
            }
        }

        /// <summary>
        /// Sends a message to JobService to notify a change of an AppConfig value.
        /// </summary>
        public static void NotifyConfigUpdate(Type notificationType, String configName)
        {
            //If Physical server split is enabled, then need to update config on all servers.

            Boolean serverSplitEnabled = JobServiceHelper.IsServerSplitEnabled();

            //If split is disabled, then notify config update to URI in default Jobservice URL
            //AppConfig Key name = Jobs.JobServiceURI
            if ( serverSplitEnabled == false )
            {
                IJobService service = JobServiceHelper.GetJobService();
                if ( service != null )
                {
                    service.OnNotifyConfigUpdate(notificationType, configName);
                }
                else
                {
                    throw new Exception("Cannot obtain a reference to the Job Service URI, make sure that the configuration file has the key 'JobServiceURI' configured correctly");
                }
            }
            else
            {
                //Split is enabled, so read the config having URIs for all modules, and notify config update to all JS instances
                Collection<String> allURIs = JobServiceHelper.GetAllJobServiceURIs();
                if ( allURIs != null )
                {
                    foreach ( String uri in allURIs )
                    {
                        IJobService service = ( IJobService ) Activator.GetObject(typeof(IJobService), uri);
                        if ( service != null )
                        {
                            service.OnNotifyConfigUpdate(notificationType, configName);
                        }
                        else
                        {
                            throw new Exception("Cannot obtain a reference to the Job Service URI, make sure that the configuration file has the key 'JobServiceURI' configured correctly");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sends a message to JobService to notify a reset of all config values cached
        /// </summary>
        public static void NotifyConfigReload(Type notificationType)
        {
            //If Physical server split is enabled, then need to update config on all servers.
            Boolean serverSplitEnabled = JobServiceHelper.IsServerSplitEnabled();

            //If split is disabled, then notify config update to URI in default Jobservice URL
            //AppConfig Key name = Jobs.JobServiceURI
            if ( serverSplitEnabled == false )
            {
                IJobService service = JobServiceHelper.GetJobService();
                if ( service != null )
                {
                    service.OnNotifyConfigReload(notificationType);
                }
                else
                {
                    throw new Exception("Cannot obtain a reference to the Job Service URI, make sure that the configuration file has the key 'JobServiceURI' configured correctly");
                }
            }
            else
            {
                //Split is enabled, so read the config having URIs for all modules, and notify config reload to all JS instances
                Collection<String> allURIs = JobServiceHelper.GetAllJobServiceURIs();
                if ( allURIs != null )
                {
                    foreach ( String uri in allURIs )
                    {
                        IJobService service = ( IJobService ) Activator.GetObject(typeof(IJobService), uri);
                        if ( service != null )
                        {
                            service.OnNotifyConfigReload(notificationType);
                        }
                        else
                        {
                            throw new Exception("Cannot obtain a reference to the Job Service URI, make sure that the configuration file has the key 'JobServiceURI' configured correctly");
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Sends a message to JobService to notify cache invalidate
        /// </summary>
        public static void NotifyCacheInvalidate(Type notificationType, String cacheKey, String action)
        {
            //If Physical server split is enabled, then need to update config on all servers.
            Boolean serverSplitEnabled = JobServiceHelper.IsServerSplitEnabled();

            //If split is disabled, then notify config update to URI in default Jobservice URL
            //AppConfig Key name = Jobs.JobServiceURI
            if (serverSplitEnabled == false)
            {
                IJobService service = JobServiceHelper.GetJobService();
                if (service != null)
                {
                    service.OnNotifyCacheInvalidate(notificationType, cacheKey, action);
                }
                else
                {
                    throw new Exception("Cannot obtain a reference to the Job Service URI, make sure that the configuration file has the key 'JobServiceURI' configured correctly");
                }
            }
            else
            {
                //Split is enabled, so read the config having URIs for all modules, and notify config reload to all JS instances
                Collection<String> allURIs = JobServiceHelper.GetAllJobServiceURIs();
                if (allURIs != null)
                {
                    foreach (String uri in allURIs)
                    {
                        IJobService service = (IJobService)Activator.GetObject(typeof(IJobService), uri);
                        if (service != null)
                        {
                            service.OnNotifyCacheInvalidate(notificationType, cacheKey, action);
                        }
                        else
                        {
                            throw new Exception("Cannot obtain a reference to the Job Service URI, make sure that the configuration file has the key 'JobServiceURI' configured correctly");
                        }
                    }
                }
            }
        }

        private static IJobService GetJobService()
        {
            return (IJobService)Activator.GetObject(
                typeof(IJobService),
				AppConfigurationHelper.GetAppConfig<String>("Jobs.JobServiceURI"));
                //ConfigurationSettings.AppSettings.Get("JobServiceURI"));        
        }

        private static IJobService GetJobService(MDMCenterModules module)
        {
            String uri = String.Empty;
            String serversplitURIs = String.Empty;
            serversplitURIs = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.PhysicalServerSplit.Jobs.JobServiceURIs");

            #region Sample Xml

            /*
             * <JobServiceURIs>
                  <JobServiceURI Module ="Import" URI="tcp://RST1061:8089/Riversand.JobService.JobService"/>
                  <JobServiceURI Module ="Export" URI="tcp://RSJOBSERVICE08:8070/Riversand.JobService.JobService"/>
               </JobServiceURIs>
             */
            #endregion Sample Xml

            if ( !String.IsNullOrWhiteSpace(serversplitURIs) )
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(serversplitURIs, XmlNodeType.Element, null);

                    while ( !reader.EOF )
                    {
                        if ( reader.NodeType == XmlNodeType.Element && reader.Name == "JobServiceURI" )
                        {
                            if ( reader.HasAttributes && reader.MoveToAttribute("Module") )
                            {
                                if ( reader.ReadContentAsString().ToLower().Equals(module.ToString().ToLower()) )
                                {
                                    if ( reader.MoveToAttribute("URI") )
                                    {
                                        uri = reader.ReadContentAsString();
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
                finally
                {
                    if ( reader != null )
                    {
                        reader.Close();
                    }
                }
            }

            return ( IJobService ) Activator.GetObject(typeof(IJobService),uri);
        }

        public static JobServiceType GetJobServiceType(String serviceTypeConfigValue)
        {
            JobServiceType jobServiceType = JobServiceType.All;

            if (!String.IsNullOrEmpty(serviceTypeConfigValue))
            {
                switch (serviceTypeConfigValue.ToLower())
                {
                    case "imports":
                        jobServiceType = JobServiceType.ImportServer;
                        break;
                    case "exports":
                        jobServiceType = JobServiceType.ExportServer;
                        break;
                }
            }
            return jobServiceType;
        }

        public static JobServiceType GetJobServiceType(String computerName, String serviceTypeConfigValue)
        {
            JobServiceType jobServiceType = JobServiceType.All;

            if (JobServiceHelper.IsServerSplitEnabled())
            {
                Collection<String> allURIs = new Collection<string>();
                String serversplitURIs = String.Empty;
                serversplitURIs = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.PhysicalServerSplit.Jobs.JobServiceURIs");

                #region Sample Xml

                /*
            * <JobServiceURIs>
                <JobServiceURI Module ="Import" URI="tcp://RST1061:8089/Riversand.JobService.JobService"/>
                <JobServiceURI Module ="Export" URI="tcp://RSJOBSERVICE08:8070/Riversand.JobService.JobService"/>
              </JobServiceURIs>
            */
                #endregion Sample Xml

                if (!String.IsNullOrWhiteSpace(serversplitURIs))
                {
                    XmlTextReader reader = null;
                    try
                    {
                        reader = new XmlTextReader(serversplitURIs, XmlNodeType.Element, null);

                        while (!reader.EOF)
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "JobServiceURI")
                            {
                                if (reader.HasAttributes && reader.MoveToAttribute("Module"))
                                {
                                    String module = reader.ReadContentAsString();

                                    if (reader.MoveToAttribute("URI"))
                                    {
                                        String uri = reader.ReadContentAsString();
                                        if (uri.Contains(computerName))
                                        {
                                            switch (module.ToLower())
                                            {
                                                case    "import":
                                                    jobServiceType = JobServiceType.ImportServer;
                                                    break;
                                                case "export":
                                                    jobServiceType = JobServiceType.ExportServer;
                                                    break;
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                reader.Read();
                            }
                        }
                    }
                    finally
                    {
                        if (reader != null)
                        {
                            reader.Close();
                        }
                    }
                }            
            }
            return jobServiceType;
        }

        public static Boolean IsServerSplitEnabled()
        {
            Boolean serverSplitEnabled = false;
            String serverSplitEnabledStr = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.JobServerSplit.Enabled");
            serverSplitEnabled = ValueTypeHelper.ConvertToBoolean(serverSplitEnabledStr);
            return serverSplitEnabled;
        }

        #endregion Public Methods

        #region Private and Protected Methods

        private static Collection<String> GetAllJobServiceURIs()
        {
            Collection<String> allURIs = new Collection<string>();
            String serversplitURIs = String.Empty;
            serversplitURIs = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.PhysicalServerSplit.Jobs.JobServiceURIs");

            #region Sample Xml

            /*
            * <JobServiceURIs>
                <JobServiceURI Module ="Import" URI="tcp://RST1061:8089/Riversand.JobService.JobService"/>
                <JobServiceURI Module ="Export" URI="tcp://RSJOBSERVICE08:8070/Riversand.JobService.JobService"/>
              </JobServiceURIs>
            */
            #endregion Sample Xml

            if ( !String.IsNullOrWhiteSpace(serversplitURIs) )
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(serversplitURIs, XmlNodeType.Element, null);

                    while ( !reader.EOF )
                    {
                        if ( reader.NodeType == XmlNodeType.Element && reader.Name == "JobServiceURI" )
                        {
                            if ( reader.HasAttributes && reader.MoveToAttribute("Module") )
                            {
                                if ( reader.MoveToAttribute("URI") )
                                {
                                    String uri = reader.ReadContentAsString();
                                    if ( !String.IsNullOrWhiteSpace(uri) )
                                    {
                                        // Send only distinct ones...
                                        if (allURIs.Contains(uri) == false)
                                        {
                                            allURIs.Add(uri);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
                finally
                {
                    if ( reader != null )
                    {
                        reader.Close();
                    }
                }
            }
            return allURIs;
        }

        #endregion Private and Protected Methods

    }
}
