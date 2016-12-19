using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Xml.Linq;

namespace MDM.BufferManager
{
    using MDM.Services;
    using MDM.ExceptionManager;
    using MDM.Utility;

    /// <summary>
    /// Specifies helper manager.
    /// </summary>
    public sealed class BufferHelper
    {
        /// <summary>
        /// ClearCacheAcrossWCF method is used to clear local cache in all the WCFServers
        /// </summary>
        /// <param name="cacheKey">cacheKey to be invalidated. If empty string is provided it will invalidate the entire local cache</param>
        public void ClearCacheAcrossWCF(String cacheKey)
        {
            ClearCacheAcrossWCF(new Collection<String>() { cacheKey });
        }

        /// <summary>
        /// ClearCacheAcrossWCF method is used to clear local cache in all the WCFServers
        /// </summary>
        /// <param name="cacheKeys">cacheKey to be invalidated. If empty string is provided or collection cache keys is null it will invalidate the entire local cache</param>
        public void ClearCacheAcrossWCF(Collection<String> cacheKeys)
        {
            try
            {
                CoreService coreService = new CoreService();
                coreService.ClearCacheAcrossWCF(cacheKeys);
            }
            catch(Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Error occurred : {0}",ex.Message));
                ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
            }
        }

        /// <summary>
        /// Gets list of Web Server names setup for the current instance
        /// </summary>
        /// <returns>List of Web Server names</returns>
        public List<String> GetWebServerList()
        {
            List<String> servers = new List<String>();

            String serverXml = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.Diagnostics.ServerConfiguration", String.Empty);

            if (!String.IsNullOrWhiteSpace(serverXml))
            {
                XDocument xDoc = XDocument.Parse(serverXml);

                servers = (from f in xDoc.Descendants("WebServers")
                           from f1 in f.Elements("WebServer")
                           select f1.Attribute("ServerName").Value).Distinct<String>().ToList<String>();
            }

            return servers;
        }
    }
}
