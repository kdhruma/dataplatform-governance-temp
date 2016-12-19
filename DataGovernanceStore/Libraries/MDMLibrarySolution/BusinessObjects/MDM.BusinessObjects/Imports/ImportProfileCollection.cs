using System;
using System.Text;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace MDM.BusinessObjects.Imports
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.BusinessObjects.Interfaces;

    /// <summary>
    /// Specifies the ImportProfile Value Instance Collection for the Object
    /// </summary>
    [DataContract]
    public class ImportProfileCollection : InterfaceContractCollection<IImportProfile, ImportProfile>, IImportProfileCollection
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ImportProfileCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public ImportProfileCollection(String valueAsXml)
        {
            LoadImportProfileCollection(valueAsXml);
        }

        /// <summary>
        /// Initialize ImportProfileCollection from IList
        /// </summary>
        /// <param name="importProfilesList">IList of importProfiles</param>
        public ImportProfileCollection(IList<ImportProfile> importProfilesList)
        {
            this._items = new Collection<ImportProfile>(importProfilesList);
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
            if (obj is ImportProfileCollection)
            {
                ImportProfileCollection objectToBeCompared = obj as ImportProfileCollection;
                Int32 importProfilesUnion = this._items.ToList().Union(objectToBeCompared._items.ToList()).Count();
                Int32 importProfilesIntersect = this._items.ToList().Intersect(objectToBeCompared._items.ToList()).Count();
                if (importProfilesUnion != importProfilesIntersect)
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
            foreach (ImportProfile attr in this._items)
            {
                hashCode += attr.GetHashCode();
            }
            return hashCode;
        }

        #endregion

        #region Private Methods

        ///<summary>
        /// Load ImportProfileCollection object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        private void LoadImportProfileCollection(String valuesAsXml)
        {
            #region Sample Xml
            /*
             * <ImportProfiles></ImportProfiles>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ProfileData")
                        {
                            String importProfileXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(importProfileXml))
                            {
                                ImportProfile importProfile = new ImportProfile(importProfileXml);
                                this.Add(importProfile);
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

        #region IImportProfileCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of ImportProfileCollection object
        /// </summary>
        /// <returns>Xml string representing the ImportProfileCollection</returns>
        public String ToXml()
        {
            String importProfilesXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (ImportProfile importProfile in this._items)
            {
                builder.Append(importProfile.ToXml());
            }

            importProfilesXml = String.Format("<ImportProfiles>{0}</ImportProfiles>", builder.ToString());
            return importProfilesXml;
        }

        /// <summary>
        /// Get Xml representation of ImportProfileCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml( ObjectSerialization serialization )
        {
            String importProfilesXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (ImportProfile importProfile in this._items)
            {
                builder.Append(importProfile.ToXml(serialization));
            }

            importProfilesXml = String.Format("<ImportProfiles>{0}</ImportProfiles>", builder.ToString());
            return importProfilesXml;
        }

        /// <summary>
        /// Determines whether the ImportProfileCollection contains a specific profileId.
        /// </summary>
        /// <param name="profileId">The importProfile object to locate in the ImportProfileCollection.</param>
        /// <returns>
        /// <para>true : If importProfile found in ImportProfileCollection</para>
        /// <para>false : If importProfile found not in ImportProfileCollection</para>
        /// </returns>
        public bool Contains(Int32 profileId)
        {
            if (GetImportProfile(profileId) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove importProfile object from ImportProfileCollection
        /// </summary>
        /// <param name="importProfileId">importProfileId of importProfile which is to be removed from collection</param>
        /// <returns>true if ImportProfile is successfully removed; otherwise, false. This method also returns false if importProfile was not found in the original collection</returns>
        public bool Remove(Int32 importProfileId)
        {
            ImportProfile importProfile = GetImportProfile(importProfileId);

            if (importProfile == null)
                throw new ArgumentException("No ImportProfile found for given Id :" + importProfileId);
            else
                return this.Remove(importProfile);
        }

        #endregion ToXml methods

        #endregion IImportProfileCollection Memebers

        #region ImportProfile Get

        /// <summary>
        /// 
        /// </summary>
        /// <param name="importProfileId"></param>
        /// <returns></returns>
        public ImportProfile GetImportProfile(Int32 importProfileId)
        {
            if (this._items == null)
            {
                throw new NullReferenceException("There are no import profiles to search in");
            }

            if (importProfileId <= 0)
            {
                throw new ArgumentException("Import Profile Id must be greater than 0", importProfileId.ToString());
            }

            ImportProfile entity = (from profile in this._items
                                    where profile.Id == importProfileId
                                    select profile).ToList().FirstOrDefault();

            return entity;
        }

        /// <summary>
        /// For a given file watcher folder name, get the profile
        /// </summary>
        /// <param name="fileWatcherFolderName"></param>
        /// <returns></returns>
        public ImportProfile GetImportProfile(String fileWatcherFolderName)
        {
            if (this._items == null)
            {
                throw new NullReferenceException("There are no import profiles to search in");
            }

            if (String.IsNullOrEmpty(fileWatcherFolderName))
            {
                throw new ArgumentException("Import Profile Id must be greater than 0", fileWatcherFolderName);
            }

            ImportProfile entity = (from profile in this._items
                                    where profile.FileWatcherFolderName.ToLower() == fileWatcherFolderName.ToLower()
                                    select profile).ToList().FirstOrDefault();

            return entity;
        }

        #endregion ImportProfile Get
    }
}
