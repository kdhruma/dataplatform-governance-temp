using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Principal;
using System.ServiceModel;

namespace MDM.Services
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Caching;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Interfaces;
    using MDM.Services.ServiceProxies;
    using MDM.Utility;
    using MDM.WCFServiceInterfaces;

    /// <summary>
    /// Core Service facilitates to perform operations in MDMCenter including caching.
    /// </summary>
    public class CoreService : ServiceClientBase
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors

        /// <summary>
        /// Use this default constructor only when calling MDM service within the execution context of an MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        public CoreService()
            : base(typeof(CoreService))
        {
        }

        /// <summary>
        /// Use this default constructor with binding configuration name only when calling MDM service within the execution context of an MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        public CoreService(String endPointConfigurationName)
            : base(typeof(CoreService), endPointConfigurationName)
        {
        }

        /// <summary>
        /// Use this constructor for form based authentication passing username and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public CoreService(String endPointConfigurationName, String userName, String password)
            : base(endPointConfigurationName, userName, password)
        {

        }

        /// <summary>
        /// Use this constructor for window authentication passing current windows identity.
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="userIdentity">Indicates User's Identity</param>
        public CoreService(String endPointConfigurationName, IIdentity userIdentity)
            : base(endPointConfigurationName, userIdentity)
        {
        }

        /// <summary>
        /// Constructor with client configuration.
        /// </summary>
        /// <param name="wcfClientConfiguration">Indicates WCF Client Configuration</param>
        public CoreService(IWCFClientConfiguration wcfClientConfiguration)
            : base(wcfClientConfiguration)
        {
        }

        /// <summary>
        /// Constructor with endPointConfigurationName, authenticationType, userIdentity, userName and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="authenticationType">Indicates Type of Authentication i.e Windows/Forms</param>
        /// <param name="userIdentity">Indicates Identity of the Login User</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public CoreService(String endPointConfigurationName, AuthenticationType authenticationType, IIdentity userIdentity, String userName, String password)
            : base(endPointConfigurationName, authenticationType, userIdentity, userName, password)
        {
        }

        /// <summary>
        /// Constructor with endPointConfigurationName, endPointAddress, authenticationType, userIdentity, userName and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="endPointAddress"> Provides a unique network address that a client uses to communicate with a service endpoint.</param>
        /// <param name="authenticationType">Indicates Type of Authentication i.e Windows/Forms</param>
        /// <param name="userIdentity">Indicates Identity of the Login User</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public CoreService(String endPointConfigurationName, EndpointAddress endPointAddress, AuthenticationType authenticationType, IIdentity userIdentity, String userName, String password)
            : base(endPointConfigurationName, endPointAddress, authenticationType, userIdentity, userName, password)
        {
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets names of all CacheKeys
        /// </summary>
        /// <returns>A collection of cachekey names</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public Collection<String> GetAllCacheKeys()
        {
            return MakeServiceCall("GetAllCacheKeys", "GetAllCacheKeys", service => service.GetAllCacheKeys());
        }

        /// <summary>
        /// Removes a cache key
        /// </summary>
        /// <param name="cacheKeyName">Indicates name of cache key</param>
        /// <returns></returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public void RemoveCacheKey(String cacheKeyName)
        {
            RemoveCacheKey(new Collection<String>() { cacheKeyName });
        }

        /// <summary>
        /// Removes a collection of cache key from internal cache.
        /// </summary>
        /// <param name="cacheKeys">collection of cache key to be removed from internal cache.</param>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public void RemoveCacheKey(Collection<String> cacheKeys)
        {
            MakeServiceCall<Object>("RemoveCacheKey",
                                    "RemoveCacheKey",
                                    service =>
                                        {
                                            if (Constants.TRACING_ENABLED)
                                            {
                                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0} key(s) found to be removed from internal cache.", cacheKeys.Count), MDMTraceSource.General);
                                            }
                                            service.RemoveCacheKey(cacheKeys);
                                            return null;
                                        });
        }

        /// <summary>
        /// Clears cache
        /// </summary>
        /// <returns></returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public void ClearCache()
        {
            MakeServiceCall<Object>("ClearCache",
                                    "ClearCache",
                                    service =>
                                        {
                                            service.ClearCache();
                                            return null;
                                        });
        }

        /// <summary>
        /// Clears distributed cache
        /// </summary>
        /// <returns>True if distributed Cache cleared successfuly</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public Boolean ClearDistributedCache()
        {
            return MakeServiceCall("ClearDistributedCache",
                                   "ClearDistributedCache",
                                   service => service.ClearDistributedCache());
        }

        /// <summary>
        /// WCFClearCacheAll method is used to clear local cache in all the WCFServers
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
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("CoreServiceClient.ClearCacheAcrossWCF", MDMTraceSource.CacheManager, false);

            try
            {
                ICoreService coreServiceClient = null;
                List<String> serviceUrls = null; //GetServers();

                var servers = GetAllServers();

                foreach (ServerInfo mac in servers)
                {
                    //GetClient(mac.
                }

                if (serviceUrls != null && serviceUrls.Count > 0)
                {
                    foreach (String server in serviceUrls)
                    {
                        try
                        {
                            coreServiceClient = GetClient(server);

                            if (cacheKeys == null || cacheKeys.Count < 1)
                            {
                                coreServiceClient.ClearCache();
                            }
                            else
                            {
                                coreServiceClient.RemoveCacheKey(cacheKeys);
                            }
                        }
                        catch (Exception ex)
                        {
                            base.HandleException(ex);
                        }
                        finally
                        {
                            //Close the client
                            this.DisposeClient(coreServiceClient);
                        }
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("CoreServiceClient.ClearCacheAcrossWCF", MDMTraceSource.CacheManager);
            }
        }

        /// <summary>
        /// Clears the lookup cache in WCF
        /// </summary>
        /// <param name="lookupName">Name of the LookupTable</param>
        /// <returns>Indicates whether the cache has been cleared successfully or not</returns>
        /// <example>
        /// <code language="c#" title="Invalidate Lookup Cache" source="..\MDM.APISamples\Administration\CoreService\MaintainCache.cs" region="Invalidate Lookup Cache" />
        /// </example>
        public Boolean InvalidateLookupCache(String lookupName)
        {
            return MakeServiceCall("InvalidateLookupCache",
                                   "InvalidateLookupCache",
                                   service => service.InvalidateLookupCache(lookupName));
        }

        /// <summary>
        /// Gets the latest build details
        /// </summary>
        /// <returns>BuildDetail object</returns>
        public IBuildDetail GetLatestBuildDetail()
        {
            return MakeServiceCall("GetLatestBuildDetail", "GetLatestBuildDetail", client => client.GetLatestBuildDetail());
        }

        /// <summary>
        /// Gets the corresponding build feature Id
        /// </summary>
        /// <param name="buildDetailId">Indicates the  buildDetailId</param>
        /// <param name="featureName">Indicates the featureName</param>
        /// <returns>Int</returns>
        public Int32 GetBuildFeatureId(Int32 buildDetailId, String featureName)
        {
            return MakeServiceCall("GetBuildFeatureId", "GetBuildFeatureId", client => client.GetBuildFeatureId(buildDetailId, featureName));
        }

        /// <summary>
        /// Process FileCheckSum
        /// </summary>
        /// <param name="iBuildDetailContext">Indicates the BuildContext</param>
        /// <returns>Operation result of the file check sum process</returns>

        public IOperationResult ProcessFileCheckSum(IBuildDetailContext iBuildDetailContext)
        {
            return MakeServiceCall("ProcessFileCheckSum", "ProcessFileCheckSum", client => client.ProcessFileCheckSum(iBuildDetailContext as BuildDetailContext));
        }

        /// <summary>
        /// Update Build Status 
        /// </summary>
        /// <param name="iBuildDetailContext">Indicates the BuildContext</param>
        /// <returns>Operation result of updating the build status</returns>
        public IOperationResult UpdateBuildStatus(IBuildDetailContext iBuildDetailContext)
        {
            return MakeServiceCall("UpdateBuildStatus", "UpdateBuildStatus", client => client.UpdateBuildStatus(iBuildDetailContext as BuildDetailContext));
        }
        /// <summary>
        /// Save Build Details 
        /// </summary>
        /// <param name="iBuildDetailContext">Indicates the BuildContext</param>
        /// <returns>Operation result of saving the entire build details</returns>

        public IOperationResult SaveBuildDetails(IBuildDetailContext iBuildDetailContext)
        {
            return MakeServiceCall("SaveBuildDetails", "SaveBuildDetails", client => client.SaveBuildDetails(iBuildDetailContext as BuildDetailContext));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="relatedKeyEnum"></param>
        /// <returns></returns>
        public IOperationResult RemoveEntityFromCache(long entityId, EntityCacheKeyEnum relatedKeyEnum)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityShortName"></param>
        /// <param name="relatedKeyEnum"></param>
        /// <returns></returns>
        public IOperationResult RemoveEntityFromCache(string entityShortName, EntityCacheKeyEnum relatedKeyEnum)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="relatedKeyEnum"></param>
        /// <returns></returns>
        public IOperationResult GetEntityFromCache(long entityId, EntityCacheKeyEnum relatedKeyEnum)
        {
            return MakeServiceCall("GetEntityFromCache", "GetEntityFromCache", client => client.GetEntityFromCache(entityId, relatedKeyEnum));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityShortName"></param>
        /// <param name="relatedKeyEnum"></param>
        /// <returns></returns>
        public IOperationResult GetEntityFromCache(string entityShortName, EntityCacheKeyEnum relatedKeyEnum)
        {
            throw new NotImplementedException();
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="objectId"></param>
        ///// <param name="config"></param>
        ///// <returns></returns>
        //public OperationResult GetObjectFromCache(int objectId, CacheConfiguration config)
        //{
        //    //return MakeServiceCall("GetObjectFromCache", "GetObjectFromCache", client => client.GetObjectFromCache(objectId, config));
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">Indicates the cache key</param>
        /// <param name="config">Indicates the cache configuration object</param>
        /// <param name="serverUrl">Indicates the server url  of the WCF server that client likes to connect</param>
        /// <returns></returns>
        public OperationResult GetObjectFromCache(String key, CacheConfiguration config, String serverUrl)
        {
            return MakeServiceCall("GetObjectFromCache", "GetObjectFromCache", client => client.GetObjectFromCache(key, config), MDMTraceSource.APIFramework, serverUrl);
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="objectId"></param>
        ///// <param name="config"></param>
        ///// <returns></returns>
        //public OperationResult RemoveObjectFromCache(int objectId, CacheConfiguration config)
        //{
        //    throw new NotImplementedException();
        //    //return MakeServiceCall("RemoveObjectFromCache", "RemoveObjectFromCache", client => client.RemoveObjectFromCache(objectId, config));
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public OperationResult RemoveObjectFromCache(String key, CacheConfiguration config)
        {
            return MakeServiceCall("RemoveObjectFromCache", "RemoveObjectFromCache", client => client.RemoveObjectFromCache(key, config));
        }
        
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public CacheConfigurationCollection GetAllCacheConfiguration()
        //{
        //    return MakeServiceCall("GetCacheConfigs", "GetCacheConfigs", client => client.GetCacheConfigs());
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CacheConfigurationCollection GetCacheCofiguration()
        {
            var xmlo = MakeServiceCall("GetCacheCofiguration", "GetCacheCofiguration", client => client.GetCacheConfiguration());

            var cc = new CacheConfigurationCollection();

            if (xmlo != null)
            {
                cc.LoadFromXml(xmlo);
            }

            return cc;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Get core service client
        /// </summary>
        /// <param name="serverName">Indicates name of the server</param>
        /// <returns>Returns service client with user name and password</returns>
        private ICoreService GetClient(String serverName = "")
        {
            ICoreService coreServiceClient = null;

            if (WCFServiceInstanceLoader.IsLocalInstancesEnabled())
            {
                coreServiceClient = WCFServiceInstanceLoader.GetServiceInstance<ICoreService>();
            }

            if (coreServiceClient == null)
            {
                CoreServiceProxy coreServiceProxyClient = null;

                if (String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress == null)
                    coreServiceProxyClient = new CoreServiceProxy();
                else if (!String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress == null)
                    coreServiceProxyClient = new CoreServiceProxy(this.EndPointConfigurationName);
                else if (!String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress != null)
                    coreServiceProxyClient = new CoreServiceProxy(this.EndPointConfigurationName, this.EndpointAddress);

                if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    coreServiceProxyClient.ClientCredentials.UserName.UserName = this.UserName;
                    coreServiceProxyClient.ClientCredentials.UserName.Password = this.Password;
                    coreServiceProxyClient.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;
                }

                if (!String.IsNullOrWhiteSpace(serverName))
                {
                    String uriString = (this.AuthenticationType == AuthenticationType.Forms) ? String.Format("http://{0}/CoreService.svc", serverName) : String.Format("net.tcp://{0}/CoreService.svc", serverName);

                    coreServiceProxyClient.Endpoint.Address = new EndpointAddress(new Uri(uriString), coreServiceProxyClient.Endpoint.Address.Identity, coreServiceProxyClient.Endpoint.Address.Headers);
                }

                coreServiceClient = coreServiceProxyClient;

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Client context for this request: IsDelegationEnabled-{0}; AuthenticationType-{1}; EndPointConfigurationName-{2}; UserName-{3}; UserIdentityName-{4}",
                    this.IsDelegationEnabled, this.AuthenticationType, this.EndPointConfigurationName, this.UserName, ((this.UserIdentity != null) ? this.UserIdentity.Name : string.Empty)));
            }

            return coreServiceClient;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        private void DisposeClient(ICoreService client)
        {
            if (client == null)
                return;

            if (client.GetType().Equals(typeof(CoreServiceProxy)))
            {
                CoreServiceProxy serviceClient = (CoreServiceProxy)client;
                if (serviceClient.State == CommunicationState.Created || serviceClient.State == CommunicationState.Opened || serviceClient.State == CommunicationState.Opening)
                {
                    serviceClient.Close();
                }
                else if (serviceClient.State == CommunicationState.Faulted)
                {
                    serviceClient.Abort();
                }
            }
            else
            {
                //Do nothing...
            }
        }


        /// <summary>
        ///  Get all the WCF servers list.
        /// </summary>
        /// <returns></returns>
        public ServerInfoCollection GetAllServers()
        {
            var xmlString = MakeServiceCall("GetAllServers", "GetAllServers", client => client.GetAllServers());

            var ss = new ServerInfoCollection(xmlString);

            return ss;

            //List<String> servers = new List<String>();

            //String serverXml = this.GetAppConfig("MDMCenter.Diagnostics.ServerConfiguration");

            //if (!String.IsNullOrWhiteSpace(serverXml))
            //{
            //    XDocument xDoc = XDocument.Parse(serverXml);

            //    servers = (from f in xDoc.Descendants("WCFServers")
            //               from f1 in f.Elements("WCFServer")
            //               select f1.Attribute("ServerName").Value + "/" + f1.Attribute("VirtualDirectory").Value).ToList<String>();
            //}

            //return servers;
        }

        /// <summary>
        /// Makes the CoreServiceClient call. Creates the client instance, executes call delegate against it in
        /// impersonated context (if necessary) and then disposes the client.
        /// Also emits traces when necessary.
        /// </summary>
        /// <typeparam name="TResult">Indicates type of the result of service call.</typeparam>
        /// <param name="clientMethodName">Indicates name of the client method to include in traces.</param>
        /// <param name="serverMethodName">Indicates name of the server method to include in traces.</param>
        /// <param name="call">Indicates call delegate to be executed against a client instance.</param>
        /// <param name="traceSource">Indicates name of the trace source</param>
        /// <param name="serverUrl">Indicates WCF Server that the client would like to connect to.</param>
        /// <returns>Returns value returned by service, or default.</returns>
        private TResult MakeServiceCall<TResult>(String clientMethodName, String serverMethodName, Func<ICoreService, TResult> call, MDMTraceSource traceSource = MDMTraceSource.APIFramework, String serverUrl = null)
        {
            //Start trace activity
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("CoreServiceClient." + clientMethodName, traceSource, false);

            TResult result = default(TResult);
            ICoreService coreServiceClient = null;

            try
            {
                if (String.IsNullOrEmpty(serverUrl))
                    serverUrl = String.Empty;

                coreServiceClient = GetClient(serverUrl);

                ValidateContext();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "CoreServiceClient sends '" + serverMethodName + "' request message.", traceSource);

                result = Impersonate(() => call(coreServiceClient));

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "CoreServiceClient receives '" + serverMethodName + "' response message.", traceSource);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                //Close the client
                DisposeClient(coreServiceClient);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("CoreServiceClient." + clientMethodName, traceSource);
            }

            return result;
        }

        #endregion Private Methods

        #endregion
    }
}