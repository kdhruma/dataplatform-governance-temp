using Microsoft.Practices.ServiceLocation;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.Utility
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents helper methods for data model activity log utility.
    /// </summary>
    public class DataModelActivityLogUtils
    {
        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataModelActivityLogCollection"></param>
        /// <param name="sqlMetadata"></param>
        /// <returns></returns>
        public static List<SqlDataRecord> GetSqlRecords(DataModelActivityLogCollection dataModelActivityLogCollection, SqlMetaData[] sqlMetadata)
        {

            List<SqlDataRecord> dataModelActivityLogSqlRecords = new List<SqlDataRecord>();

            foreach (DataModelActivityLog dataModelActivityLog in dataModelActivityLogCollection)
            {
                int i = 0;
                SqlDataRecord dataModelActivityLogRecord = new SqlDataRecord(sqlMetadata);
                dataModelActivityLogRecord.SetValue(i, -1); //id
                i++;
                dataModelActivityLogRecord.SetValue(i, 1); //type 1
                i++;
                dataModelActivityLogRecord.SetValue(i, 1); //subtype 2
                i++;
                dataModelActivityLogRecord.SetValue(i, dataModelActivityLog.OrgId); // org 3
                i++;
                dataModelActivityLogRecord.SetValue(i, dataModelActivityLog.ContainerId); // FK_Catalog 4
                i++;
                dataModelActivityLogRecord.SetValue(i, dataModelActivityLog.EntityTypeId); // FK_Nodetype 5
                i++;
                dataModelActivityLogRecord.SetValue(i, dataModelActivityLog.RelationshipTypeId); //relationshiptype 6
                i++;
                if (dataModelActivityLog.AttributeIdList != null && dataModelActivityLog.AttributeIdList.Count > 0)
                {
                    dataModelActivityLogRecord.SetValue(i, ValueTypeHelper.JoinCollection(dataModelActivityLog.AttributeIdList, ",")); //AttributeIdList 7
                }
                else
                {
                    dataModelActivityLogRecord.SetValue(i, "None");
                }
                i++;
                dataModelActivityLogRecord.SetValue(i, dataModelActivityLog.MDMObjectId); //MDMObjectId 8
                i++;
                dataModelActivityLogRecord.SetValue(i, (int)dataModelActivityLog.DataModelActivityLogAction); //Action 9
                i++;
                dataModelActivityLogRecord.SetValue(i, dataModelActivityLog.IsLoadingInProgress); //10
                i++;
                dataModelActivityLogRecord.SetValue(i, dataModelActivityLog.IsLoaded); //IsLoaded 11
                i++;
                dataModelActivityLogRecord.SetValue(i, dataModelActivityLog.IsProcessed); //IsProcessed 12
                i++;
                dataModelActivityLogRecord.SetValue(i, System.DateTime.Now); //LoadStartTime 13
                i++;
                dataModelActivityLogRecord.SetValue(i, System.DateTime.Now); //LoadEndTime 14
                i++;
                dataModelActivityLogRecord.SetValue(i, System.DateTime.Now); //ProcessStartTime 15
                i++;
                dataModelActivityLogRecord.SetValue(i, System.DateTime.Now); //ProcessEndTime 16
                i++;
                dataModelActivityLogRecord.SetValue(i, dataModelActivityLog.ImpactedCount); //ImpactedCount 17
                i++;
                dataModelActivityLogRecord.SetValue(i, dataModelActivityLog.ServerId); //FK_Server 18
                i++;
                dataModelActivityLogRecord.SetValue(i, dataModelActivityLog.ChangedData); //ChangedData 19
                i++;
                dataModelActivityLogRecord.SetValue(i, dataModelActivityLog.Weightage); // WEIGHTAGE 20
                i++;
                dataModelActivityLogRecord.SetValue(i, dataModelActivityLog.AuditRefId);//Audit RefId 21
                i++;
                dataModelActivityLogSqlRecords.Add(dataModelActivityLogRecord);
            }

            return dataModelActivityLogSqlRecords;
        }

        /// <summary>
        /// Prepares entity family change context based on given data model activity log object.
        /// </summary>
        /// <param name="activityLogCollection">Indicates data model activity log object collection</param>
        /// <param name="dataModelObjectType">Indicates object type to be processed</param>
        /// <param name="callerContext">Indicates caller context</param>
        public static void PopulateEntityFamilyChangeContext(DataModelActivityLogCollection activityLogCollection, ObjectType dataModelObjectType, CallerContext callerContext)
        {
            if (activityLogCollection != null && activityLogCollection.Count > 0)
            {
                foreach (DataModelActivityLog dataModelActivityLog in activityLogCollection)
                {
                    #region Initialization

                    LocaleCollection locales = null;
                    EntityFamilyChangeContext entityFamilyChangeContext = new EntityFamilyChangeContext();
                    EntityChangeContext entityChangeContext = new EntityChangeContext();
                    EntityChangeContextCollection entityChangeContexts = entityFamilyChangeContext.VariantsChangeContext.EntityChangeContexts;
                    LocaleChangeContextCollection localeChangeContexts = entityChangeContext.LocaleChangeContexts;

                    #endregion Initialization

                    #region LocaleChangeContexts Preparation

                    switch (dataModelObjectType)
                    {
                        case ObjectType.EntityTypeAttributeMapping:
                            {
                                locales = GetLocalesMappedToEntityTypeId(dataModelActivityLog.EntityTypeId, callerContext);
                                localeChangeContexts = GetLocaleChangeContexts(locales, dataModelActivityLog);
                            }
                            break;
                        case ObjectType.ContainerEntityTypeAttributeMapping:
                            {
                                locales = PrepareContainerSupportedLocales(dataModelActivityLog.ContainerId, callerContext);
                                localeChangeContexts = GetLocaleChangeContexts(locales, dataModelActivityLog);
                            }
                            break;
                        case ObjectType.CategoryAttributeMapping:
                        case ObjectType.AttributeModel:
                            {
                                Locale locale = new Locale() { Locale = LocaleEnum.rs_ALL };
                                localeChangeContexts = GetLocaleChangeContexts(new LocaleCollection() { locale }, dataModelActivityLog);
                            }
                            break;
                        case ObjectType.RelationshipTypeEntityTypeMapping:
                            {
                                Locale locale = new Locale() { Locale = GlobalizationHelper.GetSystemDataLocale() };
                                localeChangeContexts = GetLocaleChangeContexts(new LocaleCollection() { locale }, dataModelActivityLog);
                            }
                            break;
                        case ObjectType.ContainerRelationshipTypeEntityTypeMapping:
                            {
                                locales = PrepareContainerSupportedLocales(dataModelActivityLog.ContainerId, callerContext);
                                localeChangeContexts = GetLocaleChangeContexts(locales, dataModelActivityLog);
                            }
                            break;
                    }

                    #endregion LocaleChangeContexts Preparation

                    #region EntityFamilyChangeContext preparation

                    entityChangeContext.EntityTypeId = dataModelActivityLog.EntityTypeId;
                    entityChangeContext.SetLocaleChangeContexts(localeChangeContexts);
                    entityChangeContexts.Add(entityChangeContext);

                    entityFamilyChangeContext.OrganizationId = dataModelActivityLog.OrgId;
                    entityFamilyChangeContext.ContainerId = dataModelActivityLog.ContainerId;
                    entityFamilyChangeContext.EntityActivityList = EntityActivityList.MetadataChange;

                    #endregion EntityFamilyChangeContext preparation

                    dataModelActivityLog.ChangedData = entityFamilyChangeContext.ToXml();
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private static LocaleCollection PrepareContainerSupportedLocales(Int32 containerId, CallerContext callerContext)
        {
            LocaleCollection locales = null;
            var containerManager = ServiceLocator.Current.GetInstance(typeof(IContainerManager)) as IContainerManager;
            Container container = containerManager.Get(containerId, callerContext, false);
            if (container != null)
            {
                locales = container.SupportedLocales;
            }

            return locales;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeId"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private static LocaleCollection GetLocalesMappedToEntityTypeId(Int32 entityTypeId, CallerContext callerContext)
        {
            LocaleCollection mappedLocales = new LocaleCollection();
            LocaleCollection containerLocales = null;

            var contEntityTypeMappingManager = ServiceLocator.Current.GetInstance(typeof(IContainerEntityTypeMappingManager)) as IContainerEntityTypeMappingManager;
            ContainerEntityTypeMappingCollection mappings = contEntityTypeMappingManager.GetMappingsByEntityTypeId(entityTypeId, callerContext);

            foreach (var item in mappings)
            {
                containerLocales = PrepareContainerSupportedLocales(item.ContainerId, callerContext);

                if (containerLocales.Count > 0)
                {
                    foreach (Locale locale in containerLocales)
                    {
                        if (!mappedLocales.Contains(locale))
                        {
                            mappedLocales.Add(locale);
                        }
                    }
                }
            }

            return mappedLocales;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locales"></param>
        /// <param name="dataModelActivityLog"></param>
        /// <returns></returns>
        private static LocaleChangeContextCollection GetLocaleChangeContexts(LocaleCollection locales, DataModelActivityLog dataModelActivityLog)
        {
            LocaleChangeContextCollection localeChangeContexts = new LocaleChangeContextCollection();
            foreach (Locale locale in locales)
            {
                LocaleChangeContext localeChangeContext = new LocaleChangeContext();
                localeChangeContext.DataLocale = locale.Locale;

                if (dataModelActivityLog.AttributeIdList != null && dataModelActivityLog.AttributeIdList.Count > 0)
                {
                    AttributeChangeContextCollection attributeChangeContexts = GetAttributeChangeContexts(dataModelActivityLog.Action, dataModelActivityLog.AttributeIdList);
                    localeChangeContext.SetAttributeChangeContexts(attributeChangeContexts);
                }
                else if (dataModelActivityLog.RelationshipTypeId > 0)
                {
                    RelationshipChangeContext relationshipChangeContext = new RelationshipChangeContext();
                    relationshipChangeContext.Action = dataModelActivityLog.Action;
                    relationshipChangeContext.RelationshipTypeId = dataModelActivityLog.RelationshipTypeId;
                    localeChangeContext.RelationshipChangeContexts.Add(relationshipChangeContext);
                }

                localeChangeContexts.Add(localeChangeContext);
            }

            return localeChangeContexts;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="attributeIdList"></param>
        /// <returns></returns>
        private static AttributeChangeContextCollection GetAttributeChangeContexts(ObjectAction action, Collection<Int32> attributeIdList)
        {
            AttributeChangeContextCollection attributeChangeContexts = new AttributeChangeContextCollection();
            AttributeInfoCollection attributeInfoCollection = new AttributeInfoCollection();

            foreach (Int32 attributeId in attributeIdList)
            {
                attributeInfoCollection.Add(new AttributeInfo() { Id = attributeId });
            }

            if (attributeIdList != null && attributeIdList.Count > 0)
            {
                AttributeChangeContext attributeChangeContext = new AttributeChangeContext();
                attributeChangeContext.Action = action;
                attributeChangeContext.AttributeInfoCollection = attributeInfoCollection;
                attributeChangeContexts.Add(attributeChangeContext);
            }

            return attributeChangeContexts;
        }

        #endregion Private Methods
    }
}
