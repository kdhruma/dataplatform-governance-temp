using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQMMerging
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies MatchReviewProfile Collection
    /// </summary>
    [DataContract]
    public class MatchReviewProfileCollection : InterfaceContractCollection<IMatchReviewProfile, MatchReviewProfile>, IMatchReviewProfileCollection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MatchReviewProfile Collection
        /// </summary>
        public MatchReviewProfileCollection()
        { }

        /// <summary>
        /// Load MatchReviewProfile Collection from xml
        /// </summary>
        public MatchReviewProfileCollection(String xml)
        {
            LoadFromXml(xml);
        }

        /// <summary>
        /// Initialize MatchReviewProfile collection from IList
        /// </summary>
        /// <param name="matchReviewProfileList">Source items</param>
        public MatchReviewProfileCollection(IList<MatchReviewProfile> matchReviewProfileList)
        {
            this._items = new Collection<MatchReviewProfile>(matchReviewProfileList);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Xml representation of MatchReviewProfile Collection
        /// </summary>
        public String ToXml()
        {
            return ToXml(true);
        }

        /// <summary>
        /// Get Xml representation of MatchReviewProfile Collection
        /// </summary>
        public String ToXml(Boolean withOuterNode)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            if (withOuterNode)
            {
                //MatchReviewProfiles node start
                xmlWriter.WriteStartElement("MatchReviewProfiles");
            }

            foreach (MatchReviewProfile item in _items)
            {
                xmlWriter.WriteRaw(item.ToXml());
            }

            if (withOuterNode)
            {
                //MatchReviewProfiles node end
                xmlWriter.WriteEndElement();
            }

            xmlWriter.Flush();

            //Get the actual XML
            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Loads MatchReviewProfile Collection from "MatchReviewProfiles" XML node
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            _items.Clear();
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "MatchReviewProfile")
                {
                    MatchReviewProfile item = new MatchReviewProfile();
                    item.LoadFromXml(child);
                    _items.Add(item);
                }
            }
        }

        /// <summary>
        /// Loads MatchReviewProfile Collection from XML
        /// </summary>
        public void LoadFromXml(String xml)
        {
            _items.Clear();
            if (!String.IsNullOrWhiteSpace(xml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlNode node = doc.SelectSingleNode("MatchReviewProfiles");
                if (node != null)
                {
                    LoadFromXml(node);
                }
            }
        }

        /// <summary>
        /// Gets the merge plan profile by identifier.
        /// </summary>
        /// <param name="mergePlanProfileId">The merge plan profile identifier.</param>
        /// <returns>Merge plan profile matching the specified id</returns>
        public MatchReviewProfile GetById(Int32 mergePlanProfileId)
        {
            if (_items != null && _items.Count > 0)
            {
                foreach (MatchReviewProfile profile in _items)
                {
                    if (profile.Id == mergePlanProfileId)
                    {
                        return profile;
                    }
                }
            }
            return null;
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone MatchReviewProfile collection
        /// </summary>
        /// <returns>Cloned MatchReviewProfile collection object</returns>
        public object Clone()
        {
            MatchReviewProfileCollection clonedRules = new MatchReviewProfileCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (MatchReviewProfile item in this._items)
                {
                    MatchReviewProfile clonedItem = item.Clone() as MatchReviewProfile;
                    clonedRules.Add(clonedItem);
                }
            }

            return clonedRules;
        }

        #endregion
    }
}