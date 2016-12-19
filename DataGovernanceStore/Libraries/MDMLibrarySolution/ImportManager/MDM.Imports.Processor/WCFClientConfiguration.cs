using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Principal;
using System.Threading;

using System.IO;
using System.Xml;
using System.ServiceModel;

namespace MDM.Imports.Processor
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.Core.Exceptions;
    using MDM.BusinessObjects;
    using MDM.Services;
 
    public class WCFClientConfiguration : IWCFClientConfiguration
    {
        #region Fields

        /// <summary>
        /// Field denoting the binding prefix key
        /// </summary>
        private String _endPointConfigurationPrefix = String.Empty;

        /// <summary>
        /// Field denoting the binding prefix key
        /// </summary>
        private String _endPointConfigurationName = String.Empty;

        /// <summary>
        /// Field denoting the Address of EndPoint
        /// </summary>
        private EndpointAddress _endpointAddress = null;

        /// <summary>
        /// 
        /// </summary>
        private AuthenticationType _authenticationType = AuthenticationType.Unknown;

        /// <summary>
        /// 
        /// </summary>
        private IIdentity _userIdentity = null;

        /// <summary>
        /// Field denoting the Current Username 
        /// </summary>
        private String _userName = String.Empty;

        /// <summary>
        /// Field denoting the User's Password
        /// </summary>
        private String _password = String.Empty;
        /// <summary>
        /// 
        /// </summary>
        private WCFBindingType _wcfBindingType = WCFBindingType.Unknown;

        /// <summary>
        /// 
        /// </summary>
        private Boolean _isDelegationEnabled = true;

        #endregion

        #region Constructors

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting the Current Username 
        /// </summary>
        public String EndPointConfigurationPrefix
        {
            get { return _endPointConfigurationPrefix; }
            set { _endPointConfigurationPrefix = value; }
        }

        /// <summary>
        /// Property denoting the Current Username 
        /// </summary>
        public String EndPointConfigurationName
        {
            get { return _endPointConfigurationName; }
            set { _endPointConfigurationName = value; }
        }

        /// <summary>
        /// Property denoting EndPoint Address
        /// </summary>
        public EndpointAddress EndpointAddress
        {
            get { return _endpointAddress; }
            set { _endpointAddress = value; }
        }
        /// <summary>
        /// Property denoting authentication type
        /// </summary>
        public AuthenticationType AuthenticationType
        {
            get { return _authenticationType; }
            set { _authenticationType = value; }
        }

        /// <summary>
        /// Property denoting user identity
        /// </summary>
        public IIdentity UserIdentity
        {
            get { return _userIdentity; }
            set { _userIdentity = value; }
        }

        /// <summary>
        /// Property denoting the Current Username 
        /// </summary>
        public String UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        /// <summary>
        /// Property denoting the User's Password
        /// </summary>
        public String Password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// <summary>
        /// Property denoting if delegation is enabled..
        /// </summary>
        public Boolean IsDelegationEnabled
        {
            get { return _isDelegationEnabled; }
            set { _isDelegationEnabled = value; }
        }

        /// <summary>
        /// Property denoting WCF BindingType
        /// </summary>
        public WCFBindingType WCFBindingType
        {
            get { return _wcfBindingType; }
            set { _wcfBindingType = value; }
        }

        #endregion

        #region Methods

        #region Public Methods

        public static IWCFClientConfiguration GetConfiguration(MDMWCFServiceList wcfServiceName, String userName)
        {
            WCFClientConfiguration _instance = new WCFClientConfiguration();
            _instance.LoadConfigurations(wcfServiceName, userName);
            
            return (IWCFClientConfiguration)_instance;
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadConfigurations(MDMWCFServiceList wcfServiceName, String userName)
        { 
            String strIsDelegationEnabled = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.Authentication.Windows.DelegationEnabled");
            this._isDelegationEnabled = ValueTypeHelper.BooleanTryParse(strIsDelegationEnabled, true);

            String internalMessageSecurityPassword = AppConfigurationHelper.GetAppConfig<String>("MDM.WCFServices.MessageSecurity.IdentityKey");

            this._userIdentity = null; //set default identity to null
            this._userName = userName;
            this._password = String.Empty;

            if (userName.Equals("System") && this._isDelegationEnabled)
            {
                this._authenticationType = Core.AuthenticationType.Windows;
                this._wcfBindingType = Core.WCFBindingType.NetTcpBinding;
                this._userIdentity = SecurityPrincipalHelper.GetCurrentWindowsIdentity();

                if (this._userIdentity != null)
                {
                    this._userName = this._userIdentity.Name;
                }
                else
                {
                    throw new ApplicationException("WindowsIdentity is not found or system is not configured properly. Please check authentication settings.");
                }

                this._password = String.Empty;
            }
            else
            {
                this._authenticationType = Core.AuthenticationType.Forms;
                this._wcfBindingType = Core.WCFBindingType.WSHttpBinding;
                this._userName = userName;
                this._userIdentity = null;
                this._password = internalMessageSecurityPassword;
            }

            //Binding Config Prefix Name
            this._endPointConfigurationPrefix = this._wcfBindingType.ToString();
            String endPointConfigurationName = String.Format("{0}_I{1}", this._endPointConfigurationPrefix, wcfServiceName.ToString());
            this._endPointConfigurationName = endPointConfigurationName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String ToXml()
        {
            String conifgXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("WCFClientConfiguration");

            xmlWriter.WriteAttributeString("IsDelegationEnabled", this.IsDelegationEnabled.ToString());
            xmlWriter.WriteAttributeString("AuthenticationType", this.AuthenticationType.ToString());
            xmlWriter.WriteAttributeString("WCFBindingType", this.WCFBindingType.ToString());
            xmlWriter.WriteAttributeString("EndPointConfigurationPrefix", this.EndPointConfigurationPrefix);
            xmlWriter.WriteAttributeString("EndPointConfigurationName", this.EndPointConfigurationName);
            xmlWriter.WriteAttributeString("UserName", this.UserName.ToString());

            if(this.UserIdentity != null)
                xmlWriter.WriteAttributeString("UserIdentity_Name", this.UserIdentity.Name);
            
            //node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            conifgXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return conifgXml;
        }

        #endregion
        
        #region Private Methods

        #endregion

        #endregion

    }
}
