using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies DataQualityClass
    /// </summary>
    [DataContract]
    public class DataQualityClass : MDMObject, IDataQualityClass
    {
        #region Constants

        private const String ClassName = "DataQualityClass";

        #endregion

        #region Fields

        /// <summary>
        /// Field for the Id of an DataQualityClass
        /// </summary>
        private Int16 _id = -1;

        /// <summary>
        /// Field for ValueSegment
        /// </summary>
        private ValueSegment _valueSegment = new ValueSegment();

        /// <summary>
        /// Field for DataQualityIndicatorSummaryTableColumnName
        /// </summary>
        private String _dataQualityIndicatorSummaryTableColumnName = null;

        /// <summary>
        /// Field for FillColor
        /// </summary>
        private Color? _fillColor = null;

        /// <summary>
        /// Field for SortOrder
        /// </summary>
        private Int32? _sortOrder = null;

        /// <summary>
        /// Field for Localization Message Code
        /// </summary>
        private String _localizationMessageCode = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public DataQualityClass()
            :base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="dataQualityClassId">Indicates the Identity of DataQualityClass</param>        
        public DataQualityClass(Int16 dataQualityClassId)
            : base(0, String.Empty, String.Empty)
        {
            this.Id = dataQualityClassId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property for the Id of DataQualityClass
        /// </summary>
        [DataMember]
        public new Int16 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Property for ValueSegment
        /// </summary>
        [DataMember]
        public ValueSegment ValueSegment
        {
            get { return this._valueSegment; }
            set { this._valueSegment = value; }
        }

        /// <summary>
        /// Property for DataQualityIndicatorSummaryTableColumnName
        /// </summary>
        [DataMember]
        public String DataQualityIndicatorSummaryTableColumnName
        {
            get { return this._dataQualityIndicatorSummaryTableColumnName; }
            set { this._dataQualityIndicatorSummaryTableColumnName = value; }
        }

        /// <summary>
        /// Property for FillColor
        /// </summary>
        [DataMember]
        public Color? FillColor
        {
            get { return this._fillColor; }
            set { this._fillColor = value; }
        }

        /// <summary>
        /// Property for SortOrder
        /// </summary>
        [DataMember]
        public Int32? SortOrder
        {
            get { return this._sortOrder; }
            set { this._sortOrder = value; }
        }

        /// <summary>
        /// Property for Localization Message Code
        /// </summary>
        [DataMember]
        public String LocalizationMessageCode
        {
            get { return this._localizationMessageCode; }
            set { this._localizationMessageCode = value; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Create a new data quality class
        /// </summary>
        /// <returns>Returns data quality class object</returns>
        public object Clone()
        {
            DataQualityClass dataQualityClass = (DataQualityClass)this.MemberwiseClone();
            dataQualityClass.ValueSegment = new ValueSegment()
                {
                    LeftBound = this.ValueSegment.LeftBound,
                    LeftBoundOperator = this.ValueSegment.LeftBoundOperator,
                    RightBound = this.ValueSegment.RightBound,
                    RightBoundOperator = this.ValueSegment.RightBoundOperator
                };                    
            return dataQualityClass;
        }

        /// <summary>
        /// Determines whether two Object instances are equal
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false</returns>
        public override Boolean Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is DataQualityClass)
                {
                    DataQualityClass objectToBeCompared = obj as DataQualityClass;
                    return
                        this.Id == objectToBeCompared.Id &&
                        this.ValueSegment.Equals(objectToBeCompared.ValueSegment) &&
                        this.DataQualityIndicatorSummaryTableColumnName == objectToBeCompared.DataQualityIndicatorSummaryTableColumnName &&
                        this.FillColor == objectToBeCompared.FillColor &&
                        this.SortOrder == objectToBeCompared.SortOrder &&
                        this.LocalizationMessageCode == objectToBeCompared.LocalizationMessageCode;
                }
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object</returns>
        public override Int32 GetHashCode()
        {
            return
                base.GetHashCode()
                ^ this.Id.GetHashCode()
                ^ this.ValueSegment.GetHashCode()
                ^ ((this.DataQualityIndicatorSummaryTableColumnName != null) ? this.DataQualityIndicatorSummaryTableColumnName.GetHashCode() : 0)
                ^ this.FillColor.GetHashCode()
                ^ this.SortOrder.GetHashCode()
                ^ this.LocalizationMessageCode.GetHashCode();
        }

        #region Xml Serialization

        /// <summary>
        /// Gets Xml representation of current object
        /// </summary>
        /// <returns>Xml representation of current object as string</returns>
        public override String ToXml()
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            xmlWriter.WriteStartElement(ClassName);

			xmlWriter.WriteAttributeString("Id", Id.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteAttributeString("Name", Name);
            xmlWriter.WriteAttributeString("LongName", LongName);
			xmlWriter.WriteAttributeString("DataQualityIndicatorSummaryTableColumnName", DataQualityIndicatorSummaryTableColumnName);
			xmlWriter.WriteAttributeString("SortOrder", SortOrder.ToString());
            xmlWriter.WriteAttributeString("LocalizationMessageCode", LocalizationMessageCode);
            xmlWriter.WriteAttributeString("FillColor", FillColor.HasValue ? ColorTranslator.ToHtml(FillColor.Value) : String.Empty);

            xmlWriter.WriteRaw(ValueSegment.ToXml());

            //Information node end
            xmlWriter.WriteEndElement();
            xmlWriter.Flush();
            //Get the actual XML
            xml = sw.ToString();
            xmlWriter.Close();
            sw.Close();
            return xml;
        }

        /// <summary>
        /// Loads current object from provided XmlNode
        /// </summary>
        /// <param name="node">XmlNode for deserialization</param>
        public void LoadFromXml(XmlNode node)
        {
            if (node == null)
            {
                return;
            }
            if (node.Attributes != null)
            {
                if (node.Attributes["Id"] != null)
                {
                    Id = ValueTypeHelper.Int16TryParse(node.Attributes["Id"].Value, Id); 
                }
                if (node.Attributes["Name"] != null)
                {
                    Name = node.Attributes["Name"].Value;
                }
                if (node.Attributes["LongName"] != null)
                {
                    LongName = node.Attributes["LongName"].Value;
                }
                if (node.Attributes["DataQualityIndicatorSummaryTableColumnName"] != null)
                {
                    DataQualityIndicatorSummaryTableColumnName = node.Attributes["DataQualityIndicatorSummaryTableColumnName"].Value;
                }
                if (node.Attributes["FillColor"] != null)
                {
                    FillColor = ColorTranslator.FromHtml(node.Attributes["FillColor"].Value);
                }
                if (node.Attributes["SortOrder"] != null)
                {
                    SortOrder = ValueTypeHelper.ConvertToNullableInt32(node.Attributes["SortOrder"].Value);
                }
                if (node.Attributes["LocalizationMessageCode"] != null)
                {
                    LocalizationMessageCode = node.Attributes["LocalizationMessageCode"].Value;
                }
            }

            ValueSegment.LoadFromXml(node.SelectSingleNode("ValueSegment"));
        }

        /// <summary>
        /// Loads current object from provided XmlNode
        /// </summary>
        /// <param name="xmlWithOuterNode">Xml string for deserialization</param>
        public void LoadFromXmlWithOuterNode(String xmlWithOuterNode)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlWithOuterNode);
            XmlNode node = doc.SelectSingleNode(ClassName);
            if (node != null)
            {
                LoadFromXml(node);
            }
        }

        #endregion

        #endregion
    }
}