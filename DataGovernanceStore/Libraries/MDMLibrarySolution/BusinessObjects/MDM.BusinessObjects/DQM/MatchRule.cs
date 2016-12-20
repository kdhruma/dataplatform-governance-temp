using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Interfaces.DQM;

    /// <summary>
    /// This class holds properties or methods which are necessary for match rule
    /// </summary>
    [DataContract]
    public class MatchRule : IMatchRule
    {

        #region Fields

        /// <summary>
        /// Field indicates whether to do use and template or not
        /// </summary>
        private Boolean _useAndTemplate = false;

        /// <summary>
        /// Field indicates threshold
        /// </summary>
        private Double _threshold = 0.0;

        /// <summary>
        /// Field indicates may be threshold
        /// </summary>
        private Double _mayBeThreshold = 0.0;

        /// <summary>
        /// Filed indicates collection of match search atttribute
        /// </summary>
        private MatchSearchAttributeCollection _matchSearchAttributes = new MatchSearchAttributeCollection();

        /// <summary>
        /// Field indicates collection of match boosting attribute
        /// </summary>
        private MatchBoostingAttributeCollection _matchBoostingAttributes = new MatchBoostingAttributeCollection();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denotes whether to do use and template or not
        /// </summary>
        [DataMember]
        public Boolean UseAndTemplate
        {
            get { return _useAndTemplate; }
            set { _useAndTemplate = value; }
        }

        /// <summary>
        /// Property denotes threshold
        /// </summary>
        [DataMember]
        public Double Threshold
        {
            get { return _threshold; }
            set { _threshold = value; }
        }

        /// <summary>
        /// Property denotes may be threshold
        /// </summary>
        [DataMember]
        public Double MayBeThreshold
        {
            get { return _mayBeThreshold; }
            set { _mayBeThreshold = value; }
        }

        /// <summary>
        /// Property denotes collection of match search attribute
        /// </summary>
        [DataMember]
        public MatchSearchAttributeCollection MatchSearchAttributes
        {
            get { return _matchSearchAttributes; }
            set { _matchSearchAttributes = value; }
        }

        /// <summary>
        /// Property denotes collection of match boosting attribute
        /// </summary>
        [DataMember]
        public MatchBoostingAttributeCollection MatchBoostingAttributes
        {
            get { return _matchBoostingAttributes; }
            set { _matchBoostingAttributes = value; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Constructor without parameter
        /// </summary>
        public MatchRule()
        {

        }

        /// <summary>
        /// Constructor with XML having values of object. Populate current object using XML
        /// </summary>
        /// <param name="valueAsXml">Indicates string XML of match rule</param>
        public MatchRule(String valueAsXml)
        {
            LoadMatchRule(valueAsXml);
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets a cloned instance of the current match rule object
        /// </summary>
        /// <returns>Cloned instance of the current match rule object</returns>
        public MatchRule Clone()
        {
            MatchRule clonedObj = new MatchRule();

            clonedObj.UseAndTemplate = this.UseAndTemplate;
            clonedObj.Threshold = this.Threshold;
            clonedObj.MayBeThreshold = this.MayBeThreshold;
            clonedObj.MatchBoostingAttributes = this.MatchBoostingAttributes.Clone();
            clonedObj.MatchSearchAttributes = this.MatchSearchAttributes.Clone();

            return clonedObj;
        }

        /// <summary>
        /// Converts match rule into xml string
        /// </summary>
        /// <returns>Returns string xml of match rule.</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    xmlWriter.WriteStartElement("MatchRule"); //MatchRule node start

                    #region write Match search attribute

                    xmlWriter.WriteAttributeString("UseAndTemplate", this.UseAndTemplate.ToString());
                    xmlWriter.WriteAttributeString("Threshold", this.Threshold.ToString());
                    xmlWriter.WriteAttributeString("MayBeThreshold", this.MayBeThreshold.ToString());

                    xmlWriter.WriteRaw(this.MatchSearchAttributes.ToXml());
                    xmlWriter.WriteRaw(this.MatchBoostingAttributes.ToXml());

                    #endregion write Match search attribute

                    xmlWriter.WriteEndElement(); //MatchRule node end
                    xmlWriter.Flush();

                    //get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Load match search attribute from xml
        /// </summary>
        /// <param name="valuesAsXml">Indicates string xml of match search attribute</param>
        public void LoadMatchRule(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MatchRule")
                        {
                            if(reader.HasAttributes)
                            {
                                if(reader.MoveToAttribute("UseAndTemplate"))
                                {
                                    this.UseAndTemplate = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }

                                if(reader.MoveToAttribute("Threshold"))
                                {
                                    this.Threshold = ValueTypeHelper.DoubleTryParse(reader.ReadContentAsString(), 0.0);
                                }

                                if (reader.MoveToAttribute("MayBeThreshold"))
                                {
                                    this.MayBeThreshold = ValueTypeHelper.DoubleTryParse(reader.ReadContentAsString(), 0.0);
                                }
                            }
                            else
                            {
                                reader.Read();
                            }
                        }
                        else if(reader.NodeType == XmlNodeType.Element && reader.Name == "MatchSearchAttributes")
                        {
                            this.MatchSearchAttributes = new MatchSearchAttributeCollection(reader.ReadOuterXml());
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "MatchBoostingAttributes")
                        {
                            this.MatchBoostingAttributes = new MatchBoostingAttributeCollection(reader.ReadOuterXml());
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

        /// <summary>
        /// Convert match rule into Json format
        /// </summary>
        /// <returns></returns>
        public JObject ToJson()
        {
            return new JObject(
                    new JProperty("useAndTemplate", this.UseAndTemplate),
                    new JProperty("threshold", this.Threshold),
                    new JProperty("maybeThreshold", this.MayBeThreshold),
                    new JProperty("searchAttributes", this.MatchSearchAttributes.ToJson()),
                    new JProperty("boostingAttributes", this.MatchBoostingAttributes.ToJson())
                );
        }

        #endregion Public Methods

        #endregion Methods
    }
}