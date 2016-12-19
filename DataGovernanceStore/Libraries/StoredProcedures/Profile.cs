
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
	public class Profile
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private Profile()
		{
		}
		
		
		/// <summary>
        /// 
        /// </summary>
		public static void CheckProfileDuplicate(SqlInt32 pId, SqlString pName, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Profile.CheckProfileDuplicate(pId, pName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void CheckProfileDuplicate(SqlInt32 pId, SqlString pName, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Profile.CheckProfileDuplicate(pId, pName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void CheckProfileDuplicate(SqlInt32 pId, SqlString pName, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlProfile.CheckProfileDuplicate(pId, pName, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Profile.CheckProfileDuplicate for this provider: "+providerName);
					throw new ApplicationException("No implementation of Profile.CheckProfileDuplicate for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetProfilePermissions(SqlString vchrUserLogin, SqlString ObjectTypeSN)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Profile.GetProfilePermissions(vchrUserLogin, ObjectTypeSN, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetProfilePermissions(SqlString vchrUserLogin, SqlString ObjectTypeSN, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Profile.GetProfilePermissions(vchrUserLogin, ObjectTypeSN, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetProfilePermissions(SqlString vchrUserLogin, SqlString ObjectTypeSN, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlProfile.GetProfilePermissions(vchrUserLogin, ObjectTypeSN, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Profile.GetProfilePermissions for this provider: "+providerName);
					throw new ApplicationException("No implementation of Profile.GetProfilePermissions for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetProfileGroupPermissions(SqlString vchrUserLogin, SqlString vchrProfileType)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Profile.GetProfileGroupPermissions(vchrUserLogin, vchrProfileType, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetProfileGroupPermissions(SqlString vchrUserLogin, SqlString vchrProfileType, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Profile.GetProfileGroupPermissions(vchrUserLogin, vchrProfileType, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetProfileGroupPermissions(SqlString vchrUserLogin, SqlString vchrProfileType, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlProfile.GetProfileGroupPermissions(vchrUserLogin, vchrProfileType, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Profile.GetProfileGroupPermissions for this provider: "+providerName);
					throw new ApplicationException("No implementation of Profile.GetProfileGroupPermissions for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetExportProfiles(SqlInt32 PK_Profile, SqlInt32 FK_RuleSet, SqlString ProfileName)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Profile.GetExportProfiles(PK_Profile, FK_RuleSet, ProfileName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetExportProfiles(SqlInt32 PK_Profile, SqlInt32 FK_RuleSet, SqlString ProfileName, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Profile.GetExportProfiles(PK_Profile, FK_RuleSet, ProfileName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetExportProfiles(SqlInt32 PK_Profile, SqlInt32 FK_RuleSet, SqlString ProfileName, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlProfile.GetExportProfiles(PK_Profile, FK_RuleSet, ProfileName, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Profile.GetExportProfiles for this provider: "+providerName);
					throw new ApplicationException("No implementation of Profile.GetExportProfiles for this provider: "+providerName);
			}
		}

	}
}		
