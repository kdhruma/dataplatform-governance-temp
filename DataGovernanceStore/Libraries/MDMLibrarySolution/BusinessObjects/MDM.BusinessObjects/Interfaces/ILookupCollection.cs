using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties used for adding lookup table in table collection.
    /// </summary>
    public interface ILookupCollection : IEnumerable<Lookup> 
    {
        #region ToXml methods

        /// <summary>
        /// Gets Xml representation of TableCollection object
        /// </summary>
        /// <returns>Returns Xml string representing the TableCollection</returns>
        String ToXml();

        /// <summary>
        /// Gets Xml representation of TableCollection object
        /// </summary>
        /// <param name="serialization">Indicates the serialization option. XML representation differs based on the value selected</param>
        /// <returns>Returns Xml representation of object</returns>
        String ToXml( ObjectSerialization serialization );

        /// <summary>
        /// Gets Xml string representing lookup model for each lookup in this collection
        /// </summary>
        /// <param name="returnOnlyDisplayColumns">Indicates whether to return only display columns or not</param>
        /// <returns>Returns Xml string representing lookup model for each lookup in this collection</returns>
        String GetLookupsModel(Boolean returnOnlyDisplayColumns = false);

        #endregion ToXml methods

        #region Add Lookup Methods

        /// <summary>
        /// Add table in ITableCollection
        /// </summary>
        /// <param name="lookup">lookup to add in ITableCollection</param>
        /// <exception cref="ArgumentNullException">Thrown if table is null</exception>
        void AddLookup(ILookup lookup);

        /// <summary>
        /// Add tables in current table collection
        /// </summary>
        /// <param name="lookups">IlookupCollection to add in current collection</param>
        /// <exception cref="ArgumentNullException">Thrown if IlookupCollection is null</exception>
        void AddLookups(ILookupCollection lookups);

        #endregion

        #region Get Lookup Methods

        /// <summary>
        /// Get Lookup from collection based on Name
        /// </summary>
        /// <param name="lookupName">Name of table to search in current collection</param>
        /// <param name="locale">Locale to search in current collection</param>
        /// <returns>Lookup</returns>
        Lookup GetLookup(String lookupName, LocaleEnum locale);

        /// <summary>
        /// Get Lookup from collection based on attribute id
        /// </summary>
        /// <param name="attributeId">Attribute id to be searched in current collection</param>
        /// <param name="locale">Locale to be searched in current collection</param>
        /// <returns>Lookup</returns>
        Lookup GetLookup(Int32 attributeId, LocaleEnum locale);

        #endregion
    }
}
