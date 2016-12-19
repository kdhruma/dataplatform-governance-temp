using MDM.BusinessObjects;
using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace MDM.WCFServiceInterfaces
{
    /// <summary>
    ///  Interface for Jigsaw integration service.
    /// </summary>
     [ServiceContract(Namespace = "http://wcfservices.riversand.com")]
    public interface IJigsawIntegrationService
    {
        /// <summary>
        /// Process the message
        /// </summary>
        /// <param name="streamdata"></param>
        /// <returns></returns>
         [OperationContract]
         [FaultContract(typeof(MDMExceptionDetails))]
         [WebInvoke(Method = "POST", UriTemplate = "/ProcessEntityOperationMessage", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Int32 ProcessEntityOperationMessage(Stream streamdata);
    }
}
