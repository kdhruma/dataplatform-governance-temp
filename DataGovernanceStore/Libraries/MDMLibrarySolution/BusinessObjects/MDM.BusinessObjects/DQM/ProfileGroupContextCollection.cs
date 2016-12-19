using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Interfaces;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class ProfileGroupContextCollection : InterfaceContractCollection<IProfileGroupContext, ProfileGroupContext>, IProfileGroupContextCollection
    {
        #region Constructor
        
        /// <summary>
        /// Initializes a new instance of the ProfileGroupContextCollection class.
        /// </summary>
        public ProfileGroupContextCollection() { }

        /// <summary>
        /// Initializes a new instance of the ProfileGroupContext class.
        /// </summary>
        /// <param name="valuesAsXml">ProfileGroupContextCollection Object in XML representation</param>
        public ProfileGroupContextCollection(String valuesAsXml)
        {
            LoadProfileGroupContextCollection(valuesAsXml);
        }

        /// <summary>
        /// Initializes a new instance of the ProfileGroupContext class.
        /// </summary>
        /// <param name="ProfileGroupContextList">List of ProfileGroupContext object</param>
        public ProfileGroupContextCollection(List<ProfileGroupContext> ProfileGroupContextList)
        {
            if (ProfileGroupContextList != null)
            {
                this._items = new Collection<ProfileGroupContext>(ProfileGroupContextList);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Xml representation of ProfileGroupContext Collection
        /// </summary>
        /// <returns>Xml representation of ProfileGroupContext Collection</returns>
        public String ToXml()
        {
            String ProfileGroupContextCollectionXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Parameter node start
            xmlWriter.WriteStartElement("MatchingProfileGroup");

            if (this._items != null && this._items.Count > 0)
            {
                foreach (ProfileGroupContext item in this._items)
                {
                    xmlWriter.WriteRaw(item.ToXml());
                }
            }

            //Param node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            ProfileGroupContextCollectionXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return ProfileGroupContextCollectionXml;
        }

        /// <summary>
        /// Compare ProfileGroupContextCollection with current collection.
        /// This method will compare ProfileGroupContextCollection. If current collection has more ProfileGroupContextCollection than object to be compared, extra ProfileGroupContextCollection will be ignored.
        /// If ProfileGroupContext to be compared has categories which is not there in current collection, it will return false.
        /// </summary>
        /// <param name="subsetProfileGroupContextCollection">ProfileGroupContextCollection to be compared with current collection</param>
        /// <param name="compareIds">Flag to determine whether id based comparison is true or not</param>
        /// <returns>True : Is both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(ProfileGroupContextCollection subsetProfileGroupContextCollection, Boolean compareIds = false)
        {
            if (subsetProfileGroupContextCollection != null)
            {
                foreach (ProfileGroupContext ProfileGroupContext in subsetProfileGroupContextCollection)
                {
                    if (this._items != null && this._items.Count > 0)
                    {
                        foreach (ProfileGroupContext sourceProfileGroupContext in this._items)
                        {
                            if (sourceProfileGroupContext.IsSuperSetOf(ProfileGroupContext, compareIds))
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Adds passed ProfileGroupContext to the current collection
        /// </summary>
        /// <param name="profileGroupContextCollection">Collection of ProfileGroupContext which needs to be added</param>
        public void AddRange(ProfileGroupContextCollection profileGroupContextCollection)
        {
            foreach (ProfileGroupContext ProfileGroupContext in profileGroupContextCollection)
            {
                if (!this.Contains(ProfileGroupContext.ProfileId))
                {
                    this.Add(ProfileGroupContext);
                }
            }
        }

        /// <summary>
        /// Check if ProfileGroupContextCollection contains ProfileGroupContext with given ProfileGroupContextId
        /// </summary>
        /// <param name="profileId">Id of the ProfileGroupContext</param>        
        /// <returns>
        /// <para>true : If ProfileGroupContext found in ProfileGroupContextCollection</para>
        /// <para>false : If ProfileGroupContext found not in ProfileGroupContextCollection</para>
        /// </returns>
        public Boolean Contains(Int32 profileId)
        {
            if (GetProfileGroupContextByProfileId(profileId) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// GetProfileGroupContext By Id
        /// </summary>
        /// <param name="applicationContextId">Id of the ApplicationContext</param>
        /// <returns>ProfileGroupContext</returns>
        public ProfileGroupContext GetByApplicationContextId(Int32 applicationContextId)
        {
            ProfileGroupContext ProfileGroupContext = null;

            if (this._items != null)
            {
                foreach (ProfileGroupContext item in this._items)
                {
                    if (item.ApplicationContext != null && item.ApplicationContext.Id == applicationContextId)
                    {
                        ProfileGroupContext = item;
                        break;
                    }
                }
            }

            return ProfileGroupContext;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialize current collection of ProfileGroupContext object through Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for current ProfileGroupContext collection object</param>
        private void LoadProfileGroupContextCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Map")
                        {
                            String ProfileGroupContextXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(ProfileGroupContextXml))
                            {
                                ProfileGroupContext ProfileGroupContext = new ProfileGroupContext(ProfileGroupContextXml);

                                if (ProfileGroupContext != null)
                                {
                                    this.Add(ProfileGroupContext);
                                }
                            }
                        }
                        else
                        {
                            //Keep on reading the xml until we reach expected node.
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileId"></param>        
        /// <returns></returns>
        private ProfileGroupContext GetProfileGroupContextByProfileId(Int32 profileId)
        {
            ProfileGroupContext ProfileGroupContext = null;

            if (this._items != null)
            {
                foreach (ProfileGroupContext item in this._items)
                {
                    if (item.ProfileId == profileId)
                    {
                        ProfileGroupContext = item;
                        break;
                    }
                }
            }

            return ProfileGroupContext;
        }

        #endregion
    }
}
