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
    /// Specifies the ReaderSetting Instance Collection for the Object
    /// </summary>
    [DataContract]
    public class ReaderSettingCollection : ICollection<ReaderSetting>, IEnumerable<ReaderSetting>, IReaderSettingCollection
    {
        #region Fields

        [DataMember]
        private Collection<ReaderSetting> _readerSettings = new Collection<ReaderSetting>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ReaderSettingCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public ReaderSettingCollection(String valueAsXml)
        {
            LoadReaderSettingCollection(valueAsXml);
        }

        /// <summary>
        /// Initialize ReaderSettingCollection from IList
        /// </summary>
        /// <param name="readerSettingsList">IList of execution steps</param>
        public ReaderSettingCollection(IList<ReaderSetting> readerSettingsList)
        {
            this._readerSettings = new Collection<ReaderSetting>(readerSettingsList);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Find job parameter from ReaderSettingCollection based on parameter name
        /// </summary>
        /// <param name="readerSettingName">Parameter name to be searched</param>
        /// <returns>JobParamter object having given parameter name </returns>
        public ReaderSetting this[String readerSettingName]
        {
            get
            {
                ReaderSetting readerSetting = Get(readerSettingName);

                //if (readerSetting == null)
                //    throw new ArgumentException(String.Format("No job parameter found with name: {0}", readerSettingName), "readerSettingName");
                //else
                    return readerSetting;
            }
            set
            {
                ReaderSetting readerSetting = Get(readerSettingName);

                //if (readerSetting == null)
                //    throw new ArgumentException(String.Format("No job parameter found with name: {0}", readerSettingName), "readerSettingName");

                readerSetting = value;
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
            if (obj is ReaderSettingCollection)
            {
                ReaderSettingCollection objectToBeCompared = obj as ReaderSettingCollection;
                Int32 readerSettingsUnion = this._readerSettings.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 readerSettingsIntersect = this._readerSettings.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (readerSettingsUnion != readerSettingsIntersect)
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
            foreach (ReaderSetting attr in this._readerSettings)
            {
                hashCode += attr.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public void LoadReaderSettingCollection( String valuesAsXml )
        {
            #region Sample Xml
            /*
             * <ReaderSettings></ReaderSettings>
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
                        if ( reader.NodeType == XmlNodeType.Element && reader.Name == "ReaderSetting" )
                        {
                            String parametersXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(parametersXml))
                            {
                                ReaderSetting readerSetting = new ReaderSetting(parametersXml);
                                this.Add(readerSetting);
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

        private ReaderSetting Get(String readerSettingName)
        {
            IEnumerable<ReaderSetting> filteredReaderSettings = null;

            filteredReaderSettings = this._readerSettings.Where(p => p.Name == readerSettingName);
            if (filteredReaderSettings != null && filteredReaderSettings.Any())
                return filteredReaderSettings.First();
            else
                return null;
        }

        #endregion

        #region ICollection<ReaderSetting> Members

        /// <summary>
        /// Add readerSetting object in collection
        /// </summary>
        /// <param name="item">readerSetting to add in collection</param>
        public void Add(ReaderSetting item)
        {
            this._readerSettings.Add(item);
        }

        /// <summary>
        /// Removes all execution steps from collection
        /// </summary>
        public void Clear()
        {
            this._readerSettings.Clear();
        }

        /// <summary>
        /// Determines whether the ReaderSettingCollection contains a specific readerSetting.
        /// </summary>
        /// <param name="item">The readerSetting object to locate in the ReaderSettingCollection.</param>
        /// <returns>
        /// <para>true : If readerSetting found in mappingCollection</para>
        /// <para>false : If readerSetting found not in mappingCollection</para>
        /// </returns>
        public bool Contains(ReaderSetting item)
        {
            return this._readerSettings.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the ReaderSettingCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from ReaderSettingCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(ReaderSetting[] array, int arrayIndex)
        {
            this._readerSettings.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of execution steps in ReaderSettingCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._readerSettings.Count;
            }
        }

        /// <summary>
        /// Check if ReaderSettingCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the ReaderSettingCollection.
        /// </summary>
        /// <param name="item">The readerSetting object to remove from the ReaderSettingCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original ReaderSettingCollection</returns>
        public bool Remove(ReaderSetting item)
        {
            return this._readerSettings.Remove(item);
        }

        #endregion

        #region IEnumerable<ReaderSetting> Members

        /// <summary>
        /// Returns an enumerator that iterates through a ReaderSettingCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<ReaderSetting> GetEnumerator()
        {
            return this._readerSettings.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a ReaderSettingCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._readerSettings.GetEnumerator();
        }

        #endregion

        #region IReaderSettingCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of ReaderSettingCollection object
        /// </summary>
        /// <returns>Xml string representing the ReaderSettingCollection</returns>
        public String ToXml()
        {
            String readerSettingsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (ReaderSetting readerSetting in this._readerSettings)
            {
                builder.Append(readerSetting.ToXml());
            }

            readerSettingsXml = String.Format("<ReaderSettings>{0}</ReaderSettings>", builder.ToString());
            return readerSettingsXml;
        }

        /// <summary>
        /// Get Xml representation of ReaderSettingCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml( ObjectSerialization serialization )
        {
            String readerSettingsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach ( ReaderSetting readerSetting in this._readerSettings )
            {
                builder.Append(readerSetting.ToXml(serialization));
            }

            readerSettingsXml = String.Format("<ReaderSettings>{0}</ReaderSettings>", builder.ToString());
            return readerSettingsXml;
        }

        #endregion ToXml methods

        #region ReaderSetting Get

        #endregion ReaderSetting Get
       

        #endregion IReaderSettingCollection Memebers
    }
}
