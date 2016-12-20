using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies role mapping for SAML
    /// </summary>
    [DataContract]
    public class RoleMapping : MDMObject, IRoleMapping
    {
        #region Fields

        /// <summary>
        /// Field representing external role of role mapping
        /// </summary>
        private String _externalRole = String.Empty;

        /// <summary>
        /// Field representing collection of mdm roles of role mapping
        /// </summary>
        private Collection<String> _mdmRoles = new Collection<String>();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Specifies external role of role mapping
        /// </summary>
        [DataMember]
        public String ExternalRole
        {
            get
            {
                return _externalRole;
            }
            set
            {
                _externalRole = value;
            }
        }

        /// <summary>
        /// Specifies mdm roles of role mapping
        /// </summary>
        [DataMember]
        public Collection<String> MDMRoles
        {
            get
            {
                return _mdmRoles;
            }
            set
            {
                _mdmRoles = value;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes Role mapping object with default parameters
        /// </summary>
        public RoleMapping() : base() { }

        /// <summary>
        /// Initializes Role mapping object with the values provided as Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml string having values</param>
        public RoleMapping(String valuesAsXml)
        {
            LoadRoleMapping(valuesAsXml);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Get Xml representation of RelationshipDenormProcessingSetting object
        /// </summary>
        /// <returns>Xml string representing the RelationshipDenormProcessingSetting</returns>
        public override String ToXml()
        {
            String roleMappingXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Role Mapping Item node start
            xmlWriter.WriteStartElement("RoleMapping");

            #region write external role

            xmlWriter.WriteStartElement("ExternalRole");
            xmlWriter.WriteAttributeString("Name", this.ExternalRole);

            foreach(String mdmRole in this.MDMRoles)
            {
                xmlWriter.WriteStartElement("MDMRole");
                xmlWriter.WriteValue(mdmRole);
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();

            #endregion write external role

            //Role Mapping Item node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            roleMappingXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return roleMappingXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is RoleMapping)
                {
                    RoleMapping objectToBeCompared = obj as RoleMapping;

                    if (!this.ExternalRole.Equals(objectToBeCompared.ExternalRole))
                        return false;

                    if (!this.MDMRoles.Equals(objectToBeCompared.MDMRoles))
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
            hashCode = base.GetHashCode() ^ this.ExternalRole.GetHashCode() ^ this.MDMRoles.GetHashCode();

            return hashCode;
        }

        #endregion Public Methods

        #region Private Methods

        ///<summary>
        /// Load RoleMapping object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML representation of RoleMapping object</param>        
        private void LoadRoleMapping(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExternalRole")
                        {
                            #region Read External Role
                            
                            if (reader.HasAttributes == true)
							{
                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.ExternalRole = reader.ReadContentAsString();
                                }
							}
                            
                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRole")
                        {
                            String mdmRole = reader.ReadElementContentAsString();
                            if (mdmRole != null)
                            {
                                this.MDMRoles.Add(mdmRole);
                            }
                        }
                        else
                        {
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
    }
}
