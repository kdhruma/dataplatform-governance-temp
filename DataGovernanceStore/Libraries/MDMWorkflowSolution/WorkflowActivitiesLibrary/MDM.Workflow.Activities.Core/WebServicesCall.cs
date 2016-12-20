using System;
using System.Drawing;
using System.Activities;
using System.ComponentModel;
using System.Net;
using System.IO;
using System.Text;

using MDM.BusinessObjects.Workflow;
using MDM.Workflow.Activities.Designer;

namespace MDM.Workflow.Activities.Core
{
    /// <summary>
    /// 
    /// </summary>
    [Designer(typeof(WebServicesCallDesigner))]
    [ToolboxBitmap(typeof(WebServicesCallDesigner), "Images.WebServicesCall.bmp")]
	public class WebServicesCall : MDMCodeActivitiyBase<String>
    {
		#region Fields

		private static string _soapEnvelope = @"<soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'><soap:Body></soap:Body></soap:Envelope>";

		#endregion

		#region Members

		[DisplayName("Web Service URL")]
		[Category("Web Service Arguments")]
		[RequiredArgument]
		public InArgument<String> WebRequestUrl { get; set; }

		[DisplayName("SOAP Method Name")]
		[Category("Web Service Arguments")]
		[RequiredArgument]
		public InArgument<String> SoapAction { get; set; }

		[DisplayName("SOAP Arguments")]
		[Category("Web Service Arguments")]
		[RequiredArgument]
		public InArgument<String> SoapIn { get; set; }

		/// <summary>
		/// Action Context in workflow client, provides action details for Activity
		/// </summary>
		[Browsable(false)]
		public new OutArgument<WorkflowActionContext> MDMActionContext { get; set; }

		#endregion

		protected override String Execute(CodeActivityContext context)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(WebRequestUrl.Get(context));
			httpWebRequest.Headers.Add("SOAPAction", SoapAction.Get(context));
			httpWebRequest.ContentType = "text/xml;charset=\"utf-8\"";
			httpWebRequest.Accept = "text/xml";
			httpWebRequest.Method = "POST";
			httpWebRequest.AuthenticationLevel = System.Net.Security.AuthenticationLevel.None;

			StringBuilder sb = new StringBuilder(_soapEnvelope);
			sb.Insert(sb.ToString().IndexOf("</soap:Body>"), SoapIn.Get(context));

			using (Stream RequestStream = httpWebRequest.GetRequestStream())
				using (StreamWriter RequestStreamWriter = new StreamWriter(RequestStream))
					RequestStreamWriter.Write(SoapIn.Get(context));

			WebResponse webResponse = httpWebRequest.GetResponse();
			using (StreamReader ResponseStreamReader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8, true))
				return ResponseStreamReader.ReadToEnd();
		}
    }
}
