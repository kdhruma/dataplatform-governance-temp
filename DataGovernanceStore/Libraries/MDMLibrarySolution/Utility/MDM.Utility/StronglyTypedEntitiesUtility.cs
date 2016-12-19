using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDM.BusinessObjects;
using MDM.Interfaces;
using System.Reflection;
using MDM.Core;

namespace MDM.Utility
{
    /// <summary>
    /// Represents strongly typed entity utility for getting and setting values from/to object
    /// </summary>
    public static class StronglyTypedEntitiesUtility
    {
        private static Type _StronglyTypedMetaDataAttributeType = typeof(StronglyTypedMetaDataAttribute);
        private static Type _GenericListType = typeof(List<>);
        private static Type _GenericValueUomPairType = typeof(ValueUomPair<>);

        /// <summary>
        /// Gets proto-buf serialized version of specified entity for the specified strongly typed entity type 
        /// </summary>
        /// <typeparam name="T">System type of strongly typed entity</typeparam>
        /// <param name="entity">MDM entity</param>
        /// <returns>Proto-buf serialized version</returns>
        public static byte[] GetSerializedObjectFromEntity<T>(IEntity entity)
            where T : StronglyTypedEntityBase
        {
            return ProtoBufSerializationHelper.Serialize(GetStrongTypeObjectFromEntity<T>(entity));
        }

        /// <summary>
        /// Gets the strong type object from entity object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static T GetStrongTypeObjectFromEntity<T>(IEntity entity)
            where T : StronglyTypedEntityBase
        {
            // Note - Invalid Data will not be supported.
            T obj = (T)Activator.CreateInstance(typeof(T));

            #region Set Base Entity Properties
            obj.Id = entity.Id;
            obj.Name = entity.Name;
            obj.ExternalId = entity.ExternalId;
            obj.LongName = entity.LongName;
            obj.Path = entity.Path;
            obj.IdPath = entity.IdPath;
            obj.CategoryId = entity.CategoryId;
            obj.CategoryName = entity.CategoryName;
            obj.CategoryLongName = entity.CategoryLongName;
            obj.CategoryPath = entity.CategoryPath;
            obj.CategoryLongNamePath = entity.CategoryLongNamePath;
            obj.ContainerId = entity.ContainerId;
            obj.ContainerName = entity.ContainerName;
            obj.ContainerLongName = entity.ContainerLongName;
            obj.OrganizationId = entity.OrganizationId;
            obj.OrganizationName = entity.OrganizationName;
            obj.EntityTypeId = entity.EntityTypeId;
            obj.EntityTypeName = entity.EntityTypeName;
            obj.EntityTypeLongName = entity.EntityTypeLongName;
            obj.ParentEntityId = entity.ParentEntityId;
            obj.ParentEntityName = entity.ParentEntityName;
            obj.ParentEntityLongName = entity.ParentEntityLongName;
            obj.ParentEntityTypeId = obj.ParentEntityTypeId;
            obj.ParentExtensionEntityId = entity.ParentExtensionEntityId;
            obj.ParentExtensionEntityExternalId = entity.ParentExtensionEntityExternalId;
            obj.ParentExtensionEntityName = entity.ParentExtensionEntityName;
            obj.ParentExtensionEntityLongName = entity.ParentExtensionEntityLongName;
            obj.ParentExtensionEntityCategoryId = entity.ParentExtensionEntityCategoryId;
            obj.ParentExtensionEntityCategoryName = entity.ParentExtensionEntityCategoryName;
            obj.ParentExtensionEntityCategoryLongName = entity.ParentExtensionEntityCategoryLongName;
            obj.ParentExtensionEntityCategoryPath = entity.ParentExtensionEntityCategoryPath;
            obj.ParentExtensionEntityCategoryLongNamePath = entity.ParentExtensionEntityCategoryLongNamePath;
            obj.ParentExtensionEntityContainerId = entity.ParentExtensionEntityContainerId;
            obj.ParentExtensionEntityContainerName = entity.ParentExtensionEntityContainerName;
            obj.ParentExtensionEntityContainerLongName = entity.ParentExtensionEntityContainerLongName;
            #endregion

            IAttributeCollection attrCollection = entity.GetAttributes();

            SetPropertiesForObject(entity.GetAttributes(), obj);

            return obj;
        }

