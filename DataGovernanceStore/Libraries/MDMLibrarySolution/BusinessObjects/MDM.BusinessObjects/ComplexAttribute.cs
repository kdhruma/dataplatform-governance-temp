using System;
using System.Collections.Generic;
using System.Linq;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Class provides methods to manipulate Complex Attributes.
    /// </summary>
    internal class ComplexAttribute : IComplexAttribute
    {
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
        /// Field denoting the sequence of on complex attribute record
        /// </summary>
        private Decimal _sequence = -1;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize ComplexAttribute object from attribute
        /// </summary>
        /// <param name="attribute">Indicates the complex attribute</param>
        public ComplexAttribute(IAttribute attribute)
        {
            if (attribute != null)
            {
                _parentAttribute = (Attribute)attribute;
                var value = attribute.GetCurrentValueInstance();
                if (value != null)
                {
                    _instanceAttribute = (Attribute)attribute.GetComplexAttributeInstanceBySequence(value.Sequence);

                    this._sequence = value.Sequence;

                    if (_instanceAttribute != null)
                    {
                        this._attributes = (AttributeCollection)_instanceAttribute.GetChildAttributes();
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("Complex attribute parent is null.");
            }
        }

        /// <summary>
        /// Initialize ComplexAttribute object from attribute
        /// </summary>
        /// <param name="parentAttribute">Indicates the complex attribute</param>
        /// <param name="sequence">Indicates the sequence number</param>
        public ComplexAttribute(IAttribute parentAttribute, Decimal sequence)
        {
            if (parentAttribute != null)
            {
                this._parentAttribute = (Attribute)parentAttribute;
                this._instanceAttribute = (Attribute)parentAttribute.GetComplexAttributeInstanceBySequence(sequence);
                if (this._instanceAttribute != null)
                {
                    this._attributes = (AttributeCollection)this._instanceAttribute.GetChildAttributes();
                }
                this._sequence = sequence;
            }
            else
            {
                throw new ArgumentNullException("Complex attribute parent is null.");
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
        /// <param name="attributeName">Indicates the complex child attribute short name</param>
        /// <returns>Returns the current invariant value for the complex child attribute</returns>
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
        /// <param name="attributeName">Indicates the complex child attribute short name</param>
        /// <returns>Returns the Inherited value for the complex child attribute</returns>
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
        /// Update the complex child attribute value
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
        /// Update the complex attribute based on the child attributes
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
        /// Update the complex attribute based on the child attributes.
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
        /// Update the complex child attribute value
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
        /// Remove the complex child record value
        /// </summary>
        /// <param name="attributeName">Indicates the attribute short name</param>
        public void RemoveValue(String attributeName)
        {
            if (this._parentAttribute.Action != ObjectAction.Delete && this._instanceAttribute.Action != ObjectAction.Delete)
            {
                var attribute = GetAttribute(attributeName);
                attribute.ClearValue();

                this._parentAttribute.Action = ObjectAction.Update;
            }
        }

        /// <summary>
        /// Remove all the child values
        /// </summary>
        public void RemoveValues()
        {
            if (_parentAttribute.Action != ObjectAction.Delete)
            {
                _parentAttribute.RemoveComplexChildRecordByInstanceRefId(_instanceAttribute.InstanceRefId);
                
                if (_parentAttribute.IsCollection)
                {
                    _parentAttribute.RemoveValue(_parentAttribute.OverriddenValues.GetByValueRefId(_instanceAttribute.InstanceRefId));
                }
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

                var childs = this._parentAttribute.GetComplexChildAttributes();
                if (childs != null)
                {
                    this._attributes = (AttributeCollection)childs;
                }
                //If parent attribute is inherited then make it overridden. Else can't add the child attribute.
                this._parentAttribute.SourceFlag = AttributeValueSource.Overridden;
                this._parentAttribute.AddComplexChildRecord(this._attributes);
            }

            Attribute attribute = (Attribute)GetAttribute(attributeName);

            // Reason setting action here is, When the case, user updates the value for cell1 from UI and cell2 value updates through BR
            // In this case compare & merge logic will set the cell2 attribute action as delete since there is no value for the cell.
            // & the same time attribute.SetValue method having a logic when attribute action is delete it will not update the action. So forcefully setting the action here.
            attribute.Action = ObjectAction.Update; 
            attribute.SetValue(value);
            attribute.Sequence = _sequence;

            var attributeValue = attribute.GetCurrentValueInstance();
            if (attributeValue != null && !attribute.IsLookup)
            {
                attributeValue.ValueRefId = 0;
            }

            if (_parentAttribute.Action == ObjectAction.Read)
            {
                _parentAttribute.Action = ObjectAction.Update;
            }

            if (this._instanceAttribute.InstanceRefId > 0)
            {
                IValueCollection values = this._parentAttribute.GetOverriddenValues();
                if (values != null)
                {
                    Value parentAttrValue = (Value)values.GetBySequence(_sequence);

                    if (parentAttrValue != null)
                    {
                        // Logical Flow: Resetting the Instance Ref Id when BR is updating the complex attribute value.
                
                        Int32 newRefId = -1;

                        foreach (Value val in values)
                        {
                            newRefId = Math.Min(newRefId, val.ValueRefId);
                        }

                        this._instanceAttribute.InstanceRefId = --newRefId;
                        this._instanceAttribute.SetValue(newRefId);
                        parentAttrValue.ValueRefId = newRefId;
                        parentAttrValue.AttrVal = newRefId;
                    }
                }
            }
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
    }
}
