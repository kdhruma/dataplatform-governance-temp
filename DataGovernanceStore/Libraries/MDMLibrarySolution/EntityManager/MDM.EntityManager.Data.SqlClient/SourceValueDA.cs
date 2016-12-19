using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using Microsoft.SqlServer.Server;

namespace MDM.EntityManager.Data
{
    using MDM.Interfaces;
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;

    /// <summary>
    /// SourceValue data access layer
    /// </summary>
    public class SourceValueDA : SqlClientDataAccessBase
    {
        #region Constants

        private const String MasterRecordResultName = "FK_MasterRecordId";
        private const String MasterRecordTypeIdResultName = "FK_MasterRecordType";
        private const String AttributeIdResultName = "FK_Attribute";
        private const String AttrValIdResultName = "FK_AttrVal";
        private const String SourceIdResultName = "FK_Source";
        private const String ModDateTimeUTCResultName = "ModDateTimeUTC";
        private const String SourceEntityIdResultName = "FK_SourceEntity";

        private const String SQLParametersGeneratorName = "EntityManager_SqlParameters";

        private const String SourceValuesGetProcessName = "Get";
        private const String SourceValuesProcessProcessName = "Process";

        private const String SourceValuesGetSPName = "usp_EntityManager_SourceVal_Get";
        private const String SourceValuesProcessSPName = "usp_EntityManager_SourceVal_Process";

        private const String SourceValuesGetSPParameters = "EntityManager_SourceVal_Get_ParametersArray";
        private const String SourceValuesProcessSPParameters = "EntityManager_SourceVal_Process_ParametersArray";

        private const String TracingPrefix = "MDM.EntityManager.Data.SqlClient.SourceValueDA.";

        #endregion

        #region Private classes

        private enum MasterRecordType : byte
        {
            Unknown = 0,
            Entity = 1,
            Relationship = 2
        }

        private class DataItemLocator
        {
            public Int64 MasterRecordId { get; set; }
            public MasterRecordType MasterRecordType { get; set; }
            public Int32? AttributeId { get; set; }
            public Int64? AttrValId { get; set; }

            public DataItemLocator(Int64 masterRecordId, MasterRecordType masterRecordType, Int32? attributeId, Int64? attrValId)
            {
                this.MasterRecordId = masterRecordId;
                this.MasterRecordType = masterRecordType;
                this.AttributeId = attributeId;
                this.AttrValId = attrValId;
            }

            public DataItemLocator(SourceValue sourceValue)
                : this(sourceValue.MasterRecordId, sourceValue.MasterRecordType, sourceValue.AttributeId, sourceValue.AttrValId)
            {
            }

            public override Boolean Equals(object obj)
            {
                if (obj == null)
                {
                    return false;
                }
                if (obj is DataItemLocator)
                {
                    DataItemLocator objectToBeCompared = obj as DataItemLocator;
                    return
                        this.MasterRecordId == objectToBeCompared.MasterRecordId &&
                        this.MasterRecordType == objectToBeCompared.MasterRecordType &&
                        this.AttributeId == objectToBeCompared.AttributeId &&
                        this.AttrValId == objectToBeCompared.AttrValId;
                }
                return false;
            }

            public override Int32 GetHashCode()
            {
                return
                    this.MasterRecordId.GetHashCode()
                    ^ this.MasterRecordType.GetHashCode()
                    ^ this.AttributeId.GetHashCode()
                    ^ this.AttrValId.GetHashCode();
            }
        }

        private class SourceValue
        {
            public Int64 MasterRecordId { get; set; }
            public MasterRecordType MasterRecordType { get; set; }
            public Int32? AttributeId { get; set; }
            public Int64? AttrValId { get; set; }
            public Int32? SourceId { get; set; }
            public Int64? AuditRefId { get; set; }
            public Int64? SourceEntityId { get; set; }
            public DateTime? ModDateTime { get; set; }
            public ObjectAction Action { get; set; }
        }

        private interface ISourceValuesProcessor
        {
            void ProcessEntity(Entity entity);
            void ProcessEntityAttributeValue(Entity entity, Attribute attribute, Int64 sourceEntityId, Value value);
            void ProcessRelationship(Entity entity, Relationship relationship);
            void ProcessRelationshipAttributeValue(Relationship relationship, Attribute attribute, Int64 sourceEntityId, Value value);
        }

