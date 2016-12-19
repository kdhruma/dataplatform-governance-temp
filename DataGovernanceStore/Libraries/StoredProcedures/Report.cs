
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
	public class Report
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private Report()
		{
		}
		
		
		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAllLocales()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Report.GetAllLocales(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAllLocales(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Report.GetAllLocales(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAllLocales(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlReport.GetAllLocales(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Report.GetAllLocales for this provider: "+providerName);
					throw new ApplicationException("No implementation of Report.GetAllLocales for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetReports(SqlString whereClause)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Report.GetReports(whereClause, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetReports(SqlString whereClause, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Report.GetReports(whereClause, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetReports(SqlString whereClause, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlReport.GetReports(whereClause, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Report.GetReports for this provider: "+providerName);
					throw new ApplicationException("No implementation of Report.GetReports for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetNPI(SqlInt32 FK_Catalog, SqlString ReadFromProduction, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Report.GetNPI(FK_Catalog, ReadFromProduction, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetNPI(SqlInt32 FK_Catalog, SqlString ReadFromProduction, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Report.GetNPI(FK_Catalog, ReadFromProduction, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetNPI(SqlInt32 FK_Catalog, SqlString ReadFromProduction, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlReport.GetNPI(FK_Catalog, ReadFromProduction, vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Report.GetNPI for this provider: "+providerName);
					throw new ApplicationException("No implementation of Report.GetNPI for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAttributeChange(SqlInt32 FK_Catalog, SqlString ReadFromProduction, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Report.GetAttributeChange(FK_Catalog, ReadFromProduction, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAttributeChange(SqlInt32 FK_Catalog, SqlString ReadFromProduction, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Report.GetAttributeChange(FK_Catalog, ReadFromProduction, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAttributeChange(SqlInt32 FK_Catalog, SqlString ReadFromProduction, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlReport.GetAttributeChange(FK_Catalog, ReadFromProduction, vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Report.GetAttributeChange for this provider: "+providerName);
					throw new ApplicationException("No implementation of Report.GetAttributeChange for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetFunctionalLocationStatusReport(SqlInt32 Fk_Org, SqlInt32 FK_Catalog, SqlString ReadFromProduction, SqlString vchrUserLogin, SqlString EquipmentStatus)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Report.GetFunctionalLocationStatusReport(Fk_Org, FK_Catalog, ReadFromProduction, vchrUserLogin, EquipmentStatus, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetFunctionalLocationStatusReport(SqlInt32 Fk_Org, SqlInt32 FK_Catalog, SqlString ReadFromProduction, SqlString vchrUserLogin, SqlString EquipmentStatus, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Report.GetFunctionalLocationStatusReport(Fk_Org, FK_Catalog, ReadFromProduction, vchrUserLogin, EquipmentStatus, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetFunctionalLocationStatusReport(SqlInt32 Fk_Org, SqlInt32 FK_Catalog, SqlString ReadFromProduction, SqlString vchrUserLogin, SqlString EquipmentStatus, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlReport.GetFunctionalLocationStatusReport(Fk_Org, FK_Catalog, ReadFromProduction, vchrUserLogin, EquipmentStatus, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Report.GetFunctionalLocationStatusReport for this provider: "+providerName);
					throw new ApplicationException("No implementation of Report.GetFunctionalLocationStatusReport for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetBOMStatusReport(SqlInt32 Fk_Org, SqlInt32 FK_Catalog, SqlString ReadFromProduction, SqlString vchrUserLogin, SqlString BOMStatus)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Report.GetBOMStatusReport(Fk_Org, FK_Catalog, ReadFromProduction, vchrUserLogin, BOMStatus, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetBOMStatusReport(SqlInt32 Fk_Org, SqlInt32 FK_Catalog, SqlString ReadFromProduction, SqlString vchrUserLogin, SqlString BOMStatus, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Report.GetBOMStatusReport(Fk_Org, FK_Catalog, ReadFromProduction, vchrUserLogin, BOMStatus, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetBOMStatusReport(SqlInt32 Fk_Org, SqlInt32 FK_Catalog, SqlString ReadFromProduction, SqlString vchrUserLogin, SqlString BOMStatus, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlReport.GetBOMStatusReport(Fk_Org, FK_Catalog, ReadFromProduction, vchrUserLogin, BOMStatus, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Report.GetBOMStatusReport for this provider: "+providerName);
					throw new ApplicationException("No implementation of Report.GetBOMStatusReport for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetWFStatus(SqlInt32 WorkFlowId, SqlString WorkFlowName, SqlString ReadFromProduction)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Report.GetWFStatus(WorkFlowId, WorkFlowName, ReadFromProduction, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetWFStatus(SqlInt32 WorkFlowId, SqlString WorkFlowName, SqlString ReadFromProduction, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Report.GetWFStatus(WorkFlowId, WorkFlowName, ReadFromProduction, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetWFStatus(SqlInt32 WorkFlowId, SqlString WorkFlowName, SqlString ReadFromProduction, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlReport.GetWFStatus(WorkFlowId, WorkFlowName, ReadFromProduction, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Report.GetWFStatus for this provider: "+providerName);
					throw new ApplicationException("No implementation of Report.GetWFStatus for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetBOMCompletenessReport(SqlInt32 FK_Org, SqlInt32 FK_Catalog, SqlString ChartID, SqlString BOMStatus, SqlString ReadFromProduction)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Report.GetBOMCompletenessReport(FK_Org, FK_Catalog, ChartID, BOMStatus, ReadFromProduction, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetBOMCompletenessReport(SqlInt32 FK_Org, SqlInt32 FK_Catalog, SqlString ChartID, SqlString BOMStatus, SqlString ReadFromProduction, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Report.GetBOMCompletenessReport(FK_Org, FK_Catalog, ChartID, BOMStatus, ReadFromProduction, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetBOMCompletenessReport(SqlInt32 FK_Org, SqlInt32 FK_Catalog, SqlString ChartID, SqlString BOMStatus, SqlString ReadFromProduction, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlReport.GetBOMCompletenessReport(FK_Org, FK_Catalog, ChartID, BOMStatus, ReadFromProduction, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Report.GetBOMCompletenessReport for this provider: "+providerName);
					throw new ApplicationException("No implementation of Report.GetBOMCompletenessReport for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable Get_BOMWhereNotUsed(SqlInt32 FK_SupplierStagingCatalog, SqlInt32 FK_EquipmentStagingCatalog, SqlString vchrUserLogin, SqlString BOMStatus, SqlString ReadFromProduction)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Report.Get_BOMWhereNotUsed(FK_SupplierStagingCatalog, FK_EquipmentStagingCatalog, vchrUserLogin, BOMStatus, ReadFromProduction, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable Get_BOMWhereNotUsed(SqlInt32 FK_SupplierStagingCatalog, SqlInt32 FK_EquipmentStagingCatalog, SqlString vchrUserLogin, SqlString BOMStatus, SqlString ReadFromProduction, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Report.Get_BOMWhereNotUsed(FK_SupplierStagingCatalog, FK_EquipmentStagingCatalog, vchrUserLogin, BOMStatus, ReadFromProduction, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable Get_BOMWhereNotUsed(SqlInt32 FK_SupplierStagingCatalog, SqlInt32 FK_EquipmentStagingCatalog, SqlString vchrUserLogin, SqlString BOMStatus, SqlString ReadFromProduction, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlReport.Get_BOMWhereNotUsed(FK_SupplierStagingCatalog, FK_EquipmentStagingCatalog, vchrUserLogin, BOMStatus, ReadFromProduction, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Report.Get_BOMWhereNotUsed for this provider: "+providerName);
					throw new ApplicationException("No implementation of Report.Get_BOMWhereNotUsed for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable Get_PartRSPLWhereNotUsed(SqlInt32 Fk_Org, SqlInt32 FK_PlantCatalog, SqlString vchrUserLogin, SqlString Status, SqlString ReadFromProduction)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Report.Get_PartRSPLWhereNotUsed(Fk_Org, FK_PlantCatalog, vchrUserLogin, Status, ReadFromProduction, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable Get_PartRSPLWhereNotUsed(SqlInt32 Fk_Org, SqlInt32 FK_PlantCatalog, SqlString vchrUserLogin, SqlString Status, SqlString ReadFromProduction, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Report.Get_PartRSPLWhereNotUsed(Fk_Org, FK_PlantCatalog, vchrUserLogin, Status, ReadFromProduction, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable Get_PartRSPLWhereNotUsed(SqlInt32 Fk_Org, SqlInt32 FK_PlantCatalog, SqlString vchrUserLogin, SqlString Status, SqlString ReadFromProduction, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlReport.Get_PartRSPLWhereNotUsed(Fk_Org, FK_PlantCatalog, vchrUserLogin, Status, ReadFromProduction, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Report.Get_PartRSPLWhereNotUsed for this provider: "+providerName);
					throw new ApplicationException("No implementation of Report.Get_PartRSPLWhereNotUsed for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetAttributeUsage(SqlInt32 FK_Attribute, SqlInt32 FK_Catalog, SqlInt32 FK_Category)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Report.GetAttributeUsage(FK_Attribute, FK_Catalog, FK_Category, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetAttributeUsage(SqlInt32 FK_Attribute, SqlInt32 FK_Catalog, SqlInt32 FK_Category, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Report.GetAttributeUsage(FK_Attribute, FK_Catalog, FK_Category, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetAttributeUsage(SqlInt32 FK_Attribute, SqlInt32 FK_Catalog, SqlInt32 FK_Category, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlReport.GetAttributeUsage(FK_Attribute, FK_Catalog, FK_Category, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Report.GetAttributeUsage for this provider: "+providerName);
					throw new ApplicationException("No implementation of Report.GetAttributeUsage for this provider: "+providerName);
			}
		}

	}
}		
