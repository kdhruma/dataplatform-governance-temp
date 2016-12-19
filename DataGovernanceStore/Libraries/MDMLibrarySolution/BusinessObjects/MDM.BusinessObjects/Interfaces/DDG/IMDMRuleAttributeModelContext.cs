using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get MDMRule attribute model context
    /// </summary>
    public interface IMDMRuleAttributeModelContext
    {
        #region Properties

        /// <summary>
        /// Property denotes the container Id list
        /// </summary>
        Collection<Int32> ContainerIdList { get; set; }

        /// <summary>
        /// Property denotes whether to load full entity family or only for requested Entity
        /// </summary>
        Boolean LoadEntityFamily { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets Xml representation of MDMRuleAttributeModelContext Object
        /// </summary>
        String ToXml();

        /// <summary>
        /// Set the AttributeModel Context 
        /// </summary>
        /// <param name="modelContext">Indicates the IAttributeModelContext</param>
        void SetAttributeModelContext(IAttributeModelContext modelContext);

        /// <summary>
        /// Set the MDMRuleContextFilter
        /// </summary>
        /// <param name="mdmRuleContextFilter">Indicates the MDMRuleContextFilter</param>
        void SetMDMRuleContextFilter(IMDMRuleContextFilter mdmRuleContextFilter);

        #endregion Methods

    }
}
