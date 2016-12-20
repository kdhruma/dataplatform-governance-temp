
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
    /// Represents collection of ConnectorProfile
    /// </summary>
    [DataContract]
    public class ConnectorProfileCollection : InterfaceContractCollection<IConnectorProfile, ConnectorProfile>, IConnectorProfileCollection
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ConnectorProfileCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public ConnectorProfileCollection(String valueAsXml)
        {
            LoadConnectorProfileCollection(valueAsXml);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Determines whether the ConnectorProfileCollection contains a specific item.
        /// </summary>
        /// <param name="connectorProfileId">The connector profile object to locate in the ConnectorProfileCollection.</param>
        /// <returns>
        /// <para>true : If connector profile found in ConnectorProfileCollection</para>
        /// <para>false : If connector profile found not in ConnectorProfileCollection</para>
        /// </returns>
        public Boolean Contains(Int16 connectorProfileId)
        {
            return this.GetConnectorProfile(connectorProfileId) != null;
        }

        /// <summary>
        /// Remove connector profile object from ConnectorProfileCollection
        /// </summary>
        /// <param name="connectorProfileId">ConnectorProfileId of connector profile which is to be removed from collection</param>
        /// <returns>true if connector profile is successfully removed; otherwise, false. This method also returns false if connector profile was not found in the original collection</returns>
        public Boolean Remove(Int16 connectorProfileId)
        {
            IConnectorProfile item = GetConnectorProfile(connectorProfileId);

            if (item == null)
                throw new ArgumentException("No ConnectorProfile found for given Id :" + connectorProfileId);

            return this.Remove(item);
        }

        /// <summary>
        /// Get specific connector profile item by Id
        /// </summary>
        /// <param name="connectorProfileId">Id of connector profile</param>
        /// <returns><see cref="ConnectorProfile"/></returns>
        public IConnectorProfile GetConnectorProfile(Int16 connectorProfileId)
        {
            if (this._items == null)
            {
                throw new NullReferenceException("There are no ConnectorProfiles to search in");
            }

            if (connectorProfileId <= 0)
            {
                throw new ArgumentException("ConnectorProfile Id must be greater than 0", connectorProfileId.ToString());
            }

            return this.Get(connectorProfileId) as ConnectorProfile;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is ConnectorProfileCollection)
            {
                ConnectorProfileCollection objectToBeCompared = obj as ConnectorProfileCollection;
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
        /// Get Xml representation of ConnectorProfile object
        /// </summary>
        /// <returns>Xml String representing the ConnectorProfileCollection</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (ConnectorProfile hierarchy in this._items)
            {
                builder.Append(hierarchy.ToXml());
            }

            xml = String.Format("<ConnectorProfiles>{0}</ConnectorProfiles>", builder);
            return xml;
        }

        /// <summary>
        /// Get Xml representation of connector profile collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            return this.ToXml();
        }

        /// <summary>
        /// Clone connector profile collection.
        /// </summary>
        /// <returns>cloned connector profile collection object.</returns>
        public IConnectorProfileCollection Clone()
        {
            ConnectorProfileCollection clonedItems = new ConnectorProfileCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (ConnectorProfile item in this._items)
                {
                    IConnectorProfile clonedItem = item.Clone();
                    clonedItems.Add(clonedItem);
                }
            }

            return clonedItems;

        }

        /// <summary>
        /// Gets ConnectorProfile item by id
        /// </summary>
        /// <param name="connectorProfileId">Id of the ConnectorProfile</param>
        /// <returns>ConnectorProfile with specified Id</returns>
        public IConnectorProfile Get(Int16 connectorProfileId)
        {
            return this._items.FirstOrDefault(item => item.Id == connectorProfileId);
        }

        /// <summary>
        /// Gets ConnectorProfile item by connector ShortName
        /// </summary>
        /// <param name="connectorProfileShortName">Short name of the ConnectorProfile</param>
        /// <returns>ConnectorProfile with specified ShortName</returns>
        public IConnectorProfile Get(String connectorProfileShortName)
        {
            return this._items.FirstOrDefault(item => item.Name == connectorProfileShortName);
        }

        /// <summary>
        /// Checks if current ConnectorProfileCollection is superset of passed profile collection
        /// </summary>
        /// <param name="subsetConnectorProfiles"></param>
        /// <param name="compareIds"></param>
        /// <returns></returns>
        public Boolean IsSuperSetOf(ConnectorProfileCollection subsetConnectorProfiles, Boolean compareIds = false)
        {
            if (subsetConnectorProfiles == null)
            {
                return false;
            }
            foreach (ConnectorProfile subsetProfileInstance in subsetConnectorProfiles)
            {
                IConnectorProfile profile = this.Get(subsetProfileInstance.Name);

                if (profile != null)
                {
                    if (!((ConnectorProfile)profile).IsSuperSetOf(subsetProfileInstance, compareIds))
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

        private void LoadConnectorProfileCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ConnectorProfile")
                        {
                            String xml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(xml))
                            {
                                ConnectorProfile item = new ConnectorProfile(xml);
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
