using System;
using System.IO;
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
    public class RelationshipDenormProcessingSetting : MDMObject, IRelationshipDenormProcessingSetting
    {
        #region Fields

        /// <summary>
        /// Field representing Relationship Denorm Processing SettingContext
        /// </summary>
        private RelationshipDenormProcessingSettingContext _relationshipDenormProcessingSettingContext = new RelationshipDenormProcessingSettingContext();

        /// <summary>
        /// Field representing collection Relationship Denorm Processing Settings
        /// </summary>
        private RelationshipDenormActionCollection _relationshipDenormActions = new RelationshipDenormActionCollection();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Specifies relationshipprocessing settings context
        /// </summary>
        [DataMember]
        public RelationshipDenormProcessingSettingContext SettingContext
        {
            get
            {
                return _relationshipDenormProcessingSettingContext;
            }
            set
            {
                _relationshipDenormProcessingSettingContext = value;
            }
        }

        /// <summary>
        /// Specifies relationship denorm actions
        /// </summary>
        [DataMember]
        public RelationshipDenormActionCollection RelationshipDenormActions
        {
            get
            {
                return _relationshipDenormActions;
            }
            set
            {
                _relationshipDenormActions = value;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes RelationshipDenormProcessingSetting object with default parameters
        /// </summary>
        public RelationshipDenormProcessingSetting() : base() { }

        /// <summary>
        /// Initializes RelationshipDenormProcessingSetting object with the values provided as Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml string having values</param>
        /// <param name="objectSerialization">Indicates the options specifying which xml format to be generated</param>
        public RelationshipDenormProcessingSetting(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadRelationshipDenormProcessingSetting(valuesAsXml, objectSerialization);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Get Xml representation of RelationshipDenormProcessingSetting object
        /// </summary>
        /// <returns>Xml string representing the RelationshipDenormProcessingSetting</returns>
        public override String ToXml()
        {
            String relationshipDenormProcessingSettingXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //MDM Trace Config Item node start
            xmlWriter.WriteStartElement("RelationshipDenormProcessingSetting");

            #region write relationship Denorm Processing Setting for Full RelationshipDenormProcessingSetting Xml

            if (this.SettingContext != null)
                xmlWriter.WriteRaw(this.SettingContext.ToXml());

            #endregion write relationship Denorm Processing Setting for Full RelationshipDenormProcessingSetting Xml

            #region write relationship Denorm Action for Full RelationshipDenormAction Xml

            if (this.RelationshipDenormActions != null)
                xmlWriter.WriteRaw(this.RelationshipDenormActions.ToXml());

            #endregion write relationship Denorm Action for Full RelationshipDenormAction Xml

            //MDM Trace Config Item node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            relationshipDenormProcessingSettingXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return relationshipDenormProcessingSettingXml;
        }

        /// <summary>
        /// Get Xml representation of RelationshipDenormProcessingSetting object
        /// </summary>
        /// <param name="objectSerialization">objectSerialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml string representing the RelationshipDenormProcessingSetting</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            String relationshipDenormProcessingSettingXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            relationshipDenormProcessingSettingXml = this.ToXml();

            return relationshipDenormProcessingSettingXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is RelationshipDenormProcessingSetting)
                {
                    RelationshipDenormProcessingSetting objectToBeCompared = obj as RelationshipDenormProcessingSetting;

                    if (!this.SettingContext.Equals(objectToBeCompared.SettingContext))
                        return false;

                    if (!this.RelationshipDenormActions.Equals(objectToBeCompared.RelationshipDenormActions))
                        return false;

                    return true;
                }
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
            hashCode = base.GetHashCode() ^ this.Locale.GetHashCode() ^ this.Action.GetHashCode() ^ this.SettingContext.GetHashCode()
                        ^ this.RelationshipDenormActions.GetHashCode();

            return hashCode;
        }

        #endregion Public Methods

        #region Private Methods

        ///<summary>
        /// Load RelationshipDenormProcessingSetting object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <param name="objectSerialization">>Specifies the object serialization type</param>
        /// <example>
        ///     Sample XML:
        ///    <RelationshipDenormProcessingSetting>
        ///     <SettingContext OrganizationIds="1,2" ContainerIds="1,2"  EntityTypeIds="1,2" CategoryIds="10,15" />
        ///      <RelationshipDenormActions>
        ///             <RelationshipDenormAction Action="RelationshipDelete" ExtensionProcessingMode="ASync" HierarchyProcessingMode="ASync" WhereUsedProcessingMode="ASync" RelationshipTreeProcessingMode="ASync" />
        ///             <RelationshipDenormAction Action="RelationshipAttributeUpdate" ExtensionProcessingMode="ASync" HierarchyProcessingMode="ASync" WhereUsedProcessingMode="ASync" RelationshipTreeProcessingMode="ASync" />
        ///      </RelationshipDenormActions>
        ///    </RelationshipDenormProcessingSetting>
        /// </example>
        private void LoadRelationshipDenormProcessingSetting(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
             * <RelationshipDenormProcessingSetting>
             *      <SettingContext OrganizationIds="[RSAll]" ContainerIds="[RSAll]"  EntityTypeIds="[RSAll]" CategoryIds="[RSAll]" />
             *      <RelationshipDenormActions>
             *          <RelationshipDenormAction Action="RelationshipCreate" ExtensionProcessingMode="ASync" HierarchyProcessingMode="ASync" WhereUsedProcessingMode="ASync" RelationshipTreeProcessingMode="ASync" />
             *          <RelationshipDenormAction Action="RelationshipUpdate" ExtensionProcessingMode="ASync" HierarchyProcessingMode="ASync" WhereUsedProcessingMode="ASync" RelationshipTreeProcessingMode="ASync" />
             *      </RelationshipDenormActions>
             * </RelationshipDenormProcessingSetting>
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
                        #region Read RelationshipDenormProcessingSetting

                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "SettingContext")
                        {
                            // Read setting context
                            #region Read RelationshipDenormProcessingSettingContext

                            String relationshipDenormProcessingSettingContextXml = reader.ReadOuterXml();
                            
                            if (!String.IsNullOrEmpty(relationshipDenormProcessingSettingContextXml))
                            {
                                this.SettingContext = new RelationshipDenormProcessingSettingContext(relationshipDenormProcessingSettingContextXml);
                            }
                            
                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "RelationshipDenormActions")
                        {
                            // Read relationshipDenormActions
                            #region Read RelationshipDenormActions
                            
                            String relationshipDenormActionXml = reader.ReadOuterXml();
                            
                            if (!String.IsNullOrEmpty(relationshipDenormActionXml))
                            {
                                RelationshipDenormActionCollection relationshipDenormActionCollection = new RelationshipDenormActionCollection(relationshipDenormActionXml);
                                
                                if (relationshipDenormActionCollection != null)
                                {
                                    foreach (RelationshipDenormAction relationshipDenormAction in relationshipDenormActionCollection)
                                    {
                                        if (!this.RelationshipDenormActions.Contains(relationshipDenormAction))
                                        {
                                            this.RelationshipDenormActions.Add(relationshipDenormAction);
                                        }
                                    }
                                }
                            }
                            
                            #endregion Read RelationshipDenormActions
                        }
                        else
                        {
                            reader.Read();
                        }

                        #endregion
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
