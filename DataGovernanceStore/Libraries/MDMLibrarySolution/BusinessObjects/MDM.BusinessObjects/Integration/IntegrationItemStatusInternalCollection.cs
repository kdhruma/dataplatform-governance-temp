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
    /// Represents collection of IntegrationItemStatusInternal
    /// </summary>
    [DataContract]
    public class IntegrationItemStatusInternalCollection : InterfaceContractCollection<IIntegrationItemStatusInternal, IntegrationItemStatusInternal>, IIntegrationItemStatusInternalCollection
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public IntegrationItemStatusInternalCollection() : base() { }

        /// <summary>
        /// Xml constructor
        /// </summary>
        public IntegrationItemStatusInternalCollection(String valueAsXml)
        {
            LoadIntegrationItemStatusInternalCollection(valueAsXml);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Determines whether the IntegrationItemStatusInternalCollection contains a specific IntegrationItemStatusInternal.
        /// </summary>
        /// <param name="integrationItemStatusInternalId">The IntegrationItemStatusInternal object to locate in the IntegrationItemStatusInternalCollection.</param>
        /// <returns>
        /// <para>true : If IntegrationItemStatusInternal found in IntegrationItemStatusInternalCollection</para>
        /// <para>false : If IntegrationItemStatusInternal found not in IntegrationItemStatusInternalCollection</para>
        /// </returns>
        public Boolean Contains(Int64 integrationItemStatusInternalId)
        {
            return this.GetIntegrationItemStatusInternal(integrationItemStatusInternalId) != null;
        }

        /// <summary>
        /// Remove IntegrationItemStatusInternal object from IntegrationItemStatusInternalCollection
        /// </summary>
        /// <param name="integrationItemStatusInternalId">IntegrationItemStatusInternalId of IntegrationItemStatusInternal which is to be removed from collection</param>
        /// <returns>true if IntegrationItemStatusInternal is successfully removed; otherwise, false. This method also returns false if IntegrationItemStatusInternal was not found in the original collection</returns>
        public Boolean Remove(Int64 integrationItemStatusInternalId)
        {
            IIntegrationItemStatusInternal item = GetIntegrationItemStatusInternal(integrationItemStatusInternalId);

            if (item == null)
                throw new ArgumentException("No IntegrationItemStatusInternal found for given Id :" + integrationItemStatusInternalId);

            return this.Remove(item);
        }

        /// <summary>
        /// Get specific IntegrationItemStatusInternal by Id
        /// </summary>
        /// <param name="integrationItemStatusInternalId">Id of IntegrationItemStatusInternal</param>
        /// <returns><see cref="IntegrationItemStatusInternal"/></returns>
        public IIntegrationItemStatusInternal GetIntegrationItemStatusInternal(Int64 integrationItemStatusInternalId)
        {
            if (this._items == null)
            {
                throw new NullReferenceException("There are no IntegrationItemStatusInternals to search in");
            }

            if (integrationItemStatusInternalId <= 0)
            {
                throw new ArgumentException("IntegrationItemStatusInternal Id must be greater than 0", integrationItemStatusInternalId.ToString());
            }

            return this.Get(integrationItemStatusInternalId) as IntegrationItemStatusInternal;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is IntegrationItemStatusInternalCollection)
            {
                IntegrationItemStatusInternalCollection objectToBeCompared = obj as IntegrationItemStatusInternalCollection;
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
        /// Get Xml representation of IntegrationItemStatusInternal object
        /// </summary>
        /// <returns>Xml String representing the IntegrationItemStatusInternalCollection</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (IntegrationItemStatusInternal hierarchy in this._items)
            {
                builder.Append(hierarchy.ToXml());
            }

            xml = String.Format("<IntegrationItemStatusInternals>{0}</IntegrationItemStatusInternals>", builder);
            return xml;
        }

        /// <summary>
        /// Get Xml representation of IntegrationItemStatusInternal collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            return this.ToXml();
        }

        /// <summary>
        /// Clone IntegrationItemStatusInternal collection.
        /// </summary>
        /// <returns>cloned IntegrationItemStatusInternal collection object.</returns>
        public IIntegrationItemStatusInternalCollection Clone()
        {
            IntegrationItemStatusInternalCollection clonedItems = new IntegrationItemStatusInternalCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (IntegrationItemStatusInternal item in this._items)
                {
                    IIntegrationItemStatusInternal clonedItem = item.Clone();
                    clonedItems.Add(clonedItem);
                }
            }

            return clonedItems;

        }

        /// <summary>
        /// Gets IntegrationItemStatusInternal item by id
        /// </summary>
        /// <param name="integrationItemStatusInternalId">Id of the IntegrationItemStatusInternal</param>
        /// <returns>IntegrationItemStatusInternal with specified Id</returns>
        public IIntegrationItemStatusInternal Get(Int64 integrationItemStatusInternalId)
        {
            return this._items.FirstOrDefault(item => item.Id == integrationItemStatusInternalId);
        }

        ///// <summary>
        ///// Gets IntegrationItemStatusInternal by IntegrationItemStatusInternal ShortName
        ///// </summary>
        ///// <param name="integrationItemStatusInternalShortName">Short name of the IntegrationItemStatusInternal</param>
        ///// <returns>IntegrationItemStatusInternal with specified ShortName</returns>
        //public IIntegrationItemStatusInternal Get(String integrationItemStatusInternalShortName)
        //{
        //    return this._items.FirstOrDefault(item => item.Name == integrationItemStatusInternalShortName);
        //}

        /// <summary>
        /// Checks if current IntegrationItemStatusInternalCollection is superset of passed IntegrationItemStatusInternal collection
        /// </summary>
        /// <param name="subsetIntegrationItemStatusInternals"></param>
        /// <param name="compareIds"></param>
        /// <returns></returns>
        public Boolean IsSuperSetOf(IntegrationItemStatusInternalCollection subsetIntegrationItemStatusInternals, Boolean compareIds = false)
        {
            throw new NotImplementedException();
            //if (subsetIntegrationItemStatusInternals == null)
            //{
            //    return false;
            //}
            //foreach (IntegrationItemStatusInternal subsetInstance in subsetIntegrationItemStatusInternals)
            //{
            //    IIntegrationItemStatusInternal integrationItemStatusInternal = this.Get(subsetInstance.Name);

            //    if (integrationItemStatusInternal != null)
            //    {
            //        if (!( ( IntegrationItemStatusInternal )integrationItemStatusInternal ).IsSuperSetOf(subsetInstance, compareIds))
            //        {
            //            return false;
            //        }
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //return true;
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadIntegrationItemStatusInternalCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "IntegrationItemStatusInternal")
                        {
                            String xml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(xml))
                            {
                                IntegrationItemStatusInternal item = new IntegrationItemStatusInternal(xml);
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
