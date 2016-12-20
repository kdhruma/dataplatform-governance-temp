using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Interfaces.DQM;

    /// <summary>
    /// This class holds properites and methods which are necessary for match event
    /// </summary>
    [DataContract]
    public class MatchEvent : IMatchEvent
    {
        #region Fields

        /// <summary>
        /// Field indicates external identifier fields
        /// </summary>
        private Collection<String> _externalIdFields = new Collection<String>();

        /// <summary>
        /// Field indicates extra relation fields
        /// </summary>
        private Collection<String> _extraRelationFields = new Collection<String>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor without parameter
        /// </summary>
        public MatchEvent()
        {

        }

        /// <summary>
        /// Constructor with XML having values of object. Populate current object using XML
        /// </summary>
        /// <param name="valueAsXml">Indicates string XML of match event</param>
        public MatchEvent(String valueAsXml)
        {
            LoadMatchEvent(valueAsXml);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Property denotes external identifier fields
        /// </summary>
        [DataMember]
        public Collection<String> ExternalIdFields
        {
            get { return _externalIdFields; }
            set { _externalIdFields = value; }
        }

        /// <summary>
        /// Property denotes extra relation fields
        /// </summary>
        [DataMember]
        public Collection<String> ExtraRelationFields
        {
            get { return _extraRelationFields; }
            set { _extraRelationFields = value; }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets a cloned instance of the current match event object
        /// </summary>
        /// <returns>Cloned instance of the current match event object</returns>
        public MatchEvent Clone()
        {
            MatchEvent clonedObj = new MatchEvent();

            clonedObj.ExternalIdFields = this.ExternalIdFields;
            clonedObj.ExtraRelationFields = this.ExtraRelationFields;

            return clonedObj;
        }

        /// <summary>
        /// Converts match event into xml string
        /// </summary>
        /// <returns>Returns string xml of match event.</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    xmlWriter.WriteStartElement("MatchEvent"); //MatchEvent node start

                    #region write Match Event

                    xmlWriter.WriteStartElement("ExternalIdFields"); //ExternalIdFields node start

                    foreach (String item in this.ExternalIdFields)
                    {
                        xmlWriter.WriteStartElement("ExternalIdField"); //ExternalIdField node start
                        xmlWriter.WriteAttributeString("Value", item);
                        xmlWriter.WriteEndElement(); //ExternalIdField node end
                    }

                    xmlWriter.WriteEndElement(); //ExternalIdFields node end

                    xmlWriter.WriteStartElement("ExtraRelationFields"); //ExtraRelationFields node start

                    foreach (String item in this.ExtraRelationFields)
                    {
                        xmlWriter.WriteStartElement("ExtraRelationField"); //ExtraRelationField node start
                        xmlWriter.WriteAttributeString("Value", item);
                        xmlWriter.WriteEndElement(); //ExtraRelationField node end
                    }

                    xmlWriter.WriteEndElement(); //ExtraRelationFields node end

                    #endregion write Match Event

                    xmlWriter.WriteEndElement(); //MatchEvent node end
                    xmlWriter.Flush();

                    //get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Load match event from xml
        /// </summary>
        /// <param name="valuesAsXml">Indicates string xml of match event</param>
        public void LoadMatchEvent(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExternalIdFields")
                        {
                            #region Read ExcludedIdFields

                            LoadExternalIdFields(reader.ReadOuterXml());

                            #endregion Read ExcludedIdFields
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExtraRelationFields")
                        {
                            #region Read ExtraRelationFields

                            LoadExtraRelationFields(reader.ReadOuterXml());

                            #endregion Read ExtraRelationFields
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
        /// Convert match event into Json format
        /// </summary>
        /// <returns>Returns JObject of match event</returns>
        public JObject ToJson()
        {
            return new JObject(
                    new JProperty("externalIdFields",
                        (JArray)JToken.FromObject(this.ExternalIdFields)
                    ),
                    new JProperty("extraRelationFields",
                        (JArray)JToken.FromObject(this.ExtraRelationFields)
                    )
                );
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadExternalIdFields(String valuesAsXml)
        {
            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExternalIdField")
                        {
                            #region Read ExcludedValue

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Value"))
                                {
                                    this.ExternalIdFields.Add(reader.ReadContentAsString());
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadExtraRelationFields(String valuesAsXml)
        {
            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExtraRelationField")
                        {
                            #region Read ExcludedValue

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Value"))
                                {
                                    this.ExtraRelationFields.Add(reader.ReadContentAsString());
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
