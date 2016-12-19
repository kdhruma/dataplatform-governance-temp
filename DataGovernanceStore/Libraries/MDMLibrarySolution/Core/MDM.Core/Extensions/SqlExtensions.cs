using System;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

namespace MDM.Core.Extensions
{
    /// <summary>
    /// Represent class for Sql Extensions
    /// </summary>
    public static class SqlExtensions
    {
        /// <summary>
        /// Sets nullable value to sql data record
        /// </summary>
        /// <typeparam name="T">Structure type</typeparam>
        /// <param name="rec">Record</param>
        /// <param name="index">Index</param>
        /// <param name="value">Nullable value</param>
        public static void SetNullableValue<T>(this SqlDataRecord rec, Int32 index, T? value)
            where T : struct
        {
            if (value.HasValue)
                rec.SetValue(index, value.GetValueOrDefault());
            else
                rec.SetDBNull(index);
        }

        /// <summary>
        /// Checks if SqlDataReader has specified column name
        /// </summary>
        /// <param name="reader">SqlDataReader</param>
        /// <param name="columnName">Column name for check</param>
        /// <returns>true if reader contains specified column, false otherwise</returns>
        public static Boolean HasColumn(this SqlDataReader reader, String columnName)
        {
            for (Int32 i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }

    }
}