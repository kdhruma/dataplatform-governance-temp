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
    /// Represents collection of IntegrationItemStatus
    /// </summary>
    [DataContract]
    public class IntegrationItemStatusCollection : InterfaceContractCollection<IIntegrationItemStatus, IntegrationItemStatus>, IIntegrationItemStatusCollection
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public IntegrationItemStatusCollection() : base() { }

        /// <summary>
        /// Xml constructor
        /// </summary>
        public IntegrationItemStatusCollection(String valueAsXml)
        {
            LoadIntegrationItemStatusCollection(valueAsXml);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is IntegrationItemStatusCollection)
            {
                IntegrationItemStatusCollection objectToBeCompared = obj as IntegrationItemStatusCollection;
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
        /// Get Xml representation of IntegrationItemStatus object
        /// </summary>
        /// <returns>Xml String representing the IntegrationItemStatusCollection</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (IntegrationItemStatus integrationItemStatus in this._items)
            {
                builder.Append(integrationItemStatus.ToXml());
            }

            xml = String.Format("<IntegrationItemStatuss>{0}</IntegrationItemStatuss>", builder);
            return xml;
        }

        /// <summary>
        /// Get Xml representation of IntegrationItemStatus collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            return this.ToXml();
        }

        /// <summary>
        /// Clone IntegrationItemStatus collection.
        /// </summary>
        /// <returns>cloned IntegrationItemStatus collection object.</returns>
        public IIntegrationItemStatusCollection Clone()
        {
            IntegrationItemStatusCollection clonedItems = new IntegrationItemStatusCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (IntegrationItemStatus item in this._items)
                {
                    IIntegrationItemStatus clonedItem = item.Clone();
                    clonedItems.Add(clonedItem);
                }
            }

            return clonedItems;

        }

        /// <summary>
        /// Checks if current IntegrationItemStatusCollection is superset of passed IntegrationItemStatus collection
        /// </summary>
        /// <param name="subsetIntegrationItemStatuss"></param>
        /// <param name="compareIds"></param>
        /// <returns></returns>
        public Boolean IsSuperSetOf(IntegrationItemStatusCollection subsetIntegrationItemStatuss, Boolean compareIds = false)
        {
            throw new NotImplementedException();
            //if (subsetIntegrationItemStatuss == null)
            //{
            //    return false;
            //}
            //foreach (IntegrationItemStatus subsetInstance in subsetIntegrationItemStatuss)
            //{
            //    IIntegrationItemStatus integrationItemStatus = this.Get(subsetInstance.IntegrationItemDimensionTypeName);

            //    if (integrationItemStatus != null)
            //    {
            //        if (!( ( IntegrationItemStatus )integrationItemStatus ).IsSuperSetOf(subsetInstance, compareIds))
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

        private void LoadIntegrationItemStatusCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "IntegrationItemStatus")
                        {
                            String xml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(xml))
                            {
                                IntegrationItemStatus item = new IntegrationItemStatus(xml);
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
