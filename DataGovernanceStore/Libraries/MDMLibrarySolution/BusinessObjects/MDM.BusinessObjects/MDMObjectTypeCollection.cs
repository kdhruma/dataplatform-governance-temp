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
    /// Represents collection of MDMObjectType
    /// </summary>
    [DataContract]
    public class MDMObjectTypeCollection : InterfaceContractCollection<IMDMObjectType, MDMObjectType>, IMDMObjectTypeCollection
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public MDMObjectTypeCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public MDMObjectTypeCollection(String valueAsXml)
        {
            LoadMDMObjectTypeCollection(valueAsXml);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Determines whether the MDMObjectTypeCollection contains a specific message.
        /// </summary>
        /// <param name="mdmObjectTypeId">The MDMObjectType object to locate in the MDMObjectTypeCollection.</param>
        /// <returns>
        /// <para>true : If MDMObjectType found in MDMObjectTypeCollection</para>
        /// <para>false : If MDMObjectType found not in MDMObjectTypeCollection</para>
        /// </returns>
        public Boolean Contains(Int16 mdmObjectTypeId)
        {
            return this.GetMDMObjectType(mdmObjectTypeId) != null;
        }

        /// <summary>
        /// Remove MDMObjectType object from MDMObjectTypeCollection
        /// </summary>
        /// <param name="mdmObjectTypeId">MDMObjectTypeId of MDMObjectType which is to be removed from collection</param>
        /// <returns>true if MDMObjectType is successfully removed; otherwise, false. This method also returns false if MDMObjectType was not found in the original collection</returns>
        public Boolean Remove(Int16 mdmObjectTypeId)
        {
            MDMObjectType message = GetMDMObjectType(mdmObjectTypeId);

            if (message == null)
                throw new ArgumentException("No MDMObjectType found for given Id :" + mdmObjectTypeId);

            return this.Remove(message);
        }

        /// <summary>
        /// Get specific MDMObjectType by Id
        /// </summary>
        /// <param name="mdmObjectTypeId">Id of MDMObjectType</param>
        /// <returns><see cref="MDMObjectType"/></returns>
        public MDMObjectType GetMDMObjectType(Int16 mdmObjectTypeId)
        {
            if (this._items == null)
            {
                throw new NullReferenceException("There are no MDMObjectType to search in");
            }

            if (mdmObjectTypeId <= 0)
            {
                throw new ArgumentException("MDMObjectType Id must be greater than 0", mdmObjectTypeId.ToString());
            }

            return this.Get(mdmObjectTypeId) as MDMObjectType;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is MDMObjectTypeCollection)
            {
                MDMObjectTypeCollection objectToBeCompared = obj as MDMObjectTypeCollection;
                Int32 messageUnion = this._items.ToList().Union(objectToBeCompared._items.ToList()).Count();
                Int32 messageIntersect = this._items.ToList().Intersect(objectToBeCompared._items.ToList()).Count();
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
        /// Get Xml representation of MDMObjectType object
        /// </summary>
        /// <returns>Xml String representing the MDMObjectTypeCollection</returns>
        public String ToXml()
        {
            String messageXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (MDMObjectType item in this._items)
            {
                builder.Append(item.ToXml());
            }

            messageXml = String.Format("<MDMObjectTypes>{0}</MDMObjectTypes>", builder);
            return messageXml;
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
        /// Clone MDMObjectType collection.
        /// </summary>
        /// <returns>cloned MDMObjectType collection object.</returns>
        public IMDMObjectTypeCollection Clone()
        {
            MDMObjectTypeCollection clonedMessageTypes = new MDMObjectTypeCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (MDMObjectType mdmObjectType in this._items)
                {
                    IMDMObjectType clonedMessageType = mdmObjectType.Clone();
                    clonedMessageTypes.Add(mdmObjectType);
                }
            }

            return clonedMessageTypes;

        }

        /// <summary>
        /// Gets MDMObjectType by id
        /// </summary>
        /// <param name="mdmObjectTypeId">Id of the MDMObjectType</param>
        /// <returns>MDMObjectType with specified Id</returns>
        public IMDMObjectType Get(Int16 mdmObjectTypeId)
        {
            return this._items.FirstOrDefault(item => item.Id == mdmObjectTypeId);
        }

        /// <summary>
        /// Gets MDMObjectType by Name
        /// </summary>
        /// <param name="mdmObjectTypeName">Name of the MDMObjectType</param>
        /// <returns>MDMObjectType with specified Id</returns>
        public IMDMObjectType Get(String mdmObjectTypeName)
        {
            return this._items.FirstOrDefault(item => item.Name == mdmObjectTypeName);
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadMDMObjectTypeCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMObjectType")
                        {
                            String messageXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(messageXml))
                            {
                                MDMObjectType message = new MDMObjectType(messageXml);
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
