using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Server;

namespace MDM.Utility
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Interfaces;

    /// <summary>
    /// Represents helper methods for entity activity log utility.
    /// </summary>
    public class EntityActivityLogUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityActivityLogCollection"></param>
        /// <param name="sqlMetadata"></param>
        /// <returns></returns>
        public static List<SqlDataRecord> GetSqlRecords(EntityActivityLogCollection entityActivityLogCollection, SqlMetaData[] sqlMetadata)
        {
            List<SqlDataRecord> entityActivityLogSqlRecords = new List<SqlDataRecord>();

            foreach (EntityActivityLog entityActivityLog in entityActivityLogCollection)
            {
                SqlDataRecord entityActivityLogRecord = new SqlDataRecord(sqlMetadata);

                entityActivityLogRecord.SetValue(0, entityActivityLog.Id); // Id

                entityActivityLogRecord.SetValue(1, entityActivityLog.EntityId); // entityId
                entityActivityLogRecord.SetValue(2, entityActivityLog.ContainerId); // container id

                entityActivityLogRecord.SetValue(3, (Byte)entityActivityLog.PerformedAction); // action id..

                if (entityActivityLog.AttributeIdList != null && entityActivityLog.AttributeIdList.Count > 0)
                {
                    entityActivityLogRecord.SetValue(4, ValueTypeHelper.JoinCollection(entityActivityLog.AttributeIdList, ",")); //AttributeIdList
                    entityActivityLogRecord.SetValue(5, ValueTypeHelper.JoinCollectionGetLocaleIdList(entityActivityLog.AttributeLocaleIdList, ",")); //AttributeLocaleIdList
                }
                else
                {
                    entityActivityLogRecord.SetValue(4, String.Empty); //AttributeIdList
                    entityActivityLogRecord.SetValue(5, String.Empty); //AttributeLocaleIdList
                }

                if (entityActivityLog.RelationshipIdList != null && entityActivityLog.RelationshipIdList.Count > 0)
                {
                    entityActivityLogRecord.SetValue(6, ValueTypeHelper.JoinCollection(entityActivityLog.RelationshipIdList, ",")); //RelationshipIdList
                }
                else
                {
                    entityActivityLogRecord.SetValue(6, String.Empty); //RelationshipIdList
                }

                entityActivityLogRecord.SetValue(7, entityActivityLog.EntityData); // Entity data...

                entityActivityLogRecord.SetValue(8, entityActivityLog.IsLoadingInProgress); //IsLoadingInProgress
                entityActivityLogRecord.SetValue(9, entityActivityLog.IsLoaded); //IsLoaded
                entityActivityLogRecord.SetValue(10, entityActivityLog.IsProcessed); //IsProcessed

                entityActivityLogRecord.SetValue(11, entityActivityLog.LoadStartTime); //LoadStartTime
                entityActivityLogRecord.SetValue(12, entityActivityLog.LoadEndTime); //LoadEndTime
                entityActivityLogRecord.SetValue(13, entityActivityLog.ProcessStartTime); //ProcessStartTime
                entityActivityLogRecord.SetValue(14, entityActivityLog.ProcessEndTime); //ProcessEndTime

                entityActivityLogRecord.SetValue(15, entityActivityLog.ImpactedCount); //ImpactedCount
                entityActivityLogRecord.SetValue(16, entityActivityLog.ServerId); //ServerId
                entityActivityLogRecord.SetValue(17, entityActivityLog.Context);//Context
                entityActivityLogRecord.SetValue(18, entityActivityLog.ParentEntityActivityLogId);//ParentActivityId
                entityActivityLogRecord.SetValue(19, entityActivityLog.IsDirectChange);//is DirectChange
                entityActivityLogRecord.SetValue(20, String.Empty); // programName
                entityActivityLogRecord.SetValue(21, entityActivityLog.AuditRefId);//Audit RefId
                entityActivityLogRecord.SetValue(22, entityActivityLog.RelationshipProcessMode);

                entityActivityLogSqlRecords.Add(entityActivityLogRecord);
            }

            return entityActivityLogSqlRecords;
        }
    }
}
