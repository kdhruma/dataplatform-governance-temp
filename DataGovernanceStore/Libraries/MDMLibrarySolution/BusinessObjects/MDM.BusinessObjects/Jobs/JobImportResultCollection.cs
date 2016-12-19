using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace MDM.BusinessObjects.Jobs
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the JobImportResult Instance Collection for the Object
    /// </summary>
    [DataContract]
    public class JobImportResultCollection : ICollection<JobImportResult>, IEnumerable<JobImportResult>, IJobImportResultCollection
    {
        #region Fields

        [DataMember]
        private Collection<JobImportResult> _jobImportResults = new Collection<JobImportResult>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public JobImportResultCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public JobImportResultCollection(String valueAsXml)
        {
            LoadJobImportResultCollection(valueAsXml);
        }

        /// <summary>
        /// Initialize JobImportResultCollection from IList
        /// </summary>
        /// <param name="jobImportResultList">Indicates IList of execution steps</param>
        public JobImportResultCollection(IList<JobImportResult> jobImportResultList)
        {
            this._jobImportResults = new Collection<JobImportResult>(jobImportResultList);
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
            if (obj is JobImportResultCollection)
            {
                JobImportResultCollection objectToBeCompared = obj as JobImportResultCollection;
                Int32 jobsUnion = this._jobImportResults.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 jobsIntersect = this._jobImportResults.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (jobsUnion != jobsIntersect)
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
            foreach (JobImportResult attr in this._jobImportResults)
            {
                hashCode += attr.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public void LoadJobImportResultCollection(String valuesAsXml)
        {
            #region Sample Xml
            /*
             * <JobImportResults></JobImportResult>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "JobImportResult")
                        {
                            String jobXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(jobXml))
                            {
                                JobImportResult jobImportResult = new JobImportResult(jobXml);
                                this.Add(jobImportResult);
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

        #region ICollection<JobImportResult> Members

        /// <summary>
        /// Add JobImportResult object in collection
        /// </summary>
        /// <param name="item">jobImportResult to add in collection</param>
        public void Add(JobImportResult item)
        {
            this._jobImportResults.Add(item);
        }

        /// <summary>
        /// Removes all execution steps from collection
        /// </summary>
        public void Clear()
        {
            this._jobImportResults.Clear();
        }

        /// <summary>
        /// Determines whether the JobImportResultCollection contains a specific jobImportResult.
        /// </summary>
        /// <param name="item">The jobImportResult object to locate in the JobImportResultCollection.</param>
        /// <returns>
        /// <para>true : If jobImportResult found in mappingCollection</para>
        /// <para>false : If jobImportResult found not in mappingCollection</para>
        /// </returns>
        public bool Contains(JobImportResult item)
        {
            return this._jobImportResults.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the JobImportResultCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from JobImportResultCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(JobImportResult[] array, int arrayIndex)
        {
            this._jobImportResults.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of execution steps in JobImportResultCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._jobImportResults.Count;
            }
        }

        /// <summary>
        /// Check if JobImportResultCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the JobImportResultCollection.
        /// </summary>
        /// <param name="item">The jobImportResult object to remove from the JobImportResultCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original JobImportResultCollection</returns>
        public bool Remove(JobImportResult item)
        {
            return this._jobImportResults.Remove(item);
        }

        #endregion

        #region IEnumerable<JobImportResult> Members

        /// <summary>
        /// Returns an enumerator that iterates through a JobImportResultCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<JobImportResult> GetEnumerator()
        {
            return this._jobImportResults.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a JobImportResultCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._jobImportResults.GetEnumerator();
        }

        #endregion

        #region IJobImportResultCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of JobImportResultCollection object
        /// </summary>
        /// <returns>Xml string representing the JobImportResultCollection</returns>
        public String ToXml()
        {
            String jobsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (JobImportResult jobImportResult in this._jobImportResults)
            {
                builder.Append(jobImportResult.ToXml());
            }

            jobsXml = String.Format("<JobImportResults>{0}</JobImportResults>", builder.ToString());
            return jobsXml;
        }

        /// <summary>
        /// Get Xml representation of JobImportResultCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml( ObjectSerialization serialization )
        {
            String jobsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (JobImportResult jobImportResult in this._jobImportResults)
            {
                builder.Append(jobImportResult.ToXml(serialization));
            }

            jobsXml = String.Format("<JobImportResults>{0}</JobImportResults>", builder.ToString());
            return jobsXml;
        }

        #endregion ToXml methods

        #region JobImportResult Get

        #endregion JobImportResult Get


        #endregion IJobImportResultCollection Memebers
    }
}
