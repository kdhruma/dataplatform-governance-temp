
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
	public class Rules
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private Rules()
		{
		}


		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetBusinessRuleSetsByContext(SqlInt32 FK_Attribute, SqlInt32 FK_Node, SqlInt32 FK_Catalog)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Rules.GetBusinessRuleSetsByContext(FK_Attribute, FK_Node, FK_Catalog, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetBusinessRuleSetsByContext(SqlInt32 FK_Attribute, SqlInt32 FK_Node, SqlInt32 FK_Catalog, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Rules.GetBusinessRuleSetsByContext(FK_Attribute, FK_Node, FK_Catalog, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetBusinessRuleSetsByContext(SqlInt32 FK_Attribute, SqlInt32 FK_Node, SqlInt32 FK_Catalog, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlRules.GetBusinessRuleSetsByContext(FK_Attribute, FK_Node, FK_Catalog, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Rules.GetBusinessRuleSetsByContext for this provider: "+providerName);
					throw new ApplicationException("No implementation of Rules.GetBusinessRuleSetsByContext for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetBusinessRuleSet(SqlInt32 FK_BusinessRule_RuleSet)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Rules.GetBusinessRuleSet(FK_BusinessRule_RuleSet, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetBusinessRuleSet(SqlInt32 FK_BusinessRule_RuleSet, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Rules.GetBusinessRuleSet(FK_BusinessRule_RuleSet, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetBusinessRuleSet(SqlInt32 FK_BusinessRule_RuleSet, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlRules.GetBusinessRuleSet(FK_BusinessRule_RuleSet, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Rules.GetBusinessRuleSet for this provider: "+providerName);
					throw new ApplicationException("No implementation of Rules.GetBusinessRuleSet for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void AddUpdateBusinessRuleSet(SqlXml InputXML, SqlString ModUser, SqlString ModProgram, ref SqlString ReturnString)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Rules.AddUpdateBusinessRuleSet(InputXML, ModUser, ModProgram, ref ReturnString, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void AddUpdateBusinessRuleSet(SqlXml InputXML, SqlString ModUser, SqlString ModProgram, ref SqlString ReturnString, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Rules.AddUpdateBusinessRuleSet(InputXML, ModUser, ModProgram, ref ReturnString, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void AddUpdateBusinessRuleSet(SqlXml InputXML, SqlString ModUser, SqlString ModProgram, ref SqlString ReturnString, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlRules.AddUpdateBusinessRuleSet(InputXML, ModUser, ModProgram, ref ReturnString, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Rules.AddUpdateBusinessRuleSet for this provider: "+providerName);
					throw new ApplicationException("No implementation of Rules.AddUpdateBusinessRuleSet for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void AddUpdateCustomBusinessRuleSet(SqlXml InputXML, SqlString ModUser, SqlString ModProgram)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Rules.AddUpdateCustomBusinessRuleSet(InputXML, ModUser, ModProgram, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void AddUpdateCustomBusinessRuleSet(SqlXml InputXML, SqlString ModUser, SqlString ModProgram, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Rules.AddUpdateCustomBusinessRuleSet(InputXML, ModUser, ModProgram, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void AddUpdateCustomBusinessRuleSet(SqlXml InputXML, SqlString ModUser, SqlString ModProgram, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlRules.AddUpdateCustomBusinessRuleSet(InputXML, ModUser, ModProgram, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Rules.AddUpdateCustomBusinessRuleSet for this provider: "+providerName);
					throw new ApplicationException("No implementation of Rules.AddUpdateCustomBusinessRuleSet for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void CheckDuplicateBusinessRuleSet(SqlInt32 FK_Attribute, SqlInt32 FK_Catalog, SqlInt32 FK_Category, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Rules.CheckDuplicateBusinessRuleSet(FK_Attribute, FK_Catalog, FK_Category, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void CheckDuplicateBusinessRuleSet(SqlInt32 FK_Attribute, SqlInt32 FK_Catalog, SqlInt32 FK_Category, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Rules.CheckDuplicateBusinessRuleSet(FK_Attribute, FK_Catalog, FK_Category, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void CheckDuplicateBusinessRuleSet(SqlInt32 FK_Attribute, SqlInt32 FK_Catalog, SqlInt32 FK_Category, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlRules.CheckDuplicateBusinessRuleSet(FK_Attribute, FK_Catalog, FK_Category, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Rules.CheckDuplicateBusinessRuleSet for this provider: "+providerName);
					throw new ApplicationException("No implementation of Rules.CheckDuplicateBusinessRuleSet for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void GetRulesCount(SqlInt32 RuleSetID, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Rules.GetRulesCount(RuleSetID, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void GetRulesCount(SqlInt32 RuleSetID, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Rules.GetRulesCount(RuleSetID, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void GetRulesCount(SqlInt32 RuleSetID, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlRules.GetRulesCount(RuleSetID, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Rules.GetRulesCount for this provider: "+providerName);
					throw new ApplicationException("No implementation of Rules.GetRulesCount for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetBusinessRuleFromContext(SqlString ContextXml)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Rules.GetBusinessRuleFromContext(ContextXml, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetBusinessRuleFromContext(SqlString ContextXml, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Rules.GetBusinessRuleFromContext(ContextXml, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetBusinessRuleFromContext(SqlString ContextXml, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlRules.GetBusinessRuleFromContext(ContextXml, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Rules.GetBusinessRuleFromContext for this provider: "+providerName);
					throw new ApplicationException("No implementation of Rules.GetBusinessRuleFromContext for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetCustomBusinessRulesFromContext(SqlString ContextXml, SqlInt32 FK_BusinessRule_RuleType, SqlInt32 FK_BusinessRule_RuleSet)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Rules.GetCustomBusinessRulesFromContext(ContextXml, FK_BusinessRule_RuleType, FK_BusinessRule_RuleSet, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetCustomBusinessRulesFromContext(SqlString ContextXml, SqlInt32 FK_BusinessRule_RuleType, SqlInt32 FK_BusinessRule_RuleSet, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Rules.GetCustomBusinessRulesFromContext(ContextXml, FK_BusinessRule_RuleType, FK_BusinessRule_RuleSet, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetCustomBusinessRulesFromContext(SqlString ContextXml, SqlInt32 FK_BusinessRule_RuleType, SqlInt32 FK_BusinessRule_RuleSet, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlRules.GetCustomBusinessRulesFromContext(ContextXml, FK_BusinessRule_RuleType, FK_BusinessRule_RuleSet, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Rules.GetCustomBusinessRulesFromContext for this provider: "+providerName);
					throw new ApplicationException("No implementation of Rules.GetCustomBusinessRulesFromContext for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetRuleSetTestResult(SqlInt32 FK_JobService, SqlInt32 FK_Locale, SqlInt32 Int_BusinessRule_RuleSet, SqlString CNodeList, SqlBoolean Bit_TestMode)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Rules.GetRuleSetTestResult(FK_JobService, FK_Locale, Int_BusinessRule_RuleSet, CNodeList, Bit_TestMode, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetRuleSetTestResult(SqlInt32 FK_JobService, SqlInt32 FK_Locale, SqlInt32 Int_BusinessRule_RuleSet, SqlString CNodeList, SqlBoolean Bit_TestMode, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Rules.GetRuleSetTestResult(FK_JobService, FK_Locale, Int_BusinessRule_RuleSet, CNodeList, Bit_TestMode, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetRuleSetTestResult(SqlInt32 FK_JobService, SqlInt32 FK_Locale, SqlInt32 Int_BusinessRule_RuleSet, SqlString CNodeList, SqlBoolean Bit_TestMode, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlRules.GetRuleSetTestResult(FK_JobService, FK_Locale, Int_BusinessRule_RuleSet, CNodeList, Bit_TestMode, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Rules.GetRuleSetTestResult for this provider: "+providerName);
					throw new ApplicationException("No implementation of Rules.GetRuleSetTestResult for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateRuleSetRuleActiveFlag(SqlXml xml, SqlString ModUser, SqlString ModProgram)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Rules.UpdateRuleSetRuleActiveFlag(xml, ModUser, ModProgram, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateRuleSetRuleActiveFlag(SqlXml xml, SqlString ModUser, SqlString ModProgram, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Rules.UpdateRuleSetRuleActiveFlag(xml, ModUser, ModProgram, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateRuleSetRuleActiveFlag(SqlXml xml, SqlString ModUser, SqlString ModProgram, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlRules.UpdateRuleSetRuleActiveFlag(xml, ModUser, ModProgram, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Rules.UpdateRuleSetRuleActiveFlag for this provider: "+providerName);
					throw new ApplicationException("No implementation of Rules.UpdateRuleSetRuleActiveFlag for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeletePDRProfile(SqlString nvrchprofileID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Rules.DeletePDRProfile(nvrchprofileID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeletePDRProfile(SqlString nvrchprofileID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Rules.DeletePDRProfile(nvrchprofileID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeletePDRProfile(SqlString nvrchprofileID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlRules.DeletePDRProfile(nvrchprofileID, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Rules.DeletePDRProfile for this provider: "+providerName);
					throw new ApplicationException("No implementation of Rules.DeletePDRProfile for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetSourceAttributes()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Rules.GetSourceAttributes(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetSourceAttributes(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Rules.GetSourceAttributes(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetSourceAttributes(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlRules.GetSourceAttributes(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Rules.GetSourceAttributes for this provider: "+providerName);
					throw new ApplicationException("No implementation of Rules.GetSourceAttributes for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetViewContextDetail(SqlInt32 ContextID, SqlString LoginUser)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Rules.GetViewContextDetail(ContextID, LoginUser, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetViewContextDetail(SqlInt32 ContextID, SqlString LoginUser, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Rules.GetViewContextDetail(ContextID, LoginUser, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetViewContextDetail(SqlInt32 ContextID, SqlString LoginUser, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlRules.GetViewContextDetail(ContextID, LoginUser, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Rules.GetViewContextDetail for this provider: "+providerName);
					throw new ApplicationException("No implementation of Rules.GetViewContextDetail for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetViewContext(SqlInt32 ViewID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Rules.GetViewContext(ViewID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetViewContext(SqlInt32 ViewID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Rules.GetViewContext(ViewID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetViewContext(SqlInt32 ViewID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlRules.GetViewContext(ViewID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Rules.GetViewContext for this provider: "+providerName);
					throw new ApplicationException("No implementation of Rules.GetViewContext for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetRuleViewAttributes(SqlInt64 EntityID, SqlInt32 BusinessRule_Rule)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Rules.GetRuleViewAttributes(EntityID, BusinessRule_Rule, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
        public static DataTable GetRuleViewAttributes(SqlInt64 EntityID, SqlInt32 BusinessRule_Rule, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Rules.GetRuleViewAttributes(EntityID, BusinessRule_Rule, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
        public static DataTable GetRuleViewAttributes(SqlInt64 EntityID, SqlInt32 BusinessRule_Rule, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlRules.GetRuleViewAttributes(EntityID, BusinessRule_Rule, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Rules.GetRuleViewAttributes for this provider: "+providerName);
					throw new ApplicationException("No implementation of Rules.GetRuleViewAttributes for this provider: "+providerName);
			}
		}

	}
}		
