using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Core.Extensions;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies Matching Profiles Collection
    /// </summary>
    [DataContract]
    public class MatchingProfileCollection : InterfaceContractCollection<IMatchingProfile, MatchingProfile>, IMatchingProfileCollection
    {
        #region Fields

        private const String XmlNodeName = "MatchingProfileCollection";
        private const String ChildXmlNodeName = "MatchingProfile";

        #endregion

        #region Constructors

		/// <summary>
        /// Initializes a new instance of the MatchingProfile Collection
        /// </summary>
        public MatchingProfileCollection()
        { }

        /// <summary>
        /// Initialize MatchingProfile collection from IList
        /// </summary>
        /// <param name="matchingProfiles">Source items</param>
        public MatchingProfileCollection(IList<MatchingProfile> matchingProfiles)
        {
            this._items = new Collection<MatchingProfile>(matchingProfiles);
        }

        /// <summary>
        /// Initialize MatchingProfile collection from xml
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public MatchingProfileCollection(String valuesAsXml)
        {
            LoadFromXml(valuesAsXml);
        }

	    #endregion

        #region Public Methods

        /// <summary>
        /// Get Xml representation of MatchingProfile Collection
        /// </summary>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement(XmlNodeName);

            foreach (MatchingProfile item in _items)
            {
                xmlWriter.WriteRaw(item.ToXml());
            }

            // MatchingProfileCollection node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Loads MatchingProfile Collection from XML
        /// </summary>
        public void LoadFromXml(String xml)
        {
            if (!String.IsNullOrWhiteSpace(xml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(xml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MatchingProfile")
                        {
                            String matchingProfileXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(matchingProfileXml))
                            {
                                MatchingProfile matchingProfile = new MatchingProfile(matchingProfileXml);
                                if (matchingProfile != null)
                                {
                                    this.Add(matchingProfile);
                                }
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

        #region ICloneable Members

        /// <summary>
        /// Clone MatchingProfile collection
        /// </summary>
        /// <returns>Cloned MatchingProfile collection object</returns>
        public Object Clone()
        {
            MatchingProfileCollection clonedResults = new MatchingProfileCollection();

            if (!_items.IsNullOrEmpty())
            {
                foreach (MatchingProfile item in _items)
                {
                    MatchingProfile clonedItem = item.Clone() as MatchingProfile;
                    clonedResults.Add(clonedItem);
                }
            }

            return clonedResults;
        }

        #endregion
    }

}