        /// <summary>
        /// This class allows to prepare sources request to source values storage
        /// </summary>
        private class SourceValuesRequestPreparer : ISourceValuesProcessor
        {
            public Dictionary<DataItemLocator, SourceValue> Values = null;

            public SourceValuesRequestPreparer()
            {
                Values = new Dictionary<DataItemLocator, SourceValue>();
            }

            private void AddSourceValueIfNotExist(Int64 masterRecordId, MasterRecordType masterRecordType, Int32? attributeId, Int64? attrValId)
            {
                if (masterRecordId == -1)
                {
                    return;
                }
                DataItemLocator locator = new DataItemLocator(masterRecordId, masterRecordType, attributeId, attrValId);
                if (!Values.ContainsKey(locator))
                {
                    Values.Add(
                        locator,
                        new SourceValue()
                        {
                            MasterRecordId = masterRecordId,
                            MasterRecordType = masterRecordType,
                            AttributeId = attributeId,
                            AttrValId = attrValId
                        });
                }
            }

            public void ProcessEntity(Entity entity)
            {
                if (entity != null)
                {
                    AddSourceValueIfNotExist(entity.Id, MasterRecordType.Entity, null, null);
                }
            }

            public void ProcessEntityAttributeValue(Entity entity, Attribute attribute, Int64 sourceEntityId, Value value)
            {
                if (entity != null && attribute != null && value != null && value.Id != -1)
                {
                    AddSourceValueIfNotExist(sourceEntityId != 0 ? sourceEntityId : entity.Id, MasterRecordType.Entity, attribute.Id, value.Id);
                }
            }

            public void ProcessRelationship(Entity entity, Relationship relationship)
            {
                if (relationship != null)
                {
                    AddSourceValueIfNotExist(relationship.Id, MasterRecordType.Relationship, null, null);
                }
            }

            public void ProcessRelationshipAttributeValue(Relationship relationship, Attribute attribute, Int64 sourceEntityId, Value value)
            {
                if (relationship != null && attribute != null && value != null && value.Id != -1)
                {
                    AddSourceValueIfNotExist(sourceEntityId != 0 ? sourceEntityId : relationship.Id, MasterRecordType.Relationship, attribute.Id, value.Id);
                }
            }
        }

        /// <summary>
        /// This class allows to write sources data (recived from source values storage) to entity structures 
        /// </summary>
        private class SourceValuesWriter : ISourceValuesProcessor
        {
            private Dictionary<DataItemLocator, SourceValue> _values = null;

            public SourceValuesWriter(IEnumerable<SourceValue> sourceValues)
            {
                _values = new Dictionary<DataItemLocator, SourceValue>();
                if (sourceValues != null)
                {
                    foreach (SourceValue sourceValue in sourceValues)
                    {
                        DataItemLocator locator = new DataItemLocator(sourceValue);
                        if (!_values.ContainsKey(locator))
                        {
                            _values.Add(locator, sourceValue);
                        }
                    }
                }
            }

            private SourceInfo GetSourceValueIfExist(Int64 masterRecordId, MasterRecordType masterRecordType, Int32? attributeId, Int64? attrValId)
            {
                if (masterRecordId == -1)
                {
                    return null;
                }
                SourceValue sourceValue = null;
                if (_values.TryGetValue(new DataItemLocator(masterRecordId, masterRecordType, attributeId, attrValId), out sourceValue))
                {
                    return
                        new SourceInfo
                        {
                            SourceId = sourceValue.SourceId,
                            SourceEntityId = sourceValue.SourceEntityId
                        };
                }
                return null;
            }

            public void ProcessEntity(Entity entity)
            {
                if (entity != null)
                {
                    SourceInfo sourceInfo = GetSourceValueIfExist(entity.Id, MasterRecordType.Entity, null, null);
                    if (sourceInfo != null)
                    {
                        entity.SourceInfo = sourceInfo;
                    }
                }
            }

