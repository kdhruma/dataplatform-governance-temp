using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace MDM.EntityManager.Business.EntityOperations
{
    using Core;
    using Core.Extensions;
    using ExceptionManager;
    using AttributeModelManager.Business;
    using BusinessObjects;
    using BusinessObjects.Diagnostics;
    using BusinessObjects.EntityIdentification;
    using CacheManager.Business;
    using Interfaces;
    using Utility;
    using RS.MDM.Configuration;
    using RS.MDM.ConfigurationObjects;
    using EntityModelManager.Business;
    using Helpers;
    using MessageManager.Business;
    
    /// <summary>
    /// 
    /// </summary>
    internal sealed class EntityIdentificationManager
    {
        #region Fields

        private const String IdentificationValuesDelimiter = "#@#";

        #endregion

        #region Public methods

        /// <summary>
        /// Processing EntityIdentification
        /// </summary>
        /// <param name="entities">Entities to process EntityIdentification</param>
        /// <param name="entityOperationResults"></param>
        /// <param name="callerContext">callercontext of the current entity</param>
        /// <param name="isPreProcess">Indicates processing of Entityidentification either pre/post process</param>
        public static void ProcessEntityIdentificationData(EntityCollection entities, EntityOperationResultCollection entityOperationResults, CallerContext callerContext, Boolean isPreProcess)
        {
            EntityCollection entitiesToBeIdentified = null;
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            var currentActivity = new DiagnosticActivity();

            DurationHelper durHelper = new DurationHelper(DateTime.Now);

            if (traceSettings.IsTracingEnabled)
            {
                if (callerContext != null)
                {
	                var ec = new ExecutionContext { CallerContext = callerContext };
	                currentActivity.Start(ec);
                }
                else
                {
                    currentActivity.Start();
                }
            }
            try
            {
                if (entities != null)
                {
                    entitiesToBeIdentified = new EntityCollection();
                    foreach (Entity entity in entities)
                    {
                        if (!entity.EntityTypeName.Equals("Category"))
                        {
                            entitiesToBeIdentified.Add(entity);
                        }
                    }
                }
                //Int32 Seq = 0;
                if (entitiesToBeIdentified != null && entitiesToBeIdentified.Count > 0)
                {
                    var entityIdentifierMapCollection = new EntityUniqueIdentificationMapCollection();

                    #region Get EntityIdentification Configs

                    var entityIdentificationConfigs = GetEntityIdentificationConfigs(entitiesToBeIdentified);

                    if (traceSettings.IsBasicTracingEnabled)
                    {
                        currentActivity.LogMessageWithDuration(MessageClassEnum.Information, null, "GetEntityIdentificationConfigs Finished", durHelper.GetCumulativeTimeSpanInMilliseconds());
                        currentActivity.LogInformation("Validation of EntityIdentification Configs started");
                        durHelper.ResetDuration();
                    }

                    #endregion

                    #region Validate EntityIdentification Configs

                    Boolean isConfigValid = ValidateEntityIdentificationConfigs(entityIdentificationConfigs, entityOperationResults);

                    if (traceSettings.IsBasicTracingEnabled)
                    {
                        currentActivity.LogInformation("Validation of EntityIdentification Configs isConfigValid: " + isConfigValid);
                        currentActivity.LogMessageWithDuration(MessageClassEnum.Information, null, "Validation of EntityIdentification Configs finished", durHelper.GetCumulativeTimeSpanInMilliseconds());
                        currentActivity.LogInformation("DefaultEntityMap process started");
                        durHelper.ResetDuration();
                    }

                    #endregion

                    if (isConfigValid)
                    {
                        #region Process Default 4 parameters(ShortName/Container/NodeType/Category) map

                        if (isPreProcess)
                        {
                            ProcessDefaultEntityMap(entitiesToBeIdentified, entityOperationResults, callerContext);
                        }

                        #endregion

                        #region Ensure EntityIdentifier Attributes

                        AttributeModelBL attrModelBL = new AttributeModelBL();
                        AttributeModelCollection attrModelCollection = attrModelBL.GetAllBaseAttributeModels();

                        EnsureEntityIdentificationAttributes(entitiesToBeIdentified, entityIdentificationConfigs, attrModelCollection);

                        #endregion Ensure EntityIdentifier Attributes

                        if (traceSettings.IsBasicTracingEnabled)
                        {
                            currentActivity.LogMessageWithDuration(MessageClassEnum.Information, null, "Ensure EntityIdentification Attributes Finished", durHelper.GetCumulativeTimeSpanInMilliseconds());
                            currentActivity.LogInformation("Preparation of Identifier Concatenated Values per entity started");
                            durHelper.ResetDuration();
                        }

                        if (entitiesToBeIdentified.Count > 0)
                        {
                            entityIdentifierMapCollection = GetEntityUniqueIdentificationMap(entitiesToBeIdentified, entityOperationResults, attrModelCollection, entityIdentificationConfigs);
                        }

                        if (traceSettings.IsBasicTracingEnabled)
                        {
                            currentActivity.LogMessageWithDuration(MessageClassEnum.Information, null, "Preparation of Identifier Concatenated Values per entity finished", durHelper.GetCumulativeTimeSpanInMilliseconds());
                            currentActivity.LogInformation("Process Entity Identifier Maps started");
                            durHelper.ResetDuration();
                        }

                        //Do actual processing to figure out action per entity
                        if (entityIdentifierMapCollection != null && entityIdentifierMapCollection.Count > 0)
                        {
                            LoadEntityUniqueIdentificationDetails(entityIdentifierMapCollection, callerContext);

                            if (traceSettings.IsBasicTracingEnabled)
                            {
                                currentActivity.LogMessageWithDuration(MessageClassEnum.Information, null, "Process Entity Identifier Maps finished", durHelper.GetCumulativeTimeSpanInMilliseconds());
                                currentActivity.LogInformation("Process Entity Action flags started");
                                durHelper.ResetDuration();
                            }

                            ProcessEntityActionFlag(entitiesToBeIdentified, entityIdentifierMapCollection, entityOperationResults, entityIdentificationConfigs, callerContext, isPreProcess);
                        }
                        else
                        {
                            if (traceSettings.IsBasicTracingEnabled)
                            {
                                currentActivity.LogInformation("EntityUniqueIdentificationMap collection is null or empty");
                            }
                        }

                        if (traceSettings.IsBasicTracingEnabled)
                        {
                            currentActivity.LogMessageWithDuration(MessageClassEnum.Information, null, "Process Entity Action flags finished", durHelper.GetCumulativeTimeSpanInMilliseconds());
                            currentActivity.LogInformation("Process Entity Identification completed. Exiting now.");
                            durHelper.ResetDuration();
                        }
                    }
                    else
                    {
                        if (traceSettings.IsBasicTracingEnabled)
                        {
                            currentActivity.LogInformation("EntityIdentificationMap config is not valid/Available,Skipping EntityIdentificationMap Processing");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LocaleMessageBL localeMessageBL = new LocaleMessageBL();
                String exceptionMessage = String.Format(localeMessageBL.Get(GlobalizationHelper.GetSystemDataLocale(), "113977", false, new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity)).Message, ex.Message);

                var appException = new ApplicationException(exceptionMessage, ex);

                new ExceptionHandler(appException);
                currentActivity.LogError(exceptionMessage);

	            LocaleMessage localeMessage = new LocaleMessage();
                localeMessage.Message = String.Format(localeMessageBL.Get(GlobalizationHelper.GetSystemDataLocale(), "111957", false, new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity)).Message, ex.Message);

                //Update this exception to each entity in the transaction
                EntityOperationsCommonUtility.UpdateEntityOperationResults(entitiesToBeIdentified, entityOperationResults, localeMessage, appException.Message);
            }
            finally
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.Stop();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityUniqueIdentifier"></param>
        /// <param name="callerContext"></param>
        /// <param name="currentTraceSettings"></param>
        public static void ResolveEntityUniqueIdentifier(EntityUniqueIdentifier entityUniqueIdentifier, CallerContext callerContext, TraceSettings currentTraceSettings = null)
        {
            var diagnosticActivity = new DiagnosticActivity();

            if (currentTraceSettings == null)
            {
                currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            }

            if (currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            try
            {
                #region Step: Resolve entity metadata ids(container id, category id, entity type id) from names using EntityModel Manager

                var entityModelContext = new EntityModelContext();
                var entityModelManager = new EntityModelBL();

                var needToLoadIds = false;

                if (entityUniqueIdentifier.CategoryId < 1 && !String.IsNullOrWhiteSpace(entityUniqueIdentifier.CategoryPath))
                {
                    entityModelContext.CategoryPath = entityUniqueIdentifier.CategoryPath;
                    needToLoadIds = true;
                }

                if (entityUniqueIdentifier.ContainerId < 1 && !String.IsNullOrWhiteSpace(entityUniqueIdentifier.ContainerName))
                {
                    entityModelContext.ContainerName = entityUniqueIdentifier.ContainerName;
                    needToLoadIds = true;
                }

                if (entityUniqueIdentifier.EntityTypeId < 1 && !String.IsNullOrWhiteSpace(entityUniqueIdentifier.EntityTypeName))
                {
                    entityModelContext.EntityTypeName = entityUniqueIdentifier.EntityTypeName;
                    needToLoadIds = true;
                }

                if (needToLoadIds)
                {
                    entityModelManager.FillEntityModelContextByName(ref entityModelContext, callerContext);

                    if (entityModelContext.CategoryId > 0)
                    {
                        entityUniqueIdentifier.CategoryId = entityModelContext.CategoryId;
                    }

                    if (entityModelContext.ContainerId > 0)
                    {
                        entityUniqueIdentifier.ContainerId = entityModelContext.ContainerId;
                    }

                    if (entityModelContext.EntityTypeId > 0)
                    {
                        entityUniqueIdentifier.EntityTypeId = entityModelContext.EntityTypeId;
                    }
                }

                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Loaded entity metadata ids(container id, category id, entity type id) in entity identifier");
                }

                #endregion

                #region Step: Fill entity id if not provided using old EntityMapBL..as new module is still not feature toggle on

                if (entityUniqueIdentifier.EntityId < 1 && !String.IsNullOrWhiteSpace(entityUniqueIdentifier.ExternalId))
                {
                    //Note: for 767, we would be using existing EntityMap for finding out entity

                    var entityMapBL = new EntityMapBL();

                    var internalEntityMap = entityMapBL.Get("Internal", entityUniqueIdentifier.ExternalId, entityUniqueIdentifier.ContainerId, entityUniqueIdentifier.EntityTypeId, entityUniqueIdentifier.CategoryId, null, callerContext.Application, callerContext.Module);

                    if (internalEntityMap != null)
                    {
                        entityUniqueIdentifier.EntityId = internalEntityMap.InternalId;

                        if (entityUniqueIdentifier.CategoryId < 1)
                        {
                            entityUniqueIdentifier.CategoryId = internalEntityMap.CategoryId;
                        }

                        if (entityUniqueIdentifier.EntityTypeId < 1)
                        {
                            entityUniqueIdentifier.EntityTypeId = internalEntityMap.EntityTypeId;
                        }

                        if (entityUniqueIdentifier.ContainerId < 1)
                        {
                            entityUniqueIdentifier.ContainerId = internalEntityMap.ContainerId;
                        }
                    }
                }

                #endregion

                #region Step: Fill missing metadata ids(container id, category id and entity type id) by making base entity get call

                if (entityUniqueIdentifier.EntityId > 0 && (entityUniqueIdentifier.ContainerId < 1 || entityUniqueIdentifier.CategoryId < 1 || entityUniqueIdentifier.EntityTypeId < 1))
                {
                    var fillOptions = new EntityGetOptions.EntityFillOptions();
                    
                    fillOptions.SetAllEntityPropertiesLevelFillOptionsTo(false); // No need to load entity properties during base entity get
                    
                    fillOptions.FillLookupDisplayValues = false;
                    fillOptions.FillUOMValues = false;
                    
                    var entityGetOptions = new EntityGetOptions { ApplyAVS = false, ApplySecurity = false, PublishEvents = false, FillOptions = fillOptions, UpdateCacheStatusInDB = false, UpdateCache = false };

                    var baseEntityContext = new EntityContext { LoadEntityProperties = true, LoadAttributes = false, Locale = LocaleEnum.UnKnown };

                    var baseEntity = new EntityBL().Get(entityUniqueIdentifier.EntityId, baseEntityContext, entityGetOptions, callerContext);

                    if (baseEntity != null && baseEntity.Id == entityUniqueIdentifier.EntityId)
                    {
                        entityUniqueIdentifier.ContainerId = baseEntity.ContainerId;
                        entityUniqueIdentifier.CategoryId = baseEntity.CategoryId;
                        entityUniqueIdentifier.EntityTypeId = baseEntity.EntityTypeId;
                    }
                }

                #endregion
            }
            finally
            {
                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="entityOperationResults"></param>
        /// <param name="attributeModelCollection"></param>
        /// <param name="entityIdentificationConfigs"></param>
        /// <returns></returns>
        private static EntityUniqueIdentificationMapCollection GetEntityUniqueIdentificationMap(EntityCollection entities, EntityOperationResultCollection entityOperationResults, AttributeModelCollection attributeModelCollection, Dictionary<String, EntityIdentificationConfig> entityIdentificationConfigs)
        {
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            var currentActivity = new DiagnosticActivity();
            if (traceSettings.IsTracingEnabled)
            {
                currentActivity.Start();
            }

            String configCacheKey;
            String message;
            EntityIdentificationConfig entityIDConfig;
            EntityUniqueIdentificationMap entityIdentifierMap;
            EntityUniqueIdentificationMapCollection entityIdentifierMapCollection = null;
            String entityIdentifierName;
            String identifiervalue;
            Boolean isValid = true;
            Boolean hasAnyAttributeChanged;
            String currentAttributeValue;
            Attribute entityIdentifierAttribute;
            Boolean isMetadataConfigured;
            LocaleMessageBL localeMessageBL = new LocaleMessageBL();

			try
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogInformation("Getting EntityUniqueIdentificationMapCollection started");
                }

                entityIdentifierMapCollection = new EntityUniqueIdentificationMapCollection();

                foreach (Entity entity in entities)
                {
	                //Validate EntityType
                    if (String.IsNullOrEmpty(entity.EntityTypeName) && entity.EntityTypeId < 1)
                    {
                        message = String.Format(localeMessageBL.Get(GlobalizationHelper.GetSystemDataLocale(), "113881", false, new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity)).Message, entity.Name);
                        entityOperationResults.AddEntityOperationResult(entity.Id, "", message, OperationResultType.Error);
                        continue;
                    }

                    if (String.IsNullOrEmpty(entity.ContainerName) && entity.ContainerId < 1)
                    {
                        message = String.Format(localeMessageBL.Get(GlobalizationHelper.GetSystemDataLocale(), "113982", false, new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity)).Message, entity.Name);
                        entityOperationResults.AddEntityOperationResult(entity.Id, "", message, OperationResultType.Error);
                        continue;
                    }

                    configCacheKey = GetEntityIdentificationConfigCacheKey(entity.OrganizationId, entity.ContainerId, entity.EntityTypeId);

                    entityIDConfig = entityIdentificationConfigs[configCacheKey];

                    entityIdentifierMap = new EntityUniqueIdentificationMap
                    {
                        Id = entity.Id, 
                        ExternalId = entity.ExternalId, 
                        ContainerId = entity.ContainerId, 
                        CategoryId = entity.CategoryId, 
                        EntityTypeId = entity.EntityTypeId
                    };

                    foreach (EntityIdentificationStep entityIdentificationStep in entityIDConfig.EntityIdentificationSteps)
                    {
                        hasAnyAttributeChanged = false;
                        isMetadataConfigured = false;

                        identifiervalue = entity.ContainerName + IdentificationValuesDelimiter + entity.EntityTypeName;
                        entityIdentifierName = String.Concat("EntityIdentifier", entityIdentificationStep.Seq);

                        foreach (IdentificationField identificationField in entityIdentificationStep.IdentificationFields)
                        {
                            if (identificationField.IdentifierType == EntityIdentificationFieldType.Metadata)
                            {
                                isMetadataConfigured = true;
                                if (identificationField.IdentifierName.ToLower() == "shortname")
                                {
                                    identifiervalue += IdentificationValuesDelimiter + entity.Name;
                                }
                                if (identificationField.IdentifierName.ToLower() == "longname")
                                {
                                    identifiervalue += IdentificationValuesDelimiter + entity.LongName;
                                }
                                if (identificationField.IdentifierName.ToLower() == "category")
                                {
                                    identifiervalue += IdentificationValuesDelimiter + entity.CategoryId;
                                }
                            }
                            else if (identificationField.IdentifierType == EntityIdentificationFieldType.Attribute)
                            {
                                currentAttributeValue = GetAttributeIdentifierValue(entity, identificationField.IdentifierName, attributeModelCollection, out entityIdentifierAttribute);

                                if ((entityIdentifierAttribute != null && entityIdentifierAttribute.CheckHasChanged()))
                                {
                                    hasAnyAttributeChanged = true;
                                }

                                if (!String.IsNullOrEmpty(currentAttributeValue))
                                {
                                    identifiervalue += IdentificationValuesDelimiter + currentAttributeValue;
                                }
                                else
                                {
                                    isValid = false;
                                    break;
                                }
                            }
                        }

                        if (isValid)
                        {
                            if (hasAnyAttributeChanged || isMetadataConfigured)
                            {
                                SetEntityIdentifierValue(entity, identifiervalue, entityIdentifierName, attributeModelCollection);
                                SetEntityIdentificationMapValue(entityIdentifierMap, entityIdentifierName, identifiervalue);
                                entityIdentifierMapCollection.Add(entityIdentifierMap);
                            }
                        }
                        else
                        {
                            if (traceSettings.IsTracingEnabled)
                            {
                                currentActivity.LogInformation("EntityUniqueIdentification is invalid,One of the entity identifier attribute value is blank or empty");
                            }
                        }
                    }
                }
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogInformation("Getting EntityUniqueIdentificationMapCollection completed");
                }
            }
            catch (Exception ex)
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogError("Error while Getting EntityUniqueIdentificationMapCollection ,Error: " + ex.Message);
                }
            }
            finally
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.Stop();
                }
            }

			return entityIdentifierMapCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityIdentifierMap"></param>
        /// <param name="entityIdentifierName"></param>
        /// <param name="identifiervalue"></param>
        private static void SetEntityIdentificationMapValue(EntityUniqueIdentificationMap entityIdentifierMap, String entityIdentifierName, String identifiervalue)
        {
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            var currentActivity = new DiagnosticActivity();
            if (traceSettings.IsTracingEnabled)
            {
                currentActivity.Start();
            }
            try
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogInformation("SetEntityIdentificationMapValue started");
                }

                PropertyInfo[] properties = entityIdentifierMap.GetType().GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    if (property.Name.Equals(entityIdentifierName + "Value"))
                    {
                        property.SetValue(entityIdentifierMap, identifiervalue, null);
                    }
                }
            }
            catch (Exception ex)
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogError("Error while SetEntityIdentificationMapValue for EntityIdentifierName " + entityIdentifierName + " Error:" + ex.Message);
                }
            }
            finally
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogInformation("SetEntityIdentificationMapValue completed");
                    currentActivity.Stop();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityIdentifierMap"></param>
        /// <param name="entityIdentifierName"></param>
        /// <returns></returns>
        private static Int64 GetEntityIdentifierMapResult(EntityUniqueIdentificationMap entityIdentifierMap, String entityIdentifierName)
        {
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            var currentActivity = new DiagnosticActivity();
            if (traceSettings.IsTracingEnabled)
            {
                currentActivity.Start();
            }
            Int64 result = 0;
            Object value;

            try
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogInformation("GetEntityIdentifierMapResult started");
                }
                PropertyInfo[] properties = entityIdentifierMap.GetType().GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    if (property.Name.Equals(entityIdentifierName + "Result"))
                    {
                        value = property.GetValue(entityIdentifierMap, null);
                        if (value != null)
                        {
                            Int64.TryParse(value.ToString(), out result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogError("Error while GetEntityIdentifierMapResult for EntityIdentifierName " + entityIdentifierName + " Error:" + ex.Message);
                }
            }
            finally
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogInformation("GetEntityIdentifierMapResult completed");
                    currentActivity.Stop();
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityIdentifierMap"></param>
        /// <param name="entityIdentifierName"></param>
        /// <returns></returns>
        private static String GetEntityIdentifierMapValue(EntityUniqueIdentificationMap entityIdentifierMap, String entityIdentifierName)
        {
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            var currentActivity = new DiagnosticActivity();
            if (traceSettings.IsTracingEnabled)
            {
                currentActivity.Start();
            }

            String identifiervalue = String.Empty;
            Object value;

            try
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogInformation("GetEntityIdentifierMapValue started");
                }

                PropertyInfo[] properties = entityIdentifierMap.GetType().GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    if (property.Name.Equals(entityIdentifierName + "Value"))
                    {
                        value = property.GetValue(entityIdentifierMap, null);
                        if (value != null)
                        {
                            identifiervalue = value.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogError("Error while GetEntityIdentifierMapValue for EntityIdentifierName " + entityIdentifierName + " Error:" + ex.Message);
                }
            }
            finally
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogInformation("GetEntityIdentifierMapValue completed");
                    currentActivity.Stop();
                }
            }

            return identifiervalue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="identifierName"></param>
        /// <param name="attrModelCollection"></param>
        /// <param name="entityIdentifierAttribute"></param>
        /// <returns></returns>
        private static String GetAttributeIdentifierValue(Entity entity, String identifierName, AttributeModelCollection attrModelCollection, out Attribute entityIdentifierAttribute)
        {
            String attributeValue = String.Empty;
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            var currentActivity = new DiagnosticActivity();
            entityIdentifierAttribute = null;

            if (traceSettings.IsTracingEnabled)
            {
                currentActivity.Start();
            }

            try
            {
                if (entity != null)
                {
                    if (traceSettings.IsTracingEnabled)
                    {
                        currentActivity.LogInformation("GetAttributeIdentifierValue started");
                    }

                    entityIdentifierAttribute = GetAttribute(entity, identifierName, attrModelCollection);

                    if (entityIdentifierAttribute != null && entityIdentifierAttribute.AttributeModelType != AttributeModelType.Relationship)
                    {
                        if (entityIdentifierAttribute.IsComplex)
                        {
                            attributeValue = GetAttributeValue(entityIdentifierAttribute, identifierName);
                        }
                        else
                        {
                            attributeValue = GetAttributeValue(entityIdentifierAttribute);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogError("Error occured while getting AttributeIdentifierValue,Exception " + ex.Message);
                }
            }

            finally
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogInformation("GetAttributeIdentifierValue completed");
                    currentActivity.Stop();
                }
            }

            return attributeValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="identifierName"></param>
        /// <param name="attrModelCollection"></param>
        /// <returns></returns>
        private static Attribute GetAttribute(Entity entity, String identifierName, AttributeModelCollection attrModelCollection)
        {
            /*If EntityIdentifier is not available in Entity Object add the Attribute Model and Attribute Object
             if Identifier is complex child then adding the parent model and parent attribute*/
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            var currentActivity = new DiagnosticActivity();
            if (traceSettings.IsTracingEnabled)
            {
                currentActivity.Start();
            }

            Attribute entityIdentifierAttribute = null;
            AttributeModel attributeModel;

            try
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogInformation("Getting Attribute Identifier Started");
                }

                attributeModel = entity.AttributeModels.FirstOrDefault(x => (x.NameInLowerCase.Equals(identifierName.ToLower())));

                if (attributeModel == null)
                {
                    attributeModel = attrModelCollection.FirstOrDefault(x => (x.NameInLowerCase.Equals(identifierName.ToLower())));

                    if (attributeModel != null)
                    {
                        if (!attributeModel.IsComplexChild)
                        {
                            entity.AttributeModels.Add(attributeModel, true);
                            Attribute attr = new Attribute(attributeModel);
                            attr.Locale = entity.Locale;
                            entity.Attributes.Add(attr);
                        }
                        else
                        {
                            AttributeModel complexAttributeModel = attrModelCollection.FirstOrDefault(x => x.NameInLowerCase.Equals(attributeModel.AttributeParentName.ToLower()));
                            if (complexAttributeModel != null)
                            {
                                entity.AttributeModels.Add(complexAttributeModel, true);
                                Attribute complexAttr = new Attribute(complexAttributeModel);
                                complexAttr.Locale = entity.Locale;
                                entity.Attributes.Add(complexAttr);
                            }
                        }
                    }
                }

                //Always identifier Object exists ,In case of complex returning Parent attribute
                entityIdentifierAttribute = entity.Attributes.FirstOrDefault(x => (x.NameInLowerCase == identifierName.ToLower()));

                if (entityIdentifierAttribute == null)
                {
                    if (attributeModel != null && attributeModel.IsComplexChild)
                    {
                        entityIdentifierAttribute = (Attribute)entity.GetAttribute(attributeModel.AttributeParentName);
                    }
                }
            }
            catch (Exception ex)
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogError("Getting Attribute Identifier failed,Error: " + ex.Message);
                }
            }
            finally
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogInformation("Getting Attribute Identifier completed");
                    currentActivity.Stop();
                }
            }


            return entityIdentifierAttribute;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="identifiervalue"></param>
        /// <param name="targetentityIdentifier"></param>
        /// <param name="attrModelCollection"></param>
        private static void SetEntityIdentifierValue(Entity entity, String identifiervalue, String targetentityIdentifier, AttributeModelCollection attrModelCollection)
        {
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            var currentActivity = new DiagnosticActivity();
            if (traceSettings.IsTracingEnabled)
            {
                currentActivity.Start();
            }

			Attribute entityIdentifierAttribute;

            try
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogInformation("SetEntityIdentifierValue started");
                }
                entityIdentifierAttribute = GetAttribute(entity, targetentityIdentifier, attrModelCollection);

                if (entityIdentifierAttribute != null)
                {
                    entityIdentifierAttribute.SetValue(identifiervalue);
                }
            }
            catch (Exception ex)
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogError("Error while setting EntityIdentifierValue,Exception: " + ex.Message);
                }
            }
            finally
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogInformation("SetEntityIdentifierValue completed");
                    currentActivity.Stop();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        private static Dictionary<String, EntityIdentificationConfig> GetEntityIdentificationConfigs(EntityCollection entities)
        {
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            var currentActivity = new DiagnosticActivity();
            if (traceSettings.IsTracingEnabled)
            {
                currentActivity.Start();
            }

            Dictionary<String, EntityIdentificationConfig> entityIdentificationConfigs = null;

            try
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogInformation("GetEntityIdentificationConfigs started");
                }

                ICache cache = CacheFactory.GetCache();
                entityIdentificationConfigs = new Dictionary<String, EntityIdentificationConfig>();
                var distinctCacheKeys = entities.GroupBy(x => new { x.OrganizationId, x.ContainerId, x.EntityTypeId });

                foreach (var group in distinctCacheKeys)
                {
                    //Prepare cache key
                    String cacheKey = String.Format("RS_ApplicationConfigurations_EventSource:{0}_EventSubscriber:{1}_Role:{2}_User:{3}_Organization:{4}_Container:{5}_Category:{6}_Entity:{7}_Attribute:{8}_EntityType:{9}_RelationshipType:{10}_Locale:{11}_ApplicationConfig:{12}",
                        (int)RS.MDM.Events.EventSourceList.MDMCenter,
                        (int)RS.MDM.Events.EventSubscriberList.EntityUniqueIdentification, 0, 0, group.Key.OrganizationId,
                        group.Key.ContainerId, 0, 0, 0, group.Key.EntityTypeId, 0, 0, 0);

                    //look into cache
                    Object cachedConfig = cache[cacheKey];

                    if (cachedConfig == null)
                    {
                        //Make DB call if not found in cache
                        ApplicationConfiguration applicationConfiguration = new ApplicationConfiguration(
                            (int)RS.MDM.Events.EventSourceList.MDMCenter,
                            (int)RS.MDM.Events.EventSubscriberList.EntityUniqueIdentification, 0, 0, group.Key.OrganizationId,
                            group.Key.ContainerId, 0, 0, 0, group.Key.EntityTypeId, 0, 0, 0);

                        applicationConfiguration.GetConfigurations();
                        cachedConfig = applicationConfiguration.GetObject("EntityIdentificationConfig");

                        //set the config in cache
                        if (applicationConfiguration.Items.Count > 0 && cachedConfig != null)
                        {
                            cache.Set(cacheKey, cachedConfig, DateTime.Now.AddHours(12.0));
                        }
                    }

                    if (cachedConfig == null)
                    {
                        //TODO: Review needed
                        throw new Exception(String.Format("Failed to load Entity Identification Configuration for Org:{0}_Catalog:{1}_EntityType:{2}", group.Key.OrganizationId, group.Key.ContainerId, group.Key.EntityTypeId));
                    }

                    EntityIdentificationConfig entityIdentificationConfig = (EntityIdentificationConfig)cachedConfig;
                    String configCacheKey = GetEntityIdentificationConfigCacheKey(group.Key.OrganizationId, group.Key.ContainerId, group.Key.EntityTypeId);
                    entityIdentificationConfigs.Add(configCacheKey, entityIdentificationConfig);
                }
            }
            catch (Exception ex)
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogError("Error occured while Getting EntityIdentificationConfigs,Exception :" + ex.Message);
                }
            }
            finally
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogInformation("GetEntityIdentificationConfigs completed");
                    currentActivity.Stop();
                }
            }
            return entityIdentificationConfigs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityIdentifierMapCollection"></param>
        /// <param name="callerContext"></param>
        private static void LoadEntityUniqueIdentificationDetails(EntityUniqueIdentificationMapCollection entityIdentifierMapCollection, CallerContext callerContext)
        {
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            var currentActivity = new DiagnosticActivity();
            if (traceSettings.IsTracingEnabled)
            {
                currentActivity.Start();
            }

            try
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogInformation("ProcessEntityIdentifierMaps started");
                }

                var entityIdentifierMapManager = new EntityUniqueIdentificationMapBL();
                entityIdentifierMapManager.LoadEntityUniqueIdentificationDetails(entityIdentifierMapCollection, callerContext.Application, callerContext.Module);
            }
            catch (Exception ex)
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogError("Error occured while ProcessEntityIdentifierMaps,Exception: " + ex.Message);
                }
            }
            finally
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogInformation("ProcessEntityIdentifierMaps completed");
                    currentActivity.Stop();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="entityIdentifierMapCollection"></param>
        /// <param name="entityOperationResults"></param>
        /// <param name="entityIdentificationConfigs"></param>
        /// <param name="callerContext"></param>
        /// <param name="isPreProcess"></param>
        private static void ProcessEntityActionFlag(EntityCollection entities, EntityUniqueIdentificationMapCollection entityIdentifierMapCollection, EntityOperationResultCollection entityOperationResults, Dictionary<String, EntityIdentificationConfig> entityIdentificationConfigs, CallerContext callerContext, Boolean isPreProcess)
        {
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            var currentActivity = new DiagnosticActivity();
            
            if (traceSettings.IsTracingEnabled)
            {
                currentActivity.Start();
            }
            if (traceSettings.IsBasicTracingEnabled)
            {
                currentActivity.LogInformation("ProcessEntityActionFlags started");
            }

            LocaleMessageBL localeMessageBL = new LocaleMessageBL();

            try
            {
                foreach (Entity entity in entities)
                {
                    var entityOperationResult = entityOperationResults.GetEntityOperationResult(entity.Id);

                    if (entityOperationResult == null)
                    {
                        entityOperationResult = new EntityOperationResult(entity);
                        entityOperationResults.Add(entityOperationResult);
                    }

                    var configCacheKey = GetEntityIdentificationConfigCacheKey(entity.OrganizationId, entity.ContainerId, entity.EntityTypeId);
                    var entityIdentificationConfig = entityIdentificationConfigs[configCacheKey];

                    ObjectAction originalActionFlag = entity.Action;
                    ObjectAction currentActionFlag = ObjectAction.Unknown;
                    ObjectAction computedActionFlag = ObjectAction.Unknown;

                    //Boolean anyResultFound = false;
                    var entityIdentifierMap = (EntityUniqueIdentificationMap)entityIdentifierMapCollection.GetEntityIdentifierMap(entity.Id);

                    String message;
                    if (entityIdentifierMap != null)
                    {
                        Int16 identificationStep = 0;
                        RS.MDM.Collections.Generic.List<EntityIdentificationStep> entityIdentificationSteps = entityIdentificationConfig.EntityIdentificationSteps;

                        foreach (EntityIdentificationStep entityIdentificationStep in entityIdentificationSteps)
                        {
                            identificationStep++;
                            Boolean isLastIdentificationStep = identificationStep == entityIdentificationSteps.Count;

                            var entityIdentifierName = String.Concat("EntityIdentifier", entityIdentificationStep.Seq);
                            
                            var identifierValue = GetEntityIdentifierMapValue(entityIdentifierMap, entityIdentifierName);
                            var entityIdentifierResult = GetEntityIdentifierMapResult(entityIdentifierMap, entityIdentifierName);

                            /*EntityIdentifier result different options available
                             * 1. can be greater than 0 or 
                             * 2. less than 0 
                             * /*Create or Update
                             Create case : When Entity Identifier doesnt exists in the system
                             Update case : For an existing entity when Entity identifier is modified and in not available that case returning 0
                             */

                            if (entityIdentifierResult == 0)
                            {
                                if (originalActionFlag == ObjectAction.Create)
                                {
                                    if (entityIdentificationStep.BehaviorOnNoMatch == EntityIdentificationBehavior.Create)
                                    {
                                        currentActionFlag = ObjectAction.Create;
                                    }
                                }
                                if (originalActionFlag == ObjectAction.Update || originalActionFlag == ObjectAction.Reclassify || originalActionFlag == ObjectAction.ReParent)
                                {   //If one of the identifier attribute value is changing, but entity is already flagged as Update - do nothing
                                    currentActionFlag = originalActionFlag;

                                }
                                if (currentActionFlag != ObjectAction.Unknown)
                                {
                                    entity.Action = computedActionFlag = currentActionFlag;
                                    break;
                                }
                            }
                            else if (entityIdentifierResult > 0)
                            {
                                if ((originalActionFlag != ObjectAction.Create && entity.Id != entityIdentifierResult) ||
                                    (originalActionFlag == ObjectAction.Create && callerContext.Module != MDMCenterModules.Import))
                                {
                                    if (!isPreProcess)
                                    {
                                        if (entityIdentificationStep.BehaviorOnDuplicateMatches == EntityIdentificationBehavior.RaiseError || isLastIdentificationStep)
                                        {
                                            message = String.Format(localeMessageBL.Get(GlobalizationHelper.GetSystemDataLocale(), "113981", false, callerContext).Message, identifierValue, entityIdentifierMap.ContainerId);
                                            entityOperationResult.AddOperationResult("", message, OperationResultType.Error);
                                            currentActionFlag = ObjectAction.Unknown;
                                        }
                                        else
                                        {
                                            computedActionFlag = ObjectAction.Unknown;
                                        }
                                    }
                                }
                                else
                                {
                                    if (entity.EntityMoveContext.TargetCategoryId > 0 && entity.EntityMoveContext.FromCategoryId > 0 &&
                                             entity.CategoryId != entity.EntityMoveContext.TargetCategoryId)
                                    {
                                        currentActionFlag = ObjectAction.Reclassify;
                                    }
                                    else if (entity.Action == ObjectAction.Delete)
                                    {
                                        currentActionFlag = ObjectAction.Delete;
                                    }
                                    else
                                    {
                                        currentActionFlag = ObjectAction.Update;
                                    }

                                    entity.Id = entityOperationResult.EntityId = entityIdentifierResult;
                                    entity.Action = computedActionFlag = currentActionFlag;
                                    break;
                                }
                            }
                            else if (entityIdentifierResult == -100)
                            {
                                if (!isPreProcess)
                                {
                                    if (entityIdentificationStep.BehaviorOnDuplicateMatches == EntityIdentificationBehavior.RaiseError || isLastIdentificationStep)
                                    {
                                        message = String.Format(localeMessageBL.Get(GlobalizationHelper.GetSystemDataLocale(), "113979", false, callerContext).Message, identifierValue, entityIdentifierMap.ContainerId);
                                        entityOperationResult.AddOperationResult("", message, OperationResultType.Error);
                                        currentActionFlag = ObjectAction.Unknown;
                                    }
                                    else
                                    {
                                        computedActionFlag = ObjectAction.Unknown;
                                    }
                                }
                            }
                        }

                        if (computedActionFlag == ObjectAction.Unknown && originalActionFlag == ObjectAction.Unknown)
                        {
                            entity.Action = ObjectAction.Create;
                        }
                    }
                    else
                    {
                        //Not having EntityIdentificationMap
                        if (originalActionFlag == ObjectAction.Unknown)
                        {
                            message = localeMessageBL.Get(GlobalizationHelper.GetSystemDataLocale(), "113978", false, callerContext).Message;
                            entityOperationResult.AddOperationResult("", message, OperationResultType.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogError("Error while processing ProcessEntityActionFlags " + ex.Message);
                }
            }
            finally
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogInformation("ProcessEntityActionFlags finished");
                    currentActivity.Stop();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="entityOperationResults"></param>
        /// <param name="callerContext"></param>
        private static void ProcessDefaultEntityMap(EntityCollection entities, EntityOperationResultCollection entityOperationResults, CallerContext callerContext)
        {
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            var currentActivity = new DiagnosticActivity();

			if (traceSettings.IsTracingEnabled)
            {
                if (callerContext != null)
                {
                    var ec = new ExecutionContext();
                    ec.CallerContext = callerContext;
                    currentActivity.Start(ec);
                }
                else
                {
                    currentActivity.Start();
                }
            }

            if (traceSettings.IsBasicTracingEnabled)
            {
                currentActivity.LogInformation("Processing default entity map started");
            }

			try
            {
                EntityMapCollection entityMapCollection = new EntityMapCollection();

                foreach (Entity entity in entities)
                {
                    EntityMap entityMap = new EntityMap(entity.Id, "Internal", 1, "Entity", entity.Name, -1, entity.ContainerId, entity.CategoryId, entity.EntityTypeId);
                    entityMapCollection.Add(entityMap);
                }

                EntityMapBL entityMapManager = new EntityMapBL();
				entityMapManager.LoadInternalDetails(entityMapCollection, null, callerContext.Application, callerContext.Module);

                foreach (Entity entity in entities)
                {
                    EntityMap entityMap = (EntityMap)entityMapCollection.GetEntityMap(entity.Id);
                    EntityOperationResult entityOperationResult = entityOperationResults.GetEntityOperationResult(entity.Id);

                    if (entityOperationResult == null)
                    {
                        entityOperationResult = new EntityOperationResult(entity);
                        entityOperationResults.Add(entityOperationResult);
                    }

                    if (entityMap != null)
                    {
                        if (entityMap.InternalId > 0)
                        {
                            if(entity.Action == ObjectAction.Create && callerContext.Module == MDMCenterModules.Import)
                            {
                                entity.Id = entityOperationResult.EntityId = entityMap.InternalId;
                                //ToDo: In case of Reclassification/Reparent setting Object action flag
                                if (entity.Action != ObjectAction.Delete)
                                {
                                    entity.Action = ObjectAction.Update;
                                }
                            }
                        }
                        else if (entityMap.InternalId == -100)
                        {
                            entity.Action = ObjectAction.Unknown;
                        }
                        else
                        {
                            entity.Action = ObjectAction.Create;
                        }
                    }
                    else
                    {
                        entity.Action = ObjectAction.Unknown;
                    }
                }
            }
            catch (Exception ex)
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogError("Error while Processing default entity map " + ex.Message);
                }
            }
            finally
            {
                if (traceSettings.IsTracingEnabled)
                {
                    currentActivity.LogInformation("Processing default entity map finished");
                    currentActivity.Stop();
                }
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entities"></param>
		/// <param name="entityIdentificationConfigs"></param>
		/// <param name="baseattrModelCollection"></param>
		private static void EnsureEntityIdentificationAttributes(EntityCollection entities, Dictionary<String, EntityIdentificationConfig> entityIdentificationConfigs, AttributeModelCollection baseattrModelCollection)
		{
			var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
			var currentActivity = new DiagnosticActivity();
			if (traceSettings.IsTracingEnabled)
			{
				currentActivity.Start();

			}
			if (traceSettings.IsBasicTracingEnabled)
			{
				currentActivity.LogInformation("Processing default entity map started");
			}

			try
			{
				if (entities != null && entities.Count > 0 && entityIdentificationConfigs != null && entityIdentificationConfigs.Count > 0)
				{
					EntityBL entityManager = new EntityBL();

					CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

					var entityIdentifierAttributeIds = new Collection<Int32>
                    {
                        (Int32)SystemAttributes.EntityIdentifier1,
                        (Int32)SystemAttributes.EntityIdentifier2,
                        (Int32)SystemAttributes.EntityIdentifier3,
                        (Int32)SystemAttributes.EntityIdentifier4,
                        (Int32)SystemAttributes.EntityIdentifier5
                    };

					foreach (var baseAttributeModel in baseattrModelCollection)
					{
						if (baseAttributeModel.IsComplex)
						{
							var childAttributeModels = baseAttributeModel.AttributeModels;

							if (childAttributeModels != null && childAttributeModels.Count > 0)
							{
								foreach (var childAttributeModel in childAttributeModels)
								{
									MatchAndFillEntitiyIdentifierAttributeIds(entityIdentificationConfigs, baseAttributeModel.Id, childAttributeModel.Name, entityIdentifierAttributeIds);
								}
							}
						}
						else
						{
							MatchAndFillEntitiyIdentifierAttributeIds(entityIdentificationConfigs, baseAttributeModel.Id, baseAttributeModel.Name, entityIdentifierAttributeIds);
						}
					}

					foreach (var entity in entities)
					{
						entityManager.EnsureAttributes(entity, entityIdentifierAttributeIds, true, callerContext);
					}
				}
			}
			catch (Exception ex)
			{

				if (traceSettings.IsTracingEnabled)
				{
					currentActivity.LogError("Error while EnsureEntityIdentificationAttributes " + ex.Message);
				}
			}
			finally
			{
				if (traceSettings.IsTracingEnabled)
				{
					currentActivity.LogInformation("Processing EnsureEntityIdentificationAttributes finished");
					currentActivity.Stop();
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entityIdentificationConfigs"></param>
		/// <param name="attributeId"></param>
		/// <param name="attributeName"></param>
		/// <param name="entityIdentifierAttributeIds"></param>
		private static void MatchAndFillEntitiyIdentifierAttributeIds(Dictionary<String, EntityIdentificationConfig> entityIdentificationConfigs, Int32 attributeId, String attributeName, Collection<Int32> entityIdentifierAttributeIds)
		{
			foreach (var entityIdentificationConfig in entityIdentificationConfigs.Values)
			{
				foreach (var entityIdentificationStep in entityIdentificationConfig.EntityIdentificationSteps)
				{
					foreach (var identificationField in entityIdentificationStep.IdentificationFields)
					{
						if (identificationField.IdentifierType == EntityIdentificationFieldType.Attribute
							&& String.Compare(attributeName, identificationField.IdentifierName, StringComparison.InvariantCultureIgnoreCase) == 0)
						{
							if (!entityIdentifierAttributeIds.Contains(attributeId))
							{
								entityIdentifierAttributeIds.Add(attributeId);
							}
						}
					}
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityIdentificationConfigs"></param>
        /// <param name="entityOperationResultCollection"></param>
        /// <returns></returns>
        private static Boolean ValidateEntityIdentificationConfigs(Dictionary<String, EntityIdentificationConfig> entityIdentificationConfigs, EntityOperationResultCollection entityOperationResultCollection)
        {
            Boolean result = true;
            IEnumerable<EntityIdentificationConfig> allConfigs = entityIdentificationConfigs.Values.AsEnumerable().ToList();
            DiagnosticActivity activity = new DiagnosticActivity();
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            if (traceSettings.IsBasicTracingEnabled)
            {
                activity.Start();
            }
            try
            {
                if (traceSettings.IsBasicTracingEnabled)
                {
                    activity.LogInformation("ValidateEntityIdentificationConfigs started");
                }

                #region Check Atleast one EntityIdentifier exists

                IEnumerable<EntityIdentificationConfig> noChildEntityIdentifiers = allConfigs.Where(y => y.EntityIdentificationSteps.Count < 1);

                if (noChildEntityIdentifiers.Any())
                {
                    //TODO: Filter relevant entities and do partial processing instead of failing whole process
                    //TODO: Error message to mention exact Config where no identifiers are present                
                    entityOperationResultCollection.AddOperationResult("", "Validation failed for EntityIdentificationConfig: At least 1 EntityIdentifier is required within a EntityIdentification Config", OperationResultType.Error);
                    result = false;
                }

                #endregion Check Atleast one EntityIdentifier exists

                #region Check Atleast one Identifier exists

                IEnumerable<EntityIdentificationStep> noChildIdentifiers = allConfigs.SelectMany(y => y.EntityIdentificationSteps)
                                                        .Where(x => x.IdentificationFields.Count < 1);

                if (noChildIdentifiers.Any())
                {
                    //TODO: Filter relevant entities and do partial processing instead of failing whole process
                    //TODO: Error message to mention exact Config where no identifiers are present                
                    entityOperationResultCollection.AddOperationResult("", "Validation failed for EntityIdentificationConfig: At least 1 Identifier is required per EntityIdentifier", OperationResultType.Error);
                    result = false;
                }

                #endregion Check Atleast one Identifier exists

                #region Check Seq

                IEnumerable<EntityIdentificationStep> incorrectSeq = allConfigs.SelectMany(y => y.EntityIdentificationSteps)
                                                        .Where(x => x.Seq.GetInt(0) != 0 && !(x.Seq.GetInt(0) >= 1 && x.Seq.GetInt(0) <= 5));

                if (incorrectSeq.Any())
                {
                    //TODO: Filter relevant entities and do partial processing instead of failing whole process
                    //TODO: Error message to mention exact Config and the identifiers with wrong seq.                
                    entityOperationResultCollection.AddOperationResult("", "Only integers and 1 to 5 are allowed for Seq in EntityIdentificationConfig", OperationResultType.Error);
                    result = false;
                }

                #endregion Check Seq

                #region Check if IdentifierName is not empty

                IEnumerable<IdentificationField> emptyIdentifiers = allConfigs.SelectMany(y => y.EntityIdentificationSteps)
                                                        .SelectMany(x => x.IdentificationFields)
                                                        .Where(x => String.IsNullOrEmpty(x.IdentifierName));

                if (emptyIdentifiers.Any())
                {
                    //TODO: Filter relevant entities and do partial processing instead of failing whole process
                    //TODO: Error message to mention exact Config and the incorrect identifier name.                
                    entityOperationResultCollection.AddOperationResult("", "IdentifierName cannot be empty", OperationResultType.Error);
                    result = false;
                }

                #endregion Check if IdentifierName is not empty

                #region Check IdentifierName for Metadata

                IEnumerable<IdentificationField> incorrectIdentifiers = allConfigs.SelectMany(y => y.EntityIdentificationSteps)
                                                        .SelectMany(x => x.IdentificationFields)
                                                        .Where(x => x.IdentifierType == EntityIdentificationFieldType.Metadata
                                                        && !(x.IdentifierName.ToLower() == "shortname" || x.IdentifierName.ToLower() == "longname" || x.IdentifierName.ToLower() == "category"));

                if (incorrectIdentifiers.Any())
                {
                    //TODO: Filter relevant entities and do partial processing instead of failing whole process
                    //TODO: Error message to mention exact Config and the incorrect identifier name.                
                    entityOperationResultCollection.AddOperationResult("", "Incorrect IdentifierName found in EntityIdentificationConfig", OperationResultType.Error);
                    result = false;
                }

                #endregion Check IdentifierName for Metadata
            }
            catch (Exception ex)
            {
                if (traceSettings.IsBasicTracingEnabled)
                {
                    activity.LogError("Error occured while validating EntityIdentification Config,Exception " + ex.Message);
                }
            }
            finally
            {
                if (traceSettings.IsBasicTracingEnabled)
                {
                    activity.LogInformation("ValidateEntityIdentificationConfigs completed");
                    activity.Stop();
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        private static String GetAttributeValue(IAttribute attribute)
        {
            String delimeter = "||";
            LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();
            DiagnosticActivity activity = new DiagnosticActivity();
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            if (traceSettings.IsBasicTracingEnabled)
            {
                activity.Start();
            }

            String attributeValue = "";

            try
            {

                if (attribute != null)
                {
                    if (traceSettings.IsBasicTracingEnabled)
                    {
                        activity.LogInformation(String.Format("Utility:GetAttributeValue for an Attribute: " + attribute.Name));
                    }

                    if (attribute.IsCollection)
                    {
                        IValueCollection valueCollection = attribute.GetCurrentValues(systemDataLocale);
                        if (valueCollection != null)
                        {
                            foreach (Value val in valueCollection)
                            {
                                String value = val.AttrVal.ToString();
                                if (attribute.AttributeDataType == AttributeDataType.Decimal)
                                {
                                    value = val.InvariantVal.ToString();
                                }
                                if (attribute.IsLookup)
                                {
                                    value = val.AttrVal.ToString();
                                }

                                if (val.Action != ObjectAction.Delete)
                                {
                                    attributeValue = attributeValue + value + delimeter;
                                }
                            }

                            if (attributeValue.Length > 0)
                            {
                                attributeValue = String.Format("{0}:{1}", attribute.Id, attributeValue.Substring(0, attributeValue.Length - delimeter.Length));
                            }
                        }
                    }
                    else
                    {
                        String value = attribute.GetCurrentValue(systemDataLocale) != null ? attribute.GetCurrentValue(systemDataLocale).ToString() : "";
                        if (attribute.AttributeDataType == AttributeDataType.Decimal)
                        {
                            value = attribute.GetCurrentValueInvariant() != null ? attribute.GetCurrentValueInvariant().ToString() : "";
                        }
                        if (attribute.IsLookup)
                        {
                            if (attribute.GetCurrentValueInstance(systemDataLocale) != null)
                            {
                                value = attribute.GetCurrentValue().ToString();
                            }
                        }
                        attributeValue = value;
                    }

					if (traceSettings.IsBasicTracingEnabled)
                    {
                        activity.LogInformation(String.Format("Utility:GetAttributeValue for an Attribute: " + attribute.Name + ";Attribute value " + attributeValue));
                    }
                }
            }
            catch (Exception ex)
            {
                if (traceSettings.IsBasicTracingEnabled)
                {
                    activity.LogError(String.Format("Utility:GetAttributeValue for an Attribute" + ex.Message));
                }
            }
            finally
            {
                if (traceSettings.IsBasicTracingEnabled)
                {
                    activity.LogInformation("Utility:GetAttributeValue is completed");
                    activity.Stop();
                }
            }

            return attributeValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="complexChildAttributeName"></param>
        /// <returns></returns>
        private static String GetAttributeValue(IAttribute attribute, String complexChildAttributeName)
        {
            String delimeter = "||";
            DiagnosticActivity activity = new DiagnosticActivity();
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            if (traceSettings.IsBasicTracingEnabled)
            {
                activity.Start();
            }
            String attributeValue = "";
            try
            {
                if (attribute != null && attribute.IsComplex)
                {
                    if (traceSettings.IsBasicTracingEnabled)
                    {
                        activity.LogInformation(String.Format("Utility:GetAttributeValue for an Attribute: " + attribute.Name));
                    }

                    foreach (IAttribute complexAttributeRow in attribute.GetChildAttributes())
                    {
                        foreach (IAttribute childAttribute in complexAttributeRow.GetChildAttributes())
                        {
                            if (childAttribute.Name.Contains(complexChildAttributeName))
                            {
                                attributeValue = attributeValue + GetAttributeValue(childAttribute) + delimeter;
                            }
                        }
                    }

                    if (!String.IsNullOrWhiteSpace(attributeValue))
                    {
                        attributeValue = String.Format("{0}:{1}", attribute.Id, attributeValue.Substring(0, attributeValue.Length - delimeter.Length));
                    }

					if (traceSettings.IsBasicTracingEnabled)
					{
						activity.LogInformation(String.Format("Utility:GetAttributeValue for an Attribute: " + attribute.Name + ";Attribute value: " + attributeValue));
					}
				}
            }
            catch (Exception ex)
            {
                if (traceSettings.IsBasicTracingEnabled)
                {
                    activity.LogError(String.Format("Utility:GetAttributeValue for an Attribute" + ex.Message));
                }
            }
            finally
            {
                if (traceSettings.IsBasicTracingEnabled)
                {
                    activity.LogInformation("Utility:GetAttributeValue is completed");
                    activity.Stop();
                }
            }
            return attributeValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <returns></returns>
        private static String GetEntityIdentificationConfigCacheKey(Int32 orgId, Int32 containerId, Int32 entityTypeId)
        {
            return String.Format("EntityIdentificationConfigCache_ORG{0}_CON{1}_ET{2}", orgId, containerId, entityTypeId);
        }

        #endregion
    }
}
