
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
    public class CustomSecurity
    {
        /// <summary>
        /// Private constructor
        /// </summary>		
        private CustomSecurity()
        {
        }


        /// <summary>
        /// 
        /// </summary>
        public static void CalculateCustomSecurity_GetDT(SqlInt32 FK_Catalog, SqlString itemList, SqlString permissionLevel, SqlString vchrUserLogin, out SqlInt32 RETURN_VALUE)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            CustomSecurity.CalculateCustomSecurity_GetDT(FK_Catalog, itemList, permissionLevel, vchrUserLogin, out RETURN_VALUE, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CalculateCustomSecurity_GetDT(SqlInt32 FK_Catalog, SqlString itemList, SqlString permissionLevel, SqlString vchrUserLogin, out SqlInt32 RETURN_VALUE, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            CustomSecurity.CalculateCustomSecurity_GetDT(FK_Catalog, itemList, permissionLevel, vchrUserLogin, out RETURN_VALUE, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CalculateCustomSecurity_GetDT(SqlInt32 FK_Catalog, SqlString itemList, SqlString permissionLevel, SqlString vchrUserLogin, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    SqlCustomSecurity.CalculateCustomSecurity_GetDT(FK_Catalog, itemList, permissionLevel, vchrUserLogin, out RETURN_VALUE, connection, transaction);

                    break;

                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of CustomSecurity.CalculateCustomSecurity_GetDT for this provider: " + providerName);
                    throw new ApplicationException("No implementation of CustomSecurity.CalculateCustomSecurity_GetDT for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CalculateCustomSecurity_GetXML(SqlInt32 FK_Catalog, SqlString itemList, SqlString permissionLevel, SqlString vchrUserLogin, out SqlInt32 RETURN_VALUE)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            CustomSecurity.CalculateCustomSecurity_GetXML(FK_Catalog, itemList, permissionLevel, vchrUserLogin, out RETURN_VALUE, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CalculateCustomSecurity_GetXML(SqlInt32 FK_Catalog, SqlString itemList, SqlString permissionLevel, SqlString vchrUserLogin, out SqlInt32 RETURN_VALUE, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            CustomSecurity.CalculateCustomSecurity_GetXML(FK_Catalog, itemList, permissionLevel, vchrUserLogin, out RETURN_VALUE, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CalculateCustomSecurity_GetXML(SqlInt32 FK_Catalog, SqlString itemList, SqlString permissionLevel, SqlString vchrUserLogin, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    SqlCustomSecurity.CalculateCustomSecurity_GetXML(FK_Catalog, itemList, permissionLevel, vchrUserLogin, out RETURN_VALUE, connection, transaction);

                    break;

                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of CustomSecurity.CalculateCustomSecurity_GetXML for this provider: " + providerName);
                    throw new ApplicationException("No implementation of CustomSecurity.CalculateCustomSecurity_GetXML for this provider: " + providerName);
            }
        }

    }
}
