using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies Entity History Details Template
    /// </summary>
    [DataContract]
    public class EntityHistoryDetailsTemplate : MDMObject, IEntityHistoryDetailsTemplate
    {
        #region Fields

        /// <summary>
        /// Indicates type of change happened for entity
        /// </summary>
        private EntityChangeType _changeType = EntityChangeType.Unknown;

        /// <summary>
        /// Indicates entity history details template code 
        /// </summary>
        private String _templateCode = String.Empty;

        /// <summary>
        /// Indicates entity history details template text for a specific locale
        /// </summary>
        private String _templateText = String.Empty;

        /// <summary>
        /// Indicates description about history details template
        /// </summary>
        private String _description = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public EntityHistoryDetailsTemplate()
            : base()
        { }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="changeType">Indicates a type of change to be done for entity</param>
        /// <param name="templateCode">Indicates template Code</param>
        /// <param name="templateText">Indicates template text</param>
        /// <param name="description">Indicates description about template</param>
        /// <param name="action">Indicates action of the template</param>
        public EntityHistoryDetailsTemplate(EntityChangeType changeType, String templateCode, String templateText, String description, ObjectAction action)
        {
            InitialiseFields(changeType, templateCode, templateText, description, action);
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public EntityHistoryDetailsTemplate(String valuesAsXml)
        {
            LoadEntityHistoryDetailsTemplate(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting type of change happened for entity
        /// </summary>
        [DataMember]
        public EntityChangeType ChangeType
        {
            get { return _changeType; }
            set { _changeType = value; }
        }

        /// <summary>
        /// Property denoting template code 
        /// </summary>
        [DataMember]
        public String TemplateCode
        {
            get { return _templateCode; }
            set { _templateCode = value; }
        }

        /// <summary>
        /// Property denoting template text
        /// </summary>
        [DataMember]
        public String TemplateText
        {
            get { return _templateText; }
            set { _templateText = value; }
        }

        /// <summary>
        /// Property denoting description about template
        /// </summary>
        [DataMember]
        public String Description
        {
            get { return _description; }
            set { _description = value; }
        }
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Represents EntityHistoryDetailsTemplate in Xml format
        /// </summary>
        /// <returns>String representation of current EntityHistoryDetailsTemplate object</returns>
        public override String ToXml()
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            // Attribute node start
            xmlWriter.WriteStartElement("EntityHistoryDetailsTemplate");

            xmlWriter.WriteAttributeString("ChangeType", this.ChangeType.ToString());
            xmlWriter.WriteAttributeString("TemplateCode", this.TemplateCode);
            xmlWriter.WriteAttributeString("TemplateText", this.TemplateText);
            xmlWriter.WriteAttributeString("Description", this.Description);
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());

            // EntityHistoryRecord end node
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            // get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Get Xml representation of EntityHistoryDetailsTemplate
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            return this.ToXml();
        }

        /// <summary>
        /// clones the object to another object
        /// </summary>
        /// <returns></returns>
        public EntityHistoryDetailsTemplate Clone()
        {
            EntityHistoryDetailsTemplate clonedEntityHistoryDetailsTemplate = new EntityHistoryDetailsTemplate();

            clonedEntityHistoryDetailsTemplate.ChangeType = this.ChangeType;
            clonedEntityHistoryDetailsTemplate.TemplateCode = this.TemplateCode;
            clonedEntityHistoryDetailsTemplate.TemplateText = this.TemplateText;
            clonedEntityHistoryDetailsTemplate.Description = this.Description;
            clonedEntityHistoryDetailsTemplate.Action = this.Action;

            return clonedEntityHistoryDetailsTemplate;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="objectToBeCompared">EntityHistoryDetailsTemplate Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean Equals(EntityHistoryDetailsTemplate objectToBeCompared)
        {
            if (this.ChangeType != objectToBeCompared.ChangeType)
                return false;

            if (this.TemplateCode != objectToBeCompared.TemplateCode)
                return false;

            if (this.TemplateText != objectToBeCompared.TemplateText)
                return false;

            if (this.Description != objectToBeCompared.Description)
                return false;

            if (this.Action != objectToBeCompared.Action)
                return false;

            return true;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            int hashCode = this.ChangeType.GetHashCode() ^ this.TemplateCode.GetHashCode() ^ this.TemplateText.GetHashCode() ^ this.Description.GetHashCode() ^ this.Action.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Determines whether invoking object is superset of parameterised object.
        /// </summary>
        /// <param name="subsetEntityHistoryTemplate">Indicate the subset entity history template object</param>
        /// <returns>Returns true if it is superset, otherwise false.</returns>
        public Boolean IsSuperSetOf(EntityHistoryDetailsTemplate subsetEntityHistoryTemplate)
        {
            if (this.ChangeType != subsetEntityHistoryTemplate.ChangeType)
                return false;

            if (this.TemplateCode != subsetEntityHistoryTemplate.TemplateCode)
                return false;

            if (this.TemplateText != subsetEntityHistoryTemplate.TemplateText)
                return false;

            if (this.Description != subsetEntityHistoryTemplate.Description)
                return false;

            if (this.Action != subsetEntityHistoryTemplate.Action)
                return false;

            return true;
        }

        #endregion

        #region Private Methods

        private void InitialiseFields(EntityChangeType changeType, String templateCode, String templateText, String description, ObjectAction action)
        {
            this._changeType = changeType;
            this._templateCode = templateCode;
            this._templateText = templateText;
            this._description = description;
            this.Action = action;
        }

        private void LoadEntityHistoryDetailsTemplate(String valuesAsXml)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityHistoryDetailsTemplate")
                    {
                        #region Read EntityHistoryDetailsTemplate Properties

                        if (reader.HasAttributes)
                        {
                            EntityChangeType changeType = EntityChangeType.Unknown;
                            ObjectAction action = ObjectAction.Unknown;

                            if (reader.MoveToAttribute("ChangeType"))
                            {
                                Enum.TryParse<EntityChangeType>(reader.ReadContentAsString(), out changeType);
                            }

                            this.ChangeType = changeType;

                            if (reader.MoveToAttribute("TemplateCode"))
                            {
                                this.TemplateCode = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("TemplateText"))
                            {
                                this.TemplateText = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("Description"))
                            {
                                this.Description = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("Action"))
                            {
                                Enum.TryParse<ObjectAction>(reader.ReadContentAsString(), out action);
                            }

                            this.Action = action;
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

        #endregion

        #endregion
    }
}
