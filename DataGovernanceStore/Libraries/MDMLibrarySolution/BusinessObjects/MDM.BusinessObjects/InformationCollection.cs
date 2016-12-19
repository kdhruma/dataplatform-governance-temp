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
    /// Represent class for collection of message information
    /// </summary>
    [DataContract]
    public class InformationCollection : ICollection<Information>, IEnumerable<Information>, IInformationCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting collection of Information
        /// </summary>
        [DataMember]
        private Collection<Information> _informations = new Collection<Information>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public InformationCollection() : base() { }

        /// <summary>
        /// Initialize InformationCollection with Information
        /// </summary>
        /// <param name="information">Information object to add in InformationCollection</param>
        public InformationCollection(Information information)
        {
            if (information != null)
            {
                this._informations.Add(information);
            }
        }

        /// <summary>
        /// Initialize InformationCollection from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having Information for InformationCollection</param>
        public InformationCollection(String valuesAsXml)
        {
            LoadInformationCollection(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Find information from information collection based on the specified index
        /// </summary>
        /// <param name="index">Indicates the index to search in information collection</param>
        /// <returns>Returns the information object based on the specified index</returns>
        public Information this[Int32 index]
        {
            get
            {
                Information information = this._informations[index];
                if (information == null)
                    throw new ArgumentException(String.Format("No information found for index: {0}", index), "index");
                else
                    return information;
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
            if (obj is InformationCollection)
            {
                InformationCollection objectToBeCompared = obj as InformationCollection;
                Int32 menusUnion = this._informations.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 menuIntersect = this._informations.ToList().Intersect(objectToBeCompared.ToList()).Count();
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
            foreach (Information information in this._informations)
            {
                hashCode += information.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Determines whether the current object collection is superset of the information collection passed as parameter
        /// </summary>
        /// <param name="subsetInformationCollection">Indicates the subset object to compare with the current object</param>
        /// <returns>Returns true if the current object is superset of the subset instances; otherwise false</returns>
        public Boolean IsSuperSetOf(InformationCollection subsetInformationCollection)
        {
            foreach (Information subsetInformation in subsetInformationCollection)
            {
                Information information = this.Where(e => e.InformationCode == subsetInformation.InformationCode).ToList<Information>().FirstOrDefault();

                if (information == null)
                {
                    return false;
                }

                if (!information.IsSuperSetOf(subsetInformation))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate information collection from its xml passed as parameter
        /// </summary>
        /// <param name="valuesAsXml">Indicates xml values which is used to populate the information collection object</param>
        public void LoadInformationCollection(String valuesAsXml)
        {
            XmlTextReader reader = null;

            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Information")
                    {
                        String informationsXML = reader.ReadOuterXml();

                        if (!String.IsNullOrWhiteSpace(informationsXML))
                        {
                            Information information = new Information(informationsXML);

                            if (information != null)
                                this._informations.Add(information);
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

        #region ICollection<Information>

        /// <summary>
        /// Add Information object in collection
        /// </summary>
        /// <param name="item">Information to add in collection</param>
        public void Add(Information item)
        {
            if (Contains(item) == false)
            {
                this._informations.Add(item);
            }
        }

        /// <summary>
        /// Add a list of Information objects to the current collection
        /// </summary>
        /// <param name="items">Information collection to add in existing collection</param>
        public void AddRange(IInformationCollection items)
        {
            if (items != null && items.Count() > 0)
            {
                foreach (IInformation information in items)
                {
                    this.Add(information as Information);
                }
            }
        }

        /// <summary>
        /// Removes all information from collection
        /// </summary>
        public void Clear()
        {
            this._informations.Clear();
        }

        /// <summary>
        /// Determines whether the information collection contains a specific information object
        /// </summary>
        /// <param name="item">Indicates the information object to locate in the information collection</param>
        /// <returns>
        /// <para>true : If information is found in information collection</para>
        /// <para>false : If information is not found in information collection</para>
        /// </returns>
        public Boolean Contains(Information item)
        {
            Boolean result = false;

            if (this._informations != null && item != null)
            {
                foreach (Information info in this._informations)
                {
                    if (info.InformationCode == item.InformationCode && info.ReasonType == item.ReasonType && info.RuleId == item.RuleId && info.RuleMapContextId == item.RuleMapContextId
                         && ValueTypeHelper.CollectionExactEquals<Object>(info.Params, item.Params))
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Copies the elements of the InformationCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from InformationCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(Information[] array, int arrayIndex)
        {
            this._informations.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of information object in information collection
        /// </summary>
        public int Count
        {
            get
            {
                return this._informations.Count;
            }
        }

        /// <summary>
        /// Check if InformationCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the InformationCollection.
        /// </summary>
        /// <param name="item">The Information object to remove from the InformationCollection.</param>
        /// <returns>
        /// Returns true if item is successfully removed; otherwise, false. 
        /// This method also returns false if item was not found in the original InformationCollection
        /// </returns>
        public bool Remove(Information item)
        {
            return this._informations.Remove(item);
        }

        #endregion

        #region IEnumerable<Information>

        /// <summary>
        /// Returns an enumerator that iterates through a InformationCollection
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection</returns>
        public IEnumerator<Information> GetEnumerator()
        {
            return this._informations.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a InformationCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._informations.GetEnumerator();
        }

        #endregion

        #region IInformationCollection

        /// <summary>
        /// Get Xml representation of InformationCollection object
        /// </summary>
        /// <returns>Xml string representing the InformationCollection</returns>
        public String ToXml()
        {
            String informationCollectionXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (Information information in this._informations)
            {
                builder.Append(information.ToXml());
            }

            informationCollectionXml = String.Format("<Informations>{0}</Informations>", builder.ToString());

            return informationCollectionXml;
        }

        /// <summary>
        /// Get Xml representation of InformationCollection object
        /// </summary>
        /// <param name="objectSerialization">Indicates the serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String informationCollectionXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (Information information in this._informations)
            {
                builder.Append(information.ToXml(objectSerialization));
            }

            informationCollectionXml = String.Format("<Informations>{0}</Informations>", builder.ToString());

            return informationCollectionXml;
        }

        #endregion

        #endregion
    }
}
