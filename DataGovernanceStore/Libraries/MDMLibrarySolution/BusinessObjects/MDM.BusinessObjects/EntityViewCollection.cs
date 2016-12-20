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

    /// <summary>
    /// Specifies Entity View Collection
    /// </summary>
    [DataContract]
    public class EntityViewCollection : ICollection<EntityView>, IEnumerable<EntityView>, IEntityViewCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting collection of entity view
        /// </summary>
        [DataMember]
        private Collection<EntityView> _entityViews = new Collection<EntityView>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Entity View Collection
        /// </summary>
        public EntityViewCollection() : base() { }

        #endregion

        #region Properties

        /// <summary>
        /// Find entity view from collection based on view UniqueIdentifier
        /// </summary>
        /// <param name="uniqueIdentifier">
        ///     UniqueIdentifier using which entity view is to be searched from collection
        /// </param>
        /// <returns></returns>
        public EntityView this[String uniqueIdentifier]
        {
            get
            {
                EntityView entityView = GetEntityView(uniqueIdentifier);

                if (entityView == null)
                    throw new ArgumentException("No entity view found for given unique identifier");
                else
                    return entityView;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Check if EntityViewCollection contains entity view with given unique identifier
        /// </summary>
        /// <param name="uniqueIdentifier">UniqueIdentifier using which entity view is to be searched from collection</param>
        /// <returns>
        /// <para>true : If entity view found in EntityViewCollection</para>
        /// <para>false : If entity view found not in EntityViewCollection</para>
        /// </returns>
        public bool Contains(String uniqueIdentifier)
        {
            if (GetEntityView(uniqueIdentifier) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove entity view object from EntityViewCollection
        /// </summary>
        /// <param name="uniqueIdentifier">Unique identifier of entity view which is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(String uniqueIdentifier)
        {
            EntityView entityView = GetEntityView(uniqueIdentifier);

            if (entityView == null)
                throw new ArgumentException("No entity view found for given unique identifier");
            else
                return this.Remove(entityView);
        }

        /// <summary>
        /// Get Xml representation of Entity View Collection
        /// </summary>
        /// <returns>Xml representation of Entity View Collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<EntityViews>";

            if (this._entityViews != null && this._entityViews.Count > 0)
            {
                foreach (EntityView view in this._entityViews)
                {
                    returnXml = String.Concat(returnXml, view.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</EntityViews>");

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of Entity View Collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Entity View Collection</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String returnXml = String.Empty;

            returnXml = "<EntityViews>";

            if (this._entityViews != null && this._entityViews.Count > 0)
            {
                foreach (EntityView view in this._entityViews)
                {
                    returnXml = String.Concat(returnXml, view.ToXml(objectSerialization));
                }
            }

            returnXml = String.Concat(returnXml, "</EntityViews>");

            return returnXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is EntityViewCollection)
            {
                EntityViewCollection objectToBeCompared = obj as EntityViewCollection;

                Int32 entityViewsUnion = this._entityViews.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 entityViewsIntersect = this._entityViews.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if (entityViewsUnion != entityViewsIntersect)
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

            foreach (EntityView entityView in this._entityViews)
            {
                hashCode += entityView.GetHashCode();
            }

            return hashCode;
        }

        #endregion

        #region Private Methods

        private EntityView GetEntityView(String uniqueIdentifier)
        {
            var filteredViews = from view in this._entityViews
                                     where view.UniqueIdentifier == uniqueIdentifier
                                     select view;

            if (filteredViews.Any())
                return filteredViews.First();
            else
                return null;
        }

        #endregion

        #region ICollection<AttributeModel> Members

        /// <summary>
        /// Add entity view object in collection
        /// </summary>
        /// <param name="item">entity view to add in collection</param>
        public void Add(EntityView item)
        {
            this._entityViews.Add(item);
        }

        /// <summary>
        /// Removes all entity view from collection
        /// </summary>
        public void Clear()
        {
            this._entityViews.Clear();
        }

        /// <summary>
        /// Determines whether the EntityViewCollection contains a specific entity view
        /// </summary>
        /// <param name="item">The entity view object to locate in the EntityViewCollection.</param>
        /// <returns>
        /// <para>true : If entity view found in EntityViewCollection</para>
        /// <para>false : If entity view found not in EntityViewCollection</para>
        /// </returns>
        public bool Contains(EntityView item)
        {
            return this._entityViews.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the EntityViewCollection to a
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from AttributeModelCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(EntityView[] array, int arrayIndex)
        {
            this._entityViews.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of entity views in EntityViewCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._entityViews.Count;
            }
        }

        /// <summary>
        /// Check if EntityViewCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific entity view from the EntityViewCollection.
        /// </summary>
        /// <param name="item">The entity view object to remove from the EntityViewCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(EntityView item)
        {
            return this._entityViews.Remove(item);
        }

        #endregion

        #region IEnumerable<EntityView> Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityViewCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<EntityView> GetEnumerator()
        {
            return this._entityViews.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityViewCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._entityViews.GetEnumerator();
        }

        #endregion
    }
}