            public void ProcessEntityAttributeValue(Entity entity, Attribute attribute, Int64 sourceEntityId, Value value)
            {
                if (entity != null && attribute != null && value != null && value.Id != -1)
                {
                    SourceInfo sourceInfo = GetSourceValueIfExist(sourceEntityId != 0 ? sourceEntityId : entity.Id, MasterRecordType.Entity, attribute.Id, value.Id);
                    if (sourceInfo != null)
                    {
                        value.SourceInfo = sourceInfo;
                    }
                }
            }

            public void ProcessRelationship(Entity entity, Relationship relationship)
            {
                if (relationship != null)
                {
                    SourceInfo sourceInfo = GetSourceValueIfExist(relationship.Id, MasterRecordType.Relationship, null, null);
                    if (sourceInfo != null)
                    {
                        relationship.SourceInfo = sourceInfo;
                    }
                }
            }

            public void ProcessRelationshipAttributeValue(Relationship relationship, Attribute attribute, Int64 sourceEntityId, Value value)
            {
                if (relationship != null && attribute != null && value != null && value.Id != -1)
                {
                    SourceInfo sourceInfo = GetSourceValueIfExist(sourceEntityId != 0 ? sourceEntityId : relationship.Id, MasterRecordType.Relationship, attribute.Id, value.Id);
                    if (sourceInfo != null)
                    {
                        value.SourceInfo = sourceInfo;
                    }
                }
            }
        }

        /// <summary>
        /// This class allows to collect sources data during entity saving process. Collected data will be stored to source values storage
        /// </summary>
        private class SourceValuesDataSavingContext
        {
            public Dictionary<DataItemLocator, SourceValue> Values = null;

            public SourceValuesDataSavingContext()
            {
                Values = new Dictionary<DataItemLocator, SourceValue>();
            }

            public void AddSourceValueIfNotExist(Int64 masterRecordId, MasterRecordType masterRecordType, Int32? attributeId, Int64? attrValId, SourceInfo sourceInfo, Int64? auditRefId, ObjectAction action)
            {
                if (masterRecordId == -1 || masterRecordType == MasterRecordType.Unknown || (sourceInfo == null && action != ObjectAction.Delete) ||
                    !(action == ObjectAction.Create || action == ObjectAction.Update || action == ObjectAction.Delete)
                   )
                {
                    return;
                }
                // Initialize sourceInfo if it is null for delete operation
                if (sourceInfo == null)
                {
                    sourceInfo = new SourceInfo();
                }

                DataItemLocator locator = new DataItemLocator(masterRecordId, masterRecordType, attributeId, attrValId);
                SourceValue sourceValue = null;
                if (Values.TryGetValue(locator, out sourceValue))
                {
                    if (sourceValue.Action != ObjectAction.Delete)
                    {
                        if (sourceValue.AuditRefId < auditRefId || action == ObjectAction.Delete)
                        {
                            sourceValue.SourceId = sourceInfo.SourceId;
                            sourceValue.AuditRefId = auditRefId;
                            sourceValue.SourceEntityId = sourceInfo.SourceEntityId;
                            sourceValue.Action = action;
                        }
                    }
                }
                else
                {
                    Values.Add(
                        locator,
                        new SourceValue()
                        {
                            MasterRecordId = masterRecordId,
                            MasterRecordType = masterRecordType,
                            AttributeId = attributeId,
                            AttrValId = attrValId,
                            SourceId = sourceInfo.SourceId,
                            AuditRefId = auditRefId,
                            ModDateTime = null,
                            SourceEntityId = sourceInfo.SourceEntityId,
                            Action = (action == ObjectAction.Create || action == ObjectAction.Delete) ? action : ObjectAction.Create
                        });
                }
            }
        }

        #endregion
       
        #region Constructors

        #endregion

        #region Public Methods

