using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MDM.Services.ServiceProxies
{
    using MDM.BusinessObjects;
    using MDM.Services.LegacyDataAccessServiceClient;

    /// <summary>
    /// Represents class for legacy data access service proxy
    /// </summary>
    public class LegacyDataAccessServiceProxy : LegacyDataAccessServiceClient, MDM.WCFServiceInterfaces.ILegacyDataAccessService
    {
        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public LegacyDataAccessServiceProxy()
        {

        }

        /// <summary>
        /// Constructor with endpoint configuration name
        /// </summary>
        /// <param name="endpointConfigurationName">Indicates the endpoint configuration name</param>
        public LegacyDataAccessServiceProxy(String endpointConfigurationName)
            : base(endpointConfigurationName)
        {
        }

        /// <summary>
        /// Constructor with endpoint configuration name and remote address
        /// </summary>
        /// <param name="endpointConfigurationName">Indicates the endpoint configuration name</param>
        /// <param name="remoteAddress">Indicates the remote address</param>
        public LegacyDataAccessServiceProxy(String endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        #endregion

        /// <summary>
        /// Gets parent entities
        /// </summary>
        /// <param name="cnodeId">Indicates identifier of entity</param>
        /// <param name="cnodeParentId">Indicates parent identifier of entity</param>
        /// <param name="containerId">Indicates identifier of container</param>
        /// <param name="dataLocale">Indicates current data locale</param>
        /// <returns>Returns list of parent entities</returns>
        public new List<Entity> GetParentOPBL(long cnodeId, long cnodeParentId, int containerId, int dataLocale)
        {
            return base.GetParentOPBL(cnodeId, cnodeParentId, containerId, dataLocale).ToList();
        }

        /// <summary>
        /// Gets collection of business rule based on rule identifiers
        /// </summary>
        /// <param name="ruleIds">Indicates rule identifiers</param>
        /// <returns>Returns collection of business rule based on rule identifiers</returns>
        public Collection<BusinessRule> GetBusinessRulesById(IEnumerable<int> ruleIds)
        {
            return base.GetBusinessRulesById(new Collection<int>(ruleIds.ToList()));
        }

    }
}
