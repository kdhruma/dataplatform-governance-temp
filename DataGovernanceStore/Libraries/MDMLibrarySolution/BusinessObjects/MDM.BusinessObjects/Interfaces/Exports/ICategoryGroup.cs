using System;
namespace MDM.Interfaces.Exports
{


    using MDM.BusinessObjects;
    /// <summary>
    /// Exposes methods or properties to set or get the MDM object group related information.
    /// </summary>
    public interface ICategoryGroup 
    {
        #region Properties
        
        /// <summary>
        /// Property specifying collection of categories
        /// </summary>
        CategoryCollection Categories { get; set; }

        #endregion Properties

        #region Methods
        /// <summary>
        /// Gets Xml representation of Category Group
        /// </summary>
        /// <returns>Returns Category Group in Xml format</returns>
        String ToXml();

        #endregion Methods
    }
}
