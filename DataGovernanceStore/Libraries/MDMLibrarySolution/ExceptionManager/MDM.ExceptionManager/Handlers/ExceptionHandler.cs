
using System;
using System.Web;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Web.Services.Protocols;
using System.Xml;

namespace MDM.ExceptionManager
{
    //Doing this would give importance to our internal types than .net types
    using MDM.ExceptionManager.Config;
    using MDM.ExceptionManager.Handlers;

    /// <summary>
    /// Performs logging functions to EventViewer, Database and Send Email notification.
    /// </summary>
    public class ExceptionHandler
    {
        #region Fields

        private Exception applicationException = null;
        private ModuleSettings moduleSettings = null;
        private HttpContext context = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ExceptionHandler class.
        /// </summary>
        /// <param name="ex">Indicates the exception details.</param>
        public ExceptionHandler(Exception ex)
        {
            // Persist the exception
            applicationException = ex;
            moduleSettings = ModuleConfig.GetSettings;

            //use exception manager only if its true
            if (moduleSettings.UseExceptionManager && ex != null)
            {
                HandleException();
            }
        }

        /// <summary>
        /// Initializes a new instance of the ExceptionHandler class using specified module settings file path.
        /// </summary>
        /// <param name="ex">Indicates the exception details</param>
        /// <param name="filepath">Indicates module setting file path</param>
        public ExceptionHandler(Exception ex, string filepath)
        {
            // Persist the exception
            applicationException = ex;
            moduleSettings = ModuleConfig.GetSettingsFromFile(filepath);

            //use exception manager only if its true
            if (moduleSettings.UseExceptionManager && ex != null)
            {
                HandleException();
            }
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        private void HandleException()
        {
            try
            {
                String message = GenerateExceptionMessage(applicationException, 1);
            
                //Log to database
                if (moduleSettings.LogToDB)
                {
                    throw new NotImplementedException("This method is not implemented");
                }

                //Log to Event Log
                if (moduleSettings.LogToEventViewer)
                {
                    EventLogHandler evtLogHander = new EventLogHandler(moduleSettings);
                    evtLogHander.WriteErrorLog(message, 1);
                }

                //Send email
                if (moduleSettings.SendEmail)
                {
                    MailHandler emailHandler = new MailHandler();
                    emailHandler.SendMail(message);
                }
            }
            catch (Exception ex)
            {
                //This error will occur if either db error or mail server errors
                //if there is error when writing to error log we write it to the 
                //errorlog text file
                FileLogHandler fileHandler = new FileLogHandler();
                fileHandler.LogError("Event Log", ex.ToString());
            }
        }

        private String GenerateExceptionMessage(Exception exception, Int32 level)
        {
            Int32 exceptionId = 0;
            String formCollection = GetFormCollection(exception);
            String queryString = GetQueryString(exception);
            String serverVariables = GetServerVariables(exception);
            String exceptionSource = GetExceptionSource(exception);
            String exceptionMessage = GetExceptionMessage(exception);
            String exceptionTargetSite = GetExceptionTargetSite(exception);
            String exceptionStackTrace = GetExceptionStackTrace(exception);
            String webServiceException = String.Empty;

            //check if the exception is of type SoapException
            if (exception.GetType().ToString().Equals("System.Web.Services.Protocols.SoapException"))
            {
                webServiceException = GetWebserviceException(exception);
            }

            String message = String.Empty;

            if (level == 1)
            {
                StringBuilder builder = new StringBuilder();

                builder.Append("Main Exception");
                builder.Append("\n***********************************************\n");

                message += builder.ToString();
            }

            message += CombineData(exceptionId, formCollection, queryString, serverVariables, exceptionSource, exceptionMessage, exceptionTargetSite, exceptionStackTrace, webServiceException);

            if (exception.InnerException != null)
            {
                StringBuilder builder = new StringBuilder();

                builder.Append("Inner Exception - Level: ");
                builder.Append(level);
                builder.Append("\n***********************************************\n");

                message += builder.ToString();

                message += GenerateExceptionMessage(exception.InnerException, ++level);
            }

            return message;
        }

        private String GetFormCollection(Exception exception)
        {
            String formContents = String.Empty;
            if (moduleSettings.GetLogDataSettings.FormCollection)
            {
                String[] formKeys;
                String[] formValues;

                context = HttpContext.Current;

                if (context != null)
                {
                    // Loop forms collection
                    formKeys = context.Request.Form.AllKeys;

                    for (Int32 i = 0; i < context.Request.Form.Count; i++)
                    {
                        formValues = context.Request.Form.GetValues(i);
                        for (Int32 j = 0; j <= formValues.GetUpperBound(0); j++)
                        {
                            //reject viewstate value and eventvalidation 
                            if (formKeys[i] != "__VIEWSTATE" && formKeys[i] != "__EVENTVALIDATION")
                                formContents += formKeys[i] + "=" +
                                                formValues[j] + "\n";
                        }
                    }
                }
            }
            return formContents;
        }

        private String GetQueryString(Exception exception)
        {
            String qryString = String.Empty;

            if (moduleSettings.GetLogDataSettings.QueryString)
            {
                context = HttpContext.Current;
                if (context != null)
                {
                    qryString = context.Request.QueryString.ToString();
                }
            }

            return qryString;
        }

        private String GetServerVariables(Exception exception)
        {
            String svrVariables = String.Empty;

            if (moduleSettings.GetLogDataSettings.ServerVariables)
            {
                context = HttpContext.Current;
                StringBuilder results = new StringBuilder();

                if (context != null)
                {
                    //at present recording only referer
                    String referer = String.Empty;
                    String url = String.Empty;
                    String userAgent = String.Empty;

                    if (context.Request != null)
                    {
                        if (context.Request.ServerVariables["HTTP_REFERER"] != null)
                            referer = "HTTP_REFERER : " + context.Request.ServerVariables["HTTP_REFERER"].ToString();
                        
                        if (context.Request.ServerVariables["URL"] != null)
                            url = "URL : " + context.Request.ServerVariables["URL"].ToString();

                        if (context.Request.ServerVariables["HTTP_USER_AGENT"] != null)
                            userAgent = "HTTP_USER_AGENT : " + context.Request.ServerVariables["HTTP_USER_AGENT"].ToString();
                    }

                    results.Append(referer);
                    results.Append("\n");
                    results.Append(userAgent);
                    results.Append("\n");
                    results.Append(url);
                }

                svrVariables = results.ToString();
            }

            return svrVariables;
        }

        private String GetExceptionSource(Exception exception)
        {
            String exSource = String.Empty;

            if (moduleSettings.GetLogDataSettings.ExceptionSource)
            {
                exSource = exception.Source;
            }

            return exSource;
        }

        private String GetExceptionMessage(Exception exception)
        {
            String exMessage = String.Empty;

            if (moduleSettings.GetLogDataSettings.ExceptionMessage)
            {
                exMessage = exception.Message;
            }

            return exMessage;
        }

        private String GetExceptionTargetSite(Exception exception)
        {
            String exTargetSite = String.Empty;

            if (moduleSettings.GetLogDataSettings.ExceptionTargetSite)
            {
                if (exception.TargetSite != null)
                {
                    exTargetSite = exception.TargetSite.ToString();
                }
            }

            return exTargetSite;
        }

        private String GetExceptionStackTrace(Exception exception)
        {
            String exStackTrace = String.Empty;

            if (moduleSettings.GetLogDataSettings.ExceptionStackTrace)
            {
                exStackTrace = exception.StackTrace;
            }

            return exStackTrace;

        }

        private String GetWebserviceException(Exception exception)
        {
            String soapExceptionData = String.Empty;
            try
            {
                if (moduleSettings.GetLogDataSettings.WebServiceException)
                {
                    String soapExSource = String.Empty;
                    String soapExMessage = String.Empty;
                    String soapExTargetSite = String.Empty;
                    String soapExStackTrace = String.Empty;

                    StringBuilder soapExData = new StringBuilder();

                    //cast it
                    SoapException soapEx = (SoapException)exception;

                    //get the values from details xml node.

                    XmlNode soapExDetail = soapEx.Detail;

                    //since we know what will be the xml we are able to
                    //hard code it else that schema has to be used.
                    soapExSource = soapExDetail.SelectSingleNode("//Source").InnerText;
                    soapExMessage = soapExDetail.SelectSingleNode("//Message").InnerText;
                    soapExTargetSite = soapExDetail.SelectSingleNode("//TargetSite").InnerText;
                    soapExStackTrace = soapExDetail.SelectSingleNode("//StackTrace").InnerText;

                    soapExData.Append("\n----------------------------------------\n");
                    soapExData.Append("Soap Exception Data \n");
                    soapExData.Append("********************\n");

                    //add the actor
                    soapExData.Append("Web Service Actor \n");
                    soapExData.Append("********************\n");
                    soapExData.Append(soapEx.Actor);
                    soapExData.Append("\n\n");

                    //add the source
                    soapExData.Append("Web Service Source \n");
                    soapExData.Append("********************\n");
                    soapExData.Append(soapExSource);
                    soapExData.Append("\n\n");

                    //add the source
                    soapExData.Append("Web Service Message \n");
                    soapExData.Append("********************\n");
                    soapExData.Append(soapExMessage);
                    soapExData.Append("\n\n");

                    //add the source
                    soapExData.Append("Web Service TargetSite \n");
                    soapExData.Append("***********************\n");
                    soapExData.Append(soapExTargetSite);
                    soapExData.Append("\n\n");

                    //add the source
                    soapExData.Append("Web Service StackTrace \n");
                    soapExData.Append("***********************\n");
                    soapExData.Append(soapExStackTrace);
                    soapExData.Append("\n\n");

                    soapExceptionData = soapExData.ToString();
                }

                return soapExceptionData;
            }
            catch
            {
                throw;
            }
        }

        private String CombineData(Int32 exceptionID, String formCollection, String queryString, String serverVariables, String exceptionSource, String exceptionMessage, String exceptionTargetSite, String exceptionStackTrace, String webServiceException)
        {
            try
            {
                StringBuilder results = new StringBuilder();

                if (!String.IsNullOrEmpty(formCollection))
                {
                    results.Append("Forms Collection \n");
                    results.Append("----------------------------\n");
                    results.Append(formCollection);
                    results.Append("\n");
                }

                if (!String.IsNullOrEmpty(queryString))
                {
                    results.Append("Query String \n");
                    results.Append("----------------------------\n");
                    results.Append(queryString);
                    results.Append("\n\n");
                }

                if (!String.IsNullOrEmpty(serverVariables))
                {
                    results.Append("Server Variables \n");
                    results.Append("----------------------------\n");
                    results.Append(serverVariables);
                    results.Append("\n\n");
                }

                if (!String.IsNullOrEmpty(exceptionSource))
                {
                    results.Append("Exception Source \n");
                    results.Append("----------------------------\n");
                    results.Append(exceptionSource);
                    results.Append("\n\n");
                }

                if (!String.IsNullOrEmpty(exceptionMessage))
                {
                    results.Append("Exception Message \n");
                    results.Append("----------------------------\n");
                    results.Append(exceptionMessage);
                    results.Append("\n\n");
                }

                if (!String.IsNullOrEmpty(exceptionTargetSite))
                {
                    results.Append("Exception TargetSite \n");
                    results.Append("----------------------------\n");
                    results.Append(exceptionTargetSite);
                    results.Append("\n\n");
                }

                if (!String.IsNullOrEmpty(exceptionStackTrace))
                {
                    results.Append("Exception StackTrace \n");
                    results.Append("----------------------------\n");
                    results.Append(exceptionStackTrace);
                    results.Append("\n\n");
                }

                if (!String.IsNullOrEmpty(webServiceException))
                {
                    results.Append(webServiceException);
                }

                if (exceptionID != 0)
                {
                    String exUrl = moduleSettings.ExceptionUrl;

                    results.Append("Exception URL \n");
                    results.Append("----------------------------\n");
                    results.Append(exUrl);
                    results.Append(exceptionID.ToString());
                    results.Append("\n\n\n");
                }

                return results.ToString();
            }
            catch
            {
                throw;
            }
        }

        //private Hashtable GenerateDTO()
        //{
        //    try
        //    {
        //        Hashtable records = new Hashtable();

        //        records.Add("FormCollection", formCollection);
        //        records.Add("QueryString", queryString);
        //        records.Add("ServerVariables", serverVariables);
        //        records.Add("ExceptionSource", exceptionSource);
        //        records.Add("ExceptionMessage", exceptionMessage);
        //        records.Add("ExceptionTargetSite", exceptionTargetSite);
        //        records.Add("ExceptionStackTrace", exceptionStackTrace);
        //        records.Add("WebServiceException", webServiceException);

        //        return records;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        #endregion
    }
}
