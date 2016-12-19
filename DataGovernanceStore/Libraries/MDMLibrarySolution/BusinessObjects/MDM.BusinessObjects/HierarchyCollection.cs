using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace MDM.BusinessObjects
{
    using System.Collections.ObjectModel;
    using MDM.BusinessObjects.Imports;
    using MDM.BusinessObjects.Interfaces;
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Hierarchy Collections
    /// </summary>
    [DataContract]
    public class HierarchyCollection : InterfaceContractCollection<IHierarchy, Hierarchy>, IHierarchyCollection, IDataModelObjectCollection
    {
        #region Fields

        /// <summary>
        /// Field for allowed user actions on the object
        /// </summary>
        private Collection<UserAction> _permissionSet = null;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates allowed user actions on the object
        /// </summary>
        [DataMember]
        public Collection<UserAction> PermissionSet
        {
            get { return _permissionSet; }
            set { _permissionSet = value; }
        }

        /// <summary>
        /// Property denoting a ObjectType for an dataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return ObjectType.Taxonomy;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public HierarchyCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public HierarchyCollection(String valueAsXml)
        {
            LoadHierarchyCollection(valueAsXml);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hierarchyList"></param>
        public HierarchyCollection(IList<Hierarchy> hierarchyList)
        {
            if (hierarchyList != null)
            {
                this._items = new Collection<Hierarchy>(hierarchyList);
            }
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        /// <summary>
        /// Determines whether the hierarchyCollection contains a specific profileId.
        /// </summary>
        /// <param name="hierarchyId">The hierarchy object to locate in the hierarchyCollection.</param>
        /// <returns>
        /// <para>true : If hierarchy found in hierarchyCollection</para>
        /// <para>false : If hierarchy found not in hierarchyCollection</para>
        /// </returns>
        public bool Contains(int hierarchyId)
        {
            return this.GetHierarchy(hierarchyId) != null;
        }

        /// <summary>
        /// Remove hierarchy object from hierarchyCollection
        /// </summary>
        /// <param name="hierarchyId">HierarchyId of hierarchy which is to be removed from collection</param>
        /// <returns>true if hierarchy is successfully removed; otherwise, false. This method also returns false if hierarchy was not found in the original collection</returns>
        public bool Remove(int hierarchyId)
        {
            Hierarchy hierarchy = GetHierarchy(hierarchyId);

            if (hierarchy == null)
                throw new ArgumentException("No hierarchy found for given Id :" + hierarchyId);

            return this.Remove(hierarchy);
        }

        /// <summary>
        /// Get specific hierarchy by Id
        /// </summary>
        /// <param name="hierarchyId">Id of hierarchy</param>
        /// <returns><see cref="Hierarchy"/></returns>
        public Hierarchy GetHierarchy(Int32 hierarchyId)
        {
            if (this._items == null)
            {
                throw new NullReferenceException("There are no hierarchies to search in");
            }

            if (hierarchyId <= 0)
            {
                throw new ArgumentException("hierarchy Id must be greater than 0", hierarchyId.ToString());
            }

            return this.Get(hierarchyId) as Hierarchy;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is HierarchyCollection)
            {
                HierarchyCollection objectToBeCompared = obj as HierarchyCollection;
                Int32 hierarchiesUnion = this._items.ToList().Union(objectToBeCompared._items.ToList()).Count();
                Int32 hierarchiesIntersect = this._items.ToList().Intersect(objectToBeCompared._items.ToList()).Count();
                if (hierarchiesUnion != hierarchiesIntersect)
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
            return this._items.Sum(attr => attr.GetHashCode());
        }

        /// <summary>
        /// Get Xml representation of hierarchy object
        /// </summary>
        /// <returns>Xml string representing the hierarchyCollection</returns>
        public string ToXml()
        {
            String hierarchyXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (Hierarchy hierarchy in this._items)
            {
                builder.Append(hierarchy.ToXml());
            }

            hierarchyXml = String.Format("<Hierarchies>{0}</Hierarchies>", builder);
            return hierarchyXml;
        }

        /// <summary>
        /// Get Xml representation of hierarchy collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public string ToXml(ObjectSerialization serialization)
        {
            return this.ToXml();
        }

        /// <summary>
        /// Clone hierarchy collection.
        /// </summary>
        /// <returns>cloned hierarchy collection object.</returns>
        public IHierarchyCollection Clone()
        {
            HierarchyCollection clonedHierarchies = new HierarchyCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (Hierarchy hierarchy in this._items)
                {
                    IHierarchy clonedIhierarchy = hierarchy.Clone();
                    clonedHierarchies.Add(clonedIhierarchy);
                }
            }

            return clonedHierarchies;

        }

        /// <summary>
        /// Gets hierarchy item by id
        /// </summary>
        /// <param name="hierarchyId">Id of the hierarchy</param>
        /// <returns>hierarchy with specified Id</returns>
        public IHierarchy Get(Int32 hierarchyId)
        {
            return this._items.FirstOrDefault(item => item.Id == hierarchyId);
        }

        /// <summary>
        /// Gets hierarchy item by name
        /// </summary>
        /// <param name="hierarchyName">Name of the hierarchy</param>
        /// <returns>hierarchy with specified Id</returns>
        public Hierarchy Get(String hierarchyName)
        {
            hierarchyName = hierarchyName.ToLowerInvariant();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (Hierarchy hierarchy in this._items)
                {
                    if (hierarchy.Name.ToLowerInvariant().Equals(hierarchyName))
                    {
                        return hierarchy;
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

            HierarchyCollection hierarchies = GetHierarchies(referenceIds);

            if (hierarchies != null && hierarchies.Count > 0)
            {
                foreach (Hierarchy hierarchy in hierarchies)
                {
                    result = result && this.Remove(hierarchy);
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
            Collection<IDataModelObjectCollection> hierarchiesInBatch = null;

            if (this._items != null)
            {
                hierarchiesInBatch = Utility.Split(this, batchSize);
            }

            return hierarchiesInBatch;
        }

        /// <summary>
        /// Adds item in current collection
        /// </summary>
        /// <param name="item">Indicates item to be added in collection</param>
        public void AddDataModelObject(IDataModelObject item)
        {
            this.Add(item as Hierarchy);
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadHierarchyCollection(string valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Hierarchy")
                        {
                            String hierarchyXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(hierarchyXml))
                            {
                                Hierarchy hierarchy = new Hierarchy(hierarchyXml);
                                this.Add(hierarchy);
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
        ///  Gets the hierarchies using the reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of an entity type which is to be fetched.</param>
        /// <returns>Returns filtered entity types</returns>
        private HierarchyCollection GetHierarchies(Collection<String> referenceIds)
        {
            HierarchyCollection hierarchies = new HierarchyCollection();
            Int32 counter = 0;

            if (this._items != null && this._items.Count > 0 && referenceIds != null && referenceIds.Count > 0)
            {
                foreach (Hierarchy hierarchy in this._items)
                {
                    if (referenceIds.Contains(hierarchy.ReferenceId))
                    {
                        hierarchies.Add(hierarchy);
                        counter++;
                    }

                    if (referenceIds.Count.Equals(counter))
                        break;
                }
            }

            return hierarchies;
        }

        #endregion Private Methods

        #endregion
    }
}