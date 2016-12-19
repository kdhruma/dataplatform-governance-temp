using System;
using System.Text;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace MDM.BusinessObjects.Imports
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the JobProfile Value Instance Collection for the Object
    /// </summary>
    [DataContract]
    public class JobProfileCollection : ICollection<JobProfile>, IEnumerable<JobProfile>
    {
        #region Fields

        //This is the most funny part of the WCF which is able to serialize even a private member.. needs to do further RnD to find out the internals
        [DataMember]
        private Collection<JobProfile> _jobprofiles = new Collection<JobProfile>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public JobProfileCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public JobProfileCollection(String valueAsXml)
        {
            LoadJobProfileCollection(valueAsXml);
        }

        /// <summary>
        /// Initialize JobProfileCollection from IList
        /// </summary>
        /// <param name="jobprofilesList">IList of jobprofiles</param>
        public JobProfileCollection(IList<JobProfile> jobprofilesList)
        {
            this._jobprofiles = new Collection<JobProfile>(jobprofilesList);
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
            if (obj is JobProfileCollection)
            {
                JobProfileCollection objectToBeCompared = obj as JobProfileCollection;
                Int32 jobprofilesUnion = this._jobprofiles.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 jobprofilesIntersect = this._jobprofiles.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (jobprofilesUnion != jobprofilesIntersect)
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
            foreach (JobProfile attr in this._jobprofiles)
            {
                hashCode += attr.GetHashCode();
            }
            return hashCode;
        }

        ///<summary>
        /// Load JobProfileCollection object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        public void LoadJobProfileCollection(String valuesAsXml)
        {
            #region Sample Xml
            /*
             * <JobProfiles></JobProfiles>
             */
            #endregion Sample Xml

            if ( !String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while ( !reader.EOF )
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "JobProfile")
                        {
                            String jobprofileXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(jobprofileXml))
                            {
                                JobProfile jobprofile = new JobProfile(jobprofileXml);
                                this.Add(jobprofile);
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
                    if ( reader != null )
                    {
                        reader.Close();
                    }
                }
            }
        }

        #endregion

        #region Private Methods


        #endregion

        #region ICollection<JobProfile> Members

        /// <summary>
        /// Add jobprofile object in collection
        /// </summary>
        /// <param name="item">jobprofile to add in collection</param>
        public void Add(JobProfile item)
        {
            this._jobprofiles.Add(item);
        }

        /// <summary>
        /// Removes all jobprofiles from collection
        /// </summary>
        public void Clear()
        {
            this._jobprofiles.Clear();
        }

        /// <summary>
        /// Determines whether the JobProfileCollection contains a specific jobprofile.
        /// </summary>
        /// <param name="item">The jobprofile object to locate in the JobProfileCollection.</param>
        /// <returns>
        /// <para>true : If jobprofile found in JobProfileCollection</para>
        /// <para>false : If jobprofile found not in JobProfileCollection</para>
        /// </returns>
        public bool Contains(JobProfile item)
        {
            return this._jobprofiles.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the JobProfileCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from JobProfileCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(JobProfile[] array, int arrayIndex)
        {
            this._jobprofiles.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of jobprofiles in JobProfileCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._jobprofiles.Count;
            }
        }

        /// <summary>
        /// Check if JobProfileCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the JobProfileCollection.
        /// </summary>
        /// <param name="item">The jobprofile object to remove from the JobProfileCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original JobProfileCollection</returns>
        public bool Remove(JobProfile item)
        {
            return this._jobprofiles.Remove(item);
        }

        #endregion

        #region IEnumerable<JobProfile> Members

        /// <summary>
        /// Returns an enumerator that iterates through a JobProfileCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<JobProfile> GetEnumerator()
        {
            return this._jobprofiles.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a JobProfileCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._jobprofiles.GetEnumerator();
        }

        #endregion

        #region IJobProfileCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of JobProfileCollection object
        /// </summary>
        /// <returns>Xml string representing the JobProfileCollection</returns>
        public String ToXml()
        {
            String jobprofilesXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (JobProfile jobprofile in this._jobprofiles)
            {
                builder.Append(jobprofile.ToXml());
            }

            jobprofilesXml = String.Format("<JobProfiles>{0}</JobProfiles>", builder.ToString());
            return jobprofilesXml;
        }

        /// <summary>
        /// Get Xml representation of JobProfileCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml( ObjectSerialization serialization )
        {
            String jobprofilesXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (JobProfile jobprofile in this._jobprofiles)
            {
                builder.Append(jobprofile.ToXml(serialization));
            }

            jobprofilesXml = String.Format("<JobProfiles>{0}</JobProfiles>", builder.ToString());
            return jobprofilesXml;
        }

        #endregion ToXml methods

        #region JobProfile Get

        #endregion JobProfile Get


        #endregion IJobProfileCollection Memebers
    }
}
