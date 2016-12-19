using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Imports
{
    using MDM.Core;

    /// <summary>
    /// Import mapping specifications
    /// </summary>
    [DataContract]
    public class MappingSpecifications : ObjectBase
    {
        #region Fields

        /// <summary>
        /// Field denoting entity identification map
        /// </summary>
        private EntityIdentificationMap _entityIdentificationMap = new EntityIdentificationMap();

        /// <summary>
        /// Field denoting entity metadata map
        /// </summary>
        private EntityMetadataMap _entityMetadataMap = new EntityMetadataMap();

        /// <summary>
        /// Field denoting attribute maps
        /// </summary>
        private AttributeMapCollection _attributeMaps = new AttributeMapCollection();

        /// <summary>
        /// 
        /// </summary>
        private LocaleMap _localeMap = new LocaleMap();

        /// <summary>
        /// Field denoting RelationshipTypeMaps
        /// </summary>
        private RelationshipTypeMaps _RelationshipTypeMaps = new RelationshipTypeMaps();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public MappingSpecifications()
            : base()
        {
        }

        /// <summary>
        /// Constructor with XML having values of object. 
        /// </summary>
        /// <param name="valuesAsXml">Indicates XML having xml value</param>
        public MappingSpecifications(String valuesAsXml)
        {
            LoadMappingSpecifications(valuesAsXml);
        }
        #endregion

        #region Properties
        
        /// <summary>
        /// Property defining the type of the MDM object
        /// </summary>
        public new String ObjectType
        {
            get
            {
                return "MappingSpecifications";
            }
        }

        /// <summary>
        /// Property defining entity identification map
        /// </summary>
        [DataMember]
        public EntityIdentificationMap EntityIdentificationMap
        {
            get
            {
                return _entityIdentificationMap;
            }
            set
            {
                _entityIdentificationMap = value;
            }
        }

        /// <summary>
        /// Property defining entity metadata mappings
        /// </summary>
        [DataMember]
        public EntityMetadataMap EntityMetadataMap
        {
            get
            {
                return _entityMetadataMap;
            }
            set
            {
                _entityMetadataMap = value;
            }
        }

        /// <summary>
        /// Property defining attribute maps
        /// </summary>
        [DataMember]
        public AttributeMapCollection AttributeMaps
        {
            get
            {
                return _attributeMaps;
            }
            set
            {
                _attributeMaps = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public LocaleMap LocaleMap
        {
            get { return _localeMap; }
            set { _localeMap = value; }
        }

        /// <summary>
        /// Property defining RelationshipTypeMaps
        /// </summary>
        [DataMember]
        public RelationshipTypeMaps RelationshipTypeMaps
        {
            get { return _RelationshipTypeMaps; }
            set { _RelationshipTypeMaps = value; }
        }

        #endregion

        #region Methods

        #region Load Methods

        ///<summary>
        /// Load MappingSpecifications object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        public void LoadMappingSpecifications(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityIdentificationMap")
                        {
                            #region Read EntityIdentificationMap

                            String entityIdentificationMapXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(entityIdentificationMapXml))
                            {
                                EntityIdentificationMap entityIdentificationMap = new EntityIdentificationMap(entityIdentificationMapXml);
                                this.EntityIdentificationMap = entityIdentificationMap;
                            }

                            #endregion
                        }
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityMetadataMap")
                        {
                            #region Read EntityMetadataMap

                            String entityMetadataMapXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(entityMetadataMapXml))
                            {
                                EntityMetadataMap entityMetadataMap = new EntityMetadataMap(entityMetadataMapXml);
                                this.EntityMetadataMap = entityMetadataMap;
                            }

                            #endregion
                        }
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeMaps")
                        {
                            #region Read AttributeMaps

                            String attributeMapsXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(attributeMapsXml))
                            {
                                AttributeMapCollection attributeMaps = new AttributeMapCollection(attributeMapsXml);
                                this.AttributeMaps = attributeMaps;
                            }

                            #endregion
                        }
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "RelationshipTypeMaps")
                        {
                            #region Read RelationshipTypeMaps

                            String RelationshipTypeMapsXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(RelationshipTypeMapsXml))
                            {
                                RelationshipTypeMaps relationshipTypeMaps = new RelationshipTypeMaps(RelationshipTypeMapsXml);
                                this.RelationshipTypeMaps = relationshipTypeMaps;
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "LocaleMap")
                        {
                            #region Read LocaleMap

                            String localeMapXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(localeMapXml))
                            {
                                LocaleMap localeMap = new LocaleMap(localeMapXml);
                                this.LocaleMap = localeMap;
                            }

                            #endregion
                        }
                        else
                        {
                            //Keep on reading the xml until we reach expected node.
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

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of MappingSpecifications
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml()
        {
            String mappingSpecificationsXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //MappingSpecifications node start
            xmlWriter.WriteStartElement("MappingSpecifications");

            #region Write EntityIdentificationMap

            if (this.EntityIdentificationMap != null)
            {
                xmlWriter.WriteRaw(this.EntityIdentificationMap.ToXml());
            }

            #endregion

            #region Write EntityMetadataMap

            if (this.EntityMetadataMap != null)
            {
                xmlWriter.WriteRaw(this.EntityMetadataMap.ToXml());
            }

            #endregion

            #region Write AttributeMaps

            if (this.AttributeMaps != null)
            {
                xmlWriter.WriteRaw(this.AttributeMaps.ToXml());
            }
            #endregion

            #region Write RelationshipTypeMaps

            if (this.RelationshipTypeMaps != null)
            {
                xmlWriter.WriteRaw(this.RelationshipTypeMaps.ToXml());
            }

            #endregion

            #region Write LocaleMap

            xmlWriter.WriteRaw(this.LocaleMap.ToXml());

            #endregion

            //MappingSpecifications node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            mappingSpecificationsXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return mappingSpecificationsXml;
        }

        /// <summary>
        /// Get Xml representation of MappingSpecifications
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String mappingSpecificationsXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                mappingSpecificationsXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //MappingSpecifications node start
                xmlWriter.WriteStartElement("MappingSpecifications");

                #region Write EntityIdentificationMap

                if (this.EntityIdentificationMap != null)
                {
                    xmlWriter.WriteRaw(this.EntityIdentificationMap.ToXml());
                }

                #endregion

                #region Write EntityMetadataMap

                if (this.EntityMetadataMap != null)
                {
                    xmlWriter.WriteRaw(this.EntityMetadataMap.ToXml());
                }

                #endregion

                #region Write AttributeMaps

                if (this.AttributeMaps != null)
                {
                    xmlWriter.WriteRaw(this.AttributeMaps.ToXml());
                }

                #endregion

                #region Write RelationshipTypeMaps

                if (this.RelationshipTypeMaps != null)
                {
                    xmlWriter.WriteRaw(this.RelationshipTypeMaps.ToXml());
                }

                #endregion

                #region Write LocaleMap

                xmlWriter.WriteRaw(this.LocaleMap.ToXml(serialization));

                #endregion

                //MappingSpecifications node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                mappingSpecificationsXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            return mappingSpecificationsXml;
        }

        #endregion

        #region Private Methods

        #endregion
        
        #endregion
    }
}
