using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of user preferences.
    /// </summary>
    public interface IUserPreferencesCollection : IEnumerable<UserPreferences>
    {
        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Add user preferences in collection
        /// </summary>
        /// <param name="iUserPreferences">user preferences to add in collection</param>
        void Add(IUserPreferences iUserPreferences);

        /// <summary>
        /// Get Xml representation of User Preferences Collection
        /// </summary>
        /// <returns>Xml representation of User Preferences Collection</returns>
        String ToXml();

        /// <summary>
        /// Clone User Preferences collection.
        /// </summary>
        /// <returns>cloned User Preferences collection object.</returns>
        IUserPreferencesCollection Clone();

        #endregion
    }
}
