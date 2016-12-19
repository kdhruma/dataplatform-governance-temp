using System;
using System.Collections.Generic;
using System.Linq;

namespace MDM.EntityManager.Business.EntityOperations.Helpers
{
    using BusinessObjects;
    using Core;
    using Interfaces;
    using Utility;
    using RelationshipManager.Business;

    /// <summary>
    /// 
    /// </summary>
    internal sealed class EntityCompareAndMergeHelper
    {
        #region Methods

        /// <summary>
        ///     Compare new values with existing values, find deltas and figure out final values and actions for process
        /// </summary>
        /// <param name="entities">Field denoting the current Entity.</param>
        /// <param name="entityOperationResults"></param>
        /// <param name="entityProcessingOptions">Field denoting the Entity Processing option</param>
        /// <param name="callerContext">Field denoting the caller context</param>
        public static void CompareAndMerge(EntityCollection entities, EntityOperationResultCollection entityOperationResults, EntityProcessingOptions entityProcessingOptions, CallerContext callerContext)
        {
            Boolean flushExistingValues = entityProcessingOptions.CollectionProcessingType != CollectionProcessingType.Merge;

            #region Create comparison options

            StringComparison stringComparision = StringComparison.InvariantCulture;
            entityProcessingOptions.IgnoreCase = !AppConfigurationHelper.GetAppConfig("MDMCenter.EntityManager.EntityProcessingOptions.ValueComparison.CaseSensitive.Enabled", true); 

            if (entityProcessingOptions.IgnoreCase)
                stringComparision = StringComparison.InvariantCultureIgnoreCase;

            #endregion

            var relationshipManager = new RelationshipBL();

            Int32 relationshipCreateId = -1;

            foreach (Entity entity in entities)
            {
                #region Entity attributes

                if (entity.Action != ObjectAction.Create && entity.Action != ObjectAction.Delete && entity.OriginalEntity != null)
                {
                    //Filter only collection Attributes from the current Entity
                    var origAttributes = (AttributeCollection)entity.OriginalEntity.GetAttributes();
                    var mergedAttributes = new AttributeCollection();

                    if (origAttributes != null && origAttributes.Count > 0)
                    {
                        //If collection attribute is present in the current Entity
                        foreach (Attribute deltaAttribute in entity.Attributes)
                        {
                            if (deltaAttribute.Action == ObjectAction.Ignore)
                            {
                                mergedAttributes.Add(deltaAttribute);
                            }
                            else
                            {
                                IAttribute origAttribute = entity.OriginalEntity.GetAttribute(deltaAttribute.Id, deltaAttribute.Locale);

                                if (origAttribute != null)
                                {
                                    if (deltaAttribute.IsComplex == false)
                                    {
                                        Attribute mergedAttribute = (Attribute)origAttribute.MergeDelta(deltaAttribute, flushExistingValues, stringComparision, entityProcessingOptions.AttributeCompareAndMergeBehavior, callerContext);
                                        if (mergedAttribute != null)
                                        {
                                            if (mergedAttribute.IsCollection && mergedAttribute.Action != ObjectAction.Delete && mergedAttribute.SourceFlag == AttributeValueSource.Overridden)
                                            {
                                                foreach (IValue item in mergedAttribute.GetCurrentValues())
                                                {
                                                    //if all actions are read then do not add the attribute top collection
                                                    if (item.Action != ObjectAction.Read)
                                                    {
                                                        //if any action is delete or create, then change all the reads to update add add it to collection
                                                        foreach (Value finalItem in mergedAttribute.GetCurrentValues())
                                                        {
                                                            if (finalItem.Action == ObjectAction.Read)
                                                                finalItem.Action = ObjectAction.Update;
                                                        }
                                                        mergedAttributes.Add(mergedAttribute);
                                                        break;
                                                    }
                                                    if (item.Action == ObjectAction.Read)
                                                    {
                                                        continue;
                                                    }
                                                }

                                                FixCollectionMerge((Attribute)origAttribute, mergedAttribute, entityProcessingOptions.AttributeCompareAndMergeBehavior, false);
                                            }
                                            else
                                            {
                                                mergedAttributes.Add(mergedAttribute);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // first see if the incoming value is same as already existing values
                                        FixComplexAttributeMerge((Attribute)origAttribute, deltaAttribute, entityProcessingOptions.AttributeCompareAndMergeBehavior);

                                        //For complex attribute, since we don't calculate the action using MergeDelta, we are depending on incoming action.
                                        //Here doing parent attribute action correction based on complex child attribute.
                                        //Also need to calculate sequence if we are deleting any instance record after re-calculating the parent attribute action.
                                        if (deltaAttribute.Action != ObjectAction.Read)
                                        {
                                            UpdateComplexAttributeActionAndSequence(deltaAttribute);
                                        }

                                        //Considering complex attributes even though attribute action is set to Read in order to keep the behavior same as of simple attributes.
                                        //All attributes with action Read are going to be ignored before sending for DB processing.
                                        mergedAttributes.Add(deltaAttribute);
                                    }
                                }
                            }
                        }

                        entity.Attributes = mergedAttributes;
                    }
                }
                else if (entity.Action == ObjectAction.Create)
                {
                    IEnumerable<IAttribute> deltaAttrs = entity.GetAttributes();

                    foreach (var attribute in deltaAttrs)
                    {
                        var attr = (BusinessObjects.Attribute)attribute;
                        if (attr != null && (attr.Action != ObjectAction.Read && attr.Action != ObjectAction.Ignore && attr.Action != ObjectAction.Delete))
                        {
                            attr.Action = ObjectAction.Create;

                            Decimal seq = -1;

                            if (attr.IsCollection)
                                seq = 0;

                            foreach (Value val in attr.GetCurrentValuesInvariant())
                            {
                                val.Action = ObjectAction.Create;

                                //Correct the sequence always, in case of create. As for Create, MergeDelta will not be called. 
                                //So if some values are added from BR, and BR is not populating correct sequence, wrong sequence will be saved.
                                val.Sequence = seq++;
                            }
                        }
                    }
                }

                #endregion

                #region Entity Relationships

                if (entity.Action != ObjectAction.Delete
                    && entity.Relationships != null
                    && entity.Relationships.Count > 0)
                {
                    //Get the entity operation result
                    var entityOperationResult = entityOperationResults.GetEntityOperationResult(entity.Id) ?? new EntityOperationResult(entity.Id, entity.LongName);

                    relationshipManager.CompareAndMerge(entity, entityOperationResult, callerContext, ref relationshipCreateId, stringComparision, entityProcessingOptions.ProcessingMode);
                }

                #endregion
            }
        }

        /// <summary>
        /// Updates Complex attribute's Action and Sequence based on Action value in the incoming attribute.
        /// </summary>
        /// <param name="deltaAttribute"></param>
        private static void UpdateComplexAttributeActionAndSequence(Attribute deltaAttribute)
        {
            Int32 totalComplexInstanceAttributes = deltaAttribute.GetChildAttributes().Count;

            Int32 deletedComplexInstanceAttributes = 0;
            Decimal sequence = -1;
            if (deltaAttribute.IsCollection == true)
            {
                sequence = 0;
            }

            foreach (Value complexParentValue in deltaAttribute.OverriddenValues)
            {
                IAttribute complexInstanceAttr = deltaAttribute.GetComplexAttributeInstanceByInstanceRefId(complexParentValue.ValueRefId);

                if (complexInstanceAttr != null)
                {
                    //For each attribute object set the action as DB does a flush and fill of complex attributes.
                    if (complexInstanceAttr.Action == ObjectAction.Read)
                    {
                        complexInstanceAttr.Action = deltaAttribute.Action;
                    }

                    Int32 totalComplexChildAttributes = complexInstanceAttr.GetChildAttributes().Count;

                    Int32 deletedComplexChildAttributes = 0;

                    foreach (Attribute complexChildAttr in complexInstanceAttr.GetChildAttributes())
                    {
                        if (complexInstanceAttr.Action == ObjectAction.Delete || complexChildAttr.Action == ObjectAction.Delete || !complexChildAttr.HasValue())
                        {
                            complexChildAttr.Action = ObjectAction.Delete;
                            deletedComplexChildAttributes++;
                        }
                        else if (complexChildAttr.Action == ObjectAction.Read)
                        {
                            complexChildAttr.Action = complexInstanceAttr.Action;
                        }
                        if (complexChildAttr.IsHierarchical)
                        {
                            UpdateComplexAttributeActionAndSequence(complexChildAttr);
                        }
                    }

                    if (deletedComplexChildAttributes == totalComplexChildAttributes)
                    {
                        complexInstanceAttr.Action = ObjectAction.Delete;
                        complexParentValue.Action = ObjectAction.Delete;
                        deletedComplexInstanceAttributes++;
                        if (deltaAttribute.Attributes.Count > 1)
                        {
                            deltaAttribute.Attributes.Remove(complexInstanceAttr);
                        }
                    }
                    else
                    {
                        complexInstanceAttr.Sequence = sequence;
                        complexParentValue.Sequence = sequence;
                        sequence++;
                    }
                }
            }

            if (deletedComplexInstanceAttributes == totalComplexInstanceAttributes)
            {
                deltaAttribute.Action = ObjectAction.Delete;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origAttribute"></param>
        /// <param name="mergedAttribute"></param>
        /// <param name="attributeCompareAndMergeBehavior"></param>
        /// <returns></returns>
        private static Boolean FixComplexAttributeMerge(Attribute origAttribute, Attribute mergedAttribute, AttributeCompareAndMergeBehavior attributeCompareAndMergeBehavior)
        {
            Boolean isComplexAttributeChanged = IsComplexAttributeChanged(origAttribute, mergedAttribute, attributeCompareAndMergeBehavior);

            if (!isComplexAttributeChanged)
            {
                mergedAttribute.Action = ObjectAction.Read;
            }

            return !isComplexAttributeChanged;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origAttribute"></param>
        /// <param name="mergedAttribute"></param>
        /// <param name="attributeCompareAndMergeBehavior"></param>
        /// <returns></returns>
        private static Boolean IsComplexAttributeChanged(Attribute origAttribute, Attribute mergedAttribute, AttributeCompareAndMergeBehavior attributeCompareAndMergeBehavior)
        {
            if (origAttribute.IsComplex && mergedAttribute.IsComplex)
            {
                if (mergedAttribute.Action == ObjectAction.Delete)
                {
                    return true;
                }

                IAttributeCollection origAttributeRecordInstances = origAttribute.Attributes ?? new AttributeCollection();
                IAttributeCollection mergedAttributeRecordInstances = mergedAttribute.Attributes ?? new AttributeCollection();

                if (origAttributeRecordInstances.Count == 0 && mergedAttributeRecordInstances.Count == 0)
                {
                    return false;
                }

                if (origAttributeRecordInstances.Count > 0 && mergedAttributeRecordInstances.Count == 0)
                {
                    return true;
                }

                if (origAttributeRecordInstances.Count == 0 && mergedAttributeRecordInstances.Count > 0)
                {
                    return true;
                }

                var totalInstancesCount = origAttributeRecordInstances.Count;

                if (totalInstancesCount > 0 && origAttributeRecordInstances.Count != mergedAttributeRecordInstances.Count)
                {
                    return true;
                }

                for (Int32 currentInstanceCounter = 0; currentInstanceCounter < totalInstancesCount; currentInstanceCounter++)
                {
                    var origAttributeRecordInstance = origAttributeRecordInstances.ElementAt(currentInstanceCounter);
                    var mergedAttributeRecordInstance = mergedAttributeRecordInstances.ElementAt(currentInstanceCounter);

                    if (origAttributeRecordInstance != null && mergedAttributeRecordInstance == null)
                    {
                        return true;
                    }

                    if (mergedAttributeRecordInstance != null && origAttributeRecordInstance == null)
                    {
                        return true;
                    }

                    if (origAttributeRecordInstance != null && mergedAttributeRecordInstance != null)
                    {
                        if (mergedAttributeRecordInstance.Action == ObjectAction.Delete)
                        {
                            return true;
                        }

                        if (mergedAttributeRecordInstance.SourceFlag != origAttributeRecordInstance.SourceFlag)
                        {
                            if (origAttributeRecordInstance.SourceFlag == AttributeValueSource.Inherited && mergedAttributeRecordInstance.SourceFlag == AttributeValueSource.Overridden && attributeCompareAndMergeBehavior == AttributeCompareAndMergeBehavior.CompareOverriddenValuesOnly)
                            {
                                return true;
                            }
                        }

                        var origChildAttributes = origAttributeRecordInstance.Attributes;
                        var mergedChildAttributes = mergedAttributeRecordInstance.Attributes;

                        if (origChildAttributes != null && origChildAttributes.Count > 0 && mergedChildAttributes == null)
                        {
                            return true;
                        }

                        if (origChildAttributes == null && mergedChildAttributes != null && mergedChildAttributes.Count > 0)
                        {
                            return true;
                        }

                        if (origChildAttributes != null && mergedChildAttributes != null)
                        {
                            if (origChildAttributes.Count > 0 && origChildAttributes.Count != mergedChildAttributes.Count)
                            {
                                return true;
                            }

                            IList<Attribute> origChildAttributesOrdered = origChildAttributes.OrderBy(attr => attr.Id).ToList();
                            IList<Attribute> mergedChildAttributesOrdered = mergedChildAttributes.OrderBy(attr => attr.Id).ToList();

                            for (int i = 0; i < origChildAttributesOrdered.Count; i++)
                            {
                                var origChildAttribute = origChildAttributesOrdered[i];
                                var mergedChildAttribute = mergedChildAttributesOrdered[i];

                                if (origChildAttribute != null && mergedChildAttribute != null)
                                {
                                    if (origChildAttribute.Id != mergedChildAttribute.Id)
                                    {
                                        return true;
                                    }

                                    if (origChildAttribute.Locale != mergedChildAttribute.Locale)
                                    {
                                        return true;
                                    }

                                    if (!FixCollectionMerge(origChildAttribute, mergedChildAttribute, attributeCompareAndMergeBehavior, true))
                                    {
                                        return true;
                                    }
                                    if (mergedChildAttribute.IsHierarchical)
                                    {
                                        if (IsComplexAttributeChanged(origChildAttribute, mergedChildAttribute, attributeCompareAndMergeBehavior))
                                            return true;
                                        else
                                            continue;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attribute's MergeDelta method is always setting action = Update for matched values too and remove
        /// </summary>
        /// <param name="origAttribute">Indicates origin attribute</param>
        /// <param name="mergedAttribute">Indicates merged attribute</param>
        /// <param name="attributeCompareAndMergeBehavior"></param>
        /// <param name="isComplexChildAttribute"></param>
        private static Boolean FixCollectionMerge(Attribute origAttribute, Attribute mergedAttribute, AttributeCompareAndMergeBehavior attributeCompareAndMergeBehavior, Boolean isComplexChildAttribute)
        {
            ValueCollection origValues = (ValueCollection)origAttribute.GetCurrentValuesInvariant();
            ValueCollection mergedValues = (ValueCollection)mergedAttribute.GetCurrentValuesInvariant();

            Boolean isAllValuesExactMatched = false;

            Boolean skipCompare = true;

            if (mergedAttribute.SourceFlag != origAttribute.SourceFlag)
            {
                if (origAttribute.SourceFlag == AttributeValueSource.Inherited && mergedAttribute.SourceFlag == AttributeValueSource.Overridden && attributeCompareAndMergeBehavior == AttributeCompareAndMergeBehavior.CompareOverriddenAndInheritedValues)
                {
                    skipCompare = false;
                }

                if (skipCompare)
                {
                return isAllValuesExactMatched;
            }
            }

            // if either one of them does not have value..then return
            if (origValues == null || mergedValues == null)
            {
                return isAllValuesExactMatched;
            }

            // if the count does not match...return
            if (origValues.Count() != mergedValues.Count())
            {
                return isAllValuesExactMatched;
            }

            if ((isComplexChildAttribute || mergedAttribute.Action == ObjectAction.Update) && origValues != null && mergedValues != null)
            {
                isAllValuesExactMatched = true;

                foreach (Value mergedVal in mergedValues)
                {
                    if (isComplexChildAttribute && mergedAttribute.AttributeDataType == AttributeDataType.Date && origAttribute.HasInvalidValues == false)
                    {
                        foreach (Value origValue in origValues)
                        {
                            if (origValue != null && origValue.InvariantVal != null)
                            {
                                var invariantValString = origValue.InvariantVal.ToString();

                                if (!String.IsNullOrWhiteSpace(invariantValString))
                                {
                                    origValue.InvariantVal = FormatHelper.FormatDateOnly(invariantValString, "en-US");
                                }
                            }
                        }
                    }

                    Int32 matchedValsCount = origValues.Count(v => v.ValueEquals(mergedVal));

                    //If exact value matches..
                    if (!(matchedValsCount == 1 && mergedVal.Action != ObjectAction.Delete))
                    {
                        isAllValuesExactMatched = false;
                        break;
                    }
                }
            }

            // Now that we have confirmed all the values are the same and the action is update, we dont have anything to process. So change the action to read.
            if (isAllValuesExactMatched)
            {
                mergedValues.SetAction(ObjectAction.Read);
                mergedAttribute.Action = ObjectAction.Read;
            }

            return isAllValuesExactMatched;
        }

        #endregion
    }
}