        #region Helpers for Strongly Typed Object Serialization

        private static void SetPropertiesForObject(IAttributeCollection attributeCollection, Object obj)
        {
            Dictionary<String, PropertyInfo> typeProperties = null;
            if (ProtoBufSerializationHelper.StrongTypePropertyMap.TryGetValue(obj.GetType(), out typeProperties))
            {
                foreach (var keyValuePair in typeProperties)
                {
                    String[] splitKeyArray = keyValuePair.Key.Split(new String[] { "##@##" }, 2, StringSplitOptions.None);
                    PropertyInfo property = keyValuePair.Value;

                    MDM.BusinessObjects.Attribute attribute = attributeCollection.GetAttribute(new AttributeUniqueIdentifier(splitKeyArray[0], splitKeyArray[1])) as MDM.BusinessObjects.Attribute;

                    if (attribute != null)
                    {
                        if (!attribute.IsComplex && !attribute.IsCollection && !attribute.IsLookup)
                        {
                            try
                            {
                                SetSimpleDataTypeProperty(obj, property, attribute);
                            }
                            catch (InvalidCastException) 
                            { 
                                //Ignore the cast exceptions 
                                //TODO: How to handle the cast exceptions
                            }
                        }
                        else if (attribute.IsComplex)
                        {
                            if (attribute.IsCollection)
                            {
                                Type[] complexType = property.PropertyType.GetGenericArguments();
                                Object objInstance = Activator.CreateInstance((_GenericListType).MakeGenericType(complexType));

                                MethodInfo listAddMethod = objInstance.GetType().GetMethod("Add");

                                foreach (var item in attribute.GetChildAttributes())
                                {
                                    Object complexInstance = Activator.CreateInstance(complexType.FirstOrDefault());
                                    SetPropertiesForObject(item.GetChildAttributes(), complexInstance);
                                    listAddMethod.Invoke(objInstance, new[] { complexInstance });
                                }

                                property.SetValue(obj, objInstance, null);
                            }
                            else
                            {
                                Type complexType = property.GetType();
                                Object complexInstance = Activator.CreateInstance(complexType);

                                SetPropertiesForObject(attribute.GetChildAttributes(), complexInstance);

                                property.SetValue(obj, complexInstance, null);
                            }
                        }
                        else if (attribute.IsLookup)
                        {
                            // TODO - Where to store WSID for Lookup
                            if (attribute.IsCollection)
                            {
                                List<String> lookupValueList = new List<String>();

                                ValueCollection currentValues = attribute.CurrentValues;
                                if (currentValues != null && currentValues.Count > 0)
                                {
                                    foreach (Value valueObject in currentValues)
                                    {
                                        String displayValue = valueObject.GetDisplayValue();
                                        if (displayValue != null)
                                        {
                                            lookupValueList.Add(displayValue);
                                        }
                                    }
                                }
                                
                                property.SetValue(obj, lookupValueList, null);
                            }
                            else
                            {
                                var currentValue = attribute.GetCurrentValueInstance();
                                String displayValue = currentValue == null ? String.Empty : currentValue.GetDisplayValue();
                                property.SetValue(obj, displayValue != null ? displayValue : String.Empty, null);
                            }
                        }
                        else if (attribute.IsCollection)
                        {
                            try
                            {
                                SetCollectionObjectProperty(obj, property, attribute);
                            }
                            catch (InvalidCastException)
                            {
                                //Ignore the cast exceptions 
                                //TODO: How to handle the cast exceptions
                            }
                        }
                    }
                }
            }
        }

