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
    /// Specifies the Locale Instance Collection for the Object
    /// </summary>
    [DataContract]
    public class LocaleCollection : ICollection<Locale>, IEnumerable<Locale>, ILocaleCollection
    {
        #region Fields

        [DataMember]
        private Collection<Locale> _locales = new Collection<Locale>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public LocaleCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public LocaleCollection(String valueAsXml)
        {
            LoadLocaleCollection(valueAsXml);
        }

        /// <summary>
        /// Initialize LocaleCollection from IList
        /// </summary>
        /// <param name="localeList">IList of locales</param>
        public LocaleCollection(IList<Locale> localeList)
        {
            this._locales = new Collection<Locale>(localeList);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns the collection value wrapped by the current instance.
        /// </summary>
        public Collection<Locale> LocalCollectionValue
        {
            get { return this._locales; }
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
            if (obj is LocaleCollection)
            {
                LocaleCollection objectToBeCompared = obj as LocaleCollection;
                Int32 localeUnion = this._locales.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 localeIntersect = this._locales.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (localeUnion != localeIntersect)
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
            foreach (Locale locale in this._locales)
            {
                hashCode += locale.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public void LoadLocaleCollection(String valuesAsXml)
        {
            #region Sample Xml
            /*
             * <Locales></Locales>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Locale")
                        {
                            String localeXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(localeXml))
                            {
                                Locale locale = new Locale(localeXml);
                                this.Add(locale);
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

        /// <summary>
        /// Gets locale id list from the current locale collection
        /// </summary>
        /// <returns>Collection of locale ids in current collection</returns>
        public Collection<Int32> GetLocaleIds()
        {
            Collection<Int32> localeIds = null;

            if (this._locales != null && this._locales.Count > 0)
            {
                localeIds = new Collection<Int32>();

                foreach (Locale locale in this._locales)
                {
                    Int32 localeId = locale.Id;

                    if (localeId > 0)
                    {
                        localeIds.Add(localeId);
                    }
                }
            }

            return localeIds;
        }

        /// <summary>
        /// Returns a cloned instance of the locale collection object
        /// </summary>
        /// <returns>Cloned Locale Collection</returns>
        public ILocaleCollection Clone()
        {
            LocaleCollection clonedLocaleCollection = new LocaleCollection();
            foreach (Locale locale in _locales)
            {
                clonedLocaleCollection.Add((Locale)locale.Clone());
            }
            return clonedLocaleCollection;
        }

        #endregion

        #region Private Methods

        #endregion

        #region ICollection<Locale> Members

        /// <summary>
        /// Add Locale object in collection
        /// </summary>
        /// <param name="item">Locale to add in collection</param>
        public void Add(Locale item)
        {
            this._locales.Add(item);
        }

        /// <summary>
        /// Removes all locales from collection
        /// </summary>
        public void Clear()
        {
            this._locales.Clear();
        }

        /// <summary>
        /// Determines whether the LocaleCollection contains a specific Locale.
        /// </summary>
        /// <param name="item">The Locale object to locate in the LocaleCollection.</param>
        /// <returns>
        /// <para>true : If Locale found in mappingCollection</para>
        /// <para>false : If Locale found not in mappingCollection</para>
        /// </returns>
        public bool Contains(Locale item)
        {
            return this._locales.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the LocaleCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from LocaleCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(Locale[] array, int arrayIndex)
        {
            this._locales.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of Locales in LocaleCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._locales.Count;
            }
        }

        /// <summary>
        /// Check if LocaleCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Returns locale collection in form of LocaleEnum collection
        /// </summary>
        /// <returns>Returns locale collection in form of LocaleEnum collection</returns>
        public Collection<LocaleEnum> GetLocaleEnums()
        {
            Collection<LocaleEnum> localeEnums = new Collection<LocaleEnum>();
            LocaleEnum localeEnum = LocaleEnum.UnKnown;

            foreach (Locale locale in _locales)
            {
                ValueTypeHelper.EnumTryParse<LocaleEnum>(locale.Name, false, out localeEnum);
                localeEnums.Add(localeEnum);
            }

            return localeEnums;
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the LocaleCollection.
        /// </summary>
        /// <param name="item">The Locale object to remove from the LocaleCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original LocaleCollection</returns>
        public bool Remove(Locale item)
        {
            return this._locales.Remove(item);
        }

      
        #endregion

        #region IEnumerable<Locale> Members

        /// <summary>
        /// Returns an enumerator that iterates through a LocaleCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<Locale> GetEnumerator()
        {
            return this._locales.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a LocaleCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._locales.GetEnumerator();
        }

        #endregion

        #region ILocaleCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of LocaleColleciton object
        /// </summary>
        /// <returns>Xml string representing the LocaleCollection</returns>
        public String ToXml()
        {
            String localeXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (Locale locale in this._locales)
            {
                builder.Append(locale.ToXml());
            }

            localeXml = String.Format("<Locales>{0}</Locales>", builder.ToString());
            return localeXml;
        }

        #endregion ToXml methods

        #region Methods
        /// <summary>
        /// Determines whether the locale collection contains the locale corresponding to the specified locale enum.
        /// </summary>
        /// <param name="localeEnum">The locale enum.</param>
        /// <returns>
        /// True if locale is found else false.
        /// </returns>
        public Boolean Contains(LocaleEnum localeEnum)
        {
            Boolean localeFoundInLocaleCollection = false;

            foreach (Locale locale in _locales)
            {
                if (locale.Locale == localeEnum)
                {
                    localeFoundInLocaleCollection = true;
                    break;
                }
            }

            return localeFoundInLocaleCollection;
        }
        #endregion Methods

        #endregion ILocaleCollection Memebers


    }
}
