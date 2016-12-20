using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get attribute context instance.
    /// </summary>
    public interface IAttributeContext
    {
        #region Properties

        /// <summary>
        /// Property denoting state view name.
        /// </summary>
        String StateViewName { get; set; }

        /// <summary>
        /// Property denoting custom view name.
        /// </summary>
        String CustomViewName { get; set; }

        /// <summary>
        /// Property denoting attributes belonging to which group are to be loaded.
        /// This property works in unison with LoadAttributesWithSpecificGroupIdList.
        /// To load attributes from group ids given in AttributeGroupIdList, LoadAttributesWithSpecificGroupIdList should be set to true.
        /// </summary>
        Collection<Int32> AttributeGroupIdList { get; set; }

        /// <summary>
        /// Property denoting short name of the attribute group name with Parent Name
        /// </summary>
        Collection<String> AttributeGroupNames { get; set; }

        /// <summary>
        /// Property denoting which attributes are to be loaded.
        /// This property works in unison with LoadAttributesWithSpecificAttributeIdList.
        /// To load attributes from attribute ids given in AttributeIdList, LoadAttributesWithSpecificGroupIdList should be set to true.
        /// </summary>
        Collection<Int32> AttributeIdList { get; set; }

        /// <summary>
        /// Property denoting shortname of the attribute with Group Name
        /// </summary>
        Collection<String> AttributeNames { get; set; }

        /// <summary>
        /// Property denoting whether to load only those attributes which are marked as ShowAtCreation
        /// </summary>
        Boolean LoadCreationAttributes { get; set; }

        /// <summary>
        /// Property denoting whether to load only those attributes which are marked as Required
        /// </summary>
        Boolean LoadRequiredAttributes { get; set; }

        /// <summary>
        /// Property denoting which type of attributes are to be loaded. Possible values <see cref="AttributeModelType"/>
        /// </summary>
        AttributeModelType AttributeModelType { get; set; }

        /// <summary>
        /// Property denoting whether to load lookup display values or not
        /// </summary>
        Boolean LoadLookupDisplayValues { get; set; }

        /// <summary>
        /// Property denoting whether to load LookupRow along with Value or not
        /// </summary>
        Boolean LoadLookupRowWithValues { get; set; }

        /// <summary>
        /// Specifies whether to load state validation attributes or not.
        /// </summary>
        Boolean LoadStateValidationAttributes { get; set; }

        #endregion Properties
    }
}