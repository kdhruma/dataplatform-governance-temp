using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace MDM.Services.ServiceProxies
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.ExceptionManager;
    using MDM.WCFServiceInterfaces;
    using MDM.Services.CoreServiceClient;
    using MDM.BusinessObjects.Caching;

    /// <summary>
    /// Represents class for core service proxy
    /// </summary>
    public class CoreServiceProxy : CoreServiceClient, MDM.WCFServiceInterfaces.ICoreService
    {
        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public CoreServiceProxy()
        { 
        
        }

        /// <summary>
        /// Constructor with endpoint configuration name
        /// </summary>
        /// <param name="endpointConfigurationName">Indicates the endpoint configuration name</param>
        public CoreServiceProxy(String endpointConfigurationName) 
            : base(endpointConfigurationName) 
        {
        }

        /// <summary>
        /// Constructor with endpoint configuration name and remote address
        /// </summary>
        /// <param name="endpointConfigurationName">Indicates the endpoint configuration name</param>
        /// <param name="remoteAddress">Indicates the remote address</param>
        public CoreServiceProxy(String endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) 
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        #endregion


        #region ICoreService Members        

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

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="entityId"></param>
        ///// <param name="relatedKeyEnum"></param>
        ///// <returns></returns>
        //public OperationResult GetEntityFromCache(long entityId, EntityCacheKeyEnum relatedKeyEnum)
        //{
        //    return base.GetEntityFromCache(entityId, relatedKeyEnum);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityShortName"></param>
        /// <param name="relatedKeyEnum"></param>
        /// <returns></returns>
        public OperationResult GetEntityFromCache(string entityShortName, EntityCacheKeyEnum relatedKeyEnum)
        {
            throw new NotImplementedException();
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public String GetAllServers()
        //{
        //    throw new NotImplementedException();
        //}


        #endregion

        #region ICoreService Members

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public String GetCacheConfiguration()
        //{
        //    throw new NotImplementedException();
        //}

        //public OperationResult RemoveObjectFromCache(String name, CacheConfiguration config)
        //{
        //    throw new NotImplementedException();
        //}

        //public OperationResult GetObjectFromCache(int objectId, EntityCacheConfiguration config)
        //{
        //    throw new NotImplementedException();
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="name"></param>
        ///// <param name="config"></param>
        ///// <returns></returns>
        //public OperationResult GetObjectFromCache(String name, CacheConfiguration config)
        //{
        //    throw new NotImplementedException();
        //}



        #endregion
    }
}
