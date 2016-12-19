using System;
using System.Text;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace MDM.BusinessObjects.Diagnostics
{
    using MDM.Core;

    using MDM.Interfaces;
    using MDM.BusinessObjects.Interfaces;

    /// <summary>
    /// Specifies the DiagnosticReportProfile Value Instance Collection for the Object
    /// </summary>
    [DataContract]
    public class DiagnosticReportProfileCollection : InterfaceContractCollection<IDiagnosticReportProfile,DiagnosticReportProfile>, IDiagnosticReportProfileCollection
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DiagnosticReportProfileCollection()
        { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public DiagnosticReportProfileCollection(String valueAsXml)
        {
            LoadDiagnosticReportProfileCollection(valueAsXml);
        }

        /// <summary>
        /// Initialize DiagnosticReportProfileCollection from IList
        /// </summary>
        /// <param name="diagnosticReportProfileList">IList of DiagnosticReportProfiles</param>
        public DiagnosticReportProfileCollection(IList<DiagnosticReportProfile> diagnosticReportProfileList)
        {
            _items = new Collection<DiagnosticReportProfile>(diagnosticReportProfileList);
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
            if (obj is DiagnosticReportProfileCollection)
            {
                DiagnosticReportProfileCollection objectToBeCompared = obj as DiagnosticReportProfileCollection;
                Int32 diagnosticReportProfilesUnion = this._items.ToList().Union(objectToBeCompared._items.ToList()).Count();
                Int32 diagnosticReportProfilesIntersect = this._items.ToList().Intersect(objectToBeCompared._items.ToList()).Count();
                if (diagnosticReportProfilesUnion != diagnosticReportProfilesIntersect)
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
            foreach (DiagnosticReportProfile attr in this._items)
            {
                hashCode += attr.GetHashCode();
            }
            return hashCode;
        }

        #endregion

        #region Private Methods

        ///<summary>
        /// Load DiagnosticReportProfileCollection object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        private void LoadDiagnosticReportProfileCollection(String valuesAsXml)
        {
            #region Sample Xml
            /*
             * <DiagnosticReportProfiles></DiagnosticReportProfiles>
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
                            String diagnosticReportProfileXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(diagnosticReportProfileXml))
                            {
                                DiagnosticReportProfile diagnosticReportProfile = new DiagnosticReportProfile(diagnosticReportProfileXml);
                                this.Add(diagnosticReportProfile);
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

        #region IDiagnosticReportProfileCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of DiagnosticReportProfileCollection object
        /// </summary>
        /// <returns>Xml string representing the DiagnosticReportProfileCollection</returns>
        public String ToXml()
        {
            String DiagnosticReportProfilesXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (DiagnosticReportProfile diagnosticReportProfile in this._items)
            {
                builder.Append(diagnosticReportProfile.ToXml());
            }

            DiagnosticReportProfilesXml = String.Format("<DiagnosticReportProfiles>{0}</DiagnosticReportProfiles>", builder.ToString());
            return DiagnosticReportProfilesXml;
        }

        /// <summary>
        /// Get Xml representation of DiagnosticReportProfileCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String DiagnosticReportProfilesXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (DiagnosticReportProfile diagnosticReportProfile in this._items)
            {
                builder.Append(diagnosticReportProfile.ToXml(serialization));
            }

            DiagnosticReportProfilesXml = String.Format("<DiagnosticReportProfiles>{0}</DiagnosticReportProfiles>", builder.ToString());
            return DiagnosticReportProfilesXml;
        }

        /// <summary>
        /// Determines whether the DiagnosticReportProfileCollection contains a specific profileId.
        /// </summary>
        /// <param name="profileId">The DiagnosticReportProfile object to locate in the DiagnosticReportProfileCollection.</param>
        /// <returns>
        /// <para>true : If DiagnosticReportProfile found in DiagnosticReportProfileCollection</para>
        /// <para>false : If DiagnosticReportProfile found not in DiagnosticReportProfileCollection</para>
        /// </returns>
        public bool Contains(Int32 profileId)
        {
            if (GetDiagnosticReportProfile(profileId) != null)
                return true;
            else
                return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosticReportProfileId"></param>
        /// <returns></returns>
        public bool Remove(Int32 diagnosticReportProfileId)
        {
            DiagnosticReportProfile diagnosticReportProlfile = GetDiagnosticReportProfile(diagnosticReportProfileId);

            if (diagnosticReportProlfile == null)
                throw new ArgumentException("No diagnosticReportProlfile found for given Id :" + diagnosticReportProfileId);
            else
                return this.Remove(diagnosticReportProlfile);
        }

        #endregion ToXml methods

        #endregion IDiagnosticReportProfileCollection Memebers

        #region DiagnosticReportProfile Get

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosticReportProfileId"></param>
        /// <returns></returns>
        public DiagnosticReportProfile GetDiagnosticReportProfile(Int32 diagnosticReportProfileId)
        {
            if (this._items == null)
            {
                throw new NullReferenceException("There are no DiagnosticReport profiles to search in");
            }

            if (diagnosticReportProfileId <= 0)
            {
                throw new ArgumentException("DiagnosticReport Profile Id must be greater than 0", diagnosticReportProfileId.ToString());
            }

            return (from profile in this._items
                                    where profile.Id == diagnosticReportProfileId
                                    select profile).ToList().FirstOrDefault();
        }

        /// <summary>
        /// For a given file watcher folder name, get the profile
        /// </summary>
        /// <param name="fileWatcherFolderName"></param>
        /// <returns></returns>
        public DiagnosticReportProfile GetDiagnosticReportProfile(String fileWatcherFolderName)
        {
            if (this._items == null)
            {
                throw new NullReferenceException("There are no DiagnosticReport profiles to search in");
            }

            if (String.IsNullOrEmpty(fileWatcherFolderName))
            {
                throw new ArgumentException("DiagnosticReport Profile Id must be greater than 0", fileWatcherFolderName);
            }

            DiagnosticReportProfile reportProfile = (from profile in this._items
                                    where profile.FileWatcherFolderName.ToLower() == fileWatcherFolderName.ToLower()
                                    select profile).ToList().FirstOrDefault();

            return reportProfile;
        }

        #endregion DiagnosticReportProfile Get
    }
}
