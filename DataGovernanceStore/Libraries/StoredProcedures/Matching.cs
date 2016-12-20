
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
    public class Matching
    {
        /// <summary>
        /// Private constructor
        /// </summary>		
        private Matching()
        {
        }


        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetStatusItemCount(SqlXml InputXml)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Matching.GetStatusItemCount(InputXml, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetStatusItemCount(SqlXml InputXml, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Matching.GetStatusItemCount(InputXml, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetStatusItemCount(SqlXml InputXml, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlMatching.GetStatusItemCount(InputXml, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Matching.GetStatusItemCount for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Matching.GetStatusItemCount for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void MatchJob(SqlInt32 FK_Job, SqlString UserName)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            Matching.MatchJob(FK_Job, UserName, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void MatchJob(SqlInt32 FK_Job, SqlString UserName, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            Matching.MatchJob(FK_Job, UserName, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void MatchJob(SqlInt32 FK_Job, SqlString UserName, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    SqlMatching.MatchJob(FK_Job, UserName, connection, transaction);

                    break;

                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Matching.MatchJob for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Matching.MatchJob for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void DescriptionMatch(SqlXml xml, SqlString vchrUserID, SqlInt32 Type)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            Matching.DescriptionMatch(xml, vchrUserID, Type, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void DescriptionMatch(SqlXml xml, SqlString vchrUserID, SqlInt32 Type, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            Matching.DescriptionMatch(xml, vchrUserID, Type, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void DescriptionMatch(SqlXml xml, SqlString vchrUserID, SqlInt32 Type, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    SqlMatching.DescriptionMatch(xml, vchrUserID, Type, connection, transaction);

                    break;

                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Matching.DescriptionMatch for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Matching.DescriptionMatch for this provider: " + providerName);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public static String CreateServicesJob(SqlXml xml, SqlString vchrUserID, SqlInt32 ServiceType)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Matching.CreateServicesJob(xml, vchrUserID, ServiceType, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static String CreateServicesJob(SqlXml xml, SqlString vchrUserID, SqlInt32 ServiceType, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Matching.CreateServicesJob(xml, vchrUserID, ServiceType, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static String CreateServicesJob(SqlXml xml, SqlString vchrUserID, SqlInt32 ServiceType, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlMatching.CreateServicesJob(xml, vchrUserID, ServiceType, connection, transaction);
                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Matching.DescriptionMatch for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Matching.DescriptionMatch for this provider: " + providerName);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static void AttributeExtraction(SqlXml xml, SqlString vchrUserID, SqlInt32 Type)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            Matching.AttributeExtraction(xml, vchrUserID, Type, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void AttributeExtraction(SqlXml xml, SqlString vchrUserID, SqlInt32 Type, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            Matching.AttributeExtraction(xml, vchrUserID, Type, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void AttributeExtraction(SqlXml xml, SqlString vchrUserID, SqlInt32 Type, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    SqlMatching.AttributeExtraction(xml, vchrUserID, Type, connection, transaction);

                    break;

                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Matching.DescriptionMatch for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Matching.DescriptionMatch for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CreateNetricsResult(SqlInt32 FK_Job, SqlString Xmldata)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            Matching.CreateNetricsResult(FK_Job, Xmldata, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CreateNetricsResult(SqlInt32 FK_Job, SqlString Xmldata, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            Matching.CreateNetricsResult(FK_Job, Xmldata, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CreateNetricsResult(SqlInt32 FK_Job, SqlString Xmldata, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    SqlMatching.CreateNetricsResult(FK_Job, Xmldata, connection, transaction);

                    break;

                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Matching.CreateNetricsResult for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Matching.CreateNetricsResult for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetMatchRule(SqlInt32 JobIb, SqlInt32 MatchRulesetId, SqlString UserID)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Matching.GetMatchRule(JobIb, MatchRulesetId, UserID, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetMatchRule(SqlInt32 JobIb, SqlInt32 MatchRulesetId, SqlString UserID, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Matching.GetMatchRule(JobIb, MatchRulesetId, UserID, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetMatchRule(SqlInt32 JobIb, SqlInt32 MatchRulesetId, SqlString UserID, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlMatching.GetMatchRule(JobIb, MatchRulesetId, UserID, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Matching.GetMatchRule for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Matching.GetMatchRule for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetMatchCnodeAndAttrval(SqlInt32 JobIb, SqlInt32 MatchRulesetId, SqlString UserID, SqlString Cnodes, SqlString SourceAttIds, SqlInt32 CatalogSource)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Matching.GetMatchCnodeAndAttrval(JobIb, MatchRulesetId, UserID, Cnodes, SourceAttIds, CatalogSource, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetMatchCnodeAndAttrval(SqlInt32 JobIb, SqlInt32 MatchRulesetId, SqlString UserID, SqlString Cnodes, SqlString SourceAttIds, SqlInt32 CatalogSource, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Matching.GetMatchCnodeAndAttrval(JobIb, MatchRulesetId, UserID, Cnodes, SourceAttIds, CatalogSource, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetMatchCnodeAndAttrval(SqlInt32 JobIb, SqlInt32 MatchRulesetId, SqlString UserID, SqlString Cnodes, SqlString SourceAttIds, SqlInt32 CatalogSource, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlMatching.GetMatchCnodeAndAttrval(JobIb, MatchRulesetId, UserID, Cnodes, SourceAttIds, CatalogSource, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Matching.GetMatchCnodeAndAttrval for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Matching.GetMatchCnodeAndAttrval for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetMatchCnodeList(SqlInt32 JobIb, SqlString UserID, SqlString SourceAttrIds, SqlInt32 fk_catalog, SqlInt32 min_fk_cnode, SqlInt32 max_fk_cnode, SqlBoolean deltas, SqlBoolean autoclass)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Matching.GetMatchCnodeList(JobIb, UserID, SourceAttrIds, fk_catalog, min_fk_cnode, max_fk_cnode, deltas, autoclass, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetMatchCnodeList(SqlInt32 JobIb, SqlString UserID, SqlString SourceAttrIds, SqlInt32 fk_catalog, SqlInt32 min_fk_cnode, SqlInt32 max_fk_cnode, SqlBoolean deltas, SqlBoolean autoclass, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Matching.GetMatchCnodeList(JobIb, UserID, SourceAttrIds, fk_catalog, min_fk_cnode, max_fk_cnode, deltas, autoclass, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetMatchCnodeList(SqlInt32 JobIb, SqlString UserID, SqlString SourceAttrIds, SqlInt32 fk_catalog, SqlInt32 min_fk_cnode, SqlInt32 max_fk_cnode, SqlBoolean deltas, SqlBoolean autoclass, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlMatching.GetMatchCnodeList(JobIb, UserID, SourceAttrIds, fk_catalog, min_fk_cnode, max_fk_cnode, deltas, autoclass, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Matching.GetMatchCnodeList for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Matching.GetMatchCnodeList for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetMatchCnodeAndAttrvalsForInitLoad(SqlInt32 JobIb, SqlString attrList, SqlString cnodelist, SqlString UserID)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Matching.GetMatchCnodeAndAttrvalsForInitLoad(JobIb, attrList, cnodelist, UserID, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetMatchCnodeAndAttrvalsForInitLoad(SqlInt32 JobIb, SqlString attrList, SqlString cnodelist, SqlString UserID, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Matching.GetMatchCnodeAndAttrvalsForInitLoad(JobIb, attrList, cnodelist, UserID, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetMatchCnodeAndAttrvalsForInitLoad(SqlInt32 JobIb, SqlString attrList, SqlString cnodelist, SqlString UserID, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlMatching.GetMatchCnodeAndAttrvalsForInitLoad(JobIb, attrList, cnodelist, UserID, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Matching.GetMatchCnodeAndAttrvalsForInitLoad for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Matching.GetMatchCnodeAndAttrvalsForInitLoad for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetMatchingCnodeAttrvalXml(SqlInt32 JobIb, SqlString UserID, SqlString Cnodes, SqlString SourceAttrIds)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Matching.GetMatchingCnodeAttrvalXml(JobIb, UserID, Cnodes, SourceAttrIds, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetMatchingCnodeAttrvalXml(SqlInt32 JobIb, SqlString UserID, SqlString Cnodes, SqlString SourceAttrIds, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Matching.GetMatchingCnodeAttrvalXml(JobIb, UserID, Cnodes, SourceAttrIds, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetMatchingCnodeAttrvalXml(SqlInt32 JobIb, SqlString UserID, SqlString Cnodes, SqlString SourceAttrIds, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlMatching.GetMatchingCnodeAttrvalXml(JobIb, UserID, Cnodes, SourceAttrIds, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Matching.GetMatchingCnodeAttrvalXml for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Matching.GetMatchingCnodeAttrvalXml for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetMatchingCnodeCategoryAttrvalXml(SqlInt32 JobIb, SqlString UserID, SqlString Cnodes, SqlString SourceAttrIds)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Matching.GetMatchingCnodeCategoryAttrvalXml(JobIb, UserID, Cnodes, SourceAttrIds, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetMatchingCnodeCategoryAttrvalXml(SqlInt32 JobIb, SqlString UserID, SqlString Cnodes, SqlString SourceAttrIds, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Matching.GetMatchingCnodeCategoryAttrvalXml(JobIb, UserID, Cnodes, SourceAttrIds, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetMatchingCnodeCategoryAttrvalXml(SqlInt32 JobIb, SqlString UserID, SqlString Cnodes, SqlString SourceAttrIds, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlMatching.GetMatchingCnodeCategoryAttrvalXml(JobIb, UserID, Cnodes, SourceAttrIds, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Matching.GetMatchingCnodeCategoryAttrvalXml for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Matching.GetMatchingCnodeCategoryAttrvalXml for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetMatchRuleSetTypeDefaultXml(SqlInt32 fk_dc_matchtypes)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Matching.GetMatchRuleSetTypeDefaultXml(fk_dc_matchtypes, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetMatchRuleSetTypeDefaultXml(SqlInt32 fk_dc_matchtypes, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Matching.GetMatchRuleSetTypeDefaultXml(fk_dc_matchtypes, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetMatchRuleSetTypeDefaultXml(SqlInt32 fk_dc_matchtypes, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlMatching.GetMatchRuleSetTypeDefaultXml(fk_dc_matchtypes, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Matching.GetMatchRuleSetTypeDefaultXml for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Matching.GetMatchRuleSetTypeDefaultXml for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetmatchRuleSetsXml(SqlInt32 fk_dc_matchtypes)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Matching.GetmatchRuleSetsXml(fk_dc_matchtypes, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetmatchRuleSetsXml(SqlInt32 fk_dc_matchtypes, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Matching.GetmatchRuleSetsXml(fk_dc_matchtypes, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetmatchRuleSetsXml(SqlInt32 fk_dc_matchtypes, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlMatching.GetmatchRuleSetsXml(fk_dc_matchtypes, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Matching.GetmatchRuleSetsXml for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Matching.GetmatchRuleSetsXml for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetmatchRuleSetsDT(SqlInt32 fk_dc_matchtypes)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Matching.GetmatchRuleSetsDT(fk_dc_matchtypes, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetmatchRuleSetsDT(SqlInt32 fk_dc_matchtypes, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Matching.GetmatchRuleSetsDT(fk_dc_matchtypes, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetmatchRuleSetsDT(SqlInt32 fk_dc_matchtypes, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlMatching.GetmatchRuleSetsDT(fk_dc_matchtypes, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Matching.GetmatchRuleSetsDT for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Matching.GetmatchRuleSetsDT for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetMatchRuleSetCnodeAndAttrvalXml(SqlInt32 JobId, SqlInt32 MatchRulesetId, SqlString UserID, SqlString cnodelist, SqlBoolean debug)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Matching.GetMatchRuleSetCnodeAndAttrvalXml(JobId, MatchRulesetId, UserID, cnodelist, debug, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetMatchRuleSetCnodeAndAttrvalXml(SqlInt32 JobId, SqlInt32 MatchRulesetId, SqlString UserID, SqlString cnodelist, SqlBoolean debug, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Matching.GetMatchRuleSetCnodeAndAttrvalXml(JobId, MatchRulesetId, UserID, cnodelist, debug, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetMatchRuleSetCnodeAndAttrvalXml(SqlInt32 JobId, SqlInt32 MatchRulesetId, SqlString UserID, SqlString cnodelist, SqlBoolean debug, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlMatching.GetMatchRuleSetCnodeAndAttrvalXml(JobId, MatchRulesetId, UserID, cnodelist, debug, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Matching.GetMatchRuleSetCnodeAndAttrvalXml for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Matching.GetMatchRuleSetCnodeAndAttrvalXml for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetMatchingKeywordListXml(SqlString UserName, SqlInt32 fk_synonymList)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Matching.GetMatchingKeywordListXml(UserName, fk_synonymList, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetMatchingKeywordListXml(SqlString UserName, SqlInt32 fk_synonymList, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Matching.GetMatchingKeywordListXml(UserName, fk_synonymList, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetMatchingKeywordListXml(SqlString UserName, SqlInt32 fk_synonymList, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlMatching.GetMatchingKeywordListXml(UserName, fk_synonymList, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Matching.GetMatchingKeywordListXml for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Matching.GetMatchingKeywordListXml for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetMatchingKeyWordAndSynonymsXml(SqlString UserName, SqlInt32 Min_fk_keyword, SqlInt32 Max_fk_keyword, SqlInt32 fk_synonymList)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Matching.GetMatchingKeyWordAndSynonymsXml(UserName, Min_fk_keyword, Max_fk_keyword, fk_synonymList, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetMatchingKeyWordAndSynonymsXml(SqlString UserName, SqlInt32 Min_fk_keyword, SqlInt32 Max_fk_keyword, SqlInt32 fk_synonymList, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Matching.GetMatchingKeyWordAndSynonymsXml(UserName, Min_fk_keyword, Max_fk_keyword, fk_synonymList, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetMatchingKeyWordAndSynonymsXml(SqlString UserName, SqlInt32 Min_fk_keyword, SqlInt32 Max_fk_keyword, SqlInt32 fk_synonymList, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlMatching.GetMatchingKeyWordAndSynonymsXml(UserName, Min_fk_keyword, Max_fk_keyword, fk_synonymList, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Matching.GetMatchingKeyWordAndSynonymsXml for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Matching.GetMatchingKeyWordAndSynonymsXml for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetServiceTypes(SqlInt32 JobID)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Matching.GetServiceTypes(JobID, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetServiceTypes(SqlInt32 JobID, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Matching.GetServiceTypes(JobID, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetServiceTypes(SqlInt32 JobID, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlMatching.GetServiceTypes(JobID, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Matching.GetServiceTypes for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Matching.GetServiceTypes for this provider: " + providerName);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetRSPLMatchingStatus(SqlString CNodes, SqlInt32 FK_Catalog)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Matching.GetRSPLMatchingStatus(CNodes, FK_Catalog, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetRSPLMatchingStatus(SqlString CNodes, SqlInt32 FK_Catalog, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Matching.GetRSPLMatchingStatus(CNodes, FK_Catalog, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetRSPLMatchingStatus(SqlString CNodes, SqlInt32 FK_Catalog, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlMatching.GetRSPLMatchingStatus(CNodes, FK_Catalog, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Matching.GetRSPLMatchingStatus for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Matching.GetRSPLMatchingStatus for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string AttributeExtractionJob(SqlInt32 FK_JobService, SqlInt32 FK_Locale)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Matching.AttributeExtractionJob(FK_JobService, FK_Locale, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string AttributeExtractionJob(SqlInt32 FK_JobService, SqlInt32 FK_Locale, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Matching.AttributeExtractionJob(FK_JobService, FK_Locale, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string AttributeExtractionJob(SqlInt32 FK_JobService, SqlInt32 FK_Locale, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlMatching.AttributeExtractionJob(FK_JobService, FK_Locale, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Matching.GetRSPLMatchingStatus for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Matching.GetRSPLMatchingStatus for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetMatchedCnodes(SqlString NodeName, SqlXml DataXML)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Matching.GetMatchedCnodes(NodeName, DataXML, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetMatchedCnodes(SqlString NodeName, SqlXml DataXML, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Matching.GetMatchedCnodes(NodeName, DataXML, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetMatchedCnodes(SqlString NodeName, SqlXml DataXML, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlMatching.GetMatchedCnodes(NodeName, DataXML, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Matching.GetMatchedCnodes for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Matching.GetMatchedCnodes for this provider: " + providerName);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetTextBasedPartNumberMatchingResults(SqlString MatchingRules, SqlString ContextXML, SqlString vchrUserLogin)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            return Matching.GetTextBasedPartNumberMatchingResults(MatchingRules, ContextXML, vchrUserLogin, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetTextBasedPartNumberMatchingResults(SqlString MatchingRules, SqlString ContextXML, SqlString vchrUserLogin, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            return Matching.GetTextBasedPartNumberMatchingResults(MatchingRules, ContextXML, vchrUserLogin, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetTextBasedPartNumberMatchingResults(SqlString MatchingRules, SqlString ContextXML, SqlString vchrUserLogin, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    return SqlMatching.GetTextBasedPartNumberMatchingResults(MatchingRules, ContextXML, vchrUserLogin, connection, transaction);


                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Matching.GetTextBasedPartNumberMatchingResults for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Matching.GetTextBasedPartNumberMatchingResults for this provider: " + providerName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void AttributeExtractionJob(SqlInt32 FK_JobService, SqlInt32 FK_Profile_Id, SqlString loginUser, ref SqlInt32 TotalProcessed)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            Matching.AttributeExtractionJob(FK_JobService, FK_Profile_Id, loginUser, ref TotalProcessed, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void AttributeExtractionJob(SqlInt32 FK_JobService, SqlInt32 FK_Profile_Id, SqlString loginUser, ref SqlInt32 TotalProcessed, IDbConnection connection)
        {
            IDbTransaction transaction = null;
            Matching.AttributeExtractionJob(FK_JobService, FK_Profile_Id, loginUser, ref TotalProcessed, connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void AttributeExtractionJob(SqlInt32 FK_JobService, SqlInt32 FK_Profile_Id, SqlString loginUser, ref SqlInt32 TotalProcessed, IDbConnection connection, IDbTransaction transaction)
        {
            string providerName = ConfigurationManager.AppSettings.Get("databaseProvider");
            switch (providerName)
            {
                case "SqlProvider":
                    SqlMatching.AttributeExtractionJob(FK_JobService, FK_Profile_Id, loginUser, ref TotalProcessed, connection, transaction);

                    break;

                default:
                    MDMTraceHelper.EmitTraceEvent(System.Diagnostics.TraceEventType.Error,"No implementation of Matching.AttributeExtractionJob for this provider: " + providerName);
                    throw new ApplicationException("No implementation of Matching.AttributeExtractionJob for this provider: " + providerName);
            }
        }
    }
}
