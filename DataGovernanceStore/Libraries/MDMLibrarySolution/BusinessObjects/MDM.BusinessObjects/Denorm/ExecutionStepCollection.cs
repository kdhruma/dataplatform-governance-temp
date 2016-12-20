using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects.Denorm
{
    /// <summary>
    /// Specifies the Execution Step Instance Collection for the Object
    /// </summary>
    [DataContract]
    public class ExecutionStepCollection : ICollection<ExecutionStep>, IEnumerable<ExecutionStep>
    {
        #region Fields

        [DataMember]
        private Collection<ExecutionStep> _executionSteps = new Collection<ExecutionStep>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ExecutionStepCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public ExecutionStepCollection(String valueAsXml)
        {
            LoadExecutionStepCollection(valueAsXml);
        }

        /// <summary>
        /// Initialize ExecutionStepCollection from IList
        /// </summary>
        /// <param name="executionStepList">IList of execution steps</param>
        public ExecutionStepCollection(IList<ExecutionStep> executionStepList)
        {
            this._executionSteps = new Collection<ExecutionStep>(executionStepList);
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
            if (obj is ExecutionStepCollection)
            {
                ExecutionStepCollection objectToBeCompared = obj as ExecutionStepCollection;
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
            foreach (ExecutionStep attr in this._executionSteps)
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
             * <ExecutionSteps></ExecutionSteps>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExecutionStep")
                        {
                            String executionStepXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(executionStepXml))
                            {
                                ExecutionStep executionStep = new ExecutionStep(executionStepXml);
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

        #endregion

        #region ICollection<ExecutionStep> Members

        /// <summary>
        /// Add execution step object in collection
        /// </summary>
        /// <param name="item">execution step to add in collection</param>
        public void Add(ExecutionStep item)
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
        /// Determines whether the ExecutionStepCollection contains a specific execution step.
        /// </summary>
        /// <param name="item">The execution step object to locate in the ExecutionStepCollection.</param>
        /// <returns>
        /// <para>true : If execution step found in mappingCollection</para>
        /// <para>false : If execution step found not in mappingCollection</para>
        /// </returns>
        public bool Contains(ExecutionStep item)
        {
            return this._executionSteps.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the ExecutionStepCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from ExecutionStepCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(ExecutionStep[] array, int arrayIndex)
        {
            this._executionSteps.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of execution steps in ExecutionStepCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._executionSteps.Count;
            }
        }

        /// <summary>
        /// Check if ExecutionStepCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the ExecutionStepCollection.
        /// </summary>
        /// <param name="item">The execution step object to remove from the ExecutionStepCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original ExecutionStepCollection</returns>
        public bool Remove(ExecutionStep item)
        {
            return this._executionSteps.Remove(item);
        }

        #endregion

        #region IEnumerable<ExecutionStep> Members

        /// <summary>
        /// Returns an enumerator that iterates through a ExecutionStepCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<ExecutionStep> GetEnumerator()
        {
            return this._executionSteps.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a ExecutionStepCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._executionSteps.GetEnumerator();
        }

        #endregion

        #region IExecutionStepCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of ExecutionStepCollection object
        /// </summary>
        /// <returns>Xml string representing the ExecutionStepCollection</returns>
        public String ToXml()
        {
            String executionStepsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (ExecutionStep executionStep in this._executionSteps)
            {
                builder.Append(executionStep.ToXml());
            }

            executionStepsXml = String.Format("<ExecutionSteps>{0}</ExecutionSteps>", builder.ToString());
            return executionStepsXml;
        }

        #endregion ToXml methods

        #endregion IExecutionStepCollection Memebers
    }
}
