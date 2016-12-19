using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Imports
{
    using MDM.Core;

    /// <summary>
    /// Import Entity metadata mappings
    /// </summary>
    [DataContract]
    public class EntityMetadataMap : ObjectBase
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private ShortNameMap _shortNameMap = new ShortNameMap();

        /// <summary>
        /// 
        /// </summary>
        private LongNameMap _longNameMap = new LongNameMap();

        /// <summary>
        /// 
        /// </summary>
        private ContainerMap _containerMap = new ContainerMap();

        /// <summary>
        /// 
        /// </summary>
        private EntityTypeMap _entityTypeMap = new EntityTypeMap();

        /// <summary>
        /// 
        /// </summary>
        private SourceCategoryMap _sourceCategoryMap = new SourceCategoryMap();

        /// <summary>
        /// 
        /// </summary>
        private TargetCategoryMap _targetCategoryMap = new TargetCategoryMap();

        /// <summary>
        /// 
        /// </summary>
        private HierarchyParentEntityMap _hierarchyParentEntityMap = new HierarchyParentEntityMap();

        /// <summary>
        /// 
        /// </summary>
        private MDLParentEntityMap _mdlParentEntityMap = new MDLParentEntityMap();

        

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public EntityMetadataMap()
            : base()
        {
        }

        /// <summary>
        /// Constructor with XML having values of object. 
        /// </summary>
        /// <param name="valuesAsXml">Indicates XML having xml value</param>
        public EntityMetadataMap(String valuesAsXml)
        {
            LoadEntityMetadataMappings(valuesAsXml);
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
                return "EntityMetadataMap";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public ShortNameMap ShortNameMap
        {
            get { return _shortNameMap; }
            set { _shortNameMap = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public LongNameMap LongNameMap
        {
            get { return _longNameMap; }
            set { _longNameMap = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public ContainerMap ContainerMap
        {
            get { return _containerMap; }
            set { _containerMap = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public EntityTypeMap EntityTypeMap
        {
            get { return _entityTypeMap; }
            set { _entityTypeMap = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public SourceCategoryMap SourceCategoryMap
        {
            get { return _sourceCategoryMap; }
            set { _sourceCategoryMap = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public TargetCategoryMap TargetCategoryMap
        {
            get { return _targetCategoryMap; }
            set { _targetCategoryMap = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public HierarchyParentEntityMap HierarchyParentEntityMap
        {
            get { return _hierarchyParentEntityMap; }
            set { _hierarchyParentEntityMap = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public MDLParentEntityMap MDLParentEntityMap
        {
            get { return _mdlParentEntityMap; }
            set { _mdlParentEntityMap = value; }
        }

        #endregion

        #region Methods

        #region Load Methods

        ///<summary>
        /// Load EntityMetadataMapping object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        public void LoadEntityMetadataMappings(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ShortNameMap")
                        {
                            #region Read ShortNameMap

                            String shortNameMapXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(shortNameMapXml))
                            {
                                ShortNameMap shortNameMap = new ShortNameMap(shortNameMapXml);
                                this.ShortNameMap = shortNameMap;
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "LongNameMap")
                        {
                            #region Read LongNameMap

                            String longNameMapXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(longNameMapXml))
                            {
                                LongNameMap shortNameMap = new LongNameMap(longNameMapXml);
                                this.LongNameMap = shortNameMap;
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ContainerMap")
                        {
                            #region Read ContainerMap

                            String containerMapXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(containerMapXml))
                            {
                                ContainerMap containerMap = new ContainerMap(containerMapXml);
                                this.ContainerMap = containerMap;
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityTypeMap")
                        {
                            #region Read EntityTypeMap

                            String entityTypeMapXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(entityTypeMapXml))
                            {
                                EntityTypeMap entityTypeMap = new EntityTypeMap(entityTypeMapXml);
                                this.EntityTypeMap = entityTypeMap;
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "SourceCategoryMap")
                        {
                            #region Read SourceCategoryMap

                            String sourceCategoryMapXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(sourceCategoryMapXml))
                            {
                                SourceCategoryMap sourceCategoryMap = new SourceCategoryMap(sourceCategoryMapXml);
                                this.SourceCategoryMap = sourceCategoryMap;
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "TargetCategoryMap")
                        {
                            #region Read TargetCategoryMap

                            String targetCategoryMapXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(targetCategoryMapXml))
                            {
                                TargetCategoryMap targetCategoryMap = new TargetCategoryMap(targetCategoryMapXml);
                                this.TargetCategoryMap = targetCategoryMap;
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "HierarchyParentEntityMap")
                        {
                            #region Read HierarchyParentEntityMap

                            String hierarchyParentEntityMapXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(hierarchyParentEntityMapXml))
                            {
                                HierarchyParentEntityMap hierarchyParentEntityMap = new HierarchyParentEntityMap(hierarchyParentEntityMapXml);
                                this.HierarchyParentEntityMap = hierarchyParentEntityMap;
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDLParentEntityMap")
                        {
                            #region Read MDLParentEntityMap

                            String mdlParentEntityMapXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(mdlParentEntityMapXml))
                            {
                                MDLParentEntityMap mdlParentEntityMap = new MDLParentEntityMap(mdlParentEntityMapXml);
                                this.MDLParentEntityMap = mdlParentEntityMap;
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
            String entityMetadataMapXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //EntityMetadataMappings node start
            xmlWriter.WriteStartElement("EntityMetadataMap");

            #region Write EntityMetadataMappings Properties

            #endregion

            #region Write ShortNameMap

            xmlWriter.WriteRaw(this.ShortNameMap.ToXml());

            #endregion

            #region Write LongNameMap

            xmlWriter.WriteRaw(this.LongNameMap.ToXml());

            #endregion

            #region Write ContainerMap

            xmlWriter.WriteRaw(this.ContainerMap.ToXml());

            #endregion

            #region Write EntityTypeMap

            xmlWriter.WriteRaw(this.EntityTypeMap.ToXml());

            #endregion

            #region Write SourceCategoryMap

            xmlWriter.WriteRaw(this.SourceCategoryMap.ToXml());

            #endregion

            #region Write TargetCategoryMap

            xmlWriter.WriteRaw(this.TargetCategoryMap.ToXml());

            #endregion

            #region Write HierarchyParentEntityMap

            xmlWriter.WriteRaw(this.HierarchyParentEntityMap.ToXml());

            #endregion

            #region Write MDLParentEntityMap

            xmlWriter.WriteRaw(this.MDLParentEntityMap.ToXml());

            #endregion

            //EntityMetadataMappings node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            entityMetadataMapXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return entityMetadataMapXml;
        }

        /// <summary>
        /// Get Xml representation of Entity Mappings
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String entityMetadataMapXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                entityMetadataMapXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //EntityMetadataMapping node start
                xmlWriter.WriteStartElement("EntityMetadataMap");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    #region Write EntityMetadataMapping Properties

                    #endregion
                }
                else if (serialization == ObjectSerialization.UIRender)
                {
                    #region Write EntityMetadataMapping Properties

                    #endregion
                }

                #region Write ShortNameMap

                xmlWriter.WriteRaw(this.ShortNameMap.ToXml(serialization));

                #endregion

                #region Write LongNameMap

                xmlWriter.WriteRaw(this.LongNameMap.ToXml(serialization));

                #endregion

                #region Write ContainerMap

                xmlWriter.WriteRaw(this.ContainerMap.ToXml(serialization));

                #endregion

                #region Write EntityTypeMap

                xmlWriter.WriteRaw(this.EntityTypeMap.ToXml(serialization));

                #endregion

                #region Write SourceCategoryMap

                xmlWriter.WriteRaw(this.SourceCategoryMap.ToXml(serialization));

                #endregion

                #region Write TargetCategoryMap

                xmlWriter.WriteRaw(this.TargetCategoryMap.ToXml(serialization));

                #endregion

                #region Write HierarchyParentEntityMap

                xmlWriter.WriteRaw(this.HierarchyParentEntityMap.ToXml(serialization));

                #endregion

                #region Write MDLParentEntityMap

                xmlWriter.WriteRaw(this.MDLParentEntityMap.ToXml(serialization));

                #endregion

                //AttributeMappings node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                entityMetadataMapXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }

            return entityMetadataMapXml;
        }

        #endregion

        #region Private Methods

        #endregion
        
        #endregion

    }
}