        private static void SetSimpleDataTypeProperty(Object obj, PropertyInfo property, IAttribute attribute)
        {
            IValue value = attribute.GetCurrentValueInstanceInvariant();
            if (value != null)
            {
                switch (attribute.AttributeDataType)
                {
                    case AttributeDataType.String:
                    case AttributeDataType.ImageURL:
                    case AttributeDataType.URL:
                    case AttributeDataType.Fraction:
                        if (!String.IsNullOrWhiteSpace(value.Uom))
                        {
                            property.SetValue(obj, new ValueUomPair<String>()
                            {
                                Value = value.GetStringValue(),
                                Uom = value.Uom
                            }, null);
                        }
                        else
                        {
                            property.SetValue(obj, value.GetStringValue(), null);
                        }
                        break;
                    case AttributeDataType.Decimal:
                        if (!String.IsNullOrWhiteSpace(value.Uom))
                        {
                            property.SetValue(obj, new ValueUomPair<Decimal?>()
                            {
                                Value = value.GetNumericValue(),
                                Uom = value.Uom
                            }, null);
                        }
                        else
                        {
                            property.SetValue(obj, value.GetNumericValue(), null);
                        }
                        break;

                    case AttributeDataType.Integer:
                        if (!String.IsNullOrWhiteSpace(value.Uom))
                        {
                        }
                        else
                        {
                            property.SetValue(obj, ValueTypeHelper.ConvertToNullableInt32(value.GetStringValue()), null);
                        }
                        break;
                    case AttributeDataType.DateTime:
                    case AttributeDataType.Date:
                        property.SetValue(obj, value.GetDateTimeValue(), null);
                        break;
                    case AttributeDataType.Boolean:
                        property.SetValue(obj, ValueTypeHelper.ConvertToNullableBoolean(value.GetStringValue()), null);
                        break;
                    case AttributeDataType.File:
                        // TODO
                        break;
                    case AttributeDataType.Image:
                        // TODO
                        break;
                }
            }
        }

        private static void SetCollectionObjectProperty(object obj, PropertyInfo property, IAttribute attribute)
        {
            switch (attribute.AttributeDataType)
            {
                case AttributeDataType.ImageURL:
                case AttributeDataType.URL:
                case AttributeDataType.String:
                case AttributeDataType.Fraction:
                    List<String> valueList = new List<String>();
                    List<ValueUomPair<String>> valueListWithUOM = new List<ValueUomPair<String>>();

                    foreach (var value in attribute.GetCurrentValues())
                    {
                        if (!String.IsNullOrWhiteSpace(value.Uom))
                        {
                            valueListWithUOM.Add(new ValueUomPair<String>()
                            {
                                Value = value.GetStringValue(),
                                Uom = value.Uom
                            });
                        }
                        else
                        {
                            valueList.Add(value.GetStringValue());
                        }
                    }

                    if (valueListWithUOM.Count > 0)
                        property.SetValue(obj, valueListWithUOM, null);
                    else
                        property.SetValue(obj, valueList, null);
                    break;
                case AttributeDataType.Decimal:
                    List<ValueUomPair<Decimal?>> decimalValueListWithUOM = new List<ValueUomPair<Decimal?>>();
                    List<Decimal?> decimalValueList = new List<Decimal?>();

                    foreach (var value in attribute.GetCurrentValues())
                    {
                        if (!String.IsNullOrWhiteSpace(value.Uom))
                        {
                            decimalValueListWithUOM.Add(new ValueUomPair<Decimal?>()
                            {
                                Value = value.GetNumericValue(),
                                Uom = value.Uom
                            });
                        }
                        else
                        {
                            decimalValueList.Add(value.GetNumericValue());
                        }
                    }

                    if (decimalValueListWithUOM.Count > 0)
                        property.SetValue(obj, decimalValueListWithUOM, null);
                    else
                        property.SetValue(obj, decimalValueList, null);

                    break;
                case AttributeDataType.Integer:
                    List<ValueUomPair<Int32?>> integerValueListWithUOM = new List<ValueUomPair<int?>>();
                    List<Int32?> integerValueList = new List<Int32?>();
                    foreach (var value in attribute.GetCurrentValues())
                    {
                        if (!String.IsNullOrWhiteSpace(value.Uom))
                        {
                            integerValueListWithUOM.Add(new ValueUomPair<Int32?>()
                            {
                                Value = ValueTypeHelper.ConvertToNullableInt32(value.GetStringValue()),
                                Uom = value.Uom
                            });
                        }
                        else
                        {
                            integerValueList.Add(ValueTypeHelper.ConvertToNullableInt32(value.GetStringValue()));
                        }
                    }

                    if (integerValueListWithUOM.Count > 0)
                        property.SetValue(obj, integerValueListWithUOM, null);
                    else
                        property.SetValue(obj, integerValueList, null);
                    break;
                case AttributeDataType.DateTime:
                case AttributeDataType.Date:
                    List<DateTime?> dateValueList = new List<DateTime?>();
                    foreach (var value in attribute.GetCurrentValues())
                    {
                        dateValueList.Add(value.GetDateTimeValue());
                    }
                    property.SetValue(obj, dateValueList, null);
                    break;
                case AttributeDataType.Boolean:
                    List<Boolean?> boolValueList = new List<bool?>();
                    foreach (var value in attribute.GetCurrentValues())
                    {
                        boolValueList.Add(ValueTypeHelper.ConvertToNullableBoolean(value.GetStringValue()));
                    }
                    break;
                case AttributeDataType.File:
                    break;
                case AttributeDataType.Image:
                    break;
            }
        }

