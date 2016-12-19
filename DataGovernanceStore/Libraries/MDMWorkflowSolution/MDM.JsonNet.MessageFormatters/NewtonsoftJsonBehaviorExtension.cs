using System;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;

namespace MDM.JsonNet.MessageFormatters
{
    public class NewtonsoftJsonBehaviorExtension : BehaviorExtensionElement//, IEndpointBehavior 
    {

        public override Type BehaviorType
        {
            get
            {
                return typeof(NewtonsoftJsonBehavior);
            }
        }

        protected override object CreateBehavior()
        {
            return new NewtonsoftJsonBehavior();
        }

        //public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        //{

        //}

        //public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        //{
        //}

        //public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        //{
        //}

        //public void Validate(ServiceEndpoint endpoint)
        //{
        //}
    }
}
