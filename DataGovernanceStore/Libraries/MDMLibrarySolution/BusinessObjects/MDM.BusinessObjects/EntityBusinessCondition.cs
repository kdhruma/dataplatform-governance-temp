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
    /// Specifies entity business condition instance.
    /// </summary>
    [DataContract]
    [ProtoContract]
    [KnownType(typeof(BusinessConditionStatus))]
    [KnownType(typeof(BusinessConditionStatusCollection))]
    public class EntityBusinessCondition : MDMObject, IEntityBusinessCondition
    {
        #region Fields

        /// <summary>
        /// Field denoting id of the entity for which business conditions are calculated
        /// </summary>
        private Int64 _entityId = -1;

        /// <summary>
        /// Field denoting entity family id
        /// </summary>
        private Int64 _entityFamilyId = -1;

        /// <summary>
        /// Field denoting entity family group id
        /// </summary>
        private Int64 _entityGlobalFamilyId = -1;

        /// <summary>
        /// Field denoting business condition collection for the specified entity id.
        /// </summary>
        private BusinessConditionStatusCollection _businessConditions = new BusinessConditionStatusCollection();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public EntityBusinessCondition()
        { 
        
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">Indicates XMl representation of entity business condition</param>
        public EntityBusinessCondition(String valuesAsXml)
        {
            LoadEntityBusinessConditions(valuesAsXml);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Indicates id of the entity for which business conditions are calculated
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public Int64 EntityId
        {
            get { return _entityId; }
            set { _entityId = value; }
        }

        /// <summary>
        /// Indicates entity family id for a variant tree
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public Int64 EntityFamilyId
        {
            get 
            { 
                return this._entityFamilyId; 
            }
            set 
            { 
                this._entityFamilyId = value; 
            }
        }

        /// <summary>
        /// Indicates entity global family id across parent(including extended families)
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public Int64 EntityGlobalFamilyId
        {
            get 
            { 
                return this._entityGlobalFamilyId; 
            }
            set 
            { 
                this._entityGlobalFamilyId = value; 
            }
        }

        /// <summary>
        /// Indicates business condition collection for the specified entity id.
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        public BusinessConditionStatusCollection BusinessConditions
        {
            get 
            { 
                if(this._businessConditions == null)
                {
                    this._businessConditions = new BusinessConditionStatusCollection();
                }

                return _businessConditions; 
            }
            set 
            { 
                this._businessConditions = value; 
            }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get XMl representation of entity business condition
        /// </summary>
        /// <returns>XMl string representation of entity business condition</returns>
        public override String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    #region write EntityBusinessCondition

                    //EntityBusinessCondition node start
                    xmlWriter.WriteStartElement("EntityBusinessCondition");

                    xmlWriter.WriteAttributeString("EntityId", this.EntityId.ToString());
                    xmlWriter.WriteAttributeString("EntityFamilyId", this.EntityFamilyId.ToString());
                    xmlWriter.WriteAttributeString("EntityGlobalFamilyId", this.EntityGlobalFamilyId.ToString());

                    if (this.BusinessConditions != null)
                    {
                        xmlWriter.WriteRaw(this.BusinessConditions.ToXml());
                    }

                    //EntityBusinessCondition node end
                    xmlWriter.WriteEndElement();

                    #endregion  write EntityBusinessCondition

                    xmlWriter.Flush();

                    //get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Sets the business condition collection
        /// </summary>
        /// <param name="iBusinessConditions">Indicates business condition collection object to be set</param>
        public void SetBusinessConditions(IBusinessConditionStatusCollection iBusinessConditions)
        {
            if (iBusinessConditions == null)
            {
                throw new ArgumentNullException("iBusinessConditions");
            }

            this._businessConditions = (BusinessConditionStatusCollection)iBusinessConditions;
        }

        /// <summary>
        /// Gets business condition collection
        /// </summary>
        /// <returns>Returns business condition collection</returns>
        public IBusinessConditionStatusCollection GetBusinessConditions()
        {
            return (IBusinessConditionStatusCollection) this.BusinessConditions;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadEntityBusinessConditions(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityBusinessCondition")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("EntityId"))
                                {
                                    this.EntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this.EntityId);
                                }

                                if (reader.MoveToAttribute("EntityFamilyId"))
                                {
                                    this.EntityFamilyId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this.EntityFamilyId);
                                }

                                if (reader.MoveToAttribute("EntityGlobalFamilyId"))
                                {
                                    this.EntityGlobalFamilyId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this.EntityGlobalFamilyId);
                                }

                                reader.Read();
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "BusinessConditions")
                        {
                            #region Read BusinessConditions

                            String businessConditionsXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(businessConditionsXml))
                            {
                                BusinessConditionStatusCollection businessConditions = new BusinessConditionStatusCollection(businessConditionsXml);

                                if (businessConditions != null && businessConditions.Count > 0)
                                {
                                    this.BusinessConditions = businessConditions;
                                }
                            }

                            #endregion Read BusinessConditions
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
