using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.BusinessObjects.Interfaces;

    /// <summary>
    /// Represents the password reset request object
    /// </summary>
    [DataContract]
    public class PasswordResetRequest : IPasswordResetRequest
    {
        #region Field

        /// <summary>
        /// Field for the Id of an object
        /// </summary>
        private Int32 _id = -1;

        /// <summary>
        /// Field for the Id of MDMCenter user
        /// </summary>
        private Int32 _securityUserId = -1;

        /// <summary>
        /// Field for the token generated for a password reset request
        /// </summary>
        private String _token = String.Empty;

        /// <summary>
        /// Field for the date time stamp for a password reset request 
        /// </summary>
        private DateTime _requestedDateTime;

        /// <summary>
        /// Field to denote if the password is reset for the request
        /// </summary>
        private Boolean _isPasswordReset = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public PasswordResetRequest()
        { }

        /// <summary>
        /// Constructor with XML having values of object. Populate current object using XML
        /// </summary>
        /// <param name="valueAsXml">Indicates XML having xml value</param>
        public PasswordResetRequest(String valueAsXml)
        {
            LoadFromXml(valueAsXml);
        }

        #endregion

        #region Property

        /// <summary>
        /// Indicates the Id of an object
        /// </summary>
        [DataMember]
        public Int32 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Indicates the Id of MDMCenter user
        /// </summary>
        [DataMember]
        public Int32 SecurityUserId
        {
            get { return _securityUserId; }
            set { _securityUserId = value; }
        }

        /// <summary>
        /// Indicates the token generated for a password reset request
        /// </summary>
        [DataMember]
        public String Token
        {
            get { return _token; }
            set { _token = value; }
        }

        /// <summary>
        /// Indicates the date time stamp for a password reset request
        /// </summary>
        [DataMember]
        public DateTime RequestedDateTime
        {
            get { return _requestedDateTime; }
            set { _requestedDateTime = value; }
        }

        /// <summary>
        /// Indicates if the password is reset for the request
        /// </summary>
        [DataMember]
        public Boolean IsPasswordReset
        {
            get { return _isPasswordReset; }
            set { _isPasswordReset = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Attribute object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        /// <example>
        /// Sample XML
        /// <para>
        ///  <![CDATA[
        /// <PasswordResetRequest Id="1" SecurityUserId="1" Token="ewrprsfsflffkndfnsff" RequestedDateTime="10/10/2014 10:10" IsPasswordReset="false"></PasswordResetRequest>
        ///  ]]>
        /// </para>
        /// </example>
        public String ToXml()
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //PasswordResetRequest node start
            xmlWriter.WriteStartElement("PasswordResetRequest");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("SecurityUserId", this.SecurityUserId.ToString());
            xmlWriter.WriteAttributeString("Token", this.Token);
            xmlWriter.WriteAttributeString("RequestedDateTime", this.RequestedDateTime.ToString());
            xmlWriter.WriteAttributeString("IsPasswordReset", this.IsPasswordReset.ToString());

            //PasswordResetRequest node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Populate current object from incoming XML
        /// </summary>
        /// /// <example>
        /// Sample XML
        /// <para>
        ///  <![CDATA[
        /// <PasswordResetRequest Id="1" SecurityUserId="1" Token="ewrprsfsflffkndfnsff" RequestedDateTime="10/10/2014 10:10" IsPasswordReset="false"></PasswordResetRequest>
        ///  ]]>
        /// </para>
        /// </example>
        /// <param name="valueAsXml">XML representation for attribute from which object is to be created</param>
        private void LoadFromXml(String valueAsXml)
        {
            #region Sample XML

            /* 
             * <PasswordResetRequest Id="1" SecurityUserId="1" Token="ewrprsfsflffkndfnsff" RequestedDateTime="10/10/2014 10:10" IsPasswordReset="false">
             * </PasswordResetRequest>
            */

            #endregion

            if (!String.IsNullOrWhiteSpace(valueAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "PasswordResetRequest")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("SecurityUserId"))
                                {
                                    this.SecurityUserId = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("Token"))
                                {
                                    this.Token = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("RequestedDateTime"))
                                {
                                    this.RequestedDateTime = reader.ReadContentAsDateTime();
                                }

                                if (reader.MoveToAttribute("IsPasswordReset"))
                                {
                                    this.IsPasswordReset = reader.ReadContentAsBoolean();
                                }
                            }
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
    }
}
