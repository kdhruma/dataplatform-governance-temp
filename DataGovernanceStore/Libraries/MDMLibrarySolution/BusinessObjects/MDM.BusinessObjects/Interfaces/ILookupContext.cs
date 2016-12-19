using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get lookup context information.
    /// </summary>
    public interface ILookupContext
    {
        #region Properties

        /// <summary>
        /// Indicates attribute id list for which lookup attribute values are to be fetched
        /// </summary>
        Collection<Int32> AttributeIdList { get; set; }

        /// <summary>
        /// Indicates  which locale lookup attribute value is to be fetched
        /// </summary>
        String Locale { get; set; }

        /// <summary>
        /// Indicates name of lookup table
        /// </summary>
        String LookupTableName { get; set; }

        /// <summary>
        /// Indicates how many lookup values are to be returned. Pass -1 to get all values
        /// </summary>
        Int32 MaxRecordsToReturn { get; set; }

        /// <summary>
        /// Indicates whether to return only lookup model (columns) or to return lookup with data
        /// </summary>
        Boolean ReturnOnlyModel { get; set; }

        /// <summary>
        /// Indicates whether lookup data is to be fetched for lookup attribute or lookup maintenance
        /// </summary>
        LookupValueFilterType ValueFilterType { get; set; }
        
        /// <summary>
        /// Indicates the criteria for filtering lookup tables
        /// </summary>
        LookupTableFilterType TableFilterType { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents LookupContext in Xml format
        /// </summary>
        /// <returns>String representing LookupContext in Xml format</returns>
        String ToXml();

        /// <summary>
        /// Represents LookupContext in Xml format for given Serialization option
        /// </summary>
        /// <param name="objectSerialization">Options specifying which Xml format is to be generated</param>
        /// <returns>String representing LookupContext in Xml format</returns>
        String ToXml( ObjectSerialization objectSerialization );

        #endregion Methods
    }
}
