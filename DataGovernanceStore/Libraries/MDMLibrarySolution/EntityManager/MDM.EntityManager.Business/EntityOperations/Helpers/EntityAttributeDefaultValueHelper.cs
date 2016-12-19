using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.EntityManager.Business.EntityOperations.Helpers
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.Interfaces;
    using MDM.AdminManager.Business;
    using MDM.AttributeModelManager.Business;
    
    /// <summary>
    /// 
    /// </summary>
    internal sealed class EntityAttributeDefaultValueHelper
    {
        #region Methods
        
        /// <summary>
        /// Populates the default value for attributes UOMs in case if attribute value is non empty but attribute UOM is empty.
        /// Attributes model data should be preloaded.
        /// </summary>
        /// <param name="entities">Entities collection</param>
        public static void PopulateEmptyUOMsForNonEmptyAttributesByDefaultUOMs(EntityCollection entities)
        {
            foreach (Entity entity in entities)
            {
                if (entity.Action == ObjectAction.Create || entity.Action == ObjectAction.Update)
                {
                    foreach (Attribute attribute in entity.Attributes)
                    {
                        if ((attribute.Action != ObjectAction.Create && attribute.Action != ObjectAction.Update) ||
                                attribute.SourceFlag != AttributeValueSource.Overridden || attribute.IsComplex || attribute.IsLookup)
                        {
                            continue;
                        }

                        AttributeModel attributeModel = (AttributeModel) entity.AttributeModels.GetAttributeModel(attribute.Id, attribute.Locale);
                        if (attributeModel != null && !String.IsNullOrEmpty(attributeModel.DefaultUOM))
                        {
                            if (attribute.IsCollection)
                            {
                                IValueCollection attributeValues = attribute.GetOverriddenValuesInvariant();
                                if (attributeValues != null && attributeValues.Count > 0)
                                {
                                    foreach (Value value in attributeValues)
                                    {
                                        if (value != null && String.IsNullOrEmpty(value.Uom) && value.AttrVal != null && !String.IsNullOrEmpty(value.AttrVal.ToString()))
                                        {
                                            value.Uom = attributeModel.DefaultUOM;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Value value = (Value) attribute.GetCurrentValueInstance();
                                if (value != null && String.IsNullOrEmpty(value.Uom) && value.AttrVal != null && !String.IsNullOrEmpty(value.AttrVal.ToString()))
                                {
                                    value.Uom = attributeModel.DefaultUOM;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Populates the default value for the attributes,when new entity is created
        /// </summary>
        /// <param name="entitiesForCreate"></param>
        /// <param name="entityOperationResults"></param>
        /// <param name="allSiblingEntitiesWithSameAttributes"></param>
        /// <param name="firstEntity"></param>
        /// <param name="loginUser"></param>
        public static void PopulateDefaultAttributeValues(EntityCollection entitiesForCreate, EntityOperationResultCollection entityOperationResults, Boolean allSiblingEntitiesWithSameAttributes, Entity firstEntity, String loginUser)
        {
            Dictionary<Attribute, AttributeModel> firstEntityAttributesWithModels = null;
            var sourceBl = new SourceBL();

            if (allSiblingEntitiesWithSameAttributes && firstEntity != null)
            {
                firstEntityAttributesWithModels = GetDefaultValues(firstEntity, loginUser);
            }

            foreach (Entity entity in entitiesForCreate)
            {
                EntityOperationResult entityOR = entityOperationResults.GetEntityOperationResult(entity.Id);

                Dictionary<Attribute, AttributeModel> defaultAttributesWithModels;

                if (allSiblingEntitiesWithSameAttributes && firstEntityAttributesWithModels != null)
                    defaultAttributesWithModels = firstEntityAttributesWithModels;
                else
                    defaultAttributesWithModels = GetDefaultValues(entity, loginUser);

                if (defaultAttributesWithModels == null)
                    continue;

                Boolean isModelsLoaded = entity.AttributeModels != null;
                Boolean isEORLoaded = entityOR != null;

                Dictionary<Attribute, AttributeModel>.Enumerator enumerator = defaultAttributesWithModels.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    Attribute attribute = enumerator.Current.Key;
                    AttributeModel attributeModel = enumerator.Current.Value;

                    Attribute defaultAttribute = attribute;

                    if (allSiblingEntitiesWithSameAttributes)
                    {
                        defaultAttribute = attribute.Clone();
                        defaultAttribute.Action = attribute.Action;
                    }

                    sourceBl.SetSourceToAttributeValues(new AttributeCollection() { defaultAttribute }, (Int32)SystemSource.System);
                    entity.Attributes.Add(defaultAttribute);

                    if (isModelsLoaded)
                    {
                        entity.AttributeModels.Add(attributeModel);
                    }

                    if (isEORLoaded)
                    {
                        var attributeOperationResult = new AttributeOperationResult(defaultAttribute.Id, defaultAttribute.Name, defaultAttribute.LongName, defaultAttribute.AttributeModelType, defaultAttribute.Locale);
                        entityOR.AttributeOperationResultCollection.Add(attributeOperationResult);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        private static Dictionary<Attribute, AttributeModel> GetDefaultValues(Entity entity, String loginUser)
        {
            var outputData = new Dictionary<Attribute, AttributeModel>();

            var attributeModelBL = new AttributeModelBL();

            var attributeModelContext = new AttributeModelContext();
            attributeModelContext.ContainerId = entity.ContainerId;
            attributeModelContext.CategoryId = entity.CategoryId;
            attributeModelContext.EntityTypeId = entity.EntityTypeId;
            attributeModelContext.AttributeModelType = AttributeModelType.All;
            attributeModelContext.Locales = new Collection<LocaleEnum> { entity.Locale };
            attributeModelContext.ApplySorting = false;
            attributeModelContext.ApplySecurity = false;
            attributeModelContext.ApplyAttributeDependency = false;
            attributeModelContext.LoadPermissions = true;

            AttributeModelCollection allMappedAttributeModels = attributeModelBL.Get(attributeModelContext);

            var entityChangeContext = (EntityChangeContext)entity.GetChangeContext();

            foreach (AttributeModel attributeModel in allMappedAttributeModels)
            {
                if (String.IsNullOrEmpty(attributeModel.DefaultValue))
                    continue;

                if (entityChangeContext.AttributeIdList != null && entityChangeContext.AttributeIdList.Contains(attributeModel.Id))
                {
                    continue;
                }

                var attribute = new Attribute(attributeModel, attributeModel.Locale);

                //Create a value object.
                //Assign value in neutral format. System date will also be assigned in UTC format.
                IValue value = MDMObjectFactory.GetIValue();
                value.Action = ObjectAction.Create;
                value.Locale = attribute.Locale;
                value.AttrVal = EvaluateDefaultValue(attributeModel.DefaultValue, attributeModel.AttributeDataTypeId, loginUser);
                if (!String.IsNullOrEmpty(attributeModel.DefaultUOM))
                {
                    value.Uom = attributeModel.DefaultUOM;
                }

                if (attributeModel.IsLookup)
                {
                    if (value.AttrVal != null)
                    {
                        value.ValueRefId = ValueTypeHelper.Int32TryParse(value.AttrVal.ToString(), 0);
                    }
                }

                if (attributeModel.IsCollection)
                {
                    value.Sequence = 0;
                }

                attribute.SetValueInvariant(value);

                outputData.Add(attribute, attributeModel);
            }

            return outputData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <param name="dataTypeId"></param>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        private static String EvaluateDefaultValue(String defaultValue, Int32 dataTypeId, String loginUser)
        {
            if (String.IsNullOrEmpty(defaultValue))
                return String.Empty;

            String returnValue = defaultValue;

            switch (defaultValue.Trim().ToLowerInvariant())
            {
                case "[system date]":
                    {
                        returnValue = dataTypeId == 14 ? DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fff") : DateTime.UtcNow.ToShortDateString();
                        break;
                    }
                case "[user login]":
                    {
                        returnValue = loginUser;
                        break;
                    }
                case "[user email]":
                    {
                        returnValue = GetUserDetail(loginUser, "Email");
                        break;
                    }
                case "[user firstname]":
                    {
                        returnValue = GetUserDetail(loginUser, "FirstName");
                        break;
                    }
                case "[user lastname]":
                    {
                        returnValue = GetUserDetail(loginUser, "LastName");
                        break;
                    }
                case "[user fullname]":
                    {
                        returnValue = GetUserDetail(loginUser, "FullName");
                        break;
                    }
            }

            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginUser"></param>
        /// <param name="detailName"></param>
        /// <returns></returns>
        private static String GetUserDetail(String loginUser, String detailName)
        {
            String returnValue = String.Empty;

            var userBL = new SecurityUserBL();
            SecurityUser user = userBL.GetUser(loginUser);

            if (user != null)
            {
                switch (detailName.ToLowerInvariant())
                {
                    case "email":
                        {
                            returnValue = user.Smtp;
                            break;
                        }
                    case "firstname":
                        {
                            returnValue = user.FirstName;
                            break;
                        }
                    case "lastname":
                        {
                            returnValue = user.LastName;
                            break;
                        }
                    case "fullname":
                        {
                            returnValue = String.Concat(user.FirstName, " ", user.LastName);
                            break;
                        }
                }
            }

            return returnValue;
        }

        #endregion
    }
}
