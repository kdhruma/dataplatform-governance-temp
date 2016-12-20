
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
	public class ImportExport
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private ImportExport()
		{
		}
		
		
		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetJobsColumnDropDown(SqlString SEARCH, SqlString JOBTYPE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return ImportExport.GetJobsColumnDropDown(SEARCH, JOBTYPE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetJobsColumnDropDown(SqlString SEARCH, SqlString JOBTYPE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return ImportExport.GetJobsColumnDropDown(SEARCH, JOBTYPE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetJobsColumnDropDown(SqlString SEARCH, SqlString JOBTYPE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlImportExport.GetJobsColumnDropDown(SEARCH, JOBTYPE, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.GetJobsColumnDropDown for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.GetJobsColumnDropDown for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetProfilesData(SqlString nvchrDomain, SqlString nvchrSearchStr, SqlString nvchrPermissionAction, SqlString nvchrUser)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return ImportExport.GetProfilesData(nvchrDomain, nvchrSearchStr, nvchrPermissionAction, nvchrUser, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetProfilesData(SqlString nvchrDomain, SqlString nvchrSearchStr, SqlString nvchrPermissionAction, SqlString nvchrUser, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return ImportExport.GetProfilesData(nvchrDomain, nvchrSearchStr, nvchrPermissionAction, nvchrUser, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetProfilesData(SqlString nvchrDomain, SqlString nvchrSearchStr, SqlString nvchrPermissionAction, SqlString nvchrUser, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlImportExport.GetProfilesData(nvchrDomain, nvchrSearchStr, nvchrPermissionAction, nvchrUser, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.GetProfilesData for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.GetProfilesData for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetProfile(SqlString nvchrName, SqlString nvchrDomain)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return ImportExport.GetProfile(nvchrName, nvchrDomain, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetProfile(SqlString nvchrName, SqlString nvchrDomain, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return ImportExport.GetProfile(nvchrName, nvchrDomain, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetProfile(SqlString nvchrName, SqlString nvchrDomain, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlImportExport.GetProfile(nvchrName, nvchrDomain, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.GetProfile for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.GetProfile for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetProfileByID(SqlInt32 intProfileID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return ImportExport.GetProfileByID(intProfileID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetProfileByID(SqlInt32 intProfileID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return ImportExport.GetProfileByID(intProfileID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetProfileByID(SqlInt32 intProfileID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlImportExport.GetProfileByID(intProfileID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.GetProfileByID for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.GetProfileByID for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void SaveProfile(SqlString nvchrName, SqlString nvchrDomain, SqlString ntextProfile, SqlString nvarcharFileType, SqlInt32 PK_Profile, SqlInt32 PK_Org, SqlInt32 PK_Catalog, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			ImportExport.SaveProfile(nvchrName, nvchrDomain, ntextProfile, nvarcharFileType, PK_Profile, PK_Org, PK_Catalog, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);
		}

        /// <summary>
        /// 
        /// </summary>
        public static void GetProfileDependencies(SqlString ProfileName, out SqlInt32 RETURN_VALUE)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            ImportExport.GetProfileDependencies(ProfileName, out RETURN_VALUE, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void GetProfileDependencies(SqlString ProfileName, out SqlInt32 RETURN_VALUE, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            ImportExport.GetProfileDependencies(ProfileName, out RETURN_VALUE, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void GetProfileDependencies(SqlString ProfileName, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    SqlImportExport.GetProfileDependencies(ProfileName, out RETURN_VALUE, connection, transaction);

                    break;

                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.GetProfileDependencies for this provider: " + providerName);
                    throw new ApplicationException("No implementation of ImportExport.GetProfileDependencies for this provider: " + providerName);
            }
        }

		/// <summary>
        /// 
        /// </summary>
		public static void SaveProfile(SqlString nvchrName, SqlString nvchrDomain, SqlString ntextProfile, SqlString nvarcharFileType, SqlInt32 PK_Profile, SqlInt32 PK_Org, SqlInt32 PK_Catalog, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			ImportExport.SaveProfile(nvchrName, nvchrDomain, ntextProfile, nvarcharFileType, PK_Profile, PK_Org, PK_Catalog, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void SaveProfile(SqlString nvchrName, SqlString nvchrDomain, SqlString ntextProfile, SqlString nvarcharFileType, SqlInt32 PK_Profile, SqlInt32 PK_Org, SqlInt32 PK_Catalog, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlImportExport.SaveProfile(nvchrName, nvchrDomain, ntextProfile, nvarcharFileType, PK_Profile, PK_Org, PK_Catalog, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.SaveProfile for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.SaveProfile for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteProfile(SqlString nvchrprofileID, SqlString nvchrDomain, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			ImportExport.DeleteProfile(nvchrprofileID, nvchrDomain, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteProfile(SqlString nvchrprofileID, SqlString nvchrDomain, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			ImportExport.DeleteProfile(nvchrprofileID, nvchrDomain, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void DeleteProfile(SqlString nvchrprofileID, SqlString nvchrDomain, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlImportExport.DeleteProfile(nvchrprofileID, nvchrDomain, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.DeleteProfile for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.DeleteProfile for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void SaveProfileAtt(SqlInt32 ProfileID, SqlString nvchrName, SqlString nvchrDomain, SqlString ntextProfile, SqlString nvarcharFileType, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			ImportExport.SaveProfileAtt(ProfileID, nvchrName, nvchrDomain, ntextProfile, nvarcharFileType, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void SaveProfileAtt(SqlInt32 ProfileID, SqlString nvchrName, SqlString nvchrDomain, SqlString ntextProfile, SqlString nvarcharFileType, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			ImportExport.SaveProfileAtt(ProfileID, nvchrName, nvchrDomain, ntextProfile, nvarcharFileType, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void SaveProfileAtt(SqlInt32 ProfileID, SqlString nvchrName, SqlString nvchrDomain, SqlString ntextProfile, SqlString nvarcharFileType, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlImportExport.SaveProfileAtt(ProfileID, nvchrName, nvchrDomain, ntextProfile, nvarcharFileType, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.SaveProfileAtt for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.SaveProfileAtt for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetProfileAtt(SqlString nvchrName, SqlString nvchrDomain)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return ImportExport.GetProfileAtt(nvchrName, nvchrDomain, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetProfileAtt(SqlString nvchrName, SqlString nvchrDomain, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return ImportExport.GetProfileAtt(nvchrName, nvchrDomain, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetProfileAtt(SqlString nvchrName, SqlString nvchrDomain, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlImportExport.GetProfileAtt(nvchrName, nvchrDomain, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.GetProfileAtt for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.GetProfileAtt for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetProfilesDataAtt(SqlString nvchrDomain, SqlString nvchrSearchStr, SqlString nvchrPermissionAction, SqlString nvchrUser)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return ImportExport.GetProfilesDataAtt(nvchrDomain, nvchrSearchStr, nvchrPermissionAction, nvchrUser, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetProfilesDataAtt(SqlString nvchrDomain, SqlString nvchrSearchStr, SqlString nvchrPermissionAction, SqlString nvchrUser, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return ImportExport.GetProfilesDataAtt(nvchrDomain, nvchrSearchStr, nvchrPermissionAction, nvchrUser, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetProfilesDataAtt(SqlString nvchrDomain, SqlString nvchrSearchStr, SqlString nvchrPermissionAction, SqlString nvchrUser, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlImportExport.GetProfilesDataAtt(nvchrDomain, nvchrSearchStr, nvchrPermissionAction, nvchrUser, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.GetProfilesDataAtt for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.GetProfilesDataAtt for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAttributeExpJob(SqlInt32 intJobId)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return ImportExport.GetAttributeExpJob(intJobId, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAttributeExpJob(SqlInt32 intJobId, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return ImportExport.GetAttributeExpJob(intJobId, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetAttributeExpJob(SqlInt32 intJobId, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlImportExport.GetAttributeExpJob(intJobId, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.GetAttributeExpJob for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.GetAttributeExpJob for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void SaveProfileTaxonomy(SqlString nvchrName, SqlString nvchrDomain, SqlString ntextProfile, SqlString nvarcharFileType, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			ImportExport.SaveProfileTaxonomy(nvchrName, nvchrDomain, ntextProfile, nvarcharFileType, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void SaveProfileTaxonomy(SqlString nvchrName, SqlString nvchrDomain, SqlString ntextProfile, SqlString nvarcharFileType, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			ImportExport.SaveProfileTaxonomy(nvchrName, nvchrDomain, ntextProfile, nvarcharFileType, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void SaveProfileTaxonomy(SqlString nvchrName, SqlString nvchrDomain, SqlString ntextProfile, SqlString nvarcharFileType, SqlString vchrUserID, SqlString vchrProgramName, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlImportExport.SaveProfileTaxonomy(nvchrName, nvchrDomain, ntextProfile, nvarcharFileType, vchrUserID, vchrProgramName, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.SaveProfileTaxonomy for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.SaveProfileTaxonomy for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetProfileTypes(SqlBoolean bitCanExecuteFilter)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return ImportExport.GetProfileTypes(bitCanExecuteFilter, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetProfileTypes(SqlBoolean bitCanExecuteFilter, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return ImportExport.GetProfileTypes(bitCanExecuteFilter, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetProfileTypes(SqlBoolean bitCanExecuteFilter, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlImportExport.GetProfileTypes(bitCanExecuteFilter, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.GetProfileTypes for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.GetProfileTypes for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetProfileGroups(SqlString vchrUserLogin)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return ImportExport.GetProfileGroups(vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetProfileGroups(SqlString vchrUserLogin, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return ImportExport.GetProfileGroups(vchrUserLogin, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetProfileGroups(SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlImportExport.GetProfileGroups(vchrUserLogin, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.GetProfileGroups for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.GetProfileGroups for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetProfiles(SqlString nvchrUser, SqlInt32 PK_Profiles, SqlString nvchrProfileName, SqlInt32 PK_ProfileType, SqlString nvchrTypeShortName, SqlString nvchrSearchStr, SqlBoolean bitIncludeData)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return ImportExport.GetProfiles(nvchrUser, PK_Profiles, nvchrProfileName, PK_ProfileType, nvchrTypeShortName, nvchrSearchStr, bitIncludeData, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetProfiles(SqlString nvchrUser, SqlInt32 PK_Profiles, SqlString nvchrProfileName, SqlInt32 PK_ProfileType, SqlString nvchrTypeShortName, SqlString nvchrSearchStr, SqlBoolean bitIncludeData, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return ImportExport.GetProfiles(nvchrUser, PK_Profiles, nvchrProfileName, PK_ProfileType, nvchrTypeShortName, nvchrSearchStr, bitIncludeData, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetProfiles(SqlString nvchrUser, SqlInt32 PK_Profiles, SqlString nvchrProfileName, SqlInt32 PK_ProfileType, SqlString nvchrTypeShortName, SqlString nvchrSearchStr, SqlBoolean bitIncludeData, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlImportExport.GetProfiles(nvchrUser, PK_Profiles, nvchrProfileName, PK_ProfileType, nvchrTypeShortName, nvchrSearchStr, bitIncludeData, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.GetProfiles for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.GetProfiles for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetSubscribers(SqlInt32 PK_Subscriber, SqlString nvchrSubscriberName, SqlString nvchrSearchStr, SqlBoolean Inbound, SqlBoolean Outbound)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return ImportExport.GetSubscribers(PK_Subscriber, nvchrSubscriberName, nvchrSearchStr, Inbound, Outbound, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetSubscribers(SqlInt32 PK_Subscriber, SqlString nvchrSubscriberName, SqlString nvchrSearchStr, SqlBoolean Inbound, SqlBoolean Outbound, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return ImportExport.GetSubscribers(PK_Subscriber, nvchrSubscriberName, nvchrSearchStr, Inbound, Outbound, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetSubscribers(SqlInt32 PK_Subscriber, SqlString nvchrSubscriberName, SqlString nvchrSearchStr, SqlBoolean Inbound, SqlBoolean Outbound, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlImportExport.GetSubscribers(PK_Subscriber, nvchrSubscriberName, nvchrSearchStr, Inbound, Outbound, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.GetSubscribers for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.GetSubscribers for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void SetSubscriber(SqlInt32 PK_Subscriber, SqlString ShortName, SqlInt32 FK_ChannelType, SqlString URL, SqlInt32 Port, SqlString Directory, SqlString UserName, SqlString Password, SqlString EMail, SqlString Action, SqlString LoginUser, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			ImportExport.SetSubscriber(PK_Subscriber, ShortName, FK_ChannelType, URL, Port, Directory, UserName, Password, EMail, Action, LoginUser, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void SetSubscriber(SqlInt32 PK_Subscriber, SqlString ShortName, SqlInt32 FK_ChannelType, SqlString URL, SqlInt32 Port, SqlString Directory, SqlString UserName, SqlString Password, SqlString EMail, SqlString Action, SqlString LoginUser, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			ImportExport.SetSubscriber(PK_Subscriber, ShortName, FK_ChannelType, URL, Port, Directory, UserName, Password, EMail, Action, LoginUser, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void SetSubscriber(SqlInt32 PK_Subscriber, SqlString ShortName, SqlInt32 FK_ChannelType, SqlString URL, SqlInt32 Port, SqlString Directory, SqlString UserName, SqlString Password, SqlString EMail, SqlString Action, SqlString LoginUser, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlImportExport.SetSubscriber(PK_Subscriber, ShortName, FK_ChannelType, URL, Port, Directory, UserName, Password, EMail, Action, LoginUser, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.SetSubscriber for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.SetSubscriber for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable Subscriber_ConfirmDelete(SqlString SubscriberIDs)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return ImportExport.Subscriber_ConfirmDelete(SubscriberIDs, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable Subscriber_ConfirmDelete(SqlString SubscriberIDs, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return ImportExport.Subscriber_ConfirmDelete(SubscriberIDs, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable Subscriber_ConfirmDelete(SqlString SubscriberIDs, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlImportExport.Subscriber_ConfirmDelete(SubscriberIDs, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.Subscriber_ConfirmDelete for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.Subscriber_ConfirmDelete for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetChannels()
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return ImportExport.GetChannels(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetChannels(IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return ImportExport.GetChannels(connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetChannels(IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlImportExport.GetChannels(connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.GetChannels for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.GetChannels for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetExportProfileSubscribers(SqlInt32 PK_Profiles, SqlString nvchrProfileName, SqlBoolean bitIncludeData)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return ImportExport.GetExportProfileSubscribers(PK_Profiles, nvchrProfileName, bitIncludeData, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetExportProfileSubscribers(SqlInt32 PK_Profiles, SqlString nvchrProfileName, SqlBoolean bitIncludeData, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return ImportExport.GetExportProfileSubscribers(PK_Profiles, nvchrProfileName, bitIncludeData, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetExportProfileSubscribers(SqlInt32 PK_Profiles, SqlString nvchrProfileName, SqlBoolean bitIncludeData, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlImportExport.GetExportProfileSubscribers(PK_Profiles, nvchrProfileName, bitIncludeData, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.GetExportProfileSubscribers for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.GetExportProfileSubscribers for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void SetExportProfileSubscribers(SqlString nvchrProfileName, SqlString txtXML, SqlString nvchrLoginUser)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			ImportExport.SetExportProfileSubscribers(nvchrProfileName, txtXML, nvchrLoginUser, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void SetExportProfileSubscribers(SqlString nvchrProfileName, SqlString txtXML, SqlString nvchrLoginUser, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			ImportExport.SetExportProfileSubscribers(nvchrProfileName, txtXML, nvchrLoginUser, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void SetExportProfileSubscribers(SqlString nvchrProfileName, SqlString txtXML, SqlString nvchrLoginUser, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlImportExport.SetExportProfileSubscribers(nvchrProfileName, txtXML, nvchrLoginUser, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.SetExportProfileSubscribers for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.SetExportProfileSubscribers for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet BulkProcessCNodes(SqlInt32 intOrgID, SqlInt32 intCatalogID, SqlInt32 intJobServiceID, SqlString vchrUserID, SqlInt32 intSupplierID, SqlString vchrProgramName)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return ImportExport.BulkProcessCNodes(intOrgID, intCatalogID, intJobServiceID, vchrUserID, intSupplierID, vchrProgramName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet BulkProcessCNodes(SqlInt32 intOrgID, SqlInt32 intCatalogID, SqlInt32 intJobServiceID, SqlString vchrUserID, SqlInt32 intSupplierID, SqlString vchrProgramName, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return ImportExport.BulkProcessCNodes(intOrgID, intCatalogID, intJobServiceID, vchrUserID, intSupplierID, vchrProgramName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet BulkProcessCNodes(SqlInt32 intOrgID, SqlInt32 intCatalogID, SqlInt32 intJobServiceID, SqlString vchrUserID, SqlInt32 intSupplierID, SqlString vchrProgramName, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlImportExport.BulkProcessCNodes(intOrgID, intCatalogID, intJobServiceID, vchrUserID, intSupplierID, vchrProgramName, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.BulkProcessCNodes for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.BulkProcessCNodes for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet BulkProcessCommonAttributes(SqlInt32 intOrgID, SqlInt32 intCatalogID, SqlInt32 intJobServiceID, SqlString vchrUserID, SqlString vchrProgramName)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return ImportExport.BulkProcessCommonAttributes(intOrgID, intCatalogID, intJobServiceID, vchrUserID, vchrProgramName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet BulkProcessCommonAttributes(SqlInt32 intOrgID, SqlInt32 intCatalogID, SqlInt32 intJobServiceID, SqlString vchrUserID, SqlString vchrProgramName, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return ImportExport.BulkProcessCommonAttributes(intOrgID, intCatalogID, intJobServiceID, vchrUserID, vchrProgramName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet BulkProcessCommonAttributes(SqlInt32 intOrgID, SqlInt32 intCatalogID, SqlInt32 intJobServiceID, SqlString vchrUserID, SqlString vchrProgramName, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlImportExport.BulkProcessCommonAttributes(intOrgID, intCatalogID, intJobServiceID, vchrUserID, vchrProgramName, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.BulkProcessCommonAttributes for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.BulkProcessCommonAttributes for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet BulkProcessTechAttributes(SqlInt32 intOrgID, SqlInt32 intCatalogID, SqlInt32 intJobServiceID, SqlString vchrUserID, SqlString vchrProgramName)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return ImportExport.BulkProcessTechAttributes(intOrgID, intCatalogID, intJobServiceID, vchrUserID, vchrProgramName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet BulkProcessTechAttributes(SqlInt32 intOrgID, SqlInt32 intCatalogID, SqlInt32 intJobServiceID, SqlString vchrUserID, SqlString vchrProgramName, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return ImportExport.BulkProcessTechAttributes(intOrgID, intCatalogID, intJobServiceID, vchrUserID, vchrProgramName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet BulkProcessTechAttributes(SqlInt32 intOrgID, SqlInt32 intCatalogID, SqlInt32 intJobServiceID, SqlString vchrUserID, SqlString vchrProgramName, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlImportExport.BulkProcessTechAttributes(intOrgID, intCatalogID, intJobServiceID, vchrUserID, vchrProgramName, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.BulkProcessTechAttributes for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.BulkProcessTechAttributes for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet BulkProcessRelationships(SqlInt32 intOrgID, SqlInt32 intCatalogID, SqlInt32 intJobServiceID, SqlString vchrUserID, SqlString vchrProgramName)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return ImportExport.BulkProcessRelationships(intOrgID, intCatalogID, intJobServiceID, vchrUserID, vchrProgramName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet BulkProcessRelationships(SqlInt32 intOrgID, SqlInt32 intCatalogID, SqlInt32 intJobServiceID, SqlString vchrUserID, SqlString vchrProgramName, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return ImportExport.BulkProcessRelationships(intOrgID, intCatalogID, intJobServiceID, vchrUserID, vchrProgramName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet BulkProcessRelationships(SqlInt32 intOrgID, SqlInt32 intCatalogID, SqlInt32 intJobServiceID, SqlString vchrUserID, SqlString vchrProgramName, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlImportExport.BulkProcessRelationships(intOrgID, intCatalogID, intJobServiceID, vchrUserID, vchrProgramName, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.BulkProcessRelationships for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.BulkProcessRelationships for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable FindImportJobLog(SqlString nvchrJobID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return ImportExport.FindImportJobLog(nvchrJobID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable FindImportJobLog(SqlString nvchrJobID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return ImportExport.FindImportJobLog(nvchrJobID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable FindImportJobLog(SqlString nvchrJobID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlImportExport.FindImportJobLog(nvchrJobID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.FindImportJobLog for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.FindImportJobLog for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet ImportJobErrorLogExists(SqlString nvchrJobID)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return ImportExport.ImportJobErrorLogExists(nvchrJobID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet ImportJobErrorLogExists(SqlString nvchrJobID, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return ImportExport.ImportJobErrorLogExists(nvchrJobID, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet ImportJobErrorLogExists(SqlString nvchrJobID, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlImportExport.ImportJobErrorLogExists(nvchrJobID, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.ImportJobErrorLogExists for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.ImportJobErrorLogExists for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet ExportLookupTables(SqlString vchrInput)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return ImportExport.ExportLookupTables(vchrInput, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet ExportLookupTables(SqlString vchrInput, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return ImportExport.ExportLookupTables(vchrInput, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet ExportLookupTables(SqlString vchrInput, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlImportExport.ExportLookupTables(vchrInput, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.ExportLookupTables for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.ExportLookupTables for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetCatalogCharacteristicTemplate(SqlInt32 CatalogId, SqlInt32 CNodeId, SqlInt32 LocaleId, SqlBoolean IncludeComplexAttrChildren)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return ImportExport.GetCatalogCharacteristicTemplate(CatalogId, CNodeId, LocaleId, IncludeComplexAttrChildren, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetCatalogCharacteristicTemplate(SqlInt32 CatalogId, SqlInt32 CNodeId, SqlInt32 LocaleId, SqlBoolean IncludeComplexAttrChildren, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return ImportExport.GetCatalogCharacteristicTemplate(CatalogId, CNodeId, LocaleId, IncludeComplexAttrChildren, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetCatalogCharacteristicTemplate(SqlInt32 CatalogId, SqlInt32 CNodeId, SqlInt32 LocaleId, SqlBoolean IncludeComplexAttrChildren, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlImportExport.GetCatalogCharacteristicTemplate(CatalogId, CNodeId, LocaleId, IncludeComplexAttrChildren, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.GetCatalogCharacteristicTemplate for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.GetCatalogCharacteristicTemplate for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCatalogCharacteristicTemplateXml(SqlInt32 CatalogId, SqlInt32 CNodeId, SqlInt32 LocaleId, SqlBoolean IncludeComplexAttrChildren)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return ImportExport.GetCatalogCharacteristicTemplateXml(CatalogId, CNodeId, LocaleId, IncludeComplexAttrChildren, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCatalogCharacteristicTemplateXml(SqlInt32 CatalogId, SqlInt32 CNodeId, SqlInt32 LocaleId, SqlBoolean IncludeComplexAttrChildren, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return ImportExport.GetCatalogCharacteristicTemplateXml(CatalogId, CNodeId, LocaleId, IncludeComplexAttrChildren, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string GetCatalogCharacteristicTemplateXml(SqlInt32 CatalogId, SqlInt32 CNodeId, SqlInt32 LocaleId, SqlBoolean IncludeComplexAttrChildren, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlImportExport.GetCatalogCharacteristicTemplateXml(CatalogId, CNodeId, LocaleId, IncludeComplexAttrChildren, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.GetCatalogCharacteristicTemplateXml for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.GetCatalogCharacteristicTemplateXml for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet ObjectExport(SqlString ObjectName)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return ImportExport.ObjectExport(ObjectName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet ObjectExport(SqlString ObjectName, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return ImportExport.ObjectExport(ObjectName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataSet ObjectExport(SqlString ObjectName, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlImportExport.ObjectExport(ObjectName, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.ObjectExport for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.ObjectExport for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable FindAggregationJobs(SqlString LoginUser)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return ImportExport.FindAggregationJobs(LoginUser, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable FindAggregationJobs(SqlString LoginUser, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return ImportExport.FindAggregationJobs(LoginUser, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable FindAggregationJobs(SqlString LoginUser, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlImportExport.FindAggregationJobs(LoginUser, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.FindAggregationJobs for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.FindAggregationJobs for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetExcelExportColumnNames(SqlInt32 FK_Catalog, SqlInt32 FK_JobService)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return ImportExport.GetExcelExportColumnNames(FK_Catalog, FK_JobService, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetExcelExportColumnNames(SqlInt32 FK_Catalog, SqlInt32 FK_JobService, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return ImportExport.GetExcelExportColumnNames(FK_Catalog, FK_JobService, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetExcelExportColumnNames(SqlInt32 FK_Catalog, SqlInt32 FK_JobService, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlImportExport.GetExcelExportColumnNames(FK_Catalog, FK_JobService, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.GetExcelExportColumnNames for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.GetExcelExportColumnNames for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static string ImportGetProfileSubscriber(SqlInt32 fk_profile)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return ImportExport.ImportGetProfileSubscriber(fk_profile, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string ImportGetProfileSubscriber(SqlInt32 fk_profile, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return ImportExport.ImportGetProfileSubscriber(fk_profile, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static string ImportGetProfileSubscriber(SqlInt32 fk_profile, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlImportExport.ImportGetProfileSubscriber(fk_profile, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.ImportGetProfileSubscriber for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.ImportGetProfileSubscriber for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ImportInsertOrUpdateProfileSubscriber(SqlInt32 fk_profile, SqlString profileXml, SqlString subscriberList, SqlString userId)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			ImportExport.ImportInsertOrUpdateProfileSubscriber(fk_profile, profileXml, subscriberList, userId, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ImportInsertOrUpdateProfileSubscriber(SqlInt32 fk_profile, SqlString profileXml, SqlString subscriberList, SqlString userId, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			ImportExport.ImportInsertOrUpdateProfileSubscriber(fk_profile, profileXml, subscriberList, userId, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ImportInsertOrUpdateProfileSubscriber(SqlInt32 fk_profile, SqlString profileXml, SqlString subscriberList, SqlString userId, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlImportExport.ImportInsertOrUpdateProfileSubscriber(fk_profile, profileXml, subscriberList, userId, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.ImportInsertOrUpdateProfileSubscriber for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.ImportInsertOrUpdateProfileSubscriber for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ImportProductGetJobStatus(SqlString jobtype, SqlString status, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			ImportExport.ImportProductGetJobStatus(jobtype, status, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ImportProductGetJobStatus(SqlString jobtype, SqlString status, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			ImportExport.ImportProductGetJobStatus(jobtype, status, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void ImportProductGetJobStatus(SqlString jobtype, SqlString status, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlImportExport.ImportProductGetJobStatus(jobtype, status, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of ImportExport.ImportProductGetJobStatus for this provider: "+providerName);
					throw new ApplicationException("No implementation of ImportExport.ImportProductGetJobStatus for this provider: "+providerName);
			}
		}

	}
}		
