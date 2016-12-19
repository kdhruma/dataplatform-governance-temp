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
    /// Specifies the Provider Value Instance Collection for the Object
    /// </summary>
    [DataContract]
    public class ProviderCollection : ICollection<Provider>, IEnumerable<Provider>
    {
        #region Fields

        //This is the most funny part of the WCF which is able to serialize even a private member.. needs to do further RnD to find out the internals
        [DataMember]
        private Collection<Provider> _providers = new Collection<Provider>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ProviderCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public ProviderCollection(String valueAsXml)
        {
            LoadProviderCollection(valueAsXml);
        }

        /// <summary>
        /// Initialize ProviderCollection from IList
        /// </summary>
        /// <param name="providersList">IList of providers</param>
        public ProviderCollection(IList<Provider> providersList)
        {
            this._providers = new Collection<Provider>(providersList);
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
            if (obj is ProviderCollection)
            {
                ProviderCollection objectToBeCompared = obj as ProviderCollection;
                Int32 providersUnion = this._providers.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 providersIntersect = this._providers.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (providersUnion != providersIntersect)
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
            foreach (Provider attr in this._providers)
            {
                hashCode += attr.GetHashCode();
            }
            return hashCode;
        }

        ///<summary>
        /// Load ProviderCollection object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        public void LoadProviderCollection(String valuesAsXml)
        {
            #region Sample Xml
            /*
             * <Providers></Providers>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Provider")
                        {
                            String providerXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(providerXml))
                            {
                                Provider provider = new Provider(providerXml);
                                this.Add(provider);
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


        #endregion

        #region ICollection<Provider> Members

        /// <summary>
        /// Add provider object in collection
        /// </summary>
        /// <param name="item">provider to add in collection</param>
        public void Add(Provider item)
        {
            this._providers.Add(item);
        }

        /// <summary>
        /// Removes all providers from collection
        /// </summary>
        public void Clear()
        {
            this._providers.Clear();
        }

        /// <summary>
        /// Determines whether the ProviderCollection contains a specific provider.
        /// </summary>
        /// <param name="item">The provider object to locate in the ProviderCollection.</param>
        /// <returns>
        /// <para>true : If provider found in ProviderCollection</para>
        /// <para>false : If provider found not in ProviderCollection</para>
        /// </returns>
        public bool Contains(Provider item)
        {
            return this._providers.Contains(item);
        }

        /// <summary>
        /// Determines whether the ProviderCollection contains a specific providerId.
        /// </summary>
        /// <param name="providerName">The provider object to locate in the ProviderCollection.</param>
        /// <returns>
        /// <para>true : If provider found in ProviderCollection</para>
        /// <para>false : If provider found not in ProviderCollection</para>
        /// </returns>
        public bool Contains(String providerName)
        {
            if (GetProvider(providerName) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Copies the elements of the ProviderCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from ProviderCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(Provider[] array, int arrayIndex)
        {
            this._providers.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of providers in ProviderCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._providers.Count;
            }
        }

        /// <summary>
        /// Check if ProviderCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the ProviderCollection.
        /// </summary>
        /// <param name="item">The provider object to remove from the ProviderCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original ProviderCollection</returns>
        public bool Remove(Provider item)
        {
            return this._providers.Remove(item);
        }

        #endregion

        #region IEnumerable<Provider> Members

        /// <summary>
        /// Returns an enumerator that iterates through a ProviderCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<Provider> GetEnumerator()
        {
            return this._providers.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a ProviderCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._providers.GetEnumerator();
        }

        #endregion

        #region IProviderCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of ProviderCollection object
        /// </summary>
        /// <returns>Xml string representing the ProviderCollection</returns>
        public String ToXml()
        {
            String providersXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (Provider provider in this._providers)
            {
                builder.Append(provider.ToXml());
            }

            providersXml = String.Format("<Providers>{0}</Providers>", builder.ToString());
            return providersXml;
        }

        /// <summary>
        /// Get Xml representation of ProviderCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml( ObjectSerialization serialization )
        {
            String providersXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (Provider provider in this._providers)
            {
                builder.Append(provider.ToXml(serialization));
            }

            providersXml = String.Format("<Providers>{0}</Providers>", builder.ToString());
            return providersXml;
        }

        #endregion ToXml methods

        #region Provider Get

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        public Provider GetProvider(String providerName)
        {
            if (this._providers == null)
            {
                throw new NullReferenceException("There are no providers to search in");
            }

            if (String.IsNullOrWhiteSpace(providerName))
            {
                throw new ArgumentException("Provider name is empty");
            }

            Provider entity = (from provider in this._providers
                                    where provider.Name.ToLowerInvariant().Equals(providerName)
                             select provider).ToList().FirstOrDefault();

            return entity;
        }

        #endregion Provider Get


        #endregion IProviderCollection Memebers
    }
}
