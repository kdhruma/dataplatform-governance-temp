using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies NormalizationResults Collection
    /// </summary>
    [DataContract]
    public class NormalizationResultsCollection : InterfaceContractCollection<INormalizationResult, NormalizationResult>, INormalizationResultsCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting name of main node in Xml for NormalizationResultsCollection
        /// </summary>    
        private const String NodeName = "NormalizationResult";

        /// <summary>
        /// Field for NormalizationProfile Name
        /// </summary>        
        private String _profileName = String.Empty;

        /// <summary>
        /// Field for Simulation Mode
        /// </summary>        
        private Boolean _simulation;

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting NormalizationProfile Name
        /// </summary>        
        [DataMember]
        public String ProfileName
        {
            get { return _profileName; }
            set { _profileName = value; }
        }

        /// <summary>
        /// Property denoting Simulation Mode
        /// </summary>        
        [DataMember]
        public Boolean Simulation
        {
            get { return _simulation; }
            set { _simulation = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the NormalizationResults Collection
        /// </summary>
        public NormalizationResultsCollection()
        { }

        /// <summary>
        /// Initialize NormalizationResults collection from IList
        /// </summary>
        /// <param name="normalizationResultsList">Source items</param>
        /// <param name="outerProfileName"></param>
        /// <param name="simulation"></param>
        public NormalizationResultsCollection(IList<NormalizationResult> normalizationResultsList, string outerProfileName = null, Boolean simulation = false)
        {
            this._items = new Collection<NormalizationResult>(normalizationResultsList);
            _profileName = outerProfileName;
            _simulation = simulation;
        }

        /// <summary>
        /// Initializes a new instance of the NormalizationResults Collection by xml
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public NormalizationResultsCollection(String valuesAsXml)
        {
            LoadNormalizationResultsCollection(valuesAsXml);
        }

        #endregion

        #region Methods

        private void LoadNormalizationResultsCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals(NodeName))
                        {
                            String normalizationResultXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(normalizationResultXml))
                            {
                                NormalizationResult normalizationResult = new NormalizationResult(normalizationResultXml);
                                Add(normalizationResult);
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

    }
}