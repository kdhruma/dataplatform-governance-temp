using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using MDM.Interfaces.Exports;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;

    /// <summary>
    /// Specifies the scope specification object
    /// </summary>
    [DataContract]
    [KnownType(typeof(ExportScopeCollection))]
    [KnownType(typeof(MDMObjectGroupCollection))]
    [KnownType(typeof(ExportSettingCollection))]
    public class ScopeSpecification : MDMObject, IScopeSpecification
    {
        #region Fields
        [DataMember]
        private const String CONTENT_SETTING_NAME = "ContentSetting";
        /// <summary>
        /// Field specifying export scope collection
        /// </summary>
        private ExportScopeCollection _exportScopes = new ExportScopeCollection();

        private EntityTypeMode _entityTypeMode = EntityTypeMode.OnlyRoot;

        /// <summary>
        /// Field specifying mdmobject groups collection for Entity Types/Containers
        /// </summary>
        private MDMObjectGroupCollection _mdmObjectGroups = new MDMObjectGroupCollection();

        private ExportSettingCollection _contentSettings = new ExportSettingCollection(CONTENT_SETTING_NAME);
        #endregion Fields

        #region Properties

        

        #endregion Properties

        #region Properties

        /// <summary>
        /// Property specifies export scope collection
        /// </summary>
        [DataMember]
        public ExportScopeCollection ExportScopes
        {
            get
            {
                return _exportScopes;
            }
            set
            {
                _exportScopes = value;
            }
        }

        /// <summary>
        /// Specifies  the scope entity type mode
        /// </summary>
        [DataMember]
        public EntityTypeMode EntityTypeMode
        {
            get
            {
                return _entityTypeMode;
            }
            set
            {
                _entityTypeMode = value;
            }
        }

        /// <summary>
        /// Property specifies mdmobjectGroups collection
        /// </summary>
        [DataMember]
        public MDMObjectGroupCollection MDMObjectGroups
        {
            get
            {
                return _mdmObjectGroups;
            }
            set
            {
                _mdmObjectGroups = value;
            }
        }

        /// <summary>
        /// Content Settings
        /// </summary>
        [DataMember]
        public ExportSettingCollection ContentSettings
        {
            get
            {
                return _contentSettings;
            }

            private set
            {
                _contentSettings = value;
            }
        }
        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes scope specification object with default parameters
        /// </summary>
        public ScopeSpecification() : base() { }

        /// <summary>
        /// Initializes scope specification object with the values provided as Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml string having values</param>
        /// <param name="objectSerialization">Specifies Enum type of objectSerialization</param>
        public ScopeSpecification(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadScopeSpecification(valuesAsXml, objectSerialization);
        }

        #endregion Constructors

        #region Public Methods
        /// <summary>
        /// Adds a content Setting
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddContentSetting(String name, String value)
        {
            _contentSettings.Add(new ExportSetting(CONTENT_SETTING_NAME, name, value));
        }
        /// <summary>
        /// Represents scope specification in Xml format
        /// </summary>
        /// <returns>String representation of current scope specification object</returns>
        public override String ToXml()
        {
            String scopeSpecificationXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //ScopeSpecification Item node start
            xmlWriter.WriteStartElement("ScopeSpecification");
            xmlWriter.WriteAttributeString("EntityTypeMode", this.EntityTypeMode.ToString());

            #region Write Entity Types

            #endregion

            #region Write MDMObjectTypes 

            if (this.MDMObjectGroups != null)
            {
                xmlWriter.WriteRaw(MDMObjectGroups.ToXml());
            }
            #endregion

            #region Write ExportScopes

            if (this.ExportScopes != null)
            {
                xmlWriter.WriteRaw(this.ExportScopes.ToXml());
            }

            #endregion


            if (this.ContentSettings != null)
            {
                xmlWriter.WriteRaw(this.ContentSettings.ToXml());
            }


            //ScopeSpecification Item node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            scopeSpecificationXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return scopeSpecificationXml;
        }

        /// <summary>
        /// Represents scope specification in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current scope specification object</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            String scopeSpecificationXml = this.ToXml();

            return scopeSpecificationXml;
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
                if (obj is ScopeSpecification)
                {
                    ScopeSpecification objectToBeCompared = obj as ScopeSpecification;

                    if (!this.ExportScopes.Equals(objectToBeCompared.ExportScopes))
                    {
                        return false;
                    }

                    if (!this.ContentSettings.Equals(objectToBeCompared.ContentSettings))
                    {
                        return false;
                    }

                    if (!this.MDMObjectGroups.Equals(objectToBeCompared.MDMObjectGroups))
                    {
                        return false;
                    }

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
            hashCode = base.GetHashCode() ^ this.ExportScopes.GetHashCode() ^ this.ContentSettings.GetHashCode() ^ this.MDMObjectGroups.GetHashCode();

            return hashCode;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the scope specification with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadScopeSpecification(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
                <ScopeSpecification>
		            <ExportScopes>
			            <ExportScope ObjectType="Container" ObjectId="" ObjectUniqueIdentifier="Product Master" Include="" IsRecursive="false">
				            <SearchAttributeRules>
					            <SearchAttributeRule AttributeId="" Operator="" Value="" />
					            <SearchAttributeRule AttributeId="" Operator="" Value="" />
				            </SearchAttributeRules>
				            <ExportScopes>
					            <ExportScope ObjectType="Category" ObjectId="" ObjectUniqueIdentifier="Apparel" Include="" IsRecursive="false">
						            <SearchAttributeRules />
					            </ExportScope>
					            <ExportScope ObjectType="Entity" ObjectId="" ObjectUniqueIdentifier="P1121" Include="" IsRecursive="false">
						            <SearchAttributeRules />
					            </ExportScope>
				            </ExportScopes>
			            </ExportScope>
		            </ExportScopes>
	            </ScopeSpecification>
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
                        #region Read ScopeSpecification

                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExportScopes")
                        {
                            // Read export scopes
                            #region Read export scopes
                            String exportScopesXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(exportScopesXml))
                            {
                                ExportScopeCollection exportScopeCollection = new ExportScopeCollection(exportScopesXml,objectSerialization);
                                if (exportScopeCollection.Any())
                                {
                                    this.ExportScopes = exportScopeCollection;
                                }
                            }
                            #endregion
                        }
                        else if(reader.NodeType == XmlNodeType.Element && reader.Name == "MDMObjectGroups")
                        {
                            String exportMDMObjecGroupsXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(exportMDMObjecGroupsXml))
                            {
                                MDMObjectGroupCollection exportMDMObjectGroupCollection = new MDMObjectGroupCollection(exportMDMObjecGroupsXml,objectSerialization);
                                if (exportMDMObjectGroupCollection.Any())
                                {
                                    this.MDMObjectGroups = exportMDMObjectGroupCollection;
                                }
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == String.Format("{0}s",CONTENT_SETTING_NAME))
                        {
                            String contentSettingsXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(contentSettingsXml))
                            {
                                ExportSettingCollection contentSettingCollection = new ExportSettingCollection(CONTENT_SETTING_NAME, contentSettingsXml, objectSerialization);
                                if (contentSettingCollection.Any())
                                {
                                    this.ContentSettings = contentSettingCollection;
                                }
                            }
                        }

                        else
                        {
                            reader.Read();
                        }

                        #endregion Read ScopeSpecification
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
