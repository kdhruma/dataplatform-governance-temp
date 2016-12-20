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
    /// Represents collection of IntegrationMessageType
    /// </summary>
    [DataContract]
    public class IntegrationMessageTypeCollection : InterfaceContractCollection<IIntegrationMessageType, IntegrationMessageType>, IIntegrationMessageTypeCollection
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public IntegrationMessageTypeCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public IntegrationMessageTypeCollection(String valueAsXml)
        {
            LoadIntegrationMessageTypeCollection(valueAsXml);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Determines whether the IntegrationMessageTypeCollection contains a specific message.
        /// </summary>
        /// <param name="integrationMessageTypeId">The Integration message type object to locate in the IntegrationMessageTypeCollection.</param>
        /// <returns>
        /// <para>true : If Integration message type found in IntegrationMessageTypeCollection</para>
        /// <para>false : If Integration message type found not in IntegrationMessageTypeCollection</para>
        /// </returns>
        public Boolean Contains(Int16 integrationMessageTypeId)
        {
            return this.GetIntegrationMessageType(integrationMessageTypeId) != null;
        }

        /// <summary>
        /// Remove Integration message type object from IntegrationMessageTypeCollection
        /// </summary>
        /// <param name="integrationMessageTypeId">IntegrationMessageTypeId of Integration message type which is to be removed from collection</param>
        /// <returns>true if Integration message type is successfully removed; otherwise, false. This method also returns false if Integration message type was not found in the original collection</returns>
        public Boolean Remove(Int16 integrationMessageTypeId)
        {
            IntegrationMessageType message = GetIntegrationMessageType(integrationMessageTypeId);

            if (message == null)
                throw new ArgumentException("No IntegrationMessageType found for given Id :" + integrationMessageTypeId);

            return this.Remove(message);
        }

        /// <summary>
        /// Get specific Integration message type by Id
        /// </summary>
        /// <param name="integrationMessageTypeId">Id of Integration message type</param>
        /// <returns><see cref="IntegrationMessageType"/></returns>
        public IntegrationMessageType GetIntegrationMessageType(Int16 integrationMessageTypeId)
        {
            if (this._items == null)
            {
                throw new NullReferenceException("There are no Integration message type to search in");
            }

            if (integrationMessageTypeId <= 0)
            {
                throw new ArgumentException("IntegrationMessageType Id must be greater than 0", integrationMessageTypeId.ToString());
            }

            return this.Get(integrationMessageTypeId) as IntegrationMessageType;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is IntegrationMessageTypeCollection)
            {
                IntegrationMessageTypeCollection objectToBeCompared = obj as IntegrationMessageTypeCollection;
                Int32 typeUnion = this._items.ToList().Union(objectToBeCompared._items.ToList()).Count();
                Int32 typeIntersect = this._items.ToList().Intersect(objectToBeCompared._items.ToList()).Count();
                if (typeUnion != typeIntersect)
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
        /// Get Xml representation of MDMObjectType object
        /// </summary>
        /// <returns>Xml String representing the IntegrationMessageTypeCollection</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (IntegrationMessageType type in this._items)
            {
                builder.Append(type.ToXml());
            }

            xml = String.Format("<IntegrationMessageTypes>{0}</IntegrationMessageTypes>", builder);
            return xml;
        }

        /// <summary>
        /// Get Xml representation of MDMObjectType collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            return this.ToXml();
        }

        /// <summary>
        /// Clone Integration message type collection.
        /// </summary>
        /// <returns>cloned Integration message type collection object.</returns>
        public IIntegrationMessageTypeCollection Clone()
        {
            IntegrationMessageTypeCollection clonedMessageTypes = new IntegrationMessageTypeCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (IntegrationMessageType messageType in this._items)
                {
                    IIntegrationMessageType clonedMessageType = messageType.Clone();
                    clonedMessageTypes.Add(clonedMessageType);
                }
            }

            return clonedMessageTypes;

        }

        /// <summary>
        /// Gets MDMObjectType item by id
        /// </summary>
        /// <param name="integrationMessageTypeId">Id of the MDMObjectType</param>
        /// <returns>MDMObjectType with specified Id</returns>
        public IIntegrationMessageType Get(Int16 integrationMessageTypeId)
        {
            return this._items.FirstOrDefault(item => item.Id == integrationMessageTypeId);
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadIntegrationMessageTypeCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "IntegrationMessageType")
                        {
                            String xml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(xml))
                            {
                                IntegrationMessageType type = new IntegrationMessageType(xml);
                                this.Add(type);
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
