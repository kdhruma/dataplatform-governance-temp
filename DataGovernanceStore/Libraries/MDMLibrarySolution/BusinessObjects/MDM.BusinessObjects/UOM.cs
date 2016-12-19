using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies class for UOM
    /// </summary>
    [DataContract]
    public class UOM : MDMObject,IUOM
    {
        #region Fields

        /// <summary>
        /// Indicates Key
        /// </summary>
        String _key = String.Empty;

        /// <summary>
        /// Indicates Unit Type ShortName 
        /// </summary>
        String _unitTypeShortName = String.Empty;

        /// <summary>
        /// Indicates Unit Type LongName 
        /// </summary>
        String _unitTypeLongName = String.Empty;

        /// <summary>
        /// indicates UOMType Id
        /// </summary>
        Int32 _uomTypeId = -1;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public UOM()
            : base()
        {
        }

        /// <summary>
        /// Load Row from Xml
        /// </summary>
        /// <param name="valuesAsXml">
        /// Xml representing row value
        /// <para>
        /// Sample Xml :
        /// <![CDATA[
        ///<UnitType shortName="Angle" longName="Angle">
        ///     <Unit ID="269" key="degrees" shortName="degrees" longName="Degrees"/>
        ///</UnitType>
        ///<UnitType shortName="Angle - Plane" longName="Angle - Plane">
        ///     <Unit ID="128" key="deg" shortName="deg" longName="deg"/>
        ///     <Unit ID="354" key="rev" shortName="rev" longName="Revolutions"/>
        ///</UnitType>
        /// ]]>
        /// </para>
        /// <param name="unitTypeShortName">Short name of the unit type</param>
        /// <param name="unitTypeLongName">Long name of the unit type</param>
        /// </param>
        public UOM(String valuesAsXml, String unitTypeShortName, String unitTypeLongName)
        {

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {

                    this.UnitTypeShortName = unitTypeShortName;
                    this.UnitTypeLongName = unitTypeLongName;


                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Unit" && reader.HasAttributes)
                    {
                        if (reader.MoveToAttribute("ID"))
                        {
                            this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                        }
                        if (reader.MoveToAttribute("key"))
                        {
                            this.Key = reader.ReadContentAsString();
                        }
                        if (reader.MoveToAttribute("shortName"))
                        {
                            this.Name = reader.ReadContentAsString();
                        }
                        if (reader.MoveToAttribute("longName"))
                        {
                            this.LongName = reader.ReadContentAsString();
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

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Property denoting key of UOM
        /// </summary>
        [DataMember]
        public String Key
        {
            get { return _key; }
            set { _key = value; }
        }

        /// <summary>
        /// Property denoting Unit Type ShortName
        /// </summary>
        [DataMember]
        public String UnitTypeShortName
        {
            get
            {
                return _unitTypeShortName;
            }
            set
            {
                _unitTypeShortName = value;
            }
        }

        /// <summary>
        /// Property denoting type id of UOM
        /// </summary>
        public Int32 UomTypeId
        {
            get
            {
                return _uomTypeId;
            }
            set
            {
                _uomTypeId = value;
            }

        }

        /// <summary>
        /// Property denoting Unit Type LongName
        /// </summary>
        [DataMember]
        public String UnitTypeLongName
        {
            get
            {
                return _unitTypeLongName;
            }
            set
            {
                _unitTypeLongName = value;
            }
        }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Creates a clone copy of UOM options specifying a subset of its properties
        /// </summary>
        /// <returns>Returns a clone copy of UOM options specifying a subset of its properties</returns>
        public IUOM Clone()
        {
            UOM uom = new UOM();
            uom.Action = this.Action;
            uom.AuditRefId=this.AuditRefId;
            uom.ExtendedProperties = this.ExtendedProperties;
            uom.Id = this.Id;
            uom.Key = this.Key;
            uom.Locale = this.Locale;
            uom.Name = this.Name;
            uom.LongName = this.LongName;
            uom.UnitTypeLongName = this.UnitTypeLongName;
            uom.UnitTypeShortName = this.UnitTypeShortName;
            return uom;
        }

        /// <summary>
        /// Represents Row in Xml format
        /// </summary>
        ///  XMl Format
        ///  <UOM ID="269" ShortName="degrees" LongName="Degrees" Key="degrees" UnitTypeShortName="Angle" UnitTypeLongName="Angle" />
        ///  <UOM ID="128" ShortName="deg" LongName="deg" Key="deg" UnitTypeShortName="Angle - Plane" UnitTypeLongName="Angle - Plane" />
        ///  <UOM ID="354" ShortName="rev" LongName="Revolutions" Key="rev" UnitTypeShortName="Angle - Plane" UnitTypeLongName="Angle - Plane" />
        ///  <UOM ID="131" ShortName="rpm" LongName="rpm" Key="rpm" UnitTypeShortName="Angular Velocity" UnitTypeLongName="Angular Velocity" />
        ///  <UOM ID="121" ShortName="in**2" LongName="in**2" Key="in**2" UnitTypeShortName="Area" UnitTypeLongName="Area" />
        ///  TODO: Currently ToXml Format is different from loadAttributeModel's xml format.
        /// <returns>String representing Row in Xml format</returns>
        public override String ToXml()
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //UOM Start
            xmlWriter.WriteStartElement("UOM");
            xmlWriter.WriteAttributeString("ID", this.Id.ToString());
            xmlWriter.WriteAttributeString("ShortName", this.Name);
            xmlWriter.WriteAttributeString("LongName", this.LongName);
            xmlWriter.WriteAttributeString("Key", this.Key);
            xmlWriter.WriteAttributeString("UnitTypeShortName", this.UnitTypeShortName);
            xmlWriter.WriteAttributeString("UnitTypeLongName", this.UnitTypeLongName);

            //UOM end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Represents Row in Xml format for given Serialization option
        /// </summary>
        /// <param name="objectSerialization">Options specifying which Xml format is to be generated</param>
        /// <returns>String representing Row in Xml format</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            //Currently this is same as ToXml
            return this.ToXml();
        }

        #region Override Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is UOM)
                {
                    UOM objectToBeCompared = obj as UOM;

                    if (this.Key != objectToBeCompared.Key)
                        return false;

                    if (!this.UnitTypeShortName.Equals(objectToBeCompared.UnitTypeShortName))
                        return false;

                    if (!this.UnitTypeLongName.Equals(objectToBeCompared.UnitTypeLongName))
                        return false;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            Int32 hashCode = 0;

            hashCode = base.GetHashCode() ^ this.Key.GetHashCode() ^ this.UnitTypeShortName.GetHashCode() ^
                       this.UnitTypeLongName.GetHashCode();

            return hashCode;
        }

        #endregion

        #endregion Public Methods
    }
}
