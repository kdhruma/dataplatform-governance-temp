using Newtonsoft.Json.Linq;
using System;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace JsonNetMessageFormatter
{
    [ServiceContract]
    public interface ITestService
    {
        [WebGet, OperationContract]
        Person GetPerson();
        [WebInvoke, OperationContract]
        Pet EchoPet(Pet pet);
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest), OperationContract]
        int Add(int x, int y);
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest), OperationContract]
        int AddJObject(String x, String y);
    }
}
