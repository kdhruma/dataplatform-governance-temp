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
    /// Object Class for WarningCollection
    /// </summary>
    [DataContract]
    public class WarningCollection : ICollection<Warning>, IEnumerable<Warning>, IWarningCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting collection of Warning
        /// </summary>
        [DataMember]
        private Collection<Warning> _warnings = new Collection<Warning>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public WarningCollection() : base() { }

        /// <summary>
        /// Initialize WarningCollection with Warning
        /// </summary>
        /// <param name="warning">Warning object to add in WarningCollection</param>
        public WarningCollection(Warning warning)
        {
            if (warning != null)
            {
                this._warnings.Add(warning);
            }
        }

        /// <summary>
        /// Initialize WarningCollection from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having Warning for WarningCollection</param>
        public WarningCollection( String valuesAsXml )
        {
            LoadWarningCollection(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Warning this[Int32 index]
        {
            get
            {
                Warning warning = this._warnings[index];
                if (warning == null)
                    throw new ArgumentException(String.Format("No information found for index: {0}", index), "index");
                else
                    return warning;
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
            if (obj is WarningCollection)
            {
                WarningCollection objectToBeCompared = obj as WarningCollection;
                Int32 menusUnion = this._warnings.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 menuIntersect = this._warnings.ToList().Intersect(objectToBeCompared.ToList()).Count();
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
            foreach (Warning warning in this._warnings)
            {
                hashCode += warning.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Determine whether the current warning collection object is the superset of the specified warning collection object passed or not.
        /// </summary>
        /// <param name="subsetWarningCollection">Indicates the warning collection which needs to be compared</param>
        /// <returns>Returns a boolean flag indicating whether the current warning collection object is the superset of the specified warning collection or not</returns>
        public Boolean IsSuperSetOf(WarningCollection subsetWarningCollection)
        {
            foreach (Warning subsetWarning in subsetWarningCollection)
            {
                Warning warning = this.Where(e => e.WarningCode == subsetWarning.WarningCode).ToList<Warning>().FirstOrDefault();

                if (warning == null)
                {
                    return false;
                }

                if (!warning.IsSuperSetOf(subsetWarning))
                {
                    return false;
                }
            }
            return true;
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// All the warnings it accepts and loads it.
        /// </summary>
        /// <param name="valuesAsXml">Values which has to be added as warning in XMl format</param>
        public void LoadWarningCollection(String valuesAsXml)
        {
            XmlTextReader reader = null;

            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Warning")
                    {
                        String warningsXML = reader.ReadOuterXml();

                        if (!String.IsNullOrWhiteSpace(warningsXML))
                        {
                            Warning warning = new Warning(warningsXML);

                            if (warning != null)
                                this._warnings.Add(warning);
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

        #region ICollection<Warning>

        /// <summary>
        /// Add Warning object in collection
        /// </summary>
        /// <param name="item">Warning to add in collection</param>
        public void Add(Warning item)
        {
            if (Contains(item) == false)
            {
                this._warnings.Add(item);
            }
        }

        /// <summary>
        /// Add warning objects to collection
        /// </summary>
        /// <param name="warnings">Indicates the warning object items which need to be added into the collection</param>
        public void AddRange(IWarningCollection warnings)
        {
            foreach (Warning warning in warnings)
            {
                this.Add(warning);
            }
        }

        /// <summary>
        /// Removes all Warning from collection
        /// </summary>
        public void Clear()
        {
            this._warnings.Clear();
        }

        /// <summary>
        /// Determines whether the WarningCollection contains a specific warning.
        /// </summary>
        /// <param name="item">The Warning object to locate in the WarningCollection.</param>
        /// <returns>
        /// <para>true : If warning found in WarningCollection</para>
        /// <para>false : If warning found not in WarningCollection</para>
        /// </returns>
        public Boolean Contains(Warning item)
        {
            Boolean result = false;

            if (this._warnings != null && item != null)
            {
                foreach (Warning warning in this._warnings)
                {
                    if (warning.WarningCode == item.WarningCode && warning.ReasonType == item.ReasonType && warning.RuleId == item.RuleId && warning.RuleMapContextId == item.RuleMapContextId
                         && ValueTypeHelper.CollectionExactEquals<Object>(warning.Params, item.Params))
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Copies the elements of the WarningCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from WarningCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(Warning[] array, int arrayIndex)
        {
            this._warnings.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of Warning in WarningCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._warnings.Count;
            }
        }

        /// <summary>
        /// Check if WarningCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the WarningCollection.
        /// </summary>
        /// <param name="item">The Warning object to remove from the WarningCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original WarningCollection</returns>
        public bool Remove(Warning item)
        {
            return this._warnings.Remove(item);
        }

        #endregion

        #region IEnumerable<Warning>

        /// <summary>
        /// Returns an enumerator that iterates through a WarningCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<Warning> GetEnumerator()
        {
            return this._warnings.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a WarningCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._warnings.GetEnumerator();
        }

        #endregion

        #region IWarningCollection

        /// <summary>
        /// Get Xml representation of WarningCollection object
        /// </summary>
        /// <returns>Xml string representing the WarningCollection</returns>
        public String ToXml()
        {
            String warningCollectionXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (Warning warning in this._warnings)
            {
                builder.Append(warning.ToXml());
            }

            warningCollectionXml = String.Format("<Warnings>{0}</Warnings>", builder.ToString());

            return warningCollectionXml;
        }

        /// <summary>
        /// Get Xml representation of WarningCollection object
        /// </summary>
        /// <param name="objectSerialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String warningCollectionXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (Warning warning in this._warnings)
            {
                builder.Append(warning.ToXml(objectSerialization));
            }

            warningCollectionXml = String.Format("<Warnings>{0}</Warnings>", builder.ToString());

            return warningCollectionXml;
        }

        #endregion

        #endregion
    }
}
