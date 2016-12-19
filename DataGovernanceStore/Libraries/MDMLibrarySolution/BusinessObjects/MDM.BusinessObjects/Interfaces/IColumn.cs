using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the column object.
    /// </summary>
    public interface IColumn : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting the DefaultValue of column
        /// </summary>
        object DefaultValue { get; set; }

        /// <summary>
        /// Property denoting the DataType of column
        /// </summary>
        String DataType { get; set; }

        /// <summary>
        /// Property denoting the DataLength of column
        /// </summary>
        String DataLength { get; set; }

        /// <summary>
        /// Property denoting the Column IsUnique or not.
        /// </summary>
        Boolean IsUnique { get; set; }

        /// <summary>
        /// Property denoting the Column nullable or not.
        /// </summary>
        Boolean Nullable { get; set; }

        /// <summary>
        /// Property denoting the Column Sort order.
        /// </summary>
        SortOrder SortOrder { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents Column in Xml format
        /// </summary>
        /// <returns>String representing Column in Xml format</returns>
        String ToXml();

        /// <summary>
        /// Represents Column in Xml format for given Serialization option
        /// </summary>
        /// <param name="objectSerialization">Options specifying which Xml format is to be generated</param>
        /// <returns>String representing Column in Xml format</returns>
        String ToXml( ObjectSerialization objectSerialization );

        /// <summary>
        ///  Clones the Column
        /// </summary>
        /// <returns>Cloned Column object</returns>
        IColumn Clone();

        #endregion Methods
    }
}
