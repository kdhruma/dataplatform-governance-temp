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
    /// Represents collection of IntegrationItemDimensionStatus
    /// </summary>
    [DataContract]
    public class IntegrationItemStatusDimensionCollection : InterfaceContractCollection<IIntegrationItemStatusDimension, IntegrationItemStatusDimension>, IIntegrationItemStatusDimensionCollection
    {
        #region Fields
        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public IntegrationItemStatusDimensionCollection() : base() { }

        /// <summary>
        /// Xml constructor
        /// </summary>
        public IntegrationItemStatusDimensionCollection(String valueAsXml)
        {
            LoadIntegrationItemStatusDimensionCollection(valueAsXml);
        }

        #endregion Constructors

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is IntegrationItemStatusDimensionCollection)
            {
                IntegrationItemStatusDimensionCollection objectToBeCompared = obj as IntegrationItemStatusDimensionCollection;
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
        /// Get Xml representation of IntegrationItemDimensionStatus object
        /// </summary>
        /// <returns>Xml String representing the IntegrationItemDimensionStatusCollection</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (IntegrationItemStatusDimension integrationItemStatusDimension in this._items)
            {
                builder.Append(integrationItemStatusDimension.ToXml());
            }

            xml = String.Format("<IntegrationItemStatusDimensionCollection>{0}</IntegrationItemStatusDimensionCollection>", builder);
            return xml;
        }

        /// <summary>
        /// Get Xml representation of IntegrationItemDimensionStatus collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            return this.ToXml();
        }

        /// <summary>
        /// Clone IntegrationItemDimensionStatus collection.
        /// </summary>
        /// <returns>cloned IntegrationItemDimensionStatus collection object.</returns>
        public IIntegrationItemStatusDimensionCollection Clone()
        {
            IntegrationItemStatusDimensionCollection clonedItems = new IntegrationItemStatusDimensionCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (IntegrationItemStatusDimension item in this._items)
                {
                    IIntegrationItemStatusDimension clonedItem = item.Clone();
                    clonedItems.Add(clonedItem);
                }
            }

            return clonedItems;

        }

        /// <summary>
        /// Add status for a dimension value 
        /// </summary>
        /// <param name="dimensionTypeName">Short name for dimension Type</param>
        /// <param name="dimensionValue">value of a dimension</param>
        public void Add(String dimensionTypeName, String dimensionValue)
        {
            IntegrationItemStatusDimension dimensionStatus = new IntegrationItemStatusDimension();
            dimensionStatus.IntegrationItemDimensionTypeName = dimensionTypeName;
            dimensionStatus.IntegrationItemDimensionValue = dimensionValue;

            this._items.Add(dimensionStatus);
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadIntegrationItemStatusDimensionCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "IntegrationItemStatusDimension")
                        {
                            String xml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(xml))
                            {
                                IntegrationItemStatusDimension item = new IntegrationItemStatusDimension(xml);
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