using ProtoBuf;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies validation score for the entity
    /// </summary>
    [DataContract]
    [ProtoContract]
    [KnownType(typeof(EntityStateValidationAttributeScore))]
    [KnownType(typeof(EntityStateValidationAttributeScoreCollection))]
    public class EntityStateValidationScore : MDMObject, IEntityStateValidationScore
    {
        #region Fields

        /// <summary>
        /// Field denoting id of the entity for which validation score is calculated
        /// </summary>
        private Int64 _entityId = -1;

        /// <summary>
        /// Field denoting over all entity validation score
        /// </summary>
        private Double _overallScore = 0;

        /// <summary>
        /// Field denoting validation score for system attributes of the entity
        /// </summary>
        private EntityStateValidationAttributeScoreCollection _entityStateValidationAttributeScores = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public EntityStateValidationScore()
        {

        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">Indicates XMl representation of entity validation score</param>
        public EntityStateValidationScore(String valuesAsXml)
        {
            LoadEntityStateValidationScore(valuesAsXml);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Indicates id of the entity for which validation score is calculated
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public Int64 EntityId
        {
            get
            {
                return this._entityId;
            }
            set
            {
                this._entityId = value;
            }
        }

        /// <summary>
        /// Indicates over all entity validation score
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public Double OverallScore
        {
            get
            {
                return this._overallScore;
            }
            set
            {
                this._overallScore = value;
            }
        }

        /// <summary>
        /// Indicates validation score for system attributes of the entity
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public EntityStateValidationAttributeScoreCollection EntityStateValidationAttributeScores
        {
            get
            {
                if (this._entityStateValidationAttributeScores == null)
                {
                    this._entityStateValidationAttributeScores = new EntityStateValidationAttributeScoreCollection();
                }

                return this._entityStateValidationAttributeScores;
            }
            set
            {
                this._entityStateValidationAttributeScores = value;
            }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get XMl representation of entity validation score
        /// </summary>
        /// <returns>XMl string representation of entity validation score</returns>
        public override String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    #region write EntityStateValidationScore

                    //EntityStateValidationScore node start
                    xmlWriter.WriteStartElement("EntityStateValidationScore");

                    xmlWriter.WriteAttributeString("EntityId", this.EntityId.ToString());

                    xmlWriter.WriteAttributeString("OverallScore", this.OverallScore.ToString());

                    #region write EntityStateValidationAttributeScores xml

                    if (this.EntityStateValidationAttributeScores != null)
                    {
                        xmlWriter.WriteRaw(this.EntityStateValidationAttributeScores.ToXml());
                    }

                    #endregion write EntityStateValidationAttributeScores  xml

                    //EntityStateValidationScore node end
                    xmlWriter.WriteEndElement();

                    #endregion  write EntityValidationScore

                    xmlWriter.Flush();

                    //get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Get EntityStateValidationAttributeScoreCollection collection
        /// </summary>
        /// <returns>Returns EntityStateValidationAttributeScoreCollection interface</returns>
        public IEntityStateValidationAttributeScoreCollection GetEntityStateValidationAttributeScores()
        {
            return (IEntityStateValidationAttributeScoreCollection)this.EntityStateValidationAttributeScores;
        }

        /// <summary>
        /// Set EntityStateValidationAttributeScore collection
        /// </summary>
        /// <param name="iEntityValidationStateAttributeScores">Indicates collection object to be set</param>
        /// <exception cref="ArgumentNullException">Raised when passed EntityStateValidationAttributeScoreCollection is null</exception>
        public void SetEntityStateValidationAttributeScores(IEntityStateValidationAttributeScoreCollection iEntityValidationStateAttributeScores)
        {
            if (iEntityValidationStateAttributeScores == null)
            {
                throw new ArgumentNullException("iEntityValidationStateAttributeScores");
            }

            this.EntityStateValidationAttributeScores = (EntityStateValidationAttributeScoreCollection)iEntityValidationStateAttributeScores;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadEntityStateValidationScore(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityStateValidationScore")
                    {
                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("EntityId"))
                            {
                                this.EntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this.EntityId);
                            }

                            if (reader.MoveToAttribute("OverallScore"))
                            {
                                this.OverallScore = ValueTypeHelper.DoubleTryParse(reader.ReadContentAsString(), this.OverallScore);
                            }

                            reader.Read();
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityStateValidationAttributeScores")
                    {
                        #region Read EntityStateValidationAttributeScoreCollection

                        String eVSAttrScoresXml = reader.ReadOuterXml();

                        if (!String.IsNullOrWhiteSpace(eVSAttrScoresXml))
                        {
                            EntityStateValidationAttributeScoreCollection eVSAttrScores = new EntityStateValidationAttributeScoreCollection(eVSAttrScoresXml);

                            foreach (EntityStateValidationAttributeScore attrScore in eVSAttrScores)
                            {
                                if (!this.EntityStateValidationAttributeScores.Contains(attrScore))
                                {
                                    this.EntityStateValidationAttributeScores.Add(attrScore);
                                }
                            }
                        }

                        #endregion Read EntityStateValidationAttributeScoreCollection
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
