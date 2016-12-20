
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
	public class CNode
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private CNode()
		{
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetMatchedBulkRelationshipTypesDT(SqlString vchrUserLogin, SqlString PK_CNodeGroup, SqlInt32 FK_Catalog)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return CNode.GetMatchedBulkRelationshipTypesDT(vchrUserLogin, PK_CNodeGroup, FK_Catalog, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetMatchedBulkRelationshipTypesDT(SqlString vchrUserLogin, SqlString PK_CNodeGroup, SqlInt32 FK_Catalog, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return CNode.GetMatchedBulkRelationshipTypesDT(vchrUserLogin, PK_CNodeGroup, FK_Catalog, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetMatchedBulkRelationshipTypesDT(SqlString vchrUserLogin, SqlString PK_CNodeGroup, SqlInt32 FK_Catalog, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCNode.GetMatchedBulkRelationshipTypesDT(vchrUserLogin, PK_CNodeGroup, FK_Catalog, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of CNode.GetMatchedBulkRelationshipTypesDT for this provider: "+providerName);
					throw new ApplicationException("No implementation of CNode.GetMatchedBulkRelationshipTypesDT for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetRelationshipTypesDT(SqlInt32 FK_Catalog, SqlInt32 FK_NodeType)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return CNode.GetRelationshipTypesDT(FK_Catalog, FK_NodeType, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetRelationshipTypesDT(SqlInt32 FK_Catalog, SqlInt32 FK_NodeType, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return CNode.GetRelationshipTypesDT(FK_Catalog, FK_NodeType, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet GetRelationshipTypesDT(SqlInt32 FK_Catalog, SqlInt32 FK_NodeType, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCNode.GetRelationshipTypesDT(FK_Catalog, FK_NodeType, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of CNode.GetRelationshipTypesDT for this provider: "+providerName);
					throw new ApplicationException("No implementation of CNode.GetRelationshipTypesDT for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAllRelationshipTypesDT(SqlInt32 FK_Catalog)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return CNode.GetAllRelationshipTypesDT(FK_Catalog, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAllRelationshipTypesDT(SqlInt32 FK_Catalog, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return CNode.GetAllRelationshipTypesDT(FK_Catalog, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAllRelationshipTypesDT(SqlInt32 FK_Catalog, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCNode.GetAllRelationshipTypesDT(FK_Catalog, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of CNode.GetAllRelationshipTypesDT for this provider: "+providerName);
					throw new ApplicationException("No implementation of CNode.GetAllRelationshipTypesDT for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void AssignCNodesToUser(SqlInt32 intCatalogID, SqlString vchrLoadCategory, SqlString vchrLoadType, SqlInt32 FK_Status, SqlString vchrCurrentAssignmentStatus, SqlInt32 intBatchSize, SqlString vchrCNodeList, SqlString nvchrAssignToUser, SqlInt32 intOverRide, ref SqlString vchrReturnXML, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			CNode.AssignCNodesToUser(intCatalogID, vchrLoadCategory, vchrLoadType, FK_Status, vchrCurrentAssignmentStatus, intBatchSize, vchrCNodeList, nvchrAssignToUser, intOverRide, ref vchrReturnXML, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void AssignCNodesToUser(SqlInt32 intCatalogID, SqlString vchrLoadCategory, SqlString vchrLoadType, SqlInt32 FK_Status, SqlString vchrCurrentAssignmentStatus, SqlInt32 intBatchSize, SqlString vchrCNodeList, SqlString nvchrAssignToUser, SqlInt32 intOverRide, ref SqlString vchrReturnXML, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			CNode.AssignCNodesToUser(intCatalogID, vchrLoadCategory, vchrLoadType, FK_Status, vchrCurrentAssignmentStatus, intBatchSize, vchrCNodeList, nvchrAssignToUser, intOverRide, ref vchrReturnXML, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void AssignCNodesToUser(SqlInt32 intCatalogID, SqlString vchrLoadCategory, SqlString vchrLoadType, SqlInt32 FK_Status, SqlString vchrCurrentAssignmentStatus, SqlInt32 intBatchSize, SqlString vchrCNodeList, SqlString nvchrAssignToUser, SqlInt32 intOverRide, ref SqlString vchrReturnXML, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlCNode.AssignCNodesToUser(intCatalogID, vchrLoadCategory, vchrLoadType, FK_Status, vchrCurrentAssignmentStatus, intBatchSize, vchrCNodeList, nvchrAssignToUser, intOverRide, ref vchrReturnXML, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of CNode.AssignCNodesToUser for this provider: "+providerName);
					throw new ApplicationException("No implementation of CNode.AssignCNodesToUser for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetCNodeComplexAttributeData(SqlInt32 intAttrID, SqlString nvarCNodeIDs,SqlInt32 catalogId, SqlInt32 FK_Locale)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return CNode.GetCNodeComplexAttributeData(intAttrID, nvarCNodeIDs,catalogId, FK_Locale, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
        public static DataTable GetCNodeComplexAttributeData(SqlInt32 intAttrID, SqlString nvarCNodeIDs, SqlInt32 catalogId, SqlInt32 FK_Locale, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return CNode.GetCNodeComplexAttributeData(intAttrID, nvarCNodeIDs,catalogId, FK_Locale, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
        public static DataTable GetCNodeComplexAttributeData(SqlInt32 intAttrID, SqlString nvarCNodeIDs, SqlInt32 catalogId, SqlInt32 FK_Locale, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCNode.GetCNodeComplexAttributeData(intAttrID, nvarCNodeIDs,catalogId, FK_Locale, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of CNode.GetCNodeComplexAttributeData for this provider: "+providerName);
					throw new ApplicationException("No implementation of CNode.GetCNodeComplexAttributeData for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void PromoteCNodesToMasterCatalog(SqlInt32 intCatalogID, SqlString vchrLoadCategory, SqlString vchrLoadType, SqlString vchrCNodeList, SqlString vchrUserLogin, SqlInt32 intJobID, SqlInt32 intBatchSize, ref SqlString vchrReturnXML, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			CNode.PromoteCNodesToMasterCatalog(intCatalogID, vchrLoadCategory, vchrLoadType, vchrCNodeList, vchrUserLogin, intJobID, intBatchSize, ref vchrReturnXML, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void PromoteCNodesToMasterCatalog(SqlInt32 intCatalogID, SqlString vchrLoadCategory, SqlString vchrLoadType, SqlString vchrCNodeList, SqlString vchrUserLogin, SqlInt32 intJobID, SqlInt32 intBatchSize, ref SqlString vchrReturnXML, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			CNode.PromoteCNodesToMasterCatalog(intCatalogID, vchrLoadCategory, vchrLoadType, vchrCNodeList, vchrUserLogin, intJobID, intBatchSize, ref vchrReturnXML, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void PromoteCNodesToMasterCatalog(SqlInt32 intCatalogID, SqlString vchrLoadCategory, SqlString vchrLoadType, SqlString vchrCNodeList, SqlString vchrUserLogin, SqlInt32 intJobID, SqlInt32 intBatchSize, ref SqlString vchrReturnXML, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlCNode.PromoteCNodesToMasterCatalog(intCatalogID, vchrLoadCategory, vchrLoadType, vchrCNodeList, vchrUserLogin, intJobID, intBatchSize, ref vchrReturnXML, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of CNode.PromoteCNodesToMasterCatalog for this provider: "+providerName);
					throw new ApplicationException("No implementation of CNode.PromoteCNodesToMasterCatalog for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ProcessCnodeAssignment(SqlInt32 FK_Catalog, SqlString UserID, SqlString FromUserID, SqlString ToUserID, SqlString CnodeList, SqlInt32 NoOfItemsAssigned, SqlString SelectedLoadsCats)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return CNode.ProcessCnodeAssignment(FK_Catalog, UserID, FromUserID, ToUserID, CnodeList, NoOfItemsAssigned, SelectedLoadsCats, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ProcessCnodeAssignment(SqlInt32 FK_Catalog, SqlString UserID, SqlString FromUserID, SqlString ToUserID, SqlString CnodeList, SqlInt32 NoOfItemsAssigned, SqlString SelectedLoadsCats, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return CNode.ProcessCnodeAssignment(FK_Catalog, UserID, FromUserID, ToUserID, CnodeList, NoOfItemsAssigned, SelectedLoadsCats, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable ProcessCnodeAssignment(SqlInt32 FK_Catalog, SqlString UserID, SqlString FromUserID, SqlString ToUserID, SqlString CnodeList, SqlInt32 NoOfItemsAssigned, SqlString SelectedLoadsCats, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCNode.ProcessCnodeAssignment(FK_Catalog, UserID, FromUserID, ToUserID, CnodeList, NoOfItemsAssigned, SelectedLoadsCats, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of CNode.ProcessCnodeAssignment for this provider: "+providerName);
					throw new ApplicationException("No implementation of CNode.ProcessCnodeAssignment for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable SetEntityDeleteFlag(SqlString ItemXml, SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return CNode.SetEntityDeleteFlag(ItemXml, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable SetEntityDeleteFlag(SqlString ItemXml, SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return CNode.SetEntityDeleteFlag(ItemXml, vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable SetEntityDeleteFlag(SqlString ItemXml, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCNode.SetEntityDeleteFlag(ItemXml, vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of CNode.SetEntityDeleteFlag for this provider: "+providerName);
					throw new ApplicationException("No implementation of CNode.SetEntityDeleteFlag for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetEntityChildren(SqlInt64 FK_CNode, SqlInt32 FK_NodeType)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return CNode.GetEntityChildren(FK_CNode, FK_NodeType, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
        public static DataTable GetEntityChildren(SqlInt64 FK_CNode, SqlInt32 FK_NodeType, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return CNode.GetEntityChildren(FK_CNode, FK_NodeType, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
        public static DataTable GetEntityChildren(SqlInt64 FK_CNode, SqlInt32 FK_NodeType, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCNode.GetEntityChildren(FK_CNode, FK_NodeType, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of CNode.GetEntityChildren for this provider: "+providerName);
					throw new ApplicationException("No implementation of CNode.GetEntityChildren for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetCNodesByIds(SqlString CNodeIDs)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return CNode.GetCNodesByIds(CNodeIDs, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetCNodesByIds(SqlString CNodeIDs, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return CNode.GetCNodesByIds(CNodeIDs, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetCNodesByIds(SqlString CNodeIDs, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlCNode.GetCNodesByIds(CNodeIDs, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of CNode.GetCNodesByIds for this provider: "+providerName);
					throw new ApplicationException("No implementation of CNode.GetCNodesByIds for this provider: "+providerName);
			}
		}

	}
}		
