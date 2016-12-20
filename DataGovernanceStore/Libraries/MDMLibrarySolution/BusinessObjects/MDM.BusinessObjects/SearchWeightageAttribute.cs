using MDM.Core;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.BusinessObjects.Interfaces;

    /// <summary>
    /// Specify the SearchWeightage Object
    /// </summary>
    [DataContract]
    public class SearchWeightageAttribute : MDMObject, ISearchWeightageAttribute
    {

        #region Fields

        /// <summary>
        /// Field denotes attribute Id 
        /// </summary>
        private Int32 _attributeId = 0;

        /// <summary>
        /// Field denotes locale Id
        /// </summary>
        private Int32 _localeId = 0;

        /// <summary>
        /// Field denotes attribute Value
        /// </summary>
        private String _attributeValue = String.Empty;

        /// <summary>
        /// Field denotes Weightage for given attribute Value
        /// </summary>
        private Decimal _weightage = default(Decimal);

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denotes Attribute Id
        /// </summary>
        [DataMember]
        public Int32 AttributeId
        {
            get
            {
                return _attributeId;
            }
            set
            {
                _attributeId = value;
            }
        }

        /// <summary>
        /// Property denotes Locale Id
        /// </summary>
        [DataMember]
        public Int32 LocaleId
        {
            get
            {
                return _localeId;
            }
            set
            {
                _localeId = value;
            }
        }

        /// <summary>
        /// Property denotes Attribute Value
        /// </summary>
        [DataMember]
        public String AttributeValue
        {
            get
            {
                return _attributeValue;
            }
            set
            {
                _attributeValue = value;
            }
        }

        /// <summary>
        /// Property denotes Weightage for the attribute value
        /// </summary>
        [DataMember]
        public Decimal Weightage
        {
            get
            {
                return _weightage;
            }
            set
            {
                _weightage = value;
            }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="attributeId">attribute Id</param>
        /// <param name="localeId">locale Id</param>
        /// <param name="attributeValue">attribute Value</param>
        /// <param name="weightage">weightage</param>
        public SearchWeightageAttribute(Int32 attributeId, Int32 localeId, String attributeValue, decimal weightage)
        {
            _attributeId = attributeId;
            _localeId = localeId;
            _attributeValue = attributeValue;
            _weightage = weightage;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Xml representation of SearchWeightage object
        /// </summary>
        /// <returns></returns>
        public override String ToXml()
        {
            String searchWeightageAttributeXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("SearchWeightageAttribute");
            xmlWriter.WriteAttributeString("AttributeId", this.AttributeId.ToString());
            xmlWriter.WriteAttributeString("LocaleId", this.LocaleId.ToString());
            xmlWriter.WriteAttributeString("AttributeValue", this.AttributeValue);
            xmlWriter.WriteAttributeString("Weightage", this.Weightage.ToString());
            xmlWriter.WriteEndElement();
            xmlWriter.Flush();

            //Get the actual XML
            searchWeightageAttributeXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return searchWeightageAttributeXml;
        }

        #endregion Methods
    }
}
