using System;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    using System.Collections.Generic;

    /// <summary>
    /// A collection of MDM Feature Config items.
    /// </summary>
    [DataContract]
    public class MDMFeatureConfigCollection : InterfaceContractCollection<IMDMFeatureConfig, MDMFeatureConfig>, ICollection<MDMFeatureConfig>, IEnumerable<MDMFeatureConfig>, IMDMFeatureConfigCollection    
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MDMFeatureConfig Collection
        /// </summary>
        public MDMFeatureConfigCollection()
        { }

        /// <summary>
        /// Initialize MDMFeatureConfig collection from Xml value
        /// </summary>
        /// <param name="valuesAsXml">MDMFeatureConfig collection in xml format</param>
        public MDMFeatureConfigCollection(String valuesAsXml)
        {
            LoadMDMFeatureConfigCollection(valuesAsXml);
        }

        #endregion

        #region IMDMFeatureConfigCollection implementation

        /// <summary>
        /// Get Xml representation of IMDMFeatureConfigCollection
        /// </summary>
        /// <returns>
        /// Xml representation of IMDMFeatureConfigCollection
        /// </returns>
        public String ToXml()
        {
            StringBuilder result = new StringBuilder("<MDMFeatureConfigs>");

            foreach (MDMFeatureConfig config in _items)
            {
                result.Append(config.ToXml());
            }
            result.Append("</MDMFeatureConfigs>");

            return result.ToString();
        }

        /// <summary>
        /// Get Xml representation of IMDMFeatureConfigCollection
        /// </summary>
        /// <param name="serialization">The serialization options.</param>
        /// <returns>
        /// Xml representation of IMDMFeatureConfigCollection
        /// </returns>
        public String ToXml(ObjectSerialization serialization)
        {
            StringBuilder result = new StringBuilder("<MDMFeatureConfigs>");

            foreach (MDMFeatureConfig config in _items)
            {
                result.Append(config.ToXml(serialization));
            }
            result.Append("</MDMFeatureConfigs>");

            return result.ToString();
        }

       
        /// <summary>
        /// Add MDMFeatureConfig in collection
        /// </summary>
        /// <param name="iMDMFeatureConfigs">MDMFeatureConfig to add in collection</param>
        public void AddRange(IMDMFeatureConfigCollection iMDMFeatureConfigs)
        {
            if (iMDMFeatureConfigs == null)
            {
                throw new ArgumentNullException("MDMFeatureConfigs");
            }

            foreach (MDMFeatureConfig MDMFeatureConfig in iMDMFeatureConfigs)
            {
                this.Add(MDMFeatureConfig);
            }
        }       

        #endregion IMDMFeatureConfigCollection implementation

        #region Public Methods

        /// <summary>
        /// Gets MDM Feature Config by application, module name and version
        /// </summary>        
        /// <param name="application">Indicates the Application for requested feature config</param>
        /// <param name="moduleName">Indicates module short name for requested feature config</param>             
        /// <param name="version">Indicates the version  for requested feature config</param>   
        /// <returns>Returns MDM Feature Config</returns>
        public MDMFeatureConfig GetFeatureConfig(MDMCenterApplication application, String moduleName, String version)
        {            
            foreach (MDMFeatureConfig featureConfigResult in this)
            {
                if (featureConfigResult.Application == application && featureConfigResult.ModuleName == moduleName && featureConfigResult.Version == version)
                {
                    return featureConfigResult;                    
                }
            }
            return null;
        }

        /// <summary>
        /// Compare MDMFeatureConfigCollection
        /// </summary>
        /// <param name="subsetMDMFeatureConfigCollection">Expected MDMFeatureConfigCollection</param>
        /// <returns>If acutual MDMFeatureConfigCollection is match then true else false</returns>
        public Boolean IsSuperSetOf(MDMFeatureConfigCollection subsetMDMFeatureConfigCollection)
        {
            if (subsetMDMFeatureConfigCollection != null && subsetMDMFeatureConfigCollection.Count > 0)
            {
                foreach (MDMFeatureConfig subsetMDMFeatureConfigResult in subsetMDMFeatureConfigCollection)
                {
                    MDMFeatureConfig mdmFeatureConfig = this.GetFeatureConfig(subsetMDMFeatureConfigResult.Application, subsetMDMFeatureConfigResult.ModuleName, subsetMDMFeatureConfigResult.Version);
                    if (mdmFeatureConfig == null)
                    {
                        return false;
                    }
                    else if (!mdmFeatureConfig.IsSuperSetOf(subsetMDMFeatureConfigResult))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadMDMFeatureConfigCollection(String valuesAsXml)
        {
         
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMFeatureConfig")
                        {
                            String mdmFeatureConfigXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(mdmFeatureConfigXml))
                            {
                                Add(new MDMFeatureConfig(mdmFeatureConfigXml));
                            }
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }

        #endregion Private Methods
    }
}
