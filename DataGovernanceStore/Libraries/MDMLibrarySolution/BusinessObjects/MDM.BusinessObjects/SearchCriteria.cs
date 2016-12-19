using System;
using System.Xml;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    ///<summary>
    ///Specifies the search criteria object
    ///This object has been considering only STF 
    ///</summary>
    [DataContract]
    public class SearchCriteria : MDMObject, ISearchCriteria
    {
        #region Fields

        /// <summary>
        /// Field for the organization id of an search criteria
        /// </summary>
        private Int32 _organizationId = 0;

        /// <summary>
        /// Represents container ids
        /// </summary>
        private Collection<Int32> _containerIds;

        /// <summary>
        /// Represents category ids
        /// </summary>
        private Collection<Int64> _categoryIds;

        /// <summary>
        /// Represents entity type ids
        /// </summary>
        private Collection<Int32> _entityTypeIds;

        /// <summary>
        /// Represents relationship type ids
        /// </summary>
        private Collection<Int32> _relationshipTypeIds;

        /// <summary>
        /// Represents type of the workflow
        /// </summary>
        private String _workflowType = "";

        /// <summary>
        /// Represents name of the workflow
        /// </summary>
        private String _workflowName = String.Empty;

        /// <summary>
        /// Represents version of the workflow
        /// </summary>
        private String _workflowVersion = String.Empty;

        /// <summary>
        /// Represents stages of workflow
        /// </summary>
        private String[] _workflowStages;

        /// <summary>
        /// Represents assigned users for workflow
        /// </summary>
        private String[] _workflowAssignedUsers;

        /// <summary>
        /// Represents locale id for search criteria
        /// </summary>
        private Collection<LocaleEnum> _locales;

        /// <summary>
        /// Field denoting search attribute rules
        /// </summary>
        private Collection<SearchAttributeRule> _searchAttributeRules = new Collection<SearchAttributeRule>();

        /// <summary>
        /// Field denoting business conditions status
        /// </summary>
        private BusinessConditionStatusCollection _businessConditionsStatus = null;

        /// <summary>
        /// Field denoting workflow name configured in the system for search
        /// </summary>
        private String _configuredWorkflowForSearch = String.Empty;

        /// <summary>
        /// Field denoting enable/disable workflow result display on grid
        /// </summary>
        private Boolean _returnWorkflowResult = false;

        /// <summary>
        /// 
        /// </summary>
        private Collection<SearchWeightageAttribute> _searchWeightageAttributes = new Collection<SearchWeightageAttribute>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Search Criteria class.
        /// </summary>
        public SearchCriteria()
        {

        }

        /// <summary>
        /// Initializes a new instance of the Search Criteria class for category or entity type search.
        /// </summary>
        /// <param name="organizationId">This parameter is specifying organization id.</param>
        /// <param name="containerIds">Array of container ids</param>
        /// <param name="categoryIds">Array of category ids</param>
        /// <param name="entityTypeIds">Array of entity type ids</param>
        /// <param name="locales">locales</param>
        public SearchCriteria(Int32 organizationId, Collection<Int32> containerIds, Collection<Int64> categoryIds, Collection<Int32> entityTypeIds, Collection<LocaleEnum> locales)
        {
            this._organizationId = organizationId;
            this._containerIds = containerIds;
            this._categoryIds = categoryIds;
            this._entityTypeIds = entityTypeIds;
            this._locales = locales;
        }

        /// <summary>
        /// Initializes a new instance of the Search Criteria class for workflow search.
        /// </summary>
        /// <param name="organizationId">This parameter is specifying organization id.</param>
        /// <param name="containerIds">Array of container ids</param>
        /// <param name="workflowName">Name of the workflow</param>
        /// <param name="workflowStages">Array of workflow stages</param>
        /// <param name="workflowAssignedUsers">Array of workflow assigned users</param>
        /// <param name="locales">locales</param>
        public SearchCriteria(Int32 organizationId, Collection<Int32> containerIds, String workflowName, String[] workflowStages, String[] workflowAssignedUsers, Collection<LocaleEnum> locales)
        {
            this._organizationId = organizationId;
            this._containerIds = containerIds;
            this._workflowName = workflowName;
            this._workflowStages = workflowStages;
            this._workflowAssignedUsers = workflowAssignedUsers;
            this._locales = locales;
        }

        /// <summary>
        /// Initializes a new instance of the Search Criteria class for attribute rules
        /// </summary>
        /// <param name="organizationId">This parameter is specifying organization id.</param>
        /// <param name="containerIds">Array of container ids</param>
        /// <param name="locales">locales</param>
        /// <param name="searchAttributeRules">Collection of search attribute rules</param>
        public SearchCriteria(Int32 organizationId, Collection<Int32> containerIds, Collection<LocaleEnum> locales, Collection<SearchAttributeRule> searchAttributeRules)
        {
            this._organizationId = organizationId;
            this._containerIds = containerIds;
            this._locales = locales;
            this._searchAttributeRules = searchAttributeRules;
        }

        /// <summary>
        /// Initializes a new instance of the Search Criteria class
        /// </summary>
        /// <param name="organizationId">This parameter is specifying organization id.</param>
        /// <param name="containerIds">Array of container ids</param>
        /// <param name="categoryIds">Array of category ids</param>
        /// <param name="entityTypeIds">Array of entity type ids</param>
        /// <param name="workflowName">Name of the workflow</param>
        /// <param name="workflowStages">Array of workflow stages</param>
        /// <param name="workflowAssignedUsers">Array of workflow assigned users</param>
        /// <param name="locales">locales</param>
        /// <param name="searchAttributeRules">Collection of search attribute rules</param>
        public SearchCriteria(Int32 organizationId, Collection<Int32> containerIds, Collection<Int64> categoryIds, Collection<Int32> entityTypeIds, String workflowName, String[] workflowStages, String[] workflowAssignedUsers, Collection<LocaleEnum> locales, Collection<SearchAttributeRule> searchAttributeRules)
        {
            this._organizationId = organizationId;
            this._containerIds = containerIds;
            this._categoryIds = categoryIds;
            this._entityTypeIds = entityTypeIds;
            this._workflowName = workflowName;
            this._workflowStages = workflowStages;
            this._workflowAssignedUsers = workflowAssignedUsers;
            this._locales = locales;
            this._searchAttributeRules = searchAttributeRules;
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for current SearchContext object</param>
        public SearchCriteria(String valuesAsXml)
        {
            LoadSearchCriteria(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting Organization Id
        /// </summary>
        [DataMember]
        public Int32 OrganizationId
        {
            get
            {
                return this._organizationId;
            }
            set
            {
                this._organizationId = value;
            }
        }

        /// <summary>
        /// Property denoting container ids
        /// </summary>
        [DataMember]
        public Collection<Int32> ContainerIds
        {
            get
            {
                return this._containerIds;
            }
            set
            {
                this._containerIds = value;
            }
        }

        /// <summary>
        /// Property denoting category ids
        /// </summary>
        [DataMember]
        public Collection<Int64> CategoryIds
        {
            get
            {
                return this._categoryIds;
            }
            set
            {
                this._categoryIds = value;
            }
        }

        /// <summary>
        /// Property denoting entity type ids
        /// </summary>
        [DataMember]
        public Collection<Int32> EntityTypeIds
        {
            get
            {
                return this._entityTypeIds;
            }
            set
            {
                this._entityTypeIds = value;
            }
        }

        /// <summary>
        /// Property denoting relationship type ids
        /// </summary>
        [DataMember]
        public Collection<Int32> RelationshipTypeIds
        {
            get
            {
                return this._relationshipTypeIds;
            }
            set
            {
                this._relationshipTypeIds = value;
            }
        }

        /// <summary>
        /// Property denoting type of the workflow
        /// </summary>
        [DataMember]
        public String WorkflowType
        {
            get
            {
                return this._workflowType;
            }
            set
            {
                this._workflowType = value;
            }
        }

        /// <summary>
        /// Property denoting name of the workflow
        /// </summary>
        [DataMember]
        public String WorkflowName
        {
            get
            {
                return this._workflowName;
            }
            set
            {
                this._workflowName = value;
            }
        }

        /// <summary>
        /// Property denoting version of the workflow
        /// </summary>
        [DataMember]
        public String WorkflowVersion
        {
            get
            {
                return this._workflowVersion;
            }
            set
            {
                this._workflowVersion = value;
            }
        }

        /// <summary>
        /// Property denoting stages of workflow
        /// </summary>
        [DataMember]
        public String[] WorkflowStages
        {
            get
            {
                return this._workflowStages;
            }
            set
            {
                this._workflowStages = value;
            }
        }

        /// <summary>
        /// Property denoting assigned users for workflow
        /// </summary>
        [DataMember]
        public String[] WorkflowAssignedUsers
        {
            get
            {
                return this._workflowAssignedUsers;
            }
            set
            {
                this._workflowAssignedUsers = value;
            }
        }

        /// <summary>
        /// Property denoting locale id for search criteria
        /// </summary>
        [DataMember]
        public Collection<LocaleEnum> Locales
        {
            get
            {
                return this._locales;
            }
            set
            {
                this._locales = value;
            }
        }

        /// <summary>
        /// Property denoting search attribute rules
        /// </summary>
        [DataMember]
        public Collection<SearchAttributeRule> SearchAttributeRules
        {
            get
            {
                return this._searchAttributeRules;
            }
            set
            {
                this._searchAttributeRules = value;
            }
        }

        /// <summary>
        /// Property denoting business conditions status
        /// </summary>
        [DataMember]
        public BusinessConditionStatusCollection BusinessConditionsStatus
        {
            get
            {
                if (this._businessConditionsStatus == null)
                {
                    this._businessConditionsStatus = new BusinessConditionStatusCollection();
                }

                return this._businessConditionsStatus;
            }
            set
            {
                this._businessConditionsStatus = value;
            }
        }

        /// <summary>
        /// Property denotes configured work flow for search.
        /// </summary>
        [DataMember]
        public String ConfiguredWorkflowForSearch
        {
            get
            {
                return this._configuredWorkflowForSearch;
            }
            set
            {
                this._configuredWorkflowForSearch = value;
            }
        }

        /// <summary>
        /// Property denotes work flow result for search.
        /// </summary>
        [DataMember]
        public Boolean ReturnWorkflowResult
        {
            get
            {
                return this._returnWorkflowResult;
            }
            set
            {
                this._returnWorkflowResult = value;
            }
        }

        /// <summary>
        /// Property denoting search attribute rules
        /// </summary>
        [DataMember]
        public Collection<SearchWeightageAttribute> SearchWeightageAttributes
        {
            get
            {
                return this._searchWeightageAttributes;
            }
            set
            {
                this._searchWeightageAttributes = value;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Xml representation of SearchCriteria object
        /// </summary>
        /// <returns>Xml representation of SearchCriteria object</returns>
        public override String ToXml()
        {
            String searchCriteriaXml = String.Empty;

            String containerIds = String.Empty;
            String categoryIds = String.Empty;
            String entityTypeIds = String.Empty;
            String relationshipTypeIds = String.Empty;
            String workflowStages = String.Empty;
            String workflowAssignedUsers = String.Empty;

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
            if (this.RelationshipTypeIds != null)
            {
                relationshipTypeIds = ValueTypeHelper.JoinCollection(this.RelationshipTypeIds, ",");
            }
            if (this.WorkflowStages != null)
            {
                workflowStages = ValueTypeHelper.JoinArray(this.WorkflowStages, ",");
            }
            if (this.WorkflowAssignedUsers != null)
            {
                workflowAssignedUsers = ValueTypeHelper.JoinArray(this.WorkflowAssignedUsers, ",");
            }

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Search Criteria node start
            xmlWriter.WriteStartElement("SearchCriteria");

            xmlWriter.WriteAttributeString("OrganizationId", this.OrganizationId.ToString());
            xmlWriter.WriteAttributeString("ContainerIds", containerIds);
            xmlWriter.WriteAttributeString("CategoryIds", categoryIds);
            xmlWriter.WriteAttributeString("EntityTypeIds", entityTypeIds);
            xmlWriter.WriteAttributeString("RelationshipTypeIds", relationshipTypeIds);
            xmlWriter.WriteAttributeString("WorkflowName", this.WorkflowName);
            xmlWriter.WriteAttributeString("WorkflowStages", workflowStages);
            xmlWriter.WriteAttributeString("WorkflowAssignedUsers", workflowAssignedUsers);
            xmlWriter.WriteAttributeString("WorkflowVersion", this.WorkflowVersion);
            xmlWriter.WriteAttributeString("WorkflowType", this.WorkflowType);
            xmlWriter.WriteAttributeString("ConfiguredWorkflowForSearch", this.ConfiguredWorkflowForSearch);
            xmlWriter.WriteAttributeString("ReturnWorkflowResult", this.ReturnWorkflowResult.ToString());

            #region Locales Node

            xmlWriter.WriteStartElement("Locales");

            if (this.Locales != null)
            {
                foreach (LocaleEnum locale in this.Locales)
                {
                    xmlWriter.WriteStartElement("Locale");
                    xmlWriter.WriteAttributeString("Id", ((Int32)locale).ToString());
                    xmlWriter.WriteAttributeString("Name", locale.ToString());
                    xmlWriter.WriteEndElement();
                }
            }
            xmlWriter.WriteEndElement();

            #endregion Locales Node

            #region Search attribute rules Node

            xmlWriter.WriteStartElement("SearchAttributeRules");

            if (this.SearchAttributeRules != null && this.SearchAttributeRules.Count > 0)
            {
                foreach (SearchAttributeRule attrRule in this.SearchAttributeRules)
                {
                    xmlWriter.WriteRaw(attrRule.ToXml());
                }
            }

            xmlWriter.WriteEndElement();

            #endregion Data Column Node

            #region Search attribute rules Node

            if (this._businessConditionsStatus != null)
            {
                xmlWriter.WriteRaw(this._businessConditionsStatus.ToXml());
            }

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
        /// Get Xml representation of SearchCriteria object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of SearchCriteria object</returns>
        public override String ToXml(ObjectSerialization serialization)
        {
            String criteriaXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                criteriaXml = this.ToXml();
            }
            else
            {
                throw new NotSupportedException("Object serialization: " + serialization.ToString() + " is not supported for the Search Criteria object");
            }

            return criteriaXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>True if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is SearchCriteria)
                {
                    SearchCriteria objectToBeCompared = obj as SearchCriteria;

                    if (this.OrganizationId != objectToBeCompared.OrganizationId)
                        return false;

                    if (this.ContainerIds != objectToBeCompared.ContainerIds)
                        return false;

                    if (this.CategoryIds != objectToBeCompared.CategoryIds)
                        return false;

                    if (this.EntityTypeIds != objectToBeCompared.EntityTypeIds)
                        return false;

                    if (this.RelationshipTypeIds != objectToBeCompared.RelationshipTypeIds)
                        return false;

                    if (this.WorkflowType != objectToBeCompared.WorkflowType)
                        return false;

                    if (this.WorkflowName != objectToBeCompared.WorkflowName)
                        return false;

                    if (this.WorkflowVersion != objectToBeCompared.WorkflowVersion)
                        return false;

                    if (this.WorkflowStages != objectToBeCompared.WorkflowStages)
                        return false;

                    if (this.WorkflowAssignedUsers != objectToBeCompared.WorkflowAssignedUsers)
                        return false;

                    if (this.Locales != objectToBeCompared.Locales)
                        return false;

                    if (this.SearchAttributeRules != objectToBeCompared.SearchAttributeRules)
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
            int hashCode = base.GetHashCode() ^ this.OrganizationId.GetHashCode() ^ this.ContainerIds.GetHashCode() ^ this.CategoryIds.GetHashCode() ^ 
                this.EntityTypeIds.GetHashCode() ^ this.RelationshipTypeIds.GetHashCode() ^ this.WorkflowType.GetHashCode() ^ this.WorkflowName.GetHashCode() ^ 
                this.WorkflowVersion.GetHashCode() ^ this.WorkflowStages.GetHashCode() ^ this.WorkflowAssignedUsers.GetHashCode() ^ this.Locales.GetHashCode() ^ 
                this.SearchAttributeRules.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Gets Search Attribute Rules
        /// </summary>
        /// <returns></returns>
        public Collection<ISearchAttributeRule> GetSearchAttributeRules()
        {
            //TODO:: return Search Attribute Rules
            //return (Collection<ISearchAttributeRule>)this.SearchAttributeRules;
            return null;
        }

        /// <summary>
        /// Sets Search Attribute Rules
        /// </summary>
        /// <param name="iSearchAttributeRules"></param>
        public void SetSearchAttributeRules(Collection<ISearchAttributeRule> iSearchAttributeRules)
        {
            //TODO:: set Search Attribute Rules
            //this.SearchAttributeRules = (Collection<SearchAttributeRule>)iSearchAttributeRules;
        }

        /// <summary>
        /// Load SearchCriteria from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for current SearchCriteria
        /// <para>
        /// Sample Xml
        /// <![CDATA[
        /// <SearchCriteria OrganizationId="10" ContainerIds="1206" CategoryIds="" EntityTypeIds="" WorkflowName="1" WorkflowStages="WIP" LocaleId="1" WorkflowAssignedUsers="481">
        ///     <SearchAttributeRules>
        ///         <SystemAttribute WhereClause="##84##='s*'" />
        ///     </SearchAttributeRules>
        /// </SearchCriteria>
        /// ]]>
        /// </para>
        /// </param>
        public void LoadSearchCriteria(String valuesAsXml)
        {
            #region Sample Xml
            /*
            * <SearchCriteria OrganizationId="10" ContainerIds="1206" CategoryIds="" EntityTypeIds="" RelationshipTypeIds="" WorkflowName="1" WorkflowStages="WIP" LocaleId="1" WorkflowAssignedUsers="481">
                <SearchAttributeRules>
                  <SystemAttribute WhereClause="##84##='s*'" />
                </SearchAttributeRules>
              </SearchCriteria> 
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "SearchCriteria")
                        {
                            #region Read Search Criteria

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("OrganizationId"))
                                {
                                    this.OrganizationId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("ContainerIds"))
                                {
                                    this.ContainerIds = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                                }
                                if (reader.MoveToAttribute("CategoryIds"))
                                {
                                    this.CategoryIds = ValueTypeHelper.SplitStringToLongCollection(reader.ReadContentAsString(), ',');
                                }
                                if (reader.MoveToAttribute("EntityTypeIds"))
                                {
                                    this.EntityTypeIds = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                                }
                                if (reader.MoveToAttribute("RelationshipTypeIds"))
                                {
                                    this.RelationshipTypeIds = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                                }
                                if (reader.MoveToAttribute("WorkflowType"))
                                {
                                    this.WorkflowType = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("WorkflowName"))
                                {
                                    this.WorkflowName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("WorkflowVersion"))
                                {
                                    this.WorkflowVersion = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("ConfiguredWorkflowForSearch"))
                                {
                                    this.ConfiguredWorkflowForSearch = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("ReturnWorkflowResult"))
                                {
                                    this.ReturnWorkflowResult = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }
                                if (reader.MoveToAttribute("WorkflowStages"))
                                {
                                    this.WorkflowStages = ValueTypeHelper.SplitStringToStringArray(reader.ReadContentAsString(), ',');
                                }
                                if (reader.MoveToAttribute("WorkflowAssignedUsers"))
                                {
                                    this.WorkflowAssignedUsers = ValueTypeHelper.SplitStringToStringArray(reader.ReadContentAsString(), ',');
                                }
                            }

                            #endregion Read Search Criteria
                        }
                        else if (reader.NodeType == XmlNodeType.Element && (reader.Name == "Attribute" || reader.Name == "SystemAttribute"))
                        {
                            #region Read Search Attribute Rules

                            String ruleXml = reader.ReadOuterXml();
                            if (!String.IsNullOrWhiteSpace(ruleXml))
                            {
                                SearchAttributeRule rule = new SearchAttributeRule(ruleXml);
                                if (rule != null)
                                {
                                    if (this.SearchAttributeRules == null)
                                    {
                                        this.SearchAttributeRules = new Collection<SearchAttributeRule>();
                                    }
                                    this.SearchAttributeRules.Add(rule);
                                }
                            }
                            #endregion Read Search Attribute Rules
                        }
                        else if (reader.NodeType == XmlNodeType.Element && (reader.Name == "Locale"))
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Name"))
                                {
                                    String localeName = reader.ReadContentAsString();
                                    LocaleEnum locale = LocaleEnum.UnKnown;
                                    LocaleEnum.TryParse(localeName, out locale);
                                    if (this.Locales == null)
                                    {
                                        this.Locales = new Collection<LocaleEnum>();
                                    }
                                    this.Locales.Add(locale);
                                }
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && (reader.Name == "BusinessConditions"))
                        {
                            String businessConditionsXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(businessConditionsXml))
                            {
                                this._businessConditionsStatus = new BusinessConditionStatusCollection(businessConditionsXml);

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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeId"></param>
        /// <returns></returns>
        public SearchAttributeRule GetSearchAttributeRuleByAttributeId(Int32 attributeId)
        {
            SearchAttributeRule searchAttributeRule = null;

            foreach (SearchAttributeRule attributeRule in this.SearchAttributeRules)
            {
                if (searchAttributeRule.Attribute.Id == attributeId)
                {
                    searchAttributeRule = attributeRule;
                    break;
                }
            }

            return searchAttributeRule;
        }

        /// <summary>
        /// Gets Business Conditions status
        /// </summary>
        /// <returns>Returns IBusinessConditionsStatusCollection</returns>
        public IBusinessConditionStatusCollection GetBusinessConditionsStatus()
        {
            return (IBusinessConditionStatusCollection)this.BusinessConditionsStatus;
        }

        /// <summary>
        /// Sets Business Conditions status
        /// </summary>
        /// <param name="iBusinessConditionsStatus">iBusinessConditionsStatus</param>
        public void SetBusinessConditionsStatus(BusinessConditionStatusCollection iBusinessConditionsStatus)
        {
            if (iBusinessConditionsStatus == null)
            {
                throw new ArgumentNullException("iBusinessConditionsStatus");
            }

            this._businessConditionsStatus = (BusinessConditionStatusCollection)iBusinessConditionsStatus;
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}