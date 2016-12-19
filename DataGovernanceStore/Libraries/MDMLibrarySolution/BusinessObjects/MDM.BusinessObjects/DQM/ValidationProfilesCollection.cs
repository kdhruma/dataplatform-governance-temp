using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Collections;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies ValidationProfiles Collection
    /// </summary>
    [DataContract]
    public class ValidationProfilesCollection : IValidationProfilesCollection
    {
        #region Fields

        private const String XmlNodeName = "ValidationProfileCollection";
        private const String ChildXmlNodeName = "ValidationProfile";

        [DataMember]
        private Collection<ValidationProfile> _validationProfiles = new Collection<ValidationProfile>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ValidationProfilesCollection()
        {
            this._validationProfiles = new Collection<ValidationProfile>();
        }

        /// <summary>
        /// Initialize ValidationProfilesCollection from IList
        /// </summary>
        /// <param name="validationProfilesList">IList of ValidationProfiles</param>
        public ValidationProfilesCollection(IList<ValidationProfile> validationProfilesList)
        {
            this._validationProfiles = new Collection<ValidationProfile>(validationProfilesList);
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Indexer to getting data quality classes by Id
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ValidationProfile this[Int32 index]   
        {            
            get { return _validationProfiles [index]; }                        
        }

        #endregion

        #region Methods

        #region Public Method

        /// <summary>
        /// Clone validation profiles collection
        /// </summary>
        /// <returns>Cloned validation profiles collection object</returns>
        public IValidationProfilesCollection Clone()
        {
            ValidationProfilesCollection clonedProfiles = new ValidationProfilesCollection();

            if (this._validationProfiles != null && this._validationProfiles.Count > 0)
            {
                foreach (ValidationProfile profile in this._validationProfiles)
                {
                    ValidationProfile clonedProfile = profile.Clone() as ValidationProfile;
                    clonedProfiles.Add(clonedProfile);
                }
            }

            return clonedProfiles;
        }

        /// <summary>
        /// Remove ValidationProfile object from ValidationProfileCollection
        /// </summary>
        /// <param name="validationProfileId">Id of ValidationProfile which information is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public Boolean Remove(Int32 validationProfileId)
        {
            ValidationProfile validationProfile = Get(validationProfileId);
            if (validationProfile == null)
            {
                return false;
            }
            return this.Remove(validationProfile);
        }

        /// <summary>
        /// Determines whether two Object instances are equal
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false</returns>
        public override Boolean Equals(object obj)
        {
            if (obj != null)
            {
                ValidationProfilesCollection objectToBeCompared = obj as ValidationProfilesCollection;
                if (objectToBeCompared != null)
                {
                    if (ReferenceEquals(this, objectToBeCompared))
                    {
                        return true;
                    }

                    List<ValidationProfile> validationProfiles = this._validationProfiles.ToList();
                    List<ValidationProfile> profiles = objectToBeCompared.ToList();

                    Int32 validationProfileUnion = validationProfiles.Union(profiles).Count();
                    Int32 validationProfilesIntersect = validationProfiles.Intersect(profiles).Count();

                    return (validationProfileUnion == validationProfilesIntersect);
                }
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object</returns>
        public override Int32 GetHashCode()
        {
            return this._validationProfiles.Sum(item => item.GetHashCode());
        }

        /// <summary>
        /// Get Xml representation of ValidationProfile Collection
        /// </summary>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement(XmlNodeName);

            foreach (ValidationProfile item in _validationProfiles)
            {
                xmlWriter.WriteRaw(item.ToXml());
            }

            // ValidationProfileCollection node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Loads ValidationProfile from "ValidationProfile" XML node
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            _validationProfiles.Clear();
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == ChildXmlNodeName)
                {
                    ValidationProfile item = new ValidationProfile();
                    item.LoadPropertiesOnlyFromXml(child);
                    _validationProfiles.Add(item);
                }
            }
        }

        /// <summary>
        /// Loads ValidationProfile Collection from XML
        /// </summary>
        public void LoadFromXml(String xml)
        {
            _validationProfiles.Clear();
            if (!String.IsNullOrWhiteSpace(xml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlNode node = doc.SelectSingleNode(XmlNodeName);
                if (node != null)
                {
                    LoadFromXml(node);
                }
            }
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Get ValidationProfile from current ValidationProfileCollection based on DataQualityClass Id
        /// </summary>
        /// <param name="validationProfileId">Id of DataQualityClass which information is to be searched</param>
        /// <returns>ValidationProfile having given ValidationProfileId messageCode</returns>
        private ValidationProfile Get(Int32 validationProfileId)
        {
            return this._validationProfiles.FirstOrDefault(validationProfile => validationProfile.Id == validationProfileId);
        }

        #endregion

        #endregion

        #region ICollection<ValidationProfile> Members

        /// <summary>
        /// Add ValidationProfile object in collection
        /// </summary>
        /// <param name="item">ValidationProfile to add in collection</param>
        public void Add(ValidationProfile item)
        {
            this._validationProfiles.Add(item);
        }

        /// <summary>
        /// Add IValidationProfile object in collection
        /// </summary>
        /// <param name="item">IValidationProfile to add in collection</param>
        public void Add(IValidationProfile item)
        {
            this._validationProfiles.Add((ValidationProfile)item);
        }

        /// <summary>
        /// Removes all ValidationProfiles from collection
        /// </summary>
        public void Clear()
        {
            this._validationProfiles.Clear();
        }

        /// <summary>
        /// Determines whether the ValidationProfilesCollection contains a specific ValidationProfile
        /// </summary>
        /// <param name="item">The ValidationProfile object to locate in the ValidationProfilesCollection</param>
        /// <returns>
        /// <para>true : If ValidationProfile found in ValidationProfilesCollection</para>
        /// <para>false : If ValidationProfile found not in ValidationProfilesCollection</para>
        /// </returns>
        public Boolean Contains(ValidationProfile item)
        {
            return this._validationProfiles.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the ValidationProfilesCollection to an
        /// System.Array, starting at a particular System.Array index
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements copied from ValidationProfilesCollection. The System.Array must have zero-based indexing
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins</param>
        public void CopyTo(ValidationProfile[] array, Int32 arrayIndex)
        {
            this._validationProfiles.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of ValidationProfile in ValidationProfilesCollection
        /// </summary>
        public Int32 Count
        {
            get
            {
                return this._validationProfiles.Count;
            }
        }

        /// <summary>
        /// Check if ValidationProfilesCollection is read-only
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the ValidationProfilesCollection
        /// </summary>
        /// <param name="item">The ValidationProfile object to remove from the ValidationProfilesCollection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original ValidationProfilesCollection</returns>
        public Boolean Remove(ValidationProfile item)
        {
            return this._validationProfiles.Remove(item);
        }

        #endregion

        #region IEnumerable<ValidationProfiles> Members

        /// <summary>
        /// Returns an enumerator that iterates through a ValidationProfilesCollection
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection</returns>
        public IEnumerator<ValidationProfile> GetEnumerator()
        {
            return this._validationProfiles.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a ValidationProfilesCollection
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._validationProfiles.GetEnumerator();
        }

        #endregion
    }
}
