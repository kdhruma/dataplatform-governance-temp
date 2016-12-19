using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get search attribute rule.
    /// </summary>
    public interface ISearchAttributeRule : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Represents search operator for rule
        /// </summary>
        SearchOperator Operator { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Search Attribute Rule
        /// </summary>
        /// <returns>Xml representation of Search Attribute Rule</returns>
        String ToXml();

        /// <summary>
        /// Get Attribute for current search rule
        /// </summary>
        /// <returns>Attribute interface</returns>
        /// <exception cref="NullReferenceException">Thrown when Attribute is null</exception>
        IAttribute GetAttribute();

        /// <summary>
        /// Set attribute for current search rule
        /// </summary>
        /// <param name="attribute">Attribute to set</param>
        /// <exception cref="ArgumentNullException">Thrown if attribute is null</exception>
        void SetAttribute(IAttribute attribute);

        #endregion
    }
}
