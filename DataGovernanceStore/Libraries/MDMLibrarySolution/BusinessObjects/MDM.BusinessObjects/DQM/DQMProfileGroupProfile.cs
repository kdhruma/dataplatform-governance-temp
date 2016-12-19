using System;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;

    /// <summary>
    /// Represents class for DQM profile group
    /// </summary>
    public class DQMProfileGroupProfile : ICloneable
    {
        #region Constructors

        /// <summary>
        /// Constructs DQM Profile Group Profile
        /// </summary>
        public DQMProfileGroupProfile()
        {
            Action = ObjectAction.Read;
        }

        /// <summary>
        /// Constructs DQM Profile Group Profile using specified instance data
        /// </summary>
        public DQMProfileGroupProfile(DQMProfileGroupProfile source)
        {
            ProfileGroupProfileId = source.ProfileGroupProfileId;
            ProfileGroupId = source.ProfileGroupId;
            ProfileId = source.ProfileId;
            Enabled = source.Enabled;
            DeleteFlag = source.DeleteFlag;
            AuditRef = source.AuditRef;
            Action = source.Action;
        }

        #endregion

        #region Properties

        /// <summary>
        /// ProfileGroupProfile Id
        /// </summary>
        public Int32 ProfileGroupProfileId { get; set; }

        /// <summary>
        /// Profile Group Id
        /// </summary>
        public Int32 ProfileGroupId { get; set; }

        /// <summary>
        /// Profile Id
        /// </summary>
        public Int32 ProfileId { get; set; }

        /// <summary>
        /// Application Context Id
        /// </summary>
        public Int32 ApplicationContextId { get; set; }

        /// <summary>
        /// Enabled
        /// </summary>
        public Boolean Enabled { get; set; }

        /// <summary>
        /// Enabled
        /// </summary>
        public Boolean DeleteFlag { get; set; }
    
        /// <summary>
        /// Audit Ref
        /// </summary>
        public Int64 AuditRef { get; set; }

        /// <summary>
        /// Field for the action of an object.
        /// </summary>
        public ObjectAction Action { get; set; }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone DQMProfileGroupProfile
        /// </summary>
        /// <returns>Cloned DQMProfileGroupProfile object</returns>
        public object Clone()
        {
            var clone = new DQMProfileGroupProfile(this);
            return clone;
        }

        #endregion
        
        #region Xml Serialization

        /// <summary>
        /// Represents mdmObject in Xml format
        /// </summary>
        /// <returns>String representation of current mdmObject</returns>
        public virtual String ToXml()
        {
            String profileGroupXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            // DQMProfileGroupProfile Item node start
            xmlWriter.WriteStartElement("DQMProfileGroupProfile");

            #region write DQMProfileGroupProfile for Full DQMProfileGroupProfile Xml

            xmlWriter.WriteAttributeString("ProfileGroupProfileId", this.ProfileGroupProfileId.ToString());
            xmlWriter.WriteAttributeString("ProfileGroupId", this.ProfileGroupId.ToString());
            xmlWriter.WriteAttributeString("ProfileId", this.ProfileId.ToString());
            xmlWriter.WriteAttributeString("ApplicationContextId", this.ApplicationContextId.ToString());
            xmlWriter.WriteAttributeString("Enabled", this.Enabled.ToString());
            xmlWriter.WriteAttributeString("DeleteFlag", this.DeleteFlag.ToString());
            xmlWriter.WriteAttributeString("AuditRef", this.AuditRef.ToString());
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());

            #endregion write DQMProfileGroupProfile for Full DQMProfileGroupProfile Xml

            // DQMProfileGroupProfile Item node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            profileGroupXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return profileGroupXml;
        }

        /// <summary>
        /// Loads current object from provided XmlNode
        /// </summary>
        /// <param name="node">XmlNode for deserialization</param>
        public void LoadFromXml(XmlNode node)
        {
            if (node == null)
            {
                return;
            }
            if (node.Attributes != null)
            {
                if (node.Attributes["ProfileGroupProfileId"] != null)
                {
                    ProfileGroupProfileId = ValueTypeHelper.Int32TryParse(node.Attributes["ProfileGroupProfileId"].Value, ProfileGroupProfileId);
                }
                if (node.Attributes["ProfileGroupId"] != null)
                {
                    ProfileGroupId = ValueTypeHelper.Int32TryParse(node.Attributes["ProfileGroupId"].Value, ProfileGroupId);
                }
                if (node.Attributes["ProfileId"] != null)
                {
                    ProfileId = ValueTypeHelper.Int32TryParse(node.Attributes["ProfileId"].Value, ProfileId);
                }
                if (node.Attributes["ApplicationContextId"] != null)
                {
                    ApplicationContextId = ValueTypeHelper.Int32TryParse(node.Attributes["ApplicationContextId"].Value, ApplicationContextId);
                }
                if (node.Attributes["Enabled"] != null)
                {
                    Enabled = ValueTypeHelper.BooleanTryParse(node.Attributes["Enabled"].Value, Enabled);
                }
                if (node.Attributes["DeleteFlag"] != null)
                {
                    DeleteFlag = ValueTypeHelper.BooleanTryParse(node.Attributes["DeleteFlag"].Value, DeleteFlag);
                }
                if (node.Attributes["AuditRef"] != null)
                {
                    AuditRef = ValueTypeHelper.Int64TryParse(node.Attributes["AuditRef"].Value, AuditRef);
                }
                if (node.Attributes["Action"] != null)
                {
                    ObjectAction tempAction;
                    ValueTypeHelper.EnumTryParse(node.Attributes["Action"].Value, true, out tempAction);
                    Action = tempAction;
                }
            }
        }

        /// <summary>
        /// Loads current object from provided Xml
        /// </summary>
        /// <param name="xml">Xml string for deserialization</param>
        public void LoadFromXml(String xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode node = doc.SelectSingleNode("DQMProfileGroupProfile");
            if (node != null)
            {
                LoadFromXml(node);
            }
        }

        #endregion

    }
}