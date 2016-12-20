
using System;
using System.Xml;
using System.Data;
using System.Text;
using System.Data.SqlTypes;
using System.Configuration;
using MDM.Core;
using MDM.Utility;

namespace Riversand.StoredProcedures
{
    /// <summary>
    /// 
    /// </summary>
    public class Lookup
    {
        /// <summary>
        /// Private constructor
        /// </summary>		
        private Lookup()
        {
        }


        /// <summary>
        /// 
        /// </summary>
        public static void updateBreakerSet(SqlInt32 PK_Word_BreakerSet, SqlString ShortName, SqlString LongName, SqlString BreakersXml, SqlString DeleteFlag, SqlString UserId, SqlString ProgramName, SqlBoolean ForInsert, out SqlInt32 RETURN_VALUE)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            Lookup.updateBreakerSet(PK_Word_BreakerSet, ShortName, LongName, BreakersXml, DeleteFlag, UserId, ProgramName, ForInsert, out RETURN_VALUE, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void updateBreakerSet(SqlInt32 PK_Word_BreakerSet, SqlString ShortName, SqlString LongName, SqlString BreakersXml, SqlString DeleteFlag, SqlString UserId, SqlString ProgramName, SqlBoolean ForInsert, out SqlInt32 RETURN_VALUE, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            Lookup.updateBreakerSet(PK_Word_BreakerSet, ShortName, LongName, BreakersXml, DeleteFlag, UserId, ProgramName, ForInsert, out RETURN_VALUE, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void updateBreakerSet(SqlInt32 PK_Word_BreakerSet, SqlString ShortName, SqlString LongName, SqlString BreakersXml, SqlString DeleteFlag, SqlString UserId, SqlString ProgramName, SqlBoolean ForInsert, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    SqlLookup.updateBreakerSet(PK_Word_BreakerSet, ShortName, LongName, BreakersXml, DeleteFlag, UserId, ProgramName, ForInsert, out RETURN_VALUE, connection, transaction);

                    break;

                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.updateBreakerSet for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.updateBreakerSet for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable getBreakers(SqlInt32 FK_Word_BreakerSet)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Lookup.getBreakers(FK_Word_BreakerSet, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable getBreakers(SqlInt32 FK_Word_BreakerSet, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Lookup.getBreakers(FK_Word_BreakerSet, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable getBreakers(SqlInt32 FK_Word_BreakerSet, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlLookup.getBreakers(FK_Word_BreakerSet, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.getBreakers for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.getBreakers for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetTableTypes()
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Lookup.GetTableTypes(connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetTableTypes(IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Lookup.GetTableTypes(connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetTableTypes(IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlLookup.GetTableTypes(connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.GetTableTypes for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.GetTableTypes for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable exportLists(SqlInt32 ListId)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Lookup.exportLists(ListId, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable exportLists(SqlInt32 ListId, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Lookup.exportLists(ListId, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable exportLists(SqlInt32 ListId, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlLookup.exportLists(ListId, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.exportLists for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.exportLists for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetTableTypeTemplate(SqlInt32 FK_RST_TableType)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Lookup.GetTableTypeTemplate(FK_RST_TableType, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetTableTypeTemplate(SqlInt32 FK_RST_TableType, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Lookup.GetTableTypeTemplate(FK_RST_TableType, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetTableTypeTemplate(SqlInt32 FK_RST_TableType, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlLookup.GetTableTypeTemplate(FK_RST_TableType, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.GetTableTypeTemplate for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.GetTableTypeTemplate for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetTableNames(SqlInt32 FK_RST_ObjectType, SqlString SearchString, SqlBoolean GetFromSysObj, SqlBoolean GetAttrCountColumn, SqlBoolean GetUniqueColumnTable)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Lookup.GetTableNames(FK_RST_ObjectType, SearchString, GetFromSysObj, GetAttrCountColumn, GetUniqueColumnTable, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetTableNames(SqlInt32 FK_RST_ObjectType, SqlString SearchString, SqlBoolean GetFromSysObj, SqlBoolean GetAttrCountColumn, SqlBoolean GetUniqueColumnTable, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Lookup.GetTableNames(FK_RST_ObjectType, SearchString, GetFromSysObj, GetAttrCountColumn, GetUniqueColumnTable, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetTableNames(SqlInt32 FK_RST_ObjectType, SqlString SearchString, SqlBoolean GetFromSysObj, SqlBoolean GetAttrCountColumn, SqlBoolean GetUniqueColumnTable, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlLookup.GetTableNames(FK_RST_ObjectType, SearchString, GetFromSysObj, GetAttrCountColumn, GetUniqueColumnTable, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.GetTableNames for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.GetTableNames for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetTableStructure(SqlString TableName, SqlBoolean GetFromSysObj)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Lookup.GetTableStructure(TableName, GetFromSysObj, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetTableStructure(SqlString TableName, SqlBoolean GetFromSysObj, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Lookup.GetTableStructure(TableName, GetFromSysObj, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetTableStructure(SqlString TableName, SqlBoolean GetFromSysObj, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlLookup.GetTableStructure(TableName, GetFromSysObj, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.GetTableStructure for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.GetTableStructure for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetRefTableData(SqlString TableName, SqlString RefColumnName, SqlString RefMask, SqlString DisplayColumns, SqlString SortOrder, SqlString SearchColumns)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Lookup.GetRefTableData(TableName, RefColumnName, RefMask, DisplayColumns, SortOrder, SearchColumns, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetRefTableData(SqlString TableName, SqlString RefColumnName, SqlString RefMask, SqlString DisplayColumns, SqlString SortOrder, SqlString SearchColumns, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Lookup.GetRefTableData(TableName, RefColumnName, RefMask, DisplayColumns, SortOrder, SearchColumns, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetRefTableData(SqlString TableName, SqlString RefColumnName, SqlString RefMask, SqlString DisplayColumns, SqlString SortOrder, SqlString SearchColumns, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlLookup.GetRefTableData(TableName, RefColumnName, RefMask, DisplayColumns, SortOrder, SearchColumns, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.GetRefTableData for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.GetRefTableData for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ProcessTableMetaData(SqlString dataXml, SqlString userLogin, SqlString userProgram, out SqlInt32 RETURN_VALUE)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            Lookup.ProcessTableMetaData(dataXml, userLogin, userProgram, out RETURN_VALUE, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ProcessTableMetaData(SqlString dataXml, SqlString userLogin, SqlString userProgram, out SqlInt32 RETURN_VALUE, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            Lookup.ProcessTableMetaData(dataXml, userLogin, userProgram, out RETURN_VALUE, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ProcessTableMetaData(SqlString dataXml, SqlString userLogin, SqlString userProgram, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    SqlLookup.ProcessTableMetaData(dataXml, userLogin, userProgram, out RETURN_VALUE, connection, transaction);

                    break;

                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.ProcessTableMetaData for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.ProcessTableMetaData for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void PopulateRSTObjects(SqlString TableNames, SqlInt32 tableObjectType, SqlBoolean isSysTables, SqlString userLogin, SqlString userProgram, out SqlInt32 RETURN_VALUE)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            Lookup.PopulateRSTObjects(TableNames, tableObjectType, isSysTables, userLogin, userProgram, out RETURN_VALUE, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void PopulateRSTObjects(SqlString TableNames, SqlInt32 tableObjectType, SqlBoolean isSysTables, SqlString userLogin, SqlString userProgram, out SqlInt32 RETURN_VALUE, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            Lookup.PopulateRSTObjects(TableNames, tableObjectType, isSysTables, userLogin, userProgram, out RETURN_VALUE, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void PopulateRSTObjects(SqlString TableNames, SqlInt32 tableObjectType, SqlBoolean isSysTables, SqlString userLogin, SqlString userProgram, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    SqlLookup.PopulateRSTObjects(TableNames, tableObjectType, isSysTables, userLogin, userProgram, out RETURN_VALUE, connection, transaction);

                    break;

                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.PopulateRSTObjects for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.PopulateRSTObjects for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void DeleteRSTObject(SqlInt32 Table_Object, SqlInt32 Column_Object, out SqlInt32 RETURN_VALUE)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            Lookup.DeleteRSTObject(Table_Object, Column_Object, out RETURN_VALUE, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void DeleteRSTObject(SqlInt32 Table_Object, SqlInt32 Column_Object, out SqlInt32 RETURN_VALUE, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            Lookup.DeleteRSTObject(Table_Object, Column_Object, out RETURN_VALUE, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void DeleteRSTObject(SqlInt32 Table_Object, SqlInt32 Column_Object, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    SqlLookup.DeleteRSTObject(Table_Object, Column_Object, out RETURN_VALUE, connection, transaction);

                    break;

                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.DeleteRSTObject for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.DeleteRSTObject for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ProcessUnitWords(SqlString dataXml, SqlString userLogin, SqlString userProgram, out SqlInt32 RETURN_VALUE)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            Lookup.ProcessUnitWords(dataXml, userLogin, userProgram, out RETURN_VALUE, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ProcessUnitWords(SqlString dataXml, SqlString userLogin, SqlString userProgram, out SqlInt32 RETURN_VALUE, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            Lookup.ProcessUnitWords(dataXml, userLogin, userProgram, out RETURN_VALUE, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ProcessUnitWords(SqlString dataXml, SqlString userLogin, SqlString userProgram, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    SqlLookup.ProcessUnitWords(dataXml, userLogin, userProgram, out RETURN_VALUE, connection, transaction);

                    break;

                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.ProcessUnitWords for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.ProcessUnitWords for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetUnitWords(SqlString TableName, SqlString ColumnName, SqlInt32 RowID, SqlString ObjectType)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Lookup.GetUnitWords(TableName, ColumnName, RowID, ObjectType, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetUnitWords(SqlString TableName, SqlString ColumnName, SqlInt32 RowID, SqlString ObjectType, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Lookup.GetUnitWords(TableName, ColumnName, RowID, ObjectType, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetUnitWords(SqlString TableName, SqlString ColumnName, SqlInt32 RowID, SqlString ObjectType, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlLookup.GetUnitWords(TableName, ColumnName, RowID, ObjectType, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.GetUnitWords for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.GetUnitWords for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string ProcessWordElements(SqlString dataXml, SqlString userLogin, SqlString userProgram)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Lookup.ProcessWordElements(dataXml, userLogin, userProgram, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string ProcessWordElements(SqlString dataXml, SqlString userLogin, SqlString userProgram, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Lookup.ProcessWordElements(dataXml, userLogin, userProgram, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string ProcessWordElements(SqlString dataXml, SqlString userLogin, SqlString userProgram, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlLookup.ProcessWordElements(dataXml, userLogin, userProgram, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.ProcessWordElements for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.ProcessWordElements for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetWordElements(SqlInt32 PK_Word_List)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Lookup.GetWordElements(PK_Word_List, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetWordElements(SqlInt32 PK_Word_List, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Lookup.GetWordElements(PK_Word_List, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetWordElements(SqlInt32 PK_Word_List, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlLookup.GetWordElements(PK_Word_List, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.GetWordElements for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.GetWordElements for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetBreakersByBreakerSet(SqlInt32 PK_Word_BreakerSet)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Lookup.GetBreakersByBreakerSet(PK_Word_BreakerSet, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetBreakersByBreakerSet(SqlInt32 PK_Word_BreakerSet, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Lookup.GetBreakersByBreakerSet(PK_Word_BreakerSet, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetBreakersByBreakerSet(SqlInt32 PK_Word_BreakerSet, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlLookup.GetBreakersByBreakerSet(PK_Word_BreakerSet, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.GetBreakersByBreakerSet for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.GetBreakersByBreakerSet for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetBreakerSetByList(SqlInt32 PK_Word_List)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Lookup.GetBreakerSetByList(PK_Word_List, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetBreakerSetByList(SqlInt32 PK_Word_List, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Lookup.GetBreakerSetByList(PK_Word_List, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetBreakerSetByList(SqlInt32 PK_Word_List, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlLookup.GetBreakerSetByList(PK_Word_List, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.GetBreakerSetByList for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.GetBreakerSetByList for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetWordListsByListType(SqlInt32 PK_Word_ListType)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Lookup.GetWordListsByListType(PK_Word_ListType, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetWordListsByListType(SqlInt32 PK_Word_ListType, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Lookup.GetWordListsByListType(PK_Word_ListType, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetWordListsByListType(SqlInt32 PK_Word_ListType, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlLookup.GetWordListsByListType(PK_Word_ListType, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.GetWordListsByListType for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.GetWordListsByListType for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetListDetails(SqlInt32 PK_Word_List)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Lookup.GetListDetails(PK_Word_List, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetListDetails(SqlInt32 PK_Word_List, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Lookup.GetListDetails(PK_Word_List, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetListDetails(SqlInt32 PK_Word_List, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlLookup.GetListDetails(PK_Word_List, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.GetListDetails for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.GetListDetails for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetBreakersByList(SqlInt32 PK_Word_List)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Lookup.GetBreakersByList(PK_Word_List, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetBreakersByList(SqlInt32 PK_Word_List, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Lookup.GetBreakersByList(PK_Word_List, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetBreakersByList(SqlInt32 PK_Word_List, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlLookup.GetBreakersByList(PK_Word_List, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.GetBreakersByList for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.GetBreakersByList for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void DeleteNormalizationRules(SqlString IdsXml, SqlString UserId, SqlString ProgramName, out SqlInt32 RETURN_VALUE)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            Lookup.DeleteNormalizationRules(IdsXml, UserId, ProgramName, out RETURN_VALUE, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void DeleteNormalizationRules(SqlString IdsXml, SqlString UserId, SqlString ProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            Lookup.DeleteNormalizationRules(IdsXml, UserId, ProgramName, out RETURN_VALUE, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void DeleteNormalizationRules(SqlString IdsXml, SqlString UserId, SqlString ProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    SqlLookup.DeleteNormalizationRules(IdsXml, UserId, ProgramName, out RETURN_VALUE, connection, transaction);

                    break;

                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.DeleteNormalizationRules for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.DeleteNormalizationRules for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void UpdateNormalizationRule(SqlString dataXml, SqlString userLogin, SqlString userProgram, out SqlInt32 RETURN_VALUE)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            Lookup.UpdateNormalizationRule(dataXml, userLogin, userProgram, out RETURN_VALUE, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void UpdateNormalizationRule(SqlString dataXml, SqlString userLogin, SqlString userProgram, out SqlInt32 RETURN_VALUE, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            Lookup.UpdateNormalizationRule(dataXml, userLogin, userProgram, out RETURN_VALUE, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void UpdateNormalizationRule(SqlString dataXml, SqlString userLogin, SqlString userProgram, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    SqlLookup.UpdateNormalizationRule(dataXml, userLogin, userProgram, out RETURN_VALUE, connection, transaction);

                    break;

                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.UpdateNormalizationRule for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.UpdateNormalizationRule for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void PerformDQMNormalization(SqlInt32 FK_Jobservice, SqlXml xml, ref SqlInt32 Total_Count, ref SqlInt32 Success_Count, out SqlInt32 RETURN_VALUE)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            Lookup.PerformDQMNormalization(FK_Jobservice, xml, ref Total_Count, ref Success_Count, out RETURN_VALUE, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void PerformDQMNormalization(SqlInt32 FK_Jobservice, SqlXml xml, ref SqlInt32 Total_Count, ref SqlInt32 Success_Count, out SqlInt32 RETURN_VALUE, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            Lookup.PerformDQMNormalization(FK_Jobservice, xml, ref Total_Count, ref Success_Count, out RETURN_VALUE, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void PerformDQMNormalization(SqlInt32 FK_Jobservice, SqlXml xml, ref SqlInt32 Total_Count, ref SqlInt32 Success_Count, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    SqlLookup.PerformDQMNormalization(FK_Jobservice, xml, ref Total_Count, ref Success_Count, out RETURN_VALUE, connection, transaction);

                    break;

                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.PerformDQMNormalization for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.PerformDQMNormalization for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetTranslationMemory(SqlString OrigLocale, SqlString TransLocale, SqlString OrigText)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Lookup.GetTranslationMemory(OrigLocale, TransLocale, OrigText, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetTranslationMemory(SqlString OrigLocale, SqlString TransLocale, SqlString OrigText, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Lookup.GetTranslationMemory(OrigLocale, TransLocale, OrigText, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetTranslationMemory(SqlString OrigLocale, SqlString TransLocale, SqlString OrigText, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlLookup.GetTranslationMemory(OrigLocale, TransLocale, OrigText, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.GetTranslationMemory for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.GetTranslationMemory for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ProcessTranslationMemory(SqlString OrigLocale, SqlString TransLocale, SqlString OrigText, SqlString TransText, SqlString moduser, out SqlInt32 RETURN_VALUE)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            Lookup.ProcessTranslationMemory(OrigLocale, TransLocale, OrigText, TransText, moduser, out RETURN_VALUE, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ProcessTranslationMemory(SqlString OrigLocale, SqlString TransLocale, SqlString OrigText, SqlString TransText, SqlString moduser, out SqlInt32 RETURN_VALUE, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            Lookup.ProcessTranslationMemory(OrigLocale, TransLocale, OrigText, TransText, moduser, out RETURN_VALUE, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ProcessTranslationMemory(SqlString OrigLocale, SqlString TransLocale, SqlString OrigText, SqlString TransText, SqlString moduser, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    SqlLookup.ProcessTranslationMemory(OrigLocale, TransLocale, OrigText, TransText, moduser, out RETURN_VALUE, connection, transaction);

                    break;

                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.ProcessTranslationMemory for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.ProcessTranslationMemory for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetNormalizationRules()
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Lookup.GetNormalizationRules(connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetNormalizationRules(IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Lookup.GetNormalizationRules(connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetNormalizationRules(IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlLookup.GetNormalizationRules(connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.GetNormalizationRules for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.GetNormalizationRules for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetNormalizationRuleDetails(SqlInt32 PK_Word_Rule)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Lookup.GetNormalizationRuleDetails(PK_Word_Rule, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetNormalizationRuleDetails(SqlInt32 PK_Word_Rule, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Lookup.GetNormalizationRuleDetails(PK_Word_Rule, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetNormalizationRuleDetails(SqlInt32 PK_Word_Rule, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlLookup.GetNormalizationRuleDetails(PK_Word_Rule, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.GetNormalizationRuleDetails for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.GetNormalizationRuleDetails for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetBusinessRules(SqlInt32 FK_BusinessRule_RuleType)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Lookup.GetBusinessRules(FK_BusinessRule_RuleType, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetBusinessRules(SqlInt32 FK_BusinessRule_RuleType, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Lookup.GetBusinessRules(FK_BusinessRule_RuleType, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetBusinessRules(SqlInt32 FK_BusinessRule_RuleType, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlLookup.GetBusinessRules(FK_BusinessRule_RuleType, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.GetBusinessRules for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.GetBusinessRules for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetBusinessRuleDetails(SqlInt32 FK_BusinessRule_RuleSet)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Lookup.GetBusinessRuleDetails(FK_BusinessRule_RuleSet, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetBusinessRuleDetails(SqlInt32 FK_BusinessRule_RuleSet, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Lookup.GetBusinessRuleDetails(FK_BusinessRule_RuleSet, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetBusinessRuleDetails(SqlInt32 FK_BusinessRule_RuleSet, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlLookup.GetBusinessRuleDetails(FK_BusinessRule_RuleSet, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.GetBusinessRuleDetails for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.GetBusinessRuleDetails for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetNormalizationJobResults(SqlInt32 fk_JobServiceId)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Lookup.GetNormalizationJobResults(fk_JobServiceId, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetNormalizationJobResults(SqlInt32 fk_JobServiceId, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Lookup.GetNormalizationJobResults(fk_JobServiceId, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetNormalizationJobResults(SqlInt32 fk_JobServiceId, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlLookup.GetNormalizationJobResults(fk_JobServiceId, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Lookup.GetNormalizationJobResults for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Lookup.GetNormalizationJobResults for this provider: " + providerName);
            }
        }

    }
}
