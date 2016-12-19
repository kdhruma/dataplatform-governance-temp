using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies class for UOM Collection
    /// </summary>
    [DataContract]
    public class UOMCollection : ICollection<UOM>, IEnumerable<UOM>, IUOMCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting instance of UOM collection
        /// </summary>
        [DataMember]
        private Collection<UOM> _uoms = new Collection<UOM>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public UOMCollection()
            : base() 
        {
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public UOMCollection(String valueAsXml)
        {
            LoadUOMCollection(valueAsXml);
        }

        /// <summary>
        /// Constructs copy of given uomCollection
        /// </summary>
        /// <param name="uomCollection"></param>
        public UOMCollection(IEnumerable<UOM> uomCollection)
        {
            _uoms = new Collection<UOM>(uomCollection.ToList());
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Creates a clone copy of UOM collection
        /// </summary>
        /// <returns>Returns a clone copy of UOM collection</returns>
        public IUOMCollection Clone()
        {
            var clonedUOMCollection = new UOMCollection();

            if (this._uoms != null && this._uoms.Count > 0)
            {
                foreach (UOM uom in this._uoms)
                {
                    IUOM clonedUOM = uom.Clone();
                    clonedUOMCollection._uoms.Add((UOM)clonedUOM);
                }
            }

            return clonedUOMCollection;
        }

        #region Load Methods

        /// <summary>
        /// Load UOM Collection from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml representation of UOM Collection</param>
        public void LoadUOMCollection(String valuesAsXml)
        {

            #region SampleXml
            //<UnitsOfMeasure>
            //  <UnitType shortName="Angle" longName="Angle">
            //      <Unit ID="269" key="degrees" shortName="degrees" longName="Degrees"/>
            //  </UnitType>
            //  <UnitType shortName="Angle - Plane" longName="Angle - Plane">
            //      <Unit ID="128" key="deg" shortName="deg" longName="deg"/>
            //      <Unit ID="354" key="rev" shortName="rev" longName="Revolutions"/>
            //  </UnitType>
            //</UnitsOfMeasure>
            #endregion

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                String unitTypeShortName = String.Empty;
                String unitTypeLongName = String.Empty;

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && (reader.Name == "UnitType" || reader.Name == "Unit") && reader.HasAttributes)
                    {
                        if (reader.Name.Equals("UnitType"))
                        {
                            if (reader.MoveToAttribute("shortName"))
                            {
                                unitTypeShortName = reader.ReadContentAsString();
                            }
                            if (reader.MoveToAttribute("longName"))
                            {
                                unitTypeLongName = reader.ReadContentAsString();
                            }
                        }
                        else if (reader.Name.Equals("Unit"))
                        {
                            UOM uom = new UOM(reader.ReadOuterXml(), unitTypeShortName, unitTypeLongName);
                            if (uom != null)
                            {
                                this.Add(uom);
                            }
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

        #endregion

        #region Override Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is UOMCollection)
            {
                UOMCollection objectToBeCompared = obj as UOMCollection;
                Int32 uomsUnion = this._uoms.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 uomsIntersect = this._uoms.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if (uomsUnion != uomsIntersect)
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

            foreach (UOM uom in this._uoms)
            {
                hashCode += uom.GetHashCode();
            }

            return hashCode;
        }

        #endregion

        #region ICollection<UOM> Members

        /// <summary>
        /// Add UOM object in collection
        /// </summary>
        /// <param name="item">UOM to add in collection</param>
        public void Add(UOM item)
        {
            this._uoms.Add(item);
        }

        /// <summary>
        /// Add UOM object in collection
        /// </summary>
        /// <param name="item">IUOM to add in collection</param>
        public void Add(IUOM item)
        {
            if (item != null)
            {
                this._uoms.Add((UOM)item);
            }
        }

        /// <summary>
        /// Removes all UOMs from collection
        /// </summary>
        public void Clear()
        {
            this._uoms.Clear();
        }

        /// <summary>
        /// Determines whether the UOM collection contains a specific SecurityPermission.
        /// </summary>
        /// <param name="item">The UOM object to locate in the UOMCollection.</param>
        /// <returns>
        /// <para>true : If UOM found in mappingCollection</para>
        /// <para>false : If UOM found not in mappingCollection</para>
        /// </returns>
        public bool Contains(UOM item)
        {
            return this._uoms.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the UOM Collection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from UOM Collection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(UOM[] array, int arrayIndex)
        {
            this._uoms.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of UOMs in UOM Collection
        /// </summary>
        public int Count
        {
            get
            {
                return this._uoms.Count;
            }
        }

        /// <summary>
        /// Check if UOM Collection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the UOM Collection.
        /// </summary>
        /// <param name="item">The UOM object to remove from the UOM Collection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original UOM Collection</returns>
        public bool Remove(UOM item)
        {
            return this._uoms.Remove(item);
        }

        #endregion

        #region IEnumerable<UOM> Members

        /// <summary>
        /// Returns an enumerator that iterates through a UOM Collection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<UOM> GetEnumerator()
        {
            return this._uoms.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a UOM Collection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._uoms.GetEnumerator();
        }

        #endregion

        #region UOMCollection Members

        #region ToXml methods
        
        /// <summary>
        /// Get Xml representation of UOM Collection object
        /// </summary>
        /// XML Format:
        /// <UOMCollection>
        ///  <UOM ID="269" ShortName="degrees" LongName="Degrees" Key="degrees" UnitTypeShortName="Angle" UnitTypeLongName="Angle" />
        ///  <UOM ID="128" ShortName="deg" LongName="deg" Key="deg" UnitTypeShortName="Angle - Plane" UnitTypeLongName="Angle - Plane" />
        ///  <UOM ID="354" ShortName="rev" LongName="Revolutions" Key="rev" UnitTypeShortName="Angle - Plane" UnitTypeLongName="Angle - Plane" />
        ///  <UOM ID="131" ShortName="rpm" LongName="rpm" Key="rpm" UnitTypeShortName="Angular Velocity" UnitTypeLongName="Angular Velocity" />
        ///  <UOM ID="121" ShortName="in**2" LongName="in**2" Key="in**2" UnitTypeShortName="Area" UnitTypeLongName="Area" />
        ///  TODO: Currently ToXml Format is different from loadAttributeModel's xml format.
        /// </UOMCollection>
        /// <returns>Xml string representing the UOM Collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<UOMCollection>";

            if (this._uoms != null && this._uoms.Count > 0)
            {
                foreach (UOM uom in this._uoms)
                {
                    returnXml = String.Concat(returnXml, uom.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</UOMCollection>");

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of UOM Collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization"></param>
        /// <returns></returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            //Currently this is same as ToXml
            return this.ToXml();
        }

        #endregion ToXml methods

        #endregion UOMCollection Members

        /// <summary>
        /// Gets UOM based on provided UomId
        /// </summary>
        /// <param name="uomId">id of UOM which needs to be get from UOMCollection</param>
        /// <returns>UOM object</returns>
        public UOM GetUOMById(Int32 uomId)
        {
            UOM uom = null;

            if (this._uoms != null)
            {
                foreach (UOM currentUom in _uoms)
                {
                    if (currentUom.Id == uomId) 
                    {
                        uom = currentUom;
                        break;
                    }
                }
            }

            return uom;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uomContext"></param>
        /// <param name="stringComparison"></param>
        /// <returns></returns>
        public IUOM FindUOM(UomContext uomContext, StringComparison stringComparison = StringComparison.InvariantCulture)
        {
            UOM filteredUOM = null;
            
            if (!String.IsNullOrWhiteSpace(uomContext.UomType) && (!String.IsNullOrWhiteSpace(uomContext.UomShortName) || uomContext.UomId > 0))
            {
                foreach (UOM uom in this._uoms)
                {
                    if (uom.Key != null && uom.UnitTypeShortName != null)
                    {
                        if ((uom.Id == uomContext.UomId || String.Compare(uom.Key, uomContext.UomShortName, stringComparison) == 0) && String.Compare(uom.UnitTypeShortName, uomContext.UomType, stringComparison) == 0)
                        {
                            filteredUOM = uom;
                            break;
                        }
                    }
                }
            }

            return filteredUOM;
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}