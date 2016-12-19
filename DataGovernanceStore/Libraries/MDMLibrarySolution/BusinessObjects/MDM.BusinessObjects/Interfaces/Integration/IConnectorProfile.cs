using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the connector profile.
    /// </summary>
    public interface IConnectorProfile : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Indicates Id of connector
        /// </summary>
        new Int16 Id { get; set; }

        /// <summary>
        /// Indicates weightage for the connector
        /// </summary>
        Int32 Weightage { get; set; }

        /// <summary>
        /// Indicates if connector is enabled
        /// </summary>
        Boolean Enabled { get; set; }

        /// <summary>
        /// Indicates default IntegrationMessageType (ShortName) to be used for creating ActivityLog through FileWatcher.
        /// </summary>
        String DefaultInboundIntegrationMessageTypeName { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents ConnectorProfile in Xml format
        /// </summary>
        String ToXml();

        /// <summary>
        /// Clone the ConnectorProfile object and return new instance with the same values.
        /// </summary>
        /// <returns>New cloned IConnectorProfile</returns>
        IConnectorProfile Clone();

        /// <summary>
        /// Get run time specification for connector
        /// </summary>
        /// <returns><see cref="IRunTimeSpecifications"/></returns>
        IRunTimeSpecifications GetRunTimeSpecifications();

        /// <summary>
        /// Get qualification options for connector
        /// </summary>
        /// <returns>Returns QualificationConfiguration object</returns>
        IQualificationConfiguration GetQualificationConfiguration();
        
        /// <summary>
        /// Get aggregation configuration options for connector
        /// </summary>
        /// <returns><see cref="IAggregationConfiguration"/></returns>
        IAggregationConfiguration GetAggregationConfiguration();

        /// <summary>
        /// Get processing configuration options for connector
        /// </summary>
        /// <returns><see cref="IProcessingConfiguration"/></returns>
        IProcessingConfiguration GetProcessingConfiguration();

        /// <summary>
        /// Get additional configuration options for connector
        /// </summary>
        /// <returns>collection of additional configuration key and value</returns>
        Collection<KeyValuePair<String, String>> GetAdditionalConfiguration();

        /// <summary>
        /// Add additional configuration values.
        /// Key must be unique.
        /// </summary>
        void AddAdditionalConfiguration(String key, String value);

        /// <summary>
        /// Get AdditionalConfiguration value based on key
        /// </summary>
        /// <param name="key">Key of AdditionalConfiguration to search on</param>
        /// <returns>AdditionalConfiguration key-value pair having specified key</returns>
        KeyValuePair<String, String> GetAdditionalConfigurationByKey(String key);

        #endregion Methods
    }
}