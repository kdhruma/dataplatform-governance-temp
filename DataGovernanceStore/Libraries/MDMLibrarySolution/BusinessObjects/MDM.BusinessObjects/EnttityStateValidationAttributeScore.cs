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
    /// Specifies entity state validation attribute score
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class EntityStateValidationAttributeScore : MDMObject, IEntityStateValidationAttributeScore
    {
        #region Fields

        /// <summary>
        /// Field denoting state attribute for the entity
        /// </summary>
        private SystemAttributes _stateValidationAttribute = SystemAttributes.EntitySelfValid;

        /// <summary>
        /// Field denoting maximum score for the state attribute
        /// </summary>
        private Int32 _weightage = -1;

        /// <summary>
        /// Field denoting calculated score for state attribute
        /// </summary>
        private Double _score = 0;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public EntityStateValidationAttributeScore()
        {
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">Indicates XMl representation of entity state validation attribute score</param>
        public EntityStateValidationAttributeScore(String valuesAsXml)
        {
            LoadEntityStateValidationAttributeScore(valuesAsXml);
        }

        #endregion Construtors

        #region Properties

        /// <summary>
        /// Indicates state attribute for the entity
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public SystemAttributes StateValidationAttribute
        {
            get
            {
                return this._stateValidationAttribute;
            }
            set
            {
                this._stateValidationAttribute = value;
            }
        }

        /// <summary>
        /// Indicates maximum score for the state attribute
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public Int32 Weightage
        {
            get
            {
                return this._weightage;
            }
            set
            {
                this._weightage = value;
            }
        }

        /// <summary>
        /// Indicates calculated score for the state attribute
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public Double Score
        {
            get
            {
                return this._score;
            }
            set
            {
                this._score = value;
            }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get XMl representation of entity validation score
        /// </summary>
        /// <returns>XMl string representation of entity validation state attribute score</returns>
        public override String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    #region write EntityValidationStateAttributeScore

                    //EntityStateValidationAttributeScore node start
                    xmlWriter.WriteStartElement("EntityStateValidationAttributeScore");

                    xmlWriter.WriteAttributeString("StateValidationAttribute", this.StateValidationAttribute.ToString());
                    xmlWriter.WriteAttributeString("Weightage", this.Weightage.ToString());
                    xmlWriter.WriteAttributeString("Score", this.Score.ToString());

                    //EntityStateValidationAttributeScore node end
                    xmlWriter.WriteEndElement();

                    #endregion  write EntityStateValidationAttributeScore

                    xmlWriter.Flush();

                    //get the actual XML
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
        private void LoadEntityStateValidationAttributeScore(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityStateValidationAttributeScore")
                    {
                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("StateValidationAttribute"))
                            {
                                SystemAttributes sysAttr = SystemAttributes.EntitySelfValid;
                                ValueTypeHelper.EnumTryParse(reader.ReadContentAsString(), true, out sysAttr);
                                this.StateValidationAttribute = sysAttr;
                            }

                            if (reader.MoveToAttribute("Weightage"))
                            {
                                this.Weightage = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.Weightage);
                            }

                            if (reader.MoveToAttribute("Score"))
                            {
                                this.Score = ValueTypeHelper.DoubleTryParse(reader.ReadContentAsString(), this.Score);
                            }

                            reader.Read();
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
