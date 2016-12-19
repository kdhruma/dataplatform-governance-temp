
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
    /// Represents collection of IntegrationItemStatusSearchParameter
    /// </summary>
    [DataContract]
    public class IntegrationItemStatusSearchParameterCollection : InterfaceContractCollection<IIntegrationItemStatusSearchParameter, IntegrationItemStatusSearchParameter>, IIntegrationItemStatusSearchParameterCollection
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public IntegrationItemStatusSearchParameterCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public IntegrationItemStatusSearchParameterCollection(String valueAsXml)
        {
            LoadIntegrationItemStatusSearchParameterCollection(valueAsXml);
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
            if (obj is IntegrationItemStatusSearchParameterCollection)
            {
                IntegrationItemStatusSearchParameterCollection objectToBeCompared = obj as IntegrationItemStatusSearchParameterCollection;
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
        /// Get Xml representation of IntegrationItemStatusSearchParameter object
        /// </summary>
        /// <returns>Xml String representing the IntegrationItemStatusSearchParameterCollection</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (IntegrationItemStatusSearchParameter hierarchy in this._items)
            {
                builder.Append(hierarchy.ToXml());
            }

            xml = String.Format("<IntegrationItemStatusSearchParameters>{0}</IntegrationItemStatusSearchParameters>", builder);
            return xml;
        }

        /// <summary>
        /// Get Xml representation of IntegrationItemStatusSearchParameter collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            return this.ToXml();
        }

        /// <summary>
        /// Clone IntegrationItemStatusSearchParameter collection.
        /// </summary>
        /// <returns>cloned IntegrationItemStatusSearchParameter collection object.</returns>
        public IIntegrationItemStatusSearchParameterCollection Clone()
        {
            IntegrationItemStatusSearchParameterCollection clonedItems = new IntegrationItemStatusSearchParameterCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (IntegrationItemStatusSearchParameter item in this._items)
                {
                    IIntegrationItemStatusSearchParameter clonedItem = item.Clone();
                    clonedItems.Add(clonedItem);
                }
            }

            return clonedItems;

        }

        #endregion Public Methods

        #region Private Methods

        private void LoadIntegrationItemStatusSearchParameterCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "IntegrationItemStatusSearchParameter")
                        {
                            String xml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(xml))
                            {
                                IntegrationItemStatusSearchParameter item = new IntegrationItemStatusSearchParameter(xml);
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
