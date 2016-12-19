using System;
using System.ServiceModel;
using System.Collections.ObjectModel;

namespace MDM.WCFServiceInterfaces
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Caching;

    /// <summary>
    /// Defines operation contracts for MDM Data operations
    /// </summary>
    [ServiceContract(Namespace = "http://wcfservices.riversand.com")]
    public interface 
        ICoreService
    {
        #region Cache Contracts

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        void RemoveCacheKey(Collection<String> cacheKeys);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Collection<String> GetAllCacheKeys();

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        void ClearCache();

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean ClearDistributedCache();

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean InvalidateLookupCache(String LookupName);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult RemoveEntityFromCache(Int64 entityId, EntityCacheKeyEnum relatedKeyEnum);

        [OperationContract(Name="RemoveEntityFromCacheByShortName")]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult RemoveEntityFromCache(String entityShortName, EntityCacheKeyEnum relatedKeyEnum);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult GetEntityFromCache(Int64 entityId, EntityCacheKeyEnum relatedKeyEnum);

        [OperationContract(Name="GetEntityFromCacheByShortName")]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult GetEntityFromCache(String entityShortName, EntityCacheKeyEnum relatedKeyEnum);


        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult RemoveObjectFromCache(String key, CacheConfiguration config);


        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult GetObjectFromCache(String key, CacheConfiguration config);


        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetCacheConfiguration();

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetAllServers();

        #endregion Cache Contracts

        #region BuildContracts
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        BuildDetail GetLatestBuildDetail();

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Int32 GetBuildFeatureId(Int32 buildDetailId, String featureName);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult ProcessFileCheckSum(BuildDetailContext buildDetailContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult UpdateBuildStatus(BuildDetailContext buildDetailContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        OperationResult SaveBuildDetails(BuildDetailContext buildDetailContext);
        #endregion
    }
}