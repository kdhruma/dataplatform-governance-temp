using System;
using System.Collections.ObjectModel;


namespace MDM.Services.ServiceProxies
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Denorm;
    using MDM.Core;
    using MDM.Services.DenormServiceClient;

    /// <summary>
    /// Represents class for denorm service proxy
    /// </summary>
    public class DenormServiceProxy : DenormServiceClient, MDM.WCFServiceInterfaces.IDenormService
    {
        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public DenormServiceProxy()
        {

        }

        /// <summary>
        /// Constructor with endpoint configuration name
        /// </summary>
        /// <param name="endpointConfigurationName">Indicates the endpoint configuration name</param>
        public DenormServiceProxy(String endpointConfigurationName)
            : base(endpointConfigurationName)
        {
        }

        /// <summary>
        /// Constructor with endpoint configuration name and remote address
        /// </summary>
        /// <param name="endpointConfigurationName">Indicates the endpoint configuration name</param>
        /// <param name="remoteAddress">Indicates the remote address</param>
        public DenormServiceProxy(String endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        #endregion

        #region IDenormService methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processingStatus"></param>
        /// <param name="fromRecordNumber"></param>
        /// <param name="toRecordNumber"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public EntityActivityLogCollection GetEntityActivityLogs(ProcessingStatus processingStatus, long fromRecordNumber, long toRecordNumber, CallerContext callerContext)
        {
            return GetEntityActivityLogsWithPagination(processingStatus, fromRecordNumber, toRecordNumber, callerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityActivityLogId"></param>
        /// <param name="fromRecordNumber"></param>
        /// <param name="toRecordNumber"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public ImpactedEntityCollection GetImpactedEntitiesByEntityActivityLogId(long entityActivityLogId, long fromRecordNumber, long toRecordNumber, CallerContext callerContext)
        {
            return GetImpactedEntitiesByEntityActivityLogIdWithPagination(entityActivityLogId, fromRecordNumber,
                                                                          toRecordNumber, callerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityIdList"></param>
        /// <param name="fromRecordNumber"></param>
        /// <param name="toRecordNumber"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public EntityProcessorErrorLogCollection GetEntityProcessorErrorLog(Collection<long> entityIdList, long fromRecordNumber, long toRecordNumber, CallerContext callerContext)
        {
            return GetEntityProcessorErrorLogWithPagination(entityIdList, fromRecordNumber, toRecordNumber,
                                                            callerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromRecordNumber"></param>
        /// <param name="toRecordNumber"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public EntityProcessorErrorLogCollection GetAllEntityProcessorErrorLog(long fromRecordNumber, long toRecordNumber, CallerContext callerContext)
        {
            return GetAllEntityProcessorErrorLogWithPagination(fromRecordNumber, toRecordNumber, callerContext);
        }

        #endregion IDenormService methods

    }
}
