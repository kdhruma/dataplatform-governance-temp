using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace MDM.WCFServiceInterfaces
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Denorm;
    using MDM.Core;

    /// <summary>
    /// Defines operation contracts for Denorm Data operations
    /// </summary>
    [ServiceContract(Namespace = "http://wcfservices.riversand.com")]
    public interface IDenormService
    {
        #region Get Impacted Entity

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        ImpactedEntityCollection GetImpactedEntitiesByEntityActivityLogId(Int64 entityActivityLogId, CallerContext callerContext);

        [OperationContract(Name = "GetImpactedEntitiesByEntityActivityLogIdWithPagination")]
        [FaultContract(typeof(MDMExceptionDetails))]
        ImpactedEntityCollection GetImpactedEntitiesByEntityActivityLogId(Int64 entityActivityLogId, Int64 fromRecordNumber, Int64 toRecordNumber, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Int64 LoadImpactedEntities(EntityActivityLogCollection entityActivityLogCollection, ImpactType impactType, String programName, CallerContext callerContext);

        #endregion Get Impacted Entity

        #region Get Entity Processor Error Log

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityProcessorErrorLogCollection GetEntityProcessorErrorLog(Collection<Int64> entityIdList, CallerContext callerContext);

        [OperationContract(Name = "GetEntityProcessorErrorLogWithPagination")]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityProcessorErrorLogCollection GetEntityProcessorErrorLog(Collection<Int64> entityIdList, Int64 fromRecordNumber, Int64 toRecordNumber, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityProcessorErrorLogCollection GetAllEntityProcessorErrorLog(CallerContext callerContext);

        [OperationContract(Name = "GetAllEntityProcessorErrorLogWithPagination")]
        [FaultContract(typeof(MDMExceptionDetails))]
        EntityProcessorErrorLogCollection GetAllEntityProcessorErrorLog(Int64 fromRecordNumber, Int64 toRecordNumber, CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean RefreshEntityProcessorErrorLog(Int64 impactedEntityId, Int32 containerId, String processorName,  Int64 entityActivityLogId, CallerContext callerContext, Collection<Int32> impactedAttributeIdList = null, Collection<Int32> impactedLocaleIdList = null);

        #endregion Get Entity Processor Error Log

        #region Process Impacted EntityLog

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean ProcessEntityActivityLogs(EntityActivityLogCollection entityActivityLogCollection, CallerContext callerContext);

        #endregion Process Impacted EntityLog

        #region ParallelProcessing Engine monitoring

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        ParallelizationEngineStatus GetParallelizationEngineStatus();

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean StartParallelProcessingEngine();

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean StopParallelProcessingEngine();
       
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean RestartParallelProcessingEngine();

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean RestartProcessor(CoreDataProcessorList dataProcessor);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean StartProcessor(CoreDataProcessorList dataProcessor);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean StopProcessor(CoreDataProcessorList dataProcessor);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        ServiceStatusCollection GetServiceStatus(CallerContext callerContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Boolean ProcessServiceStatus(String serverName, MDMServiceType service, MDMServiceSubType serviceSubType, String serviceStatusXml, String serviceConfigXml, CallerContext callerContext);

        #endregion ParallelProcessing Engine monitoring
    }
}
