
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
	public class Taxonomy
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private Taxonomy()
		{
		}
		
		
		/// <summary>
        /// 
        /// </summary>
		public static void ProcessCategory(SqlInt32 intTaxonomyID, SqlString txtXML, SqlString vchrUserID, SqlString vchrProgramName, SqlString vchrUserType, ref SqlInt32 intOutput, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Taxonomy.ProcessCategory(intTaxonomyID, txtXML, vchrUserID, vchrProgramName, vchrUserType, ref intOutput, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessCategory(SqlInt32 intTaxonomyID, SqlString txtXML, SqlString vchrUserID, SqlString vchrProgramName, SqlString vchrUserType, ref SqlInt32 intOutput, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Taxonomy.ProcessCategory(intTaxonomyID, txtXML, vchrUserID, vchrProgramName, vchrUserType, ref intOutput, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessCategory(SqlInt32 intTaxonomyID, SqlString txtXML, SqlString vchrUserID, SqlString vchrProgramName, SqlString vchrUserType, ref SqlInt32 intOutput, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlTaxonomy.ProcessCategory(intTaxonomyID, txtXML, vchrUserID, vchrProgramName, vchrUserType, ref intOutput, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Taxonomy.ProcessCategory for this provider: "+providerName);
					throw new ApplicationException("No implementation of Taxonomy.ProcessCategory for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void CopyCategoryExt(SqlInt64 intCopyCNodeID, SqlInt64 intParentCNodeID, SqlInt32 intFromTaxonomyID, SqlInt32 intToTaxonomyID, SqlString vchrUserID, SqlString vchrProgramName, SqlString nvchrInheritMode, SqlBoolean bitIncludeSelf, SqlBoolean bitIncludeChildren, SqlBoolean bitRecursive, SqlBoolean bitNamePrefixCopyOf, SqlBoolean bitCreateSynchronizationLinks, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Taxonomy.CopyCategoryExt(intCopyCNodeID, intParentCNodeID, intFromTaxonomyID, intToTaxonomyID, vchrUserID, vchrProgramName, nvchrInheritMode, bitIncludeSelf, bitIncludeChildren, bitRecursive, bitNamePrefixCopyOf, bitCreateSynchronizationLinks, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void CopyCategoryExt(SqlInt64 intCopyCNodeID, SqlInt64 intParentCNodeID, SqlInt32 intFromTaxonomyID, SqlInt32 intToTaxonomyID, SqlString vchrUserID, SqlString vchrProgramName, SqlString nvchrInheritMode, SqlBoolean bitIncludeSelf, SqlBoolean bitIncludeChildren, SqlBoolean bitRecursive, SqlBoolean bitNamePrefixCopyOf, SqlBoolean bitCreateSynchronizationLinks, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Taxonomy.CopyCategoryExt(intCopyCNodeID, intParentCNodeID, intFromTaxonomyID, intToTaxonomyID, vchrUserID, vchrProgramName, nvchrInheritMode, bitIncludeSelf, bitIncludeChildren, bitRecursive, bitNamePrefixCopyOf, bitCreateSynchronizationLinks, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void CopyCategoryExt(SqlInt64 intCopyCNodeID, SqlInt64 intParentCNodeID, SqlInt32 intFromTaxonomyID, SqlInt32 intToTaxonomyID, SqlString vchrUserID, SqlString vchrProgramName, SqlString nvchrInheritMode, SqlBoolean bitIncludeSelf, SqlBoolean bitIncludeChildren, SqlBoolean bitRecursive, SqlBoolean bitNamePrefixCopyOf, SqlBoolean bitCreateSynchronizationLinks, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlTaxonomy.CopyCategoryExt(intCopyCNodeID, intParentCNodeID, intFromTaxonomyID, intToTaxonomyID, vchrUserID, vchrProgramName, nvchrInheritMode, bitIncludeSelf, bitIncludeChildren, bitRecursive, bitNamePrefixCopyOf, bitCreateSynchronizationLinks, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Taxonomy.CopyCategoryExt for this provider: "+providerName);
					throw new ApplicationException("No implementation of Taxonomy.CopyCategoryExt for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetTechSpecsMap(SqlInt32 intCategoryID, SqlInt32 intLocaleID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Taxonomy.GetTechSpecsMap(intCategoryID, intLocaleID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetTechSpecsMap(SqlInt32 intCategoryID, SqlInt32 intLocaleID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Taxonomy.GetTechSpecsMap(intCategoryID, intLocaleID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetTechSpecsMap(SqlInt32 intCategoryID, SqlInt32 intLocaleID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlTaxonomy.GetTechSpecsMap(intCategoryID, intLocaleID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Taxonomy.GetTechSpecsMap for this provider: "+providerName);
					throw new ApplicationException("No implementation of Taxonomy.GetTechSpecsMap for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteCategoryAttribute(SqlInt32 intTaxonomyID, SqlInt32 intTemplateID, SqlString vchrUserID, SqlString vchrProgramName, SqlBoolean isInheritable)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Taxonomy.DeleteCategoryAttribute(intTaxonomyID, intTemplateID, vchrUserID, vchrProgramName, isInheritable, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteCategoryAttribute(SqlInt32 intTaxonomyID, SqlInt32 intTemplateID, SqlString vchrUserID, SqlString vchrProgramName,SqlBoolean isInheritable, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Taxonomy.DeleteCategoryAttribute(intTaxonomyID, intTemplateID, vchrUserID, vchrProgramName, isInheritable, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteCategoryAttribute(SqlInt32 intTaxonomyID, SqlInt32 intTemplateID, SqlString vchrUserID, SqlString vchrProgramName, SqlBoolean isInheritable, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlTaxonomy.DeleteCategoryAttribute(intTaxonomyID, intTemplateID, vchrUserID, vchrProgramName, isInheritable, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Taxonomy.DeleteCategoryAttribute for this provider: "+providerName);
					throw new ApplicationException("No implementation of Taxonomy.DeleteCategoryAttribute for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DuplicateTaxonomy(SqlString vchrUserLogin, SqlInt32 intSourceTaxonomyID, SqlInt32 intDestTaxononomyID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Taxonomy.DuplicateTaxonomy(vchrUserLogin, intSourceTaxonomyID, intDestTaxononomyID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DuplicateTaxonomy(SqlString vchrUserLogin, SqlInt32 intSourceTaxonomyID, SqlInt32 intDestTaxononomyID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Taxonomy.DuplicateTaxonomy(vchrUserLogin, intSourceTaxonomyID, intDestTaxononomyID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DuplicateTaxonomy(SqlString vchrUserLogin, SqlInt32 intSourceTaxonomyID, SqlInt32 intDestTaxononomyID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlTaxonomy.DuplicateTaxonomy(vchrUserLogin, intSourceTaxonomyID, intDestTaxononomyID, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Taxonomy.DuplicateTaxonomy for this provider: "+providerName);
					throw new ApplicationException("No implementation of Taxonomy.DuplicateTaxonomy for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void GetCategoryID(SqlString path, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Taxonomy.GetCategoryID(path, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void GetCategoryID(SqlString path, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Taxonomy.GetCategoryID(path, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void GetCategoryID(SqlString path, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlTaxonomy.GetCategoryID(path, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Taxonomy.GetCategoryID for this provider: "+providerName);
					throw new ApplicationException("No implementation of Taxonomy.GetCategoryID for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void GetCategoryAttributeID(SqlInt32 intCategoryID, SqlInt32 intAttributeID, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Taxonomy.GetCategoryAttributeID(intCategoryID, intAttributeID, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void GetCategoryAttributeID(SqlInt32 intCategoryID, SqlInt32 intAttributeID, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Taxonomy.GetCategoryAttributeID(intCategoryID, intAttributeID, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void GetCategoryAttributeID(SqlInt32 intCategoryID, SqlInt32 intAttributeID, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlTaxonomy.GetCategoryAttributeID(intCategoryID, intAttributeID, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Taxonomy.GetCategoryAttributeID for this provider: "+providerName);
					throw new ApplicationException("No implementation of Taxonomy.GetCategoryAttributeID for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetTaxonomyMetadataByCatalogID(SqlInt32 intCatalogID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Taxonomy.GetTaxonomyMetadataByCatalogID(intCatalogID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetTaxonomyMetadataByCatalogID(SqlInt32 intCatalogID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Taxonomy.GetTaxonomyMetadataByCatalogID(intCatalogID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetTaxonomyMetadataByCatalogID(SqlInt32 intCatalogID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlTaxonomy.GetTaxonomyMetadataByCatalogID(intCatalogID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Taxonomy.GetTaxonomyMetadataByCatalogID for this provider: "+providerName);
					throw new ApplicationException("No implementation of Taxonomy.GetTaxonomyMetadataByCatalogID for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetVisibleTaxonomies(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Org, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 PK_Catalog, SqlBoolean IncludeTaxonomy, SqlBoolean IncludeDynamicTaxonomy, SqlBoolean IncludeCatalog, SqlBoolean IncludeView)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Taxonomy.GetVisibleTaxonomies(vchrTargetUserLogin, vchrUserLogin, FK_Org, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, PK_Catalog, IncludeTaxonomy, IncludeDynamicTaxonomy, IncludeCatalog, IncludeView, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetVisibleTaxonomies(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Org, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 PK_Catalog, SqlBoolean IncludeTaxonomy, SqlBoolean IncludeDynamicTaxonomy, SqlBoolean IncludeCatalog, SqlBoolean IncludeView, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Taxonomy.GetVisibleTaxonomies(vchrTargetUserLogin, vchrUserLogin, FK_Org, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, PK_Catalog, IncludeTaxonomy, IncludeDynamicTaxonomy, IncludeCatalog, IncludeView, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetVisibleTaxonomies(SqlString vchrTargetUserLogin, SqlString vchrUserLogin, SqlInt32 FK_Org, SqlInt32 FK_Locale, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSortColumn, SqlString vchrSearchColumn, SqlString vchrSearchParameter, SqlInt32 PK_Catalog, SqlBoolean IncludeTaxonomy, SqlBoolean IncludeDynamicTaxonomy, SqlBoolean IncludeCatalog, SqlBoolean IncludeView, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlTaxonomy.GetVisibleTaxonomies(vchrTargetUserLogin, vchrUserLogin, FK_Org, FK_Locale, intCountFrom, intCountTo, vchrSortColumn, vchrSearchColumn, vchrSearchParameter, PK_Catalog, IncludeTaxonomy, IncludeDynamicTaxonomy, IncludeCatalog, IncludeView, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Taxonomy.GetVisibleTaxonomies for this provider: "+providerName);
					throw new ApplicationException("No implementation of Taxonomy.GetVisibleTaxonomies for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetDependentCatalogs(SqlInt32 DraftTaxonomy)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Taxonomy.GetDependentCatalogs(DraftTaxonomy, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetDependentCatalogs(SqlInt32 DraftTaxonomy, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Taxonomy.GetDependentCatalogs(DraftTaxonomy, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetDependentCatalogs(SqlInt32 DraftTaxonomy, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlTaxonomy.GetDependentCatalogs(DraftTaxonomy, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Taxonomy.GetDependentCatalogs for this provider: "+providerName);
					throw new ApplicationException("No implementation of Taxonomy.GetDependentCatalogs for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetTaxonomyByCatalog(SqlInt32 CatalogID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Taxonomy.GetTaxonomyByCatalog(CatalogID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetTaxonomyByCatalog(SqlInt32 CatalogID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Taxonomy.GetTaxonomyByCatalog(CatalogID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetTaxonomyByCatalog(SqlInt32 CatalogID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlTaxonomy.GetTaxonomyByCatalog(CatalogID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Taxonomy.GetTaxonomyByCatalog for this provider: "+providerName);
					throw new ApplicationException("No implementation of Taxonomy.GetTaxonomyByCatalog for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCNodeAndNodeXML(SqlInt32 intCNodeID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Taxonomy.GetCNodeAndNodeXML(intCNodeID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCNodeAndNodeXML(SqlInt32 intCNodeID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Taxonomy.GetCNodeAndNodeXML(intCNodeID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCNodeAndNodeXML(SqlInt32 intCNodeID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlTaxonomy.GetCNodeAndNodeXML(intCNodeID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Taxonomy.GetCNodeAndNodeXML for this provider: "+providerName);
					throw new ApplicationException("No implementation of Taxonomy.GetCNodeAndNodeXML for this provider: "+providerName);
			}
		}

	}
}		
