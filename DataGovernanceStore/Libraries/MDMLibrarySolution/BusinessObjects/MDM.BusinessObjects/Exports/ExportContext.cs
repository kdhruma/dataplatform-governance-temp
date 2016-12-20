using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using MDM.Core;
using System.Xml;

namespace MDM.BusinessObjects.Exports
{
    /// <summary>
    /// Specifies the Export Context
    /// </summary>
    [DataContract]
    public class ExportContext : MDMObject
    {
        #region Fields
        
        /// <summary>
        /// Field denoting the attribute id list.
        /// </summary>
        private Collection<Int32> _attributeIdList;

        /// <summary>
        /// Field denoting the entity id list.
        /// </summary>
        private Collection<Int64> _entityIdList;

        /// <summary>
        /// Field denoting the locale id list.
        /// </summary>
        private Collection<LocaleEnum> _localeIdList;

        /// <summary>
        /// Field denoting the relationship id list.
        /// </summary>
        private Collection<Int64> _relationshipIdList;

        /// <summary>
        /// Field denoting to whether include all common attributes or not.
        /// </summary>
        private Boolean _includeAllCommonAttributeIds = false;

        /// <summary>
        /// Field denoting to whether include all category attributes or not.
        /// </summary>
        private Boolean _includeAllCategoryAttributeIds = false;

        /// <summary>
        /// Field denoting to whether include all relationship types or not.
        /// </summary>
        private Boolean _includeAllRelationshipTypeIds = false;

        /// <summary>
        /// Field denoting the relationship type Id list
        /// </summary>
        private Collection<Int32> _relationshipTypeIdList = new Collection<Int32>();

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ExportContext()
        {

        }

        /// <summary>
        /// Constructor with Values as xml format
        /// </summary>
        /// <param name="valuesAsXml">Xml formatted values with which object will be initalized.</param>
        public ExportContext(String valuesAsXml)
        {
            LoadExportContext(valuesAsXml);
        }

        #endregion Constructors
        
        #region Properties
        
        /// <summary>
        /// Collection of attribute ids that should be used during export
        /// </summary>
        [DataMember]
        public Collection<Int32> AttributeIdList
        {
            get
            {
                return _attributeIdList ?? (_attributeIdList = new Collection<Int32>());
            }
            set
            {
                _attributeIdList = value;
            }
        }

        /// <summary>
        /// Collection of entity ids that should be used during export
        /// </summary>
        [DataMember]
        public Collection<Int64> EntityIdList
        {
            get
            {
                return _entityIdList ?? (_entityIdList = new Collection<Int64>());
            }
            set
            {
                _entityIdList = value;
            }
        }

        /// <summary>
        /// Collection of locale ids that should be used during export
        /// </summary>
        [DataMember]
        public Collection<LocaleEnum> LocaleIdList
        {
            get
            {
                return _localeIdList ?? (_localeIdList = new Collection<LocaleEnum>());
            }
            set
            {
                _localeIdList = value;
            }
        }
        
        /// <summary>
        /// Collection of relationship entities ids that should be used during export
        /// </summary>
        [DataMember]
        public Collection<Int64> RelationshipIdList
        {
            get
            {
                return _relationshipIdList ?? (_relationshipIdList = new Collection<Int64>());
            }
            set
            {
                _relationshipIdList = value;
            }
        }

        /// <summary>
        /// Page which is the export invoker
        /// </summary>
        [DataMember]
        public PageSource PageSource { get; set; }

        /// <summary>
        /// Specifies type of current export
        /// </summary>
        [DataMember]
        public ExportType ExportType { get; set; }
        
        /// <summary>
        /// Specifies search criteria for export context.
        /// </summary>
        [DataMember]
        public SearchCriteria SearchCriteria { get; set; }
        
        /// <summary>
        /// Specifies Catalog Id for category export.
        /// </summary>
        [DataMember]
        public Int32 CatalogId { get; set; }

