using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    using System.Text;

    /// <summary>
    /// Specifies the Attribute Value
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class Value : MDMObject, IValue
    {
        #region Fields

        /// <summary>
        /// Field for the id of an object
        /// </summary>
        private Int64 _id = -1;

        /// <summary>
        /// field denoting the de-formatted value of an Attribute
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        private String _invariantVal = null;

        /// <summary>
        /// field denoting the value of an Attribute
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        private String _attrVal = null;

        /// <summary>
        /// field denoting the id of the UOM of attribute value
        /// </summary>
        private Int32 _uomId = -1;

        /// <summary>
        /// field denoting UOM of attribute value
        /// </summary>
        private String _uom = String.Empty;

        /// <summary>
        /// Field denoting the record id of referred table for attribute value
        /// </summary>
        private Int32 _valueRefId = -1;

        /// <summary>
        /// Field denoting the sequence number of attribute value
        /// </summary>
        private Decimal _sequence = -1;

        /// <summary>
        /// Field denoting string attribute value
        /// </summary>
        private String _stringVal = null;

        /// <summary>
        /// Field denoting numeric attribute value
        /// </summary>
        private Nullable<Decimal> _numericVal = null;

        /// <summary>
        /// Field denoting date time attribute value
        /// </summary>
        private Nullable<DateTime> _dateVal = null;

        /// <summary>
        /// Field denoting attribute display value
        /// </summary>
        [DataMember]
        [ProtoMember(10)]
        private String _displayVal = null;

        /// <summary>
        /// Field denoting attribute export value
        /// </summary>
        [DataMember]
        [ProtoMember(11)]
        private String _exportVal = null;

        /// <summary>
        /// Indicates if given value is invalid with respect to all the basic validations.
        /// </summary>
        private Boolean _hasInvalidValue = false;

        /// <summary>
        /// Property defines which program is the source info of changes of object
        /// </summary>
        private SourceInfo _sourceInfo;

        /// <summary>
        /// Field denoting Lookup row details for current value
        /// </summary>
        private Dictionary<String, String> _extendedValues = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public Value()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Value class
        /// </summary>
        /// <param name="attrValue">Attribute Value</param>
        public Value(Object attrValue)
        {
            // since object can be anything(may not be serialized too), we need to make string representation of object and store that only for now..
            //TODO:: Figure out real object usage
            string strAttrval = attrValue != null ? attrValue.ToString() : String.Empty;
            this._attrVal = strAttrval;
            this._invariantVal = strAttrval;
        }

        /// <summary>
        /// Populate AttributeValue object from xml
        /// </summary>
        /// <example>
        /// Sample XML
        /// <para>
        /// &lt;Value 
        ///     AttrVal="cliaff.test" 
        ///     Uom="" 
        ///     ValueRefId="104" 
        ///     Locale="en_WW" 
        ///     Sequence="-1" /&gt;
        /// </para>
        /// </example>
        /// <param name="attributeDataType">Data type of in which value is to be cast and save.</param>
        /// <param name="valuesAsXml">String XML representing the value to fill in AttributeValue object</param>
        public Value(AttributeDataType attributeDataType, String valuesAsXml)
        {
            LoadValueFromXml(valuesAsXml);

            // TODO:: Need helper or enum for attribute data type
            if (this.AttrVal != null)
            {
                switch (attributeDataType)
                {
                    case AttributeDataType.String:
                        this.StringVal = this.AttrVal.ToString();
                        break;
                    case AttributeDataType.Integer:
                        break;
                    case AttributeDataType.Fraction:
                        Decimal decimalVal = 0;
                        if (MDM.Core.ValueTypeHelper.FractionTryParse(this.AttrVal.ToString(), out decimalVal, NumberStyles.Integer, new CultureInfo("en-US")))
                        {
                            this.NumericVal = decimalVal;
                        }
                        break;
                    case AttributeDataType.Decimal:
                        Decimal decimalValForFraction = 0;
                        if (Decimal.TryParse(this.AttrVal.ToString(), out decimalValForFraction))
                        {
                            this.NumericVal = decimalValForFraction;
                        }
                        break;
                    case AttributeDataType.Date:
                    case AttributeDataType.DateTime:
                        DateTime value = DateTime.MinValue;
                        if (DateTime.TryParse(this.AttrVal.ToString(), out value))
                        {
                            this.DateVal = value;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Parameterized Consrtuctor with value As Xml
        /// </summary>
        /// <param name="valueAsXml">Values in XMl format which needs to be initialized with the object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public Value(String valueAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadValueFromXml(valueAsXml, objectSerialization);
        }

        #endregion

        #region Properties

        /// <summary>
        /// field denoting the de-formatted value of an Attribute
        /// </summary>
        public Object InvariantVal
        {
            get { return _invariantVal; }
            set
            {
                _invariantVal = (value == null) ? null : value.ToString();

                if (_dateVal != null) GetDateTimeValue();
                if (_numericVal != null) GetNumericValue();
            }
        }

        /// <summary>
        /// Property denoting the value of an Attribute
        /// </summary>
        public Object AttrVal
        {
            get
            {
                return _attrVal;
            }
            set
            {
                _attrVal = (value == null || value == DBNull.Value) ? null : value.ToString();
            }
            }

        /// <summary>
        /// Property denoting the id of the UOM of attribute value
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public Int32 UomId
        {
            get { return _uomId; }
            set { _uomId = value; }
        }

        /// <summary>
        /// Property denoting UOM of attribute value
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        public String Uom
        {
            get { return _uom; }
            set { _uom = value; }
        }

        /// <summary>
        /// Property denoting the record id of referred table for attribute value
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        public Int32 ValueRefId
        {
            get
            {
                return _valueRefId;
            }
            set
            {
                _valueRefId = value;
            }
        }

        /// <summary>
        /// Property denoting the sequence number of attribute value
        /// </summary>
        [DataMember]
        [ProtoMember(6), DefaultValue(-1)]
        public Decimal Sequence
        {
            get { return _sequence; }
            set { _sequence = value; }
        }

        /// <summary>
        /// Property denoting string attribute value
        /// </summary>
        //[DataMember]
        public String StringVal
        {
            get { return GetStringValue(); }
            set { _stringVal = value; }
        }

        /// <summary>
        /// Property denoting numeric attribute value
        /// </summary>
        //[DataMember]
        public Nullable<Decimal> NumericVal
        {
            get
            {
                if (this._invariantVal != null && this._numericVal == null)
                    GetNumericValue();

                return _numericVal;
            }
            set { _numericVal = value; }
        }

        /// <summary>
        /// Property denoting date time attribute value
        /// </summary>
        //[DataMember]
        public Nullable<DateTime> DateVal
        {
            get
            {
                if (this._invariantVal != null && this._dateVal == null)
                    GetDateTimeValue();

                return _dateVal;
            }
            set
            {
                _dateVal = value;
            }
        }

        /// <summary>
        /// Indicates the Id of an object
        /// </summary>
        [DataMember]
        [ProtoMember(7)]
        public new Int64 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Indicates if given value is invalid with respect to all the basic validations.
        /// </summary>
        [DataMember]
        [ProtoMember(8)]
        public Boolean HasInvalidValue
        {
            get { return _hasInvalidValue; }
            set { _hasInvalidValue = value; }
        }

        /// <summary>
        /// Property defines which program is the source info of changes of object
        /// </summary>
        [DataMember]
        [ProtoMember(9)]
        public SourceInfo SourceInfo
        {
            get
            {
                return _sourceInfo;
            }
            set
            {
                _sourceInfo = value;
            }
        }

        /// <summary>
        /// Property denoting extended values
        /// </summary>
        [DataMember]
        [ProtoMember(12)]
        public Dictionary<String, String> ExtendedValues
        {
            get
            {
                return this._extendedValues;
            }
            set
            {
                this._extendedValues = value;
            }
        }

        #endregion

        #region Methods

        #region IValue Members

        #region ToXml methods

        /// <summary>
        /// Get XML representation of AttributeValue object
        /// </summary>
        /// <returns>XML representation of AttributeValue</returns>
        public override String ToXml()
        {
            String valueXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            ConvertValueToXml(xmlWriter);

            xmlWriter.Flush();

            //get the actual XML
            valueXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return valueXml;
        }

        /// <summary>
        /// Get XML representation of AttributeValue object
        /// </summary>
        /// <param name="serialization">Serialization option. Based on the value given XML representation will differ</param>
        /// <param name="attributeDataType">Specifies data type of an value being serialized</param>
        /// <returns>XML representation of AttributeValue</returns>
        public String ToXml(ObjectSerialization serialization, AttributeDataType attributeDataType)
        {
            String valueXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                valueXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);
                String attrVal = (this.AttrVal == null) ? String.Empty : this.AttrVal.ToString();

                //Attribute node start
                xmlWriter.WriteStartElement("Value");

                if (serialization == ObjectSerialization.DataStorage)
                {
                    #region write Value Properties for DataStorage Xml

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("LName", this.LongName);
                    xmlWriter.WriteAttributeString("UomId", this.UomId.ToString());
                    xmlWriter.WriteAttributeString("Uom", this.Uom);
                    xmlWriter.WriteAttributeString("ValRefId", this.ValueRefId.ToString());
                    xmlWriter.WriteAttributeString("Seq", this.Sequence.ToString());
                    xmlWriter.WriteAttributeString("DispVal", this.GetDisplayValue());
                    xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());

                    if (this.InvariantVal != null)
                    {
                        xmlWriter.WriteAttributeString("InvariantVal", this.InvariantVal.ToString());
                    }

                    xmlWriter.WriteAttributeString("HasInvalidVal", this.HasInvalidValue.ToString().ToLowerInvariant());

                    if (!String.IsNullOrWhiteSpace(attrVal))
                    {
                        if (attributeDataType == AttributeDataType.DateTime || attributeDataType == AttributeDataType.Date)
                        {
                            String dateAttrVal = String.Empty;
                            Nullable<DateTime> nullableDateTime = this.GetDateTimeValue();

                            //value data type check is already done by validation, so it will always be correct
                            //the only case is keyword, for example DELETE, then value doesn't match data type
                            //so if we cant convert the value to DateTime, put blank
                            if (nullableDateTime != null)
                            {
                                dateAttrVal = nullableDateTime.Value.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
                            }

                            xmlWriter.WriteAttributeString("DateVal", dateAttrVal);
                        }
                        else if ((attributeDataType == AttributeDataType.Integer || attributeDataType == AttributeDataType.Decimal) && this.NumericVal != null)
                        {
                            //in case of keyword coming as value, this may be a non-numeric values here, don't get scared!
                            xmlWriter.WriteAttributeString("NumVal", this.NumericVal.ToString());
                        }
                        else
                        {
                            xmlWriter.WriteAttributeString("StringVal", attrVal);
                        }
                    }

                    #endregion write Value Properties for ProcessingOnly Xml
                }
                else if (serialization == ObjectSerialization.DataTransfer)
                {
                    #region write Value Properties for DataStorage Xml

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("UId", this.UomId.ToString());
                    xmlWriter.WriteAttributeString("U", this.Uom);
                    xmlWriter.WriteAttributeString("VRId", this.ValueRefId.ToString());
                    xmlWriter.WriteAttributeString("Seq", this.Sequence.ToString());
                    xmlWriter.WriteAttributeString("DispV", this.GetDisplayValue());
                    xmlWriter.WriteAttributeString("L", this.Locale.ToString());
                    xmlWriter.WriteAttributeString("A", this.Action.ToString());

                    if (this.InvariantVal != null)
                    {
                        xmlWriter.WriteAttributeString("IV", this.InvariantVal.ToString());
                    }

                    xmlWriter.WriteAttributeString("HIVV", this.HasInvalidValue.ToString().ToLowerInvariant());

                    if (!String.IsNullOrWhiteSpace(attrVal))
                    {
                        if (attributeDataType == AttributeDataType.DateTime || attributeDataType == AttributeDataType.Date)
                        {
                            String dateAttrVal = String.Empty;
                            Nullable<DateTime> nullableDateTime = this.GetDateTimeValue();

                            //value data type check is already done by validation, so it will always be correct
                            //the only case is keyword, for example DELETE, then value doesn't match data type
                            //so if we cant convert the value to DateTime, put blank
                            if (nullableDateTime != null)
                            {
                                dateAttrVal = nullableDateTime.Value.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
                            }

                            xmlWriter.WriteAttributeString("DV", dateAttrVal);
                        }
                        else if ((attributeDataType == AttributeDataType.Integer || attributeDataType == AttributeDataType.Decimal) && this.NumericVal != null)
                        {
                            //in case of keyword coming as value, this may be a non-numeric values here, don't get scared!
                            xmlWriter.WriteAttributeString("NV", this.NumericVal.ToString());
                        }
                        else
                        {
                            xmlWriter.WriteAttributeString("SV", attrVal);
                        }
                    }

                    #endregion write Value Properties for ProcessingOnly Xml
                }
                else
                {
                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());

                    xmlWriter.WriteAttributeString("Uom", this.Uom);
                    xmlWriter.WriteAttributeString("ValueRefId", this.ValueRefId.ToString());
                    xmlWriter.WriteAttributeString("Sequence", this.Sequence.ToString());
                    xmlWriter.WriteAttributeString("DisplayValue", this.GetDisplayValue());
                    xmlWriter.WriteAttributeString("HasInvalidValue", this.HasInvalidValue.ToString());

                    if (serialization == ObjectSerialization.ProcessingOnly)
                        xmlWriter.WriteAttributeString("LocaleId", ((Int32)this.Locale).ToString()); // Pass locale id instead of name while sending for processing
                    else
                        xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());

                    if (serialization == ObjectSerialization.ProcessingOnly && !String.IsNullOrWhiteSpace(attrVal))
                    {
                        if (attributeDataType == AttributeDataType.DateTime || attributeDataType == AttributeDataType.Date)
                        {
                            String dateAttrVal = String.Empty;
                            Nullable<DateTime> nullableDateTime = this.GetDateTimeValue();

                            //value data type check is already done by validation, so it will always be correct
                            //the only case is keyword, for example DELETE, then value doesn't match data type
                            //so if we cant convert the value to DateTime, put blank
                            if (nullableDateTime != null)
                            {
                                dateAttrVal = nullableDateTime.Value.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
                            }

                            xmlWriter.WriteAttributeString("DateVal", dateAttrVal);
                        }
                        else if (attributeDataType == AttributeDataType.Integer || attributeDataType == AttributeDataType.Decimal)
                        {
                            //in case of keyword coming as value, this may be a non-numeric values here, don't get scared!
                            xmlWriter.WriteAttributeString("NumVal", attrVal);
                        }
                    }

                    if (serialization == ObjectSerialization.External)
                    {
                        xmlWriter.WriteAttributeString("Action", ValueTypeHelper.GetActionString(this.Action));
                    }
                    else if (serialization != ObjectSerialization.DataTransfer)
                    {
                        xmlWriter.WriteAttributeString("Action", this.Action.ToString());
                    }
                }
                if (this.SourceInfo != null)
                {
                    xmlWriter.WriteAttributeString("SourceId", this.SourceInfo.SourceId.ToString());
                    xmlWriter.WriteAttributeString("SourceEntityId", this.SourceInfo.SourceEntityId.ToString());
                }

                //Write attribute value as value of node. Not as attribute.
                xmlWriter.WriteCData(CleanInvalidXmlChars(attrVal));

                //Value node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                valueXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }

            return valueXml;
        }

        /// <summary>
        /// Converts AttributeValue object into xml format.
        /// In future, this method can be enhanced to accept EntityConversion object
        /// </summary>
        /// <param name="xmlWriter">Indicates a writer to generate xml reperesentation of AttributeValue object</param>
        internal void ConvertValueToXml(XmlTextWriter xmlWriter)
        {
            if (xmlWriter != null)
            {
                //Attribute node start
                xmlWriter.WriteStartElement("Value");

                xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                xmlWriter.WriteAttributeString("Name", this.Name);
                xmlWriter.WriteAttributeString("LongName", this.LongName);
                xmlWriter.WriteAttributeString("UomId", this._uomId.ToString());
                xmlWriter.WriteAttributeString("Uom", this._uom);
                xmlWriter.WriteAttributeString("ValueRefId", this._valueRefId.ToString());
                xmlWriter.WriteAttributeString("Sequence", this._sequence.ToString());
                xmlWriter.WriteAttributeString("DisplayValue", this._displayVal);
                xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
                xmlWriter.WriteAttributeString("Action", this.Action.ToString());
                xmlWriter.WriteAttributeString("HasInvalidValue", this._hasInvalidValue.ToString().ToLowerInvariant());
                if (_sourceInfo != null)
                {
                    xmlWriter.WriteAttributeString("SourceId", this._sourceInfo.SourceId.ToString());
                    xmlWriter.WriteAttributeString("SourceEntityId", this._sourceInfo.SourceEntityId.ToString());
                }

                String attrVal = this._attrVal == null ? String.Empty : this._attrVal.ToString();

                //Write attribute value as value of node. Not as attribute.
                WriteCData(CleanInvalidXmlChars(attrVal), xmlWriter);

                //Value node end
                xmlWriter.WriteEndElement();
            }
            else
            {
                throw new ArgumentNullException("xmlWriter", "XmlTextWriter is null. Can not write Value object.");
            }
        }


        #endregion ToXml methods

        #region Get values in specific type

        /// <summary>
        /// Gets String value of current attribute value
        /// </summary>
        /// <returns>String representation of current value</returns>
        /// <exception cref="Exception">AttrVal is null. Cannot get String value</exception>
        public String GetStringValue()
        {
            String attrVal = String.Empty;

            if (this.AttrVal != null)
            {
                attrVal = this.AttrVal.ToString();
            }

            return attrVal;
        }

        /// <summary>
        /// Gets Numeric value of current attribute value
        /// </summary>
        /// <returns>String representation of current value</returns>
        /// <exception cref="Exception">AttrVal is null. Cannot get Numeric value</exception>
        public Nullable<Decimal> GetNumericValue()
        {
            if (_invariantVal != null)
            {
                Decimal decimalVal = 0;

                if (MDM.Core.ValueTypeHelper.FractionTryParse(_invariantVal, out decimalVal, Constants.STORAGE_NUMBER_STYLE, Constants.STORAGE_CULTUREINFO))
                {
                    this._numericVal = decimalVal;
                }
            }
            else
            {
                this._numericVal = null;
            }

            return this._numericVal;
        }

        /// <summary>
        /// Gets Date value of current attribute value
        /// </summary>
        /// <returns>String representation of current value</returns>
        /// <exception cref="Exception">AttrVal is null. Cannot get Date value</exception>
        public Nullable<DateTime> GetDateTimeValue()
        {
            if (_invariantVal != null)
            {
                DateTime value = DateTime.MinValue;

                if (DateTime.TryParse(_invariantVal, Constants.STORAGE_CULTUREINFO, Constants.STORAGE_DATETIME_STYLE, out value))
                {
                    this._dateVal = value;
                }
            }
            else
            {
                this._dateVal = null;
            }

            return this._dateVal;
        }

        /// <summary>
        /// Gets attribute display value
        /// </summary>
        /// <returns>String representation of display value</returns>
        public String GetDisplayValue()
        {
            return this._displayVal;
        }

        /// <summary>
        /// Sets attribute display value
        /// </summary>
        public void SetDisplayValue(String displayVal)
        {
            this._displayVal = displayVal;
        }

        /// <summary>
        /// Gets attribute export value
        /// </summary>
        /// <returns>String representation of display value</returns>
        public String GetExportValue()
        {
            String exportVal = this._exportVal;

            if (String.IsNullOrEmpty(exportVal))
            {
                exportVal = this.GetDisplayValue();
            }

            return exportVal;
        }

        /// <summary>
        /// Sets attribute export value
        /// </summary>
        public void SetExportValue(String exportVal)
        {
            this._exportVal = exportVal;
        }

        #endregion Get values in specific type

        #region Utility methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invariantVal"></param>
        internal void SetInVariantVal(object invariantVal)
        {
            this._invariantVal = (String)invariantVal;
            // this._attrVal = invariantVal; // do not keep attrval as it is direct now..
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetBlank()
        {
            this.AttrVal = String.Empty;
            this.InvariantVal = String.Empty;
            this._displayVal = String.Empty;
            this._exportVal = String.Empty;
        }

        /// <summary>
        /// Clear all the properties for current value.
        /// </summary>
        public void Clear()
        {
            this._id = 0;
            this._invariantVal = null;
            this._attrVal = null;
            this.UomId = 0;
            this.Uom = String.Empty;
            this.ValueRefId = 0;
            this.Sequence = -1;
            this.StringVal = null;
            this.NumericVal = null;
            this.DateVal = null;
            this.Locale = Core.LocaleEnum.UnKnown;
            this._displayVal = null;
            this._exportVal = null;
        }

        /// <summary>
        /// Determines whether two Value objects instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public bool ValueEquals(IValue obj)
        {
            return ValueEquals(obj, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Determines whether two Value objects instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <param name="stringComparison">String comparison options</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public bool ValueEquals(IValue obj, StringComparison stringComparison)
        {
            if (obj != null)
            {
                var objectToBeCompared = obj as Value;

                if (objectToBeCompared == null)
                    return false;

                if (this.InvariantVal != null && objectToBeCompared.InvariantVal != null)
                {
                    if (!this.InvariantVal.ToString().Equals(objectToBeCompared.InvariantVal.ToString(), stringComparison))
                        return false;
                }
                else if ((this.InvariantVal != null && objectToBeCompared.InvariantVal == null) || (this.InvariantVal == null && objectToBeCompared.InvariantVal != null))
                {
                    return false;
                }

                if ((this.UomId > 0 || objectToBeCompared.UomId > 0) && this.UomId != objectToBeCompared.UomId)
                    return false;

                // As UomId has already been verified and Uom value is of a lower priority, they are compared only if the objectToBeCompared has a value.
                if (!String.IsNullOrWhiteSpace(objectToBeCompared.Uom) && this.Uom != objectToBeCompared.Uom)
                    return false;

                if (this.Locale != objectToBeCompared.Locale)
                    return false;

                if ((this.ValueRefId > 0 || objectToBeCompared.ValueRefId > 0) && this.ValueRefId != objectToBeCompared.ValueRefId)
                    return false;

                if (this.HasInvalidValue != objectToBeCompared.HasInvalidValue)
                    return false;

                // In case of values are same and source info are different with source tracking module enabled
                // And MDMCenter.Entity.SourceTracking.TrackHistoryOnlyOnValueChange config value is false no need to create audit history 
                if (!Constants.TRACK_HISTORY_ONLY_ON_VALUE_CHANGE)
                {
                if (this.SourceInfo != null && objectToBeCompared.SourceInfo != null)
                {
                    if (!this.SourceInfo.IsSourceEquals(objectToBeCompared.SourceInfo))
                        return false;
                }
                else if ((this.SourceInfo != null && objectToBeCompared.SourceInfo == null) || (this.SourceInfo == null && objectToBeCompared.SourceInfo != null))
                {
                    return false;
                }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultUOM"></param>
        /// <returns></returns>
        public Value MergeCloneValue(String defaultUOM)
        {
            Value clonedVal = new Value();

            clonedVal._id = this._id;
            clonedVal._invariantVal = this._invariantVal;
            clonedVal._attrVal = this._attrVal;

            if (String.IsNullOrWhiteSpace(defaultUOM) == false && String.IsNullOrWhiteSpace(this._uom))
            {
                clonedVal._uom = defaultUOM;
            }
            else
            {
                clonedVal._uom = this._uom;
            }
            clonedVal._uomId = this._uomId;
            clonedVal._valueRefId = this._valueRefId;
            clonedVal._sequence = this._sequence;
            clonedVal._stringVal = this._stringVal;
            if (this._sourceInfo != null)
            {
                clonedVal._sourceInfo = (SourceInfo)this._sourceInfo.Clone();
            }
            else
            {
                clonedVal._sourceInfo = null;
            }
            clonedVal._numericVal = this._numericVal;
            clonedVal._dateVal = this._dateVal;
            clonedVal.Locale = this.Locale;
            clonedVal.Action = this.Action;
            clonedVal.AuditRefId = this.AuditRefId;
            clonedVal.ExtendedProperties = this.ExtendedProperties;
            clonedVal.ProgramName = this.ProgramName;
            clonedVal.UserName = this.UserName;
            clonedVal._displayVal = this._displayVal;
            clonedVal._exportVal = this._exportVal;
            clonedVal._hasInvalidValue = this._hasInvalidValue;

            return clonedVal;
        }

        /// <summary>
        /// Clear all the properties for current value.
        /// </summary>
        public Value Clone()
        {
            Value clonedVal = new Value();

            clonedVal._id = this._id;
            clonedVal._invariantVal = this._invariantVal;
            clonedVal._attrVal = this._attrVal;
            clonedVal._uom = this._uom;
            clonedVal._uomId = this._uomId;
            clonedVal._valueRefId = this._valueRefId;
            clonedVal._sequence = this._sequence;
            clonedVal._stringVal = this._stringVal;
            if (this._sourceInfo != null)
            {
                clonedVal._sourceInfo = (SourceInfo)this._sourceInfo.Clone();
            }
            else
            {
                clonedVal._sourceInfo = null;
            }
            clonedVal._numericVal = this._numericVal;
            clonedVal._dateVal = this._dateVal;
            clonedVal.Locale = this.Locale;
            clonedVal.Action = this.Action;
            clonedVal.AuditRefId = this.AuditRefId;
            clonedVal.ExtendedProperties = this.ExtendedProperties;
            clonedVal.ProgramName = this.ProgramName;
            clonedVal.UserName = this.UserName;
            clonedVal._displayVal = this._displayVal;
            clonedVal._exportVal = this._exportVal;
            clonedVal._hasInvalidValue = this._hasInvalidValue;
            return clonedVal;
        }

        #endregion

        #endregion IValue Members

        #region Object management methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is Value)
                {
                    Value objectToBeCompared = obj as Value;

                    if (this.AttrVal != null)
                    {
                        if (!this.AttrVal.Equals(objectToBeCompared.AttrVal))
                        {
                            return false;
                        }

                        if (this.StringVal != objectToBeCompared.StringVal)
                        {
                            return false;
                        }

                        if (this.NumericVal != objectToBeCompared.NumericVal)
                        {
                            return false;
                        }

                        if (this.DateVal != objectToBeCompared.DateVal)
                        {
                            return false;
                        }
                    }

                    if (this.UomId != objectToBeCompared.UomId)
                    {
                        return false;
                    }

                    if (this.Uom != objectToBeCompared.Uom)
                    {
                        return false;
                    }

                    if ((this.ValueRefId > 0 || objectToBeCompared.ValueRefId > 0) && this.ValueRefId != objectToBeCompared.ValueRefId)
                    {
                        return false;
                    }

                    if (this.Sequence != objectToBeCompared.Sequence)
                    {
                        return false;
                    }

                    //TODO:: Do we need to consider SourceInfo for Equality?
                    //if ((this.SourceInfo != null && objectToBeCompared.SourceInfo == null) ||
                    //    (this.SourceInfo == null && objectToBeCompared.SourceInfo != null) ||
                    //    (this.SourceInfo != null && objectToBeCompared.SourceInfo != null && !this.SourceInfo.Equals(objectToBeCompared.SourceInfo)))
                    //{
                    //    return false;
                    //}

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode() ^ this.UomId.GetHashCode() ^ this.ValueRefId.GetHashCode() ^ this.Sequence.GetHashCode();

            if (this.AttrVal != null)
            {
                hashCode = hashCode ^ this.AttrVal.GetHashCode();
                if (this.StringVal != null)
                {
                    hashCode = hashCode ^ this.StringVal.GetHashCode();
                }
                if (this.NumericVal != null)
                {
                    hashCode = hashCode ^ this.NumericVal.GetHashCode();
                }
                if (this.DateVal != null)
                {
                    hashCode = hashCode ^ this.DateVal.GetHashCode();
                }
            }

            if (this.Uom != null)
            {
                hashCode = hashCode ^ this.Uom.GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Populate current AttributeValue object from XML
        /// </summary>
        /// <example>
        /// Sample XML
        /// <para>
        /// <![CDATA[
        ///     <Value 
        ///         Uom="" 
        ///         ValueRefId="-1" 
        ///         Sequence="1" 
        ///         Locale="en_WW"
        ///     </Value>
        /// ]]>
        /// </para>
        /// </example>
        /// <param name="valuesAsXml">String representing XML from which Value is to be populated.</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public void LoadValueFromXml(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            //    <Value 
            //        Uom="" 
            //        ValueRefId="-1" 
            //        Sequence="1" 
            //        Locale="en_WW">
            //            <![CDATA[WEB]]>
            //    </Value>
            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                if (objectSerialization == ObjectSerialization.DataStorage)
                {
                    LoadValueForDataStorage(valuesAsXml);
                }
                else if (objectSerialization == ObjectSerialization.DataTransfer)
                {
                    LoadValueForDataTransfer(valuesAsXml);
                }
                else
                {
                    XmlTextReader reader = null;
                    try
                    {
                        reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                        while (!reader.EOF)
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "Value")
                            {
                                //Read entity metadata
                                #region Read Value Properties

                                //Using GetAttribute because we need to read element value. 
                                //"reader.MoveToAttribute" will move cursor forward and we won't be able to read element value.
                                LoadValueMetadataFromXml(reader);

                                #endregion Read Value Properties
                            }
                            reader.Read();
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
        }

        /// <summary>
        /// Populate current AttributeValue object from XML when Enum of ObjectSerialization is DataStorage
        /// </summary>
        /// <example>
        /// Sample XML
        /// <para>
        ///     <Value 
        ///         Uom="" 
        ///         ValRefId="-1" 
        ///         Seq="1" 
        ///         Locale="en_WW">
        ///             <![CDATA[WEB]]>
        ///     </Value>
        /// </para>
        /// </example>
        /// <param name="valuesAsXml">String representing XML from which Value is to be populated.</param>
        public void LoadValueForDataStorage(String valuesAsXml)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Value")
                    {
                        //Read entity metadata
                        #region Read Entity Attributes

                        if (reader.HasAttributes)
                        {
                            //Using GetAttribute because we need to read element value. 
                            //"reader.MoveToAttribute" will move cursor forward and we won't be able to read element value.

                            if (reader.GetAttribute("Id") != null)
                            {
                                this.Id = ValueTypeHelper.Int64TryParse(reader.GetAttribute("Id"), this.Id);
                            }
                            if (reader.GetAttribute("Name") != null)
                            {
                                this.Name = reader.GetAttribute("Name");
                            }
                            if (reader.GetAttribute("LName") != null)
                            {
                                this.LongName = reader.GetAttribute("LName");
                            }
                            if (reader.GetAttribute("UomId") != null)
                            {
                                this.UomId = ValueTypeHelper.Int32TryParse(reader.GetAttribute("UomId"), this.UomId);
                            }

                            if (reader.GetAttribute("Uom") != null)
                            {
                                this.Uom = reader.GetAttribute("Uom");
                            }

                            if (reader.GetAttribute("ValRefId") != null)
                            {
                                this.ValueRefId = ValueTypeHelper.Int32TryParse(reader.GetAttribute("ValRefId"), this.ValueRefId);
                            }

                            if (reader.GetAttribute("Seq") != null)
                            {
                                this.Sequence = ValueTypeHelper.DecimalTryParse(reader.GetAttribute("Seq"), this.Sequence);
                            }

                            if (reader.MoveToAttribute("LocaleId"))
                            {
                                Int32 localeId = reader.ReadContentAsInt();
                                LocaleEnum locale = LocaleEnum.UnKnown;
                                locale = (LocaleEnum)localeId;

                                this.Locale = locale;
                            }
                            else if (reader.MoveToAttribute("Locale"))
                            {
                                String strLocale = reader.ReadContentAsString();
                                LocaleEnum locale = LocaleEnum.UnKnown;
                                Enum.TryParse<LocaleEnum>(strLocale, true, out locale);

                                this.Locale = locale;
                            }

                            if (reader.MoveToAttribute("Action"))
                            {
                                this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("HasInvalidVal"))
                            {
                                this.HasInvalidValue = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                            }

                            if (reader.GetAttribute("InvariantVal") != null)
                            {
                                this.InvariantVal = reader.ReadContentAsString();
                            }

                            if (reader.GetAttribute("DateVal") != null)
                            {
                                this.DateVal = ValueTypeHelper.ConvertToDateTime(reader.ReadContentAsString());
                            }

                            if (reader.GetAttribute("NumVal") != null)
                            {
                                this.NumericVal = ValueTypeHelper.ConvertToDecimal(reader.ReadContentAsString());
                            }

                            if (reader.GetAttribute("StringVal") != null)
                            {
                                this.StringVal = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("SourceId"))
                            {
                                SourceInfo sourceInfo = new SourceInfo();
                                sourceInfo.SourceId = reader.ReadContentAsInt();
                                if (reader.MoveToAttribute("SourceEntityId"))
                                {
                                    sourceInfo.SourceEntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                }
                                this.SourceInfo = sourceInfo;
                            }

                            if (reader.MoveToElement())
                            {
                                //AttrVal is represented as Element value. So we are reading current element's context as string and assign it to AttrVal.
                                this._attrVal = reader.ReadElementContentAsObject().ToString();
                            }
                        }
                        #endregion Read Entity Attributes
                    }
                    reader.Read();
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

        /// <summary>
        /// Loads properties of Value object from xml
        /// </summary>
        /// <param name="reader">Indicates an xml reader used to read xml format data</param>	
        internal void LoadValueMetadataFromXml(XmlTextReader reader)
        {
            if (reader != null)
            {
                if (reader.HasAttributes)
                {
                    if (reader.GetAttribute("Id") != null)
                    {
                        this.Id = ValueTypeHelper.Int64TryParse(reader.GetAttribute("Id"), this.Id);
                    }
                    if (reader.GetAttribute("Name") != null)
                    {
                        this.Name = reader.GetAttribute("Name");
                    }
                    if (reader.GetAttribute("LongName") != null)
                    {
                        this.LongName = reader.GetAttribute("LongName");
                    }
                    if (reader.GetAttribute("UomId") != null)
                    {
                        this._uomId = ValueTypeHelper.Int32TryParse(reader.GetAttribute("UomId"), this.UomId);
                    }

                    if (reader.GetAttribute("Uom") != null)
                    {
                        this._uom = reader.GetAttribute("Uom");
                    }

                    if (reader.GetAttribute("ValueRefId") != null)
                    {
                        this._valueRefId = ValueTypeHelper.Int32TryParse(reader.GetAttribute("ValueRefId"), this.ValueRefId);
                    }

                    if (reader.GetAttribute("Sequence") != null)
                    {
                        this._sequence = ValueTypeHelper.DecimalTryParse(reader.GetAttribute("Sequence"), this.Sequence);
                    }

                    if (reader.MoveToAttribute("LocaleId"))
                    {
                        Int32 localeId = reader.ReadContentAsInt();
                        LocaleEnum locale = LocaleEnum.UnKnown;
                        locale = (LocaleEnum)localeId;

                        this.Locale = locale;
                    }
                    else if (reader.MoveToAttribute("Locale"))
                    {
                        String strLocale = reader.ReadContentAsString();
                        LocaleEnum locale = LocaleEnum.UnKnown;
                        Enum.TryParse<LocaleEnum>(strLocale, true, out locale);

                        this.Locale = locale;
                    }

                    if (reader.MoveToAttribute("Action"))
                    {
                        this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
                    }
                    if (reader.MoveToAttribute("HasInvalidValue"))
                    {
                        this._hasInvalidValue = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                    }
                    if (reader.MoveToAttribute("SourceId"))
                    {
                        SourceInfo sourceInfo = new SourceInfo();
                        sourceInfo.SourceId = reader.ReadContentAsInt();
                        if (reader.MoveToAttribute("SourceEntityId") && reader.HasValue && !String.IsNullOrWhiteSpace(reader.Value))
                        {
                            sourceInfo.SourceEntityId = reader.ReadContentAsLong();
                        }
                        this._sourceInfo = sourceInfo;
                    }
                    if (reader.MoveToAttribute("DisplayValue"))
                    {
                        this._displayVal = reader.ReadContentAsString();
                    }
                    if (reader.MoveToElement())
                    {
                        //AttrVal is represented as Element value. So we are reading current element's context as string and assign it to AttrVal.
                        this._attrVal = reader.ReadElementContentAsObject().ToString();
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("reader", "XmlTextReader is null. Can not read Value object.");
            }
        }

        /// <summary>
        /// Compares AttributeValue including UOM values.
        /// </summary>
        /// <param name="objectToBeCompared">The Object to compare with the current Object</param>
        /// <returns>true if the specified Value is equal to the current Object; otherwise, false.</returns>
        public Boolean IsEqualTo(Value objectToBeCompared)
        {
            if (objectToBeCompared != null)
            {
                if (this.InvariantVal != null)
                {
                    if (!this.InvariantVal.Equals(objectToBeCompared.InvariantVal))
                        return false;
                }
                if (this.UomId != objectToBeCompared.UomId)
                    return false;

                if (this.Uom != objectToBeCompared.Uom)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadValueForDataTransfer(String valuesAsXml)
        {
            #region Sample Xml

            //    <Value 
            //        Uom="" 
            //        ValRefId="-1" 
            //        Seq="1" 
            //        Locale="en_WW">
            //            <![CDATA[WEB]]>
            //    </Value>
            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Value")
                    {
                        //Read entity metadata
                        #region Read Entity Attributes

                        if (reader.HasAttributes)
                        {
                            //Using GetAttribute because we need to read element value. 
                            //"reader.MoveToAttribute" will move cursor forward and we won't be able to read element value.

                            if (reader.GetAttribute("Id") != null)
                            {
                                this.Id = ValueTypeHelper.Int64TryParse(reader.GetAttribute("Id"), this.Id);
                            }

                            if (reader.GetAttribute("UId") != null)
                            {
                                this.UomId = ValueTypeHelper.Int32TryParse(reader.GetAttribute("UId"), this.UomId);
                            }

                            if (reader.GetAttribute("U") != null)
                            {
                                this.Uom = reader.GetAttribute("U");
                            }

                            if (reader.GetAttribute("VRId") != null)
                            {
                                this.ValueRefId = ValueTypeHelper.Int32TryParse(reader.GetAttribute("VRId"), this.ValueRefId);
                            }

                            if (reader.GetAttribute("Seq") != null)
                            {
                                this.Sequence = ValueTypeHelper.DecimalTryParse(reader.GetAttribute("Seq"), this.Sequence);
                            }

                            if (reader.MoveToAttribute("L"))
                            {
                                String strLocale = reader.ReadContentAsString();
                                LocaleEnum locale = LocaleEnum.UnKnown;
                                Enum.TryParse<LocaleEnum>(strLocale, true, out locale);

                                this.Locale = locale;
                            }

                            if (reader.MoveToAttribute("A"))
                            {
                                this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("DispV"))
                            {
                                this._displayVal = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("HIVV"))
                            {
                                this.HasInvalidValue = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                            }

                            if (reader.GetAttribute("IV") != null)
                            {
                                this.InvariantVal = reader.ReadContentAsString();
                            }

                            if (reader.GetAttribute("DV") != null)
                            {
                                this.DateVal = ValueTypeHelper.ConvertToDateTime(reader.ReadContentAsString());
                            }

                            if (reader.GetAttribute("NV") != null)
                            {
                                this.NumericVal = ValueTypeHelper.ConvertToDecimal(reader.ReadContentAsString());
                            }

                            if (reader.GetAttribute("SV") != null)
                            {
                                this.StringVal = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("SourceId"))
                            {
                                SourceInfo sourceInfo = new SourceInfo();
                                sourceInfo.SourceId = reader.ReadContentAsInt();
                                if (reader.MoveToAttribute("SourceEntityId"))
                                {
                                    sourceInfo.SourceEntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                }
                                this.SourceInfo = sourceInfo;
                            }

                            if (reader.MoveToElement())
                            {
                                //AttrVal is represented as Element value. So we are reading current element's context as string and assign it to AttrVal.
                                this._attrVal = reader.ReadElementContentAsObject().ToString();
                            }
                        }

                        #endregion Read Entity Attributes
                    }

                    reader.Read();
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

        /// <summary>
        /// Remove Invalid Data
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private String CleanInvalidXmlChars(string text)
        {
            string pattern = @"[^\x09\x0A\x0D\u0020-\uD7FF\uE000-\uFFFD]";
            return Regex.Replace(text, pattern, String.Empty);
        }

        /// <summary>
        /// Writes multiple CData sections if text contains more than 1 CData section.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="xmlWriter"></param>
        private void WriteCData(string text, XmlWriter xmlWriter)
        {
            Int32 indexOfCEnd = text.IndexOf("]]>");

            if (indexOfCEnd > 0)
            {
                StringBuilder cdataWhole = new StringBuilder();

                var cdataParts = text.Split(new string[] { "]]>" }, StringSplitOptions.RemoveEmptyEntries);
                var numberOfParts = cdataParts.Count();
                int i = 0;

                for (; i < numberOfParts; i++)
                {
                    xmlWriter.WriteCData(cdataParts[i]);

                    if (i < numberOfParts - 1)
                    {
                        xmlWriter.WriteCData(">"); // There are more CDATA Parts, so end the add the >
                    }
                }
            }
            else
            {
                xmlWriter.WriteCData(text);
            }
        }
        #endregion

        #endregion
    }
}