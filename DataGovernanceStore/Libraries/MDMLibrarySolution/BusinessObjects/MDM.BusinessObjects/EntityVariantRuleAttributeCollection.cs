using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies a collection of rule attribute for entity variant definition.
    /// </summary>
    [DataContract]
    public class EntityVariantRuleAttributeCollection : ICollection<EntityVariantRuleAttribute>, IEnumerable<EntityVariantRuleAttribute>, IEntityVariantRuleAttributeCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting collection of entity variant rule attribute object
        /// </summary>
        [DataMember]
        private Collection<EntityVariantRuleAttribute> _entityVariantRuleAttributeCollection = new Collection<EntityVariantRuleAttribute>();

        #endregion

        #region Properties

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public EntityVariantRuleAttributeCollection()
        {
        }

        /// <summary>
        /// Constructor with rule attribute list as its input parameters
        /// </summary>
        /// <param name="ruleAttributeList">Indicates list of rule attribute</param>
        public EntityVariantRuleAttributeCollection(IList<EntityVariantRuleAttribute> ruleAttributeList)
        {
            if (ruleAttributeList != null)
            {
                this._entityVariantRuleAttributeCollection = new Collection<EntityVariantRuleAttribute>(ruleAttributeList);
            }
        }
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Xml representation of entity variant rule attribute collection
        /// </summary>
        /// <returns>Xml representation of entity variant rule attribute collection</returns>
        public String ToXml(bool needvalues)
        {
            String returnXml = String.Empty;

            returnXml = "<EntityVariantRuleAttributes>";

            if(this._entityVariantRuleAttributeCollection != null && this._entityVariantRuleAttributeCollection.Count > 0)
            {
                foreach(EntityVariantRuleAttribute ruleAttribute in this._entityVariantRuleAttributeCollection)
                {
                    returnXml = String.Concat(returnXml, ruleAttribute.ToXml(needvalues));
                }
            }

            returnXml = String.Concat(returnXml, "</EntityVariantRuleAttributes>");

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of Entity variant rule attribute collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Indicates type of the serialization option</param>
        /// <param name="needValues">Represents a boolean value indicating if attribute value details are required in xml representation</param>
        /// <returns>Returns Xml representation of Entity variant rule attribute collection</returns>
        public String ToXml(ObjectSerialization objectSerialization, bool needValues)
        {
            String returnXml = String.Empty;

            returnXml = "<EntityVariantRuleAttributes>";

            if(this._entityVariantRuleAttributeCollection != null && this._entityVariantRuleAttributeCollection.Count > 0)
            {
                foreach(EntityVariantRuleAttribute attrModel in this._entityVariantRuleAttributeCollection)
                {
                    returnXml = String.Concat(returnXml, attrModel.ToXml(objectSerialization, needValues));
                }
            }

            returnXml = String.Concat(returnXml, "</EntityVariantRuleAttributes>");

            return returnXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">Indicates the Object to compare with the current Object.</param>
        /// <returns>Returns true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if(obj is EntityVariantRuleAttributeCollection)
            {
                EntityVariantRuleAttributeCollection objectToBeCompared = obj as EntityVariantRuleAttributeCollection;
                Int32 EntityVariantRuleAttributesUnion = this._entityVariantRuleAttributeCollection.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 EntityVariantRuleAttributesIntersect = this._entityVariantRuleAttributeCollection.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if(EntityVariantRuleAttributesUnion != EntityVariantRuleAttributesIntersect)
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

            foreach(EntityVariantRuleAttribute EntityVariantRuleAttribute in this._entityVariantRuleAttributeCollection)
            {
                hashCode += EntityVariantRuleAttribute.GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="subsetEntityVariantRuleAttributeCollection">Indicates the object to compare with the current object.</param>
        /// <param name="compareIds">Indicates flag to determine whether id based comparison is true or not</param>
        /// <returns>Returns true if the specified object is equal to the current object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(EntityVariantRuleAttributeCollection subsetEntityVariantRuleAttributeCollection, Boolean compareIds = false)
        {
            if (subsetEntityVariantRuleAttributeCollection != null && subsetEntityVariantRuleAttributeCollection.Count > 0)
            {
                foreach (EntityVariantRuleAttribute childEntityVariantRuleAttribute in subsetEntityVariantRuleAttributeCollection)
                {
                    if (this._entityVariantRuleAttributeCollection != null && this._entityVariantRuleAttributeCollection.Count > 0)
                    {
                        foreach (EntityVariantRuleAttribute sourceEntityVariantRuleAttribute in this._entityVariantRuleAttributeCollection)
                        {
                            if (sourceEntityVariantRuleAttribute.IsSuperSetOf(childEntityVariantRuleAttribute, compareIds))
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Clone entity variant rule attribute collection object
        /// </summary>
        /// <returns>Returns cloned entity variant rule attribute collection instance</returns>
        public IEntityVariantRuleAttributeCollection Clone()
        {
            EntityVariantRuleAttributeCollection clonedEntityVariantRuleAttributeCollection = new EntityVariantRuleAttributeCollection();

            if (this._entityVariantRuleAttributeCollection != null && this._entityVariantRuleAttributeCollection.Count > 0)
            {
                foreach (EntityVariantRuleAttribute entityVariantRuleAttribute in this._entityVariantRuleAttributeCollection)
                {
                    IEntityVariantRuleAttribute clonedIEntityVariantRuleAttribute = entityVariantRuleAttribute.Clone();
                    clonedEntityVariantRuleAttributeCollection.Add(clonedIEntityVariantRuleAttribute as EntityVariantRuleAttribute);
                }
            }

            return clonedEntityVariantRuleAttributeCollection;
        }

        /// <summary>
        /// Get entity variant rule attributes by name
        /// </summary>
        /// <param name="attributeName">Indicates entity variant rule attribute name</param>
        /// <returns>Returns entity variant rule attribute for given attribute name</returns>
        public EntityVariantRuleAttribute GetByName(String attributeName)
        {
            if (String.IsNullOrWhiteSpace(attributeName))
            {
                foreach (EntityVariantRuleAttribute entityVariantRuleAttribute in this._entityVariantRuleAttributeCollection)
                {
                    if (String.Compare(entityVariantRuleAttribute.Name, attributeName) == 0)
                    {
                        return entityVariantRuleAttribute;
                    }
                }
            }

            return null;
        }

        #endregion

        #region Private Methods

        #endregion

        #region ICollection<EntityVariantRuleAttribute> Members

        /// <summary>
        /// Add Entity variant rule attribute object in collection
        /// </summary>
        /// <param name="item">Indicates the entity variant rule attribute to add in collection</param>
        public void Add(EntityVariantRuleAttribute item)
        {
            this._entityVariantRuleAttributeCollection.Add(item);
        }

        /// <summary>
        /// Removes all entity variant rule attribute from collection
        /// </summary>
        public void Clear()
        {
            this._entityVariantRuleAttributeCollection.Clear();
        }

        /// <summary>
        /// Determines whether the entity variant rule attribute collection contains a specific Entity variant rule attribute.
        /// </summary>
        /// <param name="EntityVariantRuleAttribute">Indicates the entity variant rule attribute object to locate in the EntityVariantRuleAttributeCollection.</param>
        /// <returns>
        /// <para>true : If entity variant rule attribute found in EntityVariantRuleAttributeCollection</para>
        /// <para>false : If entity variant rule attribute found not in EntityVariantRuleAttributeCollection</para>
        /// </returns>
        public bool Contains(EntityVariantRuleAttribute EntityVariantRuleAttribute)
        {
            return this._entityVariantRuleAttributeCollection.Contains(EntityVariantRuleAttribute);
        }

        /// <summary>
        /// Copies the elements of the EntityVariantRuleAttributeCollection to an
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from EntityVariantRuleAttributeCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(EntityVariantRuleAttribute[] array, int arrayIndex)
        {
            this._entityVariantRuleAttributeCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of mappings in EntityVariantRuleAttributeCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._entityVariantRuleAttributeCollection.Count;
            }
        }

        /// <summary>
        /// Check if EntityVariantRuleAttributeCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific entity variant rule attribute from the EntityVariantRuleAttributeCollection.
        /// </summary>
        /// <param name="EntityVariantRuleAttribute">Indicates the Entity variant rule attribute object to remove from the EntityVariantRuleAttributeCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(EntityVariantRuleAttribute EntityVariantRuleAttribute)
        {
            return this._entityVariantRuleAttributeCollection.Remove(EntityVariantRuleAttribute);
        }

        /// <summary>
        /// Get rule attribute by target attribute id
        /// </summary>
        /// <param name="targetAttribute">Indicates target attribute id</param>
        /// <returns>Returns the entity variant rule attributes based on target attribute id</returns>
        public EntityVariantRuleAttributeCollection GetRuleAttributeByTargetAttributeId(Int32 targetAttribute)
        {
            EntityVariantRuleAttributeCollection ruleAttr = null;

            ruleAttr = new EntityVariantRuleAttributeCollection((from rulAttrList in this._entityVariantRuleAttributeCollection
                                                                    where rulAttrList.TargetAttributeId == targetAttribute
                                                                    select rulAttrList).ToList<EntityVariantRuleAttribute>());

            return ruleAttr;
        }

        #endregion

        #region IEnumerable<EntityVariantRuleAttribute> Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityVariantRuleAttributeCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<EntityVariantRuleAttribute> GetEnumerator()
        {
            return this._entityVariantRuleAttributeCollection.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityVariantRuleAttributeCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ( IEnumerator ) this._entityVariantRuleAttributeCollection.GetEnumerator();
        }

        #endregion

        #endregion
    }
}
