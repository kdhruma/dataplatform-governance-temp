using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Lookup search
    /// </summary>
    [DataContract]
    public class LookupSearchRule : ILookupSearchRule
    {
        #region Fields

        /// <summary>
        /// unique identifier of the rule
        /// </summary>
        private Int32 _ruleId = 0;

        /// <summary>
        /// Column name on which search needs to be performed
        /// </summary>
        private String _searchColumnName = String.Empty;

        /// <summary>
        /// operator to search the value from a column
        /// </summary>
        private String _searchOperator = String.Empty;

        /// <summary>
        /// Operator to search lookup value from a column
        /// </summary>
        private LookupSearchOperatorEnum _lookupSearchOperator = LookupSearchOperatorEnum.None;

        /// <summary>
        /// value that needs to be searched from a column
        /// </summary>
        private String _searchValue = String.Empty;

        #endregion

        #region  Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public LookupSearchRule()
        { }

        /// <summary>
        /// Constructor with parameter
        /// </summary>
        /// <param name="valueAsXml">xml</param>
        public LookupSearchRule(String valueAsXml)
        {
            LoadLookupSearchRule(valueAsXml);
        }
        
        /// <summary>
        /// Parameterised Constructor
        /// </summary>
        /// <param name="ruleId">Indicates unique identifier for a  rule</param>
        /// <param name="searchColumnName">Indicates search column name</param>
        /// <param name="lookupSearchOperator">Indicates search operator to be used to search values on a column</param>
        /// <param name="searchValue">Indicates the value which needs to be searched</param>
        public LookupSearchRule(Int32 ruleId, String searchColumnName, LookupSearchOperatorEnum lookupSearchOperator, String searchValue)
        {
            this.RuleId = ruleId;
            this.SearchColumnName = searchColumnName;
            this.LookupSearchOperator = lookupSearchOperator;
            this.SearchValue = searchValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// unique identifier of the rule
        /// </summary>
        [DataMember]
        public Int32 RuleId
        {
            get { return _ruleId; }
            set { _ruleId = value; }
        }

        /// <summary>
        /// Column name on which search needs to be performed
        /// </summary>
        [DataMember]
        public String SearchColumnName
        {
            get { return _searchColumnName; }
            set { _searchColumnName = value; }
        }
        
        /// <summary>
        /// operator to search the value from a column
        /// </summary>
        [DataMember]
        public LookupSearchOperatorEnum LookupSearchOperator
        {
            get { return _lookupSearchOperator; }
            set { _lookupSearchOperator = value; }
        }

        /// <summary>
        /// value that needs to be searched from a column
        /// </summary>
        [DataMember]
        public String SearchValue
        {
            get { return _searchValue; }
            set { _searchValue = value; }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Xml representation of LookupSearchRule
        /// </summary>  
        /// <returns>Xml representation of object</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //LookupSearchRule node start
            xmlWriter.WriteStartElement("LookupSearchRule");

            #region Write Properties

            xmlWriter.WriteAttributeString("RuleId", this.RuleId.ToString());
            xmlWriter.WriteAttributeString("ColumnName", this.SearchColumnName.ToString());
            xmlWriter.WriteAttributeString("Operator", this.LookupSearchOperator.ToString());
            xmlWriter.WriteAttributeString("Value", this.SearchValue.ToString());

            #endregion

            // LookupSearchRule node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Load lookupSearhRule object from XML.
        /// </summary>
        /// <param name="valueAsXml">XML having xml value for an object</param>
        public void LoadLookupSearchRule(String valueAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valueAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name.ToLowerInvariant() == "LookupSearchRule")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("RuleId"))
                                {
                                    this.RuleId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(),this._ruleId);
                                }

                                if (reader.MoveToAttribute("ColumnName"))
                                {
                                    this.SearchColumnName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Operator"))
                                {
                                    this.LookupSearchOperator = ValueTypeHelper.ConvertToLookupSearchOperatorEnum(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("Value"))
                                {
                                    this.SearchValue = reader.ReadContentAsString();
                                }
                            }
                            else
                            {
                                reader.Read();
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
        #endregion

        #endregion Methods
    }
}
