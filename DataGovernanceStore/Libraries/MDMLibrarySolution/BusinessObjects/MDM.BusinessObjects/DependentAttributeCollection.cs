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
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Dependent Attribute Collection. 
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class DependentAttributeCollection : ICollection<DependentAttribute>, IEnumerable<DependentAttribute>, IDependentAttributeCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting collection of Dependent Attribute
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        private Collection<DependentAttribute> _dependentAttributes = new Collection<DependentAttribute>();

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public DependentAttributeCollection()
            : base()
        {
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public DependentAttributeCollection(String valueAsXml)
        {
            LoadDependentAttributeCollection(valueAsXml);
        }

        /// <summary>
        /// Constructor which takes IList object as input
        /// </summary>
        public DependentAttributeCollection(IList<DependentAttribute> list)
        {
            this._dependentAttributes = new Collection<DependentAttribute>(list);
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
            foreach (DependentAttribute dAttr in this._dependentAttributes)
            {
                hashCode += dAttr.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Initialize DependentAttribute Collection from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for DependentAttributeCollection</param>
        public void LoadDependentAttributeCollection(String valuesAsXml)
        {

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DependentAttribute")
                        {
                            String dependentAttributeXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(dependentAttributeXml))
                            {
                                DependentAttribute dependentAttribute = new DependentAttribute(dependentAttributeXml);

                                if (dependentAttribute != null)
                                {
                                    this.Add(dependentAttribute);
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
        /// Get Xml representation of Dependent Attribute Collection
        /// </summary>
        /// <returns>Xml representation of Dependent Attribute Collection</returns>
        public String ToXml()
        {
            StringBuilder retunXml = new StringBuilder();

            if (this._dependentAttributes != null && this._dependentAttributes.Count > 0)
            {
                foreach (DependentAttribute dAttr in this._dependentAttributes)
                {
                    retunXml.Append(dAttr.ToXml());
                }
            }

            return String.Format("<DependentAttributes>{0}</DependentAttributes>", retunXml);
        }

        /// <summary>
        /// Filter only 'Dependent Of' Attributes.
        /// Will return only the child dependent attribute details
        /// </summary>
        /// <param name="items">DependentAttributeCollection which contains all(parent and child) details</param>
        /// <param name="dependencyType">Type of the dependency type</param>
        /// <returns>DependentAttributeCollection</returns>
        public DependentAttributeCollection FilterDependentAttributes(DependentAttributeCollection items, DependencyType dependencyType)
        {
            return FilterBasedOnDependencyType(items, dependencyType);
        }

        /// <summary>
        /// Filter only 'Dependent Of' Attributes.
        /// Will return only the child dependent attribute details
        /// </summary>
        /// <param name="dependencyType">Type of the dependency type</param>
        /// <returns>DependentAttributeCollection</returns>
        public DependentAttributeCollection FilterDependentAttributes(DependencyType dependencyType)
        {
            return FilterBasedOnDependencyType(dependencyType);
        }

        /// <summary>
        /// Filter only 'Dependent Of' Attributes.
        /// Will return only the child dependent attribute details
        /// </summary>
        /// <param name="items">DependentAttributeCollection which contains all(parent and child) details</param>
        /// <param name="dependencyType">Type of the dependency type</param>
        /// <returns>IDependentAttributeCollection</returns>
        public IDependentAttributeCollection FilterDependentAttributes(IDependentAttributeCollection items, DependencyType dependencyType)
        {
            IDependentAttributeCollection result = null;

            if (items != null)
            {
                result = FilterBasedOnDependencyType((DependentAttributeCollection)items, dependencyType);
            }

            return result;

        }

        /// <summary>
        /// Get the list of Dependency attribute Id list.
        /// </summary>
        /// <returns>Returns list of dependency attribute identifiers</returns>
        public List<Int32> GetDepencyAttributeIdList()
        {
            List<Int32> dependencyAttributeIdlist = new List<Int32>();  //If there no dependency attribute then return empty attribute id list.

            if (this._dependentAttributes != null && this._dependentAttributes.Count > 0)
            {
                foreach (DependentAttribute attr in this._dependentAttributes)
                {
                    dependencyAttributeIdlist.Add(attr.AttributeId);
                }
            }

            return dependencyAttributeIdlist;
        }

        /// <summary>
        /// Get dependent attribute based on given attribute id
        /// </summary>
        /// <param name="attributeId">Indicates identifier of attribute which need to filter on</param>
        /// <returns>Returns dependent attribute for given attribute id</returns>
        public IDependentAttribute GetDependentAttribute(Int32 attributeId)
        {
            DependentAttribute dependentAttribute = new DependentAttribute();

            if (this._dependentAttributes != null && this._dependentAttributes.Count > 0)
            {
                var result = from attr in this._dependentAttributes where attr.AttributeId == attributeId select attr;

                if (result != null && result.Count() > 0)
                {
                    dependentAttribute = result.FirstOrDefault<DependentAttribute>();
                }
            }

            return dependentAttribute;
        }

        /// <summary>
        /// Get dependency based on Dependency Id
        /// </summary>
        /// <param name="Id">Id to find in collection</param>
        /// <returns></returns>
		public IDependentAttribute GetDependentAttributeByDependencyId(Int32 Id)
        {
            DependentAttribute dependentAttribute = null;

            if (this._dependentAttributes != null && this._dependentAttributes.Count > 0)
            {
                var result = from attr in this._dependentAttributes where attr.Id == Id select attr;

                if (result != null && result.Count() > 0)
                {
                    dependentAttribute = result.FirstOrDefault<DependentAttribute>();
                }
            }

            return dependentAttribute;
        }

        /// <summary>
        /// Get dependent attribute based on given name of link table
        /// </summary>
        /// <param name="linkTableName">Indicates name of link table which need to filter on</param>
        /// <returns>Returns dependent attribute for given name of link table</returns>
        public DependentAttribute GetDependentAttribute(String linkTableName)
        {
            DependentAttribute dependentAttribute = null;

            if (this._dependentAttributes != null && this._dependentAttributes.Count > 0)
            {
                var result = from attr in this._dependentAttributes where attr.LinkTableName == linkTableName select attr;

                if (result != null && result.Count() > 0)
                {
                    dependentAttribute = result.FirstOrDefault<DependentAttribute>();
                }
            }

            return dependentAttribute;
        }
        #endregion

        #region Private Methods

        private DependentAttributeCollection FilterBasedOnDependencyType(DependentAttributeCollection items, DependencyType dependencyType)
        {
            DependentAttributeCollection dAttrs = new DependentAttributeCollection();

            if (items != null && items.Count > 0)
            {
                var result = items.Where(da => da.DependencyType == dependencyType);

                if (result != null && result.Count() > 0)
                {
                    foreach (DependentAttribute item in result)
                    {
                        dAttrs.Add(item);
                    }
                }
            }

            return dAttrs;
        }

        private DependentAttributeCollection FilterBasedOnDependencyType(DependencyType dependencyType)
        {
            DependentAttributeCollection result = new DependentAttributeCollection();

            if (this._dependentAttributes != null && this._dependentAttributes.Count > 0)
            {
                var filterResult = from attr in this._dependentAttributes where attr.DependencyType == dependencyType select attr;

                if (filterResult != null && filterResult.Count() > 0)
                {
                    foreach (DependentAttribute item in filterResult)
                    {
                        result.Add(item);
                    }
                }
            }

            return result;
        }

        #endregion

        #endregion

        #region ICollection<DependentAttribute> Members

        /// <summary>
        /// Add 'Dependent attribute' object in collection
        /// </summary>
        /// <param name="item">Dependent attribute to add in collection</param>
        public void Add(DependentAttribute item)
        {
            this._dependentAttributes.Add(item);
        }

        /// <summary>
        /// Add Dependent Attributes object in collection
        /// </summary>
        /// <param name="items">Dependent AttributeCollection to add in collection</param>
        public void AddRange(DependentAttributeCollection items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("DependentAttributes");
            }

            foreach (DependentAttribute dattr in items)
            {
                this.Add(dattr);
            }
        }

        /// <summary>
        /// Add IDependentAttributes object in collection
        /// </summary>
        /// <param name="items">IDependentAttributeCollection to add in collection</param>
        public void AddRange(IDependentAttributeCollection items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("DependentAttributes");
            }

            foreach (IDependentAttribute dattr in items)
            {
                this.Add(dattr);
            }
        }

        /// <summary>
        /// Add 'IDependentattribute' object in collection
        /// </summary>
        /// <param name="item">Dependent attribute to add in collection</param>
        public void Add(IDependentAttribute item)
        {
            this.Add((DependentAttribute)item);
        }

        /// <summary>
        /// Removes all Dependent Attribute from collection
        /// </summary>
        public void Clear()
        {
            this._dependentAttributes.Clear();
        }

        /// <summary>
        /// Determines whether the DependentAttributeCollection contains a specific DependentAttribute.
        /// </summary>
        /// <param name="item">The DependentAttribute object to locate in the DependentAttributeCollection.</param>
        /// <returns>
        /// <para>true : If DependentAttribute found in DependentAttributeCollection</para>
        /// <para>false : If DependentAttribute found not in DependentAttributeCollection</para>
        /// </returns>
        public bool Contains(DependentAttribute item)
        {
            return this._dependentAttributes.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the DependentAttributeCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from DependentAttributeCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(DependentAttribute[] array, int arrayIndex)
        {
            this._dependentAttributes.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of DependentAttribute in DependentAttributeCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._dependentAttributes.Count;
            }
        }

        /// <summary>
        /// Check if DependentAttributeCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the DependentAttributeCollection.
        /// </summary>
        /// <param name="item">The DependentAttribute object to remove from the DependentAttributeCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original DependentAttributeCollection</returns>
        public bool Remove(DependentAttribute item)
        {
            return this._dependentAttributes.Remove(item);
        }

        /// <summary>
        /// Clones the curent instance to a newer instance. 
        /// </summary>
        /// <param name="includeValue">Specified whether the value part of the object needs to be included in the clone or not</param>
        /// <returns></returns>
        public IDependentAttributeCollection Clone(Boolean includeValue)
        {
            var clonedDependentAttributes = new DependentAttributeCollection();

            if (this._dependentAttributes != null)
            {
                foreach (DependentAttribute attr in this._dependentAttributes)
                {
                    clonedDependentAttributes.Add(attr.Clone(includeValue));
                }
            }

            return clonedDependentAttributes;
        }

        #endregion

        #region IEnumerable<DependentAttribute> Members

        /// <summary>
        /// Returns an enumerator that iterates through a DependentAttributeCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<DependentAttribute> GetEnumerator()
        {
            return this._dependentAttributes.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a DependentAttributeCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._dependentAttributes.GetEnumerator();
        }

        #endregion
    }
}
