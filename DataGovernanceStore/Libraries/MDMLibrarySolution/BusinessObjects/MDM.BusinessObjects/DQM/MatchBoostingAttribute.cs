using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQM
{
    using Core.Extensions;
    using Interfaces.DQM;
    using MDM.Core;

    /// <summary>
    /// This class holds properties and methods which are necessary for match boosting attribute
    /// </summary>
    [DataContract]
    [KnownType(typeof(MatchAttribute))]
    public class MatchBoostingAttribute : MatchAttribute, IMatchBoostingAttribute
    {
        #region Fields

        /// <summary>
        /// Fields indicates low match score of attribute
        /// </summary>
        private Double _low = 0.0;

        /// <summary>
        /// Fields indicates high match score of attribute
        /// </summary>
        private Double _high = 0.0;

        /// <summary>
        /// Field indicates jigsaw comparators for match
        /// </summary>
        private String _comparator = String.Empty;

        /// <summary>
        /// Fields indicates jigsaw comparators config for match
        /// </summary>
        private String _comparatorConfig = String.Empty;

        /// <summary>
        /// Field indicates jigsaw cleaner for match
        /// </summary>
        private String _cleaner = String.Empty;

        /// <summary>
        /// Field indicates jigsaw cleaner config for match
        /// </summary>
        private String _cleanerConfig = String.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor without parameter
        /// </summary>
        public MatchBoostingAttribute()
        {

        }

        /// <summary>
        /// Constructor with XML having values of object. Populate current object using XML
        /// </summary>
        /// <param name="valueAsXml">Indicates string XML of match boosting attribute</param>
        public MatchBoostingAttribute(String valueAsXml)
        {
            LoadMatchBoostingAttribute(valueAsXml);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Property denotes low match score of attribute
        /// </summary>
        [DataMember]
        public Double Low
        {
            get { return _low; }
            set { _low = value; }
        }

        /// <summary>
        /// Property denotes high match score of attribute
        /// </summary>
        [DataMember]
        public Double High
        {
            get { return _high; }
            set { _high = value; }
        }

        /// <summary>
        /// Property denotes jigsaw comparator for match
        /// </summary>
        [DataMember]
        public String Comparator
        {
            get { return _comparator; }
            set { _comparator = value; }
        }

        /// <summary>
        /// Property denotes jigsaw comparator config for match
        /// </summary>
        [DataMember]
        public String ComparatorConfig
        {
            get { return _comparatorConfig; }
            set { _comparatorConfig = value; }
        }

        /// <summary>
        /// Property denotes jigsaw cleaner for match
        /// </summary>
        [DataMember]
        public String Cleaner
        {
            get { return _cleaner; }
            set { _cleaner = value; }
        }

        /// <summary>
        /// Property denotes jigsaw cleaner config for match
        /// </summary>
        [DataMember]
        public String CleanerConfig
        {
            get { return _cleanerConfig; }
            set { _cleanerConfig = value; }
        }

        #endregion Properties

        #region Method

        #region Public Method

        /// <summary>
        /// Gets a cloned instance of the current match boosting attribute object
        /// </summary>
        /// <returns>Cloned instance of the current match boosting attribute object</returns>
        public MatchBoostingAttribute Clone()
        {
            MatchBoostingAttribute clonedObj = new MatchBoostingAttribute();

            clonedObj.AttributeName = this.AttributeName;
            clonedObj.Locale = this.Locale;
            clonedObj.High = this.High;
            clonedObj.Low = this.Low;
            clonedObj.ExcludedValues = this.ExcludedValues;
            clonedObj.Comparator = this.Comparator;
            clonedObj.Cleaner = this.Cleaner;
            clonedObj.ComparatorConfig = this.ComparatorConfig;
            clonedObj.CleanerConfig = this.CleanerConfig;

            return clonedObj;
        }

        /// <summary>
        /// Converts match boosting attribute into xml string
        /// </summary>
        /// <returns>Returns string xml of match boosting attribute.</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    xmlWriter.WriteStartElement("MatchBoostingAttribute"); //MatchBoostingAttribute node start

                    #region write Match boosting attribute

                    xmlWriter.WriteAttributeString("Low", this.Low.ToString());
                    xmlWriter.WriteAttributeString("High", this.High.ToString());
                    xmlWriter.WriteAttributeString("Comparator", this.Comparator);
                    xmlWriter.WriteAttributeString("Cleaner", this.Cleaner);

                    xmlWriter.WriteStartElement("Paths"); //Paths node start
                    xmlWriter.WriteAttributeString("AttributeName", this.AttributeName);
                    xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
                    xmlWriter.WriteEndElement(); //Paths node end

                    xmlWriter.WriteStartElement("ExcludedValues"); //Excluded values node start

                    foreach (String item in this.ExcludedValues)
                    {
                        xmlWriter.WriteStartElement("ExcludedValue"); //Excluded value node start
                        xmlWriter.WriteAttributeString("Value", item);
                        xmlWriter.WriteEndElement(); //Excluded value node end
                    }

                    xmlWriter.WriteEndElement(); //Excluded values node end

                    xmlWriter.WriteStartElement("ComparatorConfig");
                    xmlWriter.WriteRaw(String.Format("<![CDATA[{0}]]>", this.ComparatorConfig));
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("CleanerConfig");
                    xmlWriter.WriteRaw(String.Format("<![CDATA[{0}]]>", this.CleanerConfig));
                    xmlWriter.WriteEndElement();

                    #endregion write Match boosting attribute

                    xmlWriter.WriteEndElement(); //MatchBoostingAttribute node end
                    xmlWriter.Flush();

                    //get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Load match boosting attribute from xml
        /// </summary>
        /// <returns>Returns MatchBoostingAttribute</returns>
        public void LoadMatchBoostingAttribute(String valuesAsXml)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "MatchBoostingAttribute")
                    {
                        #region Read MatchBoostingAttribute

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("Low"))
                            {
                                this.Low = ValueTypeHelper.DoubleTryParse(reader.ReadContentAsString(), 0.0);
                            }

                            if (reader.MoveToAttribute("High"))
                            {
                                this.High = ValueTypeHelper.DoubleTryParse(reader.ReadContentAsString(), 0.0);
                            }

                            if (reader.MoveToAttribute("Comparator"))
                            {
                                this.Comparator = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("Cleaner"))
                            {
                                this.Cleaner = reader.ReadContentAsString();
                            }
                        }
                        else
                        {
                            reader.Read();
                        }

                        #endregion Read MatchBoostingAttribute
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Paths")
                    {
                        #region Read Paths

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("AttributeName"))
                            {
                                this.AttributeName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("Locale"))
                            {
                                LocaleEnum locale = LocaleEnum.UnKnown;
                                Enum.TryParse(reader.ReadContentAsString(), out locale);
                                this.Locale = locale;
                            }
                        }
                        else
                        {
                            reader.Read();
                        }

                        #endregion Read Paths
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExcludedValues")
                    {
                        #region Read ExcludedValues

                        LoadExcludedValues(reader.ReadOuterXml());

                        #endregion Read ExcludedValues
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ComparatorConfig")
                    {
                        #region Read Comparator Config

                        this.ComparatorConfig = reader.ReadElementContentAsString().Trim();

                        #endregion Read Comparator Config
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "CleanerConfig")
                    {
                        #region Read Cleaner Config

                        this.CleanerConfig = reader.ReadElementContentAsString().Trim();

                        #endregion Read Cleaner Config
                    }
                    else
                    {
                        //Keep on reading the xml until we reach expected node.
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

        /// <summary>
        /// Convert match boosting attribute into Json format
        /// </summary>
        /// <returns>Returns JObject of MatchBoostingAttribute</returns>
        public JObject ToJson()
        {
            String formattedAttributeString = String.Empty;

            if (!String.IsNullOrWhiteSpace(this.AttributeName))
            {
                if (String.Compare(this.AttributeName, "*") == 0)
                {
                    formattedAttributeString = this.AttributeName;
                }
                else
                {
                    String[] attributeSplitString = this.AttributeName.Split(new Char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    if (attributeSplitString.Length > 1)
                    {
                        formattedAttributeString = String.Format("attributesInfo.{0}.{1}.{2}", this.Locale.GetCultureName(), attributeSplitString[0].ToJsCompliant(), attributeSplitString[1].ToJsCompliant());
                    }
                    else
                    {
                        formattedAttributeString = String.Format("attributesInfo.{0}.{1}", this.Locale.GetCultureName(), this.AttributeName.ToJsCompliant());
                    }
                }
            }

            JObject boostingAttributeJson = new JObject(
                    new JProperty("paths",
                        new JArray(formattedAttributeString)
                    ),
                    new JProperty("excludeValues", 
                        (JArray)JToken.FromObject(this.ExcludedValues)
                    ),
                    new JProperty("low", this.Low),
                    new JProperty("high", this.High),
                    new JProperty("comparator", this.Comparator),
                    new JProperty("cleaner", this.Cleaner)
                );

            if (!String.IsNullOrWhiteSpace(this.ComparatorConfig))
            {
                boostingAttributeJson.Add(new JProperty("comparatorConfig", JObject.Parse(this.ComparatorConfig)));
            }
            if (!String.IsNullOrWhiteSpace(this.CleanerConfig))
            {
                boostingAttributeJson.Add(new JProperty("cleanerConfig", JObject.Parse(this.CleanerConfig)));
            }

            return boostingAttributeJson;
        }

        #endregion Public Method

        #region Private Method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml">Indicates string xml of match boosting attribute</param>

        private void LoadExcludedValues(String valuesAsXml)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExcludedValue")
                    {
                        #region Read ExcludedValue

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("Value"))
                            {
                                this.ExcludedValues.Add(reader.ReadContentAsString());
                            }
                        }
                        else
                        {
                            reader.Read();
                        }

                        #endregion Read ExcludedValue
                    }
                    else
                    {
                        //Keep on reading the xml until we reach expected node.
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

        #endregion Private Method

        #endregion Method
    }
}
