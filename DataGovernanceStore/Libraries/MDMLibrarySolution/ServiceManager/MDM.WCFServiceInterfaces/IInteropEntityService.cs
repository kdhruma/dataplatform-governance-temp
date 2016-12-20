using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using MDM.BusinessObjects;

namespace MDM.WCFServiceInterfaces
{
    /// <summary>
    /// Defines operation contracts for MDM Data operations
    /// </summary>
    [ServiceContract(Namespace = "http://wcfservices.riversand.com")]
    public interface IInteropEntityService
    {
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetEntity(String name);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetEntityAttributeValues(String name);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetEntityChildren(String parentEntityName);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetEntityRelationships(String name);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetEntityExtensions(String parentEntityName);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetEntityWorkFlowDetails(String name);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetEntityHistory(String name);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GenerateEntityContextXml(Boolean loadAttributes, Boolean loadChildren, Boolean loadRelationships, Boolean loadWorkflowInformation, Boolean loadOnlyRequiredAttributes, String attributeIdList, String dataLocale);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetEntityAdvanced(String name, String entityContext, String outputFormat);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetEntityById(Int64 entityId, String entityContext, String outputFormat);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String SearchEntities(String searchCriteria, String searchContext);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String ProcessEntity(String entityXML);
    }
}
