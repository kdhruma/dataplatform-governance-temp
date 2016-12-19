using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Collections;
using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the AttributeUniqueIdentifier Instance Collection for the Object
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class AttributeUniqueIdentifierCollection : ICollection<AttributeUniqueIdentifier>, IEnumerable<AttributeUniqueIdentifier>, IAttributeUniqueIdentifierCollection
    {
        #region Fields

        [DataMember]
        [ProtoMember(1)]
        private Collection<AttributeUniqueIdentifier> _attributeUniqueIdentifiers = new Collection<AttributeUniqueIdentifier>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter-less constructor
        /// </summary>
        public AttributeUniqueIdentifierCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public AttributeUniqueIdentifierCollection(String valuesAsXml)
        {
            LoadAttributeUniqueIdentifierCollection(valuesAsXml);
        }

        /// <summary>
        /// Initialize AttributeUniqueIdentifierCollection from IList of value
        /// </summary>
        /// <param name="attributeUniqueIdentifierCollectionList">List of AttributeUniqueIdentifier object</param>
        public AttributeUniqueIdentifierCollection(IList<AttributeUniqueIdentifier> attributeUniqueIdentifierCollectionList)
        {
            this._attributeUniqueIdentifiers = new Collection<AttributeUniqueIdentifier>(attributeUniqueIdentifierCollectionList);
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Xml representation of AttributeUniqueIdentifierCollection object
        /// </summary>
        /// <returns>Xml string representing the AttributeUniqueIdentifierCollection</returns>
        public String ToXml()
        {
            String attributeUniqueIdentifiersXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (AttributeUniqueIdentifier attributeUniqueIdentifier in this._attributeUniqueIdentifiers)
            {
                builder.Append(attributeUniqueIdentifier.ToXml());
            }

            attributeUniqueIdentifiersXml = String.Format("<AttributeUniqueIdentifiers>{0}</AttributeUniqueIdentifiers>", builder.ToString());

            return attributeUniqueIdentifiersXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is AttributeUniqueIdentifierCollection)
            {
                AttributeUniqueIdentifierCollection objectToBeCompared = obj as AttributeUniqueIdentifierCollection;

                Int32 attributeUniqueIdentifiersUnion = this._attributeUniqueIdentifiers.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 attributeUniqueIdentifiersIntersect = this._attributeUniqueIdentifiers.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if (attributeUniqueIdentifiersUnion != attributeUniqueIdentifiersIntersect)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            Int32 hashCode = 0;

            foreach (AttributeUniqueIdentifier attributeUniqueIdentifier in this._attributeUniqueIdentifiers)
            {
                hashCode += attributeUniqueIdentifier.GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Load current AttributeUniqueIdentifierCollection from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for current AttributeUniqueIdentifierCollection
        /// </param>
        public void LoadAttributeUniqueIdentifierCollection(String valuesAsXml)
        {
            #region Sample Xml
            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeUniqueIdentifier")
                        {
                            String attributeUniqueIdentifierXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(attributeUniqueIdentifierXml))
                            {
                                AttributeUniqueIdentifier attributeUniqueIdentifier = new AttributeUniqueIdentifier(attributeUniqueIdentifierXml);

                                if (attributeUniqueIdentifier != null)
                                {
                                    this.Add(attributeUniqueIdentifier);
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
        /// 
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeGroupName"></param>
        /// <returns></returns>
        public Boolean Contains(String attributeName, String attributeGroupName)
        {
            return Get(attributeName, attributeGroupName) != null;
        }

        /// <summary>
        /// Get attribute names
        /// </summary>
        /// <returns></returns>
        public Collection<String> GetAttributeNames()
        {
            var attributeNames = new Collection<String>();

            if (_attributeUniqueIdentifiers != null && _attributeUniqueIdentifiers.Count > 0)
            {
                foreach (var attributeUniqueIdentifier in _attributeUniqueIdentifiers)
                {
                    if (!attributeNames.Contains(attributeUniqueIdentifier.AttributeName))
                    {
                        attributeNames.Add(attributeUniqueIdentifier.AttributeName);
                    }
                }
            }

            return attributeNames;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeGroupName"></param>
        /// <returns></returns>
        private AttributeUniqueIdentifier Get(String attributeName, String attributeGroupName)
        {
            AttributeUniqueIdentifier matchedAttributeUniqueIdentifier = null;

            foreach (var attributeUniqueIdentifier in _attributeUniqueIdentifiers)
            {
                if (attributeUniqueIdentifier.AttributeName.Equals(attributeName, StringComparison.InvariantCultureIgnoreCase)
                    && attributeUniqueIdentifier.AttributeGroupName.Equals(attributeGroupName, StringComparison.InvariantCultureIgnoreCase))
                {
                    matchedAttributeUniqueIdentifier = attributeUniqueIdentifier;
                    break;
                }
            }

            return matchedAttributeUniqueIdentifier;
        }

        #endregion Private Methods

        #region ICollection<AttributeUniqueIdentifier> Members

        /// <summary>
        /// Add a list of attributeUniqueIdentifier objects to the collection.
        /// </summary>
        /// <param name="items">Indicates list of attributeUniqueIdentifier objects</param>
        public void Add(List<AttributeUniqueIdentifier> items)
        {
            foreach (AttributeUniqueIdentifier item in items)
            {
                this.Add(item);
            }
        }

        /// <summary>
        /// Add attributeUniqueIdentifier in collection
        /// </summary>
        /// <param name="iAttributeUniqueIdentifier">attributeUniqueIdentifier to add in collection</param>
        public void Add(IAttributeUniqueIdentifier iAttributeUniqueIdentifier)
        {
            if (iAttributeUniqueIdentifier != null)
            {
                this.Add((AttributeUniqueIdentifier)iAttributeUniqueIdentifier);
            }
        }

        /// <summary>
        /// Add attributeUniqueIdentifier in collection
        /// </summary>
        /// <param name="item">attributeUniqueIdentifier to add in collection</param>
        public void Add(AttributeUniqueIdentifier item)
        {
            _attributeUniqueIdentifiers.Add(item);
        }

        /// <summary>
        /// Removes all items from the AttributeUniqueIdentifierCollection
        /// </summary>
        public void Clear()
        {
            _attributeUniqueIdentifiers.Clear();
        }

        /// <summary>
        /// Determines whether the AttributeUniqueIdentifierCollection contains a specific value
        /// </summary>
        /// <param name="item">attributeUniqueIdentifier Object</param>
        /// <returns> true if AttributeUniqueIdentifier is found in the AttributeUniqueIdentifierCollection; otherwise, false.</returns>
        public Boolean Contains(AttributeUniqueIdentifier item)
        {
            return Get(item.AttributeName, item.AttributeGroupName) != null;
        }

        /// <summary>
        /// Copies to
        /// </summary>
        /// <param name="array">The array</param>
        /// <param name="arrayIndex">Index of the array</param>
        public void CopyTo(AttributeUniqueIdentifier[] array, Int32 arrayIndex)
        {
            _attributeUniqueIdentifiers.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of elements contained in the AttributeUniqueIdentifierCollection
        /// </summary>
        public Int32 Count
        {
            get { return _attributeUniqueIdentifiers.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the AttributeUniqueIdentifierCollection is read-only
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the AttributeUniqueIdentifierCollection
        /// </summary>
        /// <param name="item">The AttributeUniqueIdentifier to remove from the AttributeUniqueIdentifierCollection</param>
        /// <returns>True if AttributeUniqueIdentifier was successfully removed from the AttributeUniqueIdentifierCollection; otherwise, false. This method also returns false if AttributeUniqueIdentifier is not found in the original AttributeUniqueIdentifierCollection</returns>
        public Boolean Remove(AttributeUniqueIdentifier item)
        {
            return _attributeUniqueIdentifiers.Remove(item);
        }

        #endregion

        #region IEnumerable<AttributeUniqueIdentifier> Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An AttributeUniqueIdentifier object that can be used to iterate through the collection</returns>
        public IEnumerator<AttributeUniqueIdentifier> GetEnumerator()
        {
            return _attributeUniqueIdentifiers.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An AttributeUniqueIdentifier object that can be used to iterate through the collection</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #endregion Methods
    }
}