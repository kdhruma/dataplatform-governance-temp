
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
	public class Component
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private Component()
		{
		}
		
		/// <summary>
        /// 
        /// </summary>
		public static DataTable getEntityTypes(SqlInt32 NodeTypeParent)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Component.getEntityTypes(NodeTypeParent, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable getEntityTypes(SqlInt32 NodeTypeParent, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Component.getEntityTypes(NodeTypeParent, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable getEntityTypes(SqlInt32 NodeTypeParent, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlComponent.getEntityTypes(NodeTypeParent, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Component.getEntityTypes for this provider: "+providerName);
					throw new ApplicationException("No implementation of Component.getEntityTypes for this provider: "+providerName);
			}
		}

        ///// <summary>
        ///// 
        ///// </summary>
        //public static DataTable GetNodeTypesDT()
        //{		
        //    IDbConnection connection = null;
        //    IDbTransaction transaction = null;
        //    return Component.GetNodeTypesDT(connection, transaction);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //public static DataTable GetNodeTypesDT(IDbConnection connection)
        //{
        //    IDbTransaction transaction = null;
        //    return Component.GetNodeTypesDT(connection, transaction);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //public static DataTable GetNodeTypesDT(IDbConnection connection, IDbTransaction transaction)
        //{
        //    string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
        //    switch (providerName)
        //    {
        //        case "SqlProvider":
        //            return SqlComponent.GetNodeTypesDT(connection, transaction);


        //        default:
        //            MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Component.GetNodeTypesDT for this provider: "+providerName);
        //            throw new ApplicationException("No implementation of Component.GetNodeTypesDT for this provider: "+providerName);
        //    }
        //}

	}
}		
