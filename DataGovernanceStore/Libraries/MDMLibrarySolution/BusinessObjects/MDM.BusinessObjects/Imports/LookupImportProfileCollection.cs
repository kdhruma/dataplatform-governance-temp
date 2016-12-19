using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace MDM.BusinessObjects.Imports
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents a collection of the Lookup Import Profile.
    /// </summary>
    [DataContract]
    public class LookupImportProfileCollection : InterfaceContractCollection<ILookupImportProfile, LookupImportProfile>, ILookupImportProfileCollection
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public LookupImportProfileCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input parameter
        /// </summary>
        public LookupImportProfileCollection(String valueAsXml)
        {
            LoadLookupImportProfileCollection(valueAsXml);
        }

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            Boolean isEqual = false;

            if (obj is LookupImportProfileCollection)
            {
                LookupImportProfileCollection objectToBeCompared = obj as LookupImportProfileCollection;
                Int32 lkpImportProfilesUnion = this._items.ToList().Union(objectToBeCompared._items.ToList()).Count();
                Int32 lkpImportProfilesIntersect = this._items.ToList().Intersect(objectToBeCompared._items.ToList()).Count();

                if (lkpImportProfilesUnion != lkpImportProfilesIntersect)
                {
                    isEqual = false;
                }
                isEqual = true;
            }

            return isEqual;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            Int32 hashCode = 0;
            foreach (LookupImportProfile attr in this._items)
            {
                hashCode += attr.GetHashCode();
            }
            return hashCode;
        }

        #endregion

        #region Private Methods

        ///<summary>
        /// Load LookupImportProfileCollection object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        ///     <LookupImportProfiles>
        ///     <LookupImportProfile Id="-1" Name="MDMCenter - RSXml 1.1 - Default Lookup Profile" ImportType="LookupImport" Enabled="true" FileWatcherFolderName="RSXmlLookup11">
        ///     <ExecutionSteps>
        ///         <ExecutionStep Name="Process" StepType="Core" AssemblyFileName="" ClassFullName="" AbortOnError="true" Enabled="true" />
        ///     </ExecutionSteps>
        ///     <ReaderSettings />
        ///     <InputSpecifications Reader="RSXmlLookup11">
        ///     </InputSpecifications>
        ///     <LookupJobProcessingOptions NumberofLookupThreads="2" NumberofRecordThreadsPerLookupThread="2" BatchSize="100" />
        ///     </LookupImportProfile>	
        ///     </LookupImportProfiles>
        /// </example>
        private void LoadLookupImportProfileCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "LookupImportProfile")
                        {
                            String lkpLmportProfileXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(lkpLmportProfileXml))
                            {
                                LookupImportProfile lkpImportProfile = new LookupImportProfile(lkpLmportProfileXml);

                                this.Add(lkpImportProfile);
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

        #region ILookupImportProfileCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of LookupImportProfileCollection object
        /// </summary>
        /// <returns>Xml string representing the LookupImportProfileCollection</returns>
        public String ToXml()
        {
            String lkpImportProfilesXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (LookupImportProfile lkpImportProfile in this._items)
            {
                builder.Append(lkpImportProfile.ToXml());
            }

            lkpImportProfilesXml = String.Format("<LookupImportProfiles>{0}</LookupImportProfiles>", builder.ToString());
            return lkpImportProfilesXml;
        }

        /// <summary>
        /// Get Xml representation of ImportProfileCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String lkpLmportProfilesXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (LookupImportProfile lkpImportProfile in this._items)
            {
                builder.Append(lkpImportProfile.ToXml(serialization));
            }

            lkpLmportProfilesXml = String.Format("<LookupImportProfiles>{0}</LookupImportProfiles>", builder.ToString());
            return lkpLmportProfilesXml;
        }

        /// <summary>
        /// Determines whether the LookpImportProfileCollection contains a specific profileId.
        /// </summary>
        /// <param name="profileId">The importProfile object to locate in the LookupImportProfileCollection.</param>
        /// <returns>
        /// <para>true : If LookupImportProfile found in LookupImportProfileCollection</para>
        /// <para>false : If LookupimportProfile found not in LookupImportProfileCollection</para>
        /// </returns>
        public bool Contains(Int32 profileId)
        {
            if (this.GetLookupImportProfile(profileId) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove LookupImportProfile object from LookupImportProfileCollection
        /// </summary>
        /// <param name="importProfileId">ImportProfileId of LookupImportProfile which is to be removed from collection</param>
        /// <returns>true if LookupImportProfile is successfully removed; otherwise, false. This method also returns false if lookupimportProfile was not found in the original collection</returns>
        public bool Remove(Int32 importProfileId)
        {
            ILookupImportProfile lkpImportProfile = this.GetLookupImportProfile(importProfileId);

            if (lkpImportProfile == null)
                throw new ArgumentException("No LookupImportProfile found for given Id :" + importProfileId);
            else
                return this.Remove(lkpImportProfile);
        }

        #endregion ToXml methods

        #endregion ILookupImportProfileCollection Memebers
        
        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a LookupImportProfileCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public new IEnumerator<ILookupImportProfile> GetEnumerator()
        {
            return this._items.GetEnumerator();
        }
        #endregion 

        #region LookupImportProfile Get

        /// <summary>
        /// Get the Lookup Import profile based on requested profile id.
        /// </summary>
        /// <param name="profileId">Indicates the profile Id</param>
        /// <returns>Get the Lookup Import profile Interface</returns>
        public ILookupImportProfile GetLookupImportProfile(Int32 profileId)
        {
            // Initialize to NULL as the caller expects a null object back.
            LookupImportProfile LkpImportProfile = null;

            if (this._items == null)
            {
                throw new NullReferenceException("There are no import profiles to search in");
            }

            if (profileId <= 0)
            {
                throw new ArgumentException("Lookup Import Profile Id must be greater than 0", profileId.ToString());
            }

            foreach (LookupImportProfile profile in this._items)
            {
                if (profile.Id == profileId)
                {
                    LkpImportProfile = profile;
                    break;
                }
            }

            return LkpImportProfile;
        }

        /// <summary>
        /// Get the profile based on the file watcher folder name
        /// </summary>
        /// <param name="fileWatcherFolderName">Indicates the file watcher folder name</param>
        /// <returns>Get the LookupImport profile object</returns>
        public LookupImportProfile GetLookupImportProfile(String fileWatcherFolderName)
        {
            if (this._items == null)
            {
                throw new NullReferenceException("There are no lookup import profiles to search in");
            }

            if (String.IsNullOrEmpty(fileWatcherFolderName))
            {
                throw new ArgumentException("Lookup Import Profile Id must be greater than 0", fileWatcherFolderName);
            }
            LookupImportProfile lkpImportProfile = null;

            foreach (LookupImportProfile profile in this._items)
            {
                if (profile.FileWatcherFolderName.ToLower() == fileWatcherFolderName.ToLower())
                {
                    lkpImportProfile = profile;
                    break;
                }
            }

            return lkpImportProfile;
        }

        #endregion LookupImportProfile Get

    }
}
