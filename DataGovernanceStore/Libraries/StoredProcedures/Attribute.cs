
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
	public class Attribute
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private Attribute()
		{
		}
		
		
		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetByAttributeGroup(SqlInt32 AttributeGroupId)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetByAttributeGroup(AttributeGroupId, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetByAttributeGroup(SqlInt32 AttributeGroupId, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetByAttributeGroup(AttributeGroupId, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetByAttributeGroup(SqlInt32 AttributeGroupId, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetByAttributeGroup(AttributeGroupId, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetByAttributeGroup for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetByAttributeGroup for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAttributeGroupsByType(SqlInt32 Common, SqlInt32 Technical, SqlInt32 RelationShip)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetAttributeGroupsByType(Common, Technical, RelationShip, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAttributeGroupsByType(SqlInt32 Common, SqlInt32 Technical, SqlInt32 RelationShip, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetAttributeGroupsByType(Common, Technical, RelationShip, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAttributeGroupsByType(SqlInt32 Common, SqlInt32 Technical, SqlInt32 RelationShip, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetAttributeGroupsByType(Common, Technical, RelationShip, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetAttributeGroupsByType for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetAttributeGroupsByType for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCNodeAttributeValue(SqlInt64 FK_CNode, SqlInt32 FK_Catalog, SqlInt32 FK_Attribute, SqlInt32 FK_Locale, SqlString vchrUserName, SqlInt32 ReturnAttrType, SqlBoolean ShowAtCreation)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetCNodeAttributeValue(FK_CNode, FK_Catalog, FK_Attribute, FK_Locale, vchrUserName, ReturnAttrType, ShowAtCreation, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
        public static string GetCNodeAttributeValue(SqlInt64 FK_CNode, SqlInt32 FK_Catalog, SqlInt32 FK_Attribute, SqlInt32 FK_Locale, SqlString vchrUserName, SqlInt32 ReturnAttrType, SqlBoolean ShowAtCreation, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetCNodeAttributeValue(FK_CNode, FK_Catalog, FK_Attribute, FK_Locale, vchrUserName, ReturnAttrType, ShowAtCreation, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
        public static string GetCNodeAttributeValue(SqlInt64 FK_CNode, SqlInt32 FK_Catalog, SqlInt32 FK_Attribute, SqlInt32 FK_Locale, SqlString vchrUserName, SqlInt32 ReturnAttrType, SqlBoolean ShowAtCreation, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetCNodeAttributeValue(FK_CNode, FK_Catalog, FK_Attribute, FK_Locale, vchrUserName, ReturnAttrType, ShowAtCreation, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetCNodeAttributeValue for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetCNodeAttributeValue for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet UniqueComplxAttrid()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.UniqueComplxAttrid(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet UniqueComplxAttrid(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.UniqueComplxAttrid(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet UniqueComplxAttrid(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.UniqueComplxAttrid(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.UniqueComplxAttrid for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.UniqueComplxAttrid for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeByGroupXML(SqlInt32 intGroupID, SqlString nvchrShortName, SqlInt32 intLevel)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetAttributeByGroupXML(intGroupID, nvchrShortName, intLevel, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeByGroupXML(SqlInt32 intGroupID, SqlString nvchrShortName, SqlInt32 intLevel, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetAttributeByGroupXML(intGroupID, nvchrShortName, intLevel, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeByGroupXML(SqlInt32 intGroupID, SqlString nvchrShortName, SqlInt32 intLevel, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetAttributeByGroupXML(intGroupID, nvchrShortName, intLevel, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetAttributeByGroupXML for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetAttributeByGroupXML for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void RelationAttributeRollBack(SqlInt32 VersionId, SqlInt32 CnodeId, SqlInt32 attributeId, SqlInt32 CnodeParentId)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Attribute.RelationAttributeRollBack(VersionId, CnodeId, attributeId, CnodeParentId, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void RelationAttributeRollBack(SqlInt32 VersionId, SqlInt32 CnodeId, SqlInt32 attributeId, SqlInt32 CnodeParentId, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Attribute.RelationAttributeRollBack(VersionId, CnodeId, attributeId, CnodeParentId, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void RelationAttributeRollBack(SqlInt32 VersionId, SqlInt32 CnodeId, SqlInt32 attributeId, SqlInt32 CnodeParentId, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlAttribute.RelationAttributeRollBack(VersionId, CnodeId, attributeId, CnodeParentId, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.RelationAttributeRollBack for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.RelationAttributeRollBack for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeGroupsXML()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetAttributeGroupsXML(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeGroupsXML(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetAttributeGroupsXML(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeGroupsXML(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetAttributeGroupsXML(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetAttributeGroupsXML for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetAttributeGroupsXML for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeUsage(SqlInt32 AttributeID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetAttributeUsage(AttributeID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeUsage(SqlInt32 AttributeID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetAttributeUsage(AttributeID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeUsage(SqlInt32 AttributeID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetAttributeUsage(AttributeID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetAttributeUsage for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetAttributeUsage for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeGroups(SqlInt32 intAttributeID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetAttributeGroups(intAttributeID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeGroups(SqlInt32 intAttributeID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetAttributeGroups(intAttributeID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeGroups(SqlInt32 intAttributeID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetAttributeGroups(intAttributeID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetAttributeGroups for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetAttributeGroups for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void GetAttributeID(SqlString Name, SqlInt32 ParentID, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Attribute.GetAttributeID(Name, ParentID, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void GetAttributeID(SqlString Name, SqlInt32 ParentID, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Attribute.GetAttributeID(Name, ParentID, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void GetAttributeID(SqlString Name, SqlInt32 ParentID, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlAttribute.GetAttributeID(Name, ParentID, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetAttributeID for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetAttributeID for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void GetAttributeDisplayTypeID(SqlString name, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Attribute.GetAttributeDisplayTypeID(name, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void GetAttributeDisplayTypeID(SqlString name, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Attribute.GetAttributeDisplayTypeID(name, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void GetAttributeDisplayTypeID(SqlString name, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlAttribute.GetAttributeDisplayTypeID(name, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetAttributeDisplayTypeID for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetAttributeDisplayTypeID for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetAttributeDataForLookupControl(SqlString ComplexTableName, SqlString LookUpColumnName, SqlInt32 isCheckingOnly)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetAttributeDataForLookupControl(ComplexTableName, LookUpColumnName, isCheckingOnly, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetAttributeDataForLookupControl(SqlString ComplexTableName, SqlString LookUpColumnName, SqlInt32 isCheckingOnly, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetAttributeDataForLookupControl(ComplexTableName, LookUpColumnName, isCheckingOnly, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetAttributeDataForLookupControl(SqlString ComplexTableName, SqlString LookUpColumnName, SqlInt32 isCheckingOnly, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetAttributeDataForLookupControl(ComplexTableName, LookUpColumnName, isCheckingOnly, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetAttributeDataForLookupControl for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetAttributeDataForLookupControl for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAttributeValues(SqlString Object, SqlString tablename, SqlString searchvalue, SqlInt32 localeid, SqlInt32 toprows)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetAttributeValues(Object, tablename, searchvalue, localeid, toprows, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAttributeValues(SqlString Object, SqlString tablename, SqlString searchvalue, SqlInt32 localeid, SqlInt32 toprows, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetAttributeValues(Object, tablename, searchvalue, localeid, toprows, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAttributeValues(SqlString Object, SqlString tablename, SqlString searchvalue, SqlInt32 localeid, SqlInt32 toprows, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetAttributeValues(Object, tablename, searchvalue, localeid, toprows, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetAttributeValues for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetAttributeValues for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void AddAttributeGroup(SqlInt32 intParentId, SqlString vchrShortName, SqlString vchrLongName, SqlInt32 intLocaleId, SqlString vchrUserLogin, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Attribute.AddAttributeGroup(intParentId, vchrShortName, vchrLongName, intLocaleId, vchrUserLogin, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void AddAttributeGroup(SqlInt32 intParentId, SqlString vchrShortName, SqlString vchrLongName, SqlInt32 intLocaleId, SqlString vchrUserLogin, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Attribute.AddAttributeGroup(intParentId, vchrShortName, vchrLongName, intLocaleId, vchrUserLogin, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void AddAttributeGroup(SqlInt32 intParentId, SqlString vchrShortName, SqlString vchrLongName, SqlInt32 intLocaleId, SqlString vchrUserLogin, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlAttribute.AddAttributeGroup(intParentId, vchrShortName, vchrLongName, intLocaleId, vchrUserLogin, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.AddAttributeGroup for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.AddAttributeGroup for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteAttributeGroup(SqlInt32 intAttributeId, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Attribute.DeleteAttributeGroup(intAttributeId, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteAttributeGroup(SqlInt32 intAttributeId, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Attribute.DeleteAttributeGroup(intAttributeId, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteAttributeGroup(SqlInt32 intAttributeId, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlAttribute.DeleteAttributeGroup(intAttributeId, vchrUserLogin, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.DeleteAttributeGroup for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.DeleteAttributeGroup for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeGroupChildren(SqlInt32 intParentId, SqlInt32 intLocaleId, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetAttributeGroupChildren(intParentId, intLocaleId, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeGroupChildren(SqlInt32 intParentId, SqlInt32 intLocaleId, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetAttributeGroupChildren(intParentId, intLocaleId, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeGroupChildren(SqlInt32 intParentId, SqlInt32 intLocaleId, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetAttributeGroupChildren(intParentId, intLocaleId, vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetAttributeGroupChildren for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetAttributeGroupChildren for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributes(SqlInt32 intParentId, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSearchParameter, SqlString vchrSearchColumn, SqlString vchrSortColumn, SqlInt32 intLocaleId, SqlBoolean bitUnusedOnly, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetAttributes(intParentId, intCountFrom, intCountTo, vchrSearchParameter, vchrSearchColumn, vchrSortColumn, intLocaleId, bitUnusedOnly, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributes(SqlInt32 intParentId, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSearchParameter, SqlString vchrSearchColumn, SqlString vchrSortColumn, SqlInt32 intLocaleId, SqlBoolean bitUnusedOnly, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetAttributes(intParentId, intCountFrom, intCountTo, vchrSearchParameter, vchrSearchColumn, vchrSortColumn, intLocaleId, bitUnusedOnly, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributes(SqlInt32 intParentId, SqlInt32 intCountFrom, SqlInt32 intCountTo, SqlString vchrSearchParameter, SqlString vchrSearchColumn, SqlString vchrSortColumn, SqlInt32 intLocaleId, SqlBoolean bitUnusedOnly, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetAttributes(intParentId, intCountFrom, intCountTo, vchrSearchParameter, vchrSearchColumn, vchrSortColumn, intLocaleId, bitUnusedOnly, vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetAttributes for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetAttributes for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateAttributeGroup(SqlInt32 intAttributeId, SqlInt32 intParentId, SqlString vchrShortName, SqlString vchrLongName, SqlInt32 intLocaleId, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Attribute.UpdateAttributeGroup(intAttributeId, intParentId, vchrShortName, vchrLongName, intLocaleId, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateAttributeGroup(SqlInt32 intAttributeId, SqlInt32 intParentId, SqlString vchrShortName, SqlString vchrLongName, SqlInt32 intLocaleId, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Attribute.UpdateAttributeGroup(intAttributeId, intParentId, vchrShortName, vchrLongName, intLocaleId, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateAttributeGroup(SqlInt32 intAttributeId, SqlInt32 intParentId, SqlString vchrShortName, SqlString vchrLongName, SqlInt32 intLocaleId, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlAttribute.UpdateAttributeGroup(intAttributeId, intParentId, vchrShortName, vchrLongName, intLocaleId, vchrUserLogin, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.UpdateAttributeGroup for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.UpdateAttributeGroup for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteAttributes(SqlString txtXML, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Attribute.DeleteAttributes(txtXML, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteAttributes(SqlString txtXML, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Attribute.DeleteAttributes(txtXML, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteAttributes(SqlString txtXML, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlAttribute.DeleteAttributes(txtXML, vchrUserLogin, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.DeleteAttributes for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.DeleteAttributes for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeDataTypeMap(SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetAttributeDataTypeMap(vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeDataTypeMap(SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetAttributeDataTypeMap(vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeDataTypeMap(SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetAttributeDataTypeMap(vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetAttributeDataTypeMap for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetAttributeDataTypeMap for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeDataTypes(SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetAttributeDataTypes(vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeDataTypes(SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetAttributeDataTypes(vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeDataTypes(SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetAttributeDataTypes(vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetAttributeDataTypes for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetAttributeDataTypes for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeDetails(SqlInt32 intAttributeId, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetAttributeDetails(intAttributeId, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeDetails(SqlInt32 intAttributeId, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetAttributeDetails(intAttributeId, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeDetails(SqlInt32 intAttributeId, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetAttributeDetails(intAttributeId, vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetAttributeDetails for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetAttributeDetails for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeDisplayTypeMap(SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetAttributeDisplayTypeMap(vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeDisplayTypeMap(SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetAttributeDisplayTypeMap(vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeDisplayTypeMap(SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetAttributeDisplayTypeMap(vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetAttributeDisplayTypeMap for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetAttributeDisplayTypeMap for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeDisplayTypes(SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetAttributeDisplayTypes(vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeDisplayTypes(SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetAttributeDisplayTypes(vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeDisplayTypes(SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetAttributeDisplayTypes(vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetAttributeDisplayTypes for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetAttributeDisplayTypes for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void CreateOrUpdateAttribute(SqlString txtXML, SqlBoolean bitUpdate, SqlInt32 intLocaleId, SqlString nvchrEnglishName, SqlString vchrUserLogin, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Attribute.CreateOrUpdateAttribute(txtXML, bitUpdate, intLocaleId, nvchrEnglishName, vchrUserLogin, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void CreateOrUpdateAttribute(SqlString txtXML, SqlBoolean bitUpdate, SqlInt32 intLocaleId, SqlString nvchrEnglishName, SqlString vchrUserLogin, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Attribute.CreateOrUpdateAttribute(txtXML, bitUpdate, intLocaleId, nvchrEnglishName, vchrUserLogin, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void CreateOrUpdateAttribute(SqlString txtXML, SqlBoolean bitUpdate, SqlInt32 intLocaleId, SqlString nvchrEnglishName, SqlString vchrUserLogin, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlAttribute.CreateOrUpdateAttribute(txtXML, bitUpdate, intLocaleId, nvchrEnglishName, vchrUserLogin, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.CreateOrUpdateAttribute for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.CreateOrUpdateAttribute for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetNodeAttributeDetails(SqlInt32 intAttributeId, SqlInt32 intCNodeId, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetNodeAttributeDetails(intAttributeId, intCNodeId, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetNodeAttributeDetails(SqlInt32 intAttributeId, SqlInt32 intCNodeId, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetNodeAttributeDetails(intAttributeId, intCNodeId, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetNodeAttributeDetails(SqlInt32 intAttributeId, SqlInt32 intCNodeId, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetNodeAttributeDetails(intAttributeId, intCNodeId, vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetNodeAttributeDetails for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetNodeAttributeDetails for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateNodeAttributeDetails(SqlString txtXML, SqlInt32 intCNodeId, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Attribute.UpdateNodeAttributeDetails(txtXML, intCNodeId, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateNodeAttributeDetails(SqlString txtXML, SqlInt32 intCNodeId, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Attribute.UpdateNodeAttributeDetails(txtXML, intCNodeId, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateNodeAttributeDetails(SqlString txtXML, SqlInt32 intCNodeId, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlAttribute.UpdateNodeAttributeDetails(txtXML, intCNodeId, vchrUserLogin, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.UpdateNodeAttributeDetails for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.UpdateNodeAttributeDetails for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessObjectAttributes(SqlString AttributeXML, SqlInt32 ObjectId, SqlString vchrObjectType, SqlInt32 FK_Locale)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Attribute.ProcessObjectAttributes(AttributeXML, ObjectId, vchrObjectType, FK_Locale, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessObjectAttributes(SqlString AttributeXML, SqlInt32 ObjectId, SqlString vchrObjectType, SqlInt32 FK_Locale, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Attribute.ProcessObjectAttributes(AttributeXML, ObjectId, vchrObjectType, FK_Locale, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessObjectAttributes(SqlString AttributeXML, SqlInt32 ObjectId, SqlString vchrObjectType, SqlInt32 FK_Locale, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlAttribute.ProcessObjectAttributes(AttributeXML, ObjectId, vchrObjectType, FK_Locale, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.ProcessObjectAttributes for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.ProcessObjectAttributes for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetSysObjectAttributesXML(SqlString ObjectType, SqlInt32 ObjectId, SqlInt32 FK_Locale)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetSysObjectAttributesXML(ObjectType, ObjectId, FK_Locale, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetSysObjectAttributesXML(SqlString ObjectType, SqlInt32 ObjectId, SqlInt32 FK_Locale, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetSysObjectAttributesXML(ObjectType, ObjectId, FK_Locale, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetSysObjectAttributesXML(SqlString ObjectType, SqlInt32 ObjectId, SqlInt32 FK_Locale, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetSysObjectAttributesXML(ObjectType, ObjectId, FK_Locale, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetSysObjectAttributesXML for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetSysObjectAttributesXML for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCategoriesFromRuleXML(SqlString txtRulesXML)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetCategoriesFromRuleXML(txtRulesXML, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCategoriesFromRuleXML(SqlString txtRulesXML, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetCategoriesFromRuleXML(txtRulesXML, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCategoriesFromRuleXML(SqlString txtRulesXML, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetCategoriesFromRuleXML(txtRulesXML, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetCategoriesFromRuleXML for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetCategoriesFromRuleXML for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetFormattersXML()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetFormattersXML(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetFormattersXML(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetFormattersXML(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetFormattersXML(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetFormattersXML(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetFormattersXML for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetFormattersXML for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string UpdateTargetAttributeValue(SqlString txtXML, SqlInt32 targetAttributeId, SqlInt32 maxAllowableChars, SqlString inTestPage, ref SqlString retAttrValue)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.UpdateTargetAttributeValue(txtXML, targetAttributeId, maxAllowableChars, inTestPage, ref retAttrValue, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string UpdateTargetAttributeValue(SqlString txtXML, SqlInt32 targetAttributeId, SqlInt32 maxAllowableChars, SqlString inTestPage, ref SqlString retAttrValue, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.UpdateTargetAttributeValue(txtXML, targetAttributeId, maxAllowableChars, inTestPage, ref retAttrValue, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string UpdateTargetAttributeValue(SqlString txtXML, SqlInt32 targetAttributeId, SqlInt32 maxAllowableChars, SqlString inTestPage, ref SqlString retAttrValue, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.UpdateTargetAttributeValue(txtXML, targetAttributeId, maxAllowableChars, inTestPage, ref retAttrValue, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.UpdateTargetAttributeValue for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.UpdateTargetAttributeValue for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetFormatter()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetFormatter(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetFormatter(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetFormatter(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetFormatter(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetFormatter(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetFormatter for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetFormatter for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributesBasedOnLocale(SqlInt32 intAttributeId, SqlInt32 intSpecificLocaleId)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetAttributesBasedOnLocale(intAttributeId, intSpecificLocaleId, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributesBasedOnLocale(SqlInt32 intAttributeId, SqlInt32 intSpecificLocaleId, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetAttributesBasedOnLocale(intAttributeId, intSpecificLocaleId, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributesBasedOnLocale(SqlInt32 intAttributeId, SqlInt32 intSpecificLocaleId, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetAttributesBasedOnLocale(intAttributeId, intSpecificLocaleId, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetAttributesBasedOnLocale for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetAttributesBasedOnLocale for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ProcessComplexAttrMetadata(SqlInt32 ComplexAttrId, SqlString Action, SqlString userLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.ProcessComplexAttrMetadata(ComplexAttrId, Action, userLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ProcessComplexAttrMetadata(SqlInt32 ComplexAttrId, SqlString Action, SqlString userLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.ProcessComplexAttrMetadata(ComplexAttrId, Action, userLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ProcessComplexAttrMetadata(SqlInt32 ComplexAttrId, SqlString Action, SqlString userLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.ProcessComplexAttrMetadata(ComplexAttrId, Action, userLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.ProcessComplexAttrMetadata for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.ProcessComplexAttrMetadata for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet ComplexMetadata_GetDT(SqlString inputStr)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.ComplexMetadata_GetDT(inputStr, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet ComplexMetadata_GetDT(SqlString inputStr, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.ComplexMetadata_GetDT(inputStr, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet ComplexMetadata_GetDT(SqlString inputStr, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.ComplexMetadata_GetDT(inputStr, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.ComplexMetadata_GetDT for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.ComplexMetadata_GetDT for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ComplexSystemAttrs_SetXML(SqlXml inputData, SqlString user, SqlString CreateProgram)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.ComplexSystemAttrs_SetXML(inputData, user, CreateProgram, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ComplexSystemAttrs_SetXML(SqlXml inputData, SqlString user, SqlString CreateProgram, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.ComplexSystemAttrs_SetXML(inputData, user, CreateProgram, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ComplexSystemAttrs_SetXML(SqlXml inputData, SqlString user, SqlString CreateProgram, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.ComplexSystemAttrs_SetXML(inputData, user, CreateProgram, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.ComplexSystemAttrs_SetXML for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.ComplexSystemAttrs_SetXML for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ComplexMetadata_SetXML(SqlXml inputData, SqlInt64 cNode, SqlInt32 Fk_Catalog, SqlInt32 Locale, SqlString user, SqlString CreateProgram)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.ComplexMetadata_SetXML(inputData, cNode, Fk_Catalog, Locale, user, CreateProgram, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ComplexMetadata_SetXML(SqlXml inputData, SqlInt64 cNode, SqlInt32 Fk_Catalog, SqlInt32 Locale, SqlString user, SqlString CreateProgram, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.ComplexMetadata_SetXML(inputData, cNode, Fk_Catalog, Locale, user, CreateProgram, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ComplexMetadata_SetXML(SqlXml inputData, SqlInt64 cNode, SqlInt32 Fk_Catalog, SqlInt32 Locale, SqlString user, SqlString CreateProgram, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.ComplexMetadata_SetXML(inputData, cNode, Fk_Catalog, Locale, user, CreateProgram, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.ComplexMetadata_SetXML for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.ComplexMetadata_SetXML for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ComplexMetadataDelete(SqlInt32 Pkey, SqlInt32 ComplexAttrId, SqlInt32 cNode, SqlInt32 Locale, SqlInt32 Fk_Catalog)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Attribute.ComplexMetadataDelete(Pkey, ComplexAttrId, cNode, Locale, Fk_Catalog, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ComplexMetadataDelete(SqlInt32 Pkey, SqlInt32 ComplexAttrId, SqlInt32 cNode, SqlInt32 Locale, SqlInt32 Fk_Catalog, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Attribute.ComplexMetadataDelete(Pkey, ComplexAttrId, cNode, Locale, Fk_Catalog, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ComplexMetadataDelete(SqlInt32 Pkey, SqlInt32 ComplexAttrId, SqlInt32 cNode, SqlInt32 Locale, SqlInt32 Fk_Catalog, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlAttribute.ComplexMetadataDelete(Pkey, ComplexAttrId, cNode, Locale, Fk_Catalog, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.ComplexMetadataDelete for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.ComplexMetadataDelete for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
        public static string GetAttributeChildrenByType( SqlInt32 intAttributeTypeID, SqlString intAttributeID, SqlString vchrUserID, SqlInt32 intOrgID, SqlInt32 intCatalogID, SqlInt64 intCNodeID, SqlString vchrViewPath )
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetAttributeChildrenByType(intAttributeTypeID, intAttributeID, vchrUserID, intOrgID, intCatalogID, intCNodeID, vchrViewPath, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
        public static string GetAttributeChildrenByType( SqlInt32 intAttributeTypeID, SqlString intAttributeID, SqlString vchrUserID, SqlInt32 intOrgID, SqlInt32 intCatalogID, SqlInt64 intCNodeID, SqlString vchrViewPath, IDbConnection connection )
		{
			IDbTransaction transaction = null;
			return Attribute.GetAttributeChildrenByType(intAttributeTypeID, intAttributeID, vchrUserID, intOrgID, intCatalogID, intCNodeID, vchrViewPath, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
        public static string GetAttributeChildrenByType( SqlInt32 intAttributeTypeID, SqlString intAttributeID, SqlString vchrUserID, SqlInt32 intOrgID, SqlInt32 intCatalogID, SqlInt64 intCNodeID, SqlString vchrViewPath, IDbConnection connection, IDbTransaction transaction )
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetAttributeChildrenByType(intAttributeTypeID, intAttributeID, vchrUserID, intOrgID, intCatalogID, intCNodeID, vchrViewPath, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetAttributeChildrenByType for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetAttributeChildrenByType for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string getAttributesXml(SqlInt32 intGroupID, SqlString SearchValue, SqlInt32 intLevel, SqlBoolean GetComplexChildren, SqlInt32 localeId)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.getAttributesXml(intGroupID, SearchValue, intLevel, GetComplexChildren, localeId,connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string getAttributesXml(SqlInt32 intGroupID, SqlString SearchValue, SqlInt32 intLevel, SqlBoolean GetComplexChildren, SqlInt32 localeId, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.getAttributesXml(intGroupID, SearchValue, intLevel, GetComplexChildren, localeId, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string getAttributesXml(SqlInt32 intGroupID, SqlString SearchValue, SqlInt32 intLevel, SqlBoolean GetComplexChildren, SqlInt32 localeId, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.getAttributesXml(intGroupID, SearchValue, intLevel, GetComplexChildren, localeId, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.getAttributesXml for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.getAttributesXml for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAttributeColumnNames()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetAttributeColumnNames(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAttributeColumnNames(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetAttributeColumnNames(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAttributeColumnNames(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetAttributeColumnNames(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetAttributeColumnNames for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetAttributeColumnNames for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ComplexMetadataRollBack(SqlInt32 VersionId, SqlInt32 CnodeId, SqlInt32 attributeId)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Attribute.ComplexMetadataRollBack(VersionId, CnodeId, attributeId, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ComplexMetadataRollBack(SqlInt32 VersionId, SqlInt32 CnodeId, SqlInt32 attributeId, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Attribute.ComplexMetadataRollBack(VersionId, CnodeId, attributeId, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ComplexMetadataRollBack(SqlInt32 VersionId, SqlInt32 CnodeId, SqlInt32 attributeId, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlAttribute.ComplexMetadataRollBack(VersionId, CnodeId, attributeId, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.ComplexMetadataRollBack for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.ComplexMetadataRollBack for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetComplexAttributeVersions(SqlInt32 intCNodeID, SqlInt32 intAttributeID, SqlInt32 intLocaleID, SqlInt32 intCatalogID, SqlInt32 intCNodeParentID, SqlInt32 cmplValId)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetComplexAttributeVersions(intCNodeID, intAttributeID, intLocaleID, intCatalogID, intCNodeParentID, cmplValId, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetComplexAttributeVersions(SqlInt32 intCNodeID, SqlInt32 intAttributeID, SqlInt32 intLocaleID, SqlInt32 intCatalogID, SqlInt32 intCNodeParentID, SqlInt32 cmplValId, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetComplexAttributeVersions(intCNodeID, intAttributeID, intLocaleID, intCatalogID, intCNodeParentID, cmplValId, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetComplexAttributeVersions(SqlInt32 intCNodeID, SqlInt32 intAttributeID, SqlInt32 intLocaleID, SqlInt32 intCatalogID, SqlInt32 intCNodeParentID, SqlInt32 cmplValId, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetComplexAttributeVersions(intCNodeID, intAttributeID, intLocaleID, intCatalogID, intCNodeParentID, cmplValId, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetComplexAttributeVersions for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetComplexAttributeVersions for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAllAttributes()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetAllAttributes(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAllAttributes(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetAllAttributes(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAllAttributes(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetAllAttributes(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetAllAttributes for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetAllAttributes for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetAllAttributeMappings()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetAllAttributeMappings(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetAllAttributeMappings(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetAllAttributeMappings(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetAllAttributeMappings(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetAllAttributeMappings(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetAllAttributeMappings for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetAllAttributeMappings for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAttributeBusinessRules(SqlInt32 AttributeParentId, SqlString AttributeTypeName, SqlString SearchKey)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetAttributeBusinessRules(AttributeParentId, AttributeTypeName, SearchKey, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAttributeBusinessRules(SqlInt32 AttributeParentId, SqlString AttributeTypeName, SqlString SearchKey, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetAttributeBusinessRules(AttributeParentId, AttributeTypeName, SearchKey, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAttributeBusinessRules(SqlInt32 AttributeParentId, SqlString AttributeTypeName, SqlString SearchKey, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetAttributeBusinessRules(AttributeParentId, AttributeTypeName, SearchKey, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetAttributeBusinessRules for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetAttributeBusinessRules for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetBusinessExpressionRules(SqlInt32 FK_Business_Rule)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetBusinessExpressionRules(FK_Business_Rule, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetBusinessExpressionRules(SqlInt32 FK_Business_Rule, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetBusinessExpressionRules(FK_Business_Rule, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetBusinessExpressionRules(SqlInt32 FK_Business_Rule, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetBusinessExpressionRules(FK_Business_Rule, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetBusinessExpressionRules for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetBusinessExpressionRules for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessAttributeBusinessRules(SqlString txtXML, SqlString vchrProgramName, SqlString vchrUserLogin, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Attribute.ProcessAttributeBusinessRules(txtXML, vchrProgramName, vchrUserLogin, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessAttributeBusinessRules(SqlString txtXML, SqlString vchrProgramName, SqlString vchrUserLogin, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Attribute.ProcessAttributeBusinessRules(txtXML, vchrProgramName, vchrUserLogin, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessAttributeBusinessRules(SqlString txtXML, SqlString vchrProgramName, SqlString vchrUserLogin, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlAttribute.ProcessAttributeBusinessRules(txtXML, vchrProgramName, vchrUserLogin, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.ProcessAttributeBusinessRules for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.ProcessAttributeBusinessRules for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessBusinessExpressionRules(SqlString txtXML, SqlString vchrProgramName, SqlString vchrUserLogin, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Attribute.ProcessBusinessExpressionRules(txtXML, vchrProgramName, vchrUserLogin, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessBusinessExpressionRules(SqlString txtXML, SqlString vchrProgramName, SqlString vchrUserLogin, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Attribute.ProcessBusinessExpressionRules(txtXML, vchrProgramName, vchrUserLogin, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ProcessBusinessExpressionRules(SqlString txtXML, SqlString vchrProgramName, SqlString vchrUserLogin, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlAttribute.ProcessBusinessExpressionRules(txtXML, vchrProgramName, vchrUserLogin, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.ProcessBusinessExpressionRules for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.ProcessBusinessExpressionRules for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeConfig(SqlString nvchrTableName, SqlInt32 intPK)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetAttributeConfig(nvchrTableName, intPK, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeConfig(SqlString nvchrTableName, SqlInt32 intPK, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetAttributeConfig(nvchrTableName, intPK, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeConfig(SqlString nvchrTableName, SqlInt32 intPK, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetAttributeConfig(nvchrTableName, intPK, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetAttributeConfig for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetAttributeConfig for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetTechSpecsByGroup(SqlInt32 intAttrParentID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetTechSpecsByGroup(intAttrParentID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetTechSpecsByGroup(SqlInt32 intAttrParentID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetTechSpecsByGroup(intAttrParentID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetTechSpecsByGroup(SqlInt32 intAttrParentID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetTechSpecsByGroup(intAttrParentID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetTechSpecsByGroup for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetTechSpecsByGroup for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeXmlByAttributeId(SqlInt64 FK_CNode, SqlInt32 FK_Catalog, SqlString AttrList, SqlInt32 FK_Locale, SqlString vchrUserID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Attribute.GetAttributeXmlByAttributeId(FK_CNode, FK_Catalog, AttrList, FK_Locale, vchrUserID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeXmlByAttributeId(SqlInt64 FK_CNode, SqlInt32 FK_Catalog, SqlString AttrList, SqlInt32 FK_Locale, SqlString vchrUserID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Attribute.GetAttributeXmlByAttributeId(FK_CNode, FK_Catalog, AttrList, FK_Locale, vchrUserID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetAttributeXmlByAttributeId(SqlInt64 FK_CNode, SqlInt32 FK_Catalog, SqlString AttrList, SqlInt32 FK_Locale, SqlString vchrUserID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlAttribute.GetAttributeXmlByAttributeId(FK_CNode, FK_Catalog, AttrList, FK_Locale, vchrUserID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetAttributeXmlByAttributeId for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetAttributeXmlByAttributeId for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
        public static string GetSystemAttributes(SqlInt32 intOrgID, SqlBoolean ExcludeSearchable, SqlInt32 LocaleId)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
            return Attribute.GetSystemAttributes(intOrgID, ExcludeSearchable, LocaleId, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
        public static string GetSystemAttributes(SqlInt32 intOrgID, SqlBoolean ExcludeSearchable, SqlInt32 LocaleId, IDbConnection connection)
		{
			IDbTransaction transaction = null;
            return Attribute.GetSystemAttributes(intOrgID, ExcludeSearchable, LocaleId, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetSystemAttributes(SqlInt32 intOrgID, SqlBoolean ExcludeSearchable, SqlInt32 LocaleId, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
                    return SqlAttribute.GetSystemAttributes(intOrgID, ExcludeSearchable, LocaleId, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Attribute.GetSystemAttributes for this provider: "+providerName);
					throw new ApplicationException("No implementation of Attribute.GetSystemAttributes for this provider: "+providerName);
			}
		}

	}
}		