        #endregion

        /// <summary>
        /// Gets simple attribute value for specified attribute model of strongly typed entity
        /// </summary>
        /// <param name="entity">MDM entity</param>
        /// <param name="attributeModel">Attribute Model</param>
        /// <param name="uomSeparator">UOM separator</param>
        /// <returns>Simple attribute value</returns>
        public static String GetSimpleAttrValByAttrId(object entity, IAttributeModel attributeModel, String uomSeparator)
        {
            String returnValue = String.Empty;
            if (attributeModel != null)
            {
                PropertyInfo requestedProp = GetAttrPropByAttr(entity, attributeModel.Name, attributeModel.AttributeParentName);
                object attrValue = null;
                if (requestedProp != null)
                {
                    attrValue = requestedProp.GetValue(entity, null);
                }

                if (attrValue != null)
                {
                    if (String.IsNullOrWhiteSpace(attributeModel.AllowableUOM))
                        returnValue = attrValue.ToString();
                    else
                    {
                        Type attrType = attrValue.GetType();
                        String value = attrType.GetProperty("Value").GetValue(attrValue, null).ToString();
                        String uom = attrType.GetProperty("Uom").GetValue(attrValue, null).ToString();
                        returnValue = String.Format("{0}{1}{2}", value, uomSeparator, uom);
                    }
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Gets collection attribute value for specified attribute model of strongly typed entity
        /// </summary>
        /// <param name="entity">MDM entity</param>
        /// <param name="attributeModel">Attribute Model</param>
        /// <param name="uomSeparator">UOM Separator</param>
        /// <param name="collectionSeparator">Collection Separator</param>
        /// <returns>Collection attribute value</returns>
        public static String GetCollectionAttrValByAttrId(object entity, IAttributeModel attributeModel, String uomSeparator, String collectionSeparator)
        {
            String returnValue = String.Empty;
            if (attributeModel != null)
            {
                PropertyInfo requestedProp = GetAttrPropByAttr(entity, attributeModel.Name, attributeModel.AttributeParentName);
                if (requestedProp != null)
                {
                    object values = requestedProp.GetValue(entity, null);
                    if (values != null)
                    {
                        AttributeDataType dataType = (AttributeDataType)attributeModel.AttributeDataTypeId;

                        switch (dataType)
                        {
                            case AttributeDataType.Date:
                            case AttributeDataType.DateTime:
                                if (String.IsNullOrWhiteSpace(attributeModel.AllowableUOM))
                                    returnValue = String.Join<DateTime?>(collectionSeparator, values as List<DateTime?>);
                                else
                                    returnValue = GetCollectionWithUOMValue<DateTime?>(values, uomSeparator, collectionSeparator);
                                break;
                            case AttributeDataType.String:
                            case AttributeDataType.Fraction:
                            case AttributeDataType.URL:
                            case AttributeDataType.ImageURL:
                            case AttributeDataType.Unknown:
                                if (String.IsNullOrWhiteSpace(attributeModel.AllowableUOM))
                                    returnValue = String.Join<String>(collectionSeparator, values as List<String>);
                                else
                                    returnValue = GetCollectionWithUOMValue<String>(values, uomSeparator, collectionSeparator);
                                break;
                            case AttributeDataType.Decimal:
                                if (String.IsNullOrWhiteSpace(attributeModel.AllowableUOM))
                                    returnValue = String.Join<Decimal?>(collectionSeparator, values as List<Decimal?>);
                                else
                                    returnValue = GetCollectionWithUOMValue<Decimal?>(values, uomSeparator, collectionSeparator);
                                break;
                            case AttributeDataType.Boolean:
                                if (String.IsNullOrWhiteSpace(attributeModel.AllowableUOM))
                                    returnValue = String.Join<Boolean?>(collectionSeparator, values as List<Boolean?>);
                                else
                                    returnValue = GetCollectionWithUOMValue<Boolean?>(values, uomSeparator, collectionSeparator);
                                break;
                            case AttributeDataType.Integer:
                                if (String.IsNullOrWhiteSpace(attributeModel.AllowableUOM))
                                    returnValue = String.Join<Int32?>(collectionSeparator, values as List<Int32?>);
                                else
                                    returnValue = GetCollectionWithUOMValue<Int32?>(values, uomSeparator, collectionSeparator);
                                break;
                            case AttributeDataType.File:
                            case AttributeDataType.Image:
                            default:
                                returnValue = String.Join<object>(collectionSeparator, values as List<object>);
                                break;
                        }
                    }
                }
            }
            return returnValue;
        }

        #region Helper methods for Value Get from Strongly typed entity

        private static String GetCollectionWithUOMValue<T>(object values, String uomSeparator, String collectionSeparator)
        {
            StringBuilder value = new StringBuilder();
            List<ValueUomPair<T>> attributeValues = values as List<ValueUomPair<T>>;
            if (attributeValues != null && attributeValues.Count > 0)
            {
                value.AppendFormat("{0}{1}{2}", attributeValues[0].Value, uomSeparator, attributeValues[0].Uom);
                for (int index = 1; index < attributeValues.Count; index++)
                {
                    value.AppendFormat("{0}{1}{2}{3}", attributeValues[index].Value, uomSeparator, attributeValues[index].Uom, collectionSeparator);
                }
            }
            return value.ToString();
        }

        #endregion

        /// <summary>
        /// Gets property information for the specified attribute id from strongly typed entity
        /// </summary>
        /// <param name="entity">MDM entity</param>
        /// <param name="shortName">The short name.</param>
        /// <param name="parentName">Name of the parent.</param>
        /// <returns>
        /// Property information
        /// </returns>
        public static PropertyInfo GetAttrPropByAttr(object entity, String shortName, String parentName)
        {
            Dictionary<String, PropertyInfo> typeProperties = null;
            if(ProtoBufSerializationHelper.StrongTypePropertyMap.TryGetValue(entity.GetType(), out typeProperties))
            {
                PropertyInfo propertyInfo = null;
                if (typeProperties.TryGetValue(shortName + "##@##" + parentName, out propertyInfo))
                {
                    return propertyInfo;
                }
            }
            return null;
        }

        /// <summary>
        /// Removes the complex instance record and keep only the current values 
        /// </summary>
        public static void EnsureComplexCurrentValues(EntityCollection entities)
        {
            foreach (Entity entity in entities)
            {
                foreach (MDM.BusinessObjects.Attribute attribute in entity.Attributes)
                {
                    if (attribute.IsComplex == true && attribute.IsCollection == true)
                    {
                        if (attribute.GetInheritedValues().Count > 0 && attribute.GetOverriddenValues().Count > 0)
                        {
                            foreach (var val in attribute.GetInheritedValues())
                            {
                                Boolean overriddenMatchFound = false;

                                foreach (var valOverridden in attribute.GetOverriddenValues())
                                {
                                    if (valOverridden.ValueRefId == val.ValueRefId)
                                    {
                                        overriddenMatchFound = true;
                                        break;
                                    }
                                }

                                if (overriddenMatchFound == false)
                                {
                                    IAttribute attributetemp = attribute.Attributes.GetAttribute(attribute.Id, val.ValueRefId, attribute.Locale);
                                    attribute.Attributes.Remove(attributetemp);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
