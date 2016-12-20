using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;
    
    /// <summary>
    /// Exposes methods or properties used for providing cell information in a table.
    /// </summary>
    public interface ICell 
    {
        #region Properties

        /// <summary>
        /// Property denoting column Id for which the cell is. Row has collection of cells and it stores value for each column
		/// </summary>
        Int32 ColumnId { get; set; }

        /// <summary>
        /// Property denoting column Name .
        /// </summary>
        String ColumnName { get; set; }

        /// <summary>
        /// Property denoting value for given row and column
        /// </summary>
        Object Value { get; set; }
        
        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents Cell in Xml format
        /// </summary>
        /// <returns>String representing Cell Xml</returns>
        String ToXml();
        
        /// <summary>
        /// Represents Cell in Xml format for given Serialization option
        /// </summary>
        /// <param name="objectSerialization">Serialization option for Cell object</param>
        /// <returns>String representing Cell Xml</returns>
        String ToXml( ObjectSerialization objectSerialization );

        #endregion Methods
    }
}
