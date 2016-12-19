using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MDM.Interfaces.Exports
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Exports;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of lookup export scope.
    /// </summary>
    public interface ILookupExportScopeCollection : ICollection<LookupExportScope>
    {
        #region Properties

        /// <summary>
        /// Represents whether all lookups to be included in the scope or only the specific lookups.
        /// </summary>
        Boolean IncludeAllLookup
        { get; set; }

        /// <summary>
        /// Represents whether all locales to be included in the scope or only the specific locales.
        /// </summary>
        Boolean IncludeAllLocale
        { get; set; }

        /// <summary>
        /// Represents the collection of Lookup export scope list.
        /// </summary>
        Collection<LookupExportScope> LookupExportScopes
        { get; set; }

        /// <summary>
        /// Represents the lookup export group by order.
        /// </summary>
        LookupExportGroupOrder GroupBy { get; set; }

        /// <summary>
        /// Represents the lookup export file format.
        /// </summary>  
        LookupExportFileFormat FileFormat { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Represents Lookup export Scopes in Xml format
        /// </summary>
        /// <returns>String representation of Lookup export scopes object</returns>
        String ToXml();

        /// <summary>
        /// Sets the Locale list for the current export scope
        /// </summary>
        /// <param name="iLocaleCollection">Indicates the locale Collection Interface</param>
        void SetExportScopeLocales(ILocaleCollection iLocaleCollection);

        /// <summary>
        /// Gets the locales list for the current export scope
        /// </summary>
        /// <returns>Locale Collection Interface</returns>
        /// <exception cref="NullReferenceException">Locale list for the current export scope is null.</exception>
        ILocaleCollection GetExportScopeLocales();

        #endregion
    }
}
