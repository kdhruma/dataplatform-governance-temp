using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get search criteria. 
    /// </summary>
    public interface ISearchCriteria : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting organization id
        /// </summary>
        Int32 OrganizationId { get; set; }

        /// <summary>
        /// Property denoting container ids
        /// </summary>
        Collection<Int32> ContainerIds { get; set; }

        /// <summary>
        /// Property denoting category ids
        /// </summary>
        Collection<Int64> CategoryIds { get; set; }

        /// <summary>
        /// Property denoting entity type ids
        /// </summary>
        Collection<Int32> EntityTypeIds { get; set; }

        /// <summary>
        /// Property denoting relationship type ids
        /// </summary>
        Collection<Int32> RelationshipTypeIds { get; set; }

        /// <summary>
        /// Property denoting name of the workflow
        /// </summary>
        String WorkflowName { get; set; }

        /// <summary>
        /// Property denoting stages of workflow
        /// </summary>
        String[] WorkflowStages { get; set; }

        /// <summary>
        /// Property denoting assigned users for workflow
        /// </summary>
        String[] WorkflowAssignedUsers { get; set; }

        /// <summary>
        /// Property denoting locale id for search criteria
        /// </summary>
        Collection<LocaleEnum> Locales { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Search Criteria object
        /// </summary>
        /// <returns>Xml representation of Search Criteria object</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of SearchCriteria object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of SearchCriteria object</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Gets Search Attribute Rules
        /// </summary>
        /// <returns>Collection of ISearchAttributeRule</returns>
        Collection<ISearchAttributeRule> GetSearchAttributeRules();

        /// <summary>
        /// Sets Search Attribute Rules
        /// </summary>
        /// <param name="iSearchAttributeRules">iSearchAttributeRules</param>
        void SetSearchAttributeRules(Collection<ISearchAttributeRule> iSearchAttributeRules);

        /// <summary>
        /// Gets Business Conditions status
        /// </summary>
        /// <returns>Collection of IBusinessConditionStatus</returns>
        IBusinessConditionStatusCollection GetBusinessConditionsStatus();

        /// <summary>
        /// Sets Business Conditions status
        /// </summary>
        /// <param name="iBusinessConditionsStatus">iBusinessConditionsStatus</param>
        void SetBusinessConditionsStatus(BusinessConditionStatusCollection iBusinessConditionsStatus);

        #endregion
    }
}
