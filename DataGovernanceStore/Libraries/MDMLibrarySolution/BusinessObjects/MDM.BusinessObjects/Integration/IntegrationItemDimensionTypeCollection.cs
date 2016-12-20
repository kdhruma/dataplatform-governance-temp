using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents collection of IntegrationItemDimensionType
    /// </summary>
    [DataContract]
    public class IntegrationItemDimensionTypeCollection : InterfaceContractCollection<IIntegrationItemDimensionType, IntegrationItemDimensionType>, IIntegrationItemDimensionTypeCollection
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public IntegrationItemDimensionTypeCollection() : base() { }

        /// <summary>
        /// Xml constructor
        /// </summary>
        public IntegrationItemDimensionTypeCollection(String valueAsXml)
        {
            LoadIntegrationItemDimensionTypeCollection(valueAsXml);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Determines whether the IntegrationItemDimensionTypeCollection contains a specific IntegrationItemDimensionType.
        /// </summary>
        /// <param name="integrationItemDimensionTypeId">The IntegrationItemDimensionType object to locate in the IntegrationItemDimensionTypeCollection.</param>
        /// <returns>
        /// <para>true : If IntegrationItemDimensionType found in IntegrationItemDimensionTypeCollection</para>
        /// <para>false : If IntegrationItemDimensionType found not in IntegrationItemDimensionTypeCollection</para>
        /// </returns>
        public Boolean Contains(Int32 integrationItemDimensionTypeId)
        {
            return this.GetIntegrationItemDimensionType(integrationItemDimensionTypeId) != null;
        }

        /// <summary>
        /// Remove IntegrationItemDimensionType object from IntegrationItemDimensionTypeCollection
        /// </summary>
        /// <param name="integrationItemDimensionTypeId">IntegrationItemDimensionTypeId of IntegrationItemDimensionType which is to be removed from collection</param>
        /// <returns>true if IntegrationItemDimensionType is successfully removed; otherwise, false. This method also returns false if IntegrationItemDimensionType was not found in the original collection</returns>
        public Boolean Remove(Int32 integrationItemDimensionTypeId)
        {
            IIntegrationItemDimensionType item = GetIntegrationItemDimensionType(integrationItemDimensionTypeId);

            if (item == null)
                throw new ArgumentException("No IntegrationItemDimensionType found for given Id :" + integrationItemDimensionTypeId);

            return this.Remove(item);
        }

        /// <summary>
        /// Get specific IntegrationItemDimensionType by Id
        /// </summary>
        /// <param name="integrationItemDimensionTypeId">Id of IntegrationItemDimensionType</param>
        /// <returns><see cref="IntegrationItemDimensionType"/></returns>
        public IIntegrationItemDimensionType GetIntegrationItemDimensionType(Int32 integrationItemDimensionTypeId)
        {
            if (this._items == null)
            {
                throw new NullReferenceException("There are no IntegrationItemDimensionTypes to search in");
            }

            if (integrationItemDimensionTypeId <= 0)
            {
                throw new ArgumentException("IntegrationItemDimensionType Id must be greater than 0", integrationItemDimensionTypeId.ToString());
            }

            return this.Get(integrationItemDimensionTypeId) as IntegrationItemDimensionType;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is IntegrationItemDimensionTypeCollection)
            {
                IntegrationItemDimensionTypeCollection objectToBeCompared = obj as IntegrationItemDimensionTypeCollection;
                Int32 union = this._items.ToList().Union(objectToBeCompared._items.ToList()).Count();
                Int32 intersect = this._items.ToList().Intersect(objectToBeCompared._items.ToList()).Count();
                if (union != intersect)
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
            return this._items.Sum(attr => attr.GetHashCode());
        }

        /// <summary>
        /// Get Xml representation of IntegrationItemDimensionType object
        /// </summary>
        /// <returns>Xml String representing the IntegrationItemDimensionTypeCollection</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (IntegrationItemDimensionType hierarchy in this._items)
            {
                builder.Append(hierarchy.ToXml());
            }

            xml = String.Format("<IntegrationItemDimensionTypes>{0}</IntegrationItemDimensionTypes>", builder);
            return xml;
        }

        /// <summary>
        /// Get Xml representation of IntegrationItemDimensionType collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            return this.ToXml();
        }

        /// <summary>
        /// Clone IntegrationItemDimensionType collection.
        /// </summary>
        /// <returns>cloned IntegrationItemDimensionType collection object.</returns>
        public IIntegrationItemDimensionTypeCollection Clone()
        {
            IntegrationItemDimensionTypeCollection clonedItems = new IntegrationItemDimensionTypeCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (IntegrationItemDimensionType item in this._items)
                {
                    IIntegrationItemDimensionType clonedItem = item.Clone();
                    clonedItems.Add(clonedItem);
                }
            }

            return clonedItems;

        }

        /// <summary>
        /// Gets IntegrationItemDimensionType item by id
        /// </summary>
        /// <param name="integrationItemDimensionTypeId">Id of the IntegrationItemDimensionType</param>
        /// <returns>IntegrationItemDimensionType with specified Id</returns>
        public IIntegrationItemDimensionType Get(Int32 integrationItemDimensionTypeId)
        {
            return this._items.FirstOrDefault(item => item.Id == integrationItemDimensionTypeId);
        }

        /// <summary>
        /// Gets IntegrationItemDimensionType by IntegrationItemDimensionType ShortName
        /// </summary>
        /// <param name="integrationItemDimensionTypeShortName">Short name of the IntegrationItemDimensionType</param>
        /// <returns>IntegrationItemDimensionType with specified ShortName</returns>
        public IIntegrationItemDimensionTypeCollection Get(String integrationItemDimensionTypeShortName)
        {
            IntegrationItemDimensionTypeCollection filteredIntegrationItemDimensionTypes = new IntegrationItemDimensionTypeCollection();

            foreach (IntegrationItemDimensionType item in _items)
            {
                if (item.Name == integrationItemDimensionTypeShortName)
                {
                    filteredIntegrationItemDimensionTypes.Add(item);
                }
            }

            return filteredIntegrationItemDimensionTypes;
        }

        /// <summary>
        /// Gets IntegrationItemDimensionType by IntegrationItemDimensionType ShortName and connector short name
        /// </summary>
        /// <param name="integrationItemDimensionTypeShortName">Short name of the IntegrationItemDimensionType</param>
        /// <param name="connectorName"> Name of the connector</param>
        /// <returns>IntegrationItemDimensionType with specified ShortName</returns>
        public IIntegrationItemDimensionType Get(String integrationItemDimensionTypeShortName, String connectorName)
        {
            return this._items.FirstOrDefault(item => item.Name == integrationItemDimensionTypeShortName && item.ConnectorName == connectorName);
        }

        /// <summary>
        /// Gets IntegrationItemDimensionType by IntegrationItemDimensionType ShortName and connector Id
        /// </summary>
        /// <param name="integrationItemDimensionTypeShortName">Short name of the IntegrationItemDimensionType</param>
        /// <param name="connectorId"> ID of the connector</param>
        /// <returns>IntegrationItemDimensionType with specified ShortName</returns>
        public IIntegrationItemDimensionType Get(String integrationItemDimensionTypeShortName, Int16 connectorId)
        {
            return this._items.FirstOrDefault(item => item.Name == integrationItemDimensionTypeShortName && item.ConnectorId == connectorId);
        }
        
        /// <summary>
        /// Gets IntegrationItemDimensionType by connector ShortName
        /// </summary>
        /// <param name="connectorName">Short name of the connector</param>
        /// <returns>IntegrationItemDimensionType with specified connector ShortName</returns>
        public IIntegrationItemDimensionTypeCollection GetIntegrationItemDimensionTypesByConnectorName(String connectorName)
        {
            IntegrationItemDimensionTypeCollection filteredIntegrationItemDimensionTypes = new IntegrationItemDimensionTypeCollection();

            foreach (IntegrationItemDimensionType item in _items)
            {
                if (item.ConnectorName == connectorName)
                {
                    filteredIntegrationItemDimensionTypes.Add(item);
                }
            }

            return filteredIntegrationItemDimensionTypes;
        }

        /// <summary>
        /// Gets IntegrationItemDimensionType by connector Id
        /// </summary>
        /// <param name="connectorId">Id of the connector</param>
        /// <returns>IntegrationItemDimensionType with specified connector Id</returns>
        public IIntegrationItemDimensionTypeCollection GetIntegrationItemDimensionTypesByConnectorId(Int16 connectorId)
        {
            IntegrationItemDimensionTypeCollection filteredIntegrationItemDimensionTypes = new IntegrationItemDimensionTypeCollection();

            foreach (IntegrationItemDimensionType item in _items)
            {
                if (item.ConnectorId == connectorId)
                {
                    filteredIntegrationItemDimensionTypes.Add(item);
                }
            }

            return filteredIntegrationItemDimensionTypes;
        }

        /// <summary>
        /// Checks if current IntegrationItemDimensionTypeCollection is superset of passed IntegrationItemDimensionType collection
        /// </summary>
        /// <param name="subsetIntegrationItemDimensionTypes"></param>
        /// <param name="compareIds"></param>
        /// <returns></returns>
        public Boolean IsSuperSetOf(IntegrationItemDimensionTypeCollection subsetIntegrationItemDimensionTypes, Boolean compareIds = false)
        {
            if (subsetIntegrationItemDimensionTypes == null)
            {
                return false;
            }
            foreach (IntegrationItemDimensionType subsetInstance in subsetIntegrationItemDimensionTypes)
            {
                IIntegrationItemDimensionType integrationItemDimensionType = this.Get(subsetInstance.Name, subsetInstance.ConnectorName);

                if (integrationItemDimensionType != null)
                {
                    if (!( ( IntegrationItemDimensionType )integrationItemDimensionType ).IsSuperSetOf(subsetInstance, compareIds))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadIntegrationItemDimensionTypeCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "IntegrationItemDimensionType")
                        {
                            String xml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(xml))
                            {
                                IntegrationItemDimensionType item = new IntegrationItemDimensionType(xml);
                                this.Add(item);
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

        #endregion Private Methods
    }
}