        /// <summary>
        /// Specifies whether all common attributes are included or not.
        /// </summary>
        [DataMember]
        public Boolean IncludeAllCommonAttributeIds
        {
            get
            {
                return _includeAllCommonAttributeIds;
            }
            set
            {
                _includeAllCommonAttributeIds = value;
            }
        }

        /// <summary>
        /// Specifies whether all category attributes are included or not.
        /// </summary>
        [DataMember]
        public Boolean IncludeAllCategoryAttributeIds 
        {
            get
            {
                return _includeAllCategoryAttributeIds;
            }
            set
            {
                _includeAllCategoryAttributeIds = value;
            }
        }


        /// <summary>
        /// Specifies whether all relationship types are included or not.
        /// </summary>
        [DataMember]
        public Boolean IncludeAllRelationshipTypeIds
        {
            get
            {
                return _includeAllRelationshipTypeIds;
            }
            set
            {
                _includeAllRelationshipTypeIds = value;
            }
        }

        /// <summary>
        /// Property denoting the relationship type id list
        /// </summary>
        [DataMember]
        public Collection<Int32> RelationshipTypeIdList
        {
            get
            {
                return _relationshipTypeIdList ?? (_relationshipTypeIdList = new Collection<Int32>());
            }
            set
            {
                _relationshipTypeIdList = value;
            }
        }
        
        #endregion

        #region Methods

        /// <summary>
        /// Read the input xml and set the values to attribute model context
        /// </summary>
        /// <param name="valuesAsXml">Attribute Model Context as xml</param>
        private void LoadExportContext(String valuesAsXml)
        {
            #region Sample Xml

            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExportContext")
                    {
                        #region Read EntityContext Properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("AttributeIdList"))
                            {
                                this.AttributeIdList = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                            }

                            if (reader.MoveToAttribute("EntityIdList"))
                            {
                                this.EntityIdList = ValueTypeHelper.SplitStringToLongCollection(reader.ReadContentAsString(), ',');
                            }

                            if (reader.MoveToAttribute("LocaleIdList"))
                            {
                                this.LocaleIdList = ValueTypeHelper.SplitStringToLocaleEnumCollection(reader.ReadContentAsString(), ',');
                            }

                            //if (reader.MoveToAttribute("RelationshipIdList"))
                            //{
                            //    this.RelationshipIdList = ValueTypeHelper.SplitStringToLongCollection(reader.ReadContentAsString(), ',');
                            //}

                            if (reader.MoveToAttribute("PageSource"))
                            {
                                PageSource tempPageSource = BusinessObjects.PageSource.Unknown;
                                ValueTypeHelper.EnumTryParse<PageSource>(reader.ReadContentAsString(),true, out tempPageSource);
                                this.PageSource = tempPageSource;
                            }

                            if (reader.MoveToAttribute("ExportType"))
                            {
                                ExportType tempExportType = BusinessObjects.Exports.ExportType.Unknown;
                                ValueTypeHelper.EnumTryParse<ExportType>(reader.ReadContentAsString(), true, out tempExportType);
                                this.ExportType = tempExportType;
                            }

                            if (reader.MoveToAttribute("CatalogId"))
                            {
                                this.CatalogId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("IncludeAllCommonAttributeIds"))
                            {
                                this.IncludeAllCommonAttributeIds = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("IncludeAllCategoryAttributeIds"))
                            {
                                this.IncludeAllCategoryAttributeIds = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("IncludeAllRelationshipTypeIds"))
                            {
                                this.IncludeAllRelationshipTypeIds = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("RelationshipTypeIdList"))
                            {
                                this.RelationshipTypeIdList = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                            }
                        }

                        #endregion
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "SearchCriteria")
                    {
                        #region Read SearchCriteria

                        String SearchCriteriaXml = reader.ReadOuterXml();
                        this.SearchCriteria = new SearchCriteria(SearchCriteriaXml);

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

        #endregion Methods
    }
}
