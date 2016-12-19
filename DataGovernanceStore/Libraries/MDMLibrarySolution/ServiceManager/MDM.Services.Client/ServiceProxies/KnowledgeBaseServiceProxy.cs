using System;

namespace MDM.Services.ServiceProxies
{
    using KnowledgeBaseServiceClient;

    /// <summary>
    /// Specifies the KnowledgeBase ServiceProxy class
    /// </summary>
    public class KnowledgeBaseServiceProxy : KnowledgeBaseServiceClient, MDM.WCFServiceInterfaces.IKnowledgeBaseService
    {
        #region Constructors

        /// <summary>
        /// Parameter less constructor of <see cref="KnowledgeBaseServiceProxy"/> class.
        /// </summary>
        public KnowledgeBaseServiceProxy()
        {

        }

        /// <summary>
        /// Constructor with endpoint configuration Name. Initialize the <see cref="KnowledgeBaseServiceProxy"/> class.
        /// </summary>
        /// <param name="endpointConfigurationName">Indicates the endpoint configuration name.</param>
        public KnowledgeBaseServiceProxy(String endpointConfigurationName)
            : base(endpointConfigurationName)
        {
        }

        /// <summary>
        /// Constructor with endpointConfiguration name and remote point address.
        /// </summary>
        /// <param name="endpointConfigurationName">Indicates the endpoint configuration name.</param>
        /// <param name="remoteAddress">Indicates the remote point address</param>
        public KnowledgeBaseServiceProxy(String endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        #endregion
    }
}
