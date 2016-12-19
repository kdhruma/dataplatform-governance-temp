using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MDM.Utility
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// This is Utility class for Attribute Model.
    /// /// </summary>
    public static class AttributeModelUtility
    {
        /// <summary>
        /// Fills sqldata record with values in all columns like attribute shortname, longname, and other attributemetadata information
        /// </summary>
        /// <param name="attributeModelRecord">The attribute model record.</param>
        /// <param name="attributeModel">The attribute model.</param>
        /// <returns></returns>
        public static SqlDataRecord FillAttributeModelSqlDataRecord(SqlDataRecord attributeModelRecord, AttributeModel attributeModel)
        {
            attributeModelRecord.SetValue(0, attributeModel.Id);
            attributeModelRecord.SetValue(1, (attributeModel.Name ?? String.Empty).Trim());
            attributeModelRecord.SetValue(2, (attributeModel.LongName ?? String.Empty).Trim());
            attributeModelRecord.SetValue(3, attributeModel.AttributeParentId);
            attributeModelRecord.SetValue(4, attributeModel.AttributeParentName);
            attributeModelRecord.SetValue(5, attributeModel.AttributeParentLongName);
            attributeModelRecord.SetValue(6, attributeModel.AttributeTypeId);
            attributeModelRecord.SetValue(7, attributeModel.AttributeTypeName);
            attributeModelRecord.SetValue(8, attributeModel.AttributeDataTypeId);
            attributeModelRecord.SetValue(9, attributeModel.AttributeDataTypeName);
            attributeModelRecord.SetValue(10, attributeModel.AttributeDisplayTypeId);
            attributeModelRecord.SetValue(11, attributeModel.AttributeDisplayTypeName);
            attributeModelRecord.SetValue(12, (Int32)attributeModel.Locale);
            attributeModelRecord.SetValue(13, attributeModel.AllowableValues);
            attributeModelRecord.SetValue(14, attributeModel.MaxLength);
            attributeModelRecord.SetValue(15, attributeModel.MinLength);
            attributeModelRecord.SetValue(16, attributeModel.Required);
            attributeModelRecord.SetValue(17, attributeModel.AllowableUOM);
            attributeModelRecord.SetValue(18, attributeModel.DefaultUOM);
            attributeModelRecord.SetValue(19, attributeModel.UomType);
            attributeModelRecord.SetValue(20, attributeModel.Precision);
            attributeModelRecord.SetValue(21, attributeModel.IsCollection);
            attributeModelRecord.SetValue(22, attributeModel.MinInclusive);
            attributeModelRecord.SetValue(23, attributeModel.MaxInclusive);
            attributeModelRecord.SetValue(24, attributeModel.MinExclusive);
            attributeModelRecord.SetValue(25, attributeModel.MaxExclusive);
            attributeModelRecord.SetValue(26, attributeModel.Label);
            attributeModelRecord.SetValue(27, attributeModel.Definition);
            attributeModelRecord.SetValue(28, attributeModel.BusinessRule);
            attributeModelRecord.SetValue(29, attributeModel.ReadOnly);
            attributeModelRecord.SetValue(30, attributeModel.Extension);
            attributeModelRecord.SetValue(31, attributeModel.AttributeRegEx);
            attributeModelRecord.SetValue(32, attributeModel.LookUpTableName);
            if (!String.IsNullOrWhiteSpace(attributeModel.AttributeDataTypeName) && attributeModel.AttributeDataTypeName.ToLowerInvariant() == AttributeDataType.Boolean.ToString().ToLowerInvariant())
            {
                Boolean? defaultBooleanValue = ValueTypeHelper.ConvertToNullableBoolean(attributeModel.DefaultValue);
                attributeModelRecord.SetValue(33, defaultBooleanValue.HasValue ? defaultBooleanValue.Value.ToString() : null);
            }
            else
            {
                attributeModelRecord.SetValue(33, attributeModel.DefaultValue);
            }
            attributeModelRecord.SetValue(34, attributeModel.ComplexTableName);
            attributeModelRecord.SetValue(35, attributeModel.Path);
            attributeModelRecord.SetValue(36, attributeModel.Searchable);
            attributeModelRecord.SetValue(38, attributeModel.EnableHistory);
            attributeModelRecord.SetValue(39, attributeModel.ShowAtCreation);
            attributeModelRecord.SetValue(40, attributeModel.WebUri);
            attributeModelRecord.SetValue(41, attributeModel.LkSortOrder);
            attributeModelRecord.SetValue(42, attributeModel.LkSearchColumns);
            attributeModelRecord.SetValue(43, attributeModel.LkDisplayColumns);
            attributeModelRecord.SetValue(44, attributeModel.LkDisplayFormat);
            attributeModelRecord.SetValue(45, attributeModel.SortOrder);
            attributeModelRecord.SetValue(46, attributeModel.ExportMask);
            attributeModelRecord.SetValue(47, attributeModel.Inheritable);
            attributeModelRecord.SetValue(48, attributeModel.IsHidden);
            attributeModelRecord.SetValue(49, attributeModel.IsComplex);
            attributeModelRecord.SetValue(50, attributeModel.IsLookup);
            attributeModelRecord.SetValue(51, attributeModel.IsLocalizable);
            attributeModelRecord.SetValue(52, attributeModel.ApplyLocaleFormat);
            attributeModelRecord.SetValue(53, attributeModel.ApplyTimeZoneConversion);
            attributeModelRecord.SetValue(54, attributeModel.AllowNullSearch);
            attributeModelRecord.SetValue(55, attributeModel.AttributeExample);
            attributeModelRecord.SetValue(56, attributeModel.IsPrecisionArbitrary);
            attributeModelRecord.SetValue(57, attributeModel.Action.ToString());
            attributeModelRecord.SetValue(58, attributeModel.RegExErrorMessage.ToString());

            return attributeModelRecord;
        }


        /// <summary>
        /// Converts the attribute model to SQL data record.
        /// </summary>
        /// <param name="attributeMasterMetaData">Indicates attribute master meta data.</param>
        /// <param name="attributeMasterTable">Indicates attribute master table.</param>
        /// <param name="attributeModel">Indicates attribute model.</param>
        /// <param name="uniqueAttributeIds">Indicates collection of unique attribute ids that have been seen so far</param>
        public static void ConvertAttributeModelToSqlDataRecord(SqlMetaData[] attributeMasterMetaData,
                                                                          ref List<SqlDataRecord> attributeMasterTable,
                                                                          AttributeModel attributeModel,
                                                                          ref Collection<Int32> uniqueAttributeIds)
        {
            if (attributeModel.IsComplex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information,
                                              "Add sql data record for parent complex attribute starting...");

                SqlDataRecord attributeModelRecord = new SqlDataRecord(attributeMasterMetaData);
                SqlDataRecord complexAttributeModelRecord =
                    FillAttributeModelSqlDataRecord(attributeModelRecord, attributeModel);

                if (!uniqueAttributeIds.Contains(attributeModel.Id) || attributeMasterTable.Count == 0)
                {
                    attributeMasterTable.Add(complexAttributeModelRecord);

                    if (!uniqueAttributeIds.Contains(attributeModel.Id))
                        uniqueAttributeIds.Add(attributeModel.Id);
                }

                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information,
                                              "Add sql data record for parent complex attribute completing...");

                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Get complex child attributes starting...");

                AttributeModelCollection ComplexChildren =
                    (AttributeModelCollection)attributeModel.GetChildAttributeModels(); // check with return

                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Get complex child attributes completing...");

                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information,
                                              "Attribute is complex hence generate sql data record for all child attributes starting...");

                foreach (AttributeModel complexChild in ComplexChildren)
                {
                    SqlDataRecord childComplexattributeModelRecord = new SqlDataRecord(attributeMasterMetaData);
                    childComplexattributeModelRecord =
                        FillAttributeModelSqlDataRecord(childComplexattributeModelRecord, complexChild);
                    attributeMasterTable.Add(childComplexattributeModelRecord);
                }

                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information,
                                              "Attribute is complex hence generate sql data record for all child attributes completed...");
            }
            else
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information,
                                              "Add sql data record for simple attribute starting...");

                SqlDataRecord attributeModelRecord = new SqlDataRecord(attributeMasterMetaData);
                attributeModelRecord = AttributeModelUtility.FillAttributeModelSqlDataRecord(attributeModelRecord,
                                                                                             attributeModel);
                if (!uniqueAttributeIds.Contains(attributeModel.Id) || attributeMasterTable.Count == 0)
                {
                    attributeMasterTable.Add(attributeModelRecord);

                    if (!uniqueAttributeIds.Contains(attributeModel.Id))
                        uniqueAttributeIds.Add(attributeModel.Id);
                }

                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information,
                                              "Add sql data record for simple attribute completed...");
            }
        }
        /// <summary>
        /// Removes the complex child attributes from collection. This method does not remove complex child attribute from inner level.
        /// ASSUMPTION : All child complex attributes should come after parent attribute.
        /// Example :  If address is having one child attribute which is City then City should come after Address attribute.
        /// </summary>
        /// <param name="attributeModels">attribute models.</param>
        /// <returns>filtered attribute models.</returns>
        public static AttributeModelCollection FilterComplexChildAttributeModels(AttributeModelCollection attributeModels)
        {
            AttributeModelCollection filteredCollection = new AttributeModelCollection();
            Dictionary<Int32, Int32> complexIdDictionary = new Dictionary<Int32, Int32>();

            foreach (AttributeModel attrModel in attributeModels)
            {
                if (!complexIdDictionary.ContainsKey(attrModel.AttributeParentId))
                {
                    filteredCollection.Add(attrModel, true);

                    if (attrModel.IsComplex)
                    {
                        complexIdDictionary.Add(attrModel.Id, attrModel.Id);
                    }
                }
            }

            return filteredCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeUniqueIdentifier"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static String GetAttributeUniqueIdentifierKey(AttributeUniqueIdentifier attributeUniqueIdentifier, LocaleEnum locale)
        {
            String key = String.Empty;

            if (attributeUniqueIdentifier != null)
            {
                key = String.Concat(attributeUniqueIdentifier.AttributeName, "_", attributeUniqueIdentifier.AttributeGroupName, "_", locale);
            }

            return key;
        }


        /// <summary>
        /// Populates the dependent attribute values.
        /// </summary>
        /// <param name="entity">The entity object.</param>
        /// <param name="attributeModels">The attribute models.</param>
        public static void PopulateDependentAttributeValues(Entity entity, AttributeModelCollection attributeModels)
        {
            DiagnosticActivity diagnosicActivity = new DiagnosticActivity();
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            
            if (traceSettings != null && traceSettings.IsBasicTracingEnabled)
            {
                ExecutionContext executionContext = new ExecutionContext();

                executionContext.CallDataContext = new CallDataContext();
                if (entity != null)
                {
                    executionContext.CallDataContext.EntityIdList.Add(entity.Id);
                    executionContext.CallDataContext.ContainerIdList.Add(entity.ContainerId);
                    executionContext.CallDataContext.LocaleList.Add(entity.Locale);
                }

                if (attributeModels != null)
                {
                    executionContext.CallDataContext.AttributeIdList = attributeModels.GetAttributeIdList();
                }
                executionContext.LegacyMDMTraceSources = new Collection<MDMTraceSource>() { MDMTraceSource.UI };

                diagnosicActivity.Start(executionContext);
            }

            Boolean isAttributeDependencyEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.AttributeDependency.Enabled", false);

            if (isAttributeDependencyEnabled)
            {
                if (entity != null && entity.Attributes != null && attributeModels != null && attributeModels.Count > 0)
                {
                    PopulateAttributeModelDependencyValues(entity, attributeModels);
                }
            }
            else
            {
                if (traceSettings != null && traceSettings.IsBasicTracingEnabled)
                {
                    diagnosicActivity.LogInformation("Skipped populating dependency details as MDMCenter.AttributeDependency.Enabled app config value is set as 'False'");
                }
            }

            if (traceSettings != null && traceSettings.IsBasicTracingEnabled)
            {
                diagnosicActivity.Stop();
            }
        }

        /// <summary>
        /// Gets AttributeModelBaseProperties based on given key
        /// </summary>
        /// <param name="baseAttributeModels">Indicates AttributeModelBaseProperties</param>
        /// <param name="attributeId">Indicates attribute Id</param>
        /// <returns></returns>
        public static AttributeModelBaseProperties GetBaseAttributeModelByKey(Dictionary<Int32, AttributeModelBaseProperties> baseAttributeModels, Int32 attributeId)
        {
            AttributeModelBaseProperties baseAttributeModel = null;

            if (baseAttributeModels != null && baseAttributeModels.ContainsKey(attributeId))
            {
                baseAttributeModel = baseAttributeModels[attributeId];
            }

            return baseAttributeModel;
        }

        /// <summary>
        /// Populates the attribute model dependency values.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="attributeModels">The attribute models.</param>
        private static void PopulateAttributeModelDependencyValues(Entity entity, AttributeModelCollection attributeModels)
        {
            foreach (AttributeModel attributeModel in attributeModels)
            {
                if (attributeModel.IsDependentAttribute && attributeModel.DependentParentAttributes != null && attributeModel.DependentParentAttributes.Count > 0)
                {
                    foreach (DependentAttribute dAttribute in attributeModel.DependentParentAttributes)
                    {
                        PopulateDependentAttributeValue(entity, dAttribute, attributeModel.Locale);
                    }
                }

                if (attributeModel.IsComplex)
                {
                    AttributeModelCollection childAttributeModels = attributeModel.AttributeModels;

                    if (childAttributeModels != null && childAttributeModels.Count > 0)
                    {
                        PopulateAttributeModelDependencyValues(entity, childAttributeModels);
                    }
                }
            }
        }

        /// <summary>
        /// Populates the dependent attribute value.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dependentAttribute">The dependent attribute.</param>
        /// <param name="locale">The locale.</param>
        private static void PopulateDependentAttributeValue(Entity entity, DependentAttribute dependentAttribute, LocaleEnum locale)
        {
            IAttribute iAttr = entity.GetAttribute(dependentAttribute.AttributeId, locale);

            if (iAttr != null)
            {
                dependentAttribute.SetAttributeValue(iAttr);

                if (String.IsNullOrWhiteSpace(dependentAttribute.AttributeValue))
                {
                    //It could be hidden attribute with default value. Since hidden attribute not rendering in page need to get the default value.
                    IAttributeModelCollection models = entity.GetAttributeModels();

                    if (models != null)
                    {
                        IAttributeModel model = models.GetAttributeModel(iAttr.Id, iAttr.Locale);

                        if (model != null && model.IsHidden)
                        {
                            dependentAttribute.SetAttributeValue(model.DefaultValue);
                        }
                    }
                }
            }
        }
    }
}
