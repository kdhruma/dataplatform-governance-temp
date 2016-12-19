
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
	public class Catalog
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private Catalog()
		{
		}
		
		/// <summary>
        /// 
        /// </summary>
		public static DataTable DQMSearch(SqlInt32 FK_Catalog, SqlInt32 PK_Application_Config, SqlString vchrUserLogin, SqlInt32 userRole, SqlString dqmSearchXML, SqlString keyWordSearch, ref SqlString totalCount)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.DQMSearch(FK_Catalog, PK_Application_Config, vchrUserLogin, userRole, dqmSearchXML, keyWordSearch, ref totalCount, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable DQMSearch(SqlInt32 FK_Catalog, SqlInt32 PK_Application_Config, SqlString vchrUserLogin, SqlInt32 userRole, SqlString dqmSearchXML, SqlString keyWordSearch, ref SqlString totalCount, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.DQMSearch(FK_Catalog, PK_Application_Config, vchrUserLogin, userRole, dqmSearchXML, keyWordSearch, ref totalCount, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable DQMSearch(SqlInt32 FK_Catalog, SqlInt32 PK_Application_Config, SqlString vchrUserLogin, SqlInt32 userRole, SqlString dqmSearchXML, SqlString keyWordSearch, ref SqlString totalCount, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.DQMSearch(FK_Catalog, PK_Application_Config, vchrUserLogin, userRole, dqmSearchXML, keyWordSearch, ref totalCount, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.DQMSearch for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.DQMSearch for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GuidedSearch(SqlInt32 intCatalogID, SqlString vchrLoadCategory, SqlString vchrLoadType, SqlString vchrCurrentAssignmentStatus, SqlInt32 FK_Status, SqlString FK_Attributes, ref SqlString totalCount, SqlInt32 DisplayRows)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GuidedSearch(intCatalogID, vchrLoadCategory, vchrLoadType, vchrCurrentAssignmentStatus, FK_Status, FK_Attributes, ref totalCount, DisplayRows, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GuidedSearch(SqlInt32 intCatalogID, SqlString vchrLoadCategory, SqlString vchrLoadType, SqlString vchrCurrentAssignmentStatus, SqlInt32 FK_Status, SqlString FK_Attributes, ref SqlString totalCount, SqlInt32 DisplayRows, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GuidedSearch(intCatalogID, vchrLoadCategory, vchrLoadType, vchrCurrentAssignmentStatus, FK_Status, FK_Attributes, ref totalCount, DisplayRows, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GuidedSearch(SqlInt32 intCatalogID, SqlString vchrLoadCategory, SqlString vchrLoadType, SqlString vchrCurrentAssignmentStatus, SqlInt32 FK_Status, SqlString FK_Attributes, ref SqlString totalCount, SqlInt32 DisplayRows, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GuidedSearch(intCatalogID, vchrLoadCategory, vchrLoadType, vchrCurrentAssignmentStatus, FK_Status, FK_Attributes, ref totalCount, DisplayRows, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GuidedSearch for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GuidedSearch for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetLoadsGuidedSearchXML(SqlInt32 CatalogID, SqlString vchrLoadType, SqlInt32 PK_Load, SqlString vchrLoadsList, SqlString vchrSearchString, SqlString vchrUserLogin, SqlString vchrUserRole, SqlInt32 intCountFrom, SqlInt32 intCountTo)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetLoadsGuidedSearchXML(CatalogID, vchrLoadType, PK_Load, vchrLoadsList, vchrSearchString, vchrUserLogin, vchrUserRole, intCountFrom, intCountTo, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetLoadsGuidedSearchXML(SqlInt32 CatalogID, SqlString vchrLoadType, SqlInt32 PK_Load, SqlString vchrLoadsList, SqlString vchrSearchString, SqlString vchrUserLogin, SqlString vchrUserRole, SqlInt32 intCountFrom, SqlInt32 intCountTo, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetLoadsGuidedSearchXML(CatalogID, vchrLoadType, PK_Load, vchrLoadsList, vchrSearchString, vchrUserLogin, vchrUserRole, intCountFrom, intCountTo, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetLoadsGuidedSearchXML(SqlInt32 CatalogID, SqlString vchrLoadType, SqlInt32 PK_Load, SqlString vchrLoadsList, SqlString vchrSearchString, SqlString vchrUserLogin, SqlString vchrUserRole, SqlInt32 intCountFrom, SqlInt32 intCountTo, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetLoadsGuidedSearchXML(CatalogID, vchrLoadType, PK_Load, vchrLoadsList, vchrSearchString, vchrUserLogin, vchrUserRole, intCountFrom, intCountTo, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetLoadsGuidedSearchXML for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetLoadsGuidedSearchXML for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetRelationshipTypeHierarchy(SqlInt32 FK_Catalog, SqlInt32 FK_RelationshipType_Top, SqlInt32 MaxLevel)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetRelationshipTypeHierarchy(FK_Catalog, FK_RelationshipType_Top, MaxLevel, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetRelationshipTypeHierarchy(SqlInt32 FK_Catalog, SqlInt32 FK_RelationshipType_Top, SqlInt32 MaxLevel, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetRelationshipTypeHierarchy(FK_Catalog, FK_RelationshipType_Top, MaxLevel, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetRelationshipTypeHierarchy(SqlInt32 FK_Catalog, SqlInt32 FK_RelationshipType_Top, SqlInt32 MaxLevel, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetRelationshipTypeHierarchy(FK_Catalog, FK_RelationshipType_Top, MaxLevel, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetRelationshipTypeHierarchy for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetRelationshipTypeHierarchy for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string ExtractCatalogByIDLocalRel(SqlInt32 intExtSystemID, SqlString txtXML, SqlString vchrRelAttrList)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.ExtractCatalogByIDLocalRel(intExtSystemID, txtXML, vchrRelAttrList, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string ExtractCatalogByIDLocalRel(SqlInt32 intExtSystemID, SqlString txtXML, SqlString vchrRelAttrList, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.ExtractCatalogByIDLocalRel(intExtSystemID, txtXML, vchrRelAttrList, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string ExtractCatalogByIDLocalRel(SqlInt32 intExtSystemID, SqlString txtXML, SqlString vchrRelAttrList, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.ExtractCatalogByIDLocalRel(intExtSystemID, txtXML, vchrRelAttrList, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.ExtractCatalogByIDLocalRel for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.ExtractCatalogByIDLocalRel for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string ExtractBulkAttributeMetadata(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlString txtXML, SqlBoolean bitUseDraftTax, SqlInt32 localeId, SqlBoolean ignoreComplexAttributes)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
            return Catalog.ExtractBulkAttributeMetadata(vchrTargetUserLogin, vchrUserLogin, txtXML, bitUseDraftTax, localeId, connection, transaction, ignoreComplexAttributes);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string ExtractBulkAttributeMetadata(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlString txtXML, SqlBoolean bitUseDraftTax, SqlInt32 localeId, IDbConnection connection)
		{
			IDbTransaction transaction = null;
            return Catalog.ExtractBulkAttributeMetadata(vchrTargetUserLogin, vchrUserLogin, txtXML, bitUseDraftTax, localeId, connection, transaction, SqlBoolean.True);
		}

		/// <summary>
        /// 
        /// </summary>
        public static string ExtractBulkAttributeMetadata(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlString txtXML, SqlBoolean bitUseDraftTax, SqlInt32 localeId, IDbConnection connection, IDbTransaction transaction, SqlBoolean ignoreComplexAttributes)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.ExtractBulkAttributeMetadata(vchrTargetUserLogin, vchrUserLogin, txtXML, bitUseDraftTax, localeId, connection, transaction,ignoreComplexAttributes);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.ExtractBulkAttributeMetadata for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.ExtractBulkAttributeMetadata for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string ExtractBulkAttributeMetadataRel(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlString txtXML, SqlBoolean bitUseDraftTax, SqlInt32 localeId)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.ExtractBulkAttributeMetadataRel(vchrTargetUserLogin, vchrUserLogin, txtXML, bitUseDraftTax,localeId, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
        public static string ExtractBulkAttributeMetadataRel(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlString txtXML, SqlBoolean bitUseDraftTax, SqlInt32 localeId, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.ExtractBulkAttributeMetadataRel(vchrTargetUserLogin, vchrUserLogin, txtXML, bitUseDraftTax,localeId, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
        public static string ExtractBulkAttributeMetadataRel(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlString txtXML, SqlBoolean bitUseDraftTax, SqlInt32 localeId, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.ExtractBulkAttributeMetadataRel(vchrTargetUserLogin, vchrUserLogin, txtXML, bitUseDraftTax,localeId, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.ExtractBulkAttributeMetadataRel for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.ExtractBulkAttributeMetadataRel for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string ExtractAttributes(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlString txtXML, SqlBoolean bitUseDraftTax)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.ExtractAttributes(vchrTargetUserLogin, vchrUserLogin, txtXML, bitUseDraftTax, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string ExtractAttributes(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlString txtXML, SqlBoolean bitUseDraftTax, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.ExtractAttributes(vchrTargetUserLogin, vchrUserLogin, txtXML, bitUseDraftTax, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string ExtractAttributes(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlString txtXML, SqlBoolean bitUseDraftTax, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.ExtractAttributes(vchrTargetUserLogin, vchrUserLogin, txtXML, bitUseDraftTax, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.ExtractAttributes for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.ExtractAttributes for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetCategoryAttributeMap(SqlInt32 intCategoryID, SqlInt32 intCatalogID, SqlInt32 intLocaleID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetCategoryAttributeMap(intCategoryID, intCatalogID, intLocaleID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetCategoryAttributeMap(SqlInt32 intCategoryID, SqlInt32 intCatalogID, SqlInt32 intLocaleID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetCategoryAttributeMap(intCategoryID, intCatalogID, intLocaleID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetCategoryAttributeMap(SqlInt32 intCategoryID, SqlInt32 intCatalogID, SqlInt32 intLocaleID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetCategoryAttributeMap(intCategoryID, intCatalogID, intLocaleID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetCategoryAttributeMap for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetCategoryAttributeMap for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetUserVisibleCatalogsDT(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Org, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 PK_Catalog, SqlBoolean IncludeTaxonomy, SqlBoolean IncludeDynamicTaxonomy, SqlBoolean IncludeCatalog, SqlBoolean IncludeView, SqlBoolean IncludeProduction, SqlBoolean IncludeDraft)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetUserVisibleCatalogsDT(vchrTargetUserLogin, vchrUserLogin, FK_Org, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, PK_Catalog, IncludeTaxonomy, IncludeDynamicTaxonomy, IncludeCatalog, IncludeView, IncludeProduction, IncludeDraft, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetUserVisibleCatalogsDT(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Org, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 PK_Catalog, SqlBoolean IncludeTaxonomy, SqlBoolean IncludeDynamicTaxonomy, SqlBoolean IncludeCatalog, SqlBoolean IncludeView, SqlBoolean IncludeProduction, SqlBoolean IncludeDraft, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetUserVisibleCatalogsDT(vchrTargetUserLogin, vchrUserLogin, FK_Org, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, PK_Catalog, IncludeTaxonomy, IncludeDynamicTaxonomy, IncludeCatalog, IncludeView, IncludeProduction, IncludeDraft, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetUserVisibleCatalogsDT(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Org, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 PK_Catalog, SqlBoolean IncludeTaxonomy, SqlBoolean IncludeDynamicTaxonomy, SqlBoolean IncludeCatalog, SqlBoolean IncludeView, SqlBoolean IncludeProduction, SqlBoolean IncludeDraft, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetUserVisibleCatalogsDT(vchrTargetUserLogin, vchrUserLogin, FK_Org, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, PK_Catalog, IncludeTaxonomy, IncludeDynamicTaxonomy, IncludeCatalog, IncludeView, IncludeProduction, IncludeDraft, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetUserVisibleCatalogsDT for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetUserVisibleCatalogsDT for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCharacteristicTemplate(SqlInt32 intCategoryID, SqlInt32 intCatalogID, SqlInt32 intLocaleID, SqlBoolean bitUseDraftTax, SqlInt32 bitUsesChilds, SqlInt32 intOrgID, SqlBoolean ExcludeSearchable)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetCharacteristicTemplate(intCategoryID, intCatalogID, intLocaleID, bitUseDraftTax, bitUsesChilds, intOrgID, ExcludeSearchable, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCharacteristicTemplate(SqlInt32 intCategoryID, SqlInt32 intCatalogID, SqlInt32 intLocaleID, SqlBoolean bitUseDraftTax, SqlInt32 bitUsesChilds, SqlInt32 intOrgID, SqlBoolean ExcludeSearchable, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetCharacteristicTemplate(intCategoryID, intCatalogID, intLocaleID, bitUseDraftTax, bitUsesChilds, intOrgID, ExcludeSearchable, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCharacteristicTemplate(SqlInt32 intCategoryID, SqlInt32 intCatalogID, SqlInt32 intLocaleID, SqlBoolean bitUseDraftTax, SqlInt32 bitUsesChilds, SqlInt32 intOrgID, SqlBoolean ExcludeSearchable, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetCharacteristicTemplate(intCategoryID, intCatalogID, intLocaleID, bitUseDraftTax, bitUsesChilds, intOrgID, ExcludeSearchable, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetCharacteristicTemplate for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetCharacteristicTemplate for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetCharacteristicTemplateDT(SqlInt32 intCategoryID, SqlInt32 intLocaleID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetCharacteristicTemplateDT(intCategoryID, intLocaleID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetCharacteristicTemplateDT(SqlInt32 intCategoryID, SqlInt32 intLocaleID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetCharacteristicTemplateDT(intCategoryID, intLocaleID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetCharacteristicTemplateDT(SqlInt32 intCategoryID, SqlInt32 intLocaleID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetCharacteristicTemplateDT(intCategoryID, intLocaleID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetCharacteristicTemplateDT for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetCharacteristicTemplateDT for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetUserVisibleCatalogs(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Org, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 PK_Catalog, SqlBoolean IncludeTaxonomy, SqlBoolean IncludeDynamicTaxonomy, SqlBoolean IncludeCatalog, SqlBoolean IncludeView, SqlBoolean IncludeProduction, SqlBoolean IncludeDraft)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetUserVisibleCatalogs(vchrTargetUserLogin, vchrUserLogin, FK_Org, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, PK_Catalog, IncludeTaxonomy, IncludeDynamicTaxonomy, IncludeCatalog, IncludeView, IncludeProduction, IncludeDraft, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetUserVisibleCatalogs(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Org, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 PK_Catalog, SqlBoolean IncludeTaxonomy, SqlBoolean IncludeDynamicTaxonomy, SqlBoolean IncludeCatalog, SqlBoolean IncludeView, SqlBoolean IncludeProduction, SqlBoolean IncludeDraft, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetUserVisibleCatalogs(vchrTargetUserLogin, vchrUserLogin, FK_Org, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, PK_Catalog, IncludeTaxonomy, IncludeDynamicTaxonomy, IncludeCatalog, IncludeView, IncludeProduction, IncludeDraft, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetUserVisibleCatalogs(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Org, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 PK_Catalog, SqlBoolean IncludeTaxonomy, SqlBoolean IncludeDynamicTaxonomy, SqlBoolean IncludeCatalog, SqlBoolean IncludeView, SqlBoolean IncludeProduction, SqlBoolean IncludeDraft, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetUserVisibleCatalogs(vchrTargetUserLogin, vchrUserLogin, FK_Org, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, PK_Catalog, IncludeTaxonomy, IncludeDynamicTaxonomy, IncludeCatalog, IncludeView, IncludeProduction, IncludeDraft, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetUserVisibleCatalogs for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetUserVisibleCatalogs for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCatalogPermissionsByOrg(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Org, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 PK_Catalog, SqlBoolean IncludeTaxonomy, SqlBoolean IncludeDynamicTaxonomy, SqlBoolean IncludeCatalog, SqlBoolean IncludeView, SqlBoolean IncludeProduction, SqlBoolean IncludeDraft)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetCatalogPermissionsByOrg(vchrTargetUserLogin, vchrUserLogin, FK_Org, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, PK_Catalog, IncludeTaxonomy, IncludeDynamicTaxonomy, IncludeCatalog, IncludeView, IncludeProduction, IncludeDraft, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCatalogPermissionsByOrg(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Org, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 PK_Catalog, SqlBoolean IncludeTaxonomy, SqlBoolean IncludeDynamicTaxonomy, SqlBoolean IncludeCatalog, SqlBoolean IncludeView, SqlBoolean IncludeProduction, SqlBoolean IncludeDraft, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetCatalogPermissionsByOrg(vchrTargetUserLogin, vchrUserLogin, FK_Org, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, PK_Catalog, IncludeTaxonomy, IncludeDynamicTaxonomy, IncludeCatalog, IncludeView, IncludeProduction, IncludeDraft, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCatalogPermissionsByOrg(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Org, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 PK_Catalog, SqlBoolean IncludeTaxonomy, SqlBoolean IncludeDynamicTaxonomy, SqlBoolean IncludeCatalog, SqlBoolean IncludeView, SqlBoolean IncludeProduction, SqlBoolean IncludeDraft, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetCatalogPermissionsByOrg(vchrTargetUserLogin, vchrUserLogin, FK_Org, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, PK_Catalog, IncludeTaxonomy, IncludeDynamicTaxonomy, IncludeCatalog, IncludeView, IncludeProduction, IncludeDraft, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetCatalogPermissionsByOrg for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetCatalogPermissionsByOrg for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetCatalogDT(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Org, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 PK_Catalog, SqlBoolean IncludeTaxonomy, SqlBoolean IncludeDynamicTaxonomy, SqlBoolean IncludeCatalog, SqlBoolean IncludeView, SqlBoolean IncludeProduction, SqlBoolean IncludeDraft)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetCatalogDT(vchrTargetUserLogin, vchrUserLogin, FK_Org, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, PK_Catalog, IncludeTaxonomy, IncludeDynamicTaxonomy, IncludeCatalog, IncludeView, IncludeProduction, IncludeDraft, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetCatalogDT(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Org, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 PK_Catalog, SqlBoolean IncludeTaxonomy, SqlBoolean IncludeDynamicTaxonomy, SqlBoolean IncludeCatalog, SqlBoolean IncludeView, SqlBoolean IncludeProduction, SqlBoolean IncludeDraft, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetCatalogDT(vchrTargetUserLogin, vchrUserLogin, FK_Org, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, PK_Catalog, IncludeTaxonomy, IncludeDynamicTaxonomy, IncludeCatalog, IncludeView, IncludeProduction, IncludeDraft, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetCatalogDT(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Org, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 PK_Catalog, SqlBoolean IncludeTaxonomy, SqlBoolean IncludeDynamicTaxonomy, SqlBoolean IncludeCatalog, SqlBoolean IncludeView, SqlBoolean IncludeProduction, SqlBoolean IncludeDraft, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetCatalogDT(vchrTargetUserLogin, vchrUserLogin, FK_Org, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, PK_Catalog, IncludeTaxonomy, IncludeDynamicTaxonomy, IncludeCatalog, IncludeView, IncludeProduction, IncludeDraft, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetCatalogDT for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetCatalogDT for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCatalogsByOrg(SqlString orgId, SqlString vchrTargetUserLogin, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetCatalogsByOrg(orgId, vchrTargetUserLogin, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCatalogsByOrg(SqlString orgId, SqlString vchrTargetUserLogin, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetCatalogsByOrg(orgId, vchrTargetUserLogin, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCatalogsByOrg(SqlString orgId, SqlString vchrTargetUserLogin, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetCatalogsByOrg(orgId, vchrTargetUserLogin, vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetCatalogsByOrg for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetCatalogsByOrg for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ProcessCatalogs(SqlString txtXML, SqlInt32 PK_Org, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.ProcessCatalogs(txtXML, PK_Org, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ProcessCatalogs(SqlString txtXML, SqlInt32 PK_Org, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.ProcessCatalogs(txtXML, PK_Org, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ProcessCatalogs(SqlString txtXML, SqlInt32 PK_Org, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.ProcessCatalogs(txtXML, PK_Org, vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.ProcessCatalogs for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.ProcessCatalogs for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCatalogLocaleByID(SqlInt32 PK_Catalog, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetCatalogLocaleByID(PK_Catalog, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCatalogLocaleByID(SqlInt32 PK_Catalog, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetCatalogLocaleByID(PK_Catalog, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCatalogLocaleByID(SqlInt32 PK_Catalog, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetCatalogLocaleByID(PK_Catalog, vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetCatalogLocaleByID for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetCatalogLocaleByID for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessCatalogLocales(SqlString txtXML, SqlInt32 PK_Org, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Catalog.ProcessCatalogLocales(txtXML, PK_Org, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessCatalogLocales(SqlString txtXML, SqlInt32 PK_Org, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Catalog.ProcessCatalogLocales(txtXML, PK_Org, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessCatalogLocales(SqlString txtXML, SqlInt32 PK_Org, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlCatalog.ProcessCatalogLocales(txtXML, PK_Org, vchrUserLogin, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.ProcessCatalogLocales for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.ProcessCatalogLocales for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetNodePermissions(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 PK_Catalog, SqlInt32 FK_ParentCNode, SqlInt32 FK_Locale, SqlInt32 FK_Customer, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 PK_CNode, SqlBoolean bitIncludeComponents, SqlBoolean bitEnableComponentMapping, SqlBoolean bitUseDrafTax, SqlBoolean bitEnableUnassignedCategory, SqlString ToolTipAttributeList)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetNodePermissions(vchrTargetUserLogin, vchrUserLogin, PK_Catalog, FK_ParentCNode, FK_Locale, FK_Customer, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, PK_CNode, bitIncludeComponents, bitEnableComponentMapping, bitUseDrafTax, bitEnableUnassignedCategory, ToolTipAttributeList, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetNodePermissions(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 PK_Catalog, SqlInt32 FK_ParentCNode, SqlInt32 FK_Locale, SqlInt32 FK_Customer, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 PK_CNode, SqlBoolean bitIncludeComponents, SqlBoolean bitEnableComponentMapping, SqlBoolean bitUseDrafTax, SqlBoolean bitEnableUnassignedCategory, SqlString ToolTipAttributeList, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetNodePermissions(vchrTargetUserLogin, vchrUserLogin, PK_Catalog, FK_ParentCNode, FK_Locale, FK_Customer, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, PK_CNode, bitIncludeComponents, bitEnableComponentMapping, bitUseDrafTax, bitEnableUnassignedCategory, ToolTipAttributeList, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetNodePermissions(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 PK_Catalog, SqlInt32 FK_ParentCNode, SqlInt32 FK_Locale, SqlInt32 FK_Customer, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 PK_CNode, SqlBoolean bitIncludeComponents, SqlBoolean bitEnableComponentMapping, SqlBoolean bitUseDrafTax, SqlBoolean bitEnableUnassignedCategory, SqlString ToolTipAttributeList, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetNodePermissions(vchrTargetUserLogin, vchrUserLogin, PK_Catalog, FK_ParentCNode, FK_Locale, FK_Customer, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, PK_CNode, bitIncludeComponents, bitEnableComponentMapping, bitUseDrafTax, bitEnableUnassignedCategory, ToolTipAttributeList, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetNodePermissions for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetNodePermissions for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetNodePermissionsByCNode(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 PK_Catalog, SqlInt32 CnodeId, SqlInt32 FK_Locale, SqlInt32 FK_Customer, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlBoolean bitIncludeComponents, SqlBoolean bitEnableComponentMapping, SqlBoolean bitUseDrafTax, SqlBoolean bitEnableUnassignedCategory)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetNodePermissionsByCNode(vchrTargetUserLogin, vchrUserLogin, PK_Catalog, CnodeId, FK_Locale, FK_Customer, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, bitIncludeComponents, bitEnableComponentMapping, bitUseDrafTax, bitEnableUnassignedCategory, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetNodePermissionsByCNode(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 PK_Catalog, SqlInt32 CnodeId, SqlInt32 FK_Locale, SqlInt32 FK_Customer, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlBoolean bitIncludeComponents, SqlBoolean bitEnableComponentMapping, SqlBoolean bitUseDrafTax, SqlBoolean bitEnableUnassignedCategory, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetNodePermissionsByCNode(vchrTargetUserLogin, vchrUserLogin, PK_Catalog, CnodeId, FK_Locale, FK_Customer, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, bitIncludeComponents, bitEnableComponentMapping, bitUseDrafTax, bitEnableUnassignedCategory, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetNodePermissionsByCNode(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 PK_Catalog, SqlInt32 CnodeId, SqlInt32 FK_Locale, SqlInt32 FK_Customer, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlBoolean bitIncludeComponents, SqlBoolean bitEnableComponentMapping, SqlBoolean bitUseDrafTax, SqlBoolean bitEnableUnassignedCategory, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetNodePermissionsByCNode(vchrTargetUserLogin, vchrUserLogin, PK_Catalog, CnodeId, FK_Locale, FK_Customer, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, bitIncludeComponents, bitEnableComponentMapping, bitUseDrafTax, bitEnableUnassignedCategory, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetNodePermissionsByCNode for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetNodePermissionsByCNode for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
        public static string GetCoreAttrByGroup( SqlInt32 intLocaleID, SqlInt32 intCustomerID, SqlInt64 intCNodeID, SqlInt64 intCNodeParentID, SqlInt32 intGroupID, SqlInt32 intCatalogID, SqlInt32 intOrgID, SqlString vchrUserID, SqlInt32 intBackLocaleID, SqlString vchrViewPath, SqlBoolean bitUseDraftTax, SqlBoolean ShowAtCreation, SqlString AttrIDList )
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetCoreAttrByGroup(intLocaleID, intCustomerID, intCNodeID, intCNodeParentID, intGroupID, intCatalogID, intOrgID, vchrUserID, intBackLocaleID, vchrViewPath, bitUseDraftTax, ShowAtCreation, AttrIDList, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
        public static string GetCoreAttrByGroup( SqlInt32 intLocaleID, SqlInt32 intCustomerID, SqlInt64 intCNodeID, SqlInt64 intCNodeParentID, SqlInt32 intGroupID, SqlInt32 intCatalogID, SqlInt32 intOrgID, SqlString vchrUserID, SqlInt32 intBackLocaleID, SqlString vchrViewPath, SqlBoolean bitUseDraftTax, SqlBoolean ShowAtCreation, SqlString AttrIDList, IDbConnection connection )
		{
			IDbTransaction transaction = null;
			return Catalog.GetCoreAttrByGroup(intLocaleID, intCustomerID, intCNodeID, intCNodeParentID, intGroupID, intCatalogID, intOrgID, vchrUserID, intBackLocaleID, vchrViewPath, bitUseDraftTax, ShowAtCreation, AttrIDList, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
        public static string GetCoreAttrByGroup( SqlInt32 intLocaleID, SqlInt32 intCustomerID, SqlInt64 intCNodeID, SqlInt64 intCNodeParentID, SqlInt32 intGroupID, SqlInt32 intCatalogID, SqlInt32 intOrgID, SqlString vchrUserID, SqlInt32 intBackLocaleID, SqlString vchrViewPath, SqlBoolean bitUseDraftTax, SqlBoolean ShowAtCreation, SqlString AttrIDList, IDbConnection connection, IDbTransaction transaction )
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetCoreAttrByGroup(intLocaleID, intCustomerID, intCNodeID, intCNodeParentID, intGroupID, intCatalogID, intOrgID, vchrUserID, intBackLocaleID, vchrViewPath, bitUseDraftTax, ShowAtCreation, AttrIDList, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetCoreAttrByGroup for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetCoreAttrByGroup for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
        public static void ProcessCoreAttr(SqlString txtXML, SqlInt32 intCatalogID, SqlInt32 intOrgID, SqlString vchrUserID, SqlString vchrProgramName, SqlInt32 LocaleId, out SqlInt64 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Catalog.ProcessCoreAttr(txtXML, intCatalogID, intOrgID, vchrUserID, vchrProgramName, LocaleId, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
        public static void ProcessCoreAttr(SqlString txtXML, SqlInt32 intCatalogID, SqlInt32 intOrgID, SqlString vchrUserID, SqlString vchrProgramName, SqlInt32 LocaleId, out SqlInt64 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Catalog.ProcessCoreAttr(txtXML, intCatalogID, intOrgID, vchrUserID, vchrProgramName, LocaleId, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessCoreAttr(SqlString txtXML, SqlInt32 intCatalogID, SqlInt32 intOrgID, SqlString vchrUserID, SqlString vchrProgramName, SqlInt32 LocaleId, out SqlInt64 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlCatalog.ProcessCoreAttr(txtXML, intCatalogID, intOrgID, vchrUserID, vchrProgramName, LocaleId, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.ProcessCoreAttr for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.ProcessCoreAttr for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessRelAttr(SqlString txtXML, SqlInt32 intCatalogID, SqlInt32 intOrgID, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Catalog.ProcessRelAttr(txtXML, intCatalogID, intOrgID, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessRelAttr(SqlString txtXML, SqlInt32 intCatalogID, SqlInt32 intOrgID, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Catalog.ProcessRelAttr(txtXML, intCatalogID, intOrgID, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessRelAttr(SqlString txtXML, SqlInt32 intCatalogID, SqlInt32 intOrgID, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlCatalog.ProcessRelAttr(txtXML, intCatalogID, intOrgID, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.ProcessRelAttr for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.ProcessRelAttr for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
        public static void ProcessTechAttr(SqlString txtXML, SqlInt32 intCatalogID, SqlInt32 intOrgID, SqlString vchrUserID, SqlString vchrProgramName, SqlInt32 LocaleId, out SqlInt64 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Catalog.ProcessTechAttr(txtXML, intCatalogID, intOrgID, vchrUserID, vchrProgramName, LocaleId, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
        public static void ProcessTechAttr(SqlString txtXML, SqlInt32 intCatalogID, SqlInt32 intOrgID, SqlString vchrUserID, SqlString vchrProgramName, SqlInt32 LocaleId, out SqlInt64 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Catalog.ProcessTechAttr(txtXML, intCatalogID, intOrgID, vchrUserID, vchrProgramName, LocaleId, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
        public static void ProcessTechAttr(SqlString txtXML, SqlInt32 intCatalogID, SqlInt32 intOrgID, SqlString vchrUserID, SqlString vchrProgramName, SqlInt32 LocaleId, out SqlInt64 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlCatalog.ProcessTechAttr(txtXML, intCatalogID, intOrgID, vchrUserID, vchrProgramName, LocaleId, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.ProcessTechAttr for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.ProcessTechAttr for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetTechAttr(SqlInt32 intCnodeID, SqlInt32 intCnodeParentID, SqlInt32 intCatalogID, SqlInt32 intGroupID, SqlInt32 intLocaleID, SqlInt32 intCustomerID, SqlString vchrUserID, SqlInt32 intBackupLocaleID, SqlString vchrViewPath, SqlBoolean bitUseDraftTax, SqlString AttrIDList)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetTechAttr(intCnodeID, intCnodeParentID, intCatalogID, intGroupID, intLocaleID, intCustomerID, vchrUserID, intBackupLocaleID, vchrViewPath, bitUseDraftTax, AttrIDList, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetTechAttr(SqlInt32 intCnodeID, SqlInt32 intCnodeParentID, SqlInt32 intCatalogID, SqlInt32 intGroupID, SqlInt32 intLocaleID, SqlInt32 intCustomerID, SqlString vchrUserID, SqlInt32 intBackupLocaleID, SqlString vchrViewPath, SqlBoolean bitUseDraftTax, SqlString AttrIDList, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetTechAttr(intCnodeID, intCnodeParentID, intCatalogID, intGroupID, intLocaleID, intCustomerID, vchrUserID, intBackupLocaleID, vchrViewPath, bitUseDraftTax, AttrIDList, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetTechAttr(SqlInt32 intCnodeID, SqlInt32 intCnodeParentID, SqlInt32 intCatalogID, SqlInt32 intGroupID, SqlInt32 intLocaleID, SqlInt32 intCustomerID, SqlString vchrUserID, SqlInt32 intBackupLocaleID, SqlString vchrViewPath, SqlBoolean bitUseDraftTax, SqlString AttrIDList, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetTechAttr(intCnodeID, intCnodeParentID, intCatalogID, intGroupID, intLocaleID, intCustomerID, vchrUserID, intBackupLocaleID, vchrViewPath, bitUseDraftTax, AttrIDList, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetTechAttr for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetTechAttr for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetStatuses()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetStatuses(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetStatuses(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetStatuses(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetStatuses(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetStatuses(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetStatuses for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetStatuses for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetCatalogByName(SqlString ShortName)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetCatalogByName(ShortName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetCatalogByName(SqlString ShortName, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetCatalogByName(ShortName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetCatalogByName(SqlString ShortName, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetCatalogByName(ShortName, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetCatalogByName for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetCatalogByName for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAllUOMsByUOMType()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetAllUOMsByUOMType(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAllUOMsByUOMType(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetAllUOMsByUOMType(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAllUOMsByUOMType(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetAllUOMsByUOMType(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetAllUOMsByUOMType for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetAllUOMsByUOMType for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void SchemaValidationRulesExecution(SqlInt32 JobId, SqlString UserID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Catalog.SchemaValidationRulesExecution(JobId, UserID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void SchemaValidationRulesExecution(SqlInt32 JobId, SqlString UserID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Catalog.SchemaValidationRulesExecution(JobId, UserID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void SchemaValidationRulesExecution(SqlInt32 JobId, SqlString UserID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlCatalog.SchemaValidationRulesExecution(JobId, UserID, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.SchemaValidationRulesExecution for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.SchemaValidationRulesExecution for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCnodeAttachments(SqlInt32 intOrgID, SqlInt32 intCatalogID, SqlInt32 intCNodeParentID, SqlInt32 intCNodeID, SqlString vchrViewPath, SqlInt32 intLocaleID, SqlInt32 intBackLocaleID, SqlInt32 intCustomerID, SqlString nvchrUserID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetCnodeAttachments(intOrgID, intCatalogID, intCNodeParentID, intCNodeID, vchrViewPath, intLocaleID, intBackLocaleID, intCustomerID, nvchrUserID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCnodeAttachments(SqlInt32 intOrgID, SqlInt32 intCatalogID, SqlInt32 intCNodeParentID, SqlInt32 intCNodeID, SqlString vchrViewPath, SqlInt32 intLocaleID, SqlInt32 intBackLocaleID, SqlInt32 intCustomerID, SqlString nvchrUserID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetCnodeAttachments(intOrgID, intCatalogID, intCNodeParentID, intCNodeID, vchrViewPath, intLocaleID, intBackLocaleID, intCustomerID, nvchrUserID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCnodeAttachments(SqlInt32 intOrgID, SqlInt32 intCatalogID, SqlInt32 intCNodeParentID, SqlInt32 intCNodeID, SqlString vchrViewPath, SqlInt32 intLocaleID, SqlInt32 intBackLocaleID, SqlInt32 intCustomerID, SqlString nvchrUserID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetCnodeAttachments(intOrgID, intCatalogID, intCNodeParentID, intCNodeID, vchrViewPath, intLocaleID, intBackLocaleID, intCustomerID, nvchrUserID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetCnodeAttachments for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetCnodeAttachments for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetColumnPreference(SqlString user)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetColumnPreference(user, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetColumnPreference(SqlString user, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetColumnPreference(user, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetColumnPreference(SqlString user, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetColumnPreference(user, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetColumnPreference for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetColumnPreference for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdataColumnPreference(SqlString colXml, SqlString user)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Catalog.UpdataColumnPreference(colXml, user, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdataColumnPreference(SqlString colXml, SqlString user, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Catalog.UpdataColumnPreference(colXml, user, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdataColumnPreference(SqlString colXml, SqlString user, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlCatalog.UpdataColumnPreference(colXml, user, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.UpdataColumnPreference for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.UpdataColumnPreference for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetVisibleComponents(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 intOrgId, SqlInt32 intCatalogId, SqlInt32 intNodeId, SqlBoolean bitRecursive, SqlBoolean bitUseDraftTaxonomy)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetVisibleComponents(vchrTargetUserLogin, vchrUserLogin, intOrgId, intCatalogId, intNodeId, bitRecursive, bitUseDraftTaxonomy, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetVisibleComponents(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 intOrgId, SqlInt32 intCatalogId, SqlInt32 intNodeId, SqlBoolean bitRecursive, SqlBoolean bitUseDraftTaxonomy, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetVisibleComponents(vchrTargetUserLogin, vchrUserLogin, intOrgId, intCatalogId, intNodeId, bitRecursive, bitUseDraftTaxonomy, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetVisibleComponents(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 intOrgId, SqlInt32 intCatalogId, SqlInt32 intNodeId, SqlBoolean bitRecursive, SqlBoolean bitUseDraftTaxonomy, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetVisibleComponents(vchrTargetUserLogin, vchrUserLogin, intOrgId, intCatalogId, intNodeId, bitRecursive, bitUseDraftTaxonomy, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetVisibleComponents for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetVisibleComponents for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCNode(SqlInt32 FK_Catalog, SqlInt64 PK_CNode, SqlString ViewPath)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetCNode(FK_Catalog, PK_CNode, ViewPath, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
        public static string GetCNode( SqlInt32 FK_Catalog, SqlInt64 PK_CNode, SqlString ViewPath, IDbConnection connection )
		{
			IDbTransaction transaction = null;
			return Catalog.GetCNode(FK_Catalog, PK_CNode, ViewPath, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
        public static string GetCNode( SqlInt32 FK_Catalog, SqlInt64 PK_CNode, SqlString ViewPath, IDbConnection connection, IDbTransaction transaction )
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetCNode(FK_Catalog, PK_CNode, ViewPath, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetCNode for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetCNode for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetTaxonomyId(SqlInt32 intCatalogId)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetTaxonomyId(intCatalogId, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetTaxonomyId(SqlInt32 intCatalogId, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetTaxonomyId(intCatalogId, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetTaxonomyId(SqlInt32 intCatalogId, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetTaxonomyId(intCatalogId, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetTaxonomyId for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetTaxonomyId for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateCategoryAttribute(SqlInt32 CategoryID, SqlInt32 CatalogID, SqlString Action)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Catalog.UpdateCategoryAttribute(CategoryID, CatalogID, Action, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateCategoryAttribute(SqlInt32 CategoryID, SqlInt32 CatalogID, SqlString Action, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Catalog.UpdateCategoryAttribute(CategoryID, CatalogID, Action, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateCategoryAttribute(SqlInt32 CategoryID, SqlInt32 CatalogID, SqlString Action, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlCatalog.UpdateCategoryAttribute(CategoryID, CatalogID, Action, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.UpdateCategoryAttribute for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.UpdateCategoryAttribute for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAttrLookup(SqlInt32 AttributeID, SqlInt32 Cnode, SqlInt32 LocaleID, ref SqlBoolean IsCategory, ref SqlString LKTableName)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetAttrLookup(AttributeID, Cnode, LocaleID, ref IsCategory, ref LKTableName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAttrLookup(SqlInt32 AttributeID, SqlInt32 Cnode, SqlInt32 LocaleID, ref SqlBoolean IsCategory, ref SqlString LKTableName, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetAttrLookup(AttributeID, Cnode, LocaleID, ref IsCategory, ref LKTableName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAttrLookup(SqlInt32 AttributeID, SqlInt32 Cnode, SqlInt32 LocaleID, ref SqlBoolean IsCategory, ref SqlString LKTableName, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetAttrLookup(AttributeID, Cnode, LocaleID, ref IsCategory, ref LKTableName, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetAttrLookup for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetAttrLookup for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable WhereUsed(SqlInt64 CNodeID, SqlString RelationshipType, SqlString AttributeList, SqlInt32 TotalRows, SqlString CatalogFilter, SqlInt32 CustomerID, SqlInt32 LocaleID, ref SqlInt32 TotalCount)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.WhereUsed(CNodeID, RelationshipType, AttributeList, TotalRows, CatalogFilter, CustomerID, LocaleID, ref TotalCount, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable WhereUsed(SqlInt64 CNodeID, SqlString RelationshipType, SqlString AttributeList, SqlInt32 TotalRows, SqlString CatalogFilter, SqlInt32 CustomerID, SqlInt32 LocaleID, ref SqlInt32 TotalCount, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.WhereUsed(CNodeID, RelationshipType, AttributeList, TotalRows, CatalogFilter, CustomerID, LocaleID, ref TotalCount, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable WhereUsed(SqlInt64 CNodeID, SqlString RelationshipType, SqlString AttributeList, SqlInt32 TotalRows, SqlString CatalogFilter, SqlInt32 CustomerID, SqlInt32 LocaleID, ref SqlInt32 TotalCount, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.WhereUsed(CNodeID, RelationshipType, AttributeList, TotalRows, CatalogFilter, CustomerID, LocaleID, ref TotalCount, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.WhereUsed for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.WhereUsed for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ComplianceCheckAttribute()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.ComplianceCheckAttribute(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ComplianceCheckAttribute(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.ComplianceCheckAttribute(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ComplianceCheckAttribute(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.ComplianceCheckAttribute(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.ComplianceCheckAttribute for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.ComplianceCheckAttribute for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetCatalogAttributes(SqlString UserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetCatalogAttributes(UserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetCatalogAttributes(SqlString UserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetCatalogAttributes(UserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetCatalogAttributes(SqlString UserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetCatalogAttributes(UserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetCatalogAttributes for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetCatalogAttributes for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void CopyCatalogAttributes(SqlInt32 intFromCatalogId, SqlInt32 intToCatalogId, SqlString CreateProgram, SqlString CreateUser)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Catalog.CopyCatalogAttributes(intFromCatalogId, intToCatalogId, CreateProgram, CreateUser, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void CopyCatalogAttributes(SqlInt32 intFromCatalogId, SqlInt32 intToCatalogId, SqlString CreateProgram, SqlString CreateUser, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Catalog.CopyCatalogAttributes(intFromCatalogId, intToCatalogId, CreateProgram, CreateUser, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void CopyCatalogAttributes(SqlInt32 intFromCatalogId, SqlInt32 intToCatalogId, SqlString CreateProgram, SqlString CreateUser, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlCatalog.CopyCatalogAttributes(intFromCatalogId, intToCatalogId, CreateProgram, CreateUser, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.CopyCatalogAttributes for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.CopyCatalogAttributes for this provider: "+providerName);
			}
		}

        /// <summary>
        /// 
        /// </summary>
		public static DataTable GetAllCategories(SqlInt32 PK_Catalog, SqlInt32 FK_Locale, SqlString Filter, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetAllCategories(PK_Catalog, FK_Locale, Filter, vchrUserLogin, connection, transaction);
		}

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetAllCategories(SqlInt32 PK_Catalog, SqlInt32 FK_Locale, SqlString Filter, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetAllCategories(PK_Catalog, FK_Locale, Filter, vchrUserLogin, connection, transaction);
		}

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetAllCategories(SqlInt32 PK_Catalog, SqlInt32 FK_Locale, SqlString Filter, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetAllCategories(PK_Catalog, FK_Locale, Filter, vchrUserLogin, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetAllCategories for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Catalog.GetAllCategories for this provider: " + providerName);
            }
        }

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetNameValCollection(SqlString IdList)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetNameValCollection(IdList, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetNameValCollection(SqlString IdList, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetNameValCollection(IdList, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetNameValCollection(SqlString IdList, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetNameValCollection(IdList, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetNameValCollection for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetNameValCollection for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCollectionValues(SqlInt32 FK_CNode, SqlInt32 ParentId, SqlInt32 FK_Catalog, SqlInt32 FK_Customer, SqlInt32 FK_Locale, SqlString InheritanceMode)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetCollectionValues(FK_CNode, ParentId, FK_Catalog, FK_Customer, FK_Locale, InheritanceMode, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCollectionValues(SqlInt32 FK_CNode, SqlInt32 ParentId, SqlInt32 FK_Catalog, SqlInt32 FK_Customer, SqlInt32 FK_Locale, SqlString InheritanceMode, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetCollectionValues(FK_CNode, ParentId, FK_Catalog, FK_Customer, FK_Locale, InheritanceMode, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCollectionValues(SqlInt32 FK_CNode, SqlInt32 ParentId, SqlInt32 FK_Catalog, SqlInt32 FK_Customer, SqlInt32 FK_Locale, SqlString InheritanceMode, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetCollectionValues(FK_CNode, ParentId, FK_Catalog, FK_Customer, FK_Locale, InheritanceMode, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetCollectionValues for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetCollectionValues for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCatalogNodeTypeAttributesXML(SqlInt32 intOrgID, SqlInt32 intCatalogID, SqlString nvchrNodeType, SqlInt32 intBranchLevel, SqlBoolean IncludeComplexAttrChildren, SqlBoolean ExcludeSearchable)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetCatalogNodeTypeAttributesXML(intOrgID, intCatalogID, nvchrNodeType, intBranchLevel, IncludeComplexAttrChildren, ExcludeSearchable, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCatalogNodeTypeAttributesXML(SqlInt32 intOrgID, SqlInt32 intCatalogID, SqlString nvchrNodeType, SqlInt32 intBranchLevel, SqlBoolean IncludeComplexAttrChildren, SqlBoolean ExcludeSearchable, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetCatalogNodeTypeAttributesXML(intOrgID, intCatalogID, nvchrNodeType, intBranchLevel, IncludeComplexAttrChildren, ExcludeSearchable, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCatalogNodeTypeAttributesXML(SqlInt32 intOrgID, SqlInt32 intCatalogID, SqlString nvchrNodeType, SqlInt32 intBranchLevel, SqlBoolean IncludeComplexAttrChildren, SqlBoolean ExcludeSearchable, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetCatalogNodeTypeAttributesXML(intOrgID, intCatalogID, nvchrNodeType, intBranchLevel, IncludeComplexAttrChildren, ExcludeSearchable, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetCatalogNodeTypeAttributesXML for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetCatalogNodeTypeAttributesXML for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetSystemAttributes()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetSystemAttributes(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetSystemAttributes(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetSystemAttributes(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetSystemAttributes(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetSystemAttributes(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetSystemAttributes for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetSystemAttributes for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void PromoteItems(SqlString ExternalID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Catalog.PromoteItems(ExternalID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void PromoteItems(SqlString ExternalID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Catalog.PromoteItems(ExternalID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void PromoteItems(SqlString ExternalID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlCatalog.PromoteItems(ExternalID, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.PromoteItems for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.PromoteItems for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DemoteItems(SqlString ExternalID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Catalog.DemoteItems(ExternalID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DemoteItems(SqlString ExternalID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Catalog.DemoteItems(ExternalID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DemoteItems(SqlString ExternalID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlCatalog.DemoteItems(ExternalID, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.DemoteItems for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.DemoteItems for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessSearchCriteria(SqlString Action, SqlInt32 PK_Security_SearchCriteria, SqlString CriteriaName, SqlInt32 FK_Security_User, SqlInt32 FK_Catalog, SqlBoolean IsGlobalSearch, SqlString SearchCriteriaXml, SqlString loginUser, SqlString UserProgram, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Catalog.ProcessSearchCriteria(Action, PK_Security_SearchCriteria, CriteriaName, FK_Security_User, FK_Catalog, IsGlobalSearch, SearchCriteriaXml, loginUser, UserProgram, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessSearchCriteria(SqlString Action, SqlInt32 PK_Security_SearchCriteria, SqlString CriteriaName, SqlInt32 FK_Security_User, SqlInt32 FK_Catalog, SqlBoolean IsGlobalSearch, SqlString SearchCriteriaXml, SqlString loginUser, SqlString UserProgram, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Catalog.ProcessSearchCriteria(Action, PK_Security_SearchCriteria, CriteriaName, FK_Security_User, FK_Catalog, IsGlobalSearch, SearchCriteriaXml, loginUser, UserProgram, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessSearchCriteria(SqlString Action, SqlInt32 PK_Security_SearchCriteria, SqlString CriteriaName, SqlInt32 FK_Security_User, SqlInt32 FK_Catalog, SqlBoolean IsGlobalSearch, SqlString SearchCriteriaXml, SqlString loginUser, SqlString UserProgram, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlCatalog.ProcessSearchCriteria(Action, PK_Security_SearchCriteria, CriteriaName, FK_Security_User, FK_Catalog, IsGlobalSearch, SearchCriteriaXml, loginUser, UserProgram, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.ProcessSearchCriteria for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.ProcessSearchCriteria for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetSearchCriterias(SqlInt32 PK_Security_SearchCriteria, SqlInt32 FK_Security_User, SqlInt32 FK_Catalog)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetSearchCriterias(PK_Security_SearchCriteria, FK_Security_User, FK_Catalog, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetSearchCriterias(SqlInt32 PK_Security_SearchCriteria, SqlInt32 FK_Security_User, SqlInt32 FK_Catalog, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetSearchCriterias(PK_Security_SearchCriteria, FK_Security_User, FK_Catalog, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetSearchCriterias(SqlInt32 PK_Security_SearchCriteria, SqlInt32 FK_Security_User, SqlInt32 FK_Catalog, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetSearchCriterias(PK_Security_SearchCriteria, FK_Security_User, FK_Catalog, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetSearchCriterias for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetSearchCriterias for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable QuickSearch(SqlInt32 CatalogID, SqlString SearchValue, SqlInt32 PK_CNode, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.QuickSearch(CatalogID, SearchValue, PK_CNode, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable QuickSearch(SqlInt32 CatalogID, SqlString SearchValue, SqlInt32 PK_CNode, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.QuickSearch(CatalogID, SearchValue, PK_CNode, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable QuickSearch(SqlInt32 CatalogID, SqlString SearchValue, SqlInt32 PK_CNode, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.QuickSearch(CatalogID, SearchValue, PK_CNode, vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.QuickSearch for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.QuickSearch for this provider: "+providerName);
			}
		}

		/// <summary>
        /// Get the catalog 
        /// </summary>
		public static DataTable GetStagingCatalogId(SqlInt32 FK_Org, SqlInt32 FK_Catalog)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetStagingCatalogId(FK_Org, FK_Catalog, connection, transaction);
		}

		/// <summary>
        /// Get the catalog 
        /// </summary>
		public static DataTable GetStagingCatalogId(SqlInt32 FK_Org, SqlInt32 FK_Catalog, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetStagingCatalogId(FK_Org, FK_Catalog, connection, transaction);
		}

		/// <summary>
        /// Get the catalog 
        /// </summary>
		public static DataTable GetStagingCatalogId(SqlInt32 FK_Org, SqlInt32 FK_Catalog, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetStagingCatalogId(FK_Org, FK_Catalog, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetStagingCatalogId for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetStagingCatalogId for this provider: "+providerName);
			}
		}

		/// <summary>
        /// Update the items with status Ready For Promote statging catalog
        /// </summary>
		public static DataTable MarkItemComplete(SqlInt32 FK_Org, SqlInt32 FK_Catalog, SqlString CnodeList, SqlString vchrUserLogin, SqlString vchrUserRole)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.MarkItemComplete(FK_Org, FK_Catalog, CnodeList, vchrUserLogin, vchrUserRole, connection, transaction);
		}

		/// <summary>
        /// Update the items with status Ready For Promote statging catalog
        /// </summary>
		public static DataTable MarkItemComplete(SqlInt32 FK_Org, SqlInt32 FK_Catalog, SqlString CnodeList, SqlString vchrUserLogin, SqlString vchrUserRole, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.MarkItemComplete(FK_Org, FK_Catalog, CnodeList, vchrUserLogin, vchrUserRole, connection, transaction);
		}

		/// <summary>
        /// Update the items with status Ready For Promote statging catalog
        /// </summary>
		public static DataTable MarkItemComplete(SqlInt32 FK_Org, SqlInt32 FK_Catalog, SqlString CnodeList, SqlString vchrUserLogin, SqlString vchrUserRole, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.MarkItemComplete(FK_Org, FK_Catalog, CnodeList, vchrUserLogin, vchrUserRole, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.MarkItemComplete for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.MarkItemComplete for this provider: "+providerName);
			}
		}

		/// <summary>
        /// Find MASTER/STAGING ITEM BASED ON THE DATA PROVIDED
        /// </summary>
		public static DataTable Get_CatalogCNodeOrgInfo(SqlInt32 CatalogID, SqlInt64 CNodeID, SqlString SKU, SqlString FindWhat)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.Get_CatalogCNodeOrgInfo(CatalogID, CNodeID, SKU, FindWhat, connection, transaction);
		}

		/// <summary>
        /// Find MASTER/STAGING ITEM BASED ON THE DATA PROVIDED
        /// </summary>
		public static DataTable Get_CatalogCNodeOrgInfo(SqlInt32 CatalogID, SqlInt64 CNodeID, SqlString SKU, SqlString FindWhat, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.Get_CatalogCNodeOrgInfo(CatalogID, CNodeID, SKU, FindWhat, connection, transaction);
		}

		/// <summary>
        /// Find MASTER/STAGING ITEM BASED ON THE DATA PROVIDED
        /// </summary>
		public static DataTable Get_CatalogCNodeOrgInfo(SqlInt32 CatalogID, SqlInt64 CNodeID, SqlString SKU, SqlString FindWhat, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.Get_CatalogCNodeOrgInfo(CatalogID, CNodeID, SKU, FindWhat, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.Get_CatalogCNodeOrgInfo for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.Get_CatalogCNodeOrgInfo for this provider: "+providerName);
			}
		}

		/// <summary>
        /// Find MASTER/STAGING ITEM BASED ON THE DATA PROVIDED
        /// </summary>
		public static string SetBOMMatchingRelationship(SqlInt32 FK_StagingCatalogID, SqlString matchingXML, SqlString vchrProgramName, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.SetBOMMatchingRelationship(FK_StagingCatalogID, matchingXML, vchrProgramName, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// Find MASTER/STAGING ITEM BASED ON THE DATA PROVIDED
        /// </summary>
		public static string SetBOMMatchingRelationship(SqlInt32 FK_StagingCatalogID, SqlString matchingXML, SqlString vchrProgramName, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.SetBOMMatchingRelationship(FK_StagingCatalogID, matchingXML, vchrProgramName, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// Find MASTER/STAGING ITEM BASED ON THE DATA PROVIDED
        /// </summary>
		public static string SetBOMMatchingRelationship(SqlInt32 FK_StagingCatalogID, SqlString matchingXML, SqlString vchrProgramName, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.SetBOMMatchingRelationship(FK_StagingCatalogID, matchingXML, vchrProgramName, vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.SetBOMMatchingRelationship for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.SetBOMMatchingRelationship for this provider: "+providerName);
			}
		}

		/// <summary>
        /// Return All the Detail of Given SKU and CNode
        /// </summary>
		public static DataTable GetALLCatalogCnodeDetailBySKU(SqlString txtXML, SqlString PassedType)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetALLCatalogCnodeDetailBySKU(txtXML, PassedType, connection, transaction);
		}

		/// <summary>
        /// Return All the Detail of Given SKU and CNode
        /// </summary>
		public static DataTable GetALLCatalogCnodeDetailBySKU(SqlString txtXML, SqlString PassedType, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetALLCatalogCnodeDetailBySKU(txtXML, PassedType, connection, transaction);
		}

		/// <summary>
        /// Return All the Detail of Given SKU and CNode
        /// </summary>
		public static DataTable GetALLCatalogCnodeDetailBySKU(SqlString txtXML, SqlString PassedType, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetALLCatalogCnodeDetailBySKU(txtXML, PassedType, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetALLCatalogCnodeDetailBySKU for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetALLCatalogCnodeDetailBySKU for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetUserConfigMetadata(SqlXml ConfigXml)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetUserConfigMetadata(ConfigXml, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetUserConfigMetadata(SqlXml ConfigXml, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetUserConfigMetadata(ConfigXml, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetUserConfigMetadata(SqlXml ConfigXml, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetUserConfigMetadata(ConfigXml, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetUserConfigMetadata for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetUserConfigMetadata for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetUserConfigContextData(SqlInt32 FK_Application_ContextType, ref SqlString SeqDataTableforUI)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetUserConfigContextData(FK_Application_ContextType, ref SeqDataTableforUI, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetUserConfigContextData(SqlInt32 FK_Application_ContextType, ref SqlString SeqDataTableforUI, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetUserConfigContextData(FK_Application_ContextType, ref SeqDataTableforUI, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetUserConfigContextData(SqlInt32 FK_Application_ContextType, ref SqlString SeqDataTableforUI, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetUserConfigContextData(FK_Application_ContextType, ref SeqDataTableforUI, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetUserConfigContextData for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetUserConfigContextData for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetApplicationConfig(SqlInt32 EventSourceID, SqlInt32 EventSubscriberID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetApplicationConfig(EventSourceID, EventSubscriberID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetApplicationConfig(SqlInt32 EventSourceID, SqlInt32 EventSubscriberID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetApplicationConfig(EventSourceID, EventSubscriberID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetApplicationConfig(SqlInt32 EventSourceID, SqlInt32 EventSubscriberID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetApplicationConfig(EventSourceID, EventSubscriberID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetApplicationConfig for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetApplicationConfig for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetRelationshipCardinality(SqlInt32 FK_Catalog, SqlInt32 FK_NodeType_From, SqlInt32 FK_RelationshipType)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetRelationshipCardinality(FK_Catalog, FK_NodeType_From, FK_RelationshipType, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetRelationshipCardinality(SqlInt32 FK_Catalog, SqlInt32 FK_NodeType_From, SqlInt32 FK_RelationshipType, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetRelationshipCardinality(FK_Catalog, FK_NodeType_From, FK_RelationshipType, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetRelationshipCardinality(SqlInt32 FK_Catalog, SqlInt32 FK_NodeType_From, SqlInt32 FK_RelationshipType, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetRelationshipCardinality(FK_Catalog, FK_NodeType_From, FK_RelationshipType, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetRelationshipCardinality for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetRelationshipCardinality for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessRelationshipCardinality(SqlString txtXML, SqlString UserName, SqlString ProgramName, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Catalog.ProcessRelationshipCardinality(txtXML, UserName, ProgramName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessRelationshipCardinality(SqlString txtXML, SqlString UserName, SqlString ProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Catalog.ProcessRelationshipCardinality(txtXML, UserName, ProgramName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessRelationshipCardinality(SqlString txtXML, SqlString UserName, SqlString ProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlCatalog.ProcessRelationshipCardinality(txtXML, UserName, ProgramName, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.ProcessRelationshipCardinality for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.ProcessRelationshipCardinality for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string ExtractCatalogByIDLocal(SqlInt32 intExtSystemID, SqlString txtXML, SqlString vchrCoreAttrList, SqlString vchrTechAttrList, SqlInt32 FK_Locale, SqlString ProgramName, SqlBoolean IncludeInheritedValues, SqlBoolean ComputeInheritedValues)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.ExtractCatalogByIDLocal(intExtSystemID, txtXML, vchrCoreAttrList, vchrTechAttrList, FK_Locale, ProgramName, IncludeInheritedValues, ComputeInheritedValues, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string ExtractCatalogByIDLocal(SqlInt32 intExtSystemID, SqlString txtXML, SqlString vchrCoreAttrList, SqlString vchrTechAttrList, SqlInt32 FK_Locale, SqlString ProgramName, SqlBoolean IncludeInheritedValues, SqlBoolean ComputeInheritedValues, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.ExtractCatalogByIDLocal(intExtSystemID, txtXML, vchrCoreAttrList, vchrTechAttrList, FK_Locale, ProgramName, IncludeInheritedValues, ComputeInheritedValues, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string ExtractCatalogByIDLocal(SqlInt32 intExtSystemID, SqlString txtXML, SqlString vchrCoreAttrList, SqlString vchrTechAttrList, SqlInt32 FK_Locale, SqlString ProgramName, SqlBoolean IncludeInheritedValues, SqlBoolean ComputeInheritedValues, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.ExtractCatalogByIDLocal(intExtSystemID, txtXML, vchrCoreAttrList, vchrTechAttrList, FK_Locale, ProgramName, IncludeInheritedValues, ComputeInheritedValues, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.ExtractCatalogByIDLocal for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.ExtractCatalogByIDLocal for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteSearchCriteria(SqlInt32 PK_Security_SearchCriteria, SqlString loginUser, SqlString ModProgram, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Catalog.DeleteSearchCriteria(PK_Security_SearchCriteria, loginUser, ModProgram, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteSearchCriteria(SqlInt32 PK_Security_SearchCriteria, SqlString loginUser, SqlString ModProgram, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Catalog.DeleteSearchCriteria(PK_Security_SearchCriteria, loginUser, ModProgram, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteSearchCriteria(SqlInt32 PK_Security_SearchCriteria, SqlString loginUser, SqlString ModProgram, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlCatalog.DeleteSearchCriteria(PK_Security_SearchCriteria, loginUser, ModProgram, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.DeleteSearchCriteria for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.DeleteSearchCriteria for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetStageTransitionButtons(SqlString ContextXml, SqlInt32 StageId, SqlString CNodeList, SqlString ToolbarButtonXml)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetStageTransitionButtons(ContextXml, StageId, CNodeList, ToolbarButtonXml, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetStageTransitionButtons(SqlString ContextXml, SqlInt32 StageId, SqlString CNodeList, SqlString ToolbarButtonXml, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetStageTransitionButtons(ContextXml, StageId, CNodeList, ToolbarButtonXml, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetStageTransitionButtons(SqlString ContextXml, SqlInt32 StageId, SqlString CNodeList, SqlString ToolbarButtonXml, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetStageTransitionButtons(ContextXml, StageId, CNodeList, ToolbarButtonXml, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetStageTransitionButtons for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetStageTransitionButtons for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAssignmentButtons(SqlString AssignmentStatus, SqlString CNodeList, SqlString ToolbarButtonXml, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetAssignmentButtons(AssignmentStatus, CNodeList, ToolbarButtonXml, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAssignmentButtons(SqlString AssignmentStatus, SqlString CNodeList, SqlString ToolbarButtonXml, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetAssignmentButtons(AssignmentStatus, CNodeList, ToolbarButtonXml, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAssignmentButtons(SqlString AssignmentStatus, SqlString CNodeList, SqlString ToolbarButtonXml, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetAssignmentButtons(AssignmentStatus, CNodeList, ToolbarButtonXml, vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetAssignmentButtons for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetAssignmentButtons for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetWorkflowPanel(SqlInt32 ContainerID, SqlInt32 loginUserID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Catalog.GetWorkflowPanel(ContainerID, loginUserID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetWorkflowPanel(SqlInt32 ContainerID, SqlInt32 loginUserID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Catalog.GetWorkflowPanel(ContainerID, loginUserID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetWorkflowPanel(SqlInt32 ContainerID, SqlInt32 loginUserID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCatalog.GetWorkflowPanel(ContainerID, loginUserID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetWorkflowPanel for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetWorkflowPanel for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
        public static string GetSearchCategoriesByCriteria(SqlString vchrSearchValue, SqlInt32 intCatalogID, SqlInt32 intParentID, SqlString toolTipAttributeList, SqlString vchrUserLogin,SqlInt32 dataLocale)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
            return Catalog.GetSearchCategoriesByCriteria(vchrSearchValue, intCatalogID, intParentID, toolTipAttributeList, vchrUserLogin, dataLocale, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
        public static string GetSearchCategoriesByCriteria(SqlString vchrSearchValue, SqlInt32 intCatalogID, SqlInt32 intParentID, SqlString toolTipAttributeList, SqlString vchrUserLogin, SqlInt32 dataLocale, IDbConnection connection)
		{
			IDbTransaction transaction = null;
            return Catalog.GetSearchCategoriesByCriteria(vchrSearchValue, intCatalogID, intParentID, toolTipAttributeList, vchrUserLogin, dataLocale, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetSearchCategoriesByCriteria(SqlString vchrSearchValue, SqlInt32 intCatalogID, SqlInt32 intParentID, SqlString toolTipAttributeList, SqlString vchrUserLogin, SqlInt32 dataLocale, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
                    return SqlCatalog.GetSearchCategoriesByCriteria(vchrSearchValue, intCatalogID, intParentID, toolTipAttributeList, vchrUserLogin, dataLocale, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetSearchCategoriesByCriteria for this provider: "+providerName);
					throw new ApplicationException("No implementation of Catalog.GetSearchCategoriesByCriteria for this provider: "+providerName);
			}
		}

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetCategoryNavPanel(SqlInt32 catalogId, SqlString sysAttrSelectionXml, SqlString categorySearchColumn, SqlString categorySearchString, SqlInt64 ParentCategoryId, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrUserLogin, SqlInt32 dataLocale)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
            return Catalog.GetCategoryNavPanel(catalogId, sysAttrSelectionXml, categorySearchColumn, categorySearchString, ParentCategoryId, intCountFrom, intCountTo, vchrUserLogin, dataLocale, connection, transaction);
		}

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetCategoryNavPanel(SqlInt32 catalogId, SqlString sysAttrSelectionXml, SqlString categorySearchColumn, SqlString categorySearchString, SqlInt64 ParentCategoryId, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrUserLogin, SqlInt32 dataLocale, IDbConnection connection)
		{
			IDbTransaction transaction = null;
            return Catalog.GetCategoryNavPanel(catalogId, sysAttrSelectionXml, categorySearchColumn, categorySearchString, ParentCategoryId, intCountFrom, intCountTo, vchrUserLogin, dataLocale, connection, transaction);
		}

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetCategoryNavPanel(SqlInt32 catalogId, SqlString sysAttrSelectionXml, SqlString categorySearchColumn, SqlString categorySearchString, SqlInt64 ParentCategoryId, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrUserLogin, SqlInt32 dataLocale, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
                    return SqlCatalog.GetCategoryNavPanel(catalogId, sysAttrSelectionXml, categorySearchColumn, categorySearchString, ParentCategoryId, intCountFrom, intCountTo, vchrUserLogin, dataLocale, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Catalog.GetCategoryNavPanel for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Catalog.GetCategoryNavPanel for this provider: " + providerName);
            }
        }

	}
}		
