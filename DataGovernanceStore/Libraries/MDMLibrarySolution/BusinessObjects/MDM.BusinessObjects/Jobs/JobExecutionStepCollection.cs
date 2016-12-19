using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects.Jobs
{
    using MDM.Core;

    /// <summary>
    /// Specifies the Execution Step Instance Collection for the Object
    /// </summary>
    [DataContract]
    public class JobExecutionStepCollection : ICollection<JobExecutionStep>, IEnumerable<JobExecutionStep>
    {
        #region Fields

        [DataMember]
        private Collection<JobExecutionStep> _executionSteps = new Collection<JobExecutionStep>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public JobExecutionStepCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public JobExecutionStepCollection(String valueAsXml)
        {
            LoadExecutionStepCollection(valueAsXml);
        }

        /// <summary>
        /// Initialize JobExecutionStepCollection from IList
        /// </summary>
        /// <param name="executionStepList">IList of execution steps</param>
        public JobExecutionStepCollection(IList<JobExecutionStep> executionStepList)
        {
            this._executionSteps = new Collection<JobExecutionStep>(executionStepList);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Find execution step from ExecutionStepCollection based on id
        /// </summary>
        /// <param name="id">id to search in execution step collection</param>
        /// <returns>ExecutionStep object having given id</returns>
        public JobExecutionStep this[Int32 id]
        {
            get
            {
                JobExecutionStep executionStep = GetJobExecutionStep(id);
                if (executionStep == null)
                    throw new ArgumentException(String.Format("No execution step found for id: {0}", id), "id");

                return executionStep;
            }
            set
            {
                JobExecutionStep executionStep = GetJobExecutionStep(id);
                if (executionStep == null)
                    throw new ArgumentException(String.Format("No execution step found for id: {0}", id), "id");

                executionStep = value;
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
            if (obj is JobExecutionStepCollection)
            {
                JobExecutionStepCollection objectToBeCompared = obj as JobExecutionStepCollection;
                Int32 executionStepsUnion = this._executionSteps.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 executionStepsIntersect = this._executionSteps.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (executionStepsUnion != executionStepsIntersect)
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
            foreach (JobExecutionStep attr in this._executionSteps)
            {
                hashCode += attr.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public void LoadExecutionStepCollection(String valuesAsXml)
        {
            #region Sample Xml
            /*
             * <JobExecutionSteps></JobExecutionSteps>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "JobExecutionStep")
                        {
                            String executionStepXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(executionStepXml))
                            {
                                JobExecutionStep executionStep = new JobExecutionStep(executionStepXml);
                                this.Add(executionStep);
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

        #region Private Methods

        private JobExecutionStep GetJobExecutionStep(Int32 id)
        {
            var filteredExecutionStepResult = from executionStepResult in this._executionSteps
                                              where executionStepResult.Id == id
                                              select executionStepResult;

            if (filteredExecutionStepResult.Any())
                return filteredExecutionStepResult.First();
            else
                return null;
        }

        #endregion

        #region ICollection<JobExecutionStep> Members

        /// <summary>
        /// Add execution step object in collection
        /// </summary>
        /// <param name="item">execution step to add in collection</param>
        public void Add(JobExecutionStep item)
        {
            this._executionSteps.Add(item);
        }

        /// <summary>
        /// Removes all execution steps from collection
        /// </summary>
        public void Clear()
        {
            this._executionSteps.Clear();
        }

        /// <summary>
        /// Determines whether the JobExecutionStepCollection contains a specific execution step.
        /// </summary>
        /// <param name="item">The execution step object to locate in the JobExecutionStepCollection.</param>
        /// <returns>
        /// <para>true : If execution step found in mappingCollection</para>
        /// <para>false : If execution step found not in mappingCollection</para>
        /// </returns>
        public bool Contains(JobExecutionStep item)
        {
            return this._executionSteps.Contains(item);
        }

        /// <summary>
        /// Determines whether the ExecutionStepCollection contains a specific executionStep based on id.
        /// </summary>
        /// <param name="id">The id locate in the ExecutionStepCollection.</param>
        /// <returns>
        /// <para>true : If id found in mappingCollection</para>
        /// <para>false : If id found not in mappingCollection</para>
        /// </returns>
        public bool Contains(Int32 id)
        {
            if (GetJobExecutionStep(id) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Copies the elements of the JobExecutionStepCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from JobExecutionStepCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(JobExecutionStep[] array, int arrayIndex)
        {
            this._executionSteps.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of execution steps in JobExecutionStepCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._executionSteps.Count;
            }
        }

        /// <summary>
        /// Check if JobExecutionStepCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the JobExecutionStepCollection.
        /// </summary>
        /// <param name="item">The execution step object to remove from the JobExecutionStepCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original JobExecutionStepCollection</returns>
        public bool Remove(JobExecutionStep item)
        {
            return this._executionSteps.Remove(item);
        }

        #endregion

        #region IEnumerable<JobExecutionStep> Members

        /// <summary>
        /// Returns an enumerator that iterates through a JobExecutionStepCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<JobExecutionStep> GetEnumerator()
        {
            return this._executionSteps.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a JobExecutionStepCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._executionSteps.GetEnumerator();
        }

        #endregion

        #region IJobExecutionStepCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of JobExecutionStepCollection object
        /// </summary>
        /// <returns>Xml string representing the JobExecutionStepCollection</returns>
        public String ToXml()
        {
            String executionStepsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (JobExecutionStep executionStep in this._executionSteps)
            {
                builder.Append(executionStep.ToXml());
            }

            executionStepsXml = String.Format("<JobExecutionSteps>{0}</JobExecutionSteps>", builder.ToString());
            return executionStepsXml;
        }

        /// <summary>
        /// Get Xml representation of JobExecutionStepCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String executionStepsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (JobExecutionStep jobExecutionStep in this._executionSteps)
            {
                builder.Append(jobExecutionStep.ToXml(serialization));
            }

            executionStepsXml = String.Format("<JobExecutionSteps>{0}</JobExecutionSteps>", builder.ToString());
            return executionStepsXml;
        }

        #endregion ToXml methods

        #endregion IJobExecutionStepCollection Memebers
    }
}
