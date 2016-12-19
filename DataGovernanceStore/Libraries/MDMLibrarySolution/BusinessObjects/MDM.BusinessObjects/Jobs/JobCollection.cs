using System;
using System.Text;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace MDM.BusinessObjects.Jobs
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Job Instance Collection for the Object
    /// </summary>
    [DataContract]
    public class JobCollection : InterfaceContractCollection<IJob, Job>, IJobCollection
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public JobCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public JobCollection(String valueAsXml)
        {
            LoadJobCollection(valueAsXml);
        }

        /// <summary>
        /// Initialize JobCollection from IList
        /// </summary>
        /// <param name="jobsList">IList of execution steps</param>
        public JobCollection(IList<Job> jobsList)
        {
            this._items = new Collection<Job>(jobsList);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Find job from JobCollection based on id
        /// </summary>
        /// <param name="jobId">jobId to search in JobCollection</param>
        /// <returns>Job object having given jobId</returns>
        public Job this[Int32 jobId]
        {
            get
            {
                Job job = Get(jobId) as Job;
                if (job == null)
                    throw new ArgumentException(String.Format("No job found for id: {0}", jobId), "jobId");

                return job;
            }
            set
            {
                Job job = Get(jobId) as Job;
                if (job == null)
                    throw new ArgumentException(String.Format("No job found for id: {0}", jobId), "jobId");
                job = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether the JobCollection contains a specific job based on jobId.
        /// </summary>
        /// <param name="id">The jobId locate in the JobCollection.</param>
        /// <returns>
        /// <para>true : If jobId found in mappingCollection</para>
        /// <para>false : If jobId found not in mappingCollection</para>
        /// </returns>
        public Boolean Contains(Int32 id)
        {
            return this.Get(id) != null;
        }

        /// <summary>
        /// Clone job collection.
        /// </summary>
        /// <returns>cloned job collection object.</returns>
        public IJobCollection Clone()
        {
            JobCollection clonedJobs = new JobCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (Job job in this._items)
                {
                    IJob clonedIJob = job.Clone();
                    clonedJobs.Add(clonedIJob);
                }
            }
            return clonedJobs;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is JobCollection)
            {
                JobCollection objectToBeCompared = obj as JobCollection;
                Int32 jobsUnion = this._items.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 jobsIntersect = this._items.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (jobsUnion != jobsIntersect)
                    return false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets job item by id
        /// </summary>
        /// <param name="jobId">Id of the job</param>
        /// <returns>job with specified Id</returns>
        public IJob Get(Int32 jobId)
        {
            return this._items.FirstOrDefault(item => item.Id == jobId);
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            return this._items.Sum(attr => attr.GetHashCode());
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the JobCollection.
        /// </summary>
        /// <param name="jobId">The job object to remove from the JobCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original JobCollection</returns>
        public Boolean Remove(Int32 jobId)
        {
            IJob job = Get(jobId);
            if (job == null)
                throw new ArgumentException("No job found for given Id :" + jobId);

            return this.Remove(job);
        }

        /// <summary>
        /// Get Xml representation of JobCollection object
        /// </summary>
        /// <returns>Xml string representing the JobCollection</returns>
        public String ToXml()
        {
            String jobsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (Job job in this._items)
            {
                builder.Append(job.ToXml());
            }

            jobsXml = String.Format("<Jobs>{0}</Jobs>", builder);
            return jobsXml;
        }

        /// <summary>
        /// Get Xml representation of JobCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String jobsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (Job job in this._items)
            {
                builder.Append(job.ToXml(serialization));
            }

            jobsXml = String.Format("<Jobs>{0}</Jobs>", builder);
            return jobsXml;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadJobCollection(String valuesAsXml)
        {
            #region Sample Xml

            /*
             * <Jobs></Jobs>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Job")
                        {
                            String jobXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(jobXml))
                            {
                                Job job = new Job(jobXml);
                                this.Add(job);
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
    }
}
