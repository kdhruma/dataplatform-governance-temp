using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of columns.
    /// </summary>
    public interface IColumnCollection : IEnumerable<Column>
    {
        #region ToXml methods

        /// <summary>
        /// Get Xml representation of ColumnCollection object
        /// </summary>
        /// <returns>Xml string representing the ColumnCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of ColumnCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml( ObjectSerialization serialization );

        #endregion ToXml methods

        /// <summary>
        /// Determines whether the ColumnCollection contains a column with specific column name.
        /// </summary>
        /// <param name="columnName">The column object with given Column.Name to locate in the ColumnCollection.</param>
        /// <returns>
        /// <para>true : If column found in columnCollection</para>
        /// <para>false : If column found not in columnCollection</para>
        /// </returns>
        bool Contains( String columnName );

        /// <summary>
        /// Adds column object in collection
        /// Usage of this method should be avoided to ensure row's consistency for added columns.
        /// Instead use "Table.AddColumn(Column newColumn)" method
        /// </summary>
        /// <param name="column">Indicates column to add in collection</param>
        /// <exception cref="Exception">Thrown if column having same name is being added</exception>
        void Add(IColumn column);

        /// <summary>
        /// Deep clones column collection with each column in the collection
        /// </summary>
        /// <returns>Cloned column collection object</returns>
        IColumnCollection Clone();
    }
}
