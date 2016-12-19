
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
	public class Events
	{
		/// <summary>
		/// Private constructor
		/// </summary>		
		private Events()
		{
		}
		
		
		/// <summary>
        /// 
        /// </summary>
        public static DataTable GetApplicationConfigXML(SqlInt32 FK_Event_Source, SqlInt32 FK_Event_Subscriber, SqlInt32 FK_Security_Role, SqlInt32 FK_Security_user, SqlInt32 FK_Org, SqlInt32 FK_Catalog, SqlInt32 FK_Category, SqlInt32 FK_CNode, SqlInt32 FK_Attribute, SqlInt32 FK_NodeType, SqlInt32 FK_RelationshipType, SqlInt32 PK_Application_Config, SqlInt32 FK_Locale, SqlString CategoryPath, SqlString LookupName)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
            return Events.GetApplicationConfigXML(FK_Event_Source, FK_Event_Subscriber, FK_Security_Role, FK_Security_user, FK_Org, FK_Catalog, FK_Category, FK_CNode, FK_Attribute, FK_NodeType, FK_RelationshipType, PK_Application_Config, FK_Locale, CategoryPath, LookupName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
        public static DataTable GetApplicationConfigXML(SqlInt32 FK_Event_Source, SqlInt32 FK_Event_Subscriber, SqlInt32 FK_Security_Role, SqlInt32 FK_Security_user, SqlInt32 FK_Org, SqlInt32 FK_Catalog, SqlInt32 FK_Category, SqlInt32 FK_CNode, SqlInt32 FK_Attribute, SqlInt32 FK_NodeType, SqlInt32 FK_RelationshipType, SqlInt32 PK_Application_Config, SqlInt32 FK_Locale, SqlString CategoryPath, SqlString LookupName, IDbConnection connection)
		{
			IDbTransaction transaction = null;
            return Events.GetApplicationConfigXML(FK_Event_Source, FK_Event_Subscriber, FK_Security_Role, FK_Security_user, FK_Org, FK_Catalog, FK_Category, FK_CNode, FK_Attribute, FK_NodeType, FK_RelationshipType, PK_Application_Config, FK_Locale, CategoryPath, LookupName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
        public static DataTable GetApplicationConfigXML(SqlInt32 FK_Event_Source, SqlInt32 FK_Event_Subscriber, SqlInt32 FK_Security_Role, SqlInt32 FK_Security_user, SqlInt32 FK_Org, SqlInt32 FK_Catalog, SqlInt32 FK_Category, SqlInt32 FK_CNode, SqlInt32 FK_Attribute, SqlInt32 FK_NodeType, SqlInt32 FK_RelationshipType, SqlInt32 PK_Application_Config, SqlInt32 FK_Locale, SqlString CategoryPath, SqlString LookupName, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
                    return SqlEvents.GetApplicationConfigXML(FK_Event_Source, FK_Event_Subscriber, FK_Security_Role, FK_Security_user, FK_Org, FK_Catalog, FK_Category, FK_CNode, FK_Attribute, FK_NodeType, FK_RelationshipType, PK_Application_Config, FK_Locale, CategoryPath, LookupName, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Events.GetApplicationConfigXML for this provider: "+providerName);
					throw new ApplicationException("No implementation of Events.GetApplicationConfigXML for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetChildApplicationConfigsXML(SqlInt32 FK_Application_ConfigParent)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Events.GetChildApplicationConfigsXML(FK_Application_ConfigParent, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetChildApplicationConfigsXML(SqlInt32 FK_Application_ConfigParent, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Events.GetChildApplicationConfigsXML(FK_Application_ConfigParent, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetChildApplicationConfigsXML(SqlInt32 FK_Application_ConfigParent, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlEvents.GetChildApplicationConfigsXML(FK_Application_ConfigParent, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Events.GetChildApplicationConfigsXML for this provider: "+providerName);
					throw new ApplicationException("No implementation of Events.GetChildApplicationConfigsXML for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetEventSources(SqlInt32 PK_Event_Source, SqlString EventSourceName)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Events.GetEventSources(PK_Event_Source, EventSourceName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetEventSources(SqlInt32 PK_Event_Source, SqlString EventSourceName, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Events.GetEventSources(PK_Event_Source, EventSourceName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetEventSources(SqlInt32 PK_Event_Source, SqlString EventSourceName, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlEvents.GetEventSources(PK_Event_Source, EventSourceName, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Events.GetEventSources for this provider: "+providerName);
					throw new ApplicationException("No implementation of Events.GetEventSources for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetEventSubscribers(SqlInt32 PK_Event_Subscriber, SqlString EventSubscriberName)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Events.GetEventSubscribers(PK_Event_Subscriber, EventSubscriberName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetEventSubscribers(SqlInt32 PK_Event_Subscriber, SqlString EventSubscriberName, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Events.GetEventSubscribers(PK_Event_Subscriber, EventSubscriberName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetEventSubscribers(SqlInt32 PK_Event_Subscriber, SqlString EventSubscriberName, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlEvents.GetEventSubscribers(PK_Event_Subscriber, EventSubscriberName, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Events.GetEventSubscribers for this provider: "+providerName);
					throw new ApplicationException("No implementation of Events.GetEventSubscribers for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetApplicationConfigTypes(SqlInt32 PK_Application_ConfigType, SqlString ApplicationConfigTypeName)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Events.GetApplicationConfigTypes(PK_Application_ConfigType, ApplicationConfigTypeName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetApplicationConfigTypes(SqlInt32 PK_Application_ConfigType, SqlString ApplicationConfigTypeName, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Events.GetApplicationConfigTypes(PK_Application_ConfigType, ApplicationConfigTypeName, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetApplicationConfigTypes(SqlInt32 PK_Application_ConfigType, SqlString ApplicationConfigTypeName, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlEvents.GetApplicationConfigTypes(PK_Application_ConfigType, ApplicationConfigTypeName, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Events.GetApplicationConfigTypes for this provider: "+providerName);
					throw new ApplicationException("No implementation of Events.GetApplicationConfigTypes for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateApplicationConfigXML(SqlInt32 FK_Application_ContextDefinition, SqlInt32 FK_Application_ConfigParent, SqlString ShortName, SqlString LongName, SqlInt32 FK_Event_Source, SqlInt32 FK_Event_Subscriber, SqlInt32 FK_Org, SqlInt32 FK_Catalog, SqlInt32 FK_Category, SqlInt32 FK_CNode, SqlInt32 FK_Attribute, SqlInt32 FK_NodeType, SqlInt32 FK_RelationshipType, SqlInt32 FK_Security_Role, SqlInt32 FK_Security_user, SqlString ConfigXML, SqlString Description, SqlString PreCondition, SqlString PostCondition, SqlString XSDSchema, SqlString SampleXML, SqlString loginUser, SqlString userProgram, SqlInt32 FK_Locale, out SqlInt32 RETURN_VALUE)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			Events.UpdateApplicationConfigXML(FK_Application_ContextDefinition, FK_Application_ConfigParent, ShortName, LongName, FK_Event_Source, FK_Event_Subscriber, FK_Org, FK_Catalog, FK_Category, FK_CNode, FK_Attribute, FK_NodeType, FK_RelationshipType, FK_Security_Role, FK_Security_user, ConfigXML, Description, PreCondition, PostCondition, XSDSchema, SampleXML, loginUser, userProgram, FK_Locale, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateApplicationConfigXML(SqlInt32 FK_Application_ContextDefinition, SqlInt32 FK_Application_ConfigParent, SqlString ShortName, SqlString LongName, SqlInt32 FK_Event_Source, SqlInt32 FK_Event_Subscriber, SqlInt32 FK_Org, SqlInt32 FK_Catalog, SqlInt32 FK_Category, SqlInt32 FK_CNode, SqlInt32 FK_Attribute, SqlInt32 FK_NodeType, SqlInt32 FK_RelationshipType, SqlInt32 FK_Security_Role, SqlInt32 FK_Security_user, SqlString ConfigXML, SqlString Description, SqlString PreCondition, SqlString PostCondition, SqlString XSDSchema, SqlString SampleXML, SqlString loginUser, SqlString userProgram, SqlInt32 FK_Locale, out SqlInt32 RETURN_VALUE, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			Events.UpdateApplicationConfigXML(FK_Application_ContextDefinition, FK_Application_ConfigParent, ShortName, LongName, FK_Event_Source, FK_Event_Subscriber, FK_Org, FK_Catalog, FK_Category, FK_CNode, FK_Attribute, FK_NodeType, FK_RelationshipType, FK_Security_Role, FK_Security_user, ConfigXML, Description, PreCondition, PostCondition, XSDSchema, SampleXML, loginUser, userProgram, FK_Locale, out RETURN_VALUE, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static void UpdateApplicationConfigXML(SqlInt32 FK_Application_ContextDefinition, SqlInt32 FK_Application_ConfigParent, SqlString ShortName, SqlString LongName, SqlInt32 FK_Event_Source, SqlInt32 FK_Event_Subscriber, SqlInt32 FK_Org, SqlInt32 FK_Catalog, SqlInt32 FK_Category, SqlInt32 FK_CNode, SqlInt32 FK_Attribute, SqlInt32 FK_NodeType, SqlInt32 FK_RelationshipType, SqlInt32 FK_Security_Role, SqlInt32 FK_Security_user, SqlString ConfigXML, SqlString Description, SqlString PreCondition, SqlString PostCondition, SqlString XSDSchema, SqlString SampleXML, SqlString loginUser, SqlString userProgram, SqlInt32 FK_Locale, out SqlInt32 RETURN_VALUE, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					SqlEvents.UpdateApplicationConfigXML(FK_Application_ContextDefinition, FK_Application_ConfigParent, ShortName, LongName, FK_Event_Source, FK_Event_Subscriber, FK_Org, FK_Catalog, FK_Category, FK_CNode, FK_Attribute, FK_NodeType, FK_RelationshipType, FK_Security_Role, FK_Security_user, ConfigXML, Description, PreCondition, PostCondition, XSDSchema, SampleXML, loginUser, userProgram, FK_Locale, out RETURN_VALUE, connection, transaction);

					break;

				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Events.UpdateApplicationConfigXML for this provider: "+providerName);
					throw new ApplicationException("No implementation of Events.UpdateApplicationConfigXML for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetMatchinRuleSets(SqlInt32 FK_Org, SqlInt32 FK_Catalog, SqlInt32 eventSource, SqlInt32 eventSubscriber, SqlInt32 fkSecurityUser)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Events.GetMatchinRuleSets(FK_Org, FK_Catalog, eventSource, eventSubscriber, fkSecurityUser, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetMatchinRuleSets(SqlInt32 FK_Org, SqlInt32 FK_Catalog, SqlInt32 eventSource, SqlInt32 eventSubscriber, SqlInt32 fkSecurityUser, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Events.GetMatchinRuleSets(FK_Org, FK_Catalog, eventSource, eventSubscriber, fkSecurityUser, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetMatchinRuleSets(SqlInt32 FK_Org, SqlInt32 FK_Catalog, SqlInt32 eventSource, SqlInt32 eventSubscriber, SqlInt32 fkSecurityUser, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlEvents.GetMatchinRuleSets(FK_Org, FK_Catalog, eventSource, eventSubscriber, fkSecurityUser, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Events.GetMatchinRuleSets for this provider: "+providerName);
					throw new ApplicationException("No implementation of Events.GetMatchinRuleSets for this provider: "+providerName);
			}
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetTranslationMemory(SqlString OrigLocale, SqlString TransLocale, SqlString OrigText)
		{		
			IDbConnection connection = null;
			IDbTransaction transaction = null;
			return Events.GetTranslationMemory(OrigLocale, TransLocale, OrigText, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetTranslationMemory(SqlString OrigLocale, SqlString TransLocale, SqlString OrigText, IDbConnection connection)
		{
			IDbTransaction transaction = null;
			return Events.GetTranslationMemory(OrigLocale, TransLocale, OrigText, connection, transaction);
		}

		/// <summary>
        /// 
        /// </summary>
		public static DataTable GetTranslationMemory(SqlString OrigLocale, SqlString TransLocale, SqlString OrigText, IDbConnection connection, IDbTransaction transaction)
		{
			string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
			switch (providerName)
			{
				case "SqlProvider":
					return SqlEvents.GetTranslationMemory(OrigLocale, TransLocale, OrigText, connection, transaction);


				default:
					MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Events.GetTranslationMemory for this provider: "+providerName);
					throw new ApplicationException("No implementation of Events.GetTranslationMemory for this provider: "+providerName);
			}
		}

	}
}		
