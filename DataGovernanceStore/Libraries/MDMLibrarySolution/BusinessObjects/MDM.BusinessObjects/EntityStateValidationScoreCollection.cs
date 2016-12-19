using ProtoBuf;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Specifies collection of entity validation score
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class EntityStateValidationScoreCollection : InterfaceContractCollection<IEntityStateValidationScore, EntityStateValidationScore>, IEntityStateValidationScoreCollection
    {
        #region Fields
        #endregion Fields

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public EntityStateValidationScoreCollection()
        {

        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">Indicates XMl representation of entity validation score collection</param>
        public EntityStateValidationScoreCollection(String valuesAsXml)
        {
            LoadEntityStateValidationScoreCollection(valuesAsXml);
        }

         /// <summary>
        /// Converts list into current instance
        /// </summary>
        /// <param name="entityStateValidationScoreList">List of Entity State Validation</param>
        public EntityStateValidationScoreCollection(IList<EntityStateValidationScore> entityStateValidationScoreList)
        {
            if (entityStateValidationScoreList!= null)
            {
                this._items = new Collection<EntityStateValidationScore>(entityStateValidationScoreList);
            }
        }

        #endregion Constructors

        #region Properties
        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets Xml representation of the EntityStateValidationScoreCollection object
        /// </summary>
        /// <returns>Returns Xml string representing the EntityStateValidationScoreCollection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    #region Write EntityStateValidationScoreCollection

                    //EntityStateValidationScoreCollection node start
                    xmlWriter.WriteStartElement("EntityStateValidationScores");

                    if (_items != null)
                    {
                        foreach (EntityStateValidationScore entityStateValidationScore in this._items)
                        {
                            xmlWriter.WriteRaw(entityStateValidationScore.ToXml());
                        }
                    }

                    //EntityStateValidationScoreCollection node end
                    xmlWriter.WriteEndElement();

                    #endregion

                    xmlWriter.Flush();

                    //Get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Get entity validation score for specified entity id
        /// </summary>
        /// <param name="entityId">Indicates entity id</param>
        /// <returns>Returns entity validation score</returns>
        public IEntityStateValidationScore GetByEntityId(Int64 entityId)
        {
            if (this._items != null && this._items.Count > 0)
            {
                foreach (EntityStateValidationScore item in this._items)
                {
                    if (item.EntityId == entityId)
                    {
                        return item;
                    }
                }
            }

            return null;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadEntityStateValidationScoreCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityValidationScore")
                    {
                        String eVScoreXml = reader.ReadOuterXml();

                        if (!String.IsNullOrWhiteSpace(eVScoreXml))
                        {
                            EntityStateValidationScore eVSScore = new EntityStateValidationScore(eVScoreXml);

                            this.Add(eVSScore);
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
