using System;
using System.Xml;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.IO;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies class for Security Permission Definition
    /// </summary>
    [DataContract]
    public class SecurityPermissionDefinition : MDMObject, ISecurityPermissionDefinition
    {
        #region Fields

        /// <summary>
        /// Field denoting application Context Id.
        /// </summary>
        private Int32 _applicationContextId = 0;

        /// <summary>
        /// Field denoting application context instance.
        /// </summary>
        private ApplicationContext _applicationContext = new ApplicationContext();

        /// <summary>
        /// Field denoting attribute values used for permissions.
        /// </summary>
        private Collection<String> _permissionValues = new Collection<String>();

        /// <summary>
        /// Field denoting permission actions.
        /// </summary>
        private Collection<UserAction> _permissionSet = new Collection<UserAction>();

        /// <summary>   
        /// Field denoting context Weightage
        /// </summary>
        private Int32 _contextWeightage = 0;

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public SecurityPermissionDefinition()
            : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public SecurityPermissionDefinition(String valuesAsXml)
        {
            LoadSecurityPermissionDefinition(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting Application Context Id
        /// </summary>
        [DataMember]
        public Int32 ApplicationContextId
        {
            get
            {
                return _applicationContextId;
            }
            set
            {
                _applicationContextId = value;
            }
        }

        /// <summary>
        /// Property denoting Application Context instance.
        /// </summary>
        [DataMember]
        public ApplicationContext ApplicationContext
        {
            get
            {
                return _applicationContext;
            }
            set
            {
                _applicationContext = value;
            }
        }

        /// <summary>
        /// Property denoting attribute values used for permissions.
        /// </summary>
        [DataMember]
        public Collection<String> PermissionValues
        {
            get
            {
                return _permissionValues;
            }
            set
            {
                _permissionValues = value;
            }
        }

        /// <summary>
        /// Property denoting permission action.
        /// </summary>
        [DataMember]
        public new Collection<UserAction> PermissionSet
        {
            get
            {
                return _permissionSet;
            }
            set
            {
                _permissionSet = value;
            }
        }

        /// <summary>
        /// Property denoting context weightage
        /// </summary>
        [DataMember]
        public Int32 ContextWeightage
        {
            get
            {
                return _contextWeightage;
            }
            set
            {
                _contextWeightage = value;
            }
        }
        #endregion

        #region Methods

        #region Public Methods

        #region Load Methods

        /// <summary>
        /// Load SecurityPermission object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        ///     <para>
        ///     <![CDATA[
        ///         <SecurityPermissionDefinition
        ///             Name="Edit And View Security Permissions" 
        ///             LongName="Edit And View Security Permissions"
        ///             RoleId="55"
        ///             RoleName="Vendor01"
        ///             PermissionValues="1"
        ///             PermissionSet="Edit,View">
        ///             <ApplicationContext
        ///                 OrganizationId="1"
        ///                 OrganizationName="river Works Workspace"
        ///                 ContainerId="4"
        ///                 ContainerName="staging Master"
        ///                 AttributeId="4053"
        ///                 AttributeName="vendor Name">
        ///             </ApplicationContext
        ///         </SecurityPermissionDefinition>
        ///     ]]>    
        ///     </para>
        /// </example>
        public void LoadSecurityPermissionDefinition(String valuesAsXml)
        {
            #region Sample Xml
           
                   //<SecurityPermissionDefinition
                   //      Id="-1"
                   //      Name="Edit And View Security Permissions" 
                   //      LongName="Edit And View Security Permissions"
                   //      RoleId="55"
                   //      RoleName="Vendor01"
                   //      PermissionValues="1"
                   //      PermissionSet="Edit,View"
                   //      ContextWeightage="11">
                   //      <ApplicationContext
                   //          OrganizationId="1"
                   //          OrganizationName="river Works Workspace"
                   //          ContainerId="4"
                   //          ContainerName="staging Master"
                   //          AttributeId="4053"
                   //          AttributeName="vendor Name">
                   //      </ApplicationContext
                   //  </SecurityPermissionDefinition>
            #endregion

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "SecurityPermissionDefinition")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("LongName"))
                                {
                                    this.LongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ApplicationContextId"))
                                {
                                    this.ApplicationContextId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("PermissionValues"))
                                {
                                    this.PermissionValues = ValueTypeHelper.SplitStringToStringCollection(reader.ReadContentAsString(), ',');
                                }

                                if (reader.MoveToAttribute("PermissionSet"))
                                {
                                    Collection<UserAction> permissionSet = null;
                                    String strPermissionSet = reader.ReadContentAsString();

                                    if (!String.IsNullOrWhiteSpace(strPermissionSet))
                                    {
                                        permissionSet = new Collection<UserAction>();

                                        String[] strOutput = strPermissionSet.Split(',');
                                        foreach (String s in strOutput)
                                        {
                                            UserAction userAction = UserAction.Unknown;
                                            Enum.TryParse<UserAction>(s, out userAction);

                                            permissionSet.Add(userAction);
                                        }
                                    }

                                    this.PermissionSet = permissionSet;
                                }
                                if (reader.MoveToAttribute("ContextWeightage"))
                                {
                                    this.ContextWeightage = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                reader.Read();
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ApplicationContext")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("OrganizationId"))
                                {
                                    this.ApplicationContext.OrganizationId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("OrganizationName"))
                                {
                                    this.ApplicationContext.OrganizationName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("ContainerId"))
                                {
                                    this.ApplicationContext.ContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("ContainerName"))
                                {
                                    this.ApplicationContext.ContainerName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("AttributeId"))
                                {
                                    this.ApplicationContext.AttributeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("AttributeName"))
                                {
                                    this.ApplicationContext.AttributeName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("RoleId"))
                                {
                                    this.ApplicationContext.RoleId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("RoleName"))
                                {
                                    this.ApplicationContext.RoleName = reader.ReadContentAsString();
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

        #region ToXml Methods

        /// <summary>
        /// Get Xml representation of SecurityPermissionDefinition
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml()
        {
            String returnXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //SecurityPermissionDefinition node start
            xmlWriter.WriteStartElement("SecurityPermissionDefinition");

            #region Write SecurityPermissionDefinition attributes

            xmlWriter.WriteAttributeString("Name", this.Name.ToString());
            xmlWriter.WriteAttributeString("LongName", this.LongName.ToString());
            xmlWriter.WriteAttributeString("ApplicationContextId", this.ApplicationContextId.ToString());

            String permissionValues = String.Empty;

            if (this.PermissionValues != null && this.PermissionValues.Count > 0)
            {
                permissionValues = ValueTypeHelper.JoinCollection(this.PermissionValues, ",");
            }

            xmlWriter.WriteAttributeString("PermissionValues", permissionValues);

            String strPermissionSet = String.Empty;
            if (this.PermissionSet != null && this.PermissionSet.Count > 0)
            {
                foreach (UserAction userAction in this.PermissionSet)
                {
                    if (!String.IsNullOrWhiteSpace(strPermissionSet))
                    {
                        strPermissionSet += ",";
                    }

                    strPermissionSet += userAction.ToString();
                }
            }
            xmlWriter.WriteAttributeString("PermissionSet", strPermissionSet);

            xmlWriter.WriteAttributeString("ContextWeightage", this.ContextWeightage.ToString());

            #endregion

            #region Write application context

            xmlWriter.WriteStartElement("ApplicationContext");

            if (this.ApplicationContext != null)
            {
                xmlWriter.WriteAttributeString("OrganizationId", this.ApplicationContext.OrganizationId.ToString());
                xmlWriter.WriteAttributeString("OrganizationName", this.ApplicationContext.OrganizationName.ToString());
                xmlWriter.WriteAttributeString("ContainerId", this.ApplicationContext.ContainerId.ToString());
                xmlWriter.WriteAttributeString("ContainerName", this.ApplicationContext.ContainerName.ToString());
                xmlWriter.WriteAttributeString("AttributeId", this.ApplicationContext.AttributeId.ToString());
                xmlWriter.WriteAttributeString("AttributeName", this.ApplicationContext.AttributeName.ToString());

                xmlWriter.WriteAttributeString("EntityTypeId", this.ApplicationContext.EntityTypeId.ToString());
                xmlWriter.WriteAttributeString("RelationshipTypeId", this.ApplicationContext.RelationshipTypeId.ToString());
                xmlWriter.WriteAttributeString("CategoryId", this.ApplicationContext.CategoryId.ToString());
                xmlWriter.WriteAttributeString("EntityId", this.ApplicationContext.EntityId.ToString());
                xmlWriter.WriteAttributeString("UserId", this.ApplicationContext.UserId.ToString());
                xmlWriter.WriteAttributeString("RoleId", this.ApplicationContext.RoleId.ToString());
                xmlWriter.WriteAttributeString("RoleName", this.ApplicationContext.RoleName.ToString());
            }

            xmlWriter.WriteEndElement(); //ApplicationContext

            #endregion Write application context

            xmlWriter.WriteEndElement();//SecurityPermissionDefinition

            xmlWriter.Flush();

            //get the actual XML
            returnXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of SecurityPermissionDefinition
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            if (objectSerialization == ObjectSerialization.Full)
            {
                return this.ToXml();
            }
            else
            {
                String returnXml = String.Empty;

                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //SecurityPermissionDefinition node start
                xmlWriter.WriteStartElement("SecurityPermissionDefinition");

                #region Write SecurityPermissionDefinition attributes

                xmlWriter.WriteAttributeString("Name", this.Name.ToString());
                xmlWriter.WriteAttributeString("LongName", this.LongName.ToString());
                xmlWriter.WriteAttributeString("ApplicationContextId", this.ApplicationContextId.ToString());

                String permissionValues = String.Empty;
                permissionValues = ValueTypeHelper.JoinCollection(this.PermissionValues, ",");
                xmlWriter.WriteAttributeString("PermissionValues", permissionValues);

                String strPermissionSet = String.Empty;
                if (this.PermissionSet != null && this.PermissionSet.Count > 0)
                {
                    foreach (UserAction userAction in this.PermissionSet)
                    {
                        if (!String.IsNullOrWhiteSpace(strPermissionSet))
                        {
                            strPermissionSet += ",";
                        }

                        strPermissionSet += userAction.ToString();
                    }
                }
                xmlWriter.WriteAttributeString("PermissionSet", strPermissionSet);

                xmlWriter.WriteAttributeString("ContextWeightage", this.ContextWeightage.ToString());

                #endregion

                #region Write application context

                xmlWriter.WriteStartElement("ApplicationContext");

                if (this.ApplicationContext != null)
                {
                    xmlWriter.WriteAttributeString("OrganizationId", this.ApplicationContext.OrganizationId.ToString());
                    xmlWriter.WriteAttributeString("OrganizationName", this.ApplicationContext.OrganizationName.ToString());
                    xmlWriter.WriteAttributeString("ContainerId", this.ApplicationContext.ContainerId.ToString());
                    xmlWriter.WriteAttributeString("ContainerName", this.ApplicationContext.ContainerName.ToString());
                    xmlWriter.WriteAttributeString("AttributeId", this.ApplicationContext.AttributeId.ToString());
                    xmlWriter.WriteAttributeString("AttributeName", this.ApplicationContext.AttributeName.ToString());
                    xmlWriter.WriteAttributeString("RoleId", this.ApplicationContext.RoleId.ToString());
                    xmlWriter.WriteAttributeString("RoleName", this.ApplicationContext.RoleName.ToString());
                }

                xmlWriter.WriteEndElement(); //ApplicationContext

                #endregion Write application context

                xmlWriter.WriteEndElement();//SecurityPermissionDefinition

                xmlWriter.Flush();

                //get the actual XML
                returnXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();

                return returnXml;
            }
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is SecurityPermissionDefinition)
                {
                    SecurityPermissionDefinition objectToBeCompared = obj as SecurityPermissionDefinition;

                    if (this.ApplicationContextId != objectToBeCompared.ApplicationContextId)
                        return false;

                    if (!this.ApplicationContext.Equals(objectToBeCompared.ApplicationContext))
                        return false;

                    if (this.PermissionValues != objectToBeCompared.PermissionValues)
                        return false;

                    if (this.PermissionSet != objectToBeCompared.PermissionSet)
                        return false;

                    if (this.ContextWeightage != objectToBeCompared.ContextWeightage)
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
            Int32 hashCode = 0;

            hashCode = base.GetHashCode() ^ this.ApplicationContextId.GetHashCode() ^ this.ApplicationContext.GetHashCode() ^
                       this.PermissionValues.GetHashCode() ^ this.PermissionSet.GetHashCode() ^ this.ContextWeightage.GetHashCode();

            return hashCode;
        }

        #endregion

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}
