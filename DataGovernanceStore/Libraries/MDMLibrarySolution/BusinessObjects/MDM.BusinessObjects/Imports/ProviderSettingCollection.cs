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
    /// Specifies the ProviderSetting Instance Collection for the Object
    /// </summary>
    [DataContract]
    public class ProviderSettingCollection : ICollection<ProviderSetting>, IEnumerable<ProviderSetting>, IProviderSettingCollection
    {
        #region Fields

        [DataMember]
        private Collection<ProviderSetting> _providerSettings = new Collection<ProviderSetting>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ProviderSettingCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public ProviderSettingCollection(String valueAsXml)
        {
            LoadProviderSettingCollection(valueAsXml);
        }

        /// <summary>
        /// Initialize ProviderSettingCollection from IList
        /// </summary>
        /// <param name="providerSettingsList">IList of execution steps</param>
        public ProviderSettingCollection(IList<ProviderSetting> providerSettingsList)
        {
            this._providerSettings = new Collection<ProviderSetting>(providerSettingsList);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Find job parameter from ProviderSettingCollection based on parameter name
        /// </summary>
        /// <param name="providerSettingName">Parameter name to be searched</param>
        /// <returns>JobParamter object having given parameter name </returns>
        public ProviderSetting this[String providerSettingName]
        {
            get
            {
                ProviderSetting providerSetting = Get(providerSettingName);

                //if (providerSetting == null)
                //    throw new ArgumentException(String.Format("No job parameter found with name: {0}", providerSettingName), "providerSettingName");
                //else
                    return providerSetting;
            }
            set
            {
                ProviderSetting providerSetting = Get(providerSettingName);

                //if (providerSetting == null)
                //    throw new ArgumentException(String.Format("No job parameter found with name: {0}", providerSettingName), "providerSettingName");

                providerSetting = value;
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
            if (obj is ProviderSettingCollection)
            {
                ProviderSettingCollection objectToBeCompared = obj as ProviderSettingCollection;
                Int32 providerSettingsUnion = this._providerSettings.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 providerSettingsIntersect = this._providerSettings.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (providerSettingsUnion != providerSettingsIntersect)
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
            foreach (ProviderSetting attr in this._providerSettings)
            {
                hashCode += attr.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public void LoadProviderSettingCollection( String valuesAsXml )
        {
            #region Sample Xml
            /*
             * <ProviderSettings></ProviderSettings>
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
                        if ( reader.NodeType == XmlNodeType.Element && reader.Name == "ProviderSetting" )
                        {
                            String parametersXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(parametersXml))
                            {
                                ProviderSetting providerSetting = new ProviderSetting(parametersXml);
                                this.Add(providerSetting);
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

        private ProviderSetting Get(String providerSettingName)
        {
            IEnumerable<ProviderSetting> filteredProviderSettings = null;

            filteredProviderSettings = this._providerSettings.Where(p => p.Name == providerSettingName);
            if (filteredProviderSettings != null && filteredProviderSettings.Any())
                return filteredProviderSettings.First();
            else
                return null;
        }

        #endregion

        #region ICollection<ProviderSetting> Members

        /// <summary>
        /// Add providerSetting object in collection
        /// </summary>
        /// <param name="item">providerSetting to add in collection</param>
        public void Add(ProviderSetting item)
        {
            this._providerSettings.Add(item);
        }

        /// <summary>
        /// Removes all execution steps from collection
        /// </summary>
        public void Clear()
        {
            this._providerSettings.Clear();
        }

        /// <summary>
        /// Determines whether the ProviderSettingCollection contains a specific providerSetting.
        /// </summary>
        /// <param name="item">The providerSetting object to locate in the ProviderSettingCollection.</param>
        /// <returns>
        /// <para>true : If providerSetting found in mappingCollection</para>
        /// <para>false : If providerSetting found not in mappingCollection</para>
        /// </returns>
        public bool Contains(ProviderSetting item)
        {
            return this._providerSettings.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the ProviderSettingCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from ProviderSettingCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(ProviderSetting[] array, int arrayIndex)
        {
            this._providerSettings.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of execution steps in ProviderSettingCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._providerSettings.Count;
            }
        }

        /// <summary>
        /// Check if ProviderSettingCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the ProviderSettingCollection.
        /// </summary>
        /// <param name="item">The providerSetting object to remove from the ProviderSettingCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original ProviderSettingCollection</returns>
        public bool Remove(ProviderSetting item)
        {
            return this._providerSettings.Remove(item);
        }

        #endregion

        #region IEnumerable<ProviderSetting> Members

        /// <summary>
        /// Returns an enumerator that iterates through a ProviderSettingCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<ProviderSetting> GetEnumerator()
        {
            return this._providerSettings.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a ProviderSettingCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._providerSettings.GetEnumerator();
        }

        #endregion

        #region IProviderSettingCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of ProviderSettingCollection object
        /// </summary>
        /// <returns>Xml string representing the ProviderSettingCollection</returns>
        public String ToXml()
        {
            String providerSettingsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (ProviderSetting providerSetting in this._providerSettings)
            {
                builder.Append(providerSetting.ToXml());
            }

            providerSettingsXml = String.Format("<ProviderSettings>{0}</ProviderSettings>", builder.ToString());
            return providerSettingsXml;
        }

        /// <summary>
        /// Get Xml representation of ProviderSettingCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml( ObjectSerialization serialization )
        {
            String providerSettingsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach ( ProviderSetting providerSetting in this._providerSettings )
            {
                builder.Append(providerSetting.ToXml(serialization));
            }

            providerSettingsXml = String.Format("<ProviderSettings>{0}</ProviderSettings>", builder.ToString());
            return providerSettingsXml;
        }

        #endregion ToXml methods

        #region ProviderSetting Get

        #endregion ProviderSetting Get
       

        #endregion IProviderSettingCollection Memebers
    }
}
