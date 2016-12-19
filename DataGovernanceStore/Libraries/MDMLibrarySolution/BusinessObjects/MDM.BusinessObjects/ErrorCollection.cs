using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    
    /// <summary>
    /// Object Class for ErrorCollection
    /// </summary>
    [DataContract]
    public class ErrorCollection : ICollection<Error>, IEnumerable<Error>, IErrorCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting collection of Error
        /// </summary>
        [DataMember]
        private Collection<Error> _errors = new Collection<Error>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ErrorCollection() : base() { }

        /// <summary>
        /// Initialize ErrorCollection with Error
        /// </summary>
        /// <param name="error">Error object to add in ErrorCollection</param>
        public ErrorCollection(Error error)
        {
            if (error != null)
            {
                this._errors.Add(error);
            }
        }

        /// <summary>
        /// Initialize ErrorCollection from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having Error for ErrorCollection</param>
        public ErrorCollection(String valuesAsXml)
        {
            LoadErrorCollection(valuesAsXml);
        }

        #endregion
        
        #region Properties

        /// <summary>
        /// readonly property Error based on indexer
        /// </summary>
        /// <param name="index">array position</param>
        /// <returns>the error object</returns>
        public Error this[Int32 index]
        {
            get
            {
                Error error = this._errors[index];
                if (error == null)
                    throw new ArgumentException(String.Format("No error found for index: {0}", index), "index");
                else
                    return error;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is ErrorCollection)
            {
                ErrorCollection objectToBeCompared = obj as ErrorCollection;
                Int32 menusUnion = this._errors.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 menuIntersect = this._errors.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (menusUnion != menuIntersect)
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
            foreach (Error error in this._errors)
            {
                hashCode += error.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Compares to check if the current error collection is super set of the specified error collection in parameter
        /// </summary>
        /// <param name="subsetErrorCollection">ErrorCollection to be compared with current attribute object</param>
        /// <returns>True : Is both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(ErrorCollection subsetErrorCollection)
        {
            foreach (Error subsetError in subsetErrorCollection)
            {
                Error error = this.Where(e => e.ErrorCode == subsetError.ErrorCode).ToList<Error>().FirstOrDefault();

                if (error == null)
                {
                    return false;
                }

                if (!error.IsSuperSetOf(subsetError))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determine whether any system error exists in error collection
        /// </summary>
        /// <returns>Returns true if there us any system error exists in error collection; otherwise false.</returns>
        public Boolean HasAnySystemError()
        {
            Boolean reprocessEntities = false;

            foreach (Error error in this._errors)
            {
                if (error.ReasonType == ReasonType.SystemError)
                {
                    reprocessEntities = true;
                    break;
                }
            }

            return reprocessEntities;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// It accepts an XML value, parses it and then loads the error collection 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public void LoadErrorCollection(String valuesAsXml)
        {
            XmlTextReader reader = null;

            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Error")
                    {
                        String errorsXML = reader.ReadOuterXml();

                        if (!String.IsNullOrWhiteSpace(errorsXML))
                        {
                            Error error = new Error(errorsXML);

                            if (error != null)
                                this._errors.Add(error);
                        }
                    }                    
                    else
                    {
                        reader.Read();
                    }
                }

                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        #endregion

        #region ICollection<Error>

        /// <summary>
        /// Add Error object in collection
        /// </summary>
        /// <param name="item">Error to add in collection</param>
        public void Add(Error item)
        {
            if (Contains(item) == false)
            {
                this._errors.Add(item);
            }
        }

        /// <summary>
        /// Add Error object in collection
        /// </summary>
        /// <param name="item">Error to add in collection</param>
        public void Add(IError item)
        {
            if (item != null)
            {
                this._errors.Add((Error)item);
            }
        }

        /// <summary>
        /// Add a list of Error objects to the current collection
        /// </summary>
        /// <param name="items"></param>
        public void AddRange(Collection<IError> items)
        {
            if (items != null)
            {
                foreach (IError error in items)
                {
                    this.Add(error);
                }
            }
        }

        /// <summary>
        /// Add a list of Error objects to the current collection
        /// </summary>
        /// <param name="items">Error collection to add in existing collection</param>
        public void AddRange(IErrorCollection items)
        {
            if (items != null && items.Count > 0)
            {
                foreach (IError error in items)
                {
                    this.Add(error);
                }
            }
        }

        /// <summary>
        /// Removes all Error from collection
        /// </summary>
        public void Clear()
        {
            this._errors.Clear();
        }

        /// <summary>
        /// Determines whether the ErrorCollection contains a specific Error.
        /// </summary>
        /// <param name="item">The Error object to locate in the ErrorCollection.</param>
        /// <returns>
        /// <para>true : If Error found in ErrorCollection</para>
        /// <para>false : If Error found not in ErrorCollection</para>
        /// </returns>
        public Boolean Contains(Error item)
        {
            Boolean result = false;

            if (this._errors != null && item != null)
            {
                foreach (Error error in this._errors)
                {
                    if (error.ErrorCode == item.ErrorCode && error.ReasonType == item.ReasonType && error.RuleId == item.RuleId && error.RuleMapContextId == item.RuleMapContextId
                        && ValueTypeHelper.CollectionExactEquals<Object>(error.Params,item.Params))
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Copies the elements of the ErrorCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from ErrorCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(Error[] array, int arrayIndex)
        {
            this._errors.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of Error in ErrorCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._errors.Count;
            }
        }

        /// <summary>
        /// Check if ErrorCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the ErrorCollection.
        /// </summary>
        /// <param name="item">The Error object to remove from the ErrorCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original ErrorCollection</returns>
        public bool Remove(Error item)
        {
            return this._errors.Remove(item);
        }

        #endregion

        #region IEnumerable<Error>

        /// <summary>
        /// Returns an enumerator that iterates through a ErrorCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<Error> GetEnumerator()
        {
            return this._errors.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a ErrorCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._errors.GetEnumerator();
        }

        #endregion

        #region IErrorCollection

        /// <summary>
        /// Get Xml representation of ErrorCollection object
        /// </summary>
        /// <returns>Xml string representing the ErrorCollection</returns>
        public String ToXml()
        {
            String errorCollectionXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (Error error in this._errors)
            {
                builder.Append(error.ToXml());
            }

            errorCollectionXml = String.Format("<Errors>{0}</Errors>", builder.ToString());

            return errorCollectionXml;
        }

        /// <summary>
        /// Get Xml representation of ErrorCollection object
        /// </summary>
        /// <param name="objectSerialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String errorCollectionXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (Error error in this._errors)
            {
                builder.Append(error.ToXml(objectSerialization));
            }

            errorCollectionXml = String.Format("<Errors>{0}</Errors>", builder.ToString());

            return errorCollectionXml;
        }

        #endregion

        #endregion
    }
}
