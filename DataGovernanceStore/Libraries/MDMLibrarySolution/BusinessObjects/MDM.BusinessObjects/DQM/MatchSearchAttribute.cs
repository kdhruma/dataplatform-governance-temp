using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Core.Extensions;
    using MDM.Interfaces.DQM;

    /// <summary>
    /// This class holds properties and methods which are necessary for match search attribute
    /// </summary>
    [DataContract]
    public class MatchSearchAttribute : MatchAttribute, IMatchSearchAttribute 
    {
        #region Field

        /// <summary>
        /// Field indicates extension of match
        /// </summary>
        private String _extension = String.Empty;

        #endregion Field

        #region Constructor

        /// <summary>
        /// Constructor without parameter
        /// </summary>
        public MatchSearchAttribute()
        {

        }

        /// <summary>
        /// Constructor with XML having values of object. Populate current object using XML
        /// </summary>
        /// <param name="valueAsXml">Indicates string XML of match search attribute</param>
        public MatchSearchAttribute(String valueAsXml)
        {
            LoadMatchSearchAttribute(valueAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Property denotes extention of match
        /// </summary>
        [DataMember]
        public String Extension
        {
            get { return _extension; }
            set { _extension = value; }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets a cloned instance of the current match search attribute object
        /// </summary>
        /// <returns>Cloned instance of the current match search attribute object</returns>
        public MatchSearchAttribute Clone()
        {
            MatchSearchAttribute clonedObj = new MatchSearchAttribute();

            clonedObj.AttributeName = this.AttributeName;
            clonedObj.Locale = this.Locale;
            clonedObj.ExcludedValues = this.ExcludedValues;

            return clonedObj;
        }

        /// <summary>
        /// Converts match search attribute into xml string
        /// </summary>
        /// <returns>Returns string xml of match search attribute.</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    xmlWriter.WriteStartElement("MatchSearchAttribute"); //MatchSearchAttribute node start

                    #region write Match search attribute

                    xmlWriter.WriteAttributeString("Extension", this.Extension);
      
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

                    #endregion write Match search attribute

                    xmlWriter.WriteEndElement(); //MatchSearchAttribute node end
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
        public void LoadMatchSearchAttribute(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MatchSearchAttribute")
                        {
                            #region Read MatchSearchAttribute

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Extension"))
                                {
                                    this.Extension = reader.ReadContentAsString();
                                }
                            }
                            else
                            {
                                reader.Read();
                            }

                            #endregion Read MatchSearchAttribute
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
        }

        /// <summary>
        /// Converts MatchSearchAttribute into Json format
        /// </summary>
        /// <returns>Returns JObject of match search attribute</returns>
        public JObject ToJson()
        {
            String formattedAttributeString = String.Empty;

            if (!String.IsNullOrWhiteSpace(this.AttributeName))
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

            return new JObject(
                    new JProperty("paths",
                        new JArray(formattedAttributeString)
                        ),
                    new JProperty("extension", this.Extension),
                    new JProperty("excludedValues",
                        (JArray)JToken.FromObject(this.ExcludedValues)
                        )
                    );
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadExcludedValues(String valuesAsXml)
        {
            if (!String.IsNullOrEmpty(valuesAsXml))
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
        }

        #endregion Private Methods

        #endregion Methods
    }
}
