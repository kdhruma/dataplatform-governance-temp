using System;
using System.Diagnostics;
using System.Web.Security;
using System.ServiceModel.Activation;
using System.Web.Routing;

namespace MDM.WCFServices
{
    using MDM.AdminManager.Business;
    using MDM.Core;
    using MDM.Utility;

    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            AppConfigurationHelper.InitializeAppConfig(new AppConfigProviderUsingDB());

            // Note: We dont need any initalization on WCF's Application_Start event.
            // All application startup initialization has to be done at GlobalEvents.OnApplicationStart method
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("WCFServices.Session_Start", true);
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Nothing to execute in WCFServices.Server.Session_Start.");
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("WCFServices.Session_Start");
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        void FormsAuthentication_OnAuthenticate(Object sender, FormsAuthenticationEventArgs e)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("WCFServices.Forms_Authenticate", true);
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Request URL:{0}", e.Context.Request.CurrentExecutionFilePath));
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("WCFServices.Forms_Authenticate");
        }

        void WindowsAuthentication_OnAuthenticate(Object sender, WindowsAuthenticationEventArgs e)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("WCFServices.Application.Windows_Authenticate", true);
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Request URL:{0}", e.Context.Request.CurrentExecutionFilePath));
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("WCFServices.Application.Windows_Authenticate");
        }

        protected void Application_AuthorizeRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            GlobalEvents.OnApplicationEnd();
        }

        #region Private Methods

        #endregion
    }
}