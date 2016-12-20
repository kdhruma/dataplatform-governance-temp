using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get row information.
    /// </summary>
    public interface IRow : IMDMObject
    {
        #region Methods

        /// <summary>
        /// Represents Row in Xml format
        /// </summary>
        /// <returns>String representing Row in Xml format</returns>
        String ToXml();

        /// <summary>
        /// Represents Row in Xml format for given Serialization option
        /// </summary>
        /// <param name="objectSerialization">Options specifying which Xml format is to be generated</param>
        /// <returns>String representing Row in Xml format</returns>
        String ToXml(ObjectSerialization objectSerialization);

        /// <summary>
        ///Property denotes the cells for the row
        /// </summary>
        CellCollection Cells { get; set; }

        #region Set methods

        /// <summary>
        /// Set value for given column in current row
        /// </summary>
        /// <param name="column">Column for which value is to be added</param>
        /// <param name="value">Value to set for current Row and Column</param>
        /// <exception cref="ArgumentNullException">Thrown if column is null</exception>
        /// <exception cref="ArgumentException">thrown if Column.Name is not provided</exception>
        /// <exception cref="Exception">Thrown if there is no cell available for given Column.Id and Column.Name</exception>
        void SetValue(Column column, Object value);

        /// <summary>
        /// Set value for given column id in current row
        /// </summary>
        /// <param name="columnId">Column id for which value is to be added</param>
        /// <param name="value">Value to set for current Row and Column id</param>
        /// <exception cref="Exception">Thrown if there is no cell available for given ColumnId</exception>
        void SetValue(Int32 columnId, Object value);

        /// <summary>
        /// Set value for given column Name in current row
        /// </summary>
        /// <param name="columnName">Column Name for which value is to be added</param>
        /// <param name="value">Value to set for current Row and Column id</param>
        /// <exception cref="Exception">Thrown if there is no cell available for given ColumnName</exception>
        void SetValue(String columnName, Object value);

        #endregion

        #region Get methods

        /// <summary>
        /// Get value for given column in current row
        /// </summary>
        /// <param name="column">Column for which value is to be fetched</param>
        /// <returns>Value at current row and given column</returns>
        /// <exception cref="ArgumentNullException">Thrown if Column is null</exception>
        /// <exception cref="ArgumentException">Thrown if Column.Id is less than 0 or Column.Name is not provided</exception>
        Object GetValue(Column column);

        /// <summary>
        /// Get value for given column in current row
        /// </summary>
        /// <param name="columnId">Column id for which value is to be fetched</param>
        /// <returns>Value at current row and given column id</returns>
        Object GetValue(Int32 columnId);

        /// <summary>
        /// Get value for given column in current row
        /// </summary>
        /// <param name="columnName">Column Name for which value is to be fetched</param>
        /// <returns>Value at current row and given column Name</returns>
        Object GetValue(String columnName);

        /// <summary>
        /// Compares row for given column, search operator and filter value
        /// </summary>
        /// <param name="columnName">Name of the column</param>
        /// <param name="searchOperator">Indicates search Operator</param>
        /// <param name="filterValue">Indiactes Filter value</param>
        /// <param name="valueSeparator">Indicates value separator</param>
        /// <returns>True if matched successfully</returns>
        Boolean CompareValue(String columnName, SearchOperator searchOperator, String filterValue, String valueSeparator);

        /// <summary>
        /// Compares row for given column, search operator and filter value
        /// </summary>
        /// <param name="columnName">Name of the column</param>
        /// <param name="searchOperator">Indicates search opeator</param>
        /// <param name="filterValue">Indicates Filter value</param>
        /// <param name="comparisionType">Indicated comparision Type</param>
        /// <param name="valueSeparator">Indicates Value Separator</param>
        /// <returns>True if matched successfully</returns>
        Boolean CompareValue(String columnName, SearchOperator searchOperator, String filterValue, StringComparison comparisionType, String valueSeparator);

        #endregion

        #endregion Methods
    }
}
