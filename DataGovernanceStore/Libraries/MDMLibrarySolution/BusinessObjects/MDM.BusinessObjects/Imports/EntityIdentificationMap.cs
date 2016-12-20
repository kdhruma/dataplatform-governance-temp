using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Imports
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Import Entity Uniqueness identity specifications
    /// </summary>
    [DataContract]
    public class EntityIdentificationMap : IEntityIdentificationMap
    {
        #region Fields
        
        /// <summary>
        /// 
        /// </summary>
        private MappingCollection _mappings = new MappingCollection();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public EntityIdentificationMap()
            : base()
        {
        }

        /// <summary>
        /// Constructor with XML having values of object. 
        /// </summary>
        /// <param name="valuesAsXml">Indicates XML having xml value</param>
        public EntityIdentificationMap(String valuesAsXml)
        {
            LoadEntityIdentificationMappings(valuesAsXml);
        }
        #endregion

        #region Properties
        
        /// <summary>
        /// Property defining the type of the MDM object
        /// </summary>
        public String ObjectType
        {
            get
            {
                return "EntityIdentificationMap";
            }
        }

        /// <summary>
        /// Property defining the mapping collection
        /// </summary>
        [DataMember]
        public MappingCollection Mappings
        {
            get
            {
                return _mappings;
            }
            set
            {
                _mappings = value;
            }
        }

        #endregion

        #region Methods

        #region Load Methods

        ///<summary>
        /// Load EntityIdentificationMapping object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        public void LoadEntityIdentificationMappings(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Mappings")
                        {
                            #region Read MappingCollection

                            String mappingsXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(mappingsXml))
                            {
                                MappingCollection mappings = new MappingCollection(mappingsXml);
                                this.Mappings = mappings;
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
        /// Get Xml representation of Entity Mappings
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml()
        {
            String entityIdentificationMappingsXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //EntityIdentificationMappings node start
            xmlWriter.WriteStartElement("EntityIdentificationMap");

            #region Write EntityIdentificationMappings Properties

            #endregion

            #region Write MappingCollection

            xmlWriter.WriteStartElement("Mappings");

            foreach (Mapping mapping in this.Mappings)
            {
                xmlWriter.WriteRaw(mapping.ToXml());
            }

            //MappingCollection node end
            xmlWriter.WriteEndElement();

            #endregion

            //EntityIdentificationMappings node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            entityIdentificationMappingsXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return entityIdentificationMappingsXml;
        }

        /// <summary>
        /// Get Xml representation of Entity Mappings
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String entityIdentificationMappingsXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                entityIdentificationMappingsXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //EntityIdentificationMapping node start
                xmlWriter.WriteStartElement("EntityIdentificationMap");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    #region Write EntityIdentificationMapping Properties

                    #endregion
                }
                else if (serialization == ObjectSerialization.UIRender)
                {
                    #region Write EntityIdentificationMapping Properties

                    #endregion
                }

                #region Write MappingCollection

                xmlWriter.WriteStartElement("Mappings");

                if (this.Mappings != null)
                {
                    foreach (Mapping mapping in this.Mappings)
                    {
                        xmlWriter.WriteRaw(mapping.ToXml());
                    }
                }

                //MappingCollection node end
                xmlWriter.WriteEndElement();

                #endregion

                //AttributeMappings node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                entityIdentificationMappingsXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }

            return entityIdentificationMappingsXml;
        }

        #endregion

        #region Private Methods

        #endregion
        
        #endregion

    }
}
