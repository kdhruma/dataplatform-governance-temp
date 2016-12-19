using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties used for providing locale change context related information.
    /// </summary>
    public interface ILocaleChangeContext
    {
        #region Properties

        /// <summary>
        /// Specifies the action for based on attribute change context.
        /// </summary>
        LocaleEnum DataLocale { get; set; }

        #endregion Properties

        #region Methods

        #region ToXml Methods

        /// <summary>
        /// Gets XML representation of locale change context object
        /// </summary>
        /// <returns>XML representation of locale change context object</returns>
        String ToXml();

        #endregion ToXml Methods

        #region Attribute Change Contexts related methods

        /// <summary>
        /// Gets the attribute change contexts per locale
        /// </summary>
        /// <returns>Attribute change context for a locale .</returns>
        IAttributeChangeContextCollection GetAttributeChangeContexts();

        /// <summary>
        /// Sets the attribute change context per locale.
        /// </summary>
        /// <param name="iAttributeChangeContexts">Indicates the attribute change contexts to be set</param>
        void SetAttributeChangeContexts(IAttributeChangeContextCollection iAttributeChangeContexts);

        #endregion

        #region Relationship Change Contexts related methods

        /// <summary>
        /// Gets the relationship change contexts per locale.
        /// </summary>
        /// <returns>Gets the relationship change contexts per locale.</returns>
        IRelationshipChangeContextCollection GetRelationshipChangeContexts();

        /// <summary>
        /// Sets the relationship change contexts per locale.
        /// </summary>
        /// <param name="iRelationshipChangeContexts">Indicates the relationship change contexts to be set</param>
        void SetRelationshipChangeContexts(IRelationshipChangeContextCollection iRelationshipChangeContexts);

        #endregion

        #endregion
    }
}