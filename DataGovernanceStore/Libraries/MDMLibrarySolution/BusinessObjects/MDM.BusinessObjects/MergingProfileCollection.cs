using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies MergingProfiles Collection
    /// </summary>
    [DataContract]
    public class MergingProfileCollection: InterfaceContractCollection<IMergingProfile, MergingProfile>, IMergingProfileCollection
    {
        #region Fields

        /// <summary>
        /// Describes name of outer node in ProfileData column xml
        /// </summary>
        private const String OuterNodeName = "MergingProfiles";

        /// <summary>
        /// Describes name of child nodes in xml
        /// </summary>
        private const String ChildNodesName = "MergingProfile";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MergingProfiles Collection
        /// </summary>
        public MergingProfileCollection()
        { }

        /// <summary>
        /// Load MergingProfileCollection from xml
        /// </summary>
        public MergingProfileCollection(String xml)
        {
            LoadFromXml(xml);
        }

        /// <summary>
        /// Initialize MergingProfiles collection from IList
        /// </summary>
        /// <param name="mergingProfileList">Source items</param>
        public MergingProfileCollection(IList<MergingProfile> mergingProfileList)
        {
            this._items = new Collection<MergingProfile>(mergingProfileList);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Xml representation of MergeProfile Collection
        /// </summary>
        public String ToXml()
        {
            return ToXml(true);
        }

        /// <summary>
        /// Get Xml representation of MergeProfile Collection
        /// </summary>
        public String ToXml(Boolean withOuterNode)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            if (withOuterNode)
            {
                //MergeProfiles node start
                xmlWriter.WriteStartElement(OuterNodeName);
            }

            foreach (MergingProfile item in _items)
            {
                xmlWriter.WriteRaw(item.ToXml());
            }

            if (withOuterNode)
            {
                //MergeProfiles node end
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
        /// Loads MergeProfile Collection from "MergeProfiles" XML node
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            _items.Clear();
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == ChildNodesName)
                {
                    MergingProfile item = new MergingProfile();
                    item.LoadFromXml(child, false);
                    _items.Add(item);
                }
            }
        }

        /// <summary>
        /// Loads MergeProfile Collection from XML
        /// </summary>
        public void LoadFromXml(String xml)
        {
            _items.Clear();
            if (!String.IsNullOrWhiteSpace(xml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlNode node = doc.SelectSingleNode(OuterNodeName);
                if (node != null)
                {
                    LoadFromXml(node);
                }
            }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone MergeProfile collection
        /// </summary>
        /// <returns>Cloned MergeProfile collection object</returns>
        public object Clone()
        {
            MergingProfileCollection clonedProfiles = new MergingProfileCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (MergingProfile item in this._items)
                {
                    MergingProfile clonedItem = item.Clone() as MergingProfile;
                    clonedProfiles.Add(clonedItem);
                }
            }

            return clonedProfiles;
        }

        #endregion
    }
}
