using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel;
using RS.MDM.Configuration;
using MDM.Core;
using System.IO;
using System.Xml;

namespace RS.MDM.ConfigurationObjects
{
    [XmlRoot("InjectionSearchCriteria")]
    [Serializable()]
    public sealed class InjectionSearchCriteria : Object
    {
        /// <summary>
        /// Field for the organization id of an search criteria
        /// </summary>
        private Int32 _organizationId = 0;

        /// <summary>
        /// Represents container ids
        /// </summary>
        private Collection<Int32> _containerIds=new Collection<Int32>() ;

        /// <summary>
        /// Represents category ids
        /// </summary>
        private Collection<Int64> _categoryIds = new Collection<Int64>();

        /// <summary>
        /// Represents entity type ids
        /// </summary>
        private Collection<Int32> _entityTypeIds=new Collection<Int32>();

        /// <summary>
        /// Represents type of the workflow
        /// </summary>
        private String _workflowType = String.Empty;

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
        private String[] _workflowStages=new String[]{};

        /// <summary>
        /// Represents assigned users for workflow
        /// </summary>
        private String[] _workflowAssignedUsers=new String[]{};

        /// <summary>
        /// Represents locale id for search criteria
        /// </summary>
        private Collection<LocaleEnum> _locales=new Collection<LocaleEnum>();

        /// <summary>
        /// Field denoting search attribute rules
        /// </summary>
        private Collection<InjectionSearchAttributeRule> _injectionSearchAttributeRules = new Collection<InjectionSearchAttributeRule>();

        /// <summary>
        /// Field denotes the attribute injection mode 
        /// </summary>
        private AttributeValueInjectionMode _attributeValueInjectionMode = AttributeValueInjectionMode.Merge;

        #region Properties

        /// <summary>
        /// Property denoting Organization Id
        /// </summary>
        [XmlAttribute("OrganizationId")]
        [Category("InjectionSearchCriteria")]
        [Description("Property denoting Organization Id")]
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
        [XmlAttribute("ContainerIds")]
        [Category("InjectionSearchCriteria")]
        [Description("Property denoting container ids")]
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
        [XmlAttribute("CategoryIds")]
        [Category("InjectionSearchCriteria")]
        [Description("Property denoting category ids")]
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
        [XmlAttribute("EntityTypeIds")]
        [Description("Property denoting entity type ids")]
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
        /// Property denoting type of the workflow
        /// </summary>
        [XmlAttribute("WorkflowType")]
        [Category("InjectionSearchCriteria")]
        [Description("Property denoting type of the workflow")]
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
        [XmlAttribute("WorkflowName")]
        [Category("InjectionSearchCriteria")]
        [Description("Property denoting name of the workflow")]
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
        [XmlAttribute("WorkflowVersion")]
        [Category("InjectionSearchCriteria")]
        [Description("Property denoting version of the workflow")]
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
        /// Property denoting stages of workflowStages
        /// </summary>
        [XmlAttribute("WorkflowStages")]
        [Category("InjectionSearchCriteria")]
        [Description("Property denoting stages of workflowStages")]
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
        [XmlAttribute("WorkflowAssignedUsers")]
        [Category("InjectionSearchCriteria")]
        [Description("Property denoting assigned users for workflow")]
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
        [XmlElement("Locales")]
        [Category("InjectionSearchCriteria")]
        [Description("Property denoting locale id for search criteria")]
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
        [XmlElement("InjectionSearchAttributeRules")]
        [Category("InjectionSearchCriteria")]
        [Description("Property denoting search attribute rules")]
        public Collection<InjectionSearchAttributeRule> InjectionSearchAttributeRules
        {
            get
            {
                return this._injectionSearchAttributeRules;
            }
            set
            {
                this._injectionSearchAttributeRules = value;
            }
        }

        /// <summary>
        /// Property denotes the attribute injection mode 
        /// </summary>
        [XmlAttribute("AttributeValueInjectionMode")]
        [Category("InjectionSearchCriteria")]
        [Description("Property denotes the attribute injection mode ")]
        public AttributeValueInjectionMode AttributeValueInjectionMode
        {
            get
            {
                return this._attributeValueInjectionMode;
            }
            set
            {
                this._attributeValueInjectionMode = value;
            }
        }

