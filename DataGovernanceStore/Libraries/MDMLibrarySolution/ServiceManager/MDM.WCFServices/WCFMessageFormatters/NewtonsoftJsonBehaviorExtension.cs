using System;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;

namespace MDM.WCFServices.WCFMessageFormatters
{
    public class NewtonsoftJsonBehaviorExtension : BehaviorExtensionElement
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
    }
}
