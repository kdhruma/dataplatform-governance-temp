using System;
using System.Collections.ObjectModel;
using System.ServiceModel;

namespace MDM.WCFServiceInterfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Defines operation contracts for MDM Knowledge Base
    /// </summary>
    [ServiceContract(Namespace = "http://wcfservices.riversand.com")]
    public interface IKnowledgeBaseService
    {
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Locale GetLocale(Int32 localeId);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        LocaleCollection GetAvailableLocales();
        
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        LocaleCollection GetAllLocales();
        
        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        LocaleCollection GetLocalesByContainer(Int32 containerId);

        [OperationContract]
        [FaultContract(typeof(MDMExceptionDetails))]
        Collection<String> GetAllLookupTableNames(CallerContext callerContext, LookupContext lookupContext = null);
    }
}
