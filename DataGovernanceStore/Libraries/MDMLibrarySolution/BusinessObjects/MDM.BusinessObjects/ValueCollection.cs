using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Attribute Value Instance Collection for the Object
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class ValueCollection : ICollection<Value>, IEnumerable<Value>, IValueCollection
    {
        #region Fields

        //This is the most funny part of the WCF which is able to serialize even a private member.. needs to do further RnD to find out the internals
        [DataMember]
        [ProtoMember(1)]
        private Collection<Value> _values = new Collection<Value>();

        /// <summary>
        /// Indicates any of the values in current collection has invalid values.
        /// </summary>
        private Boolean _hasInvalidValues = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ValueCollection() : base() { }

        /// <summary>
        /// Initialize ValueCollection with Value
        /// </summary>
        /// <param name="value">Value object to add in ValueCollection</param>
        public ValueCollection(Value value)
        {
            if (value != null)
            {
                this._values.Add(value);
            }
        }

        /// <summary>
        /// Initialize ValueCollection from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for ValueCollection</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public ValueCollection(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadValuesFromXml(valuesAsXml, objectSerialization);
        }

        /// <summary>
        /// Initialize ValueCollection from IList of value
        /// </summary>
        /// <param name="valuesList">List of Value object</param>
        public ValueCollection(IList<Value> valuesList)
        {
            this._values = new Collection<Value>(valuesList);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Value this[Int32 index]
        {
            get
            {
                Value value = this._values[index];
                if (value == null)
                    throw new ArgumentException(String.Format("No value found for index: {0}", index), "index");
                else
                    return value;
            }
        }

        /// <summary>
        /// Indicates any of the values in current collection has invalid values.
        /// </summary>
        public Boolean HasInvalidValues
        {
            get
            {
                if (this._values != null)
                {
                    foreach (Value val in this._values)
                    {
                        if (val.HasInvalidValue == true)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            set
            {
                if (this._values != null)
                {
                    foreach (Value val in this._values)
                    {
                        val.HasInvalidValue = value;
                    }
                }
                this._hasInvalidValues = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Xml representation of ValueCollection object
        /// </summary>
        /// <returns>Xml string representing the ValueCollection</returns>
        public String ToXml()
        {
            String valuesXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (Value value in this._values)
            {
                builder.Append(value.ToXml());
            }

            valuesXml = String.Format("<Values>{0}</Values>", builder.ToString());
            return valuesXml;
        }

        /// <summary>
        /// Get Xml representation of ValueCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <param name="attributeDataType">Data type of the attribute</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization, AttributeDataType attributeDataType)
        {
            String valuesXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (Value value in this._values)
            {
                builder.Append(value.ToXml(serialization, attributeDataType));
            }

            valuesXml = String.Format("<Values>{0}</Values>", builder.ToString());
            return valuesXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is ValueCollection)
            {
                ValueCollection objectToBeCompared = obj as ValueCollection;
                Int32 valuesUnion = this._values.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 valuesIntersect = this._values.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (valuesUnion != valuesIntersect)
                    return false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            Int32 hashCode = 0;
            foreach (Value attr in this._values)
            {
                hashCode += attr.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Loads the values from given XMl and add into this
        /// </summary>
        /// <param name="valuesAsXml">Values Which needs to be added in the collection in XMl format</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public void LoadValuesFromXml(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Value")
                        {
                            String valueXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(valueXml))
                            {
                                Value val = new Value(valueXml, objectSerialization);
                                if (val != null)
                                {
                                    this.Add(val);
                                }
                            }

                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Loads ValueCollection object from xml
        /// </summary>
        /// <param name="reader">Indicates an xml reader used to read xml format data</param>		
        internal void LoadValueCollectionFromXml(XmlTextReader reader)
        {
            if (reader != null)
            {
                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Value")
                    {
                        if (reader.HasAttributes)
                        {
                            Value value = new Value();

                            // Load Value metadata from xml
                            value.LoadValueMetadataFromXml(reader);

                            this.Add(value);
                        }
                    }
                    else if ((reader.NodeType == XmlNodeType.EndElement || reader.IsEmptyElement) && reader.Name == "Values")
                    {
                        return;
                    }
                    else
                    {
                        //Keep on reading the xml until we reach expected node.
                        reader.Read();
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("reader", "XmlTextReader is null. Can not read ValueCollection object.");
            }
        }

        #endregion

        #region ICollection<Value> Members

        /// <summary>
        /// Add Value object in collection
        /// </summary>
        /// <param name="item">Value to add in collection</param>
        public void Add(Value item)
        {
            this._values.Add(item);
        }

        /// <summary>
        /// Removes all Values from collection
        /// </summary>
        public void Clear()
        {
            this._values.Clear();
        }

        /// <summary>
        /// Determines whether the ValueCollection contains a specific Value.
        /// </summary>
        /// <param name="item">The Value object to locate in the ValueCollection.</param>
        /// <returns>
        /// <para>true : If Value found in ValueCollection</para>
        /// <para>false : If Value found not in ValueCollection</para>
        /// </returns>
        public bool Contains(Value item)
        {
            return this._values.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the ValueCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from ValueCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(Value[] array, int arrayIndex)
        {
            this._values.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of Values in ValueCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._values.Count;
            }
        }

        /// <summary>
        /// Check if ValueCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the ValueCollection.
        /// </summary>
        /// <param name="item">The Value object to remove from the ValueCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original ValueCollection</returns>
        public bool Remove(Value item)
        {
            return this._values.Remove(item);
        }

        /// <summary>
        /// Remove item from value collection based on the attrval
        /// </summary>
        /// <param name="value">the attrval object to remove from the ValueCollection.</param>
        public Boolean RemoveByAttrVal(Object value)
        {
            Boolean result = false;

            if (value != null)
            {
                var filterValue = (from val in this._values where (val.AttrVal != null && val.AttrVal.ToString().Equals(value)) select val).FirstOrDefault<Value>();

                if (filterValue != null)
                {
                    result = this._values.Remove(filterValue);
                }
            }

            return result;
        }

        #endregion

        #region IEnumerable<Value> Members

        /// <summary>
        /// Returns an enumerator that iterates through a ValueCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<Value> GetEnumerator()
        {
            return this._values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a ValueCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._values.GetEnumerator();
        }

        #endregion

        #region Helper Get methods

        /// <summary>
        /// Get value from value collection based on ValueRefId
        /// </summary>
        /// <param name="valueRefId">ValueRefId to search</param>
        /// <returns>Value interface having given ValueRefId</returns>
        /// <exception cref="DuplicateObjectException">Raised if multiple values having same ValueRefId is found</exception>
        public IValue GetByValueRefId(Int32 valueRefId)
        {
            Value value = null;

            IList<Value> expectedValues = (from val in this._values
                                           where
                                               val.ValueRefId == valueRefId
                                           select val).ToList<Value>();

            if (expectedValues != null && expectedValues.Any())
            {
                if (expectedValues.Count() > 1)
                {
                    throw new DuplicateObjectException(String.Concat("Multiple values having ValueRefId = ", valueRefId, " found."));
                }
                value = expectedValues.FirstOrDefault();
            }

            return value;
        }

        /// <summary>
        /// Get value from value collection based on Sequence
        /// </summary>
        /// <param name="sequence">Sequence to search</param>
        /// <returns>Value interface having given sequence</returns>
        /// <exception cref="DuplicateObjectException">Raised if multiple values having same Sequence is found</exception>
        public IValue GetBySequence(Decimal sequence)
        {
            Value value = null;

            IList<Value> expectedValues = (from val in this._values
                                           where
                                               val.Sequence == sequence
                                           select val).ToList<Value>();

            if (expectedValues != null && expectedValues.Any())
            {
                if (expectedValues.Count() > 1)
                {
                    throw new DuplicateObjectException(String.Concat("Multiple values having sequence = ", sequence, " found."));
                }

                value = expectedValues.FirstOrDefault();
            }

            return value;
        }

        /// <summary>
        /// Get values having given locale name
        /// </summary>
        /// <param name="locale">Locale to search</param>
        /// <returns>Value collection interface having given locale name</returns>
        /// <exception cref="ArgumentNullException">Raised if locale is empty</exception>
        public IValueCollection GetByLocale(Core.LocaleEnum locale)
        {
            #region Validation

            if (locale == Core.LocaleEnum.UnKnown)
            {
                throw new ArgumentNullException("Locale");
            }

            #endregion Validation

            ValueCollection values = null;

            IList<Value> expectedValues = (from val in this._values
                                           where
                                               val.Locale == locale
                                           select val).ToList<Value>();

            if (expectedValues != null && expectedValues.Any())
            {
                values = new ValueCollection(expectedValues);
            }

            return values;
        }

        #endregion Helper Get methods

        #region IValueCollection methods

        /// <summary>
        /// Clear each value's properties from current ValueCollection
        /// </summary>
        public void ClearValues()
        {
            foreach (Value value in this._values)
            {
                value.Clear();
            }
        }

        /// <summary>
        /// Get next sequence no. Calculated from existing sequence value + 1
        /// </summary>
        /// <returns>Next sequence value (Last sequence value + 1)</returns>
        /// <exception cref="Exception">Raised when current _value is not initialized</exception>
        public Decimal GetNextSequenceValue()
        {
            if (this._values == null)
            {
                throw new Exception("No values found in current value collection");
            }

            Decimal nextSeq = -1;

            //Get last sequence no. Get distinct Sequence from current ValueCollection, sort it in descending order and get first value
            nextSeq = this._values.Select(seq => seq.Sequence).Distinct().OrderByDescending(s => s).FirstOrDefault(); ;

            //Increment by 1 to get next sequence value
            nextSeq = nextSeq + 1;

            return nextSeq;
        }

        /// <summary>
        /// Add Value object in collection
        /// </summary>
        /// <param name="item">Value to add in collection</param>
        public void Add(IValue item)
        {
            if (item != null)
            {
                this.Add((Value)item);
            }
        }

        /// <summary>
        /// Determines whether the ValueCollection contains a specific Value.
        /// </summary>
        /// <param name="item">The Value object to locate in the ValueCollection.</param>
        /// <returns>
        /// <para>true : If Value found in ValueCollection</para>
        /// <para>false : If Value found not in ValueCollection</para>
        /// </returns>
        public bool Contains(IValue item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Value");
            }

            return this.Contains((Value)item);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the ValueCollection.
        /// </summary>
        /// <param name="item">The Value object to remove from the ValueCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original ValueCollection</returns>
        public bool Remove(IValue item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Value");
            }

            return this.Remove((Value)item);
        }

        /// <summary>
        /// Set provided object action to each instance of value in the collection
        /// </summary>
        /// <param name="action"></param>
        public void SetAction(ObjectAction action)
        {
            foreach (Value val in this._values)
                val.Action = action;
        }
        #endregion IValueCollection methods
    }
}
