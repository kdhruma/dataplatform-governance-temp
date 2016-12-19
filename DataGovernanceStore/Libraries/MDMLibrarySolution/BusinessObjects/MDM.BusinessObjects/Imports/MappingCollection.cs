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
    /// Specifies the Mapping Value Instance Collection for the Object
    /// </summary>
    [DataContract]
    public class MappingCollection : ICollection<Mapping>, IEnumerable<Mapping>
    {
        #region Fields

        //This is the most funny part of the WCF which is able to serialize even a private member.. needs to do further RnD to find out the internals
        [DataMember]
        private Collection<Mapping> _mappings = new Collection<Mapping>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public MappingCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public MappingCollection(String valueAsXml)
        {
            LoadMappingCollection(valueAsXml);
        }

        /// <summary>
        /// Initialize MappingCollection from IList
        /// </summary>
        /// <param name="mappingsList">IList of mappings</param>
        public MappingCollection(IList<Mapping> mappingsList)
        {
            this._mappings = new Collection<Mapping>(mappingsList);
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
            if (obj is MappingCollection)
            {
                MappingCollection objectToBeCompared = obj as MappingCollection;
                Int32 mappingsUnion = this._mappings.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 mappingsIntersect = this._mappings.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (mappingsUnion != mappingsIntersect)
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
            foreach (Mapping attr in this._mappings)
            {
                hashCode += attr.GetHashCode();
            }
            return hashCode;
        }

        ///<summary>
        /// Load MappingCollection object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        public void LoadMappingCollection( String valuesAsXml )
        {
            #region Sample Xml
            /*
             * <Mappings></Mappings>
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
                        if ( reader.NodeType == XmlNodeType.Element && reader.Name == "Mapping" )
                        {
                            String mappingXml = reader.ReadOuterXml();
                            if ( !String.IsNullOrEmpty(mappingXml) )
                            {
                                Mapping mapping= new Mapping(mappingXml);
                                this.Add(mapping);
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

        #region ICollection<Mapping> Members

        /// <summary>
        /// Add mapping object in collection
        /// </summary>
        /// <param name="item">mapping to add in collection</param>
        public void Add(Mapping item)
        {
            this._mappings.Add(item);
        }

        /// <summary>
        /// Removes all mappings from collection
        /// </summary>
        public void Clear()
        {
            this._mappings.Clear();
        }

        /// <summary>
        /// Determines whether the MappingCollection contains a specific mapping.
        /// </summary>
        /// <param name="item">The mapping object to locate in the MappingCollection.</param>
        /// <returns>
        /// <para>true : If mapping found in mappingCollection</para>
        /// <para>false : If mapping found not in mappingCollection</para>
        /// </returns>
        public bool Contains(Mapping item)
        {
            return this._mappings.Contains(item);
        }

        /// <summary>
        /// Determines whether the MappingCollection contains a specific mapping.
        /// </summary>
        /// <param name="sourceName">Expected Source Name</param>
        /// <param name="targetName">Expected Target Name</param>
        /// <returns>
        /// <para>true : If mapping found in mappingCollection</para>
        /// <para>false : If mapping found not in mappingCollection</para>
        /// </returns>
        public Boolean Contains(String sourceName, String targetName)
        {
            return this._mappings.Any(mapping => String.Compare(mapping.SourceName, sourceName, true) == 0 &&
                String.Compare(mapping.TargetName, targetName, true) == 0);
        }

        /// <summary>
        /// Copies the elements of the MappingCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from MappingCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(Mapping[] array, int arrayIndex)
        {
            this._mappings.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of mappings in MappingCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._mappings.Count;
            }
        }

        /// <summary>
        /// Check if MappingCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the MappingCollection.
        /// </summary>
        /// <param name="item">The mapping object to remove from the MappingCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original MappingCollection</returns>
        public bool Remove(Mapping item)
        {
            return this._mappings.Remove(item);
        }

        #endregion

        #region IEnumerable<Mapping> Members

        /// <summary>
        /// Returns an enumerator that iterates through a MappingCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<Mapping> GetEnumerator()
        {
            return this._mappings.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a MappingCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._mappings.GetEnumerator();
        }

        #endregion

        #region IMappingCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of MappingCollection object
        /// </summary>
        /// <returns>Xml string representing the MappingCollection</returns>
        public String ToXml()
        {
            String mappingsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach ( Mapping mapping in this._mappings )
            {
                builder.Append(mapping.ToXml());
            }

            mappingsXml = String.Format("<Mappings>{0}</Mappings>", builder.ToString());
            return mappingsXml;
        }

        /// <summary>
        /// Get Xml representation of MappingCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml( ObjectSerialization serialization )
        {
            String mappingsXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach ( Mapping mapping in this._mappings )
            {
                builder.Append(mapping.ToXml(serialization));
            }

            mappingsXml = String.Format("<Mappings>{0}</Mappings>", builder.ToString());
            return mappingsXml;
        }

        #endregion ToXml methods

        #region Mapping Get

        #endregion Mapping Get
       

        #endregion IMappingCollection Memebers
    }
}
