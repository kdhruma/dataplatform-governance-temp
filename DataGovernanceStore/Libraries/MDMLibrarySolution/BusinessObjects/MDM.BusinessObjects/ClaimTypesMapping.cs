using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents class for claim types mapping
    /// </summary>
    [DataContract]
    public class ClaimTypesMapping : MDMObject, IClaimTypesMapping
    {
        #region Fields

        /// <summary>
        /// Field representing login name of claim types mapping
        /// </summary>
        private String _loginName = String.Empty;

        /// <summary>
        /// Field representing email address of claim types mapping
        /// </summary>
        private String _emailAddress = String.Empty;

        /// <summary>
        /// Field representing external group name of claim types mapping
        /// </summary>
        private String _externalGroupName = String.Empty;

        /// <summary>
        /// Field representing display name of claim types mapping
        /// </summary>
        private String _displayName = String.Empty;

        /// <summary>
        /// Field representing first name of claim types mapping 
        /// </summary>
        private String _firstName = String.Empty;

        /// <summary>
        /// Fields representing last name of claim types mapping 
        /// </summary>
        private String _lastName = String.Empty;

        /// <summary>
        /// Fields representing initials of claim types mapping
        /// </summary>
        private String _initials = String.Empty;

        /// <summary>
        /// Fields representing manager Id of current user
        /// </summary>
        private String _managerId = String.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Specifies login name of claim types mapping
        /// </summary>
        [DataMember]
        public String LoginName
        {
            get
            {
                return _loginName;
            }
            set
            {
                _loginName = value;
            }
        }

        /// <summary>
        /// Specifies email address of claim types mapping
        /// </summary>
        [DataMember]
        public String EmailAddress
        {
            get
            {
                return _emailAddress;
            }
            set
            {
                _emailAddress = value;
            }
        }

        /// <summary>
        /// Specifies external group name of claim types mapping
        /// </summary>
        [DataMember]
        public String ExternalGroupName
        {
            get
            {
                return _externalGroupName;
            }
            set
            {
                _externalGroupName = value;
            }
        }

        /// <summary>
        /// Specifies display name of claim types mapping
        /// </summary>
        [DataMember]
        public String DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                _displayName = value;
            }
        }

        /// <summary>
        /// Specifies first name of claim types mapping
        /// </summary>
        [DataMember]
        public String FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
            }
        }

        /// <summary>
        /// Specifies last name of claim types mapping
        /// </summary>
        [DataMember]
        public String LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
            }
        }

        /// <summary>
        /// Specifies initials of claim types mapping
        /// </summary>
        [DataMember]
        public String Initials
        {
            get
            {
                return _initials;
            }
            set
            {
                _initials = value;
            }
        }

        /// <summary>
        /// Specifies manager id of claim types mapping
        /// </summary>
        [DataMember]
        public String ManagerId
        {
            get
            {
                return this._managerId;
            }
            set
            {
                this._managerId = value;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes claim types mapping object with default parameters
        /// </summary>
        public ClaimTypesMapping() : base() { }

        /// <summary>
        /// Initializes claim types mapping object with the values provided as XML
        /// </summary>
        /// <param name="valuesAsXml">XML string having values</param>
        public ClaimTypesMapping(String valuesAsXml)
        {
            LoadClaimTypesMapping(valuesAsXml);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Get XML representation of ClaimTypesMapping object
        /// </summary>
        /// <returns>XML string representing the ClaimTypesMapping</returns>
        public override String ToXml()
        {
            String claimTypesMappingXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Claim types Mapping Item node start
            xmlWriter.WriteStartElement("ClaimTypesMapping");

            xmlWriter.WriteAttributeString("LoginName", this.LoginName);
            xmlWriter.WriteAttributeString("EmailAddress", this.EmailAddress);
            xmlWriter.WriteAttributeString("ExternalGroupName", this.ExternalGroupName);
            xmlWriter.WriteAttributeString("DisplayName", this.DisplayName);
            xmlWriter.WriteAttributeString("FirstName", this.FirstName);
            xmlWriter.WriteAttributeString("LastName", this.LastName);
            xmlWriter.WriteAttributeString("Initials", this.Initials);
            xmlWriter.WriteAttributeString("ManagerId", this.ManagerId);

            //Claim types Mapping Item node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            claimTypesMappingXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return claimTypesMappingXml;
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
                if (obj is ClaimTypesMapping)
                {
                    ClaimTypesMapping objectToBeCompared = obj as ClaimTypesMapping;

                    if (!this.LoginName.Equals(objectToBeCompared.LoginName))
                    {
                        return false;
                    }

                    if (!this.EmailAddress.Equals(objectToBeCompared.EmailAddress))
                    {
                        return false;
                    }

                    if (!this.ExternalGroupName.Equals(objectToBeCompared.ExternalGroupName))
                    {
                        return false;
                    }

                    if (!this.DisplayName.Equals(objectToBeCompared.DisplayName))
                    {
                        return false;
                    }

                    if (!this.FirstName.Equals(objectToBeCompared.FirstName))
                    {
                        return false;
                    }

                    if (!this.LastName.Equals(objectToBeCompared.LastName))
                    {
                        return false;
                    }

                    if (!this.Initials.Equals(objectToBeCompared.Initials))
                    {
                        return false;
                    }

                    if (!this.ManagerId.Equals(objectToBeCompared.ManagerId))
                    {
                        return false;
                    }

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
            hashCode = base.GetHashCode() ^ this.LoginName.GetHashCode() ^ this.EmailAddress.GetHashCode() ^ this.ExternalGroupName.GetHashCode() 
                        ^ this.DisplayName.GetHashCode();

            return hashCode;
        }

        #endregion Public Methods

        #region Private Methods

        ///<summary>
        /// Load ClaimTypesMapping object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        private void LoadClaimTypesMapping(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ClaimType")
                        {
                            #region Read claim types mapping properties

                            if (reader.HasAttributes == true)
                            {
                                String tagName = String.Empty;

                                if (reader.GetAttribute("Name") != null)
                                {
                                    tagName = reader.GetAttribute("Name");
                                }

                                String value = reader.ReadElementContentAsString();

                                if (tagName == "LoginName")
                                {
                                    this.LoginName = value;
                                }
                                else if (tagName == "EmailAddress")
                                {
                                    this.EmailAddress = value;
                                }
                                else if (tagName == "ExternalGroupName")
                                {
                                    this.ExternalGroupName = value;
                                }
                                else if (tagName == "DisplayName")
                                {
                                    this.DisplayName = value;
                                }
                                else if (tagName == "FirstName")
                                {
                                    this.FirstName = value;
                                }
                                else if (tagName == "LastName")
                                {
                                    this.LastName = value;
                                }
                                else if (tagName == "Initials")
                                {
                                    this.Initials = value;
                                }
                                else if (tagName == "ManagerId")
                                {
                                    this.ManagerId = value;
                                }
                            }

                            #endregion Read claim types mapping properties
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
