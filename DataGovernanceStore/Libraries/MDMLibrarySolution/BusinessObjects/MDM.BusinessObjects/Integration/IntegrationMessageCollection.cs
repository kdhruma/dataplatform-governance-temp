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
    /// Represents collection of IntegrationMessage
    /// </summary>
    [DataContract]
    public class IntegrationMessageCollection : InterfaceContractCollection<IIntegrationMessage, IntegrationMessage>, IIntegrationMessageCollection
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public IntegrationMessageCollection() : base() {}

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public IntegrationMessageCollection(String valueAsXml)
        {
            LoadIntegrationMessageCollection(valueAsXml);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Determines whether the IntegrationMessageCollection contains a specific message.
        /// </summary>
        /// <param name="integrationMessageId">The integration message object to locate in the IntegrationMessageCollection.</param>
        /// <returns>
        /// <para>true : If integration message found in IntegrationMessageCollection</para>
        /// <para>false : If integration message found not in IntegrationMessageCollection</para>
        /// </returns>
        public Boolean Contains(Int64 integrationMessageId)
        {
            return this.GetIntegrationMessage(integrationMessageId) != null;
        }

        /// <summary>
        /// Remove integration message object from IntegrationMessageCollection
        /// </summary>
        /// <param name="integrationMessageId">IntegrationMessageId of integration message which is to be removed from collection</param>
        /// <returns>true if integration message is successfully removed; otherwise, false. This method also returns false if integration message was not found in the original collection</returns>
        public Boolean Remove(Int64 integrationMessageId)
        {
            IntegrationMessage message = GetIntegrationMessage(integrationMessageId);

            if (message == null)
                throw new ArgumentException("No Integration Message found for given Id :" + integrationMessageId);
            
            return this.Remove(message);
        }

        #region IntegrationMessage Get

        /// <summary>
        /// Get specific integration message by Id
        /// </summary>
        /// <param name="integrationMessageId">Id of integration message</param>
        /// <returns><see cref="IntegrationMessage"/></returns>
        public IntegrationMessage GetIntegrationMessage(Int64 integrationMessageId)
        {
            if (this._items == null)
            {
                throw new NullReferenceException("There are no integration message to search in");
            }

            if (integrationMessageId <= 0)
            {
                throw new ArgumentException("Integration Message Id must be greater than 0", integrationMessageId.ToString());
            }

            return this.Get(integrationMessageId) as IntegrationMessage;
        }

        #endregion IntegrationMessage Get

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is IntegrationMessageCollection)
            {
                IntegrationMessageCollection objectToBeCompared = obj as IntegrationMessageCollection;
                Int64 messageUnion = this._items.ToList().Union(objectToBeCompared._items.ToList()).Count();
                Int64 messageIntersect = this._items.ToList().Intersect(objectToBeCompared._items.ToList()).Count();
                if (messageUnion != messageIntersect)
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
        /// Get Xml representation of hierarchy object
        /// </summary>
        /// <returns>Xml String representing the IntegrationMessageCollection</returns>
        public String ToXml()
        {
            String messageXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (IntegrationMessage hierarchy in this._items)
            {
                builder.Append(hierarchy.ToXml());
            }

            messageXml = String.Format("<IntegrationMessages>{0}</IntegrationMessages>", builder);
            return messageXml;
        }

        /// <summary>
        /// Get Xml representation of Integration Messages collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            return this.ToXml();
        }

        /// <summary>
        /// Clone integration message collection.
        /// </summary>
        /// <returns>cloned integration message collection object.</returns>
        public IIntegrationMessageCollection Clone()
        {
            IntegrationMessageCollection clonedMessages = new IntegrationMessageCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (IntegrationMessage message in this._items)
                {
                    IIntegrationMessage clonedMessage = message.Clone();
                    clonedMessages.Add(clonedMessage);
                }
            }

            return clonedMessages;
        
        }

        /// <summary>
        /// Gets hierarchy item by id
        /// </summary>
        /// <param name="integrationMessageId">Id of the hierarchy</param>
        /// <returns>hierarchy with specified Id</returns>
        public IIntegrationMessage Get(Int64 integrationMessageId)
        {
            return this._items.FirstOrDefault(item => item.Id == integrationMessageId);
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadIntegrationMessageCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "IntegrationMessage")
                        {
                            String messageXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(messageXml))
                            {
                                IntegrationMessage message = new IntegrationMessage(messageXml);
                                this.Add(message);
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
