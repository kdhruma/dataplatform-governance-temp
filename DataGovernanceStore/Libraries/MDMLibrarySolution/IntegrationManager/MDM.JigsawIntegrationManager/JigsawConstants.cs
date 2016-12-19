using System;
using System.Diagnostics;

namespace MDM.JigsawIntegrationManager
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Integration;
    using MDM.Core;
    using MDM.IntegrationManager.Business;
    using MDM.Utility;

    /// <summary>
    /// Represents class for JigsawConstants.
    /// </summary>
    public class JigsawConstants
    {
        private static String _categoryPathSeparator;
        private static String _tenant;
        private static String _segment;
        private static readonly String _jigsawSeparator = "/";
        private static Boolean? _isJigsawIntegrationEnabled;
		private static Boolean? _isJigsawMatchingEnabled;
		private static Boolean? _isJigsawInformationGovernanceReportsEnabled;
		private static JigsawIntegrationBrokerType? _brokerType;
	    private static String _messageFolder;

        /// <summary>
        /// Indicates the category path separator.
        /// </summary>
        public static String CategoryPathSeparator
        {
            get
            {
                if (String.IsNullOrEmpty(_categoryPathSeparator))
                {
                    _categoryPathSeparator = AppConfigurationHelper.GetAppConfig<String>("Catalog.Category.PathSeparator");

                    if (String.IsNullOrEmpty(_categoryPathSeparator))
                    {
                        _categoryPathSeparator = "»";
                    }
                }

                return _categoryPathSeparator;
            }
        }

        /// <summary>
        /// Indicates the tenant name for the current MDMCenter system.
        /// </summary>
        public static String Tenant
        {
            get
            {
                if (String.IsNullOrEmpty(_tenant))
                {
                    try
                    {
                        ConnectorProfile connectorProfile = new ConnectorProfileBL().GetByName(JigsawIntegrationConnectorName,
                            new CallerContext());
                        if (connectorProfile != null && connectorProfile.JigsawIntegrationConfiguration != null && connectorProfile.JigsawIntegrationConfiguration.TenantConfiguration != null)
                        {
                            _tenant = connectorProfile.JigsawIntegrationConfiguration.TenantConfiguration.Tenant;
                        }
                    }
                    catch (Exception ex)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "The following exception has occurred while trying to get ConnectorProfile:\n" + ex.ToString());
                        _tenant = "t1";
                    }
                }

                return _tenant;
            }
        }

        /// <summary>
        /// Indicates the segment.
        /// </summary>
        public static String Segment
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_segment))
                {
                    try
                    {
                        ConnectorProfile connectorProfile = new ConnectorProfileBL().GetByName(JigsawIntegrationConnectorName, new CallerContext());

                        if (connectorProfile != null && connectorProfile.JigsawIntegrationConfiguration != null && connectorProfile.JigsawIntegrationConfiguration.TenantConfiguration != null)
                        {
                            _segment = connectorProfile.JigsawIntegrationConfiguration.TenantConfiguration.SegmentCommonAttribute;

                            if(String.IsNullOrWhiteSpace(_segment))
                            {
                                _segment = "NONE";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "The following exception has occurred while trying to get ConnectorProfile:\n" + ex.ToString());
                        _segment = "NONE";
                    }
                }

                return _segment;
            }
        }

        /// <summary>
        /// Indicates the jigsaw separator.
        /// </summary>
        public static String JigsawSeparator
        {
            get { return _jigsawSeparator; }
        }

        /// <summary>
        /// Indicates the jigsaw integration connector name.
        /// </summary>
        public static String JigsawIntegrationConnectorName
        {
            get { return "JigsawIntegrationConnector"; }
        }

        /// <summary>
        /// Indicates a flag mentioning if jigsaw integration is enabled.
        /// </summary>
        public static Boolean IsJigsawIntegrationEnabled
        {
            get
            {
                if (!_isJigsawIntegrationEnabled.HasValue)
                {
                    _isJigsawIntegrationEnabled = MDMFeatureConfigHelper.IsMDMFeatureEnabled(MDMCenterApplication.Jigsaw, "JigsawIntegration", "1", false);
                }

                return _isJigsawIntegrationEnabled.Value;
            }
        }

        /// <summary>
        /// Indicates the integration broker type.
        /// </summary>
        public static JigsawIntegrationBrokerType IntegrationBrokerType
        {
            get
            {
                if (!_brokerType.HasValue)
                {
                    _brokerType = JigsawIntegrationBrokerType.KafkaDashNetBroker;

                    String brokerTypeString = AppConfigurationHelper.GetAppConfig<String>("Jigsaw.IntegrationBrokerType");

                    if (!String.IsNullOrEmpty(brokerTypeString))
                    {
                        JigsawIntegrationBrokerType brokerType;

                        if (ValueTypeHelper.EnumTryParse<JigsawIntegrationBrokerType>(brokerTypeString, true, out brokerType))
                        {
                            _brokerType = brokerType;
                        }
                    }
                }

                return _brokerType.Value;
            }
        }

        /// <summary>
        /// When broker is of FileFolderBroker type, message files will be created in this folder.
        /// </summary>
        public static String IntegrationMessageFolder
        {
            get
            {
                if(_messageFolder == null)
                {
                    _messageFolder = AppConfigurationHelper.GetAppConfig<String>("Jigsaw.IntegrationMessageFolder");
                }

                return _messageFolder;
            }
            set
            {
                _messageFolder = value;
            }
        }

		/// <summary>
		/// Indicates a flag mentioning if jigsaw matching is enabled.
		/// </summary>
		public static Boolean IsJigsawMatchingEnabled
		{
			get
			{
				if (!_isJigsawMatchingEnabled.HasValue )
				{
					_isJigsawMatchingEnabled = IsJigsawIntegrationEnabled && MDMFeatureConfigHelper.IsMDMFeatureEnabled(MDMCenterApplication.Jigsaw, "JigsawMatching", "1", false);
				}

				return _isJigsawMatchingEnabled.Value;
			}

		}

        /// <summary>
        /// Indicates a flag mentioning if jigsaw information governance reporting is enabled.
        /// </summary>
        public static Boolean IsJigsawInformationGovernanceReportsEnabled
		{
			get
			{
				if (!_isJigsawInformationGovernanceReportsEnabled.HasValue)
				{
					_isJigsawInformationGovernanceReportsEnabled = IsJigsawIntegrationEnabled && MDMFeatureConfigHelper.IsMDMFeatureEnabled(MDMCenterApplication.Jigsaw, "JigsawInformationGovernanceReports", "1", false);
				}

				return _isJigsawInformationGovernanceReportsEnabled.Value;
			}

		}

        /// <summary>
        /// Indicates the dummy workflow activity.
        /// </summary>
        public static String DummyWorkflowActivityName
        {
            get { return "NOACTIVITY"; }
        }
	}
}