using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces.Exports
{
    using MDM.Core;
    using MDM.Core.DataModel;

    /// <summary>
    /// Exposes methods or properties to set or get the data model export profile business object.
    /// </summary>
    public interface IDataModelExportProfileData
    {
        #region Properties

        /// <summary>
        /// Property denoting the organization ids.
        /// </summary>
        Collection<Int32> OrganizationIds { get; set; }

        /// <summary>
        /// Property denoting the container ids.
        /// </summary>
        Collection<Int32> ContainerIds { get; set; }

        /// <summary>
        /// Property denoting the locale id list.
        /// </summary>
        Collection<LocaleEnum> LocaleIds { get; set; }

        /// <summary>
        /// Property denoting the sheet id list.
        /// </summary>
        Collection<DataModelSheet> SheetIds { get; set; }

        /// <summary>
        /// Property denoting whether to exclude non translated data or not
        /// </summary>
        Boolean ExcludeNonTranslatedModelData { get; set; }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Represents data model export profile data in XML format
        /// </summary>
        /// <returns>String representation of current data model export profile data object</returns>
        String ToXml();

        #endregion Public Methods

        #endregion Methods
    }
}
