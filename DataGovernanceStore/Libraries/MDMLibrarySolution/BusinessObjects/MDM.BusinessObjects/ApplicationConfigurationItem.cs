using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies ApplicationConfigurationItem
    /// </summary>
    [DataContract]
    public class ApplicationConfigurationItem : MDMObject, IApplicationConfigurationItem
    {
        #region Constants

        private const String DateTimeXmlFormat = "yyyy/mm/dd hh:mm:ss.ffff tt";

        #endregion

        #region Fields

        /// <summary>
        /// Field indicates ConfigParentId
        /// </summary>
        private Int32? _configParentId = null;

        /// <summary>
        /// Field indicates ContextDefinitionId
        /// </summary>
        private Int32? _contextDefinitionId = null;

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
        /// Field indicates ConfigXML
        /// </summary>
        private String _configXml = String.Empty;

        /// <summary>
        /// Field indicates Description
        /// </summary>
        private String _description = String.Empty;

        /// <summary>
        /// Field indicates Precondition
        /// </summary>
        private String _precondition = String.Empty;

        /// <summary>
        /// Field indicates Postcondition
        /// </summary>
        private String _postcondition = String.Empty;

        /// <summary>
        /// Field indicates XsdSchema
        /// </summary>
        private String _xsdSchema = String.Empty;

        /// <summary>
        /// Field indicates SampleXml
        /// </summary>
        private String _sampleXml = String.Empty;

        /// <summary>
        /// Field indicates CreateDateTime
        /// </summary>
        private DateTime? _createDateTime = null;

        /// <summary>
        /// Field indicates ModDateTime
        /// </summary>
        private DateTime? _modDateTime = null;

        /// <summary>
        /// Field indicates CreateUser
        /// </summary>
        private String _createUser = String.Empty;

        /// <summary>
        /// Field indicates ModUser
        /// </summary>
        private String _modUser = String.Empty;

        /// <summary>
        /// Field indicates CreateProgram
        /// </summary>
        private String _createProgram = String.Empty;

        /// <summary>
        /// Field indicates ModProgram
        /// </summary>
        private String _modProgram = String.Empty;

        /// <summary>
        /// Field indicates SequenceNumber
        /// </summary>
        private Int32? _sequenceNumber = null;

        /// <summary>
        /// Field indicates Locale
        /// </summary>
        private LocaleEnum? _locale = null;

        /// <summary>
        /// Field indicates Lookup Name
        /// </summary>
        private String _objectName = String.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property indicates ConfigParentId
        /// </summary>
        [DataMember]
        public Int32? ConfigParentId
        {
            get { return this._configParentId; }
            set { this._configParentId = value; }
        }

        /// <summary>
        /// Property indicates ContextDefinitionId
        /// </summary>
        [DataMember]
        public Int32? ContextDefinitionId
        {
            get { return this._contextDefinitionId; }
            set { this._contextDefinitionId = value; }
        }

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
        /// Property indicates ConfigXML
        /// </summary>
        [DataMember]
        public String ConfigXml
        {
            get { return this._configXml; }
            set { this._configXml = value; }
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
        /// Property indicates Precondition
        /// </summary>
        [DataMember]
        public String Precondition
        {
            get { return this._precondition; }
            set { this._precondition = value; }
        }

        /// <summary>
        /// Property indicates Postcondition
        /// </summary>
        [DataMember]
        public String Postcondition
        {
            get { return this._postcondition; }
            set { this._postcondition = value; }
        }

        /// <summary>
        /// Property indicates XsdSchema
        /// </summary>
        [DataMember]
        public String XsdSchema
        {
            get { return this._xsdSchema; }
            set { this._xsdSchema = value; }
        }

        /// <summary>
        /// Property indicates SampleXml
        /// </summary>
        [DataMember]
        public String SampleXml
        {
            get { return this._sampleXml; }
            set { this._sampleXml = value; }
        }

        /// <summary>
        /// Property indicates CreateDateTime
        /// </summary>
        [DataMember]
        public DateTime? CreateDateTime
        {
            get { return this._createDateTime; }
            set { this._createDateTime = value; }
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
        /// Property indicates CreateUser
        /// </summary>
        [DataMember]
        public String CreateUser
        {
            get { return this._createUser; }
            set { this._createUser = value; }
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
        /// Property indicates CreateProgram
        /// </summary>
        [DataMember]
        public String CreateProgram
        {
            get { return this._createProgram; }
            set { this._createProgram = value; }
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
        /// Property indicates Locale
        /// </summary>
        [DataMember]
        public new LocaleEnum? Locale
        {
            get { return this._locale; }
            set { this._locale = value; }
        }

        /// <summary>
        /// Property indicates Lookup Name
        /// </summary>
        [DataMember]
        public String ObjectName
        {
            get { return this._objectName; }
            set { this._objectName = value; }
        }
        #endregion Properties

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ApplicationConfigurationItem()
            : base()
        {
        }

        /// <summary>
        /// Constructor with XML
        /// </summary>
        /// <param name="valuesAsxml">XML having ApplicationConfigurationItem data</param>
        public ApplicationConfigurationItem(String valuesAsxml)
        {
            LoadFromXml(valuesAsxml);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Creates a clone copy of application configuration item object
        /// </summary>
        /// <returns>Returns clone copy of application configuration item object</returns>
        public ApplicationConfigurationItem DeepClone()
        {
            ApplicationConfigurationItem clonedConfig = new ApplicationConfigurationItem();

            clonedConfig.Id = this.Id;
            clonedConfig.ReferenceId = this.ReferenceId;
            clonedConfig.PermissionSet = this.PermissionSet;
            clonedConfig.Name = this.Name;
            clonedConfig.LongName = this.LongName;
            clonedConfig.ConfigParentId = this.ConfigParentId;
            clonedConfig.ContextDefinitionId = this.ContextDefinitionId;
            clonedConfig.OrganizationId = this.OrganizationId;
            clonedConfig.ContainerId = this.ContainerId;
            clonedConfig.CategoryId = this.CategoryId;
            clonedConfig.EntityId = this.EntityId;
            clonedConfig.AttributeId = this.AttributeId;
            clonedConfig.NodeTypeId = this.NodeTypeId;
            clonedConfig.RelationshipTypeId = this.RelationshipTypeId;
            clonedConfig.SecurityRoleId = this.SecurityRoleId;
            clonedConfig.SecurityUserId = this.SecurityUserId;
            clonedConfig.ConfigXml = this.ConfigXml;
            clonedConfig.Description = this.Description;
            clonedConfig.Precondition = this.Precondition;
            clonedConfig.Postcondition = this.Postcondition;
            clonedConfig.XsdSchema = this.XsdSchema;
            clonedConfig.CreateDateTime = this.CreateDateTime;
            clonedConfig.ModDateTime = this.ModDateTime;
            clonedConfig.CreateUser = this.CreateUser;
            clonedConfig.ModUser = this.ModUser;
            clonedConfig.ModProgram = this.ModProgram;
            clonedConfig.SequenceNumber = this.SequenceNumber;
            clonedConfig.Locale = this.Locale;
            clonedConfig._objectName = this.ObjectName;

            return clonedConfig;
        }

        /// <summary>
        /// Loads from XML node
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            foreach (XmlNode child in xmlNode.ChildNodes)
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
                if (child.Name == "Locale")
                {
                    this.Locale = null;
                    LocaleEnum tmp = LocaleEnum.en_WW;
                    if (Enum.TryParse(child.InnerText, true, out tmp))
                    {
                        this.Locale = tmp;
                    }
                    continue;
                }
                if (child.Name == "ConfigParentId")
                {
                    ConfigParentId = ValueTypeHelper.ConvertToNullableInt32(child.InnerText);
                    continue;
                }
                if (child.Name == "ContextDefinitionId")
                {
                    ContextDefinitionId = ValueTypeHelper.ConvertToNullableInt32(child.InnerText);
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
                if (child.Name == "ConfigXml")
                {
                    ConfigXml = String.Empty;
                    if (!String.IsNullOrWhiteSpace(child.InnerText))
                    {
                        ConfigXml = child.InnerText;
                    }
                    continue;
                }
                if (child.Name == "Description")
                {
                    Description = child.InnerText;
                    continue;
                }
                if (child.Name == "Precondition")
                {
                    Precondition = child.InnerText;
                    continue;
                }
                if (child.Name == "Postcondition")
                {
                    Postcondition = child.InnerText;
                    continue;
                }
                if (child.Name == "XsdSchema")
                {
                    XsdSchema = String.Empty;
                    if (!String.IsNullOrWhiteSpace(child.InnerText))
                    {
                        XsdSchema = child.InnerText;
                    }
                    continue;
                }
                if (child.Name == "SampleXml")
                {
                    SampleXml = String.Empty;
                    if (!String.IsNullOrWhiteSpace(child.InnerText))
                    {
                        SampleXml = child.InnerText;
                    }
                    continue;
                }
                if (child.Name == "CreateDateTime")
                {
                    CreateDateTime = null;
                    if (String.IsNullOrWhiteSpace(child.InnerText))
                    {
                        continue;
                    }
                    DateTime tmp;
                    if (DateTime.TryParseExact(child.InnerText, DateTimeXmlFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out tmp))
                    {
                        CreateDateTime = tmp;
                    }
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
                    if (DateTime.TryParseExact(child.InnerText, DateTimeXmlFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out tmp))
                    {
                        ModDateTime = tmp;
                    }
                    continue;
                }
                if (child.Name == "CreateUser")
                {
                    CreateUser = child.InnerText;
                    continue;
                }
                if (child.Name == "ModUser")
                {
                    ModUser = child.InnerText;
                    continue;
                }
                if (child.Name == "CreateProgram")
                {
                    CreateProgram = child.InnerText;
                    continue;
                }
                if (child.Name == "ModProgram")
                {
                    ModProgram = child.InnerText;
                    continue;
                }
                if (child.Name == "SequenceNumber")
                {
                    SequenceNumber = ValueTypeHelper.ConvertToNullableInt32(child.InnerText);
                    continue;
                }
                if (child.Name== "ObjectName")
                {
                    ObjectName = child.InnerText;
                    continue;
                }
            }
        }

        /// <summary>
        /// Loads from XML
        /// </summary>
        public void LoadFromXml(String xml)
        {
            if (!String.IsNullOrWhiteSpace(xml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlNode node = doc.SelectSingleNode("ApplicationConfigurationItem");
                if (node != null)
                {
                    LoadFromXml(node);
                }
            }
        }

        /// <summary>
        /// Get Xml representation of ApplicationConfigurationItem
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public new String ToXml()
        {
            String resultXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //ApplicationConfigurationItem node start
            xmlWriter.WriteStartElement("ApplicationConfigurationItem");

            #region Write Properties

            xmlWriter.WriteStartElement("Id");
            xmlWriter.WriteValue(this.Id.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ShortName");
            xmlWriter.WriteValue(this.Name);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("LongName");
            xmlWriter.WriteValue(this.LongName);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Locale");
            xmlWriter.WriteValue(this.Locale.HasValue ? this.Locale.Value.ToString() : "");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ConfigParentId");
            xmlWriter.WriteValue(this.ConfigParentId.HasValue ? this.ConfigParentId.Value.ToString(CultureInfo.InvariantCulture) : "");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ContextDefinitionId");
            xmlWriter.WriteValue(this.ContextDefinitionId.HasValue ? this.ContextDefinitionId.Value.ToString(CultureInfo.InvariantCulture) : "");
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

            xmlWriter.WriteStartElement("ConfigXml");
            xmlWriter.WriteRaw(WrapStringToCDataBlock(ConfigXml));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Description");
            xmlWriter.WriteValue(this.Description);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Precondition");
            xmlWriter.WriteValue(this.Precondition);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Postcondition");
            xmlWriter.WriteValue(this.Postcondition);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("XsdSchema");
            xmlWriter.WriteRaw(WrapStringToCDataBlock(XsdSchema));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("SampleXml");
            xmlWriter.WriteRaw(WrapStringToCDataBlock(SampleXml));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("CreateDateTime");
            xmlWriter.WriteValue(this.CreateDateTime.HasValue ? CreateDateTime.Value.ToString(DateTimeXmlFormat) : "");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ModDateTime");
            xmlWriter.WriteValue(this.ModDateTime.HasValue ? ModDateTime.Value.ToString(DateTimeXmlFormat) : "");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("CreateUser");
            xmlWriter.WriteValue(this.CreateUser);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ModUser");
            xmlWriter.WriteValue(this.ModUser);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("CreateProgram");
            xmlWriter.WriteValue(this.CreateProgram);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ModProgram");
            xmlWriter.WriteValue(this.ModProgram);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("SequenceNumber");
            xmlWriter.WriteValue(this.SequenceNumber.HasValue ? this.SequenceNumber.Value.ToString(CultureInfo.InvariantCulture) : "");
            xmlWriter.WriteEndElement();


            xmlWriter.WriteStartElement("ObjectName");
            xmlWriter.WriteValue(this.ObjectName);
            xmlWriter.WriteEndElement();
            #endregion

            //ApplicationConfigurationItem node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            resultXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return resultXml;
        }

        /// <summary>
        /// Get Xml representation of ApplicationConfigurationItem
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public new String ToXml(ObjectSerialization serialization)
        {
            // No serialization implemented for now...
            return this.ToXml();
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone ApplicationConfigurationItem
        /// </summary>
        /// <returns>Cloned SurvivorshipRuleSourceListItem object</returns>
        public object Clone()
        {
            ApplicationConfigurationItem cloned = (ApplicationConfigurationItem)this.MemberwiseClone();
            return cloned;
        }

        #endregion

        #region Private Methods

        private String WrapStringToCDataBlock(String source)
        {
            if (String.IsNullOrEmpty(source))
            {
                return String.Empty;
            }
            return "<![CDATA[" + source.Replace("]]>", "]]]]><![CDATA[>") + "]]>";
        }

        #endregion
    }
}