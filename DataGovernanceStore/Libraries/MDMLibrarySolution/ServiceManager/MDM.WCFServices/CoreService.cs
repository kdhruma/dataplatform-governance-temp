using System;
using System.ServiceModel;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Collections.Generic;

namespace MDM.WCFServices
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.WCFServiceInterfaces;
    using MDM.CacheManager.Business;
    using MDM.KnowledgeManager.Business;
    using MDM.BusinessObjects;
    using MDM.BufferManager;
    using MDM.LookupManager.Business;
    using MDM.AdminManager.Business;
    using MDM.BusinessObjects.Caching;
    using MDM.MonitoringManager.Business;
    using MDM.ExceptionManager.Handlers;
    using MDM.BusinessObjects.Diagnostics;

    [DiagnosticActivity]
    [ServiceBehavior(Namespace = "http://wcfservices.riversand.com", InstanceContextMode = InstanceContextMode.PerCall)]
    public class CoreService : MDMWCFBase, ICoreService
    {
        private EventLogHandler eventLogHandler = new EventLogHandler();

        #region Constructors

        public CoreService()
            : base(true)
        {

        }

        public CoreService(Boolean loadSecurityPrincipal)
            : base(loadSecurityPrincipal)
        {

        }

        #endregion

        #region Cache Methods

        /// <summary>
        /// Clears everyting from Cache
        /// </summary>
        public void ClearCache()
        {
            try
            {
                ICache cache = null;
                cache = CacheFactory.GetCache();
                cache.ClearCache();
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

        }

        /// <summary>
        /// Gets a list of all Cache Keys
        /// </summary>
        /// <returns>
        /// Collection of Cache Keys
        /// </returns>
        public Collection<String> GetAllCacheKeys()
        {
            try
            {
                ICache cache = null;
                cache = CacheFactory.GetCache();
                Collection<String> cacheKeyNames = cache.GetAllCacheKeys();
                return cacheKeyNames;
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
        }

        /// <summary>
        /// Removes a Key from Cache
        /// </summary>
        /// <param name="cacheKeys">collects collection of key and remove one by one from internal cache.</param>
        public void RemoveCacheKey(Collection<String> cacheKeys)
        {
            try
            {
                //Initialize trace source and start trace activity
                MDMTraceHelper.InitializeTraceSource();
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("CoreService.RemoveCacheKey", MDMTraceSource.General, false);

                ICache cache = CacheFactory.GetCache();

                if (cacheKeys != null && cacheKeys.Count > 0)
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0} key(s) found to be removed from internal cache.", cacheKeys.Count), MDMTraceSource.General);

                    foreach (String key in cacheKeys)
                    {
                        if (key.ToLower() == "appconfig_all")
                        {
                            //Reload all Appconfigs which are stored in in memory.
                            AppConfigurationHelper.LoadAllAppConfigs(true);
                        }
                        else if (key.ToLower() == "appconfig_traceconfiguration")
                        {
                            AppConfigurationHelper.ReloadAppConfig("MDMCenter.TraceConfiguration");
                        }
                        else
                        {
                            if (Constants.TRACING_ENABLED)
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, String.Format("Trying to remove {0} key from internal cache...", key), MDMTraceSource.General);

                            cache.Remove(key);

                            if (Constants.TRACING_ENABLED)
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, String.Format("Removed {0} key from internal cache.", key), MDMTraceSource.General);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            finally
            {
                //Stop trace activity
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("CoreService.RemoveCacheKey", MDMTraceSource.General);
            }
        }

        /// <summary>
        /// Clears distributed cache
        /// </summary>
        public Boolean ClearDistributedCache()
        {
            Boolean successFlag = false;
            try
            {

                MDM.CacheManager.Business.IDistributedCache distributedCache = CacheFactory.GetDistributedCache();
                distributedCache.ClearAll();
                successFlag = true;
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            return successFlag;
        }

        /// <summary>
        /// This method invalidates the lookup cache in all the available locales
        /// </summary>
        /// <param name="lookupName">name of the lookup</param>
        /// <returns>returns true is succeeds</returns>
        public Boolean InvalidateLookupCache(String lookupName)
        {
            Boolean result = false;

            try
            {
                LocaleBL localeManager = new LocaleBL();
                LocaleCollection allowableDataLocales = localeManager.GetAvailableLocales();
                LookupBL lookupBL = new LookupBL();

                foreach (Locale locale in allowableDataLocales)
                {
                    lookupBL.InvalidateLookupData(lookupName, locale.Locale);
                }

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            return result;
        }

        #region Build
        /// <summary>
        /// Gets the latest build details
        /// </summary>
        /// <returns>BuildDetail object</returns>
        public BuildDetail GetLatestBuildDetail()
        {

            try
            {
                BuildInfoBL buildInfoBl = new BuildInfoBL();
                return buildInfoBl.GetLatestBuildDetail();
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
        }


        /// <summary>
        /// Gets the corresponding build feature Id
        /// </summary>
        /// <param name="buildDetailId">Indicates the  buildDetailId</param>
        /// <param name="featureName">Indicates the featureName</param>
        /// <returns>Int</returns>
        public Int32 GetBuildFeatureId(Int32 buildDetailId, String featureName)
        {
            Int32 buildFeatureId = 0;
            try
            {
                BuildInfoBL buildInfoBl = new BuildInfoBL();
                buildFeatureId = buildInfoBl.GetBuildFeatureId(buildDetailId, featureName);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
            return buildFeatureId;
        }

        /// <summary>
        /// Process FileCheckSum
        /// </summary>
        /// <param name="buildDetailContext">Indicates the BuildContext</param>
        /// <returns>OperationResult</returns>

        public OperationResult ProcessFileCheckSum(BuildDetailContext buildDetailContext)
        {

            try
            {
                BuildInfoBL buildInfoBl = new BuildInfoBL();
                return buildInfoBl.ProcessFileCheckSum(buildDetailContext);

            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

        }
        /// <summary>
        /// Update Build Status 
        /// </summary>
        /// <param name="buildDetailContext">Indicates the BuildContext</param>

        /// <returns>OperationResult</returns>


        public OperationResult UpdateBuildStatus(BuildDetailContext buildDetailContext)
        {
            try
            {
                BuildInfoBL buildInfoBl = new BuildInfoBL();
                return buildInfoBl.UpdateBuildStatus(buildDetailContext);

            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }
        }
        /// <summary>
        /// Save Build Details 
        /// </summary>
        /// <param name="version">Indicates the BuildContext</param>
        /// <returns></returns>

        public OperationResult SaveBuildDetails(BuildDetailContext buildDetailContext)
        {

            try
            {
                BuildInfoBL buildInfoBl = new BuildInfoBL();
                return buildInfoBl.SaveBuildDetails(buildDetailContext);

            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="relatedKeyEnum"></param>
        /// <returns></returns>
        public OperationResult RemoveEntityFromCache(long entityId, EntityCacheKeyEnum relatedKeyEnum)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityShortName"></param>
        /// <param name="relatedKeyEnum"></param>
        /// <returns></returns>
        public OperationResult RemoveEntityFromCache(string entityShortName, EntityCacheKeyEnum relatedKeyEnum)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityShortName"></param>
        /// <param name="relatedKeyEnum"></param>
        /// <returns></returns>
        public OperationResult GetEntityFromCache(String entityShortName, EntityCacheKeyEnum relatedKeyEnum)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="relatedKeyEnum"></param>
        /// <returns></returns>
        public OperationResult GetEntityFromCache(Int64 entityId, EntityCacheKeyEnum relatedKeyEnum)
        {
            var ebm = new EntityBufferManager();

            int eid = 0;

            var ect = new EntityContext();

            if (relatedKeyEnum.equals(EntityCacheKeyEnum.EntityLocalAttributesCacheKey))
                ect.LoadAttributes = true;

            if (relatedKeyEnum.equals(EntityCacheKeyEnum.EntityInheritedAttributesCacheKey))
                ect.LoadOnlyInheritedValues = true;

            var eo = ebm.FindBaseEntity(eid, ect);

            var or = new OperationResult();

            if (eo != null)
            {
                var el = ebm.FindLocalAttributes(eo);
                eo.SetAttributes(el);

                var eh = ebm.FindInheritedAttributes(eo);
                eo.SetAttributes(eh);

                var xml = eo.ToXml();

                or.OperationResultStatus = OperationResultStatusEnum.Successful;
                or.ReturnValues.Add(xml);
            }
            else
            {
                var xml = "<Error> Entity not available in Cache.</Error>";
                or.OperationResultStatus = OperationResultStatusEnum.Failed;
                or.Errors.Add(new Error("1001", xml));
            }

            return or;
        }

        #endregion

        #region ICoreService Members

        /// <summary>
        /// Removes an object from cache 
        /// </summary>
        /// <param name="objectId">Indicates the id of the object to be removed from cache</param>
        /// <param name="config">Indicates the cache configuration</param>
        /// <returns></returns>

        //public OperationResult RemoveObjectFromCache(int objectId, CacheConfiguration config)
        //{
        //    return RemoveObjectFromCache(objectId.ToString(), config);
        //}
        public OperationResult RemoveObjectFromCache(String key, CacheConfiguration config)
        {
            return RemoveObjectFromCacheByKey(key, config);
        }


        /// <summary>
        /// Gets an object from cache
        /// </summary>
        /// <param name="objectId">Indicates the id of the object to be removed from cache</param>
        /// <param name="config">Indicates the cache configuration</param>
        /// <returns></returns>
        //public OperationResult GetObjectFromCache(int objectId, CacheConfiguration config)
        //{
        //    return GetObjectFromCache(objectId.ToString(), config);
        //}
        public OperationResult GetObjectFromCache(String key, CacheConfiguration config)
        {
            return GetObjectFromCacheByKey(key, config);
        }

        public CacheConfigurationCollection GetCacheConfigs()
        {
            var acb = new AppConfigProviderUsingDB();

            var val = acb.GetCacheConfigs();

            var col = new CacheConfigurationCollection();
            col.LoadFromXml(val);

            return col;
        }


        public String GetCacheConfiguration()
        {
            var acb = new AppConfigProviderUsingDB();
            return acb.GetCacheConfigs();
        }

        /// <summary>
        /// This method gets ServerInfoCollection in XML format. 
        /// 
        /// </summary>
        /// <returns>Returns the colletion in XML format</returns>
        public String GetAllServers()
        {
            ServerInfoBL bl = new ServerInfoBL();

            return bl.GetAllServerInfo();
        }

        #endregion

        private OperationResult RemoveObjectFromCacheByKey(String key, CacheConfiguration config)
        {
            DiagnosticActivity curActivity = new DiagnosticActivity();
            curActivity.ActivityName = "Removed From Cache";
            curActivity.OperationId = Constants.SystemTracingOperationId;
            curActivity.Start();

            var or = new OperationResult();
            or.OperationResultStatus = OperationResultStatusEnum.Failed;
            //String key = String.Empty;

            //    key = String.Format(config.KeyFormat, value);
            var principal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            var message = "Removed Cache Object with Key : " + key + Environment.NewLine +
                            "Server Name : " + Environment.MachineName + Environment.NewLine +
                           "User Name : " + principal.CurrentUserName + Environment.NewLine +
                           "User Id : " + principal.CurrentUserId + Environment.NewLine +
                            "Cache Config Name : " + config.Name + Environment.NewLine +
                            "Cache Config Type : " + config.CacheType.ToString();

            if (config.CacheType.Equals("Distributed", StringComparison.OrdinalIgnoreCase))
            {
                var cache = CacheFactory.GetDistributedCache();

                if (cache.Remove(key))
                {
                    or.OperationResultStatus = OperationResultStatusEnum.Successful;

                    eventLogHandler.WriteWarningLog(message, 9001);
                    curActivity.LogWarning(message);
                }
                else
                    or.Errors.Add(new Error("1001", "Removing from cache failed in distributed cache."));

            }
            else
            {
                var cache = CacheFactory.GetCache();

                if (cache.Remove(key))
                {
                    var cacheSync = new CacheSynchronizationHelper();
                    DateTime expiryTime = DateTime.Now.AddHours(1);

                    cacheSync.NotifyLocalCacheInsert(key, expiryTime, false);

                    or.OperationResultStatus = OperationResultStatusEnum.Successful;

                    eventLogHandler.WriteWarningLog(message, 9001);
                    curActivity.LogWarning(message);
                }
                else
                    or.Errors.Add(new Error("1001", "Removing from cache failed in framework cache"));
            }


            curActivity.Stop();

            return or;
        }

        private OperationResult GetObjectFromCacheByKey(String key, CacheConfiguration config)
        {
            var or = new OperationResult();
            or.OperationResultStatus = OperationResultStatusEnum.Failed;
            or.ReferenceId = key;
            //String key = String.Empty;


            //    key = String.Format(config.KeyFormat, value);
            object returnObject = null;

            if (config.CacheType.Equals("Distributed", StringComparison.OrdinalIgnoreCase))
            {
                var cache = CacheFactory.GetDistributedCache();

                returnObject = cache.Get(key);

                if (returnObject == null)
                    or.Errors.Add(new Error("1001", "Object not in  distributed cache."));
            }
            else
            {
                var cache = CacheFactory.GetCache();

                returnObject = cache.Get(key);

                if (returnObject == null) 
                    or.Errors.Add(new Error("1001", "Object not in WCF framework cache"));
            }

            if (returnObject != null)
            {
                var mns = returnObject.GetType().GetMethods();

                var methodInfos = new List<MethodInfo>(mns);

                var xmlMethod = methodInfos.Find(x => x.Name.Equals("ToXml", StringComparison.OrdinalIgnoreCase));

                if (xmlMethod != null)
                {
                    var res = xmlMethod.Invoke(returnObject, null);

                    if (res != null)
                    {
                        or.ReturnValues.Add(res.ToString());
                        or.OperationResultStatus = OperationResultStatusEnum.Successful;
                    }
                    else
                    {
                        or.Errors.Add(new Error("1001", "Found the object in cache, ToXml() returned null"));
                    }
                }
                //else if (returnObject.GetType().IsGenericType)
                //{
                //    var properties = returnObject.GetType().GetProperties();

                //    Int32 count = 0;
                //    PropertyInfo itemProp = null;
                //    foreach( var prop in properties)
                //    {
                //        if ( prop.Name == "Count")                //method.Name.Equals("GetEnumerator", StringComparison.OrdinalIgnoreCase))
                //        {
                //            count = (Int32)prop.GetValue(returnObject);
                //        }

                //        if (prop.Name == "Item")
                //        {
                //            itemProp = prop;
                //        }
                //    }

                //    var sb = new StringBuilder();
                //    sb.AppendLine("Found generic Collection");
                //    sb.AppendLine("<Collection>");

                //    ArrayList indx = new ArrayList();

                //    for (int i = 0; i < count; i++)
                //    {
                //        indx.Add(i);

                //        var obj = itemProp.GetValue(returnObject, indx.ToArray() );

                //        var methodInfo1 = obj.GetType().GetMethod("ToXml", BindingFlags.IgnoreCase);

                //            if (methodInfo1 != null)
                //            {
                //                var result = methodInfo1.Invoke(obj, null);

                //                sb.AppendLine(result.ToString());
                //            }

                //            indx.Clear();
                //    }


                //    sb.AppendLine("</Collection>");

                //    or.ReturnValues.Add(sb.ToString());
                //    or.OperationResultStatus = OperationResultStatusEnum.Successful;
                //}
                else
                {
                    var stringMethod = methodInfos.Find(x => x.Name.Equals("ToString", StringComparison.OrdinalIgnoreCase));

                    if (stringMethod != null)
                    {
                        var res = stringMethod.Invoke(returnObject, null);

                        if (res != null)
                        {
                            or.ReturnValues.Add("Not able to find ToXml method. Got the data from ToString method.");
                            or.ReturnValues.Add(res.ToString());
                            or.OperationResultStatus = OperationResultStatusEnum.Successful;
                        }
                        else
                        {
                            or.Errors.Add(new Error("1001", "Found the object in cache, ToString() returned null"));
                        }
                    }
                    else
                    {
                        or.Errors.Add(new Error("1001", "Found the object in cache, No ToXml() or ToString() methods to return details"));
                    }
                }
            }

            return or;

        }
    }
}