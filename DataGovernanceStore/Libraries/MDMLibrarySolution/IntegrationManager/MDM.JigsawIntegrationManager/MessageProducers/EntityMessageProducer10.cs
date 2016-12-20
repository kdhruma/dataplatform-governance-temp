using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace MDM.JigsawIntegrationManager.MessageProducers
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.BusinessObjects.Workflow;
    using MDM.Core;
    using MDM.Core.Extensions;
    using MDM.JigsawIntegrationManager.DataPackages;
    using MDM.JigsawIntegrationManager.DTO;
    using MDM.Utility;

    /// <summary>
    /// 
    /// </summary>
    public class EntityMessageProducer10
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        private const String DeletedValue = "__deleted__";

        /// <summary>
        /// 
        /// </summary>
        private static readonly IReadOnlyCollection<ObjectAction> ProcessedAttributeActions = new List<ObjectAction>
        {
            ObjectAction.Create,
            ObjectAction.Update,
            ObjectAction.Delete,
            ObjectAction.Replace
        };

        /// <summary>
        /// 
        /// </summary>
        private const Double DefaultQualityScore = 0;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<IMessageBase> GenerateEntityMessage(List<EntityMessageDataPackage> entityMessageDataPackages, CallerContext callerContext)
        {
            #region Diagnostics Initialization

            DiagnosticActivity diagnosticActivity = null;

            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            if (currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = new DiagnosticActivity();
                diagnosticActivity.Start();
            }

            #endregion

            var entityMessages = new List<IMessageBase>();

            try
            {
                if (!entityMessageDataPackages.IsNullOrEmpty())
                {
                    var defaultCulture = GlobalizationHelper.GetSystemDataLocale().GetCultureName();

                    foreach (var entityMessageDataPackage in entityMessageDataPackages)
                    {
                        var entityMessage = new EntityMessage();
                        
                        var entity = entityMessageDataPackage.Entity;

                        if(entity == null)
                        {
                            //TODO:: throw error?
                            continue;
                        }

                        entityMessage.Eid = entity.EntityGuid.HasValue ? entity.EntityGuid.ToString() : null;

                        #region Entity info and system info

                        entityMessage.EntityInfo = new DTO.EntityInfo {DefaultLocale = defaultCulture, EntityType = entity.EntityTypeName.ToJsCompliant() };

                        entityMessage.SystemInfo = new DTO.SystemInfo {TenantId = JigsawConstants.Tenant };

                        #endregion

                        #region attributes info

                        entityMessage.AttributesInfo = CreateAttributesInfo(entity, (MDM.BusinessObjects.Attribute attribute) => { return !attribute.HasInvalidValues; }, callerContext);

                        #endregion

                        #region invalid attributes info

                        entityMessage.InvalidAttributesInfo = CreateAttributesInfo(entity, (MDM.BusinessObjects.Attribute attribute) => { return attribute.HasInvalidValues; }, callerContext);
                        
                        #endregion

                        #region extended attributes info

                        var extendedAttributesInfo = new EntityExtendedAttributesInfo();

                        extendedAttributesInfo.JsRelationship = ProducerHelper.CreateRelationship(entity);

                        extendedAttributesInfo.JsChangeContext = ProducerHelper.CreateChangeContext(callerContext);

                        if (!entityMessageDataPackage.StateValidationScores.IsNullOrEmpty())
                        {
                            var stateValidations = entityMessageDataPackage.StateValidations;
                            var stateValidationScores= entityMessageDataPackage.StateValidationScores;
                            var ruleMaps = entityMessageDataPackage.RuleMaps;

                            extendedAttributesInfo.JsValidationStates = CreateValidationStates(entity);
                            extendedAttributesInfo.JsValidationStatesSummary = CreateValidationStatesSummary(extendedAttributesInfo.JsValidationStates, entityMessageDataPackage.StateValidations, entityMessageDataPackage.StateValidationScores);

                            if (entityMessageDataPackage.BusinessCondition != null)
                            {
                                var businessConditionStatuses = entityMessageDataPackage.BusinessCondition.BusinessConditions;

                                if (businessConditionStatuses != null && businessConditionStatuses.Count > 0)
                                {
                                    extendedAttributesInfo.JsBusinessConditionsSummary = CreateBusinessConditionsSummary(businessConditionStatuses, stateValidations, ruleMaps);
                                }
                            }
                        }

                        extendedAttributesInfo.JsWorkflow = CreateWorkflow(entityMessageDataPackage.WorkflowStates);

                        entityMessage.ExtendedAttributesInfo = extendedAttributesInfo;

                        #endregion

                        entityMessages.Add((IMessageBase)entityMessage);
                    }
                }
            }
            finally
            {
                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return entityMessages;
        }

        #region Private Methods


        /// <summary>
        /// Creates the attributes information.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="EvaluateAttributeForInvalidValues">The evaluate attribute for invalid values.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <returns></returns>
        private DTO.AttributesInfo CreateAttributesInfo(Entity entity, Func<MDM.BusinessObjects.Attribute, Boolean> EvaluateAttributeForInvalidValues, CallerContext callerContext)
        {
            #region Diagnostics Initialization

            DiagnosticActivity diagnosticActivity = null;

            var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            if (currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = new DiagnosticActivity();
                diagnosticActivity.Start();
            }

            #endregion

            var attributesInfo = new AttributesInfo { ExternalId = entity.Id };

            try
            {
                var localizedAttributesDictionary = new Dictionary<LocaleEnum, List<DTO.Attribute>>();

                if (entity.Attributes != null && entity.Attributes.Count > 0)
                {
                    foreach (var entityAttribute in entity.Attributes)
                    {
                        if (EvaluateAttributeForInvalidValues(entityAttribute) && !(entityAttribute.IsHierarchical || entityAttribute.IsValidationStateAttribute()))
                        {
                            if (ProcessedAttributeActions.Contains(entityAttribute.Action))
                            {
                                var attr = new DTO.Attribute(entityAttribute.Name.ToJsCompliant(), GetAttributeObjectValue(entityAttribute, callerContext));

                                if (!localizedAttributesDictionary.ContainsKey(entityAttribute.Locale))
                                {
                                    localizedAttributesDictionary.Add(entityAttribute.Locale, new List<DTO.Attribute>());
                                }

                                localizedAttributesDictionary[entityAttribute.Locale].Add(attr);
                            }
                        }
                    }

                    if (localizedAttributesDictionary != null && localizedAttributesDictionary.Count > 0)
                    {
                        foreach (var pair in localizedAttributesDictionary)
                        {
                            var localeAttributesJObject = new JObject();

                            var locale = pair.Key;
                            var attributes = pair.Value;

                            foreach (var attr in attributes)
                            {
                                localeAttributesJObject.Add(attr.Name, attr.Value);
                            }

                            attributesInfo.Attributes.Add(new DTO.Attribute(locale.GetCultureName(), localeAttributesJObject));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (!currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity = new DiagnosticActivity();
                    diagnosticActivity.Start();
                }

                diagnosticActivity.LogError(ex.StackTrace);
            }
            finally
            {
                if (diagnosticActivity != null)
                {
                    diagnosticActivity.Stop();
                }
            }

            return attributesInfo;
        }


        /// <summary>
        /// Gets the attribute object value.
        /// The object can be simple value in name value pair or colleciton of values.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="callerContext">The attribute.</param>
        /// <returns>JToken.</returns>
        private JToken GetAttributeObjectValue(MDM.BusinessObjects.Attribute attribute, CallerContext callerContext)
        {
            if (attribute == null)
            {
                return null;
            }

            if (attribute.Action == ObjectAction.Delete)
            {
                return new JValue(DeletedValue);
            }

            if (attribute.IsHierarchical)
            {
                return "Hierarchical attributes are not supported.";
            }
            else if (attribute.IsComplex)
            {
                JArray complexAttribute = new JArray();

                Int32 sequence = 0;

                foreach (var complexRow in attribute.GetChildAttributes())
                {
                    JObject complexRecord = new JObject();

                    complexRecord.Add(new JProperty("rowKeyId", sequence++.ToString()));

                    foreach (var complexCell in complexRow.GetChildAttributes())
                    {
                        var complexCellValue = GetAttributeValue(complexCell);

                        if (complexCellValue != null && complexCellValue.HasValues)
                        {
                            if (complexCell.IsCollection)
                            {
                                complexRecord.Add(new JProperty(complexCell.Name.ToJsCompliant(), complexCellValue));
                            }
                            else
                            {
                                complexRecord.Add(new JProperty(complexCell.Name.ToJsCompliant(), complexCellValue.FirstOrDefault()));
                            }
                        }
                    }

                    complexAttribute.Add(complexRecord);

                    if (!attribute.IsCollection)
                    {
                        return complexAttribute;
                    }
                }

                return complexAttribute;
            }
            else if (attribute.IsCollection)
            {
                return GetAttributeValue(attribute);
            }
            else
            {
                return GetAttributeValue(attribute).FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets collection of values of given attribute.
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns>
        /// Collection of Values of various data types - 
        ///         "textArea": "txt100\r\ntxt200",
        ///         "textBoxCollection": [
        ///             "txt box 1",
        ///             "txt box 2"
        ///          ]
        ///         "cmnlocintegeruomcoll": [
        ///             4,
        ///             5
        ///          ]
        ///         "decimalCollection": [
        ///             1.10,
        ///             2.20,
        ///             3.30
        ///          ]
        ///          "fractionCollection": [
        ///             "1 1/4",
        ///             "2 2/3"
        ///           ]
        /// </returns>
        private JArray GetAttributeValue(MDM.BusinessObjects.Attribute attribute)
        {
            JArray values = new JArray();

            var currentValues = attribute.GetCurrentValuesInvariant();

            if (currentValues != null && currentValues.Count > 0)
            {
                foreach (var value in currentValues)
                {
                    if (value.Action == ObjectAction.Delete)
                    {
                        continue;
                    }

                    if (attribute.IsLookup)
                    {
                        if (!value.HasInvalidValue)
                        {
                            values.Add(value.GetDisplayValue());
                        }
                        else
                        {
                            values.Add(value.InvariantVal != null ? value.InvariantVal.ToString() : String.Empty);
                        }
                    }
                    else
                    {
                        String currentValue = value.InvariantVal != null ? value.InvariantVal.ToString() : String.Empty;

                        if (!attribute.HasInvalidValues)
                        {
                            if ((attribute.AttributeDataType == AttributeDataType.Date || attribute.AttributeDataType == AttributeDataType.DateTime))
                            {
                                DateTime defaultValue = DateTime.MinValue;
                                DateTime dateTimeValue = ValueTypeHelper.DateTimeTryParse(currentValue, defaultValue);
                                if (dateTimeValue != DateTime.MinValue)
                                {
                                    values.Add(dateTimeValue.ToUniversalTime().ToString("O"));
                                }
                            }
                            else if (attribute.AttributeDataType == AttributeDataType.Boolean)
                            {
                                Boolean defaultValue = ValueTypeHelper.BooleanTryParse(currentValue, false);
                                values.Add(defaultValue);
                            }
                            else if (attribute.AttributeDataType == AttributeDataType.Integer || attribute.AttributeDataType == AttributeDataType.Decimal)
                            {
                                Decimal? numericalValue = value.GetNumericValue();

                                if (numericalValue.HasValue)
                                {
                                    if (attribute.AttributeDataType == AttributeDataType.Integer)
                                    {
                                        values.Add(ValueTypeHelper.ConvertToInt32(value.InvariantVal));
                                    }
                                    else
                                    {
                                        values.Add(numericalValue.Value);
                                    }
                                }
                            }
                            else
                            {
                                values.Add(currentValue);
                            }
                        }
                        else
                        {
                            values.Add(currentValue);
                        }
                    }
                } // foreach
            }

            return values;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationStates"></param>
        /// <param name="stateValidations"></param>
        /// <param name="stateValidationScores"></param>
        private DTO.ValidationStatesSummary CreateValidationStatesSummary(Collection<DTO.ValidationState> validationStates, EntityStateValidationCollection stateValidations, EntityStateValidationScoreCollection stateValidationScores)
        {
            var validStates = new Collection<String>();
            var inValidStates = new Collection<String>();
            var errorCodesForTypeError = new Collection<String>();
            var errorCodesForTypeWarning = new Collection<String>();
            var attributesWithErrors = new Collection<String>();
            var attributesWithWarnings = new Collection<String>();
            var attributesErrorCodeList = new Collection<String>();
            var reasonTypes = new Collection<String>();
            var dataQualityScore = DefaultQualityScore;

            if (validationStates != null && validationStates.Count > 0)
            {
                var stateValidationScore = stateValidationScores.FirstOrDefault();
                
                dataQualityScore =  stateValidationScore != null ?  stateValidationScore.OverallScore : DefaultQualityScore;

                foreach (var validationState in validationStates)
                {
                    #region valid / invalid states

                    if (validationState.Value == true)
                    {
                        if (!validStates.Contains(validationState.Name))
                        {
                            validStates.Add(validationState.Name);
                        }
                    }
                    else if (!inValidStates.Contains(validationState.Name))
                    {
                        inValidStates.Add(validationState.Name);
                    }

                    #endregion
                }
            }

            if (stateValidations != null && stateValidations.Count > 0)
            {
                foreach (var stateValidation in stateValidations)
                {
                    if (stateValidation.OperationResultType == OperationResultType.Error || stateValidation.OperationResultType == OperationResultType.Warning)
                    {
                        #region error / warning

                        String errorMessage = stateValidation.MessageCode;

                        if (!String.IsNullOrWhiteSpace(errorMessage))
                        {
                            if (stateValidation.OperationResultType == OperationResultType.Error && !errorCodesForTypeError.Contains(errorMessage))
                            {
                                errorCodesForTypeError.Add(errorMessage);
                            }
                            else if (stateValidation.OperationResultType == OperationResultType.Warning && !errorCodesForTypeWarning.Contains(errorMessage))
                            {
                                errorCodesForTypeWarning.Add(errorMessage);
                            }
                        }

                        #endregion

                        #region attribute with error / warning

                        var attributeLongName = stateValidation.AttributeLongName;

                        if (!String.IsNullOrWhiteSpace(attributeLongName))
                        {
                            if (stateValidation.OperationResultType == OperationResultType.Error && !attributesWithErrors.Contains(attributeLongName))
                            {
                                attributesWithErrors.Add(attributeLongName);
                            }
                            else if (stateValidation.OperationResultType == OperationResultType.Warning && !attributesWithWarnings.Contains(attributeLongName))
                            {
                                attributesWithWarnings.Add(attributeLongName);
                            }
                        }

                        #endregion

                        #region reason types

                        var reasonType = stateValidation.ReasonType;

                        if (reasonType != ReasonType.Unknown)
                        {
                            var resonTypeAsString = reasonType.ToString();

                            if (!reasonTypes.Contains(resonTypeAsString))
                            {
                                reasonTypes.Add(resonTypeAsString);
                            }
                        }

                        #endregion

                        #region attributes error desc list

                        String attributeErrorDesc = String.Format("{0} - {1}", attributeLongName, errorMessage);

                        if (!attributesErrorCodeList.Contains(attributeErrorDesc))
                        {
                            attributesErrorCodeList.Add(attributeErrorDesc);
                        }

                        #endregion
                    }
                }
            }

            var validationStatesSummary = new ValidationStatesSummary 
            {
                DataQualityScore = dataQualityScore,
                ValidStates = validStates,
                InValidStates = inValidStates,
                ErrorCodesForTypeError = errorCodesForTypeError,
                ErrorCodesForTypeWarning = errorCodesForTypeWarning,
                AttributesWithErrors = attributesWithErrors,
                AttributesWithWarnings = attributesWithWarnings,
                AttributesErrorCodeList = attributesErrorCodeList,
                ReasonTypes = reasonTypes
            };

            return validationStatesSummary;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessConditionStatues"></param>
        /// <param name="stateValidations"></param>
        /// <param name="ruleMaps"></param>
        /// <returns></returns>
        private DTO.BusinessConditionsSummary CreateBusinessConditionsSummary(BusinessConditionStatusCollection businessConditionStatues, EntityStateValidationCollection stateValidations, MDMRuleMapDetailCollection ruleMaps)
        {
            var passedBusinessConditions = new Collection<String>();
            var failedBusinessConditions = new Collection<String>();
            var unknownBusinessConditions = new Collection<String>();
            var errorCodesForTypeError = new Collection<String>();
            var errorCodesForTypeWarning = new Collection<String>();
            var attributesWithErrors = new Collection<String>();
            var attributesWithWarnings = new Collection<String>();
            var attributesErrorCodeList = new Collection<String>();
            var reasonTypes = new Collection<String>();

            if (businessConditionStatues != null && businessConditionStatues.Count > 0)
            {
                foreach (var businessConditionStatus in businessConditionStatues)
                {
                    if (businessConditionStatus != null)
                    {
                        #region Business conditions valid / invalid

                        if (businessConditionStatus.Status == ValidityStateValue.Valid && !passedBusinessConditions.Contains(businessConditionStatus.Name))
                        {
                            passedBusinessConditions.Add(businessConditionStatus.Name);
                        }
                        else if (businessConditionStatus.Status == ValidityStateValue.InValid && !failedBusinessConditions.Contains(businessConditionStatus.Name))
                        {
                            failedBusinessConditions.Add(businessConditionStatus.Name);
                        }
                        else if ((businessConditionStatus.Status == ValidityStateValue.Unknown || businessConditionStatus.Status == ValidityStateValue.NotChecked) && !unknownBusinessConditions.Contains(businessConditionStatus.Name))
                        {
                            unknownBusinessConditions.Add(businessConditionStatus.Name);
                        }

                        #endregion

                        if (businessConditionStatus.Status == ValidityStateValue.InValid)
                        {
                            MDMRuleMapDetailCollection rules = null;

                            if(ruleMaps != null)
                            {
                                rules = ruleMaps.GetBusinessRulesByBusinessConditionName(businessConditionStatus.Name);
                            }

                            if (rules != null && rules.Count > 0)
                            {
                                foreach(var rule in rules)
                                {
                                    if(rule.MDMRule == null)
                                    {
                                        continue;
                                    }
                                    
                                    var filteredStateValidations = stateValidations.Where(vs => vs.RuleName.Equals(rule.MDMRule.Name));

                                    if (filteredStateValidations != null && filteredStateValidations.Any())
                                    {
                                        foreach (var stateValidation in filteredStateValidations)
                                        {
                                            if (stateValidation.OperationResultType == OperationResultType.Error || stateValidation.OperationResultType == OperationResultType.Warning)
                                            {
                                                #region Validation states valid / invalid

                                                String errorMessage = stateValidation.MessageCode;

                                                if (!String.IsNullOrWhiteSpace(errorMessage))
                                                {
                                                    if (stateValidation.OperationResultType == OperationResultType.Error && !errorCodesForTypeError.Contains(errorMessage))
                                                    {
                                                        errorCodesForTypeError.Add(errorMessage);
                                                    }
                                                    else if (stateValidation.OperationResultType == OperationResultType.Warning && !errorCodesForTypeWarning.Contains(errorMessage))
                                                    {
                                                        errorCodesForTypeWarning.Add(errorMessage);
                                                    }
                                                }

                                                #endregion

                                                #region attribute with error / warning

                                                var attributeLongName = stateValidation.AttributeLongName;

                                                if (!String.IsNullOrWhiteSpace(attributeLongName))
                                                {
                                                    if (stateValidation.OperationResultType == OperationResultType.Error && !attributesWithErrors.Contains(attributeLongName))
                                                    {
                                                        attributesWithErrors.Add(attributeLongName);
                                                    }
                                                    else if (stateValidation.OperationResultType == OperationResultType.Warning && !attributesWithWarnings.Contains(attributeLongName))
                                                    {
                                                        attributesWithWarnings.Add(attributeLongName);
                                                    }
                                                }

                                                #endregion

                                                #region reason types

                                                var reasonType = stateValidation.ReasonType;

                                                if (reasonType != ReasonType.Unknown)
                                                {
                                                    var resonTypeAsString = reasonType.ToString();

                                                    if (!reasonTypes.Contains(resonTypeAsString))
                                                    {
                                                        reasonTypes.Add(resonTypeAsString);
                                                    }
                                                }

                                                #endregion

                                                #region attributes error desc list

                                                String attributeErrorDesc = String.Format("{0} - {1}", attributeLongName, errorMessage);

                                                if (!attributesErrorCodeList.Contains(attributeErrorDesc))
                                                {
                                                    attributesErrorCodeList.Add(attributeErrorDesc);
                                                }

                                                #endregion
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            var businessConditionsSummary = new BusinessConditionsSummary
            {
                PassedBusinessConditions = passedBusinessConditions,
                FailedBusinessConditions = failedBusinessConditions,
                UnknownBusinessConditions = unknownBusinessConditions,
                ErrorCodesForTypeError = errorCodesForTypeError,
                ErrorCodesForTypeWarning = errorCodesForTypeWarning,
                AttributesWithErrors = attributesWithErrors,
                AttributesWithWarnings = attributesWithWarnings,
                AttributesErrorCodeList = attributesErrorCodeList,
                ReasonTypes = reasonTypes
            };

            return businessConditionsSummary;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workflowDataContext"></param>
        /// <param name="workflowActionContext"></param>
        /// <returns></returns>
        private DTO.WorkflowInfo CreateWorkflow(WorkflowStateCollection workflowStates)
        {
            Dictionary<String, DTO.Workflow> workflows = new Dictionary<String, DTO.Workflow>();

            var currentWorkflows = new Collection<String>();
            var currentStages = new Collection<String>();

            if(workflowStates != null && workflowStates.Count > 0)
            {
                foreach(WorkflowState workflowState in workflowStates)
                {
                    if(workflowState == null)
                    {
                        continue;
                    }

                    DTO.Workflow workflow = null;
                    
                    if(workflows.ContainsKey(workflowState.WorkflowName))
                    {
                        workflow = workflows[workflowState.WorkflowName];
                    }
                    else
                    {
                        workflow = new DTO.Workflow { WorkflowName = workflowState.WorkflowName, WorkflowVersion = workflowState.WorkflowVersionName };
                        workflows.Add(workflowState.WorkflowName, workflow);

                        if(!currentWorkflows.Contains(workflowState.WorkflowName) && !workflowState.ActivityLongName.Equals(JigsawConstants.DummyWorkflowActivityName))
                        {
                            currentWorkflows.Add(workflowState.WorkflowName);
                        }
                    }

                    if (workflow != null && !workflowState.ActivityLongName.Equals(JigsawConstants.DummyWorkflowActivityName))
                    {
                        var workflowStage = new DTO.WorkflowStage
                        {
                            StageName = workflowState.ActivityLongName,
                            AssignedUser = workflowState.AssignedUser,
                            AssignedRole = workflowState.AssignedRole,
                            StageEnteredTime = ValueTypeHelper.DateTimeTryParse(workflowState.EventDate, DateTime.Now, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal).ToString("O")
                        };

                        workflow.WorkflowStages.Add(workflowStage);

                        if(!currentStages.Contains(workflowStage.StageName))
                        {
                            currentStages.Add(workflowStage.StageName);
                        }
                    }
                }
            }

            return new DTO.WorkflowInfo
            {
                Workflows = workflows.Values.ToList(),
                CurrentWorkflows = currentWorkflows,
                CurrentStages = currentStages
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private Collection<DTO.ValidationState> CreateValidationStates(Entity entity)
        {
            var validationStates = new Collection<DTO.ValidationState>();

            validationStates.Add(new DTO.ValidationState { Name = "IsSelfValid", Value = entity.ValidationStates.IsSelfValid == ValidityStateValue.Valid ? true : false });
            validationStates.Add(new DTO.ValidationState { Name = "IsMetaDataValid", Value = entity.ValidationStates.IsMetaDataValid == ValidityStateValue.Valid ? true : false });
            validationStates.Add(new DTO.ValidationState { Name = "IsCommonAttributesValid", Value = entity.ValidationStates.IsCommonAttributesValid == ValidityStateValue.Valid ? true : false });
            validationStates.Add(new DTO.ValidationState { Name = "IsCategoryAttributesValid", Value = entity.ValidationStates.IsCategoryAttributesValid == ValidityStateValue.Valid ? true : false });
            validationStates.Add(new DTO.ValidationState { Name = "IsRelationshipsValid", Value = entity.ValidationStates.IsRelationshipsValid == ValidityStateValue.Valid ? true : false });
            validationStates.Add(new DTO.ValidationState { Name = "IsEntityVariantValid", Value = entity.ValidationStates.IsEntityVariantValid == ValidityStateValue.Valid ? true : false });
            validationStates.Add(new DTO.ValidationState { Name = "IsExtensionsValid", Value = entity.ValidationStates.IsExtensionsValid == ValidityStateValue.Valid ? true : false });

            return validationStates;
        }

        #endregion
    }
}
