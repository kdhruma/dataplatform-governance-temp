using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies configurations for Relationship Denorm Processing Setting
    /// </summary>
    [DataContract]
    public class RelationshipDenormProcessingSettingCollection : InterfaceContractCollection<IRelationshipDenormProcessingSetting, RelationshipDenormProcessingSetting> , IRelationshipDenormProcessingSettingCollection
    {
        #region Fields

        /// <summary>
        /// Field representing collection Relationship Denorm Processing Settings
        /// </summary>
        [DataMember]
        private Collection<RelationshipDenormProcessingSetting> _relationshipDenormProcessingSettings = new Collection<RelationshipDenormProcessingSetting>();

        #endregion Fields

        #region Constructors

		/// <summary>
		/// Parameterless constructor
		/// </summary>
		public RelationshipDenormProcessingSettingCollection() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valueAsXml">Values Which needs to be added in the collection in XMl format</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public RelationshipDenormProcessingSettingCollection(String valueAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadRelationshipDenormProcessingSettingCollection(valueAsXml, objectSerialization);
        }

		/// <summary>
        /// Initialize RelationshipDenormProcessingSettingCollection from IList
		/// </summary>
        /// <param name="relationshipDenormProcessingSettingsList">IList of relationshipDenormProcessingSettings</param>
		public RelationshipDenormProcessingSettingCollection(IList<RelationshipDenormProcessingSetting> relationshipDenormProcessingSettingsList)
		{
			this._relationshipDenormProcessingSettings = new Collection<RelationshipDenormProcessingSetting>(relationshipDenormProcessingSettingsList);
		}

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Get Xml representation of RelationshipDenormProcessingSettingCollection object
        /// </summary>
        /// <returns>Xml string representing the RelationshipDenormProcessingSettingCollection</returns>
        public string ToXml()
        {
            String relationshipDenormProcessingSettingsXml = String.Empty;

            relationshipDenormProcessingSettingsXml = "<RelationshipDenormProcessingSettings>";

            if (this._relationshipDenormProcessingSettings != null && this._relationshipDenormProcessingSettings.Count > 0)
            {
                foreach (RelationshipDenormProcessingSetting relationshipDenormProcessingSetting in this._relationshipDenormProcessingSettings)
                {
                    relationshipDenormProcessingSettingsXml = String.Concat(relationshipDenormProcessingSettingsXml, relationshipDenormProcessingSetting.ToXml());
                }
            }

            relationshipDenormProcessingSettingsXml = String.Concat(relationshipDenormProcessingSettingsXml, "</RelationshipDenormProcessingSettings>");

            return relationshipDenormProcessingSettingsXml;
        }

        /// <summary>
        /// Get Xml representation of RelationshipDenormProcessingSettingCollection object
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>Xml string representing the RelationshipDenormProcessingSettingCollection</returns>
        public string ToXml(ObjectSerialization objectSerialization)
        {
            String relationshipDenormProcessingSettingsXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            relationshipDenormProcessingSettingsXml = this.ToXml();

            return relationshipDenormProcessingSettingsXml;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the RelationshipDenormProcessingSettings from given XMl and add into this
        /// </summary>
        /// <param name="valuesAsXml">Values Which needs to be added in the collection in XMl format</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadRelationshipDenormProcessingSettingCollection(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "RelationshipDenormProcessingSettings")
                        {
                            #region Read RelationshipDenormProcessingSetting Collection

                            String relationshipDenormProcessingSettingXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(relationshipDenormProcessingSettingXml))
                            {
                                RelationshipDenormProcessingSetting relationshipDenormProcessingSetting = new RelationshipDenormProcessingSetting(relationshipDenormProcessingSettingXml, objectSerialization);
                                if (relationshipDenormProcessingSetting != null)
                                {
                                    this.Add(relationshipDenormProcessingSetting);
                                }
                            }

                            #endregion Read RelationshipDenormProcessingSetting Collection
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
