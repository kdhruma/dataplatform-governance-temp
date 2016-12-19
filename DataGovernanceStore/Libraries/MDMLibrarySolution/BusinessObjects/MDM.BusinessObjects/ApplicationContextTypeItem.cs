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
    /// Specifies ApplicationContextTypeItem
    /// </summary>
    [DataContract]
    public class ApplicationContextTypeItem : MDMObject, IApplicationContextTypeItem
    {
        #region Constants

        private const String DateTimeXmlFormat = "yyyy/mm/dd hh:mm:ss.ffff tt";

        #endregion

        #region Fields

        /// <summary>
        /// Field indicates OrganizationFilterWeight
        /// </summary>
        private Int32? _organizationFilterWeight = null;

        /// <summary>
        /// Field indicates ContainerFilterWeight
        /// </summary>
        private Int32? _containerFilterWeight = null;

        /// <summary>
        /// Field indicates NodeTypeFilterWeight
        /// </summary>
        private Int32? _nodeTypeFilterWeight = -1;

        /// <summary>
        /// Field indicates RelationshipTypeFilterWeight
        /// </summary>
        private Int32? _relationshipTypeFilterWeight = -1;

        /// <summary>
        /// Field indicates CategoryFilterWeight
        /// </summary>
        private Int32? _categoryFilterWeight = -1;

        /// <summary>
        /// Field indicates AttributeFilterWeight
        /// </summary>
        private Int32? _attributeFilterWeight = -1;

        /// <summary>
        /// Field indicates EntityFilterWeight
        /// </summary>
        private Int32? _entityFilterWeight = -1;

        /// <summary>
        /// Field indicates RoleFilterWeight
        /// </summary>
        private Int32? _roleFilterWeight = -1;

        /// <summary>
        /// Field indicates UserFilterWeight
        /// </summary>
        private Int32? _userFilterWeight = -1;
        
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
        /// Field indicates LocaleFilterWeight
        /// </summary>
        private Int32? _localeFilterWeight = null;

        /// <summary>
        /// Field indicates AttributeSourceFilterWeight
        /// </summary>
        private Int32? _attributeSourceFilterWeight = null;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property indicates OrganizationFilterWeight
        /// </summary>
        [DataMember]
	    public Int32? OrganizationFilterWeight
        {
            get { return this._organizationFilterWeight; }
            set { this._organizationFilterWeight = value; }
        }

        /// <summary>
        /// Property indicates ContainerFilterWeight
        /// </summary>
        [DataMember]
	    public Int32? ContainerFilterWeight
        {
            get { return this._containerFilterWeight; }
            set { this._containerFilterWeight = value; }
        }

        /// <summary>
        /// Property indicates NodeTypeFilterWeight
        /// </summary>
        [DataMember]
	    public Int32? NodeTypeFilterWeight
        {
            get { return this._nodeTypeFilterWeight; }
            set { this._nodeTypeFilterWeight = value; }
        }

        /// <summary>
        /// Property indicates RelationshipTypeFilterWeight
        /// </summary>
        [DataMember]
	    public Int32? RelationshipTypeFilterWeight
        {
            get { return this._relationshipTypeFilterWeight; }
            set { this._relationshipTypeFilterWeight = value; }
        }

        /// <summary>
        /// Property indicates CategoryFilterWeight
        /// </summary>
        [DataMember]
        public Int32? CategoryFilterWeight
        {
            get { return this._categoryFilterWeight; }
            set { this._categoryFilterWeight = value; }
        }

        /// <summary>
        /// Property indicates AttributeFilterWeight
        /// </summary>
        [DataMember]
        public Int32? AttributeFilterWeight
        {
            get { return this._attributeFilterWeight; }
            set { this._attributeFilterWeight = value; }
        }

        /// <summary>
        /// Property indicates EntityFilterWeight
        /// </summary>
        [DataMember]
        public Int32? EntityFilterWeight
        {
            get { return this._entityFilterWeight; }
            set { this._entityFilterWeight = value; }
        }

        /// <summary>
        /// Property indicates RoleFilterWeight
        /// </summary>
        [DataMember]
        public Int32? RoleFilterWeight
        {
            get { return this._roleFilterWeight; }
            set { this._roleFilterWeight = value; }
        }

        /// <summary>
        /// Property indicates UserFilterWeight
        /// </summary>
        [DataMember]
        public Int32? UserFilterWeight
        {
            get { return this._userFilterWeight; }
            set { this._userFilterWeight = value; }
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
        /// Property indicates LocaleFilterWeight
        /// </summary>
        [DataMember]
	    public Int32? LocaleFilterWeight
        {
            get { return this._localeFilterWeight; }
            set { this._localeFilterWeight = value; }
        }

        /// <summary>
        /// Property indicates AttributeSourceFilterWeight
        /// </summary>
        [DataMember]
        public Int32? AttributeSourceFilterWeight
        {
            get { return this._attributeSourceFilterWeight; }
            set { this._attributeSourceFilterWeight = value; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ApplicationContextTypeItem()
            : base()
        {
        }

        /// <summary>
        /// Constructor with XML
        /// </summary>
        /// <param name="valuesAsxml">XML having ApplicationContextTypeItem data</param>
        public ApplicationContextTypeItem(String valuesAsxml)
        {
            LoadFromXml(valuesAsxml);
        }

        #endregion Constructors

        #region Public Methods

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
                if (child.Name == "OrganizationFilterWeight")
                {
                    OrganizationFilterWeight = ValueTypeHelper.ConvertToNullableInt32(child.InnerText);
                    continue;
                }
                if (child.Name == "ContainerFilterWeight")
                {
                    ContainerFilterWeight = ValueTypeHelper.ConvertToNullableInt32(child.InnerText);
                    continue;
                }
                if (child.Name == "NodeTypeFilterWeight")
                {
                    NodeTypeFilterWeight = ValueTypeHelper.ConvertToNullableInt32(child.InnerText);
                    continue;
                }
                if (child.Name == "RelationshipTypeFilterWeight")
                {
                    RelationshipTypeFilterWeight = ValueTypeHelper.ConvertToNullableInt32(child.InnerText);
                    continue;
                }
                if (child.Name == "CategoryFilterWeight")
                {
                    CategoryFilterWeight = ValueTypeHelper.ConvertToNullableInt32(child.InnerText);
                    continue;
                }
                if (child.Name == "AttributeFilterWeight")
                {
                    AttributeFilterWeight = ValueTypeHelper.ConvertToNullableInt32(child.InnerText);
                    continue;
                }
                if (child.Name == "EntityFilterWeight")
                {
                    EntityFilterWeight = ValueTypeHelper.ConvertToNullableInt32(child.InnerText);
                    continue;
                }
                if (child.Name == "RoleFilterWeight")
                {
                    RoleFilterWeight = ValueTypeHelper.ConvertToNullableInt32(child.InnerText);
                    continue;
                }
                if (child.Name == "UserFilterWeight")
                {
                    UserFilterWeight = ValueTypeHelper.ConvertToNullableInt32(child.InnerText);
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
                if (child.Name == "LocaleFilterWeight")
                {
                    LocaleFilterWeight = ValueTypeHelper.ConvertToNullableInt32(child.InnerText);
                    continue;
                }
                if (child.Name == "AttributeSourceFilterWeight")
                {
                    AttributeSourceFilterWeight = ValueTypeHelper.ConvertToNullableInt32(child.InnerText);
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
                XmlNode node = doc.SelectSingleNode("ApplicationContextTypeItem");
                if (node != null)
                {
                    LoadFromXml(node);
                }
            }
        }

        /// <summary>
        /// Get Xml representation of ApplicationContextTypeItem
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public new String ToXml()
        {
            String resultXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //ApplicationContextTypeItem node start
            xmlWriter.WriteStartElement("ApplicationContextTypeItem");

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

            xmlWriter.WriteStartElement("OrganizationFilterWeight");
            xmlWriter.WriteValue(this.OrganizationFilterWeight.HasValue ? this.OrganizationFilterWeight.Value.ToString(CultureInfo.InvariantCulture) : "");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ContainerFilterWeight");
            xmlWriter.WriteValue(this.ContainerFilterWeight.HasValue ? this.ContainerFilterWeight.Value.ToString(CultureInfo.InvariantCulture) : "");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("NodeTypeFilterWeight");
            xmlWriter.WriteValue(this.NodeTypeFilterWeight.HasValue ? this.NodeTypeFilterWeight.Value.ToString(CultureInfo.InvariantCulture) : "");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("RelationshipTypeFilterWeight");
            xmlWriter.WriteValue(this.RelationshipTypeFilterWeight.HasValue ? this.RelationshipTypeFilterWeight.Value.ToString(CultureInfo.InvariantCulture) : "");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("CategoryFilterWeight");
            xmlWriter.WriteValue(this.CategoryFilterWeight.HasValue ? this.CategoryFilterWeight.Value.ToString(CultureInfo.InvariantCulture) : "");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("AttributeFilterWeight");
            xmlWriter.WriteValue(this.AttributeFilterWeight.HasValue ? this.AttributeFilterWeight.Value.ToString(CultureInfo.InvariantCulture) : "");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("EntityFilterWeight");
            xmlWriter.WriteValue(this.EntityFilterWeight.HasValue ? this.EntityFilterWeight.Value.ToString(CultureInfo.InvariantCulture) : "");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("RoleFilterWeight");
            xmlWriter.WriteValue(this.RoleFilterWeight.HasValue ? this.RoleFilterWeight.Value.ToString(CultureInfo.InvariantCulture) : "");
            xmlWriter.WriteEndElement();
            
            xmlWriter.WriteStartElement("UserFilterWeight");
            xmlWriter.WriteValue(this.UserFilterWeight.HasValue ? this.UserFilterWeight.Value.ToString(CultureInfo.InvariantCulture) : "");
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

            xmlWriter.WriteStartElement("LocaleFilterWeight");
            xmlWriter.WriteValue(this.LocaleFilterWeight.HasValue ? this.LocaleFilterWeight.Value.ToString(CultureInfo.InvariantCulture) : "");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("AttributeSourceFilterWeight");
            xmlWriter.WriteValue(this.AttributeSourceFilterWeight.HasValue ? this.AttributeSourceFilterWeight.Value.ToString(CultureInfo.InvariantCulture) : "");
            xmlWriter.WriteEndElement();

            #endregion

            //ApplicationContextTypeItem node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            resultXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return resultXml;
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone ApplicationContextTypeItem
        /// </summary>
        /// <returns>Cloned ApplicationContextTypeItem object</returns>
        public object Clone()
        {
            ApplicationContextTypeItem cloned = (ApplicationContextTypeItem) this.MemberwiseClone();
            return cloned;
        }

        #endregion
    }
}