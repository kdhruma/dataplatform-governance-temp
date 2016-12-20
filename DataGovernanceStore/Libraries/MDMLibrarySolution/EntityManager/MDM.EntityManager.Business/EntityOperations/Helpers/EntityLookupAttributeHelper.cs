using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace MDM.EntityManager.Business.EntityOperations.Helpers
{
    using BusinessObjects;
    using BusinessObjects.Diagnostics;
    using Core;
    using LookupManager.Business;
    using AttributeModelManager.Business;
    using Utility;

    /// <summary>
    /// 
    /// </summary>
    internal sealed class EntityLookupAttributeHelper
    {
        #region Methods

        /// <summary>
        ///     Populate display format values for lookup attributes contained in the entity collection.
        /// </summary>
        /// <param name="entityCollection"></param>
        /// <param name="fillLookupDisplayValues"></param>
        /// <param name="fillLookupRowWithValue"></param>
        /// <param name="callerContext"></param>
        public static void PopulateLookupValues(EntityCollection entityCollection, Boolean fillLookupDisplayValues, Boolean fillLookupRowWithValue, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Populating lookup display values for attributes..."), MDMTraceSource.EntityGet);

            // Build an AttributeModelCollection representing all lookups specified in the entity attributes and entity relationships's attributes.
            var lookupAttributeModels = GetLookupAttributeModels(entityCollection);

            if (lookupAttributeModels != null && lookupAttributeModels.Count > 0)
            {
                var lookupManager = new LookupBL();
                var applicationContext = new ApplicationContext();

                //TODO:: Currently we haven't identified a scenario where we need to look into each entity locale for populating lookup values.
                //Hence considering first entity's locale. In case if we need to consider all entities locale then this logic has to be corrected...
                applicationContext.Locale = entityCollection.FirstOrDefault().Locale.ToString();    

                foreach (AttributeModel attributeModel in lookupAttributeModels)
                {
                    if (attributeModel == null)
                    {
                        //Ideally, this should never happen but in some multi-threaded usecase, attribute model collection is getting null object as one of the item..
                        //Shoud we throw error here? or write diagnostic..
                        var diagnosticActivity = new DiagnosticActivity();
                        diagnosticActivity.LogError("PopulateLookupValues failed with exception: One of the attribute model object is null inside attribute model collection for following set of entity ids: {" + entityCollection.ToString() + "}");
                        continue;
                    }

                    if (attributeModel.IsComplex && attributeModel.AttributeModels != null)
                    {
                        foreach (AttributeModel childAttributeModel in attributeModel.AttributeModels)
                        {
                            if (childAttributeModel.IsLookup)
                            {
                                FillLookupValues(entityCollection, childAttributeModel, fillLookupDisplayValues, fillLookupRowWithValue, lookupManager, applicationContext, callerContext);
                            }
                        }
                    }
                    else
                    {
                        FillLookupValues(entityCollection, attributeModel, fillLookupDisplayValues, fillLookupRowWithValue, lookupManager, applicationContext, callerContext);
                    }
                }
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Done with populating lookup display values for attributes..."), MDMTraceSource.EntityGet);
        }

        /// <summary>
        ///     Returns the AttributeModelCollection for all lookups available in the entity collection.
        /// </summary>
        /// <param name="entityCollection"></param>
        /// <returns></returns>
        private static AttributeModelCollection GetLookupAttributeModels(EntityCollection entityCollection)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Building AttributeModelCollection for all lookup attributes"), MDMTraceSource.EntityGet);

            var lookupAttributeModels = new AttributeModelCollection();

            var attributeModelManager = new AttributeModelBL();

            var relationshipAttributeModelsUniqueKeys = new HashSet<String>();

            foreach (Entity entity in entityCollection)
            {
                var attributeModels = entity.AttributeModels;

                if (attributeModels != null && attributeModels.Count > 0)
                {
                    foreach (AttributeModel attributeModel in attributeModels)
                    {
                        // Filter only lookups for the requested data locales.
                        if (attributeModel.IsLookup && lookupAttributeModels.GetAttributeModel(attributeModel.Id, attributeModel.Locale) == null)
                        {
                            lookupAttributeModels.Add(attributeModel);
                        }
                        else if (attributeModel.IsComplex && attributeModel.AttributeModels != null)
                        {
                            foreach (var childAttributeModel in attributeModel.AttributeModels)
                            {
                                if (childAttributeModel.IsLookup && 
                                    lookupAttributeModels.GetAttributeModel(childAttributeModel.Id, childAttributeModel.Locale) == null)
                                {
                                    lookupAttributeModels.Add(attributeModel);
                                }
                            }
                        }
                    }
                }

                if (entity.Relationships != null && entity.Relationships.Count > 0)
                {
                    var dataLocales = new Collection<LocaleEnum> { entity.Locale };

                    GetRelationshipLookupAttributeModels(entity.Relationships, lookupAttributeModels, attributeModelManager, relationshipAttributeModelsUniqueKeys, dataLocales, (Int32)entity.Locale);
                }
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Completed building AttributeModelCollection for all lookup attributes."), MDMTraceSource.EntityGet);

            return lookupAttributeModels;
        }

        /// <summary>
        /// Gets the relationship lookup attribute models.
        /// </summary>
        /// <param name="relationships">The relationships.</param>
        /// <param name="lookupAttributeModels">The lookup attribute models.</param>
        /// <param name="attributeModelManager">The attribute model manager.</param>
        /// <param name="relationshipAttributeModelsUniqueKeys">The relationship attribute models unique keys.</param>
        /// <param name="dataLocales">The data locales.</param>
        /// <param name="entityLocale">The entity locale.</param>
        private static void GetRelationshipLookupAttributeModels(RelationshipCollection relationships, AttributeModelCollection lookupAttributeModels, AttributeModelBL attributeModelManager, HashSet<String> relationshipAttributeModelsUniqueKeys, Collection<LocaleEnum> dataLocales, Int32 entityLocale)
        {
            foreach (var relationship in relationships)
            {
                var relationshipAttributeModelsUniqueKey = String.Format("CON{0}_RT{1}_L{2}", relationship.ContainerId, relationship.RelationshipTypeId, entityLocale);

                if (!relationshipAttributeModelsUniqueKeys.Contains(relationshipAttributeModelsUniqueKey))
                {
                    var attributeModelContext = new AttributeModelContext
                    {
                        AttributeModelType = AttributeModelType.Relationship,
                        RelationshipTypeId = relationship.RelationshipTypeId,
                        ContainerId = relationship.ContainerId,
                        Locales = dataLocales
                    };

                    var relationshipAttributeModels = attributeModelManager.Get(attributeModelContext);

                    if (relationshipAttributeModels != null && relationshipAttributeModels.Count > 0)
                    {
                        foreach (AttributeModel attributeModel in relationshipAttributeModels)
                        {
                            if (attributeModel.IsLookup && lookupAttributeModels.GetAttributeModel(attributeModel.Id, attributeModel.Locale) == null)
                            {
                                lookupAttributeModels.Add(attributeModel);
                            }
                        }
                    }

                    relationshipAttributeModelsUniqueKeys.Add(relationshipAttributeModelsUniqueKey);
                }

                if (relationship.RelationshipCollection != null && relationship.RelationshipCollection.Count > 0)
                {
                    GetRelationshipLookupAttributeModels(relationship.RelationshipCollection, lookupAttributeModels, attributeModelManager, relationshipAttributeModelsUniqueKeys, dataLocales, entityLocale);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityCollection"></param>
        /// <param name="attributeModel"></param>
        /// <param name="loadLookupDisplayValues"></param>
        /// <param name="loadLookupRowWithValue"></param>
        /// <param name="lookupManager"></param>
        /// <param name="applicationContext"></param>
        /// <param name="callerContext"></param>
        private static void FillLookupValues(EntityCollection entityCollection, AttributeModel attributeModel, Boolean loadLookupDisplayValues, Boolean loadLookupRowWithValue, LookupBL lookupManager, ApplicationContext applicationContext, CallerContext callerContext)
        {
            Int32 attributeId = attributeModel.Id;
            Int32 childAttributeId = -1;
            Int32 attributeIdForLookupGet = attributeId;

            //Identify locale for lookup values...
            var locale = attributeModel.Locale;

            if (attributeModel.IsComplexChild)
            {
                childAttributeId = attributeModel.Id;
                attributeId = attributeModel.AttributeParentId;
                attributeIdForLookupGet = childAttributeId;
            }

            // Build a dictionary of reference id to value collection for the attribute id and locale.
            var refIdToValuesDictionary = GetRefIdToValuesDictionaryForEntities(entityCollection, attributeModel.AttributeModelType, attributeId, locale, childAttributeId);

            if (refIdToValuesDictionary.Count > 0)
            {
                var valueRefIds = new Collection<Int32>(refIdToValuesDictionary.Keys.ToList());

                //Display values has to be fetched as per the current application context locale.. Hence considering application context locale.
                ValueTypeHelper.EnumTryParse<LocaleEnum>(applicationContext.Locale, true, out locale);

                //Get lookup record for thus obtained value ref Ids
                Lookup lookup = lookupManager.Get(attributeIdForLookupGet, locale, 0, valueRefIds, applicationContext, callerContext, false);

                if (lookup != null && lookup.Rows.Count > 0)
                {
                    // Fill the Value collection with the lookup display values.
                    FillValuesCollectionWithDisplayValues(refIdToValuesDictionary, lookup, loadLookupDisplayValues, loadLookupRowWithValue);
                }
            }
        }

        /// <summary>
        ///     Returns a dictionary of reference id to values for the specified entities.
        /// </summary>
        /// <param name="entityCollection"></param>
        /// <param name="attributeModelType"></param>
        /// <param name="attributeId"></param>
        /// <param name="locale"></param>
        /// <param name="childAttributeId"></param>
        /// <returns></returns>
        private static Dictionary<Int32, ValueCollection> GetRefIdToValuesDictionaryForEntities(EntityCollection entityCollection, AttributeModelType attributeModelType, Int32 attributeId, LocaleEnum locale, Int32 childAttributeId)
        {
            //Dictionary of valueRefId to their Values maintained in order to efficiently put display values 
            var refIdToValuesDictionary = new Dictionary<Int32, ValueCollection>();

            //Collect Value Ref Ids for all entities
            foreach (Entity entity in entityCollection)
            {
                if (attributeModelType == AttributeModelType.Relationship)
                {
                    #region Relationship attributes

                    if (entity.Relationships != null && entity.Relationships.Count > 0)
                    {
                        GetRefIdToValuesDictionaryForRelationships(entity.Relationships, attributeId, locale, refIdToValuesDictionary);
                    }

                    #endregion
                }
                else
                {
                    #region Entity attributes

                    var attribute = (Attribute) entity.GetAttribute(attributeId, locale);

                    if (attribute != null)
                    {
                        if (attribute.IsComplex && attribute.Attributes != null && childAttributeId > 0)
                        {
                            foreach (Attribute instanceAttribute in attribute.Attributes)
                            {
                                if (instanceAttribute.Attributes != null)
                                {
                                    foreach (Attribute childAttribute in instanceAttribute.Attributes)
                                    {
                                        if (childAttribute.Id == childAttributeId)
                                        {
                                            PopulateValueCollectionInDictionary((ValueCollection) childAttribute.GetInheritedValues(), refIdToValuesDictionary);
                                            PopulateValueCollectionInDictionary((ValueCollection) childAttribute.GetOverriddenValues(), refIdToValuesDictionary);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            PopulateValueCollectionInDictionary((ValueCollection) attribute.GetInheritedValues(), refIdToValuesDictionary);
                            PopulateValueCollectionInDictionary((ValueCollection) attribute.GetOverriddenValues(), refIdToValuesDictionary);
                        }
                    }

                    #endregion
                }
            }

            return refIdToValuesDictionary;
        }

        /// <summary>
        /// Gets the reference identifier to values dictionary for relationships.
        /// </summary>
        /// <param name="relationships">The relationships.</param>
        /// <param name="attributeId">The attribute identifier.</param>
        /// <param name="locale">The locale.</param>
        /// <param name="refIdToValuesDictionary">The reference identifier to values dictionary.</param>
        private static void GetRefIdToValuesDictionaryForRelationships(RelationshipCollection relationships, Int32 attributeId, LocaleEnum locale, Dictionary<Int32, ValueCollection> refIdToValuesDictionary )
        {
            foreach (var relationship in relationships)
            {
                var relationshipAttribute = relationship.RelationshipAttributes.GetAttribute(attributeId, locale);

                if (relationshipAttribute != null)
                {
                    PopulateValueCollectionInDictionary((ValueCollection)relationshipAttribute.GetInheritedValues(), refIdToValuesDictionary);
                    PopulateValueCollectionInDictionary((ValueCollection)relationshipAttribute.GetOverriddenValues(), refIdToValuesDictionary);
                }

                if (relationship.RelationshipCollection != null && relationship.RelationshipCollection.Count > 0)
                {
                    GetRefIdToValuesDictionaryForRelationships(relationship.RelationshipCollection, attributeId, locale, refIdToValuesDictionary);
                }
            }
        }

        /// <summary>
        ///     Populates the Value collection in the dictionary.
        /// </summary>
        /// <param name="valueCollection"></param>
        /// <param name="refIdToValuesDictionary"></param>
        private static void PopulateValueCollectionInDictionary(ValueCollection valueCollection, Dictionary<Int32, ValueCollection> refIdToValuesDictionary)
        {
            if (valueCollection != null && valueCollection.Count > 0)
            {
                foreach (Value val in valueCollection)
                {
                    // If val.valueRefId is less than 0, then only check for the attrVal.
                    // Otherwise, always val.valueRefId will be given a priority.
                    Int32 valueRefId = val.ValueRefId > 0 ? val.ValueRefId : ValueTypeHelper.Int32TryParse(val.GetStringValue(), val.ValueRefId);

                    if (valueRefId > 0)
                    {
                        val.ValueRefId = valueRefId;

                        ValueCollection valueObjectsForLookupRefId;
                        if (refIdToValuesDictionary.ContainsKey(valueRefId))
                        {
                            //This value ref Id is already present in dictionary..
                            //Update value field(ValueCollection) for this key with the current attribute value
                            valueObjectsForLookupRefId = refIdToValuesDictionary[valueRefId];
                            valueObjectsForLookupRefId.Add(val);
                        }
                        else
                        {
                            //This value ref Id is not available in dictionary..
                            //Add new entry in dictionary
                            valueObjectsForLookupRefId = new ValueCollection { val };
                            refIdToValuesDictionary.Add(valueRefId, valueObjectsForLookupRefId);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Fills the ValueCollection with the look up display values
        /// </summary>
        /// <param name="refIdToValuesDictionary"></param>
        /// <param name="lookup"></param>
        /// <param name="loadLookupDisplayValue"></param>
        /// <param name="loadLookupRowWithValue"></param>
        private static void FillValuesCollectionWithDisplayValues(Dictionary<Int32, ValueCollection> refIdToValuesDictionary, Lookup lookup, Boolean loadLookupDisplayValue, Boolean loadLookupRowWithValue)
        {
            foreach (var refIdToValuesPair in refIdToValuesDictionary)
            {
                Int32 lookupId = refIdToValuesPair.Key;
                String displayValue = lookup.GetDisplayFormatById(lookupId);
                String exportValue = lookup.GetExportFormatById(lookupId);
                Row lookupRow = (Row)lookup.GetRecordById(lookupId);

                //Populate all value objects having this ref Id
                foreach (Value value in refIdToValuesPair.Value)
                {
                    if (loadLookupDisplayValue)
                    {
                        value.InvariantVal = value.ValueRefId;
                        value.AttrVal = value.ValueRefId;
                        
                        if (!String.IsNullOrWhiteSpace(displayValue))
                            value.SetDisplayValue(displayValue);

                        if (!String.IsNullOrWhiteSpace(exportValue))
                            value.SetExportValue(exportValue);
                    }

                    if (loadLookupRowWithValue && lookupRow != null)
                    {
                        var lookupRowDetails = new Dictionary<String, String>();

                        if (lookupRow.Cells != null)
                        {
                            foreach (Cell lookupDataCell in lookupRow.Cells)
                            {
                                lookupRowDetails.Add(lookupDataCell.ColumnName, lookupDataCell.Value != null ? lookupDataCell.Value.ToString() : String.Empty);
                            }
                        }

                        value.ExtendedValues = lookupRowDetails;
                    }
                }
            }
        }

        #endregion
    }
}
