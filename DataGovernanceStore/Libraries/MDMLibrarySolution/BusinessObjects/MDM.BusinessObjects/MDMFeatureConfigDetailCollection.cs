using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// A collection of MDM Feature Config Detail items.
    /// </summary>
    [DataContract]
    public class MDMFeatureConfigDetailCollection : InterfaceContractCollection<IMDMFeatureConfigDetail, MDMFeatureConfigDetail>, IMDMFeatureConfigDetailCollection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MDM Feature Config Detail Collection
        /// </summary>
        public MDMFeatureConfigDetailCollection()
        { }

        /// <summary>
        /// Initialize MDM Feature Config Detail collection from Xml value
        /// </summary>
        /// <param name="valuesAsXml">MDM Feature Config Detail collection in xml format</param>
        public MDMFeatureConfigDetailCollection(String valuesAsXml)
        {
            LoadMDMFeatureConfigDetailCollection(valuesAsXml);
        }

        #endregion

        #region IMDMFeatureConfigDetailCollection implementation

        /// <summary>
        /// Determines whether the MDM feature config detail collection contains a specific value.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        ///   <c>true</c> if item is found in the MDM Feature Config Detail Collection; otherwise, <c>false</c>.
        /// </returns>
        public Boolean Contains(int id)
        {
            return GetMDMFeatureConfigDetail(id) != null;
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the MDM feature config detail collection.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        ///   <c>true</c> if item was successfully removed from the MDM feature config detail collection; otherwise, <c>false</c>.
        /// This method also returns false if item is not found in the original MDM feature config detail collection.
        /// </returns>
        /// <exception cref="System.ArgumentException">No ExportSubscriber found for given Id : + id</exception>
        public Boolean Remove(int id)
        {
            MDMFeatureConfigDetail item = GetMDMFeatureConfigDetail(id);

            if (item == null)
                throw new ArgumentException("No MDM feature config detail found for given Id :" + id);

            return Remove(item);
        }

        /// <summary>
        /// Get Xml representation of MDM feature config detail collection
        /// </summary>
        /// <returns>
        /// Xml representation of MDM feature config detail collection
        /// </returns>
        public String ToXml()
        {
            StringBuilder result = new StringBuilder("<MDMFeatureConfigDetails>");

            foreach (MDMFeatureConfigDetail config in _items)
            {
                result.Append(config.ToXml());
            }
            result.Append("</MDMFeatureConfigDetails>");

            return result.ToString();
        }

        /// <summary>
        /// Get Xml representation of MDM feature config detail collection
        /// </summary>
        /// <param name="serialization">The serialization options.</param>
        /// <returns>
        /// Xml representation of MDM feature config detail collection
        /// </returns>
        public String ToXml(ObjectSerialization serialization)
        {
            StringBuilder result = new StringBuilder("<MDMFeatureConfigDetails>");

            foreach (MDMFeatureConfigDetail config in _items)
            {
                result.Append(config.ToXml(serialization));
            }
            result.Append("</MDMFeatureConfigDetails>");

            return result.ToString();
        }       

        /// <summary>
        /// Add MDMFeatureConfigDetail in collection
        /// </summary>
        /// <param name="iMDMFeatureConfigs">MDM feature config detail to add in collection</param>
        public void AddRange(IMDMFeatureConfigDetailCollection iMDMFeatureConfigs)
        {
            if (iMDMFeatureConfigs == null)
            {
                throw new ArgumentNullException("MDMFeatureConfigs");
            }

            foreach (MDMFeatureConfigDetail MDMFeatureConfigDetail in iMDMFeatureConfigs)
            {
                this.Add(MDMFeatureConfigDetail);
            }
        }

        #endregion IMDMFeatureConfigDetailCollection implementation
        
        #region Private Methods

        private MDMFeatureConfigDetail GetMDMFeatureConfigDetail(int id)
        {
            return _items.FirstOrDefault(ac => ac.Id == id);
        }

        private void LoadMDMFeatureConfigDetailCollection(String valuesAsXml)
        { 
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMFeatureConfigDetail")
                        {
                            String appConfigXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(appConfigXml))
                            {
                                Add(new MDMFeatureConfigDetail(appConfigXml));
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

        #endregion
    }
}
