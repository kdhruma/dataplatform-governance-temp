using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Specifies SurvivorshipRuleset
    /// </summary>
    [DataContract]
    public class SurvivorshipRuleset : MDMObject, ISurvivorshipRuleset
    {
        #region Fields

        /// <summary>
        /// Field indicates OrganizationId
        /// </summary>
        private Int32 _organizationId = 0;

        /// <summary>
        /// Field indicates ContainerId
        /// </summary>
        private Int32 _containerId = 0;

        /// <summary>
        /// Field indicates CategoryId
        /// </summary>
        private Int64 _categoryId = 0;

        /// <summary>
        /// Field indicates CategoryPath
        /// </summary>
        private String _categoryPath = String.Empty;

        /// <summary>
        /// Field indicates EntityId
        /// </summary>
        private Int64 _entityId = 0;

        /// <summary>
        /// Field indicates AttributeId
        /// </summary>
        private Int32 _attributeId = 0;

        /// <summary>
        /// Field indicates NodeTypeId
        /// </summary>
        private Int32 _nodeTypeId = 0;

        /// <summary>
        /// Field indicates RelationshipTypeId
        /// </summary>
        private Int32 _relationshipTypeId = 0;

        /// <summary>
        /// Field indicates SecurityRoleId
        /// </summary>
        private Int32 _securityRoleId = 0;

        /// <summary>
        /// Field indicates SecurityUserId
        /// </summary>
        private Int32 _securityUserId = 0;

        /// <summary>
        /// Field indicates Description
        /// </summary>
        private String _description = String.Empty;

        /// <summary>
        /// Field indicates ModDateTime
        /// </summary>
        private DateTime? _modDateTime = null;

        /// <summary>
        /// Field indicates ModUser
        /// </summary>
        private String _modUser = String.Empty;

        /// <summary>
        /// Field indicates ModProgram
        /// </summary>
        private String _modProgram = String.Empty;

        /// <summary>
        /// Field indicates SequenceNumber
        /// </summary>
        private Int32? _sequenceNumber = null;

        /// <summary>
        /// Field for SurvivorshipRules
        /// </summary>
        private SurvivorshipRuleCollection _survivorshipRules = new SurvivorshipRuleCollection();

        /// <summary>
        /// Field for StrategyPriorities
        /// </summary>
        private SurvivorshipRuleStrategyPriorityCollection _strategyPriorities = new SurvivorshipRuleStrategyPriorityCollection();

        #endregion

        #region Constuctors

        /// <summary>
        /// Constructs SurvivorshipRuleset
        /// </summary>
        public SurvivorshipRuleset()
            : base()
        {
        }

        /// <summary>
        /// Copy constructor. Constructs SurvivorshipRuleset using specified instance data
        /// </summary>
        public SurvivorshipRuleset(SurvivorshipRuleset source)
            : base(source.Id, source.Name, source.LongName, source.Locale, source.AuditRefId, source.ProgramName)
        {
            this.OrganizationId = source.OrganizationId;
            this.ContainerId = source.ContainerId;
            this.CategoryId = source.CategoryId;
            this.CategoryPath = source.CategoryPath;
            this.EntityId = source.EntityId;
            this.AttributeId = source.AttributeId;
            this.NodeTypeId = source.NodeTypeId;
            this.RelationshipTypeId = source.RelationshipTypeId;
            this.SecurityRoleId = source.SecurityRoleId;
            this.SecurityUserId = source.SecurityUserId;
            this.Description = source.Description;
            this.ModDateTime = source.ModDateTime;
            this.ModUser = source.ModUser;
            this.ModProgram = source.ModProgram;
            this.SequenceNumber = source.SequenceNumber;
            this.SurvivorshipRules = (SurvivorshipRuleCollection) source.SurvivorshipRules.Clone();
            this.StrategyPriorities = (SurvivorshipRuleStrategyPriorityCollection) source.StrategyPriorities.Clone();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property indicates OrganizationId
        /// </summary>
        [DataMember]
        public Int32 OrganizationId
        {
            get { return this._organizationId; }
            set { this._organizationId = value; }
        }

        /// <summary>
        /// Property indicates ContainerId
        /// </summary>
        [DataMember]
        public Int32 ContainerId
        {
            get { return this._containerId; }
            set { this._containerId = value; }
        }

        /// <summary>
        /// Property indicates CategoryId
        /// </summary>
        [DataMember]
        public Int64 CategoryId
        {
            get { return this._categoryId; }
            set { this._categoryId = value; }
        }

        /// <summary>
        /// Property indicates CategoryPath
        /// </summary>
        [DataMember]
        public String CategoryPath
        {
            get { return this._categoryPath; }
            set { this._categoryPath = value; }
        }

        /// <summary>
        /// Property indicates EntityId
        /// </summary>
        [DataMember]
        public Int64 EntityId
        {
            get { return this._entityId; }
            set { this._entityId = value; }
        }

        /// <summary>
        /// Property indicates AttributeId
        /// </summary>
        [DataMember]
        public Int32 AttributeId
        {
            get { return this._attributeId; }
            set { this._attributeId = value; }
        }

        /// <summary>
        /// Property indicates NodeTypeId
        /// </summary>
        [DataMember]
        public Int32 NodeTypeId
        {
            get { return this._nodeTypeId; }
            set { this._nodeTypeId = value; }
        }

        /// <summary>
        /// Property indicates RelationshipTypeId
        /// </summary>
        [DataMember]
        public Int32 RelationshipTypeId
        {
            get { return this._relationshipTypeId; }
            set { this._relationshipTypeId = value; }
        }

        /// <summary>
        /// Property indicates SecurityRoleId
        /// </summary>
        [DataMember]
        public Int32 SecurityRoleId
        {
            get { return this._securityRoleId; }
            set { this._securityRoleId = value; }
        }

        /// <summary>
        /// Property indicates SecurityUserId
        /// </summary>
        [DataMember]
        public Int32 SecurityUserId
        {
            get { return this._securityUserId; }
            set { this._securityUserId = value; }
        }

        /// <summary>
        /// Property indicates Description
        /// </summary>
        [DataMember]
        public String Description
        {
            get { return this._description; }
            set { this._description = value; }
        }

        /// <summary>
        /// Property indicates ModDateTime
        /// </summary>
        [DataMember]
        public DateTime? ModDateTime
        {
            get { return this._modDateTime; }
            set { this._modDateTime = value; }
        }

        /// <summary>
        /// Property indicates ModUser
        /// </summary>
        [DataMember]
        public String ModUser
        {
            get { return this._modUser; }
            set { this._modUser = value; }
        }

        /// <summary>
        /// Property indicates ModProgram
        /// </summary>
        [DataMember]
        public String ModProgram
        {
            get { return this._modProgram; }
            set { this._modProgram = value; }
        }

        /// <summary>
        /// Property indicates SequenceNumber
        /// </summary>
        [DataMember]
        public Int32? SequenceNumber
        {
            get { return this._sequenceNumber; }
            set { this._sequenceNumber = value; }
        }

        /// <summary>
        /// Property denoting SurvivorshipRules
        /// </summary>
        [DataMember]
        public SurvivorshipRuleCollection SurvivorshipRules
        {
            get { return _survivorshipRules; }
            set { _survivorshipRules = value; }
        }

        /// <summary>
        /// Property denoting StrategyPriorities
        /// </summary>
        [DataMember]
        public SurvivorshipRuleStrategyPriorityCollection StrategyPriorities
        {
            get { return _strategyPriorities; }
            set { _strategyPriorities = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Xml representation of SurvivorshipRuleset
        /// </summary>
        public new String ToXml()
        {
            return ToXml(ObjectSerialization.Full);
        }

        /// <summary>
        /// Get Xml representation of SurvivorshipRuleset
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public new String ToXml(ObjectSerialization objectSerialization)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //SurvivorshipRuleset node start
            xmlWriter.WriteStartElement("SurvivorshipRuleset");

            if (objectSerialization != ObjectSerialization.Compact)
            {
                xmlWriter.WriteStartElement("Id");
                xmlWriter.WriteValue(this.Id.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("ShortName");
                xmlWriter.WriteValue(this.Name);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("LongName");
                xmlWriter.WriteValue(this.LongName);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("Description");
                xmlWriter.WriteValue(this.Description);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("SequenceNumber");
                xmlWriter.WriteValue(this.SequenceNumber.HasValue ? this.SequenceNumber.Value.ToString(CultureInfo.InvariantCulture) : "");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("Locale");
                xmlWriter.WriteValue(this.Locale.ToString());
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("OrganizationId");
                xmlWriter.WriteValue(this.OrganizationId.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("ContainerId");
                xmlWriter.WriteValue(this.ContainerId.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("CategoryId");
                xmlWriter.WriteValue(this.CategoryId.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("CategoryPath");
                xmlWriter.WriteValue(this.CategoryPath);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("EntityId");
                xmlWriter.WriteValue(this.EntityId.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("AttributeId");
                xmlWriter.WriteValue(this.AttributeId.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("NodeTypeId");
                xmlWriter.WriteValue(this.NodeTypeId.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("RelationshipTypeId");
                xmlWriter.WriteValue(this.RelationshipTypeId.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("SecurityRoleId");
                xmlWriter.WriteValue(this.SecurityRoleId.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("SecurityUserId");
                xmlWriter.WriteValue(this.SecurityUserId.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("AuditRefId");
                xmlWriter.WriteValue(this.AuditRefId.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteEndElement();

                if (ModDateTime.HasValue)
                {
                    xmlWriter.WriteStartElement("ModDateTime");
                    xmlWriter.WriteValue(ModDateTime.Value.ToString("o", CultureInfo.InvariantCulture));
                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteStartElement("ModUser");
                xmlWriter.WriteValue(this.ModUser);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("ModProgram");
                xmlWriter.WriteValue(this.ModProgram);
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteStartElement("SurvivorshipRules");
            xmlWriter.WriteRaw(SurvivorshipRules.ToXml());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("StrategyPriorities");
            xmlWriter.WriteRaw(StrategyPriorities.ToXml());
            xmlWriter.WriteEndElement();

            //SurvivorshipRuleset node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Loads SurvivorshipRuleset from XML node
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            SurvivorshipRules.Clear();
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "Id")
                {
                    Id = ValueTypeHelper.Int32TryParse(child.InnerText, -1);
                    continue;
                }
                if (child.Name == "ShortName")
                {
                    Name = child.InnerText;
                    continue;
                }
                if (child.Name == "LongName")
                {
                    LongName = child.InnerText;
                    continue;
                }
                if (child.Name == "Description")
                {
                    Description = child.InnerText;
                    continue;
                }
                if (child.Name == "SequenceNumber")
                {
                    SequenceNumber = ValueTypeHelper.ConvertToNullableInt32(child.InnerText);
                    continue;
                }
                if (child.Name == "Locale")
                {
                    this.Locale = LocaleEnum.UnKnown;
                    LocaleEnum tmp = LocaleEnum.en_WW;
                    if (Enum.TryParse(child.InnerText, true, out tmp))
                    {
                        this.Locale = tmp;
                    }
                    continue;
                }
                if (child.Name == "OrganizationId")
                {
                    OrganizationId = ValueTypeHelper.Int32TryParse(child.InnerText, 0);
                    continue;
                }
                if (child.Name == "ContainerId")
                {
                    ContainerId = ValueTypeHelper.Int32TryParse(child.InnerText, 0);
                    continue;
                }
                if (child.Name == "CategoryId")
                {
                    CategoryId = ValueTypeHelper.Int64TryParse(child.InnerText, 0);
                    continue;
                }
                if (child.Name == "CategoryPath")
                {
                    CategoryPath = child.InnerText;
                    continue;
                }
                if (child.Name == "EntityId")
                {
                    EntityId = ValueTypeHelper.Int64TryParse(child.InnerText, 0);
                    continue;
                }
                if (child.Name == "AttributeId")
                {
                    AttributeId = ValueTypeHelper.Int32TryParse(child.InnerText, 0);
                    continue;
                }
                if (child.Name == "NodeTypeId")
                {
                    NodeTypeId = ValueTypeHelper.Int32TryParse(child.InnerText, 0);
                    continue;
                }
                if (child.Name == "RelationshipTypeId")
                {
                    RelationshipTypeId = ValueTypeHelper.Int32TryParse(child.InnerText, 0);
                    continue;
                }
                if (child.Name == "SecurityRoleId")
                {
                    SecurityRoleId = ValueTypeHelper.Int32TryParse(child.InnerText, 0);
                    continue;
                }
                if (child.Name == "SecurityUserId")
                {
                    SecurityUserId = ValueTypeHelper.Int32TryParse(child.InnerText, 0);
                    continue;
                }
                if (child.Name == "AuditRefId")
                {
                    AuditRefId = ValueTypeHelper.Int64TryParse(child.InnerText, 0);
                    continue;
                }
                if (child.Name == "ModDateTime")
                {
                    ModDateTime = null;
                    if (String.IsNullOrWhiteSpace(child.InnerText))
                    {
                        continue;
                    }
                    DateTime tmp;
                    if (DateTime.TryParseExact(child.InnerText, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out tmp))
                    {
                        ModDateTime = tmp;
                    }
                    continue;
                }
                if (child.Name == "ModUser")
                {
                    ModUser = child.InnerText;
                    continue;
                }
                if (child.Name == "ModProgram")
                {
                    ModProgram = child.InnerText;
                    continue;
                }
                if (child.Name == "SurvivorshipRules")
                {
                    SurvivorshipRules.LoadFromXml(child);
                    continue;
                }
                if (child.Name == "StrategyPriorities")
                {
                    StrategyPriorities.LoadFromXml(child);
                    continue;
                }
            }
        }

        /// <summary>
        /// Loads SurvivorshipRuleset from XML with outer node
        /// </summary>
        public virtual void LoadFromXml(String xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode node = doc.SelectSingleNode("SurvivorshipRuleset");
            if (node != null)
            {
                LoadFromXml(node);
            }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone SurvivorshipRuleset
        /// </summary>
        /// <returns>Cloned SurvivorshipRuleset object</returns>
        public object Clone()
        {
            SurvivorshipRuleset cloned = new SurvivorshipRuleset(this);
            return cloned;
        }

        #endregion
    }
}