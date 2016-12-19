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
    /// Represents collection of IntegrationActivityLog
    /// </summary>
    [DataContract]
    public class IntegrationActivityLogCollection : InterfaceContractCollection<IIntegrationActivityLog, IntegrationActivityLog>, IIntegrationActivityLogCollection
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public IntegrationActivityLogCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public IntegrationActivityLogCollection(String valueAsXml)
        {
            LoadIntegrationActivityLogCollection(valueAsXml);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Determines whether the IntegrationActivityLogCollection contains a specific message.
        /// </summary>
        /// <param name="integrationActivityLogId">The IntegrationActivityLog object to locate in the IntegrationActivityLogCollection.</param>
        /// <returns>
        /// <para>true : If IntegrationActivityLog found in IntegrationActivityLogCollection</para>
        /// <para>false : If IntegrationActivityLog found not in IntegrationActivityLogCollection</para>
        /// </returns>
        public Boolean Contains(Int64 integrationActivityLogId)
        {
            return this.GetIntegrationActivityLog(integrationActivityLogId) != null;
        }

        /// <summary>
        /// Remove IntegrationActivityLog object from IntegrationActivityLogCollection
        /// </summary>
        /// <param name="integrationActivityLogId">IntegrationActivityLogId of IntegrationActivityLog which is to be removed from collection</param>
        /// <returns>true if IntegrationActivityLog is successfully removed; otherwise, false. This method also returns false if IntegrationActivityLog was not found in the original collection</returns>
        public Boolean Remove(Int64 integrationActivityLogId)
        {
            IIntegrationActivityLog message = GetIntegrationActivityLog(integrationActivityLogId);

            if (message == null)
                throw new ArgumentException("No IntegrationActivityLog found for given Id :" + integrationActivityLogId);

            return this.Remove(message);
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is IntegrationActivityLogCollection)
            {
                IntegrationActivityLogCollection objectToBeCompared = obj as IntegrationActivityLogCollection;
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
        /// <returns>Xml String representing the IntegrationActivityLogCollection</returns>
        public String ToXml()
        {
            String messageXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (IntegrationActivityLog log in this._items)
            {
                builder.Append(log.ToXml());
            }

            messageXml = String.Format("<IntegrationActivityLogs>{0}</IntegrationActivityLogs>", builder);
            return messageXml;
        }

        /// <summary>
        /// Get Xml representation of IntegrationActivityLogs collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            return this.ToXml();
        }

        /// <summary>
        /// Clone IntegrationActivityLog collection.
        /// </summary>
        /// <returns>cloned IntegrationActivityLog collection object.</returns>
        public IIntegrationActivityLogCollection Clone()
        {
            IntegrationActivityLogCollection clonedMessages = new IntegrationActivityLogCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (IntegrationActivityLog message in this._items)
                {
                    IIntegrationActivityLog clonedIhierarchy = message.Clone();
                    clonedMessages.Add(clonedIhierarchy);
                }
            }

            return clonedMessages;

        }

        /// <summary>
        /// Gets hierarchy item by id
        /// </summary>
        /// <param name="integrationActivityLogId">Id of the hierarchy</param>
        /// <returns>hierarchy with specified Id</returns>
        public IIntegrationActivityLog Get(Int64 integrationActivityLogId)
        {
            return this._items.FirstOrDefault(item => item.Id == integrationActivityLogId);
        }

        /// <summary>
        /// Get specific IntegrationActivityLog by Id
        /// </summary>
        /// <param name="integrationActivityLogId">Id of IntegrationActivityLog</param>
        /// <returns><see cref="IntegrationActivityLog"/></returns>
        public IIntegrationActivityLog GetIntegrationActivityLog(Int64 integrationActivityLogId)
        {
            if (this._items == null)
            {
                throw new NullReferenceException("There are no IntegrationActivityLog to search in");
            }

            if (integrationActivityLogId <= 0)
            {
                throw new ArgumentException("IntegrationActivityLog Id must be greater than 0", integrationActivityLogId.ToString());
            }

            return this.Get(integrationActivityLogId) as IntegrationActivityLog;
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadIntegrationActivityLogCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "IntegrationActivityLog")
                        {
                            String messageXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(messageXml))
                            {
                                IntegrationActivityLog message = new IntegrationActivityLog(messageXml);
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
