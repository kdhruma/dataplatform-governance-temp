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
    /// A collection of AppConfig items.
    /// </summary>
    [DataContract]
    public class AppConfigCollection : InterfaceContractCollection<IAppConfig, AppConfig>, IAppConfigCollection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the AppConfig Collection
        /// </summary>
        public AppConfigCollection()
        { }

        /// <summary>
        /// Initialize AppConfig collection from Xml value
        /// </summary>
        /// <param name="valuesAsXml">AppConfig collection in xml format</param>
        public AppConfigCollection(String valuesAsXml)
        {
            LoadAppConfigCollection(valuesAsXml);
        }

        #endregion

        #region IAppConfigCollection implementation

        /// <summary>
        /// Determines whether the IAppConfigCollection contains a specific value.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        ///   <c>true</c> if item is found in the IAppConfigCollection; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(int id)
        {
            return GetAppConfig(id) != null;
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the IAppConfigCollectio.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        ///   <c>true</c> if item was successfully removed from the IAppConfigCollectio; otherwise, <c>false</c>.
        /// This method also returns false if item is not found in the original IAppConfigCollectio.
        /// </returns>
        /// <exception cref="System.ArgumentException">No ExportSubscriber found for given Id : + id</exception>
        public bool Remove(int id)
        {
            AppConfig item = GetAppConfig(id);

            if (item == null)
                throw new ArgumentException("No AppConfig found for given Id :" + id);

            return Remove(item);
        }

        /// <summary>
        /// Get Xml representation of IAppConfigCollection
        /// </summary>
        /// <returns>
        /// Xml representation of IAppConfigCollection
        /// </returns>
        public String ToXml()
        {
            StringBuilder result = new StringBuilder("<AppConfigs>");

            foreach (AppConfig config in _items)
            {
                result.Append(config.ToXml());
            }
            result.Append("</AppConfigs>");

            return result.ToString();
        }

        /// <summary>
        /// Get Xml representation of IAppConfigCollection
        /// </summary>
        /// <param name="serialization">The serialization options.</param>
        /// <returns>
        /// Xml representation of IAppConfigCollection
        /// </returns>
        public String ToXml(ObjectSerialization serialization)
        {
            StringBuilder result = new StringBuilder("<AppConfigs>");

            foreach (AppConfig config in _items)
            {
                result.Append(config.ToXml(serialization));
            }
            result.Append("</AppConfigs>");

            return result.ToString();
        }

        #endregion IAppConfigCollection implementation
        
        #region Private Methods

        private AppConfig GetAppConfig(int id)
        {
            return _items.FirstOrDefault(ac => ac.Id == id);
        }

        private void LoadAppConfigCollection(String valuesAsXml)
        {
            #region Sample Xml
            /*
			 * <AppConfigs>
                <AppConfig Id="1" ShortName="Option1" LongName="Option1"/>
              </AppConfigs>
			 */
            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "AppConfig")
                        {
                            String appConfigXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(appConfigXml))
                            {
                                Add(new AppConfig(appConfigXml));
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
