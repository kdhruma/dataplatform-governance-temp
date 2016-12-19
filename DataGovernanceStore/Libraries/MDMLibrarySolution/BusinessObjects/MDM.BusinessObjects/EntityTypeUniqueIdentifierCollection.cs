using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    
    /// <summary>
    /// Specifies the EntityTypeUniqueIdentifier Instance Collection for the Object
    /// </summary>
    [DataContract]
    [System.Obsolete("EntityTypeUniqueIdentifierCollection is no longer needed as EntityType short name is enough to identify entityType uniquely.")]
    public class EntityTypeUniqueIdentifierCollection : InterfaceContractCollection<IEntityTypeUniqueIdentifier, EntityTypeUniqueIdentifier>, IEntityTypeUniqueIdentifierCollection
    {
        #region Fields
        #endregion

        #region Constructors

        /// <summary>
        /// Parameter-less constructor
        /// </summary>
        public EntityTypeUniqueIdentifierCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public EntityTypeUniqueIdentifierCollection(String valuesAsXml)
        {
            LoadEntityTypeUniqueIdentifierCollection(valuesAsXml);
        }

        /// <summary>
        /// Initialize entityTypeUniqueIdentifierCollection from IList of value
        /// </summary>
        /// <param name="entityTypeUniqueIdentifierCollectionList">List of entityTypeUniqueIdentifier object</param>
        public EntityTypeUniqueIdentifierCollection(IList<EntityTypeUniqueIdentifier> entityTypeUniqueIdentifierCollectionList)
        {
            base._items = new Collection<EntityTypeUniqueIdentifier>(entityTypeUniqueIdentifierCollectionList);
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Xml representation of entityTypeUniqueIdentifierCollection object
        /// </summary>
        /// <returns>Xml string representing the entityTypeUniqueIdentifierCollection</returns>
        public String ToXml()
        {
            String entityTypeUniqueIdentifiersXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            if (base._items != null && base._items.Count > 0)
            {
                foreach (EntityTypeUniqueIdentifier entityTypeUniqueIdentifier in base._items)
                {
                    builder.Append(entityTypeUniqueIdentifier.ToXml());
                }
            }
            entityTypeUniqueIdentifiersXml = String.Format("<EntityTypeUniqueIdentifier>{0}</EntityTypeUniqueIdentifier>", builder.ToString());

            return entityTypeUniqueIdentifiersXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is EntityTypeUniqueIdentifierCollection)
            {
                EntityTypeUniqueIdentifierCollection objectToBeCompared = obj as EntityTypeUniqueIdentifierCollection;

                Int32 entityTypeUniqueIdentifiersUnion = base._items.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 entityTypeUniqueIdentifiersIntersect = base._items.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if (entityTypeUniqueIdentifiersUnion != entityTypeUniqueIdentifiersIntersect)
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

            foreach (EntityTypeUniqueIdentifier entityTypeUniqueIdentifier in base._items)
            {
                hashCode += entityTypeUniqueIdentifier.GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Load current EntityTypeUniqueIdentifierCollection from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for current entityTypeUniqueIdentifierCollection
        /// </param>
        public void LoadEntityTypeUniqueIdentifierCollection(String valuesAsXml)
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityTypeUniqueIdentifier")
                        {
                            String entityTypeUniqueIdentifierXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(entityTypeUniqueIdentifierXml))
                            {
                                EntityTypeUniqueIdentifier entityTypeUniqueIdentifier = new EntityTypeUniqueIdentifier(entityTypeUniqueIdentifierXml);

                                if (entityTypeUniqueIdentifier != null)
                                {
                                    this.Add(entityTypeUniqueIdentifier);
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

        #endregion Public Methods

        #region Private Methods
        #endregion Private Methods

        #region ICollection<EntityTypeUniqueIdentifier> Members

        /// <summary>
        /// Add a list of entityTypeUniqueIdentifier objects to the collection.
        /// </summary>
        /// <param name="items">Indicates list of entityTypeUniqueIdentifier objects</param>
        public void Add(List<EntityTypeUniqueIdentifier> items)
        {
            if (items != null && items.Count > 0)
            {
                foreach (EntityTypeUniqueIdentifier item in items)
                {
                    base.Add(item);
                }
            }
        }

        /// <summary>
        /// Add entityTypeUniqueIdentifier in collection
        /// </summary>
        /// <param name="iEntityTypeUniqueIdentifier">entityTypeUniqueIdentifier to add in collection</param>
        public new void Add(IEntityTypeUniqueIdentifier iEntityTypeUniqueIdentifier)
        {
            if (iEntityTypeUniqueIdentifier != null)
            {
                base.Add((EntityTypeUniqueIdentifier)iEntityTypeUniqueIdentifier);
            }
        }

        #endregion

        #endregion Methods
    }
}
