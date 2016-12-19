using System;
using System.Collections.Generic;
using System.Linq;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Class provides methods to manipulate Hierarchical Attributes.
    /// </summary>
    public class HierarchicalAttribute //: IHierarchicalAttribute
    {
        /* // Most of this class will be implemented/tested in upcoming releases. - Prasad Ballingam
        #region Fields

        /// <summary>
        /// Field denoting the list of attributes.
        /// </summary>
        private AttributeCollection _attributes = null;

        /// <summary>
        /// Field denoting the list of attributes.
        /// </summary>
        private Attribute _instanceAttribute = null;

        /// <summary>
        /// Indicates the parent Attribute 
        /// </summary>
        private Attribute _parentAttribute = null;

        /// <summary>
        /// Field denoting the sequence of on Hierarchical attribute record
        /// </summary>
        private Decimal _sequence = -1;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize HierarchicalAttribute object from attribute
        /// </summary>
        /// <param name="attribute">Indicates the Hierarchical attribute</param>
        public HierarchicalAttribute(IAttribute attribute)
        {
            if (attribute != null)
            {
                _parentAttribute = (Attribute)attribute;
                var value = attribute.GetCurrentValueInstance();
                if (value != null)
                {
                    // TODO PRASAD Implement GetHierarchicalAttributeInstanceBySequence
                    // _instanceAttribute = (Attribute)attribute.GetHierarchicalAttributeInstanceBySequence(value.Sequence);

                    this._sequence = value.Sequence;

                    if (_instanceAttribute != null)
                    {
                        this._attributes = (AttributeCollection)_instanceAttribute.GetChildAttributes();
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("Hierarchical attribute parent is null.");
            }
        }

        /// <summary>
        /// Initialize HierarchicalAttribute object from attribute
        /// </summary>
        /// <param name="parentAttribute">Indicates the Hierarchical attribute</param>
        /// <param name="sequence">Indicates the sequence number</param>
        public HierarchicalAttribute(IAttribute parentAttribute, Decimal sequence)
        {
            if (parentAttribute != null)
            {
                this._parentAttribute = (Attribute)parentAttribute;
                // TODO PRASAD Implement GetHierarchicalAttributeInstanceBySequence
                // this._instanceAttribute = (Attribute)parentAttribute.GetHierarchicalAttributeInstanceBySequence(sequence);
                if (this._instanceAttribute != null)
                {
                    this._attributes = (AttributeCollection)this._instanceAttribute.GetChildAttributes();
                }
                this._sequence = sequence;
            }
            else
            {
                throw new ArgumentNullException("Hierarchical attribute parent is null.");
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get and set value for the child attribute
        /// </summary>
        /// <param name="attributeName">Indicates the attribute short name</param>
        /// <returns>Returns the attribute current value</returns>
        public Object this[String attributeName]
        {
            get
            {
                IAttribute attr = GetAttribute(attributeName);
                if (attr != null)
                {
                    return attr.GetCurrentValue();
                }
                return null;
            }
            set
            {
                SetAttributeValue(attributeName, value);
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        #region Child Attribute Utilities

        /// <summary>
        /// Get all the child attributes
        /// </summary>
        /// <returns>Returns the child attributes</returns>
        public IAttributeCollection GetChildAttributes()
        {
            return (IAttributeCollection)this._attributes;
        }

        /// <summary>
        /// Gets the child attribute based on the attribute short  name
        /// </summary>
        /// <param name="attributeName">Indicates the attribute Name</param>
        /// <returns>Returns the attribute object</returns>
        public IAttribute GetChildAttribute(String attributeName)
        {
            return GetAttribute(attributeName);
        }

        /// <summary>
        /// Gets the child attribute's current value as Dictionary format.
        /// Dictionary key is attribute name and the value is attribute current value.
        /// </summary>
        /// <returns>Returns the Dictionary object.Dictionary key is attribute name and the value is attribute current value.</returns>
        public Dictionary<String, Object> GetChildAttributesAsDictionary()
        {
            Dictionary<String, Object> result = new Dictionary<String, Object>();

            if (this._attributes != null)
            {
                foreach (Attribute attribute in this._attributes)
                {
                    result.Add(attribute.Name, attribute.GetCurrentValue());
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the child lookup attribute based on the attribute short name
        /// </summary>
        /// <param name="attributeName">Indicates the attribute Name</param>
        /// <returns>Returns the ILookupAttribute object</returns>
        public ILookupAttribute GetChildAttributeAsLookup(String attributeName)
        {
            IAttribute attribute = GetAttribute(attributeName);

            if (!attribute.IsLookup)
            {
                throw new NotSupportedException(String.Format("{0} is not a lookup attribute. Use GetChildAttribute() method to get non lookup child attribute", attribute.Name));
            }

            return new LookupAttribute(attribute);
        }

        #endregion Child Attribute Utilities

        #region Utilities for getting child attribute values

        /// <summary>
        /// Returns the current (considering source flag) value object of the requested child attribute
        /// </summary>
        /// <returns>Current value object (considering source flag)</returns>
        public IValue GetChildAttributeCurrentValueInstance(String attributeName)
        {
            var attribute = GetAttribute(attributeName);
            if (attribute != null)
            {
                return attribute.GetCurrentValueInstance();
            }
            return null;
        }

        /// <summary>
        /// Gets the child attribute's current invariant value based on child attribute name
        /// </summary>
        /// <param name="attributeName">Indicates the Hierarchical child attribute short name</param>
        /// <returns>Returns the current invariant value for the Hierarchical child attribute</returns>
        public IValue GetChildAttributeCurrentValueInstanceInvariant(String attributeName)
        {
            var attribute = GetAttribute(attributeName);
            if (attribute != null)
            {
                return attribute.GetCurrentValueInstanceInvariant();
            }
            return null;
        }

        /// <summary>
        /// Gets the child attribute's Inherited value based on child attribute name
        /// </summary>
        /// <param name="attributeName">Indicates the Hierarchical child attribute short name</param>
        /// <returns>Returns the Inherited value for the Hierarchical child attribute</returns>
        public IValue GetChildAttributeInheritedValueInstance(String attributeName)
        {
            var attribute = GetAttribute(attributeName);
            if (attribute != null)
            {
                return attribute.GetInheritedValueInstance();
            }
            return null;
        }

        /// <summary>
        /// Get the child attribute's current value based on requested data type
        /// </summary>
        /// <param name="shortName">Indicates the child attribute short name</param>
        /// <exception cref="InvalidCastException">Not able to format the attribute value to the requested data type</exception>
        /// <returns>Returns the child attribute current value as requested data type</returns>
        public T GetChildAttributeCurrentValue<T>(String shortName)
        {
            return GetChildAttributeValue<T>(shortName, false);
        }

        /// <summary>
        /// Get the child attribute's current invariant value based on requested data type
        /// </summary>
        /// <param name="shortName">Indicates the child attribute short name</param>
        /// <exception cref="InvalidCastException">Not able to format the attribute value to the requested data type</exception>
        /// <returns>Returns the child attribute current invariant value as requested data type</returns>
        public T GetChildAttributeCurrentValueInvariant<T>(String shortName)
        {
            return GetChildAttributeValue<T>(shortName, true);
        }

        #endregion Utilities for getting child attribute Values

        #region Utilities for updating child attribute values

        /// <summary>
        /// Update the Hierarchical child attribute value
        /// </summary>
        /// <param name="attributeName">Indicates the child attribute short name</param>
        /// <param name="value">Indicates the value of the child attribute</param>
        public void SetValue(String attributeName, IValue value)
        {
            if (value != null)
            {
                SetAttributeValue(attributeName, value.AttrVal);
            }
            else
            {
                throw new ArgumentNullException(String.Format("There is no value to set for attribute '{0}'.", attributeName));
            }
        }

        /// <summary>
        /// Update the Hierarchical attribute based on the child attributes
        /// </summary>
        /// <param name="childAttributes">Indicates the child attribute collections</param>
        public void SetValues(IAttributeCollection childAttributes)
        {
            foreach (IAttribute attribute in childAttributes)
            {
                SetAttributeValue(attribute.Name, attribute.GetCurrentValue());
            }
        }

        /// <summary>
        /// Update the Hierarchical attribute based on the child attributes.
        /// </summary>
        /// <param name="values">Indicates the child attribute values. Dictionary key is attribute short name and the value is 
        /// corresponding values for the child.</param>
        public void SetValues(Dictionary<String, Object> values)
        {
            foreach (KeyValuePair<String, Object> attribute in values)
            {
                SetAttributeValue(attribute.Key, attribute.Value);
            }
        }

        /// <summary>
        /// Update the Hierarchical child attribute value
        /// </summary>
        /// <param name="attributeName">Indicates the child attribute short name</param>
        /// <param name="value">Indicates the value of the child attribute</param>
        public void SetValue(String attributeName, Object value)
        {
            SetAttributeValue(attributeName, value);
        }

        #endregion Utilities for updating child attribute values

        #region Utilities for clearing child attribute values

        /// <summary>
        /// Remove the Hierarchical child record value
        /// </summary>
        /// <param name="attributeName">Indicates the attribute short name</param>
        public void RemoveValue(String attributeName)
        {
            var attribute = GetAttribute(attributeName);
            attribute.ClearValue();
            UpdateHierarchicalAttributeAction();
        }

        /// <summary>
        /// Remove all the child values
        /// </summary>
        public void RemoveValues()
        {
            UpdateHierarchicalAttributeAction();

            foreach (Attribute childAttribute in _attributes)
            {
                childAttribute.ClearValue();
            }
        }

        #endregion Utilities for clearing child attribute values

        #endregion

        #region Private Methods

        private IAttribute GetAttribute(String attributeName)
        {
            IAttribute result = null;

            if (!String.IsNullOrWhiteSpace(attributeName))
            {
                foreach (Attribute attribute in this._attributes)
                {
                    if (attribute.Name.ToLower().Trim() == attributeName.ToLower().Trim())
                    {
                        result = attribute;
                        break;
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("Attribute name is empty or null");
            }

            if (result == null)
            {
                throw new ArgumentException(String.Format("{0} is not valid or not belongs to {1} group", attributeName, _parentAttribute.Name));
            }

            return result;
        }

        private void SetAttributeValue(String attributeName, Object value)
        {
            if (this._attributes == null || this._attributes.Count < 1)
            {
                this._instanceAttribute = (Attribute)this._parentAttribute.GetChildAttributes().FirstOrDefault();

                // TODO PRASAD implement _parentAttribute.GetHierarchicalChildAttributes();
                AttributeCollection childs = null; // this._parentAttribute.GetHierarchicalChildAttributes();
                if (childs != null)
                {
                    this._attributes = (AttributeCollection)childs;
                }
                //If parent attribute is inherited then make it overridden. Else can't add the child attribute.
                this._parentAttribute.SourceFlag = AttributeValueSource.Overridden;
                // TODO PRASAD implement this._parentAttribute.AddHierarchicalChildRecord(this._attributes);
                // this._parentAttribute.AddHierarchicalChildRecord(this._attributes);
            }

            Attribute attribute = (Attribute)GetAttribute(attributeName);
            attribute.SetValue(value);
            attribute.Sequence = _sequence;

            var attributeValue = attribute.GetCurrentValueInstance();
            if (attributeValue != null)
            {
                attributeValue.ValueRefId = 0;
            }

            UpdateHierarchicalAttributeAction(attribute);
        }

        private void UpdateHierarchicalAttributeAction(IAttribute attribute = null)
        {
            if (attribute != null)
            {
                attribute.Action = ObjectAction.Create;
            }
            this._instanceAttribute.Action = ObjectAction.Create;
            this._parentAttribute.Action = ObjectAction.Create;
        }

        private T GetChildAttributeValue<T>(String shortName, Boolean getInvariantValue)
        {
            T value = default(T);
            IAttribute attribute = GetAttribute(shortName);

            if (attribute != null)
            {
                if (getInvariantValue)
                {
                    value = attribute.GetCurrentValueInvariant<T>();
                }
                else
                {
                    value = attribute.GetCurrentValue<T>();
                }
            }

            return value;
        }

        #endregion

        #endregion
        
        */ // End of commented 

        #region Public Static Methods

        /// <summary>
        /// UpdateHierarchicalAttributeAction iterates thru the tree of hierachical attribute and updates action.
        /// As Hierarchical Attribute uses Flush and Fill logic, this helper method sets Action field for all child attributes recursively.
        /// </summary>
        /// <param name="hierarchicalAttribute"></param>
        /// <param name="targetAction"></param>
        public static void UpdateHierarchicalAttributeAction(IAttribute hierarchicalAttribute, ObjectAction targetAction)
        {
            if (hierarchicalAttribute == null)
            {
                return;
            }

            foreach (MDM.BusinessObjects.Attribute instanceRecord in hierarchicalAttribute.GetChildAttributes())
            {
                if (instanceRecord.Action == ObjectAction.Delete)
                {
                    continue;
                }

                if (instanceRecord.Action == ObjectAction.Read)
                {
                    instanceRecord.Action = targetAction;
                }

                foreach (MDM.BusinessObjects.Attribute attribute in instanceRecord.GetChildAttributes())
                {
                    if (attribute.IsHierarchical)
                    {
                        UpdateHierarchicalAttributeAction(attribute, targetAction);
                    }
                    else if (attribute.Action == ObjectAction.Read)
                    {
                        attribute.Action = targetAction;
                    }
                }
            }

            if (hierarchicalAttribute.IsCollection)
            {
                foreach (Value value in hierarchicalAttribute.GetCurrentValues())
                {
                    if (value.Action == ObjectAction.Read && value.Sequence > -1)
                    {
                        value.Id = -1;
                        value.AttrVal = "";
                        value.Action = targetAction;
                        hierarchicalAttribute.Action = targetAction;
                    }
                }
            }
        }

        /// <summary>
        /// Updates Hierarchical attribute based on its action, recursively:
        ///     - All Delete Action child attributes will be removed.
        ///     - All child Attributes with empty values/no values will be removed.
        ///     - If all child attributes are removed, that particular attribute itself will also be removed.
        /// </summary>
        /// <param name="attribute"></param>
        public static void UpdateAttributePerAction(Attribute attribute)
        {
            Int32 totalComplexInstanceAttributes = attribute.GetChildAttributes().Count;

            Int32 deletedComplexInstanceAttributes = 0;
            Decimal sequence = -1;

            if (attribute.IsCollection == true)
            {
                sequence = 0;
            }

            var attributesToDelete = new AttributeCollection();

            foreach (Value complexParentValue in attribute.OverriddenValues)
            {
                IAttribute complexInstanceAttr = attribute.GetComplexAttributeInstanceByInstanceRefId(complexParentValue.ValueRefId);

                if (complexInstanceAttr != null)
                {
                    //For each attribute object set the action as DB does a flush and fill of complex attributes.
                    if (complexInstanceAttr.Action == ObjectAction.Read)
                    {
                        complexInstanceAttr.Action = attribute.Action;
                    }

                    Int32 totalComplexChildAttributes = complexInstanceAttr.GetChildAttributes().Count;

                    Int32 deletedComplexChildAttributes = 0;

                    foreach (Attribute complexChildAttr in complexInstanceAttr.GetChildAttributes())
                    {
                        if (attribute.IsHierarchical)
                        {
                            // Delete child attr if its Action is Delete of if parent attr Action is Delete
                            if (complexInstanceAttr.Action == ObjectAction.Delete || complexChildAttr.Action == ObjectAction.Delete)
                            {
                                complexChildAttr.Action = ObjectAction.Delete;
                                deletedComplexChildAttributes++;
                                attributesToDelete.Add(complexChildAttr);
                            }
                            else if (complexChildAttr.IsHierarchical)
                            {
                                // Make recursive call to calculate sub attribute level action/sequence.
                                UpdateAttributePerAction(complexChildAttr);

                                // Delete child hierarchical attr if all its children have Delete action or have no values.
                                // Note that complexChildAttr.Action may get updated to Delete in the recursive call above.
                                if (complexChildAttr.Action == ObjectAction.Delete)
                                {
                                    complexChildAttr.Action = ObjectAction.Delete;
                                    deletedComplexChildAttributes++;
                                    attributesToDelete.Add(complexChildAttr);
                                }
                            }
                            else if (!complexChildAttr.HasValue())
                            {
                                // Delete child attr if there is no value present
                                complexChildAttr.Action = ObjectAction.Delete;
                                attributesToDelete.Add(complexChildAttr);
                                deletedComplexChildAttributes++;
                            }
                            else if (complexChildAttr.Action == ObjectAction.Read)
                            {
                                complexChildAttr.Action = complexInstanceAttr.Action;
                            }
                        }
                    }

                    foreach (var attributeToDelete in attributesToDelete)
                    {
                        complexInstanceAttr.GetChildAttributes().Remove(attributeToDelete);
                    }

                    attributesToDelete.Clear();

                    if (deletedComplexChildAttributes == totalComplexChildAttributes)
                    {
                        complexInstanceAttr.Action = ObjectAction.Delete;
                        complexParentValue.Action = ObjectAction.Delete;
                        deletedComplexInstanceAttributes++;
                        if (attribute.Attributes.Count > 1)
                        {
                            attribute.Attributes.Remove(complexInstanceAttr);
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

            // Instance Records were already Removed above.
            // Remove 'Delete' action value objects from collection.
            if (attribute.IsHierarchical)
            {
                var valueCollection = attribute.GetOverriddenValues();

                for (Int32 i = valueCollection.Count - 1; i >= 0; i--)
                {
                    if (valueCollection.ElementAt(i).Action == ObjectAction.Delete)
                    {
                        valueCollection.Remove(valueCollection.ElementAt(i));
                    }
                }
            }

            if (deletedComplexInstanceAttributes == totalComplexInstanceAttributes)
            {
                attribute.Action = ObjectAction.Delete;
            }
        }

        #endregion

    }
}
