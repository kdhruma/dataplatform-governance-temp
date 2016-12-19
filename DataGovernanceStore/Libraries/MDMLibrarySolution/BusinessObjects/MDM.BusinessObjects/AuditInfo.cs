using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
   
    /// <summary>
    /// Represents class for audit information about MDM object
    /// </summary>
    [DataContract]
    public class AuditInfo : MDMObject, IAuditInfo
    {
        #region Fields

        /// <summary>
        /// Field Denoting Id of ParentMenu
        /// </summary>
        private Int64 _id = 0;

        /// <summary>
        /// Field Denoting Sequence of Menu
        /// </summary>
        private String _userLogin = String.Empty;

        /// <summary>
        /// Field Denoting Link of the Menu
        /// </summary>
        private DateTime _changeDateTime = DateTime.Now;

        #endregion

        #region Constructors

         /// <summary>
        /// Parameter Less Constructor
        /// </summary>
        public AuditInfo()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of a AuditInfo Instance</param>
        public AuditInfo(Int64 id)
            :base()
        {
            this._id = id;
        }

        /// <summary>
        /// Constructor with All AuditInfo properties as parameter.
        /// </summary>
        /// <param name="id">Indicates Identity if AuditInfo Instance</param>
        /// <param name="programName">Indicates programName in AuditInfo</param>
        /// <param name="userLogin">Indicates Logged in User</param>
        /// <param name="changeDateTime">Indicates at which time AuditInfo Instance has been changed</param>
        /// <param name="action">Indicates Action on AuditInfo instance</param>
        /// <param name="locale">Indicates locale of AuditInfo instance</param>
        public AuditInfo(Int64 id, String programName, String userLogin, DateTime changeDateTime, ObjectAction action, LocaleEnum locale)
            : base()
        {
            this._id = id;
            this.ProgramName = programName;
            this._userLogin = userLogin;
            this._changeDateTime = changeDateTime;
            this.Action = action;
            this.Locale = locale;
        }

        /// <summary>
        /// Create AuditInfo object with property values xml as input parameter
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
        public AuditInfo(String valueAsXml)
        {
            LoadAuditInfo(valueAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property Denoting Id of AuditInfo
        /// </summary>
        [DataMember]
        public new Int64 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Property Denoting userLogin 
        /// </summary>
        [DataMember]
        public String UserLogin 
        {
            get { return _userLogin; }
            set { _userLogin = value; }
        }

        /// <summary>
        /// Property Denoting ChangeDateTime
        /// </summary>
        [DataMember]
        public DateTime ChangeDateTime
        {
            get { return _changeDateTime; }
            set { _changeDateTime = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (base.Equals(obj))
            {
                if (obj != null && obj is AuditInfo)
                {
                    AuditInfo objectToBeCompared = obj as AuditInfo;

                    if (this.Id != objectToBeCompared.Id)
                        return false;

                    if (this.UserLogin != objectToBeCompared.UserLogin)
                        return false;

                    if (this.ChangeDateTime != objectToBeCompared.ChangeDateTime)
                        return false;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            Int32 hashCode = base.GetHashCode() ^ this.Id.GetHashCode() ^ this.UserLogin.GetHashCode() ^ this.ChangeDateTime.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Populate current object from incoming XML
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
        /// <param name="valuesAsXml">XML representation for AuditInfo from which object is to be created</param>
        public void LoadAuditInfo(String valuesAsXml)
        {
            #region Sample Xml
                /* <AuditInfo 
                         Id="001" 
                         ProgramName="BusinessRule.100.1" 
                         UserLogin="cfadmin" 
                         ChangeDateTime="" 
                         Action="Read" 
                         Locale="en-WW">    
                   </AuditInfo>
                 */
            #endregion

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "AuditInfo")
                        {
                            #region Read AuditInfo Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = reader.ReadContentAsLong();
                                }

                                if (reader.MoveToAttribute("ProgramName"))
                                {
                                    this.ProgramName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("UserLogin"))
                                {
                                    this.UserLogin = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("ChangeDateTime"))
                                {
                                    this.ChangeDateTime = ValueTypeHelper.ConvertToDateTime(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("Action"))
                                {
                                    ObjectAction action = ObjectAction.Read;
                                    Enum.TryParse(reader.ReadContentAsString(), true, out action);
                                    this.Action = action;
                                }
                                if (reader.MoveToAttribute("Locale"))
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
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }

        #endregion

        #region IAuditInfo Methods

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
            xmlWriter.WriteStartElement("AuditInfo");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
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
        /// Get Xml representation of Audit Info object
        /// </summary>
        /// <param name="objectSerialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            String auditInfoXml = String.Empty;

            if (objectSerialization == ObjectSerialization.Full)
            {
                auditInfoXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //AuditInfo node start
                xmlWriter.WriteStartElement("AuditInfo");

                xmlWriter.WriteAttributeString("Id", this.Id.ToString());
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

        #endregion
    }
}

