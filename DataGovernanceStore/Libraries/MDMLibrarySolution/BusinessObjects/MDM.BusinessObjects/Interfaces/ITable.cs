using System;
using System.Collections;
using System.Data;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get a table. A Table is matrix of rows and columns.
    /// </summary>
    public interface ITable : IMDMObject
    {
        #region Xml Methods

        /// <summary>
        /// Represents Table in Xml format
        /// </summary>
        /// <returns>String representing Table in Xml format</returns>
        String ToXml();

        /// <summary>
        /// Represents Table in Xml format for given Serialization option
        /// </summary>
        /// <param name="objectSerialization">Options specifying which Xml format is to be generated</param>
        /// <returns>String representing Table in Xml format</returns>
        String ToXml( ObjectSerialization objectSerialization );

        #endregion Xml Methods

        #region Get Set Methods

        /// <summary>
        /// Get rows of current table
        /// </summary>
        /// <returns>Rows collection interface</returns>
        IRowCollection GetRows();

        /// <summary>
        /// Get columns of current table
        /// </summary>
        /// <returns>Columns collection interface</returns>
        IColumnCollection GetColumns();

        /// <summary>
        /// Get column by ColumnName
        /// </summary>
        /// <param name="columnName">Name of the Column</param>
        /// <returns>Column having given name</returns>
        IColumn GetColumn( String columnName );

        /// <summary>
        /// Get Extended properties from current table
        /// </summary>
        /// <returns>Hashtable having key - value pair of extended properties</returns>
        Hashtable GetExtendedProperties();

        #endregion Get Set Methods

        /// <summary>
        /// Add column in Current table. This method will make sure all existing rows has structure setup for newly added column
        /// </summary>
        /// <param name="newColumn">Column to add</param>
        /// <returns>Indicates whether column is successfully added or not</returns>
        Boolean AddColumn( IColumn newColumn );

        #region Filter lookup rows methods

        /// <summary>
        /// Filter lookup rows for the given column name with provided searchValue
        /// </summary>
        /// <param name="columnName">Name of the column</param>
        /// <param name="searchOperator">Search Operartor</param>
        /// <param name="filterValue">Value to be filtered</param>
        /// <param name="valueSeparator">Value Separartor</param>
        /// <returns>Collection of filtered row</returns>
        IRowCollection Filter(String columnName, SearchOperator searchOperator, String filterValue, String valueSeparator);

        /// <summary>
        /// Filter lookup rows for the given column name with provided searchValue
        /// </summary>
        /// <param name="columnName">Name of the column</param>
        /// <param name="searchOperator">Search Operartor</param>
        /// <param name="filterValue">Value to be filtered</param>
        /// <param name="comparisionType">comparisionType</param>
        /// <param name="valueSeparator">valueSeparator</param>
        /// <returns>Collection of filtered row</returns>
        IRowCollection Filter(String columnName, SearchOperator searchOperator, String filterValue, StringComparison comparisionType, String valueSeparator);

        /// <summary>
        /// Filter lookup rows for the given column name with provided searchValue
        /// </summary>
        /// <param name="filterExpression">filterExpression</param>
        /// <returns>IRowCollection</returns>
        IRowCollection Filter(String filterExpression);

        #endregion

        #region ToSystemDataTable
        
        /// <summary>
        /// Convert Table to System DataTable
        /// </summary>
        /// <returns>Returns the DataTable equivalent of Table</returns>
        DataTable ToSystemDataTable();

        #endregion

    }
}
