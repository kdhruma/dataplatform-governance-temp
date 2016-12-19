using System;
using System.Xml;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Interop;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Runtime.Serialization;

namespace MDM.Workflow.Designer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields

        private static String _viewType = String.Empty;

        private static String _instanceGuid = String.Empty;

        private static Int32 _workflowVersionId = 0;

        private static String _loginUser = String.Empty;

        private static EndpointAddress _remoteAddress = null;

        private static Binding _binding = null;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public static String ViewType
        {
            get
            {
                return _viewType;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static String InstanceGuid
        {
            get
            {
                return _instanceGuid;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static Int32 WorkflowVersionId
        {
            get
            {
                return _workflowVersionId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static String LoginUser
        {
            get
            {
                return _loginUser;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static EndpointAddress WCFRemoteAddress
        {
            get
            {
                return _remoteAddress;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static Binding WCFBinding
        {
            get
            {
                return _binding;
            }
        }

        #endregion

        #region App Events

        void Application_Start(Object sender, StartupEventArgs e)
        {
            try
            {
                String queryString = BrowserInteropHelper.Source.Query;

                if (!String.IsNullOrEmpty(queryString))
                {
                    queryString = queryString.Trim('?');

                    String[] queryData = queryString.Split("&".ToCharArray(), 5);

                    if (queryData != null && queryData.Length > 0)
                    {
                        if (queryData[0] != null)
                            _viewType = queryData[0];

                        if (queryData[1] != null)
                            _loginUser = queryData[1];

                        if (queryData[2] != null)
                            Int32.TryParse(queryData[2], out _workflowVersionId);

                        if (queryData[3] != null)
                            _instanceGuid = queryData[3];

                        if (queryData[4] != null)
                        {
                            String serviceURL = queryData[4];

                            //Get the remote address
                            if (!String.IsNullOrEmpty(serviceURL))
                            {
                                _remoteAddress = new EndpointAddress(serviceURL);
                                _binding = CreateBinding("WSHttpBinding_IWorkflowDesignerService");
                            }
                        }

                        if (_viewType == "DefinitionView")
                            this.StartupUri = new Uri("DefinitionView.xaml", UriKind.Relative);
                        else if (_viewType == "ExecutionView")
                            this.StartupUri = new Uri("ExecutionView.xaml", UriKind.Relative);
                        else if (_viewType == "Designer")
                            this.StartupUri = new Uri("DesignerMainPage.xaml", UriKind.Relative);
                    }
                }
                else
                {
                    this.StartupUri = new Uri("DesignerMainPage.xaml", UriKind.Relative);
                }

            }
            catch (Exception ex)
            {
                String errorMessage = String.Format("The following error occurred while loading.\nError: {0}", ex.Message);
                MessageBox.Show(errorMessage, "MDM.Workflow.Designer", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Methods

        private WSHttpBinding CreateBinding(String bindingName)
        {
            WSHttpBinding httpBinding = new WSHttpBinding();

            //binding
            httpBinding.Name = bindingName;
            httpBinding.CloseTimeout = TimeSpan.Parse("00:12:00");
            httpBinding.OpenTimeout = TimeSpan.Parse("00:12:00");
            httpBinding.ReceiveTimeout = TimeSpan.Parse("00:12:00");
            httpBinding.SendTimeout = TimeSpan.Parse("00:12:00");
            httpBinding.BypassProxyOnLocal = false;
            httpBinding.TransactionFlow = false;
            httpBinding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            httpBinding.MaxBufferPoolSize = 524288;
            httpBinding.MaxReceivedMessageSize = 2147483647;
            httpBinding.MessageEncoding = WSMessageEncoding.Text;
            httpBinding.TextEncoding = System.Text.Encoding.UTF8;
            httpBinding.UseDefaultWebProxy = true;
            httpBinding.AllowCookies = false;

            //readerQuotas
            httpBinding.ReaderQuotas.MaxDepth = 32;
            httpBinding.ReaderQuotas.MaxStringContentLength = 2048000;
            httpBinding.ReaderQuotas.MaxArrayLength = 2147483647;
            httpBinding.ReaderQuotas.MaxBytesPerRead = 4096;
            httpBinding.ReaderQuotas.MaxNameTableCharCount = 16384;

            //reliableSession
            httpBinding.ReliableSession.Ordered = true;
            httpBinding.ReliableSession.InactivityTimeout = TimeSpan.Parse("00:12:00");
            httpBinding.ReliableSession.Enabled = false;

            //security
            httpBinding.Security.Mode = SecurityMode.None;
            httpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
            httpBinding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
            httpBinding.Security.Transport.Realm = "";
            httpBinding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
            httpBinding.Security.Message.NegotiateServiceCredential = true;
            httpBinding.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.Default;

            //return
            return httpBinding;
        }

        #endregion
    }
}
