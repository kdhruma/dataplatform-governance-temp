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
    /// Represents collection of IntegrationItemStatusDimensionInternal
    /// </summary>
    [DataContract]
    public class IntegrationItemStatusDimensionInternalCollection : InterfaceContractCollection<IIntegrationItemStatusDimensionInternal, IntegrationItemStatusDimensionInternal>, IIntegrationItemDimensionStatusInternalCollection
    {
        #region Fields
        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public IntegrationItemStatusDimensionInternalCollection() : base() { }

        /// <summary>
        /// Xml constructor
        /// </summary>
        public IntegrationItemStatusDimensionInternalCollection(String valueAsXml)
        {
            LoadIntegrationItemStatusDimensionInternalCollection(valueAsXml);
        }

        #endregion Constructors

        #region Properties
        #endregion

        #region Methods
        
        #region Public Methods

        /// <summary>
        /// Determines whether the IntegrationItemStatusDimensionInternalCollection contains a specific IntegrationItemStatusDimensionInternal.
        /// </summary>
        /// <param name="IntegrationItemStatusDimensionInternalId">The IntegrationItemStatusDimensionInternal object to locate in the IntegrationItemStatusDimensionInternalCollection.</param>
        /// <returns>
        /// <para>true : If IntegrationItemStatusDimensionInternal found in IntegrationItemStatusDimensionInternalCollection</para>
        /// <para>false : If IntegrationItemStatusDimensionInternal found not in IntegrationItemStatusDimensionInternalCollection</para>
        /// </returns>
        public Boolean Contains(Int32 IntegrationItemStatusDimensionInternalId)
        {
            return this.GetIntegrationItemStatusDimensionInternal(IntegrationItemStatusDimensionInternalId) != null;
        }

        /// <summary>
        /// Remove IntegrationItemStatusDimensionInternal object from IntegrationItemStatusDimensionInternalCollection
        /// </summary>
        /// <param name="IntegrationItemStatusDimensionInternalId">IntegrationItemStatusDimensionInternalId of IntegrationItemStatusDimensionInternal which is to be removed from collection</param>
        /// <returns>true if IntegrationItemStatusDimensionInternal is successfully removed; otherwise, false. This method also returns false if IntegrationItemStatusDimensionInternal was not found in the original collection</returns>
        public Boolean Remove(Int32 IntegrationItemStatusDimensionInternalId)
        {
            IIntegrationItemStatusDimensionInternal item = GetIntegrationItemStatusDimensionInternal(IntegrationItemStatusDimensionInternalId);

            if (item == null)
                throw new ArgumentException("No IntegrationItemStatusDimensionInternal found for given Id :" + IntegrationItemStatusDimensionInternalId);

            return this.Remove(item);
        }

        /// <summary>
        /// Get specific IntegrationItemStatusDimensionInternal by Id
        /// </summary>
        /// <param name="IntegrationItemStatusDimensionInternalId">Id of IntegrationItemStatusDimensionInternal</param>
        /// <returns><see cref="IntegrationItemStatusDimensionInternal"/></returns>
        public IIntegrationItemStatusDimensionInternal GetIntegrationItemStatusDimensionInternal(Int32 IntegrationItemStatusDimensionInternalId)
        {
            if (this._items == null)
            {
                throw new NullReferenceException("There are no IntegrationItemStatusDimensionInternals to search in");
            }

            if (IntegrationItemStatusDimensionInternalId <= 0)
            {
                throw new ArgumentException("IntegrationItemStatusDimensionInternal Id must be greater than 0", IntegrationItemStatusDimensionInternalId.ToString());
            }

            return this.Get(IntegrationItemStatusDimensionInternalId) as IntegrationItemStatusDimensionInternal;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is IntegrationItemStatusDimensionInternalCollection)
            {
                IntegrationItemStatusDimensionInternalCollection objectToBeCompared = obj as IntegrationItemStatusDimensionInternalCollection;
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
        /// Get Xml representation of IntegrationItemStatusDimensionInternal object
        /// </summary>
        /// <returns>Xml String representing the IntegrationItemStatusDimensionInternalCollection</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (IntegrationItemStatusDimensionInternal hierarchy in this._items)
            {
                builder.Append(hierarchy.ToXml());
            }

            xml = String.Format("<IntegrationItemStatusDimensionInternalCollection>{0}</IntegrationItemStatusDimensionInternalCollection>", builder);
            return xml;
        }

        /// <summary>
        /// Get Xml representation of IntegrationItemStatusDimensionInternal collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            return this.ToXml();
        }

        /// <summary>
        /// Clone IntegrationItemStatusDimensionInternal collection.
        /// </summary>
        /// <returns>cloned IntegrationItemStatusDimensionInternal collection object.</returns>
        public IIntegrationItemDimensionStatusInternalCollection Clone()
        {
            IntegrationItemStatusDimensionInternalCollection clonedItems = new IntegrationItemStatusDimensionInternalCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (IntegrationItemStatusDimensionInternal item in this._items)
                {
                    IIntegrationItemStatusDimensionInternal clonedItem = item.Clone();
                    clonedItems.Add(clonedItem);
                }
            }

            return clonedItems;

        }

        /// <summary>
        /// Gets IntegrationItemStatusDimensionInternal item by id
        /// </summary>
        /// <param name="IntegrationItemStatusDimensionInternalId">Id of the IntegrationItemStatusDimensionInternal</param>
        /// <returns>IntegrationItemStatusDimensionInternal with specified Id</returns>
        public IIntegrationItemStatusDimensionInternal Get(Int32 IntegrationItemStatusDimensionInternalId)
        {
            return this._items.FirstOrDefault(item => item.Id == IntegrationItemStatusDimensionInternalId);
        }

        /// <summary>
        /// Adds specified dimension parameters into integration item status dimension internal
        /// </summary>
        /// <param name="dimensionTypeId">Indicates dimension type identifier</param>
        /// <param name="integrationItemDimensionTypeName">Indicates integration item status dimension type name</param>
        /// <param name="integrationItemDimensionValue">Indicates integration item dimension value</param>
        /// <param name="integrationItemDimensionTypeLongName">Indicates integration item status dimension type long name</param>
        public void Add(Int32 dimensionTypeId, String integrationItemDimensionTypeName, String integrationItemDimensionTypeLongName, String integrationItemDimensionValue)
        {
            if (dimensionTypeId < 1)
            {
                throw new ArgumentException("Dimension Type Id must not be less than 1");
            }

            if (String.IsNullOrEmpty(integrationItemDimensionTypeName) || String.IsNullOrEmpty(integrationItemDimensionValue))
            {
                throw new ArgumentException("Dimension Type name or value can not be null or empty");
            }


            IntegrationItemStatusDimensionInternal dimensionStatus = new IntegrationItemStatusDimensionInternal();
            dimensionStatus.IntegrationItemDimensionTypeId = dimensionTypeId;
            dimensionStatus.IntegrationItemDimensionTypeName = integrationItemDimensionTypeName;
            dimensionStatus.IntegrationItemDimensionTypeLongName = integrationItemDimensionTypeLongName;
            dimensionStatus.IntegrationItemDimensionValue = integrationItemDimensionValue;

            this._items.Add(dimensionStatus);
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadIntegrationItemStatusDimensionInternalCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "IntegrationItemStatusDimensionInternal")
                        {
                            String xml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(xml))
                            {
                                IntegrationItemStatusDimensionInternal item = new IntegrationItemStatusDimensionInternal(xml);
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

        #endregion
    }
}
