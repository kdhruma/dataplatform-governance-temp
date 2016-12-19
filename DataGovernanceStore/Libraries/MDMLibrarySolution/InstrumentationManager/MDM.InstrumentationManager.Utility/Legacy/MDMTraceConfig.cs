using System;
using System.Xml;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace MDM.Instrumentation.Utility
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies configurations for MDM Trace Source
    /// </summary>
    [DataContract]
    public class MDMTraceConfig
    {
        #region Fields

        /// <summary>
        /// Field representing collection MDM Trace Config Items
        /// </summary>
        private Collection<MDMTraceConfigItem> _mdmTraceConfigItems = new Collection<MDMTraceConfigItem>();

        #endregion

        #region Properties

        /// <summary>
        /// Property representing collection MDM Trace Config Items
        /// </summary>
        [DataMember]
        public Collection<MDMTraceConfigItem> MDMTraceConfigItems
        {
            get
            {
                return _mdmTraceConfigItems;
            }
            set
            {
                _mdmTraceConfigItems = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes MDMTraceConfig object
        /// </summary>
        public MDMTraceConfig()
        {

        }

        /// <summary>
        /// Initializes MDMTraceConfig object with the values provided as Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml string having values</param>
        public MDMTraceConfig(String valuesAsXml)
        {
            LoadMDMTraceConfig(valuesAsXml);
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns the appconfig cache key.
        /// </summary>
        /// <returns></returns>
        public static String GetMDMTraceConfigCacheKey()
        {
            return "MDMTraceConfigCacheKey";
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>True if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is MDMTraceConfig)
            {
                MDMTraceConfig objectToBeCompared = obj as MDMTraceConfig;

                if (this._mdmTraceConfigItems != null && objectToBeCompared.MDMTraceConfigItems != null)
                {
                    Int32 traceConfigItemUnion = this._mdmTraceConfigItems.ToList().Union(objectToBeCompared.MDMTraceConfigItems.ToList()).Count();
                    Int32 traceConfigItemIntersect = this._mdmTraceConfigItems.ToList().Intersect(objectToBeCompared.MDMTraceConfigItems.ToList()).Count();

                    if (traceConfigItemUnion != traceConfigItemIntersect)
                        return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            Int32 hashCode = 0;

            if (this._mdmTraceConfigItems != null && this._mdmTraceConfigItems.Count > 0)
            {
                foreach (MDMTraceConfigItem traceConfigItem in this._mdmTraceConfigItems)
                {
                    hashCode += traceConfigItem.GetHashCode();
                }
            }

            return hashCode;
        }

        /// <summary>
        /// Get Xml representation of MDM Trace Config
        /// </summary>
        /// <returns>Xml representation of MDM Trace Config</returns>
        public String ToXml()
        {
            String mdmTraceConfigXml = String.Empty;

            mdmTraceConfigXml = "<MDMTraceConfig>";

            if (this._mdmTraceConfigItems != null)
            {
                mdmTraceConfigXml = String.Concat(mdmTraceConfigXml, "<MDMTraceConfigItems>");

                foreach (MDMTraceConfigItem configItem in this._mdmTraceConfigItems)
                {
                    mdmTraceConfigXml = String.Concat(mdmTraceConfigXml, configItem.ToXml());
                }

                mdmTraceConfigXml = String.Concat(mdmTraceConfigXml, "</MDMTraceConfigItems>");
            }

            mdmTraceConfigXml = String.Concat(mdmTraceConfigXml, "</MDMTraceConfig>");

            return mdmTraceConfigXml;
        }

        /// <summary>
        /// Get Xml representation of MDM Trace Config based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of MDM Trace Config</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String mdmTraceConfigItemXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            mdmTraceConfigItemXml = this.ToXml();

            return mdmTraceConfigItemXml;
        }

        #region MDM Trace Config Items methods

        /// <summary>
        /// Gets MDM Trace Config Items
        /// </summary>
        /// <returns>MDM Trace Config Items</returns>
        public Collection<IMDMTraceConfigItem> GetMDMTraceConfigItems()
        {
            Collection<IMDMTraceConfigItem> iMDMTraceConfigItems = new Collection<IMDMTraceConfigItem>();

            if (this._mdmTraceConfigItems != null && this._mdmTraceConfigItems.Count > 0)
            {
                foreach (MDMTraceConfigItem mdmTraceConfigItem in this._mdmTraceConfigItems)
                {
                    iMDMTraceConfigItems.Add(mdmTraceConfigItem);
                }
            }

            return iMDMTraceConfigItems;
        }

        /// <summary>
        /// Sets MDM Trace Config Items
        /// </summary>
        /// <param name="iMDMTraceConfigItems">MDM Trace Config Items to be set</param>
        public void SetMDMTraceConfigItems(Collection<IMDMTraceConfigItem> iMDMTraceConfigItems)
        {
            Collection<MDMTraceConfigItem> mdmTraceConfigItems = new Collection<MDMTraceConfigItem>();

            if (iMDMTraceConfigItems != null && iMDMTraceConfigItems.Count > 0)
            {
                foreach (MDMTraceConfigItem mdmTraceConfigItem in iMDMTraceConfigItems)
                {
                    mdmTraceConfigItems.Add(mdmTraceConfigItem);
                }
            }

            this._mdmTraceConfigItems = mdmTraceConfigItems;
        }

        #endregion

        #endregion

        #region Private Methods

        private void LoadMDMTraceConfig(String valuesAsXml)
        {
            #region Sample Xml

            /*
             *  <MDMTraceConfig>
             *      <MDMTraceConfigItems>
             *          <MDMTraceConfigItem TraceSource="Application" LogActivityTrace="true" LogError="true" LogWarning="true" LogInformation="false" LogVerbose="true" />
             *      </MDMTraceConfigItems>
             *  </MDMTraceConfig>
             */

            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMTraceConfigItem")
                        {
                            String traceConfigItemXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(traceConfigItemXml))
                            {
                                MDMTraceConfigItem traceConfigItem = new MDMTraceConfigItem(traceConfigItemXml);

                                if (traceConfigItem != null)
                                {
                                    this._mdmTraceConfigItems.Add(traceConfigItem);
                                }
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

        #endregion

        #endregion
    }
}
