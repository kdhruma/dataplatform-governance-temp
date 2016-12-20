using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using ProtoBuf;

namespace MDM.BusinessObjects.Diagnostics
{
    using MDM.Core;
    using MDM.Core.Extensions;
    using MDM.Interfaces.Diagnostics;

    /// <summary>
    /// Specifies SecurityContext filter
    /// </summary>
    [DataContract]
    [ProtoBuf.ProtoContract]
    [Serializable()]
    public class SecurityContextFilter : ISecurityContextFilter
    {
        #region Fields

        [DataMember]
        [ProtoMember(1)]
        private Collection<Int32> _userIdList = new Collection<Int32>();

        [DataMember]
        [ProtoMember(2)]
        private Collection<String> _userLoginNameList = new Collection<String>();

        [DataMember]
        [ProtoMember(3)]
        private Collection<Int32> _userRoleIdList = new Collection<Int32>();

        [DataMember]
        [ProtoMember(4)]
        private Collection<String> _userRoleNameList = new Collection<String>();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs <see cref="SecurityContextFilter"/> using specified instance data
        /// </summary>
        public SecurityContextFilter(SecurityContextFilter source)
        {
            this._userIdList = source._userIdList.CopyCollection();
            this._userLoginNameList = source._userLoginNameList.CloneCollection();
            this._userRoleIdList = source._userRoleIdList.CopyCollection();
            this._userRoleNameList = source._userRoleNameList.CloneCollection();
        }

        /// <summary>
        /// Constructs <see cref="SecurityContextFilter"/> using data provided as XML
        /// </summary>
        /// <param name="securityContextFilterAsXml">XML string having data</param>
        public SecurityContextFilter(String securityContextFilterAsXml)
        {
            LoadFromXml(securityContextFilterAsXml);
        }

        /// <summary>
        /// Prameterless constructor
        /// </summary>
        public SecurityContextFilter()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Specifies User Id filter
        /// </summary>
        public Collection<Int32> UserIdList
        {
            get { return _userIdList; }
            set { _userIdList = value; }
        }

        /// <summary>
        /// Specifies User Login Name filter
        /// </summary>
        public Collection<String> UserLoginNameList
        {
            get { return _userLoginNameList; }
            set { _userLoginNameList = value; }
        }

        /// <summary>
        /// Specifies User Role Id filter
        /// </summary>
        public Collection<Int32> UserRoleIdList
        {
            get { return _userRoleIdList; }
            set { _userRoleIdList = value; }
        }

        /// <summary>
        /// Specifies User Role Name filter
        /// </summary>
        public Collection<String> UserRoleNameList
        {
            get { return _userRoleNameList; }
            set { _userRoleNameList = value; }
        }

        #endregion

        #region Public Methods
        
        /// <summary>
        /// Adds <see cref="SecurityContext"/> values into filter collections
        /// </summary>
        /// <param name="securityContext"><see cref="SecurityContext"/> instance which data should be added into filter collections</param>
        public void AddSecurityContextData(SecurityContext securityContext)
        {
            if (securityContext != null)
            {
                if (securityContext.UserId > 0 && !UserIdList.Contains(securityContext.UserId))
                {
                    UserIdList.Add(securityContext.UserId);
                }
                if (!String.IsNullOrWhiteSpace(securityContext.UserLoginName) && !UserLoginNameList.Contains(securityContext.UserLoginName))
                {
                    UserLoginNameList.Add(securityContext.UserLoginName);
                }

                if (securityContext.UserRoleId > 0 && !UserRoleIdList.Contains(securityContext.UserRoleId))
                {
                    UserRoleIdList.Add(securityContext.UserRoleId);
                }
                if (!String.IsNullOrWhiteSpace(securityContext.UserRoleName) && !UserRoleNameList.Contains(securityContext.UserRoleName))
                {
                    UserRoleNameList.Add(securityContext.UserRoleName);
                }
            }
        }

        /// <summary>
        /// Returns <see cref="SecurityContextFilter"/> in Xml format
        /// </summary>
        /// <returns>String representation of current <see cref="SecurityContextFilter"/></returns>
        public String ToXml()
        {
            String result = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            // SecurityContextFilter node start
            xmlWriter.WriteStartElement("SecurityContextFilter");

            xmlWriter.WriteStartElement("UserIdList");
            if (this.UserIdList != null && this.UserIdList.Count > 0)
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.UserIdList, "|"));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("UserLoginNameList");
            if (this.UserLoginNameList != null && this.UserLoginNameList.Count > 0)
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.UserLoginNameList, "|"));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("UserRoleIdList");
            if (this.UserRoleIdList != null && this.UserRoleIdList.Count > 0)
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.UserRoleIdList, "|"));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("UserRoleNameList");
            if (this.UserRoleNameList != null && this.UserRoleNameList.Count > 0)
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.UserRoleNameList, "|"));
            xmlWriter.WriteEndElement();

            // SecurityContextFilter node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            // Get the actual XML
            result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Clones <see cref="SecurityContextFilter"/>
        /// </summary>
        public object Clone()
        {
            return
                new SecurityContextFilter(this);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads the <see cref="SecurityContextFilter"/> with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        private void LoadFromXml(String valuesAsXml)
        {
            #region Sample Xml

            /*
            <SecurityContextFilter>
                <UserIdList>1|2|3|...|...</UserIdList>
	            <UserLoginNameList>AccessoriesVendor|...</UserLoginNameList>
	            <UserRoleIdList />
	            <UserRoleNameList />
            </SecurityContextFilter>
            */

            #endregion

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType != XmlNodeType.Element)
                    {
                        reader.Read();
                        continue;
                    }

                    String data = String.Empty;

                    if (reader.Name == "UserIdList")
                    {
                        data = reader.ReadElementContentAsString();
                        if (!String.IsNullOrWhiteSpace(data))
                            this.UserIdList = ValueTypeHelper.SplitStringToIntCollection(data, '|');
                    }
                    else if (reader.Name == "UserLoginNameList")
                    {
                        data = reader.ReadElementContentAsString();
                        if (!String.IsNullOrWhiteSpace(data))
                            this.UserLoginNameList = ValueTypeHelper.SplitStringToStringCollection(data, "|");
                    }
                    else if (reader.Name == "UserRoleIdList")
                    {
                        data = reader.ReadElementContentAsString();
                        if (!String.IsNullOrWhiteSpace(data))                        
                            this.UserRoleIdList = ValueTypeHelper.SplitStringToIntCollection(data, '|');
                    }
                    else if (reader.Name == "UserRoleNameList")
                    {
                        data = reader.ReadElementContentAsString();
                        if (!String.IsNullOrWhiteSpace(data))
                            this.UserRoleNameList = ValueTypeHelper.SplitStringToStringCollection(data, "|");
                    }
                    else
                    {
                        // Keep on reading the xml until we reach expected node
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

        #endregion
    }
}