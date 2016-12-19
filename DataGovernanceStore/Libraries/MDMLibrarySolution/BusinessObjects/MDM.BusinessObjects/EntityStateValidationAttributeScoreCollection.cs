using ProtoBuf;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies collection of entity state validation attribute score
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class EntityStateValidationAttributeScoreCollection : InterfaceContractCollection<IEntityStateValidationAttributeScore, EntityStateValidationAttributeScore>, IEntityStateValidationAttributeScoreCollection
    {
        #region Fields
        #endregion Fields

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public EntityStateValidationAttributeScoreCollection()
        {

        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">Indicates XMl representation of entity validation state attribute score collection</param>
        public EntityStateValidationAttributeScoreCollection(String valuesAsXml)
        {
            LoadEntityStateValidationAttributeScoreCollection(valuesAsXml);
        }

        #endregion Constructors

        #region Properties
        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets Xml representation of the EntityStateValidationAttributeScoreCollection object
        /// </summary>
        /// <returns>Returns Xml string representing the EntityStateValidationAttributeScoreCollection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    #region Write EntityStateValidationAttributeScoreCollection

                    //EntityStateValidationAttributeScoreCollection node start
                    xmlWriter.WriteStartElement("EntityStateValidationAttributeScores");

                    if (this._items != null)
                    {
                        foreach (EntityStateValidationAttributeScore entityStateValidationAttributeScore in this._items)
                        {
                            xmlWriter.WriteRaw(entityStateValidationAttributeScore.ToXml());
                        }
                    }

                    //EntityStateValidationAttributeScoreCollection node end
                    xmlWriter.WriteEndElement();

                    #endregion

                    xmlWriter.Flush();

                    //Get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadEntityStateValidationAttributeScoreCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityStateValidationAttributeScores")
                    {
                        String eVSattrScoreXml = reader.ReadOuterXml();

                        if (!String.IsNullOrWhiteSpace(eVSattrScoreXml))
                        {
                            EntityStateValidationAttributeScore eVSattrScore = new EntityStateValidationAttributeScore(eVSattrScoreXml);
                            this.Add(eVSattrScore);
                        }
                    }
                    else
                    {
                        //Keep on reading the xml until we reach expected node.
                        reader.Read();
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
