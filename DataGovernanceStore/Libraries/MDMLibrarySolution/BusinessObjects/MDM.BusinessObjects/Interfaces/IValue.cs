using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDM.BusinessObjects;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get value related information.
    /// </summary>
    public interface IValue : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting the value of an Attribute
        /// </summary>
        Object AttrVal { get; set; }

        /// <summary>
        /// Property denoting UOM of attribute value
        /// </summary>
        String Uom { get; set; }

        /// <summary>
        /// Property denoting the record id of referred table for attribute value
        /// </summary>
        Int32 ValueRefId { get; set; }

        /// <summary>
        /// Property denoting the sequence number of attribute value
        /// </summary>
        Decimal Sequence { get; set; }

        /// <summary>
        /// Indicates the Action of an object
        /// </summary>
        new ObjectAction Action { get; set; }

        /// <summary>
        /// Indicates the Id of an object
        /// </summary>
        new Int64 Id { get; set; }

        /// <summary>
        /// Indicates if given value is invalid with respect to all the basic validations.
        /// </summary>
        Boolean HasInvalidValue { get; set; }

        /// <summary>
        /// SourceInfo of changes for value
        /// </summary>
        SourceInfo SourceInfo { get; set; }

        #endregion

        #region Methods

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of Value object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Value object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <param name="attributeDataType">Specifies data type of an value being serialized</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization, AttributeDataType attributeDataType);

        #endregion ToXml methods

        #region Get values in specific type

        /// <summary>
        /// Gets String value of current attribute value
        /// </summary>
        /// <returns>String representation of current value</returns>
        /// <exception cref="Exception">AttrVal is null. Cannot get String value</exception>
        String GetStringValue();

        /// <summary>
        /// Gets Numeric value of current attribute value
        /// </summary>
        /// <returns>String representation of current value</returns>
        /// <exception cref="Exception">AttrVal is null. Cannot get Numeric value</exception>
        Nullable<Decimal> GetNumericValue();

        /// <summary>
        /// Gets Date value of current attribute value
        /// </summary>
        /// <returns>String representation of current value</returns>
        /// <exception cref="Exception">AttrVal is null. Cannot get Date value</exception>
        Nullable<DateTime> GetDateTimeValue();

        /// <summary>
        /// Gets current attribute display value
        /// </summary>
        /// <returns>String representation of display value</returns>
        String GetDisplayValue();

        /// <summary>
        /// Sets current attribute display value
        /// </summary>
        /// <param name="displayVal">Display Value</param>
        void SetDisplayValue(String displayVal);

        /// <summary>
        /// Gets current attribute export value
        /// </summary>
        /// <returns>String representation of export value</returns>
        String GetExportValue();

        /// <summary>
        /// Sets current attribute export value
        /// </summary>
        /// <param name="exportval">Export Value</param>
        void SetExportValue(String exportval);

        #endregion Get values in specific type

        /// <summary>
        /// Set Blank Value
        /// </summary>
        void SetBlank();

        /// <summary>
        /// Clear all the properties for current value.
        /// </summary>
        void Clear();

        /// <summary>
        /// Compare the Object
        /// </summary>
        /// <param name="obj">Value Object</param>
        /// <returns>True if Succesful</returns>
        bool ValueEquals(IValue obj);

        /// <summary>
        /// Compare the Object
        /// </summary>
        /// <param name="obj">Value Object</param>
        /// <param name="stringComparison">String comparison options</param>
        /// <returns>True if Succesful</returns>
        bool ValueEquals(IValue obj, StringComparison stringComparison);

        #endregion

    }
}
