using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Transactions;

namespace MDM.EntityManager.Business.EntityOperations
{
    using BusinessObjects;
    using Core;
    using Utility;
    using Interfaces;
    using AttributeModelManager.Business;
    using ConfigurationManager.Business;
    using Data;

    /// <summary>
    /// 
    /// </summary>
    internal class EntityReclassificationManager : BusinessLogicBase
    {
        #region Methods

        /// <summary>
        ///     This method reclassified child entities
        /// </summary>
        /// <param name="entities">Collection of Entities to be reclassified</param>
        /// <param name="entityOperationResultCollection">collection of entity operation result</param>
        /// <param name="entityIds">collection of entity Ids</param>
        /// <param name="isCacheEnabled"></param>
        /// <param name="loginUser">indicates logged in user</param>
        /// <param name="programName">indicates program name</param>
        /// <param name="callerContext">indicates caller context</param>
        /// <param name="processingMode">Indicates the mode of processing</param>
        public void ReclassifyChildren(EntityCollection entities, EntityOperationResultCollection entityOperationResultCollection, Collection<Int64> entityIds, Boolean isCacheEnabled, String loginUser, String programName, CallerContext callerContext, ProcessingMode processingMode = ProcessingMode.Sync)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, GetTransactionOptions(processingMode)))
            {
                var entityDA = new EntityDA();

                //Get command
                DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Update);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "About to call entityDA.ReclassifyChildren...", MDMTraceSource.EntityProcess);

                //Call DA Method
                entityDA.ReclassifyChildren(entities, entityOperationResultCollection, entityIds, loginUser, programName, command);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Done with entityDA.ReclassifyChildren", MDMTraceSource.EntityProcess);

                transactionScope.Complete();
            }
        }

        /// <summary>
        ///     Populate target category's technical attributes to Source Entities.
        /// </summary>
        /// <param name="reclassifyEntities">Collection of Entities to be reclassified</param>
        /// <param name="entityProcessingOptions"></param>
        /// <param name="application">Indicates program name</param>
        /// <param name="module">Indicates Module</param>
        /// <returns></returns>
        public void LoadTargetCategoryAttributesForReclasification(EntityCollection reclassifyEntities, EntityProcessingOptions entityProcessingOptions, MDMCenterApplication application, MDMCenterModules module)
        {
            //Get a entity from a collection. 
            //Target Category Attributes. Because tech attributes are mapped to all entities
            Entity entity = reclassifyEntities.FirstOrDefault(e => e.EntityMoveContext != null && e.EntityMoveContext.ReParentType == ReParentTypeEnum.CategoryReParent);

            if (entity != null && entity.Action == ObjectAction.Reclassify)
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Populating target category's technical attributes to Source Entities process Started...", MDMTraceSource.EntityProcess);

                try
                {
                    var attributeModelBL = new AttributeModelBL();

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Get the target category's attribute model for Target Category Id {0}", entity.EntityMoveContext.TargetCategoryId), MDMTraceSource.EntityProcess);

                    var attributeModelContext = new AttributeModelContext();
                    attributeModelContext.ContainerId = entity.ContainerId;
                    attributeModelContext.CategoryId = entity.EntityMoveContext.TargetCategoryId;
                    attributeModelContext.EntityTypeId = entity.EntityTypeId;
                    attributeModelContext.AttributeModelType = AttributeModelType.Category;
                    attributeModelContext.Locales = new Collection<LocaleEnum> { entity.Locale };
                    attributeModelContext.ApplySorting = false;

                    //Get the target category's attribute model.
                    AttributeModelCollection categoryAttributeModels = attributeModelBL.Get(attributeModelContext);

                    if (categoryAttributeModels != null && categoryAttributeModels.Count > 0)
                    {
                        #region Prepare Entity Context

                        var entityContext = new EntityContext();
                        entityContext.CategoryId = entity.EntityMoveContext.TargetCategoryId;
                        entityContext.AttributeIdList = categoryAttributeModels.GetAttributeIdList();
                        entityContext.ContainerId = entity.ContainerId;
                        entityContext.LoadAttributes = true;
                        entityContext.LoadEntityProperties = true;
                        entityContext.AttributeModelType = AttributeModelType.Category;
                        entityContext.LoadHierarchyRelationships = false;

                        #endregion

                        //Make a Entity get call for target category's technical attribute.
                        Entity targetCategory = new EntityBL().Get(entityContext.CategoryId, entityContext, false, application, module, false, false);

                        if (targetCategory != null)
                        {
                            //Get technical attributes from target Category.
                            IAttributeCollection categoryAttributes = targetCategory.GetCategorySpecificAttributes();

                            var categoryAttributesToBeAdded = new AttributeCollection();

                            foreach (AttributeModel model in categoryAttributeModels)
                            {
                                var attribute = (Attribute)categoryAttributes.GetAttribute(model.Id, model.Locale);

                                //If attribute having value and Inheritable , Changed the source flag to Inherited and values moved from overridden to Inherited.
                                if (attribute != null)
                                {
                                    if (attribute.HasAnyValue())
                                    {
                                        if (model.Inheritable && attribute.OverriddenValues != null && attribute.OverriddenValues.Count > 0)
                                        {
                                            attribute.SetInheritedValue(new ValueCollection(attribute.OverriddenValues.ToXml()));
                                            attribute.OverriddenValues.Clear();
                                            attribute.SourceFlag = AttributeValueSource.Inherited;
                                        }
                                    }

                                    categoryAttributesToBeAdded.Add(attribute);
                                }
                            }

                            // Populate filtered technical attributes to source Entities.
                            foreach (Entity rEntity in reclassifyEntities)
                            {
                                if (rEntity.EntityMoveContext != null && rEntity.EntityMoveContext.ReParentType == ReParentTypeEnum.CategoryReParent)
                                {
                                    foreach (Attribute attribute in categoryAttributesToBeAdded)
                                    {
                                        Boolean isAttributeAvailableInSourceEntity = false;

                                        if (rEntity.Attributes != null)
                                        {
                                            Attribute entityAttribute = rEntity.Attributes.FirstOrDefault(a => a.Id == attribute.Id);

                                            // If a attribute has any values, remove from source Entity.
                                            if (entityAttribute != null)
                                            {
                                                isAttributeAvailableInSourceEntity = true;
                                            }
                                        }
                                        else
                                        {
                                            rEntity.Attributes = new AttributeCollection();
                                        }

                                        if (isAttributeAvailableInSourceEntity == false)
                                        {
                                            var attributeToBeAdded = new Attribute(attribute.ToXml());
                                            attributeToBeAdded.EntityId = rEntity.Id;
                                            rEntity.Attributes.Add(attributeToBeAdded);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (Constants.TRACING_ENABLED)
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "There is no Technical Attributes Mapped to target category", MDMTraceSource.EntityProcess);
                        }
                    }
                }
                finally
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Populating target category's technical attributes to Source Entities process Completed...", MDMTraceSource.EntityProcess);
                }
            }
        }

        #endregion
    }
}
