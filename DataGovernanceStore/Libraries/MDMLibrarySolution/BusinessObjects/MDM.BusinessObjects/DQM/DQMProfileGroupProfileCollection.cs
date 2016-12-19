using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    /// <summary>
    /// Represents collection of DQM profile group
    /// </summary>
    [DataContract]
    public class DQMProfileGroupProfileCollection : ICollection<DQMProfileGroupProfile>, ICloneable
    {
        private readonly Collection<DQMProfileGroupProfile> _items;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Profile Group Profile Collection
        /// </summary>
        public DQMProfileGroupProfileCollection()
        {
            _items = new Collection<DQMProfileGroupProfile>();
        }

        /// <summary>
        /// Initialize Profile Group Profiles collection from IList
        /// </summary>
        /// <param name="DQMProfileGroupProfilesList">Source items</param>
        public DQMProfileGroupProfileCollection(IList<DQMProfileGroupProfile> DQMProfileGroupProfilesList)
        {
            this._items = new Collection<DQMProfileGroupProfile>(DQMProfileGroupProfilesList);
        }

        /// <summary>
        /// Initialize Profile Group Profiles collection from xml
        /// </summary>
        /// <param name="xml">Xml String</param>
        public DQMProfileGroupProfileCollection(String xml)
            : this()
        {
            LoadFromXml(xml);
        }

        #endregion
        
        #region ICloneable Members

        /// <summary>
        /// Clone Profile Group profiles collection
        /// </summary>
        /// <returns>Cloned Profile Group profiles collection object</returns>
        public object Clone()
        {
            var clones = new DQMProfileGroupProfileCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (DQMProfileGroupProfile item in this._items)
                {
                    var clone = item.Clone() as DQMProfileGroupProfile;
                    clones.Add(clone);
                }
            }

            return clones;
        }

        #endregion

        /// <summary>
        /// Find DQM profile group from DQM profile group collection based on index passed in parameter
        /// </summary>
        /// <param name="index">Indicates the index based on which DQM profile group is returned</param>
        /// <returns>Returns DQM profile group from DQM profile group collection based on index passed in parameter</returns>
        public DQMProfileGroupProfile this[Int32 index]
        {
            get { return _items[index]; }
        }

        /// <summary>
        /// Get an enumerator that iterates through a DQM profile group collection 
        /// </summary>
        /// <returns>Returns an IEnumerator object that iterate through a DQM profile group collection</returns>
        public IEnumerator<DQMProfileGroupProfile> GetEnumerator()
        {
            return this._items.GetEnumerator();
        }

        /// <summary>
        /// Add DQM profile group object in collection
        /// </summary>
        /// <param name="item">Indicates the DQM profile group object to add in collection</param>
        public void Add(DQMProfileGroupProfile item)
        {
            this._items.Add(item);
        }

        /// <summary>
        /// Removes all DQM profile group from its collection
        /// </summary>
        public void Clear()
        {
            this._items.Clear();
        }

        /// <summary>
        /// Determines whether the DQM profile group collection contains a specific DQM profile group
        /// </summary>
        /// <param name="item">Indicates the DQM profile group object to locate in the DQM profile group collection</param>
        /// <returns>
        /// <para>true : If DQM profile group is found in DQM profile group collection</para>
        /// <para>false : If DQM profile group is not found in DQM profile group collection</para>
        /// </returns>
        public bool Contains(DQMProfileGroupProfile item)
        {
            return this._items.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the DQM profile group collection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array">Indicates the one-dimensional System.Array that is the destination of the elements
        ///  copied from DQM profile group collection. The System.Array must have zero-based indexing</param>
        /// <param name="arrayIndex">Indicates the zero-based index in array at which copying begins</param>
        public void CopyTo(DQMProfileGroupProfile[] array, int arrayIndex)
        {
            this._items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the DQM profile group collection
        /// </summary>
        /// <param name="item">Indicates the DQM profile group object to remove from the DQM profile group collection</param>
        /// <returns>
        /// Returns true if item is successfully removed; otherwise, false. 
        /// This method also returns false if item was not found in the original DQM profile group collection
        /// </returns>
        public bool Remove(DQMProfileGroupProfile item)
        {
            return this._items.Remove(item);
        }

        /// <summary>
        /// Get the count of DQM profile group in DQM profile group collection
        /// </summary>
        public int Count
        {
            get { return this._items.Count; }
        }

        /// <summary>
        /// Check if DQM profile group is read only
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }

        }

        /// <summary>
        /// Get an enumerator that iterates through a DQM profile group collection
        /// </summary>
        /// <returns>Returns an enumerator that iterates through a DQM profile group collection</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Get DQM profile group based on profile group id
        /// </summary>
        /// <param name="profileId">Indicates the profile group id based on which the DQM profile group is returned</param>
        /// <returns>Returns DQM profile group based on profile group id</returns>
        public DQMProfileGroupProfile GetByProfileId(Int32 profileId)
        {
            DQMProfileGroupProfile profileGroupProfile = null;

            if (this._items != null)
            {
                foreach (DQMProfileGroupProfile profileGroup in this._items)
                {
                    if (profileGroup.ProfileId == profileId)
                    {
                        profileGroupProfile = profileGroup;
                        break;
                    }
                }
            }

            return profileGroupProfile;
        }
        
        #region Xml Serialization

        /// <summary>
        /// Gets Xml representation of current collection
        /// </summary>
        /// <returns>Returns Xml representation of current collection as string</returns>
        public String ToXml()
        {
            String result = String.Empty;

            using (StringWriter sw = new StringWriter())
            using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
            {
                xmlWriter.WriteStartElement("DQMProfileGroupProfileCollection");
                foreach (DQMProfileGroupProfile item in _items)
                {
                    xmlWriter.WriteRaw(item.ToXml());
                }
                xmlWriter.WriteEndElement();
                xmlWriter.Flush();

                //Get the actual XML
                result = sw.ToString();
            }

            return result;
        }

        /// <summary>
        /// Loads current collection from provided XmlNode
        /// </summary>
        /// <param name="node">XmlNode for deserialization</param>
        public void LoadFromXml(XmlNode node)
        {
            _items.Clear();
            if (node == null)
            {
                return;
            }
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "DQMProfileGroupProfile")
                {
                    DQMProfileGroupProfile item = new DQMProfileGroupProfile();
                    item.LoadFromXml(child);
                    _items.Add(item);
                }
            }
        }

        /// <summary>
        /// Loads current collection from provided Xml
        /// </summary>
        /// <param name="xml">Xml string for deserialization</param>
        public void LoadFromXml(String xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode node = doc.SelectSingleNode("DQMProfileGroupProfileCollection");
            if (node != null)
            {
                LoadFromXml(node);
            }
        }

        #endregion

    }
}
