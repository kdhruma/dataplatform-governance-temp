using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using MDM.BusinessObjects;
using MDM.Core;

namespace MDM.WCFServiceInterfaces
{
    /// <summary>
    /// Defines operation contracts for MDM Data operations
    /// </summary>
    [ServiceContract(Namespace = "http://wcfservices.riversand.com")]
    public interface IInteropDataModelService
    {
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetOrganizationByName(String token, String organizationShortName);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetContainerById(String token, Int32 containerId);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetContainerByName(String token, String containerShortName);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetAllCategories(String token, Int32 hierarchyId, LocaleEnum locale);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetCategoryById(String token, Int32 hierarchyId, Int64 categoryId);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetCategoryByName(String token, Int32 hierarchyId, String categoryName);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetLookupModel(String token, String lookupTableName);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetLookupData(String token, String lookupTableName, LocaleEnum locale, Int32 maxRecordsToReturn);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetAttributeLookupData(String token, Int32 attributeId, LocaleEnum locale, Int32 maxRecordsToReturn);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        String GetAllEntityTypes(String token);
    }
}
