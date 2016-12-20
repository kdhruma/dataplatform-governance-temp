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
    using MDM.Interfaces.DataModel;

    /// <summary>
    /// Represents a collection of the DataModel Import Profile.
    /// </summary>
    [DataContract]
    public class DataModelImportProfileCollection : InterfaceContractCollection<IDataModelImportProfile, DataModelImportProfile>, IDataModelImportProfileCollection
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DataModelImportProfileCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input parameter
        /// </summary>
        public DataModelImportProfileCollection(String valueAsXml)
        {
            LoadDataModelImportProfileCollection(valueAsXml);
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

            if (obj is DataModelImportProfileCollection)
            {
                DataModelImportProfileCollection objectToBeCompared = obj as DataModelImportProfileCollection;
                Int32 dataModelImportProfilesUnion = this._items.ToList().Union(objectToBeCompared._items.ToList()).Count();
                Int32 dataModelImportProfilesIntersect = this._items.ToList().Intersect(objectToBeCompared._items.ToList()).Count();

                if (dataModelImportProfilesUnion != dataModelImportProfilesIntersect)
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
            foreach (DataModelImportProfile attr in this._items)
            {
                hashCode += attr.GetHashCode();
            }
            return hashCode;
        }

        #endregion

        #region Private Methods

        ///<summary>
        /// Load DataModelImportProfileCollection object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        ///     <DataModelImportProfiles>
        ///     <DataModelImportProfile Id="-1" Name="MDMCenter - RSXml 1.1 - Default DataModel Profile" ImportType="DataModelImport" Enabled="true" FileWatcherFolderName="RSXmlDataModel11">
        ///     <ExecutionSteps>
        ///         <ExecutionStep Name="Process" StepType="Core" AssemblyFileName="" ClassFullName="" AbortOnError="true" Enabled="true" />
        ///     </ExecutionSteps>
        ///     <ReaderSettings />
        ///     <InputSpecifications Reader="RSXmlDataModel11">
        ///     </InputSpecifications>
        ///     <DataModelJobProcessingOptions NumberofDataModelThreads="2" NumberofRecordThreadsPerDataModelThread="2" BatchSize="100" />
        ///     </DataModelImportProfile>	
        ///     </DataModelImportProfiles>
        /// </example>
        private void LoadDataModelImportProfileCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DataModelImportProfile")
                        {
                            String dataModelLmportProfileXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(dataModelLmportProfileXml))
                            {
                                DataModelImportProfile dataModelImportProfile = new DataModelImportProfile(dataModelLmportProfileXml);

                                this.Add(dataModelImportProfile);
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

        #region IDataModelImportProfileCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of DataModelImportProfileCollection object
        /// </summary>
        /// <returns>Xml string representing the DataModelImportProfileCollection</returns>
        public String ToXml()
        {
            String dataModelImportProfilesXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (DataModelImportProfile dataModelImportProfile in this._items)
            {
                builder.Append(dataModelImportProfile.ToXml());
            }

            dataModelImportProfilesXml = String.Format("<DataModelImportProfiles>{0}</DataModelImportProfiles>", builder.ToString());
            return dataModelImportProfilesXml;
        }

        /// <summary>
        /// Get Xml representation of ImportProfileCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String dataModelLmportProfilesXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (DataModelImportProfile dataModelImportProfile in this._items)
            {
                builder.Append(dataModelImportProfile.ToXml(serialization));
            }

            dataModelLmportProfilesXml = String.Format("<DataModelImportProfiles>{0}</DataModelImportProfiles>", builder.ToString());
            return dataModelLmportProfilesXml;
        }

        /// <summary>
        /// Determines whether the LookpImportProfileCollection contains a specific profileId.
        /// </summary>
        /// <param name="profileId">The importProfile object to locate in the DataModelImportProfileCollection.</param>
        /// <returns>
        /// <para>true : If DataModelImportProfile found in DataModelImportProfileCollection</para>
        /// <para>false : If DataModelimportProfile found not in DataModelImportProfileCollection</para>
        /// </returns>
        public bool Contains(Int32 profileId)
        {
            if (this.GetDataModelImportProfile(profileId) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove DataModelImportProfile object from DataModelImportProfileCollection
        /// </summary>
        /// <param name="importProfileId">ImportProfileId of DataModelImportProfile which is to be removed from collection</param>
        /// <returns>true if DataModelImportProfile is successfully removed; otherwise, false. This method also returns false if dataModelimportProfile was not found in the original collection</returns>
        public bool Remove(Int32 importProfileId)
        {
            IDataModelImportProfile dataModelImportProfile = this.GetDataModelImportProfile(importProfileId);

            if (dataModelImportProfile == null)
                throw new ArgumentException("No DataModelImportProfile found for given Id :" + importProfileId);
            else
                return this.Remove(dataModelImportProfile);
        }

        #endregion ToXml methods

        #endregion IDataModelImportProfileCollection Memebers
        
        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a DataModelImportProfileCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public new IEnumerator<IDataModelImportProfile> GetEnumerator()
        {
            return this._items.GetEnumerator();
        }
        #endregion 

        #region DataModelImportProfile Get

        /// <summary>
        /// Get the DataModel Import profile based on requested profile id.
        /// </summary>
        /// <param name="profileId">Indicates the profile Id</param>
        /// <returns>Get the DataModel Import profile Interface</returns>
        public IDataModelImportProfile GetDataModelImportProfile(Int32 profileId)
        {
            // Initialize to NULL as the caller expects a null object back.
            DataModelImportProfile dataModelImportProfile = null;

            if (this._items == null)
            {
                throw new NullReferenceException("There are no import profiles to search in");
            }

            if (profileId <= 0)
            {
                throw new ArgumentException("DataModel Import Profile Id must be greater than 0", profileId.ToString());
            }

            foreach (DataModelImportProfile profile in this._items)
            {
                if (profile.Id == profileId)
                {
                    dataModelImportProfile = profile;
                    break;
                }
            }

            return dataModelImportProfile;
        }

        /// <summary>
        /// Get the profile based on the file watcher folder name
        /// </summary>
        /// <param name="fileWatcherFolderName">Indicates the file watcher folder name</param>
        /// <returns>Get the DataModelImport profile object</returns>
        public DataModelImportProfile GetDataModelImportProfile(String fileWatcherFolderName)
        {
            if (this._items == null)
            {
                throw new NullReferenceException("There are no dataModel import profiles to search in");
            }

            if (String.IsNullOrEmpty(fileWatcherFolderName))
            {
                throw new ArgumentException("DataModel Import Profile Id must be greater than 0", fileWatcherFolderName);
            }
            DataModelImportProfile dataModelImportProfile = null;

            foreach (DataModelImportProfile profile in this._items)
            {
                if (profile.FileWatcherFolderName.ToLower() == fileWatcherFolderName.ToLower())
                {
                    dataModelImportProfile = profile;
                    break;
                }
            }

            return dataModelImportProfile;
        }

        #endregion DataModelImportProfile Get

    }
}
