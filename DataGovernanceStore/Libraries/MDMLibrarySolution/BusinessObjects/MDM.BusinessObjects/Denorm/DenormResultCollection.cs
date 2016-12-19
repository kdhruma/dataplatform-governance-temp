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
    /// Specifies the Denorm Result Collection for the Object
    /// </summary>
    /// 
    [DataContract]  
    public class DenormResultCollection : ICollection<DenormResult>, IEnumerable<DenormResult>
    {
        #region Fields

        [DataMember]
        private Collection<DenormResult> _denormResults = new Collection<DenormResult>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DenormResultCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public DenormResultCollection(String valueAsXml)
        {
            LoadDenormResultCollection(valueAsXml);
        }

        /// <summary>
        /// Initialize DenormResultCollection from IList
        /// </summary>
        /// <param name="denormResultList">IList of denorm result</param>
        public DenormResultCollection(IList<DenormResult> denormResultList)
        {
            this._denormResults = new Collection<DenormResult>(denormResultList);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Find denorm result from DenormResultCollection based on jobId
        /// </summary>
        /// <param name="jobId">JobId to search in denorm result collection</param>
        /// <returns>DenormResult object having given jobId</returns>
        public DenormResult this[Int32 jobId]
        {
            get
            {
                DenormResult denormResult = GetDenormResult(jobId);
                if (denormResult == null)
                    throw new ArgumentException(String.Format("No denorm result found for job id: {0}",jobId), "jobId");
                else
                    return denormResult;
            }
            set
            {
                DenormResult denormResult = GetDenormResult(jobId);
                if (denormResult == null)
                    throw new ArgumentException(String.Format("No denorm result found for job id: {0}", jobId), "jobId");

                denormResult = value;
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
            if (obj is DenormResultCollection)
            {
                DenormResultCollection objectToBeCompared = obj as DenormResultCollection;
                Int32 denormResultUnion = this._denormResults.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 denormResultIntersect = this._denormResults.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (denormResultUnion!= denormResultIntersect)
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
            foreach (DenormResult attr in this._denormResults)
            {
                hashCode += attr.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public void LoadDenormResultCollection(String valuesAsXml)
        {
            #region Sample Xml
            /*
             * <DenormResults></DenormResults>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DenormResult")
                        {
                            String denormResultXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(denormResultXml))
                            {
                                DenormResult denormResult = new DenormResult(denormResultXml);
                                this.Add(denormResult);
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

        private DenormResult GetDenormResult(Int32 jobId)
        {
            var filteredDenormResult= from denormResult in this._denormResults
                                       where denormResult.JobId ==jobId
                                       select denormResult;

            if (filteredDenormResult.Any())
                return filteredDenormResult.First();
            else
                return null;
        }

        #endregion

        #region ICollection<DenormResult> Members

        /// <summary>
        /// Add denorm result object in collection
        /// </summary>
        /// <param name="item">denorm result to add in collection</param>
        public void Add(DenormResult item)
        {
            this._denormResults.Add(item);
        }

        /// <summary>
        /// Removes all denorm result from collection
        /// </summary>
        public void Clear()
        {
            this._denormResults.Clear();
        }

        /// <summary>
        /// Determines whether the DenormResultCollection contains a specific denorm result.
        /// </summary>
        /// <param name="item">The denorm result object to locate in the DenormResultCollection.</param>
        /// <returns>
        /// <para>true : If denorm result found in mappingCollection</para>
        /// <para>false : If denorm result found not in mappingCollection</para>
        /// </returns>
        public bool Contains(DenormResult item)
        {
            return this._denormResults.Contains(item);
        }

        /// <summary>
        /// Determines whether the DenormResultCollection contains a specific denormResult based on jobId.
        /// </summary>
        /// <param name="jobId">The jobId locate in the DenormResultCollection.</param>
        /// <returns>
        /// <para>true : If jobId found in mappingCollection</para>
        /// <para>false : If jobId found not in mappingCollection</para>
        /// </returns>
        public bool Contains(Int32 jobId)
        {
            if (GetDenormResult(jobId) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Copies the elements of the DenormResultCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from DenormResultCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(DenormResult[] array, int arrayIndex)
        {
            this._denormResults.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of denorm result in DenormResultCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._denormResults.Count;
            }
        }

        /// <summary>
        /// Check if DenormResultCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the DenormResultCollection.
        /// </summary>
        /// <param name="item">The denorm result object to remove from the DenormResultCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original DenormResultCollection</returns>
        public bool Remove(DenormResult item)
        {
            return this._denormResults.Remove(item);
        }

        #endregion

        #region IEnumerable<DenormResult> Members

        /// <summary>
        /// Returns an enumerator that iterates through a DenormResultCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<DenormResult> GetEnumerator()
        {
            return this._denormResults.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a DenormResultCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._denormResults.GetEnumerator();
        }

        #endregion

        #region IDenormResultCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of DenormResultCollection object
        /// </summary>
        /// <returns>Xml string representing the DenormResultCollection</returns>
        public String ToXml()
        {
            String denormResultsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (DenormResult denormResult in this._denormResults)
            {
                builder.Append(denormResult.ToXml());
            }

            denormResultsXml = String.Format("<DenormResults>{0}</DenormResults>", builder.ToString());
            return denormResultsXml;
        }

        #endregion ToXml methods

        #endregion IDenormResultCollection Memebers

    }
}
