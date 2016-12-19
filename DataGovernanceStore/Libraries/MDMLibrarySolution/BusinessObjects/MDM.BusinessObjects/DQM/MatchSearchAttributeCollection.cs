using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Interfaces.DQM;

    /// <summary>
    /// This class holds properties or methods which are necessary for match search attribute collection
    /// </summary>
    [DataContract]
    public class MatchSearchAttributeCollection : InterfaceContractCollection<IMatchSearchAttribute, MatchSearchAttribute>, IMatchSearchAttributeCollection
    {
        #region Fields

        #endregion Fields

        #region Properties

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Constructor without parameter
        /// </summary>
        public MatchSearchAttributeCollection()
        {

        }

        /// <summary>
        /// Constructor with XML having values of object. Populate current object using XML
        /// </summary>
        /// <param name="valueAsXml">Indicates string XML of match search attribute collection</param>
        public MatchSearchAttributeCollection(String valueAsXml)
        {
            LoadMatchSearchAttributes(valueAsXml);
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets a cloned instance of the current match search attribute collection object
        /// </summary>
        /// <returns>Cloned instance of the current match search attribute collection object</returns>
        public MatchSearchAttributeCollection Clone()
        {
            MatchSearchAttributeCollection clonedObj = new MatchSearchAttributeCollection();

            if (this.Count > 0)
            {
                foreach (MatchSearchAttribute item in this)
                {
                    clonedObj.Add(item.Clone());
                }
            }

            return clonedObj;
        }

        /// <summary>
        /// Converts match search attribute collection into xml string
        /// </summary>
        /// <returns>Returns string xml of match search attribute collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    xmlWriter.WriteStartElement("MatchSearchAttributes"); //MatchSearchAttributes node start

                    #region write Match search attributes

                    foreach(MatchSearchAttribute item in this._items)
                    {
                        xmlWriter.WriteRaw(item.ToXml());
                    }

                    #endregion write Match search attributes

                    xmlWriter.WriteEndElement(); //MatchSearchAttributes node end
                    xmlWriter.Flush();

                    //get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Load match search attributes from xml
        /// </summary>
        /// <param name="valuesAsXml">Indicates string xml of match search attributes</param>
        public void LoadMatchSearchAttributes(String valuesAsXml)
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
                            String matchSearchAttributeXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(matchSearchAttributeXml))
                            {
                                MatchSearchAttribute matchSearchAttribute = new MatchSearchAttribute(matchSearchAttributeXml);
                                if (matchSearchAttribute != null)
                                {
                                    this.Add(matchSearchAttribute);
                                }
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

        /// <summary>
        /// Convert match search attribute collection into Json format
        /// </summary>
        /// <returns>Returns JObject of match search attibute collection</returns>
        public JArray ToJson()
        {
            JArray jarray = new JArray();

            foreach (MatchSearchAttribute item in this)
            {
                jarray.Add(item.ToJson());
            }

            return jarray;
        }

        #endregion Public Methods

        #endregion Methods
    }
}
