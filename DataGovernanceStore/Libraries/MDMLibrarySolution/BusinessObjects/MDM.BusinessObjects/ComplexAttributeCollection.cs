using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.BusinessObjects
{

    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Complex attribute Instance Collection for the Object
    /// </summary>
    internal class ComplexAttributeCollection : IComplexAttributeCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting the complex attribute parent
        /// </summary>
        private Collection<ComplexAttribute> _complexAttributeCollection = new Collection<ComplexAttribute>();

        /// <summary>
        /// Indicates the parent Attribute 
        /// </summary>
        private Attribute _parentAttribute = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor with complex parent attribute
        /// </summary>
        /// <param name="parentAttribute">Indicates the complex attribute</param>
        public ComplexAttributeCollection(IAttribute parentAttribute)
        {
            LoadComplexAttributeCollection(parentAttribute);
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            Int32 hashCode = 0;

            foreach (ComplexAttribute complexAttribute in this._complexAttributeCollection)
            {
                hashCode += complexAttribute.GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Remove all child attribute values
        /// </summary>
        public void RemoveValues()
        {
            foreach (ComplexAttribute cAttribute in this._complexAttributeCollection)
            {
                cAttribute.RemoveValues();
            }
        }

        /// <summary>
        /// Get all the child attribute's current value based on the child attribute short name
        /// </summary>
        /// <param name="shortName">Indicates the child attribute short name</param>
        /// <typeparam name="T">Indicates the date type in which format want the result</typeparam>
        /// <returns>Returns the child attribute current values as requested data type</returns>
        public Collection<T> GetChildAttributeCurrentValues<T>(String shortName)
        {
            return this.GetAttributeValues<T>(shortName, false);
        }

        /// <summary>
        /// Get all the child attribute's current invariant values based on the child attribute short name
        /// </summary>
        /// <param name="shortName">Indicates the child attribute short name</param>
        /// <typeparam name="T">Indicates the date type in which format want the result</typeparam>
        /// <returns>Returns the child attribute's current invariant values as requested data type</returns>
        public Collection<T> GetChildAttributeCurrentValuesInvariant<T>(String shortName)
        {
            return this.GetAttributeValues<T>(shortName, true);
        }

        /// <summary>
        /// Add a new complex record
        /// </summary>
        /// <param name="childAttributes">Indicates the child attributes</param>
        public void AddRow(IAttributeCollection childAttributes)
        {
            this.CreateNewRow(childAttributes);
        }

        /// <summary>
        /// Add a new complex record
        /// </summary>
        /// <param name="values">Indicates the child attributes as key values format. Key is child attribute short name and values is child attribute value</param>
        public void AddRow(Dictionary<String, Object> values)
        {
            var attributes = (AttributeCollection)_parentAttribute.GetComplexChildAttributes(); //This will returns only the 1st row attributes
            var children = attributes.Clone();    //Clone the child attributes since we are adding into the same collection.

            //Clear the values as clone copies the overridden values as well
            ClearValuesForChildAttributes(children);

            foreach (KeyValuePair<String, Object> attr in values)
            {
                var childAttribute = children.GetAttribute(attr.Key, _parentAttribute.Locale);

                if (childAttribute != null)
                {
                    childAttribute.SetValue(attr.Value);
                }
            }

            this.CreateNewRow(children);
        }

        /// <summary>
        /// Add a new complex record
        /// </summary>
        /// <param name="values">Indicates the child attributes as key values format. Key is child attribute short name and values is child attribute value</param>
        public void AddRow(Dictionary<String, IValue> values)
        {
            var attributes = (AttributeCollection)_parentAttribute.GetComplexChildAttributes(); //This will returns only the 1st row attributes
            var children = attributes.Clone();    //Clone the child attributes since we are adding into the same collection.

            //Clear the values as clone copies the overridden values as well
            ClearValuesForChildAttributes(children);

            foreach (KeyValuePair<String, IValue> attr in values)
            {
                var childAttribute = children.GetAttribute(attr.Key, _parentAttribute.Locale);

                if (childAttribute != null)
                {
                    childAttribute.SetValue(attr.Value);
                }
            }

            this.CreateNewRow(children);
        }

        #endregion

        #region Private Methods

        private void ClearValuesForChildAttributes(IAttributeCollection attributeCollection)
        {
            foreach (Attribute attribute in attributeCollection)
            {
                attribute.OverriddenValues.Clear();
                attribute.InheritedValues.Clear();
                if (attribute.Attributes != null && attribute.Attributes.Count > 0)
                {
                    ClearValuesForChildAttributes(attribute.Attributes);
                }
            }
        }

        private void LoadComplexAttributeCollection(IAttribute parentAttribute)
        {
            if (parentAttribute != null)
            {
                _parentAttribute = (Attribute)parentAttribute;

                if (!parentAttribute.IsComplex)
                {
                    String exceptionMessage = String.Format("Attribute with name: {0} and group name: {1} is not a complex attribute", parentAttribute.Name, parentAttribute.AttributeParentName);
                    throw new ArgumentException(exceptionMessage);
                }
                else if (!parentAttribute.IsCollection)
                {
                    String exceptionMessage = String.Format("Attribute with name: {0} and group name: {1} is not a collection attribute", parentAttribute.Name, parentAttribute.AttributeParentName);
                    throw new ArgumentException(exceptionMessage);
                }

                IValueCollection values = parentAttribute.GetCurrentValues();

                foreach (Value value in values)
                {
                    ComplexAttribute complexAttribute = new ComplexAttribute(parentAttribute, value.Sequence);
                    this._complexAttributeCollection.Add(complexAttribute);
                }
            }
            else
            {
                throw new ArgumentNullException("Complex attribute parent is null.");
            }
        }

        private void CreateNewRow(IAttributeCollection attributes)
        {
            //If parent attribute is inherited then make it overridden. Else can't add the child attribute.
            _parentAttribute.SourceFlag = AttributeValueSource.Overridden;
            var attr = this._parentAttribute.AddComplexChildRecord(attributes);

            ComplexAttribute attribute = new ComplexAttribute(_parentAttribute, attr.Sequence);
            this._complexAttributeCollection.Add(attribute);
            AcceptChanges();
        }

        private void AcceptChanges()
        {
            //For complex attribute there is no logic available for merging the attribute value as part of entity process
            //So remaining all the record make action as update. Else entity process will ignore the read action attribute values.
            IValueCollection values = _parentAttribute.GetCurrentValues();

            foreach (Value value in values)
            {
                var childs = _parentAttribute.GetComplexAttributeInstanceBySequence(value.Sequence);

                value.Id = -1;

                foreach (IAttribute attr in childs.GetChildAttributes())
                {
                    Attribute attribute = (Attribute)attr;
                    if (attribute.Action != ObjectAction.Delete)
                    {
                        attribute.Action = ObjectAction.Update;
                    }
                }
            }
        }

        public Collection<T> GetAttributeValues<T>(String shortName, Boolean getInvariantValue)
        {
            Collection<T> result = new Collection<T>();
            if (!String.IsNullOrWhiteSpace(shortName))
            {
                foreach (ComplexAttribute complexAttribute in this._complexAttributeCollection)
                {
                    if (getInvariantValue)
                    {
                        result.Add(complexAttribute.GetChildAttributeCurrentValueInvariant<T>(shortName));
                    }
                    else
                    {
                        result.Add(complexAttribute.GetChildAttributeCurrentValue<T>(shortName));
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("Child attribute short name is empty or null");
            }

            return result;
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a complex attribute collection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._complexAttributeCollection.GetEnumerator();
        }

        #endregion IEnumerable Members

        #region IEnumerable<ComplexAttribute> Members

        /// <summary>
        /// Returns an enumerator that iterates through a Complex attribute collection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<ComplexAttribute> GetEnumerator()
        {
            return this._complexAttributeCollection.GetEnumerator();
        }

        #endregion IEnumerable<ComplexAttribute> Members

        #endregion
    }
}
