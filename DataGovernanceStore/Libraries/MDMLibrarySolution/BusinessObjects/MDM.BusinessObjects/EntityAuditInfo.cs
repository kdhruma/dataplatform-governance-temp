using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;    

    /// <summary>
    /// Represents class to represent entity audit information
    /// </summary>
    [DataContract]
    public class EntityAuditInfo : AuditInfo, IEntityAuditInfo
    {
        #region Fields

        /// <summary>
        /// Entity id for current audit info
        /// </summary>
        private Int64 _entityId = -1;

        /// <summary>
        /// Attribute id for current audit info
        /// </summary>
        private Int32 _attributeId = -1;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Parameter Less Constructor
        /// </summary>
        public EntityAuditInfo()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of a AuditInfo Instance</param>
        public EntityAuditInfo(Int64 id)
            :base(id)
        {
        }

        /// <summary>
        /// Constructor with All EntityAuditInfo properties as parameter.
        /// </summary>
        /// <param name="attributeId">Id of attribute for which current audit is populated</param>
        /// <param name="entityId">Id of entity for which current audit is populated</param>
        /// <param name="locale">Locale for current audit info</param>
        /// <param name="id">Indicates Identity if AuditInfo Instance</param>
        /// <param name="programName">Indicates programName in AuditInfo</param>
        /// <param name="userLogin">Indicates Logged in User</param>
        /// <param name="changeDateTime">Indicates at which time AuditInfo Instance has been changed</param>
        /// <param name="action">Indicates Action on AuditInfo instance</param>
        public EntityAuditInfo(Int64 entityId, Int32 attributeId, Int64 id, String programName, String userLogin, DateTime changeDateTime, ObjectAction action, LocaleEnum locale)
            : base(id, programName, userLogin, changeDateTime, action, locale)
        {
            this._entityId = entityId;
            this._attributeId = attributeId;
        }

        /// <summary>
        /// Create EntityAuditInfo object with property values xml as input parameter
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// <AuditInfo 
        ///     Id="001" 
        ///     ProgramName="BusinessRule.100.1" 
        ///     UserLogin="cfadmin" 
        ///     ChangeDateTime="" 
        ///     Action="Read" 
        ///     Locale="en-WW">    
        /// </AuditInfo>
        /// ]]>
        /// </example>
        /// <param name="valueAsXml">XML representation for AuditInfo from which object is to be created</param>
        public EntityAuditInfo(String valueAsXml)
        {
            LoadEntityAuditInfo(valueAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Entity id for current audit info
        /// </summary>
        [DataMember]
        public Int64 EntityId
        {
            get { return _entityId; }
            set { _entityId = value; }
        }

        /// <summary>
        /// Attribute id for current audit info
        /// </summary>
        [DataMember]
        public Int32 AttributeId
        {
            get { return _attributeId; }
            set { _attributeId = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Populate current object from incoming XML
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// <EntityAuditInfo 
        ///     Id="001" 
        ///     EntityId = "1001"
        ///     AttributeId = "4001"
        ///     ProgramName="BusinessRule.100.1" 
        ///     UserLogin="cfadmin" 
        ///     ChangeDateTime="" 
        ///     Action="Read" 
        ///     Locale="en-WW">    
        /// </EntityAuditInfo>
        /// ]]>
        /// </example>
        /// <param name="valuesAsXml">XML representation for AuditInfo from which object is to be created</param>
        public void LoadEntityAuditInfo(String valuesAsXml)
        {
            #region Sample Xml
            /* <EntityAuditInfo 
             *           EntityId = "1001"
             *           AttributeId = "4001"
                         Id="001" 
                         ProgramName="BusinessRule.100.1" 
                         UserLogin="cfadmin" 
                         ChangeDateTime="" 
                         Action="Read" 
                         Locale="en-WW">    
                   </EntityAuditInfo>
                 */
            #endregion

            if(!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while(!reader.EOF)
                    {
                        if(reader.NodeType == XmlNodeType.Element && reader.Name == "EntityAuditInfo")
                        {
                            #region Read AuditInfo Properties

                            if(reader.HasAttributes)
                            {
                                if(reader.MoveToAttribute("Id"))
                                {
                                    this.Id = reader.ReadContentAsLong();
                                }

                                if(reader.MoveToAttribute("EntityId"))
                                {
                                    this.EntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);
                                }

                                if(reader.MoveToAttribute("AttributeId"))
                                {
                                    this.AttributeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                                }

                                if(reader.MoveToAttribute("ProgramName"))
                                {
                                    this.ProgramName = reader.ReadContentAsString();
                                }

                                if(reader.MoveToAttribute("UserLogin"))
                                {
                                    this.UserLogin = reader.ReadContentAsString();
                                }
                                if(reader.MoveToAttribute("ChangeDateTime"))
                                {
                                    this.ChangeDateTime = ValueTypeHelper.ConvertToDateTime(reader.ReadContentAsString());
                                }
                                if(reader.MoveToAttribute("Action"))
                                {
                                    ObjectAction action = ObjectAction.Read;
                                    Enum.TryParse(reader.ReadContentAsString(), true, out action);
                                    this.Action = action;
                                }
                                if(reader.MoveToAttribute("Locale"))
                                {
                                    LocaleEnum locale = LocaleEnum.UnKnown;
                                    Enum.TryParse(reader.ReadContentAsString(), out locale);
                                    this.Locale = locale;
                                }
                            }

                            #endregion
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
                    if(reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }

        #region IEntityAuditInfo methods

        /// <summary>
        /// Get Xml representation of AuditInfo object
        /// </summary>
        /// <returns>XML String of AuditInfo Object</returns>
        public override String ToXml()
        {
            String auditInfoXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //AuditInfo node start
            xmlWriter.WriteStartElement("EntityAuditInfo");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("EntityId", this.EntityId.ToString());
            xmlWriter.WriteAttributeString("AttributeId", this.AttributeId.ToString());
            xmlWriter.WriteAttributeString("ProgramName", this.ProgramName);
            xmlWriter.WriteAttributeString("UserLogin", this.UserLogin);
            xmlWriter.WriteAttributeString("ChangeDateTime", this.ChangeDateTime.ToString());
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());
            xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());

            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            auditInfoXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return auditInfoXml;
        }

        /// <summary>
        /// Get Xml representation of Menu object
        /// </summary>
        /// <param name="objectSerialization">Indicates the serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            String auditInfoXml = String.Empty;

            if(objectSerialization == ObjectSerialization.Full)
            {
                auditInfoXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //AuditInfo node start
                xmlWriter.WriteStartElement("EntityAuditInfo");

                xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                xmlWriter.WriteAttributeString("EntityId", this.EntityId.ToString());
                xmlWriter.WriteAttributeString("AttributeId", this.AttributeId.ToString());
                xmlWriter.WriteAttributeString("ProgramName", this.ProgramName);
                xmlWriter.WriteAttributeString("UserLogin", this.UserLogin);
                xmlWriter.WriteAttributeString("ChangeDateTime", this.ChangeDateTime.ToString());
                xmlWriter.WriteAttributeString("Action", this.Action.ToString());
                xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());

                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                auditInfoXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            return auditInfoXml;
        }

        #endregion IEntityAuditInfo methods

        #endregion Methods
    }
}
