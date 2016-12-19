using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using MDM.ConfigurationManager.Business;
using MDM.EntityManager.Data;
using System.Text;

namespace MDM.EntityManager.Business.EntityOperations.Helpers
{
    using BusinessObjects;
    using CategoryManager.Business;
    using ContainerManager.Business;
    using Core;
    using DataModelManager.Business;
    using Interfaces;
    using AttributeModelManager.Business;
    using EntityModelManager.Business;
    using RelationshipManager.Business;
    using UomManager.Business;
    using Utility;

    /// <summary>
    /// 
    /// </summary>
    internal sealed class EntityFillHelper
    {
        #region Fields
        #endregion

        #region Constructors
        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="entityProcessingOptions"></param>
        /// <param name="callerContext"></param>
        public static void FillEntitiesSources(EntityCollection entities, EntityProcessingOptions entityProcessingOptions, CallerContext callerContext)
        {
            if ((callerContext.Module == MDMCenterModules.UIProcess ||
                callerContext.Module == MDMCenterModules.Entity) &&
                (callerContext.Application == MDMCenterApplication.PIM
                || callerContext.Application == MDMCenterApplication.VendorPortal
                || callerContext.Application == MDMCenterApplication.MDMCenter))
            {
                SourceBL sourceBl = new SourceBL();
                sourceBl.SetSourceToAllEntityDescendants(entities, (int)SystemSource.User);
                entityProcessingOptions.ProcessSources = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fillOptions"></param>
        /// <param name="entityManager"></param>
        /// <param name="callerContext"></param>
        public static void FillEntity(Entity entity, EntityGetOptions.EntityFillOptions fillOptions, IEntityManager entityManager, CallerContext callerContext)
        {
            FillEntities(new EntityCollection { entity }, fillOptions, entityManager, callerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="fillOptions"></param>
        /// <param name="entityManager"></param>
        /// <param name="callerContext"></param>
        public static void FillEntities(EntityCollection entities, EntityGetOptions.EntityFillOptions fillOptions, IEntityManager entityManager, CallerContext callerContext)
        {
            if (entities != null && entities.Count > 0)
            {
                var relationshipManager = new RelationshipBL();
                UOMCollection allUoms = null;

                if (fillOptions.FillUOMValues)
                {
                    allUoms = new UomBL().GetAllUoms(new UomContext(), callerContext);
                }

                foreach (var entity in entities)
                {
                    if (fillOptions.FillEntityProperties)
                    {
                        FillEntityProperties(entity, fillOptions, entityManager, callerContext);
                    }

                    if (fillOptions.FillUOMValues)
                    {
                        AttributeModelCollection attributeModels = entity.AttributeModels;

                        if (attributeModels != null && attributeModels.Count > 0)
                        {
                            foreach (Attribute attribute in entity.Attributes)
                            {
                                var attributeModel = (AttributeModel)attributeModels.GetAttributeModel(attribute.Id, attribute.Locale);

                                if (attributeModel != null)
                                {
                                    FillAttribute(attribute, attributeModel, allUoms, callerContext);
                                }
                            }
                        }
                    }

                    if (entity.Relationships != null && entity.Relationships.Count > 0)
                    {
                        relationshipManager.FillEntityRelationships(entity, entityManager, allUoms, fillOptions, callerContext);
                    }
                }
            }

            //var applicationContext = new ApplicationContext();
            //PopulateApplicationContext(entity, applicationContext, callerContext);
            //TODO: How to validate lookup values against application context?

            if (fillOptions.FillLookupDisplayValues || fillOptions.FillLookupRowWithValues)
            {
                EntityLookupAttributeHelper.PopulateLookupValues(entities, fillOptions.FillLookupDisplayValues, fillOptions.FillLookupRowWithValues, callerContext);
            }
        }

        /// <summary>
        /// Fills missing Ids in Entities by names provided
        /// </summary>
        /// <param name="entities">Represents Entities in which ids need to be filled</param>
        /// <param name="iEntityManager"></param>
        /// <param name="callerContext"></param>
        public static void FillIdsInEntityByNames(EntityCollection entities, IEntityManager iEntityManager, CallerContext callerContext)
        {
            //Scope : All Model Related Ids and AttributeIds

            foreach (Entity entity in entities)
            {
                #region Fill Entity Basic Property Ids

                FillEntityBasicPropertyIds(entity, iEntityManager, callerContext);

                #endregion

                #region Fill Entity Attributes Ids

                if (entity.Attributes != null)
                {
                    FillAttributeIds(entity.Attributes, callerContext);
                }

                #endregion

                #region Fill Relationship Ids

                if (entity.Relationships != null)
                {
                    FillRelationshipIds(entity, entity.Relationships, iEntityManager, callerContext);
                }

                #endregion

                #region Fill Extension Ids

                if (entity.ExtensionRelationships != null)
                {
                    FillExtensionIds(entity, entity.ExtensionRelationships, iEntityManager, callerContext);
                }

                #endregion
            }
        }

        /// <summary>
        /// Filling entities CategoryId, CategoryName, and ParentEntityId using EntityMoveContext for reclassification
        /// </summary>
        /// <param name="entities">Indicates the entity collection</param>
        public static void FillEntitiesUsingEntityMoveContext(EntityCollection entities)
        {
            if (entities != null && entities.Count > 0)
            {
                foreach (Entity entity in entities)
                {
                    FillEntityUsingEntityMoveContext(entity);
                }
            }
        }

        /// <summary>
        /// Filling entity CategoryId, CategoryName, and ParentEntityId using EntityMoveContext for reclassification
        /// </summary>
        /// <param name="entity">Indicates the entity</param>
        public static void FillEntityUsingEntityMoveContext(Entity entity)
        {
            if (entity != null && entity.Action == ObjectAction.Reclassify && entity.EntityMoveContext != null)
            {
                switch (entity.EntityMoveContext.ReParentType)
                {
                    case ReParentTypeEnum.CategoryReParent:
                        entity.CategoryId = entity.EntityMoveContext.TargetCategoryId;
                        entity.CategoryName = entity.EntityMoveContext.TargetCategoryName;
                        entity.ParentEntityId = entity.EntityMoveContext.TargetCategoryId;
                        break;
                    case ReParentTypeEnum.HiearchyReParent:
                        entity.ParentEntityId = entity.EntityMoveContext.TargetParentEntityId;
                        break;
                }
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entities"></param>
		/// <param name="loginUser"></param>
		/// <param name="programName"></param>
		/// <param name="populateProgramNameSystemAttribute"></param>
		/// <param name="locale"></param>
		public static void FillAuditInfoSystemAttributes(EntityCollection entities, String loginUser, String programName, Boolean populateProgramNameSystemAttribute, LocaleEnum locale)
		{
			// Get the attribute models for the AuditInfo System Attributes
			var attributeModelBL = new AttributeModelBL();
			var attributeModels = attributeModelBL.GetBaseAttributeModelsByIds(new Collection<Int32> { (Int32)SystemAttributes.CreateDateTime,
																									   (Int32)SystemAttributes.CreateUser,
																									   (Int32)SystemAttributes.CreateProgram,
																									   (Int32)SystemAttributes.LastUpdateDateTime,
																									   (Int32)SystemAttributes.LastUpdateUser,
																									   (Int32)SystemAttributes.LastUpdateProgram });

			foreach (Entity entity in entities)
			{
				if (entity.Action == ObjectAction.Create)
				{
					// Add the CreateDateTime attribute to the entity
					AddAuditRefSystemAttribute(entity, attributeModels, (int)SystemAttributes.CreateDateTime, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), locale);

					// Add the CreateUser attribute to the entity
					AddAuditRefSystemAttribute(entity, attributeModels, (int)SystemAttributes.CreateUser, loginUser, locale);

					if (populateProgramNameSystemAttribute)
					{
						// Add the CreateProgram attribute to the entity
						AddAuditRefSystemAttribute(entity, attributeModels, (int)SystemAttributes.CreateProgram, programName, locale);
					}
				}
				else if (entity.Action == ObjectAction.Update || entity.Action == ObjectAction.Delete || entity.Action == ObjectAction.Reclassify || entity.Action == ObjectAction.Rename || entity.Action == ObjectAction.ReParent)
				{
					var changeContext = entity.GetChangeContext();
					if (changeContext.IsAttributesChanged || changeContext.IsRelationshipsChanged)
					{
						// Add the LastUpdateDateTime attribute to the entity
						AddAuditRefSystemAttribute(entity, attributeModels, (int)SystemAttributes.LastUpdateDateTime, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), locale);

						// Add the LastUpdateUser attribute to the entity
						AddAuditRefSystemAttribute(entity, attributeModels, (int)SystemAttributes.LastUpdateUser, loginUser, locale);

						if (populateProgramNameSystemAttribute)
						{
							// Add the LastUpdateProgram attribute to the entity
							AddAuditRefSystemAttribute(entity, attributeModels, (int)SystemAttributes.LastUpdateProgram, programName, locale);
						}
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entities"></param>
		/// <param name="action"></param>
		/// <param name="populateProgramNameSystemAttribute"></param>
		/// <param name="locale"></param>
		public static void SetAuditInfoSystemAttributeAction(EntityCollection entities, ObjectAction action, Boolean populateProgramNameSystemAttribute, LocaleEnum locale)
		{
			IAttribute attribute;

			foreach (Entity entity in entities)
			{
				if (entity.Action == ObjectAction.Create)
				{
					attribute = entity.GetAttribute((Int32)SystemAttributes.CreateDateTime, locale);
					if (attribute != null)
					{
						attribute.Action = action;
					}

					attribute = entity.GetAttribute((Int32)SystemAttributes.CreateUser, locale);
					if (attribute != null)
					{
						attribute.Action = action;
					}

					attribute = entity.GetAttribute((Int32)SystemAttributes.CreateProgram, locale);
					if (attribute != null)
					{
						attribute.Action = action;
					}
				}
				else if (entity.Action == ObjectAction.Update || entity.Action == ObjectAction.Delete || entity.Action == ObjectAction.Reclassify || entity.Action == ObjectAction.Rename || entity.Action == ObjectAction.ReParent)
				{
					attribute = entity.GetAttribute((Int32)SystemAttributes.LastUpdateDateTime, locale);
					if (attribute != null)
					{
						attribute.Action = action;
					}

					attribute = entity.GetAttribute((Int32)SystemAttributes.LastUpdateUser, locale);
					if (attribute != null)
					{
						attribute.Action = action;
					}

					attribute = entity.GetAttribute((Int32)SystemAttributes.LastUpdateProgram, locale);
					if (attribute != null)
					{
						attribute.Action = action;
					}
				}
			}
		}

        #endregion

        #region Private Methods
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="attributeModel"></param>
        /// <param name="allUoms"></param>
        /// <param name="callerContext"></param>
        private static void FillAttribute(Attribute attribute, AttributeModel attributeModel, UOMCollection allUoms, CallerContext callerContext)
        {
            if (attributeModel.IsComplex)
            {
                FillComplexAttribute(attribute, attributeModel, allUoms, callerContext);
            }
            else
            {
                FillAttributeDetails(attribute, attributeModel, allUoms, callerContext);    
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fillOptions"></param>
        /// <param name="entityManager"></param>
        /// <param name="callerContext"></param>
        public static void FillEntityProperties(Entity entity, EntityGetOptions.EntityFillOptions fillOptions, IEntityManager entityManager, CallerContext callerContext)
        {
            #region Fill Entity basic info

            Int32 hierarchyId = 0;

            //If short name is not available then populate from original entity
            if (String.IsNullOrWhiteSpace(entity.Name) && entity.OriginalEntity != null)
            {
                entity.Name = entity.OriginalEntity.Name;
            }

            //If long name is not available then populate from original entity.
            if (String.IsNullOrWhiteSpace(entity.LongName) && entity.OriginalEntity != null)
            {
                entity.LongName = entity.OriginalEntity.LongName;
            }

            //If external id is not provided then try to fetch it from original entity..else fill ExternalId = SN
            if (String.IsNullOrWhiteSpace(entity.ExternalId))
            {
                if (entity.OriginalEntity != null)
                {
                    entity.ExternalId = entity.OriginalEntity.ExternalId;
                }
                else
                {
                    entity.ExternalId = entity.Name; // Entity SN = External Id
                }
            }

            //Fill branch level for category entities. Default value for BranchLevel is component which is good for any entity except category
            if (entity.EntityTypeId == Constants.CATEGORY_ENTITYTYPE)
            {
                entity.BranchLevel = ContainerBranchLevel.Node;
            }

            //If entity family id or global family id is not available then populate from original entity
            if (entity.OriginalEntity != null)
            {
                entity.EntityFamilyId = entity.OriginalEntity.EntityFamilyId;
                entity.EntityFamilyLongName = entity.OriginalEntity.EntityFamilyLongName;
                entity.EntityGlobalFamilyId = entity.OriginalEntity.EntityGlobalFamilyId;
                entity.EntityGlobalFamilyLongName = entity.OriginalEntity.EntityGlobalFamilyLongName;
            }

            #endregion

            #region Fill entity type info

            if (fillOptions.FillEntityTypeInfo && entity.EntityTypeId > 0
                && (String.IsNullOrWhiteSpace(entity.EntityTypeName) || String.IsNullOrWhiteSpace(entity.EntityTypeLongName)))
            {
                //Try to get entity type info from original entity.
                if (entity.OriginalEntity != null && entity.EntityTypeId.Equals(entity.OriginalEntity.EntityTypeId))
                {
                    entity.EntityTypeName = entity.OriginalEntity.EntityTypeName;
                    entity.EntityTypeLongName = entity.OriginalEntity.EntityTypeLongName;
                }
                else // not found in original entity, now make EntityTypeBL call..as no choice
                {
                    EntityType entityType = new EntityTypeBL().GetById(entity.EntityTypeId, callerContext);

                    if (entityType != null)
                    {
                        entity.EntityTypeName = entityType.Name;
                        entity.EntityTypeLongName = entityType.LongName;
                    }
                }
            }

            #endregion

            #region Fill container and org info

            if (fillOptions.FillContainerAndOrganizationInfo && entity.ContainerId > 0
                && (String.IsNullOrWhiteSpace(entity.ContainerName) || entity.OrganizationId < 1 || String.IsNullOrWhiteSpace(entity.OrganizationName)))
            {
                //Try to get entity container/org info from original entity.
                if (entity.OriginalEntity != null && entity.ContainerId.Equals(entity.OriginalEntity.ContainerId))
                {
                    entity.ContainerName = entity.OriginalEntity.ContainerName;
                    entity.ContainerLongName = entity.OriginalEntity.ContainerLongName;
                    entity.OrganizationId = entity.OriginalEntity.OrganizationId;
                    entity.OrganizationName = entity.OriginalEntity.OrganizationName;
                    entity.OrganizationLongName = entity.OriginalEntity.OrganizationLongName;
                }
                else // not found in orignal entity, now make ContainerBL call..as no choice
                {
                    Container container = new ContainerBL().GetById(entity.ContainerId);

                    if (container != null)
                    {
                        entity.ContainerName = container.Name;
                        entity.ContainerLongName = container.LongName;
                        entity.OrganizationId = container.OrganizationId;
                        entity.OrganizationName = container.OrganizationShortName;
                        entity.OrganizationLongName = container.OrganizationLongName;

                        hierarchyId = container.HierarchyId;
                    }
                }
            }

            #endregion

            #region Fill category info

            if (fillOptions.FillCategoryInfo && entity.CategoryId > 0
                && (String.IsNullOrWhiteSpace(entity.CategoryName) || String.IsNullOrWhiteSpace(entity.CategoryPath)))
            {
                //Try to get category info from original entity.
                if (entity.OriginalEntity != null && entity.CategoryId.Equals(entity.OriginalEntity.CategoryId))
                {
                    entity.CategoryName = entity.OriginalEntity.CategoryName;
                    entity.CategoryLongName = entity.OriginalEntity.CategoryLongName;
                    entity.CategoryPath = entity.OriginalEntity.CategoryPath;
                    entity.CategoryLongNamePath = entity.OriginalEntity.CategoryLongNamePath;
                }
                else // not found in original entity. Now make category get call..as there is no choice
                {
                    if (hierarchyId < 1)
                    {
                        var container = new ContainerBL().GetById(entity.ContainerId);
                        if (container != null) hierarchyId = container.HierarchyId;
                    }

                    if (hierarchyId > 0)
                    {
                        Category category = new CategoryBL().GetById(hierarchyId, entity.CategoryId, entity.Locale, callerContext, false);

                        //Populate Category name, Category LongName and Category path from Category to Entity
                        if (category != null)
                        {
                            entity.CategoryName = category.Name;
                            entity.CategoryLongName = category.LongName;

                            // Note: When we category using export to excel the categoryPath and categoryLongNamePath should not include the category name instead it 
                            //       should the value upto its parent
                            // For ex. We have a Category Electronics->Camera->DigitalCamera
                            //         If user is exporting Digital Camera then file should not contain categoryPath as "Electronics->Camera->DigitalCamera"
                            //         instead it should be "Electronics->Camera". Refer to Bug:- 170953
                            if (entity.EntityTypeId != Constants.CATEGORY_ENTITYTYPE)
                            {
                                entity.CategoryPath = category.Path;
                                entity.CategoryLongNamePath = category.LongNamePath;
                            }
                            else
                            {
                                String seperator = AppConfigurationHelper.GetAppConfig<String>("Catalog.Category.PathSeparator", " ");
                                entity.CategoryPath = RemoveCategoryNameFromCategoryPath(category.Path, entity.Name, seperator);
                                entity.CategoryLongNamePath = RemoveCategoryNameFromCategoryPath(category.LongNamePath, entity.LongName, seperator);
                            }
                        }
                    }
                }
            }

            #endregion

            #region Fill parent entity info

            // Id filling is not an optional step. normally, entity.ParentId comes as 0 only in case when some consumer is constructing enitty object from scratch and sending it for processing by just filling entity id
            if (entity.ParentEntityId <= 0 && entity.OriginalEntity != null)
            {
                entity.ParentEntityId = entity.OriginalEntity.ParentEntityId;
            }

            if (fillOptions.FillParentInfo
                && entity.ParentEntityId > 0
                && (entity.ParentEntityTypeId < 1 || String.IsNullOrWhiteSpace(entity.ParentEntityName)))
            {
                //Try to get parent entity info from original entity.
                if (entity.OriginalEntity != null && entity.ParentEntityId.Equals(entity.OriginalEntity.ParentEntityId))
                {
                    entity.ParentEntityTypeId = entity.OriginalEntity.ParentEntityTypeId;
                    entity.ParentEntityName = entity.OriginalEntity.ParentEntityName;
                    entity.ParentEntityLongName = entity.OriginalEntity.ParentEntityLongName;
                }
                // Check whether the parent is category.. If so, directly get the details from Category details if available
                else if (entity.CategoryId == entity.ParentEntityId && !String.IsNullOrWhiteSpace(entity.CategoryName) && !String.IsNullOrWhiteSpace(entity.CategoryLongName))
                {
                    entity.ParentEntityTypeId = 6; //TODO:: Get category Id from configuration instead of hard coding. 
                    entity.ParentEntityName = entity.CategoryName;
                    entity.ParentEntityLongName = entity.CategoryLongName;
                }
                else // not found in original entity and also Parent is not a Category, now make parent entity get call..as no choice
                {
                    var entityContext = new EntityContext();
                    entityContext.ContainerId = entity.ContainerId;

                    var entityGetOptions = new EntityGetOptions { PublishEvents = false, ApplyAVS = false };
                    entityGetOptions.FillOptions.SetAllEntityPropertiesLevelFillOptionsTo(false);

                    Entity parentEntity = entityManager.Get(entity.ParentEntityId, entityContext, entityGetOptions, callerContext);

                    if (parentEntity != null)
                    {
                        entity.ParentEntityTypeId = parentEntity.EntityTypeId;
                        entity.ParentEntityName = parentEntity.Name;
                        entity.ParentEntityLongName = parentEntity.LongName;
                    }
                }
            }

            #endregion

            #region Fill parent extension info

            if (entity.ParentExtensionEntityId <= 0 && entity.OriginalEntity != null)
            {
                entity.ParentExtensionEntityId = entity.OriginalEntity.ParentExtensionEntityId;
            }

            if (fillOptions.FillParentExtensionInfo && entity.ParentExtensionEntityId > 0)
            {
                Int32 parentExtensionHierarchyId = 0;

                #region Fill parent extension basic info

                if (String.IsNullOrWhiteSpace(entity.ParentExtensionEntityName))
                {
                    //Try to get parent entity info from original entity.
                    if (entity.OriginalEntity != null && entity.ParentExtensionEntityId.Equals(entity.OriginalEntity.ParentExtensionEntityId))
                    {
                        entity.ParentExtensionEntityExternalId = entity.OriginalEntity.ParentExtensionEntityExternalId;
                        entity.ParentExtensionEntityName = entity.OriginalEntity.ParentExtensionEntityName;
                        entity.ParentExtensionEntityLongName = entity.OriginalEntity.ParentExtensionEntityLongName;

                        entity.ParentExtensionEntityContainerId = entity.OriginalEntity.ParentExtensionEntityContainerId;
                        entity.ParentExtensionEntityContainerName = entity.OriginalEntity.ParentExtensionEntityContainerName;
                        entity.ParentExtensionEntityContainerLongName = entity.OriginalEntity.ParentExtensionEntityContainerLongName;

                        entity.ParentExtensionEntityCategoryId = entity.OriginalEntity.ParentExtensionEntityCategoryId;
                        entity.ParentExtensionEntityCategoryPath = entity.OriginalEntity.ParentExtensionEntityCategoryPath;
                        entity.ParentExtensionEntityCategoryName = entity.OriginalEntity.ParentExtensionEntityCategoryName;
                        entity.ParentExtensionEntityCategoryLongName = entity.OriginalEntity.ParentExtensionEntityCategoryLongName;
                        entity.ParentExtensionEntityCategoryLongNamePath = entity.OriginalEntity.ParentExtensionEntityCategoryLongNamePath;
                    }
                    else // not found in original entity..fill basic properties making get call
                    {
                        var entityContext = new EntityContext();
                        entityContext.ContainerId = entity.ParentExtensionEntityContainerId;
                        entityContext.EntityTypeId = entity.EntityTypeId;
                        entityContext.Locale = entity.Locale;
                        entityContext.LoadEntityProperties = true;
                        entityContext.LoadAttributes = false;

                        var entityGetOptions = new EntityGetOptions { PublishEvents = false, ApplyAVS = false };
                        entityGetOptions.FillOptions.SetAllEntityPropertiesLevelFillOptionsTo(false);

                        Entity parentExtensionEntity = entityManager.Get(entity.ParentExtensionEntityId, entityContext, entityGetOptions, callerContext);

                        if (parentExtensionEntity != null)
                        {
                            entity.ParentExtensionEntityExternalId = parentExtensionEntity.ExternalId;
                            entity.ParentExtensionEntityName = parentExtensionEntity.Name;
                            entity.ParentExtensionEntityLongName = parentExtensionEntity.LongName;
                            entity.ParentExtensionEntityContainerId = parentExtensionEntity.ContainerId;
                            entity.ParentExtensionEntityCategoryId = parentExtensionEntity.CategoryId;
                        }
                    }
                }

                #endregion

                #region Fill parent extension container info

                if (entity.ParentExtensionEntityContainerId > 0 && String.IsNullOrWhiteSpace(entity.ParentExtensionEntityContainerName))
                {
                    Container parentExtensionContainer = new ContainerBL().GetById(entity.ParentExtensionEntityContainerId);

                    if (parentExtensionContainer != null)
                    {
                        entity.ParentExtensionEntityContainerName = parentExtensionContainer.Name;
                        entity.ParentExtensionEntityContainerLongName = parentExtensionContainer.LongName;

                        parentExtensionHierarchyId = parentExtensionContainer.HierarchyId;
                    }
                }

                #endregion

                #region Fill parent extension category info

                if (entity.ParentExtensionEntityCategoryId > 0 && String.IsNullOrWhiteSpace(entity.ParentExtensionEntityCategoryPath))
                {
                    Category category = new CategoryBL().GetById(parentExtensionHierarchyId, entity.ParentExtensionEntityCategoryId, entity.Locale, callerContext, false);

                    //Populate Category name, Category LongName and Category path from Category to Entity
                    if (category != null)
                    {
                        entity.ParentExtensionEntityCategoryName = category.Name;
                        entity.ParentExtensionEntityCategoryLongName = category.LongName;
                        entity.ParentExtensionEntityCategoryPath = category.Path;
                        entity.ParentExtensionEntityCategoryLongNamePath = category.LongNamePath;
                    }
                }

                #endregion
            }

            #endregion

            #region Fill state validations info

            if (entity.OriginalEntity != null)
            {
                entity.SetValidationStates(entity.OriginalEntity.ValidationStates.Clone());
            }

            #endregion Fill state validations info
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="attributeModel"></param>
        /// <param name="allUoms"></param>
        /// <param name="callerContext"></param>
        private static void FillAttributeDetails(Attribute attribute, AttributeModel attributeModel, UOMCollection allUoms, CallerContext callerContext)
        {
            #region Fill Uom Values

            if (!String.IsNullOrWhiteSpace(attributeModel.AllowableUOM))
            {
                PopulateUomValues(attribute, attributeModel, allUoms, callerContext);
            }

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="attributeModel"></param>
        /// <param name="allUoms"></param>
        /// <param name="callerContext"></param>
        private static void FillComplexAttribute(Attribute attribute, AttributeModel attributeModel, UOMCollection allUoms, CallerContext callerContext)
        {
            if (attributeModel.IsComplex)
            {
                //Populate attribute id for Instance Records
                if (attribute.Attributes != null && attribute.Attributes.Count > 0)
                {
                    foreach (Attribute instanceRecord in attribute.Attributes)
                    {
                        if (instanceRecord.Attributes != null && instanceRecord.Attributes.Count > 0)
                        {
                            if (attributeModel.AttributeModels != null && attributeModel.AttributeModels.Count > 0)
                            {
                                foreach (AttributeModel childAttrModel in attributeModel.AttributeModels)
                                {
                                    try
                                    {
                                        var childAttribute = (Attribute)instanceRecord.Attributes.GetAttribute(childAttrModel.Id, childAttrModel.Locale);

                                        if (childAttribute != null)
                                        {
                                            FillAttributeDetails(childAttribute, childAttrModel, allUoms, callerContext);
                                        }
                                        else
                                        {
                                            if (Constants.TRACING_ENABLED)
                                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, "No value found for child attribute: " + childAttrModel.Name);
                                        }
                                    }
                                    catch (InvalidOperationException)
                                    {
                                        throw new InvalidOperationException("More than one attributes found for Name: " + childAttrModel.Name + " LongName: " + childAttrModel.LongName);
                                    }
                                }
                            }
                            else
                            {
                                if (Constants.TRACING_ENABLED)
                                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, "No child attributes models found for Attribute: " + attributeModel.Name);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="attributeModel"></param>
        /// <param name="allUoms"></param>
        /// <param name="callerContext"></param>
        private static void PopulateUomValues(Attribute attribute, AttributeModel attributeModel, UOMCollection allUoms, CallerContext callerContext)
        {
            var values = (ValueCollection)attribute.GetCurrentValuesInvariant();

            if (values != null && values.Count > 0)
            {
                if (allUoms != null && allUoms.Count > 0)
                {
                    var isCaseSensitiveComparison = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.UomManager.CaseSensitiveUomNames.Enabled", false);
                    StringComparison stringComparison = isCaseSensitiveComparison ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase;

                    foreach (Value value in values)
                    {
                        var uomContext = new UomContext();
                        uomContext.UomId = value.UomId;
                        uomContext.UomShortName = value.Uom;
                        uomContext.UomType = attributeModel.UomType;

                        IUOM uom = allUoms.FindUOM(uomContext, stringComparison);

                        if (uom != null)
                        {
                            value.UomId = uom.Id;
                            value.Uom = uom.Key;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="applicationContext"></param>
        /// <param name="callerContext"></param>
        private static void PopulateApplicationContext(Entity entity, ApplicationContext applicationContext, CallerContext callerContext)
        {
            applicationContext.OrganizationId = entity.OrganizationId;
            applicationContext.OrganizationName = entity.OrganizationName;
            applicationContext.ContainerId = entity.ContainerId;
            applicationContext.ContainerName = entity.ContainerName;
            applicationContext.CategoryId = entity.CategoryId;
            applicationContext.CategoryPath = entity.CategoryPath;
            applicationContext.EntityTypeId = entity.EntityTypeId;
            applicationContext.EntityTypeName = entity.EntityTypeName;
            applicationContext.Locale = entity.Locale.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="iEntityManager"></param>
        /// <param name="callerContext"></param>
		private static void FillEntityBasicPropertyIds(Entity entity, IEntityManager iEntityManager, CallerContext callerContext)
        {
            var entityModelContext = new EntityModelContext();
            Boolean needToLoadIds = false;

            if (entity.OrganizationId < 1 && !String.IsNullOrWhiteSpace(entity.OrganizationName))
            {
                entityModelContext.OrganizationName = entity.OrganizationName;
                needToLoadIds = true;
            }

            if (entity.ContainerId < 1 && !String.IsNullOrWhiteSpace(entity.ContainerName))
            {
                entityModelContext.ContainerName = entity.ContainerName;
                needToLoadIds = true;
            }

            if (entity.EntityTypeId < 1 && !String.IsNullOrWhiteSpace(entity.EntityTypeName))
            {
                entityModelContext.EntityTypeName = entity.EntityTypeName;
                needToLoadIds = true;
            }

            if (entity.CategoryId < 1 && !String.IsNullOrWhiteSpace(entity.CategoryPath))
            {
                entityModelContext.CategoryPath = entity.CategoryPath;
                needToLoadIds = true;
            }

            if (needToLoadIds)
            {
                var entityModelManager = new EntityModelBL();

                entityModelManager.FillEntityModelContextByName(ref entityModelContext, callerContext);

                if (entityModelContext.OrganizationId > 0)
                    entity.OrganizationId = entityModelContext.OrganizationId;

                if (entityModelContext.CategoryId > 0)
                    entity.CategoryId = entityModelContext.CategoryId;

                if (entityModelContext.ContainerId > 0)
                    entity.ContainerId = entityModelContext.ContainerId;

                if (entityModelContext.EntityTypeId > 0)
                    entity.EntityTypeId = entityModelContext.EntityTypeId;
            }

            //NOTE : Create Scenario no need to populate Id of an entity
            if (entity.Action != ObjectAction.Create && entity.Id < 1)
            {
                String entityName = !String.IsNullOrWhiteSpace(entity.ExternalId) ? entity.ExternalId : String.Empty;

                if (String.IsNullOrWhiteSpace(entityName) && !String.IsNullOrWhiteSpace(entity.Name))
                    entityName = entity.Name;

                entity.Id = GetEntityId(iEntityManager, entityName, entity.ContainerName, entity.CategoryName);
            }

            if (entity.ParentEntityId < 1)
            {
                String parentEntityName = !String.IsNullOrWhiteSpace(entity.ParentExternalId) ? entity.ParentExternalId : String.Empty;

                if (String.IsNullOrWhiteSpace(parentEntityName) && !String.IsNullOrWhiteSpace(entity.ParentEntityName))
                    parentEntityName = entity.ParentEntityName;

                if (parentEntityName.Equals(entity.CategoryName) && entity.CategoryId > 0)
                {
                    entity.ParentEntityId = entity.CategoryId;
                }
                else if (!String.IsNullOrWhiteSpace(entity.ContainerName) && !String.IsNullOrWhiteSpace(entity.CategoryName))
                {
                    entity.ParentEntityId = GetEntityId(iEntityManager, parentEntityName, entity.ContainerName, entity.CategoryName);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="callerContext"></param>
		private static void FillAttributeIds(AttributeCollection attributes, CallerContext callerContext)
        {
            var attributeWithAttributeUniqueIdentifier = new Dictionary<String, Attribute>();
            var attributeUniqueIdentifiers = new AttributeUniqueIdentifierCollection();

            foreach (Attribute attribute in attributes)
            {
                if (attribute.Id < 1 && !String.IsNullOrWhiteSpace(attribute.Name) && !String.IsNullOrWhiteSpace(attribute.AttributeParentName))
                {
                    AttributeUniqueIdentifier attributeUniqueIdentifier = new AttributeUniqueIdentifier(attribute.Name, attribute.AttributeParentName);
                    String attributeUniquekey = String.Format("{0}_{1}", attribute.Name, attribute.AttributeParentName);
                    attributeWithAttributeUniqueIdentifier.Add(attributeUniquekey, attribute);
                    attributeUniqueIdentifiers.Add(attributeUniqueIdentifier);
                }
            }

            if (attributeWithAttributeUniqueIdentifier.Count > 0)
            {
                AttributeModelCollection attributeModels = new AttributeModelBL().GetByUniqueIdentifiers(attributeUniqueIdentifiers, callerContext);

                if (attributeModels != null)
                {
                    PopulateAttributeIdByModels(attributeModels, attributeWithAttributeUniqueIdentifier);
                }
                else
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "No AttributeIds found for given attributeNames/GroupNames : " + attributeUniqueIdentifiers.ToXml());
                }
            }

            ICollection<Attribute> attributesWithoutIds = attributes.Where(attr => attr.Id < 1 && !String.IsNullOrEmpty(attr.Name)).ToList();

            if (attributesWithoutIds.Any())
            {
                new AttributeModelBL().PopulateAttributeIdsByNames(attributesWithoutIds);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModels"></param>
        /// <param name="attributeWithIdetifiers"></param>
		private static void PopulateAttributeIdByModels(AttributeModelCollection attributeModels, Dictionary<String, Attribute> attributeWithIdetifiers)
        {
            foreach (AttributeModel attributeModel in attributeModels)
            {
                String attributeUniquekey = String.Format("{0}_{1}", attributeModel.Name, attributeModel.AttributeParentName);
                Attribute attribute = attributeWithIdetifiers[attributeUniquekey];

                if (attribute != null)
                {
                    attribute.Id = attributeModel.Id;
                    attribute.AttributeParentId = attributeModel.AttributeParentId;

                    if (attributeModel.IsComplex && attributeModel.AttributeModels != null && attribute.Attributes != null)
                    {
                        foreach (AttributeModel childAttributeModel in attributeModel.AttributeModels)
                        {
                            foreach (Attribute instanceRecord in attribute.Attributes)
                            {
                                if (instanceRecord.Attributes != null && instanceRecord.Attributes.Count > 0)
                                {
                                    IAttribute childAttribute = instanceRecord.Attributes.GetAttribute(childAttributeModel.Name, childAttributeModel.AttributeParentName, childAttributeModel.Locale);

                                    if (childAttribute != null)
                                    {
                                        childAttribute.Id = childAttributeModel.Id;
                                        childAttribute.AttributeParentId = childAttributeModel.AttributeParentId;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="relationships"></param>
        /// <param name="iEntityManager"></param>
        /// <param name="callerContext"></param>
		private static void FillRelationshipIds(Entity entity, RelationshipCollection relationships, IEntityManager iEntityManager, CallerContext callerContext)
        {
            if (entity.Relationships != null)
            {
                foreach (Relationship relationship in relationships)
                {
                    #region Basic Properties

                    FillRelationshipBasicPropertyIds(entity, relationship, iEntityManager, callerContext);

                    #endregion

                    #region Relationship Attribute Ids

                    AttributeCollection relationshipAttributes = (AttributeCollection)relationship.GetRelationshipAttributes();

                    if (relationshipAttributes != null)
                    {
                        FillAttributeIds(relationshipAttributes, callerContext);
                    }

                    #endregion

                    #region Child Relationships

                    if (relationship.RelationshipCollection != null)
                    {
                        FillRelationshipIds(entity, relationship.RelationshipCollection, iEntityManager, callerContext);
                    }

                    #endregion
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="extensions"></param>
        /// <param name="iEntityManager"></param>
        /// <param name="callerContext"></param>
		private static void FillExtensionIds(Entity entity, ExtensionRelationshipCollection extensions, IEntityManager iEntityManager, CallerContext callerContext)
        {
            if (entity.ExtensionRelationships != null)
            {
                var entityModelManager = new EntityModelBL();

                foreach (ExtensionRelationship extension in extensions)
                {
                    if (extension.ContainerId < 1 && !String.IsNullOrWhiteSpace(extension.ContainerName))
                    {
                        extension.ContainerId = entityModelManager.GetContainerIdByName(extension.ContainerName, callerContext);

                        if (extension.ContainerId < 1)
                        {
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Failed to resolve {0} extension's containerId by container name: {1} for entity: [Id: {2} name: {3}]. Please provide valid ExtensionContainerName."
                                , extension.Name, extension.ContainerName, entity.Id, entity.Name));
                        }
                    }

                    if (extension.CategoryId < 1 && !String.IsNullOrWhiteSpace(extension.CategoryPath))
                    {
                        Category category = entityModelManager.GetCategoryByPath(extension.ContainerName, extension.CategoryPath, callerContext);

                        if (category != null)
                        {
                            extension.CategoryId = category.Id;
                            extension.CategoryName = category.Name;
                            extension.CategoryLongName = category.LongName;
                            extension.CategoryPath = category.Path;
                        }
                        else
                        {
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error,
                                String.Format("Failed to resolve {0} extension's categoryId by categoryPath: {1} for entity: [Id: {2} name: {3}]. Please provide valid CategoryPath.",
                                extension.Name, extension.CategoryPath, entity.Id, entity.Name));
                        }
                    }

                    if (extension.RelatedEntityId < 1)
                    {
                        String entityName = !String.IsNullOrWhiteSpace(extension.ExternalId) ? extension.ExternalId : String.Empty;

                        if (String.IsNullOrWhiteSpace(entityName) && !String.IsNullOrWhiteSpace(extension.Name))
                            entityName = entity.Name;

                        extension.RelatedEntityId = GetEntityId(iEntityManager, entityName, extension.ContainerName, extension.CategoryName);

                        if (extension.RelatedEntityId < 1)
                        {
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Failed to resolve {0} extension.RelatedEntityId by Name : {1}, Container name :{2} , Category Name: {3} for entity: [Id: {4} and name: {5}]. Please provide valid extension.externalId/extension.Name",
                                extension.Name, entityName, extension.ContainerName, extension.CategoryName, entity.Id, entity.Name));
                        }
                    }

                    // When the direction is up (parent)
                    if (entity.ParentExtensionEntityId < 1 && extension.Direction == RelationshipDirection.Up)
                    {
                        entity.ParentExtensionEntityId = entity.Id;
                    }

                    if (extension.RelationshipCollection != null)
                    {
                        FillExtensionIds(entity, extension.RelationshipCollection, iEntityManager, callerContext);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="relationship"></param>
        /// <param name="iEntityManager"></param>
        /// <param name="callerContext"></param>
		private static void FillRelationshipBasicPropertyIds(Entity entity, Relationship relationship, IEntityManager iEntityManager, CallerContext callerContext)
        {
            if (relationship != null)
            {
                if (relationship.ContainerId < 1)
                {
                    relationship.ContainerId = entity.ContainerId;
                }

                if (relationship.FromEntityId < 1)
                {
                    relationship.FromEntityId = entity.Id;
                }

                EntityModelBL entityModelManager = new EntityModelBL();

                #region Fill RelatedEntity (ToEntity) details
                
                if (relationship.ToEntityTypeId < 1 && !String.IsNullOrWhiteSpace(relationship.ToEntityTypeName))
                {
                    relationship.ToEntityTypeId = entityModelManager.GetEntityTypeIdByName(relationship.ToEntityTypeName, callerContext);
                    
                    if (relationship.ToEntityTypeId < 1)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Failed to resolve ToEntityTypeId by Name: {0} for entity: [Id: {1} name: {2}]. Please provide valid ToEntityTypeName.", relationship.ToEntityTypeName, entity.Id, entity.Name));
                    }
                }

                if (relationship.ToContainerId < 1 && !String.IsNullOrWhiteSpace(relationship.ToContainerName))
                {
                    relationship.ToContainerId = entityModelManager.GetContainerIdByName(relationship.ToContainerName, callerContext);

                    if (relationship.ToContainerId < 1)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Failed to resolve ToContainerId by Name: {0} for entity: [Id: {1} name: {2}]. Please provide valid ToContainerName.", relationship.ToContainerName, entity.Id, entity.Name));
                    }
                }

                if (relationship.RelatedEntityId < 1 && !String.IsNullOrWhiteSpace(relationship.ToExternalId) && !String.IsNullOrWhiteSpace((relationship.ToCategoryPath)))
                {
                    if (!String.IsNullOrWhiteSpace(relationship.ToContainerName))
                    {
                        Category category = entityModelManager.GetCategoryByPath(relationship.ToContainerName, relationship.ToCategoryPath, callerContext);
                        
                        if (category != null)
                        {
                            relationship.RelatedEntityId = GetEntityId(iEntityManager, relationship.ToExternalId, relationship.ToContainerName, category.Name);

                            if (relationship.RelatedEntityId < 1)
                            {
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Failed to resolve RelatedEntityId by Name: {0} under CategoryPath {1} for entity: [Id: {2} name: {3}]. Please provide valid RelatedEntityName or ToCategoryPath.", relationship.ToEntityTypeName, relationship.ToCategoryPath, entity.Id, entity.Name));
                            }
                        }
                    }
                }

                #endregion

                if (relationship.RelationshipTypeId <= 0 && !String.IsNullOrWhiteSpace(relationship.RelationshipTypeName))
                {
                    relationship.RelationshipTypeId = entityModelManager.GetRelationshipTypeIdByName(relationship.RelationshipTypeName, callerContext);

                    if (relationship.RelationshipTypeId < 1)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Failed to resolve RelationshipTypeId by Name: {0} for entity: [Id: {1} name: {2}]. Please provide valid RelationshipTypeName.", relationship.RelationshipTypeName, entity.Id, entity.Name));
                    }
                }

                if (String.IsNullOrWhiteSpace(relationship.Path) && relationship.RelatedEntityId > 0)
                {
                    relationship.Path = String.Format("{0}_{1}", relationship.FromEntityId, relationship.RelatedEntityId);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iEntityManager"></param>
        /// <param name="entityName"></param>
        /// <param name="containerName"></param>
        /// <param name="categoryName"></param>
        /// <returns></returns>
		private static Int64 GetEntityId(IEntityManager iEntityManager, String entityName, String containerName, String categoryName)
        {
            var entityDA = new EntityDA();
            var dbCommandProperties = DBCommandHelper.Get(MDMCenterApplication.PIM, MDMCenterModules.Entity, MDMCenterModuleAction.Read);
            return entityDA.GetEntityId(entityName, categoryName, containerName, dbCommandProperties);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="attributeModels"></param>
		/// <param name="attributeId"></param>
		/// <param name="value"></param>
		/// <param name="locale"></param>
		private static void AddAuditRefSystemAttribute(Entity entity, AttributeModelCollection attributeModels, Int32 attributeId, String value, LocaleEnum locale)
		{
			var attributeModel = (AttributeModel)attributeModels.GetAttributeModel(attributeId, locale);

			if (!entity.AttributeModels.Contains(attributeId, locale))
			{
				entity.AttributeModels.Add(attributeModel, true);
			}

			if (!entity.Attributes.Contains(attributeId))
			{
				var attribute = new Attribute(attributeModel);
				attribute.SetValue(value);

				entity.Attributes.Add(attribute, true);
			}
        }
        /// <summary>
        /// This method returns Empty String for root category by design.
        /// For any other child categories, it will return path up to its parent.
        /// </summary>
        /// <param name="categoryPath"></param>
        /// <param name="categoryName"></param>
        /// <param name="seperator"></param>
        /// <returns></returns>
        private static String RemoveCategoryNameFromCategoryPath(String categoryPath, String categoryName, String seperator)
        {
            Int32 lastIndexofSeperator = categoryPath.LastIndexOf(seperator);

            String newCategoryPath = String.Empty;

            if (lastIndexofSeperator > 0)
            {
                newCategoryPath = categoryPath.Remove(lastIndexofSeperator, categoryPath.Length - lastIndexofSeperator);
            }
            
            return newCategoryPath;
		}

		#endregion

        #endregion
    }
}