        /// <summary>
        /// Source values loding from the database
        /// </summary>
        /// <param name="entities">Entities collection</param>
        /// <param name="callerContext">Context details of caller</param>
        /// <param name="command">Connection string and operation type</param>
        /// <returns>Operation result</returns>
        public OperationResult Get(EntityCollection entities, CallerContext callerContext, DBCommandProperties command)
        {
            OperationResult operationResult = new OperationResult();
            operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;

            StartTraceActivity(SourceValuesGetProcessName, MDMTraceSource.EntityGet);
            try
            {
                SqlDataReader reader = null;
                try
                {
                    #region Request preparing

                    SourceValuesRequestPreparer preparingVisitor = new SourceValuesRequestPreparer();
                    ProcessEntitiesByLoadingVisitor(preparingVisitor, entities);

                    if (!preparingVisitor.Values.Any())
                    {
                        return operationResult;
                    }

                    #endregion

                    #region Requesting source data from the database

                    SqlParametersGenerator generator = new SqlParametersGenerator(SQLParametersGeneratorName);
                    SqlParameter[] parameters = generator.GetParameters(SourceValuesGetSPParameters);
                    List<SqlDataRecord> preparedItems = new List<SqlDataRecord>();
                    SqlMetaData[] itemMeta = generator.GetTableValueMetadata(SourceValuesGetSPParameters, parameters[0].ParameterName);

                    foreach (KeyValuePair<DataItemLocator, SourceValue> pair in preparingVisitor.Values)
                    {
                        AddSourceItemAsIs(preparedItems, itemMeta, pair.Value.MasterRecordId, pair.Value.MasterRecordType, pair.Value.AttributeId, pair.Value.AttrValId, null, null, null);
                    }

                    if (preparedItems.Any())
                    {
                        //parameters[0].Value  ...this filter not used here for now
                        parameters[1].Value = preparedItems;

                        reader = ExecuteProcedureReader(command.ConnectionString, parameters, SourceValuesGetSPName);

                        if (reader != null)
                        {
                            #region Received source data wrating back to the entities structures

                            List<SourceValue> receivedSourceValues = ExtractSourceValuesFromReader(reader);
                            if (receivedSourceValues != null && receivedSourceValues.Any())
                            {
                                SourceValuesWriter writingVisitor = new SourceValuesWriter(receivedSourceValues);
                                ProcessEntitiesByLoadingVisitor(writingVisitor, entities);
                            }

                            #endregion
                        }
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    operationResult.Errors.Add(new Error(String.Empty, ex.Message));
                    operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;

                    TraceError("Process - Failed. " + ex.Message, MDMTraceSource.EntityGet);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }
            finally
            {
                StopTraceActivity(SourceValuesGetProcessName, MDMTraceSource.EntityGet);
            }

            return operationResult;
        }

        /// <summary>
        /// Source values processing and saving to the database
        /// </summary>
        /// <param name="entities">Entities collection</param>
        /// <param name="callerContext">Context details of caller</param>
        /// <param name="command">Connection string and operation type</param>
        /// <returns>Operation result</returns>
        public OperationResult Process(EntityCollection entities, CallerContext callerContext, DBCommandProperties command)
        {
            OperationResult operationResult = new OperationResult();
            operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;

            StartTraceActivity(SourceValuesProcessProcessName, MDMTraceSource.EntityProcess);
            try
            {
                SourceValuesDataSavingContext savingContext = new SourceValuesDataSavingContext();

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL}))
                {
                    try
                    {
                        foreach (Entity entity in entities)
                        {
                            #region Entity source information processing
                            
                            savingContext.AddSourceValueIfNotExist(entity.Id, MasterRecordType.Entity, null, null, entity.SourceInfo, entity.AuditRefId, entity.Action);

                            #endregion

                            #region Entity attrbutes processing

                            if (entity.Action != ObjectAction.Delete)
                                ProcessEntityAttributes(savingContext, entity, entity.Attributes, callerContext);

                            #endregion

                            #region Relationships processing

                            ProcessRelationships(savingContext, entity.Relationships, callerContext);

                            #endregion
                        }

                        if (savingContext.Values.Any())
                        {
                            SqlParametersGenerator generator = new SqlParametersGenerator(SQLParametersGeneratorName);
                            SqlParameter[] parameters = generator.GetParameters(SourceValuesProcessSPParameters);
                            List<SqlDataRecord> preparedItems = new List<SqlDataRecord>();
                            SqlMetaData[] itemMeta = generator.GetTableValueMetadata(SourceValuesProcessSPParameters, parameters[0].ParameterName);

                            foreach (KeyValuePair<DataItemLocator, SourceValue> pair in savingContext.Values)
                            {
                                AddSourceItemAsIs(
                                    preparedItems, itemMeta,
                                    pair.Value.MasterRecordId, pair.Value.MasterRecordType,
                                    pair.Value.AttributeId, pair.Value.AttrValId,
                                    pair.Value.SourceId,
                                    pair.Value.SourceEntityId,
                                    pair.Value.Action);
                            }

                            if (preparedItems.Any())
                            {
                                parameters[0].Value = preparedItems;

                                ExecuteProcedureNonQuery(command.ConnectionString, parameters, SourceValuesProcessSPName);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        operationResult.Errors.Add(new Error(String.Empty, ex.Message));
                        operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;

                        TraceError("Process - Failed. " + ex.Message, MDMTraceSource.EntityProcess);
                    }

                    if (operationResult.OperationResultStatus == OperationResultStatusEnum.Successful)
                        transactionScope.Complete();
                }
            }
            finally
            {
                StopTraceActivity(SourceValuesProcessProcessName, MDMTraceSource.EntityProcess);
            }

            return operationResult;
        }

        #endregion

        #region Private Methods

        #region Loading routines

        private void ProcessEntitiesByLoadingVisitor(ISourceValuesProcessor visitor, EntityCollection entities)
        {
            foreach (Entity entity in entities)
            {
                #region Entity source information processing

                visitor.ProcessEntity(entity);

                #endregion

                #region Entity attrbutes processing

                ProcessEntityAttributesByLoadingVisitor(visitor, entity, entity.Attributes);

                #endregion

                #region Relationships processing

                ProcessRelationshipsByLoadingVisitor(visitor, entity, entity.Relationships);

                #endregion
            }
        }

        private void ProcessEntityAttributesByLoadingVisitor(ISourceValuesProcessor visitor, Entity entity, IAttributeCollection attributeCollection)
        {
            if (attributeCollection != null && attributeCollection.Any())
            {
                foreach (Attribute attribute in attributeCollection)
                {
                    IValueCollection values = null;

                    values = attribute.GetInheritedValuesInvariant();
                    if (values != null)
                    {
                        foreach (Value value in values)
                        {
                            visitor.ProcessEntityAttributeValue(entity, attribute, attribute.SourceEntityIdInherited, value);
                        }
                    }

                    values = attribute.GetOverriddenValuesInvariant();
                    if (values != null)
                    {
                        foreach (Value value in values)
                        {
                            visitor.ProcessEntityAttributeValue(entity, attribute, attribute.SourceEntityIdOverridden, value);
                        }
                    }

                    if (attribute.IsComplex)
                    {
                        if (attribute.GetChildAttributes() != null && attribute.GetChildAttributes().Any())
                        {
                            foreach (Attribute childAttribute in attribute.GetChildAttributes())
                            {
                                if (childAttribute.GetChildAttributes() != null && childAttribute.GetChildAttributes().Any())
                                {
                                    ProcessEntityAttributesByLoadingVisitor(visitor, entity, childAttribute.GetChildAttributes());
                                }
                            }
                        }
                    }
                }
            }
        }
        
        private void ProcessRelationshipsByLoadingVisitor(ISourceValuesProcessor visitor, Entity entity, RelationshipCollection relationships)
        {
            if (relationships != null && relationships.Any())
            {
                foreach (Relationship relationship in relationships)
                {
                    if (relationship != null)
                    {
                        if (relationship.Action != ObjectAction.Read)
                        {
                            visitor.ProcessRelationship(entity, relationship);

                            ProcessRelationshipsAttributesByLoadingVisitor(visitor, relationship, relationship.RelationshipAttributes);
                        }
                        ProcessRelationshipsByLoadingVisitor(visitor, entity, relationship.RelationshipCollection);
                    }
                }
            }
        }

        private void ProcessRelationshipsAttributesByLoadingVisitor(ISourceValuesProcessor visitor, Relationship relationship, AttributeCollection relationshipAttributes)
        {
            if (relationshipAttributes != null && relationshipAttributes.Any())
            {
                foreach (Attribute attribute in relationshipAttributes)
                {
                    IValueCollection values = null;

                    values = attribute.GetInheritedValuesInvariant();
                    if (values != null)
                    {
                        foreach (Value value in values)
                        {
                            visitor.ProcessRelationshipAttributeValue(relationship, attribute, attribute.SourceEntityIdInherited, value);
                        }
                    }

                    values = attribute.GetOverriddenValuesInvariant();
                    if (values != null)
                    {
                        foreach (Value value in values)
                        {
                            visitor.ProcessRelationshipAttributeValue(relationship, attribute, attribute.SourceEntityIdOverridden, value);
                        }
                    }

                    if (attribute.IsComplex)
                    {
                        if (attribute.GetChildAttributes() != null && attribute.GetChildAttributes().Any())
                        {
                            foreach (Attribute childAttribute in attribute.GetChildAttributes())
                            {
                                if (childAttribute.GetChildAttributes() != null && childAttribute.GetChildAttributes().Any())
                                {
                                    ProcessRelationshipsAttributesByLoadingVisitor(visitor, relationship, (AttributeCollection)childAttribute.GetChildAttributes());
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Saving routines

        private void ProcessRelationships(SourceValuesDataSavingContext savingContext, RelationshipCollection relationships, CallerContext callerContext)
        {
            if (relationships != null && relationships.Any())
            {
                foreach (Relationship relationship in relationships)
                {
                    if (relationship != null)
                    {
                        if (relationship.Action != ObjectAction.Read)
                        {
                            savingContext.AddSourceValueIfNotExist(relationship.Id, MasterRecordType.Relationship, null, null, relationship.SourceInfo, relationship.AuditRefId, relationship.Action);

                            ProcessRelationshipsAttributes(savingContext, relationship, relationship.RelationshipAttributes, callerContext);
                        }
                        ProcessRelationships(savingContext, relationship.RelationshipCollection, callerContext);
                    }
                }
            }
        }

        private void ProcessRelationshipsAttributes(SourceValuesDataSavingContext savingContext, Relationship relationship, AttributeCollection relationshipAttributes, CallerContext callerContext)
        {
            if (relationshipAttributes != null && relationshipAttributes.Any())
            {
                foreach (Attribute relAttr in relationshipAttributes)
                {
                    if (relAttr.Action != ObjectAction.Create && relAttr.Action != ObjectAction.Update && relAttr.Action != ObjectAction.Delete)
                        continue;

                    //If attribute action is delete then pass the attribute as Action = Delete
                    //For Denorm module delete, we need to pass attr record for each value in attribute with their PK DNI Attrval thus it goes to else flow only
                    if ((!relAttr.IsCollection || relAttr.IsComplex == true) && relAttr.Action == ObjectAction.Delete && callerContext.Module != MDMCenterModules.Denorm)
                    {
                        IValueCollection iValues = relAttr.GetOverriddenValuesInvariant();
                        Int64 valueId = (iValues == null || iValues.Count < 1) ? -1 : iValues.SingleOrDefault().Id;
                        if (valueId != -1)
                        {
                            savingContext.AddSourceValueIfNotExist(relationship.Id, MasterRecordType.Relationship, relAttr.Id, valueId, null, null, ObjectAction.Delete);
                        }
                    }
                    else
                    {
                        IValueCollection values = MDMObjectFactory.GetIValueCollection();

                        if (callerContext.Module == MDMCenterModules.Denorm)
                        {
                            if (relAttr.SourceFlag != AttributeValueSource.Inherited) // Inherited atributes not needed
                            {
                                values = relAttr.GetCurrentValuesInvariant();
                            }
                        }
                        else
                        {
                            values = relAttr.GetOverriddenValuesInvariant();
                        }

                        if (values != null)
                        {
                            foreach (Value value in values)
                            {
                                if (value.Action == ObjectAction.Ignore)
                                    continue;

                                ObjectAction valueAction = relAttr.Action;

                                if (relAttr.IsCollection && !relAttr.IsComplex)
                                {
                                    valueAction = value.Action;
                                }

                                savingContext.AddSourceValueIfNotExist(relationship.Id, MasterRecordType.Relationship, relAttr.Id, value.Id, value.SourceInfo, value.AuditRefId, valueAction);
                            }
                        }

                        if (relAttr.IsComplex && callerContext.Module != MDMCenterModules.Denorm)
                        {
                            if (relAttr.GetChildAttributes() != null && relAttr.GetChildAttributes().Any())
                            {
                                foreach (Attribute childAttribute in relAttr.GetChildAttributes())
                                {
                                    if (childAttribute.HasInvalidValues)
                                        continue;

                                    if (childAttribute.GetChildAttributes() != null && childAttribute.GetChildAttributes().Any())
                                    {
                                        ProcessRelationshipsAttributes(savingContext, relationship, (AttributeCollection)childAttribute.GetChildAttributes(), callerContext);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ProcessEntityAttributes(SourceValuesDataSavingContext savingContext, Entity entity, IAttributeCollection attributeCollection, CallerContext callerContext)
        {
            if (attributeCollection != null && attributeCollection.Any())
            {
                foreach (Attribute attribute in attributeCollection)
                {
                    if (attribute.Action != ObjectAction.Create && attribute.Action != ObjectAction.Update && attribute.Action != ObjectAction.Delete)
                        continue;

                    //If attribute action is delete then pass the attribute as Action = Delete
                    //For Denorm module delete, we need to pass attr record for each value in attribute with their PK DNI Attrval thus it goes to else flow only
                    if ((!attribute.IsCollection || attribute.IsComplex == true) && attribute.Action == ObjectAction.Delete && callerContext.Module != MDMCenterModules.Denorm)
                    {
                        IValueCollection iValues = attribute.GetOverriddenValuesInvariant();
                        Int64 valueId = (iValues == null || iValues.Count < 1) ? -1 : iValues.SingleOrDefault().Id;
                        if (valueId != -1)
                        {
                            savingContext.AddSourceValueIfNotExist(entity.Id, MasterRecordType.Entity, attribute.Id, valueId, null, null, ObjectAction.Delete);
                        }
                    }
                    else
                    {
                        IValueCollection values = MDMObjectFactory.GetIValueCollection();

                        if (callerContext.Module == MDMCenterModules.Denorm)
                        {
                            if (attribute.SourceFlag != AttributeValueSource.Inherited) // Inherited atributes not needed
                            {
                                values = attribute.GetCurrentValuesInvariant();
                            }
                        }
                        else
                        {
                            values = attribute.GetOverriddenValuesInvariant();
                        }

                        if (values != null)
                        {
                            foreach (Value value in values)
                            {
                                if (value.Action == ObjectAction.Ignore)
                                    continue;

                                ObjectAction valueAction = attribute.Action;

                                if (attribute.IsCollection && !attribute.IsComplex)
                                {
                                    valueAction = value.Action;
                                }

                                savingContext.AddSourceValueIfNotExist(entity.Id, MasterRecordType.Entity, attribute.Id, value.Id, value.SourceInfo, value.AuditRefId, valueAction);
                            }
                        }

                        if (attribute.IsComplex && callerContext.Module != MDMCenterModules.Denorm)
                        {
                            if (attribute.GetChildAttributes() != null && attribute.GetChildAttributes().Any())
                            {
                                foreach (Attribute childAttribute in attribute.GetChildAttributes())
                                {
                                    if (childAttribute.HasInvalidValues)
                                        continue;

                                    if (childAttribute.GetChildAttributes() != null && childAttribute.GetChildAttributes().Any())
                                    {
                                        ProcessEntityAttributes(savingContext, entity, childAttribute.GetChildAttributes(), callerContext);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void AddSourceItemAsIs(List<SqlDataRecord> preparedItems, SqlMetaData[] itemMeta, Int64 masterRecordId, MasterRecordType masterRecordType, Int32? attributeId, Int64? attrValId, Int32? sourceId, Int64? sourceEntityId, ObjectAction? action)
        {
            SqlDataRecord preparedItem = new SqlDataRecord(itemMeta);

            preparedItem.SetValue(0, masterRecordId);
            preparedItem.SetValue(1, (Byte) masterRecordType);
            preparedItem.SetValue(2, attributeId.HasValue ? attributeId.Value : (object) System.DBNull.Value);
            preparedItem.SetValue(3, attrValId.HasValue ? attrValId.Value : (object) System.DBNull.Value);
            preparedItem.SetValue(4, sourceId.HasValue ? sourceId.Value : (object) System.DBNull.Value);
            preparedItem.SetValue(5, sourceEntityId.HasValue ? sourceEntityId.Value : (object)System.DBNull.Value);
            preparedItem.SetValue(6, (object)System.DBNull.Value /* Null specified, so current server UTC time will be used as record ModDateTimeUTC */);
            preparedItem.SetValue(7, action.HasValue ? (object)action.ToString() : (object)System.DBNull.Value);

            preparedItems.Add(preparedItem);
        }

        private List<SourceValue> ExtractSourceValuesFromReader(SqlDataReader reader)
        {
            List<SourceValue> result = new List<SourceValue>();
            while (reader.Read())
            {
                Int64 masterRecordId = 0;
                MasterRecordType masterRecordType = MasterRecordType.Unknown;
                Int32? attributeId = null;
                Int64? attrValId = null;
                Int32? sourceId = null;
                Int64? sourceEntityId = null;
                DateTime? modDateTime = null;

                if (reader[MasterRecordResultName] != null)
                    masterRecordId = ValueTypeHelper.Int64TryParse(reader[MasterRecordResultName].ToString(), 0);

                if (reader[MasterRecordTypeIdResultName] != null)
                {
                    MasterRecordType tmp = MasterRecordType.Unknown;
                    if (Enum.TryParse(reader[MasterRecordTypeIdResultName].ToString(), out tmp))
                    {
                        masterRecordType = tmp;
                    }
                }

                if (reader[AttributeIdResultName] != null)
                    attributeId = ValueTypeHelper.ConvertToNullableInt32(reader[AttributeIdResultName].ToString());

                if (reader[AttrValIdResultName] != null)
                    attrValId = ValueTypeHelper.ConvertToNullableInt64(reader[AttrValIdResultName].ToString());

                if (reader[SourceIdResultName] != null)
                    sourceId = ValueTypeHelper.ConvertToNullableInt32(reader[SourceIdResultName].ToString());

                modDateTime = ExtractNullableUtcDateTimeFromReader(reader, ModDateTimeUTCResultName);

                if (reader[SourceEntityIdResultName] != null)
                    sourceEntityId = ValueTypeHelper.ConvertToNullableInt64(reader[SourceEntityIdResultName].ToString());

                if (masterRecordId != -1 && masterRecordType != MasterRecordType.Unknown && sourceId.HasValue)
                {
                    SourceValue item = new SourceValue()
                    {
                        MasterRecordId = masterRecordId,
                        MasterRecordType = masterRecordType,
                        AttributeId = attributeId,
                        AttrValId = attrValId,
                        SourceId = sourceId,
                        ModDateTime = modDateTime,
                        SourceEntityId = sourceEntityId
                    };
                    result.Add(item);
                }
            }
            return result;
        }

        #endregion

        private static DateTime? ExtractNullableUtcDateTimeFromReader(SqlDataReader reader, String paramName)
        {
            DateTime? result = null;
            if (reader[paramName] != null && reader[paramName] != DBNull.Value)
            {
                result = (DateTime)reader[paramName];
                if (result.Value.Kind == DateTimeKind.Unspecified)
                {
                    result = new DateTime(result.Value.Ticks, DateTimeKind.Utc).ToLocalTime();
                }
            }
            return result;
        }

        private static Boolean StartTraceActivity(String record, MDMTraceSource traceSourceValue)
        {
            return Constants.TRACING_ENABLED ? MDMTraceHelper.StartTraceActivity(PopulateTraceRecord(record), traceSourceValue, false) : true;
        }

        private static Boolean StopTraceActivity(String record, MDMTraceSource traceSourceValue)
        {
            return Constants.TRACING_ENABLED ? MDMTraceHelper.StopTraceActivity(PopulateTraceRecord(record), traceSourceValue) : true;
        }

        private static Boolean TraceInformation(String record, MDMTraceSource traceSourceValue)
        {
            return Constants.TRACING_ENABLED ? MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, PopulateTraceRecord(record), traceSourceValue) : true;
        }

        private static Boolean TraceError(String record, MDMTraceSource traceSourceValue)
        {
            return MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, PopulateTraceRecord(record), traceSourceValue);
        }

        private static String PopulateTraceRecord(String record)
        {
            return TracingPrefix + record;
        }

        #endregion
    }
}