        #endregion


        
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Search Criteria class.
        /// </summary>
        public InjectionSearchCriteria():base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the Search Criteria class for category or entity type search.
        /// </summary>
        /// <param name="organizationId">This parameter is specifying organization id.</param>
        /// <param name="containerIds">Array of container ids</param>
        /// <param name="categoryIds">Array of category ids</param>
        /// <param name="entityTypeIds">Array of entity type ids</param>
        /// <param name="localeId">LocaleId</param>
        public InjectionSearchCriteria(Int32 organizationId, Collection<Int32> containerIds, Collection<Int64> categoryIds, Collection<Int32> entityTypeIds, Collection<LocaleEnum> locales)
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
        /// <param name="localeId">LocaleId</param>
        public InjectionSearchCriteria(Int32 organizationId, Collection<Int32> containerIds, String workflowName, String[] workflowStages, String[] workflowAssignedUsers, Collection<LocaleEnum> locales)
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
        /// <param name="localeId">LocaleId</param>
        /// <param name="searchAttributeRules">Collection of search attribute rules</param>
        public InjectionSearchCriteria(Int32 organizationId, Collection<Int32> containerIds, Collection<LocaleEnum> locales, Collection<InjectionSearchAttributeRule> searchAttributeRules)
        {
            this._organizationId = organizationId;
            this._containerIds = containerIds;
            this._locales = locales;
            this._injectionSearchAttributeRules = searchAttributeRules;
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
        /// <param name="localeId">LocaleId</param>
        /// <param name="searchAttributeRules">Collection of search attribute rules</param>
        public InjectionSearchCriteria(Int32 organizationId, Collection<Int32> containerIds, Collection<Int64> categoryIds, Collection<Int32> entityTypeIds, String workflowName, String[] workflowStages, String[] workflowAssignedUsers, Collection<LocaleEnum> locales, Collection<InjectionSearchAttributeRule> searchAttributeRules)
        {
            this._organizationId = organizationId;
            this._containerIds = containerIds;
            this._categoryIds = categoryIds;
            this._entityTypeIds = entityTypeIds;
            this._workflowName = workflowName;
            this._workflowStages = workflowStages;
            this._workflowAssignedUsers = workflowAssignedUsers;
            this._locales = locales;
            this._injectionSearchAttributeRules = searchAttributeRules;
        }

    
        #endregion

        #region Serialization & Deserialization

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        [System.Runtime.Serialization.OnDeserialized()]
        private void OnDeserialized(System.Runtime.Serialization.StreamingContext context)
        {
            this.SetParent();
        }

        #endregion  

        #region Methods

        #region Public Methods


        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>True if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is InjectionSearchCriteria)
                {
                    InjectionSearchCriteria objectToBeCompared = obj as InjectionSearchCriteria;

                    if (this.OrganizationId != objectToBeCompared.OrganizationId)
                        return false;

                    if (this.ContainerIds != objectToBeCompared.ContainerIds)
                        return false;

                    if (this.CategoryIds != objectToBeCompared.CategoryIds)
                        return false;

                    if (this.EntityTypeIds != objectToBeCompared.EntityTypeIds)
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

                    if (this.InjectionSearchAttributeRules != objectToBeCompared.InjectionSearchAttributeRules)
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
        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode() ^ this.OrganizationId.GetHashCode() ^ this.ContainerIds.GetHashCode() ^ this.CategoryIds.GetHashCode() ^ this.EntityTypeIds.GetHashCode() ^ this.WorkflowType.GetHashCode() ^ this.WorkflowName.GetHashCode() ^ this.WorkflowVersion.GetHashCode() ^ this.WorkflowStages.GetHashCode() ^ this.WorkflowAssignedUsers.GetHashCode() ^ this.Locales.GetHashCode() ^ this.InjectionSearchAttributeRules.GetHashCode();
            return hashCode;
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}
