using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using MDM.Core;
using MDM.Interfaces.Exports;

namespace MDM.BusinessObjects.Exports
{
    /// <summary>
    /// Represents Collection of ExportProfile objects
    /// </summary>
    [DataContract]
    public class ExportProfileCollection : ICollection<ExportProfile>, IExportProfileCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting collection of ExportProfile
        /// </summary>
        [DataMember]
        private Collection<ExportProfile> _exportProfiles = new Collection<ExportProfile>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ExportProfile Collection
        /// </summary>
        public ExportProfileCollection() { }

        /// <summary>
        /// Initialize export profile from Xml value
        /// </summary>
        /// <param name="valuesAsXml">Export profile in xml format</param>
        public ExportProfileCollection(String valuesAsXml)
        {
            LoadExportProfile(valuesAsXml);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Check if ExportProfileCollection contains ExportProfile with given Id
        /// </summary>
        /// <param name="Id">Id using which ExportProfile is to be searched from collection</param>
        /// <returns>
        /// <para>true : If ExportProfile found in ExportProfileCollection</para>
        /// <para>false : If ExportProfile found not in ExportProfileCollection</para>
        /// </returns>
        public bool Contains(Int32 Id)
        {
            return GetExportProfile(Id) != null;
        }

        /// <summary>
        /// Remove exportSubscriber object from ExportProfileCollection
        /// </summary>
        /// <param name="exportProfileId">exportProfileId of exportSubscriber which is to be removed from collection</param>
        /// <returns>true if exportSubscriber is successfully removed; otherwise, false. This method also returns false if exportSubscriber was not found in the original collection</returns>
        public bool Remove(Int32 exportProfileId)
        {
            ExportProfile exportProfile = GetExportProfile(exportProfileId);

            if (exportProfile == null)
                throw new ArgumentException("No ExportProfile found for given Id :" + exportProfileId);
            
            return this.Remove(exportProfile);
        }

        /// <summary>
        /// Get Xml representation of ExportProfileCollection
        /// </summary>
        /// <returns>Xml representation of ExportProfileCollection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<ExportProfiles>";

            if (this._exportProfiles != null && this._exportProfiles.Count > 0)
            {
                foreach (ExportProfile exportSubscriber in this._exportProfiles)
                {
                    returnXml = String.Concat(returnXml, exportSubscriber.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</ExportProfiles>");

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of ExportProfileCollection
        /// </summary>
        /// <returns>Xml representation of ExportProfileCollection</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String returnXml = String.Empty;

            returnXml = "<ExportProfiles>";

            if (this._exportProfiles != null && this._exportProfiles.Count > 0)
            {
                foreach (ExportProfile exportSubscriber in this._exportProfiles)
                {
                    returnXml = String.Concat(returnXml, exportSubscriber.ToXml(serialization));
                }
            }

            returnXml = String.Concat(returnXml, "</ExportProfiles>");

            return returnXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is ExportProfileCollection)
            {
                ExportProfileCollection objectToBeCompared = obj as ExportProfileCollection;

                Int32 exportSubscribersUnion = this._exportProfiles.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 exportSubscribersIntersect = this._exportProfiles.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if (exportSubscribersUnion != exportSubscribersIntersect)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            Int32 hashCode = 0;

            foreach (ExportProfile ExportProfile in this._exportProfiles)
            {
                hashCode += ExportProfile.GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Get specific export Profile
        /// </summary>
        /// <param name="exportProfileId"></param>
        /// <returns></returns>
        public ExportProfile GetExportProfile(Int32 exportProfileId)
        {
            foreach (ExportProfile profile in this._exportProfiles)
            {
                if (profile.Id == exportProfileId)
                {
                    return profile;
                }
            }

            return null;
        }

        /// <summary>
        /// Get specific export Profile
        /// </summary>
        /// <param name="exportProfileName">Indicates the profile name</param>
        /// <returns>Returns the export profile if requested profile available in system.</returns>
        public ExportProfile GetExportProfile(String exportProfileName)
        {
            foreach (ExportProfile profile in this._exportProfiles)
            {
                if (String.Compare(profile.Name, exportProfileName, true) == 0)
                {
                    return profile;
                }
            }

            return null;
        }

        /// <summary>
        /// Get export Profile by Profile Type
        /// </summary>
        /// <param name="exportProfileType">Indicates the export profile type</param>
        /// <returns>Collection of export profile</returns>
        public ExportProfileCollection GetExportProfilesByProfileType(ExportProfileType exportProfileType)
        {
            ExportProfileCollection exportProfilesToReturn = new ExportProfileCollection();

            if (this._exportProfiles != null && this._exportProfiles.Count > 0)
            {
                foreach (ExportProfile exportProfile in this._exportProfiles)
                {
                    if (exportProfile.ProfileType == exportProfileType)
                    {
                        exportProfilesToReturn.Add(exportProfile);
                    }
                }
            }

            return exportProfilesToReturn;
        }

        /// <summary>
        /// Get collection of attribute identifiers specified in attribute filter scope in export profile
        /// </summary>
        /// <returns></returns>
        public Collection<Int32> GetAttributeFilterIds()
        {
            Collection<Int32> attrIds = new Collection<Int32>();

            if(this != null && this.Count > 0)
            {
                foreach (EntityExportProfile exportProfile in this)
                {
                    EntityExportSyndicationProfileData profileData = (EntityExportSyndicationProfileData)exportProfile.DataObject;

                    if (profileData != null && profileData.ScopeSpecification != null)
                    {
                        ScopeSpecification scopeData = profileData.ScopeSpecification;

                        ExportScopeCollection containerRuleScopes = scopeData.ExportScopes;
                        if (containerRuleScopes != null && containerRuleScopes.Count > 0)
                        {
                            foreach (ExportScope containerRuleScope in containerRuleScopes)
                            {
                                SearchAttributeRuleGroupCollection searchAttributeRuleGroups = containerRuleScope.SearchAttributeRuleGroups;

                                if (searchAttributeRuleGroups != null)
                                {
                                    SearchAttributeRuleGroup searchAttributeRuleGroup = searchAttributeRuleGroups.FirstOrDefault();

                                    if (searchAttributeRuleGroup != null)
                                    {
                                        SearchAttributeRuleCollection searchAttributeRules = searchAttributeRuleGroup.SearchAttributeRules;

                                        if (searchAttributeRules != null && searchAttributeRules.Count > 0)
                                        {
                                            foreach (SearchAttributeRule searchAttrRule in searchAttributeRules)
                                            {
                                                if (!attrIds.Contains(searchAttrRule.Attribute.Id))
                                                {
                                                    attrIds.Add(searchAttrRule.Attribute.Id);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return attrIds;
        }

        #endregion Public Methods

        #region IEnumerable<ExportProfile> Members

        /// <summary>
        /// Returns an enumerator that iterates through a AttribureVersionCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<ExportProfile> GetEnumerator()
        {
            return this._exportProfiles.GetEnumerator();
        }

        #endregion IEnumerable<ExportProfile> Members

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityTypeCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion IEnumerable Members


        #region ICollection<ExportProfile> Members

        /// <summary>
        /// Add ExportProfile object in collection
        /// </summary>
        /// <param name="exportProfile">ExportProfile to add in collection</param>
        public void Add(IExportProfile exportProfile)
        {
            if (exportProfile != null)
            {
                this.Add((IExportProfile)exportProfile);
            }
        }

        /// <summary>
        /// Add ExportProfile object in collection
        /// </summary>
        /// <param name="exportProfile">ExportProfile to add in collection</param>
        public void Add(ExportProfile exportProfile)
        {
            this._exportProfiles.Add(exportProfile);
        }

        /// <summary>
        /// Removes all ExportProfile from collection
        /// </summary>
        public void Clear()
        {
            this._exportProfiles.Clear();
        }

        /// <summary>
        /// Determines whether the ExportProfileCollection contains a specific ExportProfile
        /// </summary>
        /// <param name="exportProfile">The ExportProfile object to locate in the ExportProfileCollection.</param>
        /// <returns>
        /// <para>true : If ExportProfile found in ExportProfileCollection</para>
        /// <para>false : If ExportProfile found not in ExportProfileCollection</para>
        /// </returns>
        public bool Contains(IExportProfile exportProfile)
        {
            if (exportProfile != null)
            {
                return this.Contains((IExportProfile)exportProfile);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether the ExportProfileCollection contains a specific ExportProfile
        /// </summary>
        /// <param name="exportProfile">The ExportProfile object to locate in the ExportProfileCollection.</param>
        /// <returns>
        /// <para>true : If ExportProfile found in ExportProfileCollection</para>
        /// <para>false : If ExportProfile found not in ExportProfileCollection</para>
        /// </returns>
        public bool Contains(ExportProfile exportProfile)
        {
            return this._exportProfiles.Contains(exportProfile);
        }

        /// <summary>
        /// Copies the elements of the ExportProfileCollection to a
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(ExportProfile[] array, int arrayIndex)
        {
            this._exportProfiles.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of ExportProfile in ExportProfileCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._exportProfiles.Count;
            }
        }

        /// <summary>
        /// Check if ExportProfileCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific ExportProfile from the ExportProfileCollection.
        /// </summary>
        /// <param name="exportProfile">The ExportProfile object to remove from the ExportProfileCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(IExportProfile exportProfile)
        {
            if (exportProfile != null)
            {
                return this.Remove((IExportProfile)exportProfile);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific ExportProfile from the ExportProfileCollection.
        /// </summary>
        /// <param name="exportProfile">The ExportProfile object to remove from the ExportProfileCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(ExportProfile exportProfile)
        {
            return this._exportProfiles.Remove(exportProfile);
        }

        #endregion ICollection<ExportProfile> Members

        #region PrivateMethods

        private void LoadExportProfile(string valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExportProfile")
                        {
                            String profileXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(profileXml))
                            {
                                ExportProfile exportProfile = new ExportProfile(profileXml);
                                this.Add(exportProfile);
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

        #endregion PrivateMethods
    }
}
