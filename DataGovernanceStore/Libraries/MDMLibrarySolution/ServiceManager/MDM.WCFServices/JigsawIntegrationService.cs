using System;
using System.Text;

using MDM.WCFServiceInterfaces;
using MDM.AdminManager.Business;
using MDM.Core;
using MDM.IntegrationManager.Business;
using MDM.BusinessObjects;
using System.ServiceModel;
using System.ServiceModel.Web;
//using System.ServiceModel.Activation;
using System.IO;
using System.Diagnostics;
using MDM.BusinessObjects.Diagnostics;
using MDM.Utility;

namespace MDM.WCFServices
{
    /// <summary>
    /// This class implements the RestAPI that will be used by Jigsaw for integrating with MDMCenter.
    /// This Service doesn't enable any standard MDMC Authentication framework. The assumption is only Jigsaw knows about this service. 
    /// </summary>
    public class JigsawIntegrationService : IJigsawIntegrationService
    {
        private static CallerContext __callerContext = new CallerContext(MDMCenterApplication.DataQualityManagement, MDMCenterModules.JigsawIntegration);

        #region IJigsawIntegrationService Members

        /// <summary>
        /// This method gets the EntityOperation from Jigsaw as JSON string and saves that JSON in the Integration Activity Log. 
        /// The integration process will pick up the JSON and does further processing. 
        /// </summary>
        /// <param name="streamdata"></param>
        /// <returns></returns>
        public int ProcessEntityOperationMessage(Stream streamdata)
        {
            TraceSettings currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            ExecutionContext executionContext = null;

            try
            {
                #region Step : Diagnostic Activity initialization

                String traceMessage = String.Empty;

                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    executionContext = new ExecutionContext(__callerContext, null, null, null);
                    executionContext.LegacyMDMTraceSources.Add(MDMTraceSource.DQM);
                    diagnosticActivity.Start(executionContext);
                }

                #endregion

                //Read all the JSON from the call; 
                //We have created the parameter to be a stream object rather than a string because for some reason the WCF framework calls the XML serializer from JSON serializer.
                //After considerable amount of time  in debugging the issue, we found that it is core to the WCF framework and hard to change. The only hack to get the JSON payload
                //is to get the whole stream and convert that to a string. In this case, our integration protocol is in JSON format so we are good to use this hack. 
                StreamReader reader = new StreamReader(streamdata);
                string entityOperationJson = reader.ReadToEnd();
                reader.Close();
                reader.Dispose();

                if (currentTraceSettings.IsTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Entity Operation Json: " + entityOperationJson);
                    diagnosticActivity.LogDurationInfo("Processing of EntityOperationRequest started");
                }

                //Call the File Provider and generate a file Id
                FileBL blinfo = new FileBL();

                BusinessObjects.File file = new BusinessObjects.File();

                file.FileData = Encoding.UTF8.GetBytes(entityOperationJson);
                file.Action = ObjectAction.Create;

                int fileId = blinfo.Process(file, new BusinessObjects.CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.JigsawIntegration));

                if (currentTraceSettings.IsTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Filed Id : " + fileId);
                }

                if (fileId > 0)
                {
                    //Write to integration activity log
                    IntegrationActivityLogBL ialogbl = new IntegrationActivityLogBL();

                    //The parameters for this call is provided by Prasad Ballingam and it is believed that it is documented somwehere. 
                    OperationResult result = ialogbl.Create(fileId, "File", "MessageContext", "JigsawIntegrationConnector", "Inbound Jigsaw Message", IntegrationType.Inbound, 0, 0, new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.JigsawIntegration));

                    if (result.OperationResultStatus == OperationResultStatusEnum.Successful || result.OperationResultStatus == OperationResultStatusEnum.None)
                    {
                        if (currentTraceSettings.IsTracingEnabled)
                        {
                            diagnosticActivity.LogInformation("Inserted the json in the IAL");
                        }

                        return 0;
                    }
                    else
                    {
                        diagnosticActivity.LogError(result.ToString());
                    }
                }
            }
            finally
            {
                if (currentTraceSettings.IsTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Processing of EntityOperationRequest Finished");
                    diagnosticActivity.Stop();
                }
            }

            return -1;
        }

        #endregion
    }
}
