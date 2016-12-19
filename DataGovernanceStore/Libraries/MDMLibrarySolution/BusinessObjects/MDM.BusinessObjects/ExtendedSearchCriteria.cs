using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class ExtendedSearchCriteria : SearchCriteria, IExtendedSearchCriteria
    {
        #region Fields

        Dictionary<Int32, String> _additionalCatalogIds;
        Collection<Int32> _additionalEntityTypeIds;
        Boolean _extendSearchToAdditionalEntityTypes = false;
        Boolean _isSearchInMasterContainer = false;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public ExtendedSearchCriteria()
            : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public ExtendedSearchCriteria(String valuesAsXml)
            : this()
        {
            LoadSearchCriteria(valuesAsXml);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="containerIds"></param>
        /// <param name="categoryIds"></param>
        /// <param name="entityTypeIds"></param>
        /// <param name="locales"></param>
        /// <param name="additionalCatalogIds"></param>
        /// <param name="additionalEntityTypeIds"></param>
        /// <param name="extendSearchToAdditionalEntityTypes"></param>
        /// <param name="isSearchInMasterContainer"></param>
        public ExtendedSearchCriteria(Int32 organizationId, Collection<Int32> containerIds, Collection<Int64> categoryIds, Collection<Int32> entityTypeIds,
            Collection<LocaleEnum> locales, Dictionary<Int32, String> additionalCatalogIds, Collection<Int32> additionalEntityTypeIds, Boolean extendSearchToAdditionalEntityTypes, Boolean isSearchInMasterContainer)
            : base(organizationId, containerIds, categoryIds, entityTypeIds, locales)
        {
            _additionalCatalogIds = additionalCatalogIds;
            _additionalEntityTypeIds = additionalEntityTypeIds;
            _extendSearchToAdditionalEntityTypes = extendSearchToAdditionalEntityTypes;
            _isSearchInMasterContainer = isSearchInMasterContainer;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Dictionary<Int32, String> AdditionalCatalogIds
        {
            get
            {
                return _additionalCatalogIds;
            }
            set
            {
                _additionalCatalogIds = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Collection<Int32> AdditionalEntityTypeIds
        {
            get
            {
                return _additionalEntityTypeIds;
            }
            set
            {
                _additionalEntityTypeIds = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Boolean ExtendSearchToAdditionalEntityTypes
        {
            get
            {
                return _extendSearchToAdditionalEntityTypes;
            }
            set
            {
                _extendSearchToAdditionalEntityTypes = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Boolean IsSearchInMasterContainer
        {
            get
            {
                return _isSearchInMasterContainer;
            }
            set
            {
                _isSearchInMasterContainer = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public new String ToXml()
        {
            String searchCriteriaXml = String.Empty;

            String containerIds = String.Empty;
            String categoryIds = String.Empty;
            String entityTypeIds = String.Empty;
            String workflowStages = String.Empty;
            String workflowAssignedUsers = String.Empty;
            String additionalEntityTypeIds = String.Empty;

            if (this.ContainerIds != null)
            {
                containerIds = ValueTypeHelper.JoinCollection(this.ContainerIds, ",");
            }
            if (this.CategoryIds != null)
            {
                categoryIds = ValueTypeHelper.JoinCollection(this.CategoryIds, ",");
            }
            if (this.EntityTypeIds != null)
            {
                entityTypeIds = ValueTypeHelper.JoinCollection(this.EntityTypeIds, ",");
            }
            if (this.WorkflowStages != null)
            {
                workflowStages = ValueTypeHelper.JoinArray(this.WorkflowStages, ",");
            }
            if (this.WorkflowAssignedUsers != null)
            {
                workflowAssignedUsers = ValueTypeHelper.JoinArray(this.WorkflowAssignedUsers, ",");
            }
            if (this.AdditionalEntityTypeIds != null)
            {
                additionalEntityTypeIds = ValueTypeHelper.JoinCollection(this.AdditionalEntityTypeIds, ",");
            }

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Search Criteria node start
            xmlWriter.WriteStartElement("SearchCriteria");

            xmlWriter.WriteAttributeString("OrganizationId", this.OrganizationId.ToString());
            xmlWriter.WriteAttributeString("ContainerIds", containerIds);
            xmlWriter.WriteAttributeString("CategoryIds", categoryIds);
            xmlWriter.WriteAttributeString("EntityTypeIds", entityTypeIds);
            xmlWriter.WriteAttributeString("WorkflowName", this.WorkflowName);
            xmlWriter.WriteAttributeString("WorkflowStages", workflowStages);
            xmlWriter.WriteAttributeString("WorkflowAssignedUsers", workflowAssignedUsers);
            xmlWriter.WriteAttributeString("WorkflowVersion", this.WorkflowVersion);
            xmlWriter.WriteAttributeString("WorkflowType", this.WorkflowType);
            xmlWriter.WriteAttributeString("ConfiguredWorkflowForSearch", this.ConfiguredWorkflowForSearch);
            xmlWriter.WriteAttributeString("ReturnWorkflowResult", this.ReturnWorkflowResult.ToString());
            xmlWriter.WriteAttributeString("AdditionalEntityTypeIds", additionalEntityTypeIds);
            xmlWriter.WriteAttributeString("ExtendSearchToAdditionalEntityTypes", this.ExtendSearchToAdditionalEntityTypes.ToString());
            xmlWriter.WriteAttributeString("IsSearchInMasterContainer", this.IsSearchInMasterContainer.ToString());

            #region Additional Catalogs

            xmlWriter.WriteStartElement("AdditionalCatalogIds");

            foreach (KeyValuePair<Int32, String> additionalCatalog in this.AdditionalCatalogIds)
            {
                xmlWriter.WriteStartElement("AdditionalCatalog");
                xmlWriter.WriteAttributeString("ContainerId", additionalCatalog.Key.ToString());
                xmlWriter.WriteAttributeString("CategoryName", additionalCatalog.Value);
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();

            #endregion

            #region Locales Node

            xmlWriter.WriteStartElement("Locales");

            foreach (LocaleEnum locale in this.Locales)
            {
                xmlWriter.WriteStartElement("Locale");
                xmlWriter.WriteAttributeString("Id", ((Int32)locale).ToString());
                xmlWriter.WriteAttributeString("Name", locale.ToString());
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();

            #endregion Locales Node

            #region Search attribute rules Node

            xmlWriter.WriteStartElement("SearchAttributeRules");

            foreach (SearchAttributeRule attrRule in this.SearchAttributeRules)
            {
                xmlWriter.WriteRaw(attrRule.ToXml());
            }

            xmlWriter.WriteEndElement();

            #endregion Data Column Node

            //Search Criteria node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            searchCriteriaXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return searchCriteriaXml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public new void LoadSearchCriteria(String valuesAsXml)
        {
            base.LoadSearchCriteria(valuesAsXml);

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "SearchCriteria")
                        {
                            #region Read Search Criteria

                            if (reader.HasAttributes)
                            {
                                
                                if (reader.MoveToAttribute("AdditionalEntityTypeIds"))
                                {
                                    this.AdditionalEntityTypeIds = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                                }

                                if (reader.MoveToAttribute("ExtendSearchToAdditionalEntityTypes"))
                                {
                                    this.ExtendSearchToAdditionalEntityTypes = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }

                                if (reader.MoveToAttribute("IsSearchInMasterContainer"))
                                {
                                    this.IsSearchInMasterContainer = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }
                            }

                            #endregion Read Search Criteria
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "AdditionalCatalogIds")
                        {
                            String additinalCatalogXml = reader.ReadOuterXml();
                            this.AdditionalCatalogIds = LoadAdditionalCatalogs(additinalCatalogXml);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        /// <returns></returns>
        public Dictionary<Int32, String> LoadAdditionalCatalogs(String valuesAsXml)
        {
            Dictionary<Int32, String> additionalCatalogs = new Dictionary<Int32, String>();

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "AdditionalCatalog")
                        {
                            if (reader.HasAttributes)
                            {
                                Int32 containerId = 0;
                                String categoryName = String.Empty;

                                if (reader.MoveToAttribute("ContainerId"))
                                {
                                    containerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("CategoryName"))
                                {
                                    categoryName = reader.ReadContentAsString();
                                }

                                if (containerId > 0)
                                {
                                    additionalCatalogs.Add(containerId, categoryName);
                                }
                            }
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

            return additionalCatalogs;
        }

        #endregion
    }
}
