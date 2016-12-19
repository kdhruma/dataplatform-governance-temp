using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace MDM.JigsawIntegrationManager.MessageProducers
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.Core;
    using MDM.Core.Extensions;
    using MDM.JigsawIntegrationManager.DataPackages;
    using MDM.JigsawIntegrationManager.DTO;
    using MDM.Utility;

    /// <summary>
    /// 
    /// </summary>
    internal class AppConfigManageMessageProducer10
    {
        #region Properties

        private const String _appEntityType = "tenantConfig";

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<IMessageBase> GenerateAppManageMessage(List<AppConfigManageDataPackage> appDataPackages, CallerContext callerContext)
        {
            #region Diagnostics Initialization

            DiagnosticActivity diagnosticActivity = null;

            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            if (currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = new DiagnosticActivity();
                diagnosticActivity.Start();
            }

            #endregion

            var appMessages = new List<IMessageBase>();

            try
            {
                if (!appDataPackages.IsNullOrEmpty())
                {
                    var defaultCulture = GlobalizationHelper.GetSystemDataLocale().GetCultureName();

                    foreach (var appDataPackage in appDataPackages)
                    {
                        var appMessage = new AppConfigManageMessage();

                        appMessage.Eid = JigsawConstants.Tenant;

                        #region Entity info and system info

                        appMessage.EntityInfo = new DTO.EntityInfo { DefaultLocale = defaultCulture, EntityType = _appEntityType };

                        appMessage.SystemInfo = new DTO.SystemInfo { TenantId = "jigsaw" };

                        #endregion

                        #region attributes info

                        var attributesInfo = new AttributesInfo();

                        if (appDataPackage.MessageData != null)
                        {
                            attributesInfo.Attributes = new List<DTO.Attribute>() {
                                new DTO.Attribute() {
                                    Name = appDataPackage.Name.ToJigsawString(),
                                    Value = appDataPackage.MessageData } };

                            appMessage.AttributesInfo = attributesInfo;
                        }

                        #endregion

                        #region extended attributes info

                        var extendedAttributesInfo = new EventExtendedAttributesInfo();

                        extendedAttributesInfo.JsChangeContext = ProducerHelper.CreateChangeContext(callerContext);

                        appMessage.ExtendedAttributesInfo = extendedAttributesInfo;

                        #endregion

                        appMessages.Add((IMessageBase)appMessage);
                    }
                }
            }
            finally
            {
                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return appMessages;
        }
    }
}
