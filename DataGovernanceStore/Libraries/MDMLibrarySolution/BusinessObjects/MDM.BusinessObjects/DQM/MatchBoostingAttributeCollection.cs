using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQM
{
    using MDM.BusinessObjects.Interfaces.DQM;
    using MDM.Interfaces.DQM;

    /// <summary>
    /// This class holds properties or methods which are necessary for match boosting attribute collection
    /// </summary>
    [DataContract]
    public class MatchBoostingAttributeCollection : InterfaceContractCollection<IMatchBoostingAttribute, MatchBoostingAttribute>, IMatchBoostingAttributeCollection
    {

        #region Fields
        
        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor without parameter
        /// </summary>
        public MatchBoostingAttributeCollection()
        {

        }

        /// <summary>
        /// Constructor with XML having values of object. Populate current object using XML
        /// </summary>
        /// <param name="valueAsXml">Indicates string XML of match boosting attribute collection</param>
        public MatchBoostingAttributeCollection(String valueAsXml)
        {
            LoadMatchBoostingAttributes(valueAsXml);
        }

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets a cloned instance of the current match boosting attribute collection object
        /// </summary>
        /// <returns>Cloned instance of the current match boosting attribute collection object</returns>
        public MatchBoostingAttributeCollection Clone()
        {
            MatchBoostingAttributeCollection clonedObj = new MatchBoostingAttributeCollection();

            if(this.Count > 0)
            {
                foreach(MatchBoostingAttribute item in this)
                {
                    clonedObj.Add(item.Clone());
                }
            }

            return clonedObj;
        }

        /// <summary>
        /// Converts match boosting attribute collection into xml string
        /// </summary>
        /// <returns>Returns string xml of match boosting attribute collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    xmlWriter.WriteStartElement("MatchBoostingAttributes"); //MatchSearchAttributes node start

                    #region write Match search attributes

                    foreach (MatchBoostingAttribute item in this)
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
        /// Load match boosting attributes from xml
        /// </summary>
        /// <param name="valuesAsXml">Indicates string xml of match boosting attributes</param>
        public void LoadMatchBoostingAttributes(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MatchBoostingAttribute")
                        {
                            String matchSearchAttributeXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(matchSearchAttributeXml))
                            {
                                MatchBoostingAttribute matchBoostingAttribute = new MatchBoostingAttribute(matchSearchAttributeXml);
                                if (matchBoostingAttribute != null)
                                {
                                    this.Add(matchBoostingAttribute);
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
        /// Convert match boosting attribute collection into Json format
        /// </summary>
        /// <returns>Returnds JObject of match boosting attribute collection</returns>
        public JArray ToJson()
        {
            JArray jarray = new JArray();

            foreach (MatchBoostingAttribute item in this)
            {
                jarray.Add(item.ToJson());
            }

            return jarray;
        }

        #endregion Public Methods

        #endregion Methods
    }
}
