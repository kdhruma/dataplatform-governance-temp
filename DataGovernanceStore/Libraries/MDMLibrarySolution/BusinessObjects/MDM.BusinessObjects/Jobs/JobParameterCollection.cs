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
    /// Specifies the JobParameter Instance Collection for the Object
    /// </summary>
    [DataContract]
    public class JobParameterCollection : ICollection<JobParameter>, IEnumerable<JobParameter>, IJobParameterCollection
    {
        #region Fields

        [DataMember]
        private Collection<JobParameter> _jobParameters = new Collection<JobParameter>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public JobParameterCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public JobParameterCollection(String valueAsXml)
        {
            LoadJobParameterCollection(valueAsXml);
        }

        /// <summary>
        /// Initialize JobParameterCollection from IList
        /// </summary>
        /// <param name="jobParametersList">IList of execution steps</param>
        public JobParameterCollection(IList<JobParameter> jobParametersList)
        {
            this._jobParameters = new Collection<JobParameter>(jobParametersList);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Find job parameter from JobParameterCollection based on parameter name
        /// </summary>
        /// <param name="jobParameterName">Parameter name to be searched</param>
        /// <returns>JobParamter object having given parameter name </returns>
        public JobParameter this[String jobParameterName]
        {
            get
            {
                JobParameter jobParameter = Get(jobParameterName);

                //if (jobParameter == null)
                //    throw new ArgumentException(String.Format("No job parameter found with name: {0}", jobParameterName), "jobParameterName");
                //else
                    return jobParameter;
            }
            set
            {
                JobParameter jobParameter = Get(jobParameterName);

                //if (jobParameter == null)
                //    throw new ArgumentException(String.Format("No job parameter found with name: {0}", jobParameterName), "jobParameterName");

                jobParameter = value;
            }
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is JobParameterCollection)
            {
                JobParameterCollection objectToBeCompared = obj as JobParameterCollection;
                Int32 jobParametersUnion = this._jobParameters.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 jobParametersIntersect = this._jobParameters.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (jobParametersUnion != jobParametersIntersect)
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
            foreach (JobParameter attr in this._jobParameters)
            {
                hashCode += attr.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public void LoadJobParameterCollection( String valuesAsXml )
        {
            #region Sample Xml
            /*
             * <JobParameters></JobParameters>
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
                        if ( reader.NodeType == XmlNodeType.Element && reader.Name == "JobParameter" )
                        {
                            String parametersXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(parametersXml))
                            {
                                JobParameter jobParameter = new JobParameter(parametersXml);
                                this.Add(jobParameter);
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

        private JobParameter Get(String jobParameterName)
        {
            IEnumerable<JobParameter> filteredJobParameters = null;

            filteredJobParameters = this._jobParameters.Where(p => p.Name == jobParameterName);
            if (filteredJobParameters != null && filteredJobParameters.Any())
                return filteredJobParameters.First();
            else
                return null;
        }

        #endregion

        #region ICollection<JobParameter> Members

        /// <summary>
        /// Add jobParameter object in collection
        /// </summary>
        /// <param name="item">jobParameter to add in collection</param>
        public void Add(JobParameter item)
        {
            this._jobParameters.Add(item);
        }

        /// <summary>
        /// Removes all execution steps from collection
        /// </summary>
        public void Clear()
        {
            this._jobParameters.Clear();
        }

        /// <summary>
        /// Determines whether the JobParameterCollection contains a specific jobParameter.
        /// </summary>
        /// <param name="item">Indicates Name of the job parameter to be located in JobParameterCollection</param>
        /// <returns>
        /// <para>true : If jobParameter is found in mappingCollection</para>
        /// <para>false : If jobParameter is not found in mappingCollection</para>
        /// </returns>
        public bool Contains(JobParameter item)
        {
            return this._jobParameters.Contains(item);
        }

        /// <summary>
        /// Determines whether the JobParameterCollection contains jobParameter with a specific name.
        /// </summary>
        /// <param name="name">Name of the job parameter to be located in JobParameterCollection</param>
        /// <returns>
        /// <para>true : If jobParameter is found in mappingCollection</para>
        /// <para>false : If jobParameter is not found in mappingCollection</para>
        /// </returns>
        public bool Contains(String name)
        {
            foreach (JobParameter jobParameter in this._jobParameters)
            {
                if(jobParameter.Name.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Copies the elements of the JobParameterCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from JobParameterCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(JobParameter[] array, int arrayIndex)
        {
            this._jobParameters.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of execution steps in JobParameterCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._jobParameters.Count;
            }
        }

        /// <summary>
        /// Check if JobParameterCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the JobParameterCollection.
        /// </summary>
        /// <param name="item">The jobParameter object to remove from the JobParameterCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original JobParameterCollection</returns>
        public bool Remove(JobParameter item)
        {
            return this._jobParameters.Remove(item);
        }

        #endregion

        #region IEnumerable<JobParameter> Members

        /// <summary>
        /// Returns an enumerator that iterates through a JobParameterCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<JobParameter> GetEnumerator()
        {
            return this._jobParameters.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a JobParameterCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._jobParameters.GetEnumerator();
        }

        #endregion

        #region IJobParameterCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of JobParameterCollection object
        /// </summary>
        /// <returns>Xml string representing the JobParameterCollection</returns>
        public String ToXml()
        {
            String jobParametersXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach ( JobParameter jobParameter in this._jobParameters )
            {
                builder.Append(jobParameter.ToXml());
            }

            jobParametersXml = String.Format("<JobParameters>{0}</JobParameters>", builder.ToString());
            return jobParametersXml;
        }

        /// <summary>
        /// Get Xml representation of JobParameterCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml( ObjectSerialization serialization )
        {
            String jobParametersXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach ( JobParameter jobParameter in this._jobParameters )
            {
                builder.Append(jobParameter.ToXml(serialization));
            }

            jobParametersXml = String.Format("<JobParameters>{0}</JobParameters>", builder.ToString());
            return jobParametersXml;
        }

        #endregion ToXml methods

        #region JobParameter Get

        #endregion JobParameter Get
       

        #endregion IJobParameterCollection Memebers
    }
}
