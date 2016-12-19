using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQMNormalization
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies NormalizationProfiles Collection
    /// </summary>
    [DataContract]
    public class NormalizationProfilesCollection : INormalizationProfilesCollection, ICloneable
    {
        #region Fields

        [DataMember]
        private Collection<NormalizationProfile> _normalizationProfiles = new Collection<NormalizationProfile>();

        private const String XmlNodeName = "NormalizationProfileCollection";
        private const String ChildXmlNodeName = "NormalizationProfile";

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public NormalizationProfilesCollection()
        {
            this._normalizationProfiles = new Collection<NormalizationProfile>();
        }

        /// <summary>
        /// Initialize NormalizationProfilesCollection from IList
        /// </summary>
        /// <param name="normalizationProfilesList">IList of NormalizationProfiles</param>
        public NormalizationProfilesCollection(IList<NormalizationProfile> normalizationProfilesList)
        {
            this._normalizationProfiles = new Collection<NormalizationProfile>(normalizationProfilesList);
        }

        /// <summary>
        /// Initialize NormalizationProfilesCollection from xml
        /// </summary>
        /// <param name="xml">Xml String</param>
        public NormalizationProfilesCollection(String xml)
            : this()
        {
            LoadFromXml(xml);
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Indexer to getting data Normalization Profiles by Id
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public NormalizationProfile this[Int32 index]   
        {            
            get { return _normalizationProfiles [index]; }                        
        }

        #endregion

        #region Methods

        #region Public Method

        /// <summary>
        /// Clone Normalization profiles collection
        /// </summary>
        /// <returns>Cloned Normalization profiles collection object</returns>
        public object Clone()
        {
            NormalizationProfilesCollection clonedProfiles = new NormalizationProfilesCollection();

            if (this._normalizationProfiles != null && this._normalizationProfiles.Count > 0)
            {
                foreach (NormalizationProfile profile in this._normalizationProfiles)
                {
                    NormalizationProfile clonedProfile = profile.Clone() as NormalizationProfile;
                    clonedProfiles.Add(clonedProfile);
                }
            }

            return clonedProfiles;
        }

        /// <summary>
        /// Remove NormalizationProfile object from NormalizationProfileCollection
        /// </summary>
        /// <param name="normalizationProfileId">Id of NormalizationProfile which information is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public Boolean Remove(Int32 normalizationProfileId)
        {
            NormalizationProfile normalizationProfile = Get(normalizationProfileId);
            if (normalizationProfile == null)
            {
                return false;
            }
            return this.Remove(normalizationProfile);
        }

        /// <summary>
        /// Determines whether two Object instances are equal
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is NormalizationProfilesCollection)
            {
                NormalizationProfilesCollection objectToBeCompared = obj as NormalizationProfilesCollection;
                List<NormalizationProfile> normalizationProfiles = this._normalizationProfiles.ToList();
                List<NormalizationProfile> profiles = objectToBeCompared.ToList();

                Int32 normalizationProfileUnion = normalizationProfiles.Union(profiles).Count();
                Int32 normalizationProfilesIntersect = normalizationProfiles.Intersect(profiles).Count();

                return (normalizationProfileUnion == normalizationProfilesIntersect);
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object</returns>
        public override Int32 GetHashCode()
        {
            return this._normalizationProfiles.Sum(item => item.GetHashCode());
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
                xmlWriter.WriteStartElement(XmlNodeName);
                foreach (NormalizationProfile item in _normalizationProfiles)
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
            _normalizationProfiles.Clear();
            if (node == null)
            {
                return;
            }
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == ChildXmlNodeName)
                {
                    NormalizationProfile item = new NormalizationProfile();
                    item.LoadFromXml(child, false);
                    _normalizationProfiles.Add(item);
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
            XmlNode node = doc.SelectSingleNode(XmlNodeName);
            if (node != null)
            {
                LoadFromXml(node);
            }
        }

        #endregion

        #endregion

        #region Private Method

        /// <summary>
        /// Get NormalizationProfile from current NormalizationProfileCollection based on NormalizationProfiles Id
        /// </summary>
        /// <param name="normalizationProfileId">Id of NormalizationProfiles which information is to be searched</param>
        /// <returns>NormalizationProfile having given NormalizationProfileId messageCode</returns>
        private NormalizationProfile Get(Int32 normalizationProfileId)
        {
            return this._normalizationProfiles.FirstOrDefault(normalizationProfile => normalizationProfile.Id == normalizationProfileId);
        }

        #endregion

        #endregion

        #region ICollection<NormalizationProfile> Members

        /// <summary>
        /// Add NormalizationProfile object in collection
        /// </summary>
        /// <param name="item">NormalizationProfile to add in collection</param>
        public void Add(NormalizationProfile item)
        {
            this._normalizationProfiles.Add(item);
        }

        /// <summary>
        /// Add INormalizationProfile object in collection
        /// </summary>
        /// <param name="item">INormalizationProfile to add in collection</param>
        public void Add(INormalizationProfile item)
        {
            this._normalizationProfiles.Add((NormalizationProfile)item);
        }

        /// <summary>
        /// Removes all NormalizationProfiles from collection
        /// </summary>
        public void Clear()
        {
            this._normalizationProfiles.Clear();
        }

        /// <summary>
        /// Determines whether the NormalizationProfilesCollection contains a specific NormalizationProfile
        /// </summary>
        /// <param name="item">The NormalizationProfile object to locate in the NormalizationProfilesCollection</param>
        /// <returns>
        /// <para>true : If NormalizationProfile found in NormalizationProfilesCollection</para>
        /// <para>false : If NormalizationProfile found not in NormalizationProfilesCollection</para>
        /// </returns>
        public Boolean Contains(NormalizationProfile item)
        {
            return this._normalizationProfiles.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the NormalizationProfilesCollection to an
        /// System.Array, starting at a particular System.Array index
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements copied from NormalizationProfilesCollection. The System.Array must have zero-based indexing
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins</param>
        public void CopyTo(NormalizationProfile[] array, Int32 arrayIndex)
        {
            this._normalizationProfiles.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of NormalizationProfile in NormalizationProfilesCollection
        /// </summary>
        public Int32 Count
        {
            get
            {
                return this._normalizationProfiles.Count;
            }
        }

        /// <summary>
        /// Check if NormalizationProfilesCollection is read-only
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the NormalizationProfilesCollection
        /// </summary>
        /// <param name="item">The NormalizationProfile object to remove from the NormalizationProfilesCollection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original NormalizationProfilesCollection</returns>
        public Boolean Remove(NormalizationProfile item)
        {
            return this._normalizationProfiles.Remove(item);
        }

        #endregion

        #region IEnumerable<NormalizationProfiles> Members

        /// <summary>
        /// Returns an enumerator that iterates through a NormalizationProfilesCollection
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection</returns>
        public IEnumerator<NormalizationProfile> GetEnumerator()
        {
            return this._normalizationProfiles.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a NormalizationProfilesCollection
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._normalizationProfiles.GetEnumerator();
        }

        #endregion
    }
}
