using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represent Collection of RelationshipType Object
    /// </summary>
    [DataContract]
    public class RelationshipTypeCollection : InterfaceContractCollection<IRelationshipType, RelationshipType>, IRelationshipTypeCollection, IDataModelObjectCollection
    {
        #region Fields
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the RelationshipType Collection
        /// </summary>
        public RelationshipTypeCollection() { }

        /// <summary>
        /// Initialize subscriber from Xml value
        /// </summary>
        /// <param name="valuesAsXml">Relationship type in xml format</param>
        public RelationshipTypeCollection(String valuesAsXml)
        {
            LoadRelationshipType(valuesAsXml);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipTypeList"></param>
        public RelationshipTypeCollection(IList<RelationshipType> relationshipTypeList)
        {
            if (relationshipTypeList != null)
            {
                this._items = new Collection<RelationshipType>(relationshipTypeList);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting a ObjectType for an dataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return ObjectType.RelationshipType;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Check if RelationshipTypeCollection contains RelationshipType with given Id
        /// </summary>
        /// <param name="Id">Id using which RelationshipType is to be searched from collection</param>
        /// <returns>
        /// <para>true : If RelationshipType found in RelationshipTypeCollection</para>
        /// <para>false : If RelationshipType found not in RelationshipTypeCollection</para>
        /// </returns>
        public bool Contains(Int32 Id)
        {
            if (GetRelationshipType(Id) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove relationshipType object from RelationshipTypeCollection
        /// </summary>
        /// <param name="relationshipTypeId">relationshipTypeId of relationshipType which is to be removed from collection</param>
        /// <returns>true if relationshipType is successfully removed; otherwise, false. This method also returns false if relationshipType was not found in the original collection</returns>
        public bool Remove(Int32 relationshipTypeId)
        {
            IRelationshipType relationshipType = GetRelationshipType(relationshipTypeId);

            if (relationshipType == null)
                throw new ArgumentException("No RelationshipType found for given Id :" + relationshipTypeId);
            else
                return this.Remove(relationshipType);
        }

        /// <summary>
        /// Get relationship type based on given relationship type id
        /// </summary>
        /// <param name="Id">Id of relationship type to search on</param>
        /// <returns>Relationship type with given id.</returns>
        public IRelationshipType GetRelationshipType(Int32 Id)
        {
            var filteredRelationshipType = from relationshipType in this._items
                                           where relationshipType.Id == Id
                                           select relationshipType;

            if (filteredRelationshipType.Any())
                return filteredRelationshipType.First();
            else
                return null;
        }

        /// <summary>
        /// Get Xml representation of RelationshipTypeCollection
        /// </summary>
        /// <returns>Xml representation of RelationshipTypeCollection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<RelationshipTypes>";

            if (this._items != null && this._items.Count > 0)
            {
                foreach (RelationshipType relationshipType in this._items)
                {
                    returnXml = String.Concat(returnXml, relationshipType.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</RelationshipTypes>");

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of RelationshipTypeCollection
        /// </summary>
        /// <returns>Xml representation of RelationshipTypeCollection</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String returnXml = String.Empty;

            returnXml = "<RelationshipTypes>";

            if (this._items != null && this._items.Count > 0)
            {
                foreach (RelationshipType relationshipType in this._items)
                {
                    returnXml = String.Concat(returnXml, relationshipType.ToXml(serialization));
                }
            }

            returnXml = String.Concat(returnXml, "</RelationshipTypes>");

            return returnXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is RelationshipTypeCollection)
            {
                RelationshipTypeCollection objectToBeCompared = obj as RelationshipTypeCollection;

                Int32 relationshipTypesUnion = this._items.ToList().Union(objectToBeCompared._items.ToList()).Count();
                Int32 relationshipTypesIntersect = this._items.ToList().Intersect(objectToBeCompared._items.ToList()).Count();

                if (relationshipTypesUnion != relationshipTypesIntersect)
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

            foreach (RelationshipType RelationshipType in this._items)
            {
                hashCode += RelationshipType.GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Collection<Int32> GetRelationshipTypeIds()
        {
            Collection<Int32> relTypeIds = null;
            if (this._items != null)
            {
                relTypeIds = new Collection<int>();
                foreach (RelationshipType relationshipType in this._items)
                {
                    relTypeIds.Add(relationshipType.Id);
                }
            }
            return relTypeIds;
        }

        /// <summary>
        /// Gets the relationship type for given id.
        /// </summary>
        /// <param name="relationshipTypeId">id of relationship type to be searched in the collection</param>
        /// <returns>Relationship type having given id</returns>
        public RelationshipType Get(Int32 relationshipTypeId)
        {
            if (this._items != null && this._items.Count > 0)
            {
                foreach (RelationshipType relationshipType in this._items)
                {
                    if (relationshipType.Id == relationshipTypeId)
                    {
                        return relationshipType;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the relationship type for given name.
        /// </summary>
        /// <param name="relationshipTypeName">name of relationship type to be searched in the collection</param>
        /// <returns>Relationship type having given name</returns>
        public RelationshipType Get(String relationshipTypeName)
        {
            relationshipTypeName = relationshipTypeName.ToLowerInvariant();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (RelationshipType relationshipType in this._items)
                {
                    if (relationshipType.Name.ToLowerInvariant().Equals(relationshipTypeName))
                    {
                        return relationshipType;
                    }
                }
            }

            return null;
        }

        #region IDataModelObjectCollection Members

        /// <summary>
        /// Remove dataModelObject based on reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of an entity type which is to be fetched.</param>
        /// <returns>true if item was successfully removed from the collection;otherwise, false. This method also returns false if item is not found in the original</returns>
        public Boolean Remove(Collection<String> referenceIds)
        {
            Boolean result = true;

            RelationshipTypeCollection relationshipTypes = GetRelationshipTypes(referenceIds);

            if (relationshipTypes != null && relationshipTypes.Count > 0)
            {
                foreach (RelationshipType relationshipType in relationshipTypes)
                {
                    result = result && this.Remove(relationshipType);
                }
            }

            return result;
        }

        /// <summary>
        /// Splits current collection based on batchSize
        /// </summary>
        /// <param name="batchSize">Indicates no of objects to be put in each batch</param>
        /// <returns>IDataModelObjectCollection in Batch</returns>
        public Collection<IDataModelObjectCollection> Split(Int32 batchSize)
        {
            Collection<IDataModelObjectCollection> relationshipTypesInBatch = null;

            if (this._items != null)
            {
                relationshipTypesInBatch = Utility.Split(this, batchSize);
            }

            return relationshipTypesInBatch;
        }

        /// <summary>
        /// Adds item in current collection
        /// </summary>
        /// <param name="item">Indicates item to be added in collection</param>
        public void AddDataModelObject(IDataModelObject item)
        {
            this.Add(item as RelationshipType);
        }

        #endregion

        #endregion

        #region Private Methods

        private void LoadRelationshipType(String valuesAsXml)
        {
            #region Sample Xml
            /*
			 * <RelationshipTypes>
                <RelationshipType
                  Id="2" 
                  Name="Kit Code Products" 
                  LongName="Kit Code Products" 
                  ValidationRequired="1" 
                  ShowValidFlag="0 
                  ReadOnly="0" 
                  DrillDown="0" 
                  IsDefault="1" 
              </RelationshipType>
              </RelationshipTypes>
			 */
            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "RelationshipType")
                        {
                            String relTypeXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(relTypeXml))
                            {
                                RelationshipType relationshipType = new RelationshipType(relTypeXml);
                                if (relationshipType != null)
                                {
                                    this.Add(relationshipType);
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
        ///  Gets the relationship type using the reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of an entity type which is to be fetched.</param>
        /// <returns>Returns filtered relationship types</returns>
        private RelationshipTypeCollection GetRelationshipTypes(Collection<String> referenceIds)
        {
            RelationshipTypeCollection relationshipTypes = new RelationshipTypeCollection();
            Int32 counter = 0;

            if (this._items != null && this._items.Count > 0 && referenceIds != null && referenceIds.Count > 0)
            {
                foreach (RelationshipType relationshipType in this._items)
                {
                    if (referenceIds.Contains(relationshipType.ReferenceId))
                    {
                        relationshipTypes.Add(relationshipType);
                        counter++;
                    }

                    if (referenceIds.Count.Equals(counter))
                        break;
                }
            }

            return relationshipTypes;
        }

        #endregion

        #endregion
    }
}