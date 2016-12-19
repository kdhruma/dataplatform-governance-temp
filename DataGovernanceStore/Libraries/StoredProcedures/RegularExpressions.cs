
using System;
using System.Xml;
using System.Data;
using System.Text;
using System.Data.SqlTypes;
using System.Configuration;
using MDM.Utility;

namespace Riversand.StoredProcedures
{
	/// <summary>
	/// 
	/// </summary>
	public class RegularExpressions
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private RegularExpressions()
		{
		}
		
		
		/// <summary>
        /// 
        /// </summary>
		public static void DeleteAttributeRegEx(SqlInt32 PK_AttributeRegularExpression)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			RegularExpressions.DeleteAttributeRegEx(PK_AttributeRegularExpression, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteAttributeRegEx(SqlInt32 PK_AttributeRegularExpression, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			RegularExpressions.DeleteAttributeRegEx(PK_AttributeRegularExpression, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteAttributeRegEx(SqlInt32 PK_AttributeRegularExpression, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlRegularExpressions.DeleteAttributeRegEx(PK_AttributeRegularExpression, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of RegularExpressions.DeleteAttributeRegEx for this provider: "+providerName);
					throw new ApplicationException("No implementation of RegularExpressions.DeleteAttributeRegEx for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAttributeRegEx(SqlInt32 FK_Attribute)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return RegularExpressions.GetAttributeRegEx(FK_Attribute, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAttributeRegEx(SqlInt32 FK_Attribute, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return RegularExpressions.GetAttributeRegEx(FK_Attribute, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAttributeRegEx(SqlInt32 FK_Attribute, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlRegularExpressions.GetAttributeRegEx(FK_Attribute, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of RegularExpressions.GetAttributeRegEx for this provider: "+providerName);
					throw new ApplicationException("No implementation of RegularExpressions.GetAttributeRegEx for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void CreateAttributeRegEx(SqlInt32 FK_Attribute, SqlString Expression, SqlInt32 Priority)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			RegularExpressions.CreateAttributeRegEx(FK_Attribute, Expression, Priority, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void CreateAttributeRegEx(SqlInt32 FK_Attribute, SqlString Expression, SqlInt32 Priority, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			RegularExpressions.CreateAttributeRegEx(FK_Attribute, Expression, Priority, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void CreateAttributeRegEx(SqlInt32 FK_Attribute, SqlString Expression, SqlInt32 Priority, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlRegularExpressions.CreateAttributeRegEx(FK_Attribute, Expression, Priority, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of RegularExpressions.CreateAttributeRegEx for this provider: "+providerName);
					throw new ApplicationException("No implementation of RegularExpressions.CreateAttributeRegEx for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateAttributeRegEx(SqlInt32 PK_AttributeRegularExpression, SqlInt32 FK_Attribute, SqlString Expression, SqlInt32 Priority)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			RegularExpressions.UpdateAttributeRegEx(PK_AttributeRegularExpression, FK_Attribute, Expression, Priority, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateAttributeRegEx(SqlInt32 PK_AttributeRegularExpression, SqlInt32 FK_Attribute, SqlString Expression, SqlInt32 Priority, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			RegularExpressions.UpdateAttributeRegEx(PK_AttributeRegularExpression, FK_Attribute, Expression, Priority, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateAttributeRegEx(SqlInt32 PK_AttributeRegularExpression, SqlInt32 FK_Attribute, SqlString Expression, SqlInt32 Priority, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlRegularExpressions.UpdateAttributeRegEx(PK_AttributeRegularExpression, FK_Attribute, Expression, Priority, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of RegularExpressions.UpdateAttributeRegEx for this provider: "+providerName);
					throw new ApplicationException("No implementation of RegularExpressions.UpdateAttributeRegEx for this provider: "+providerName);
			}
		}

	}
}		
