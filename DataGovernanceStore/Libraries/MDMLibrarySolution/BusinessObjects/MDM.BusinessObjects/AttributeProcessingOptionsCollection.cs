using System;
using System.Xml;
using System.Text;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.Core.Exceptions;

    /// <summary>
    /// Specifies Attribute Options Collection
    /// </summary>
    [DataContract]
    public class AttributeProcessingOptionsCollection : ICollection<AttributeProcessingOptions>, IEnumerable<AttributeProcessingOptions>
    {
        #region Fields

        /// <summary>
        /// Field denoting collection of attribute Options
        /// </summary>
        [DataMember]
        private Collection<AttributeProcessingOptions> _attributeProcessingOptions = new Collection<AttributeProcessingOptions>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter-less constructor
        /// </summary>
        public AttributeProcessingOptionsCollection() : base() { }

        /// <summary>
        /// Constructor for attribute processing options collection from xml
        /// </summary>
        public AttributeProcessingOptionsCollection(String AttributeProcessingOptionsCollectionAsXML)
        {
            LoadAttributeProcessingOptionsCollection(AttributeProcessingOptionsCollectionAsXML);
        }

        #endregion Constructors

        #region Properties
        /// <summary>
        /// Find AttributeProcessingOptions from AttributeProcessingOptionsCollection based on attributeId
        /// </summary>
        /// <param name="attributeName">Attribute name</param>
        /// <returns>Attribute Processing Options object having given attributeName</returns>
        public AttributeProcessingOptions this[String attributeName]
        {
            get
            {
                AttributeProcessingOptions attributeProcessingOptions = null;

                foreach (AttributeProcessingOptions attributeProcessingOption in _attributeProcessingOptions)
                {
                    if (attributeProcessingOption.AttributeName == attributeName)
                    {
                        attributeProcessingOptions = attributeProcessingOption;
                    }
                }

                if (attributeProcessingOptions == null)
                    throw new ArgumentException("No attribute processing options found for given attribute name");
                else
                    return attributeProcessingOptions;
            }
        }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Get xml representation of attribute processing options collection
        /// </summary>
        /// <returns>Xml representation of attribute processing options collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<AttributeProcessingOptionsCollection>";

            if (this._attributeProcessingOptions != null && this._attributeProcessingOptions.Count > 0)
            {
                foreach (AttributeProcessingOptions attributeProcessingOptions in this._attributeProcessingOptions)
                {
                    returnXml = String.Concat(returnXml, attributeProcessingOptions.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</AttributeProcessingOptionsCollection>");

            return returnXml;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Load current attribute processing options from xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for current attribute processing options collection
        /// </param>
        private void LoadAttributeProcessingOptionsCollection(String valuesAsXml)
        {
            #region Sample Xml
            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeProcessingOptions")
                        {
                            String attributeProcessingOptionsInXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(attributeProcessingOptionsInXml))
                            {
                                AttributeProcessingOptions attributeProcessingOptions = new AttributeProcessingOptions(attributeProcessingOptionsInXml);

                                this.Add(attributeProcessingOptions);
                            }
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
            }
        }

        #endregion

        #region ICollection<AttributeModel> Members

        /// <summary>
        /// Add attribute option object in collection
        /// </summary>
        /// <param name="item">attribute option to add in collection</param>
        /// <exception cref="DuplicateObjectException"></exception>
        public void Add(AttributeProcessingOptions item)
        {
            this._attributeProcessingOptions.Add(item);
        }


        /// <summary>
        /// Removes all attribute model from collection
        /// </summary>
        public void Clear()
        {
            this._attributeProcessingOptions.Clear();
        }

        /// <summary>
        /// Determines whether the AttributeProcessingOptionsCollection contains a specific attribute processing option.
        /// </summary>
        /// <param name="item">The attribute model object to locate in the AttributeProcessingOptionsCollection.</param>
        /// <returns>
        /// <para>true : If attribute option found in AttributeProcessingOptionsCollection</para>
        /// <para>false : If attribute option found not in AttributeProcessingOptionsCollection</para>
        /// </returns>
        public bool Contains(AttributeProcessingOptions item)
        {
            return this._attributeProcessingOptions.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the AttributeModelCollection to an
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from AttributeProcessingOptionsCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(AttributeProcessingOptions[] array, int arrayIndex)
        {
            this._attributeProcessingOptions.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of attributes in AttributeProcessingOptionsCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._attributeProcessingOptions.Count;
            }
        }

        /// <summary>
        /// Check if AttributeProcessingOptionsCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific attribute model from the AttributeProcessingOptionsCollection.
        /// </summary>
        /// <param name="item">The attribute option object to remove from the AttributeProcessingOptionsCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(AttributeProcessingOptions item)
        {
            return this._attributeProcessingOptions.Remove(item);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the Attribute model collection
        /// </summary>
        /// <param name="iAttributeModel">The object to remove from the Attribute Model collection</param>
        /// <returns>true if item is successfully removed; otherwise, false</returns>
        public Boolean Remove(IAttributeModel iAttributeModel)
        {
            return this.Remove((AttributeProcessingOptions)iAttributeModel);
        }

        #endregion

        #region IEnumerable<AttributeOption> Members

        /// <summary>
        /// Returns an enumerator that iterates through a AttributeProcessingOptionsCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<AttributeProcessingOptions> GetEnumerator()
        {
            return this._attributeProcessingOptions.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a AttributeOptionCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._attributeProcessingOptions.GetEnumerator();
        }

        #endregion

        #region IAttributeModelCollection Members

        #endregion IAttributeModelCollection Members

        #region Private Methods


        #endregion
    }
}