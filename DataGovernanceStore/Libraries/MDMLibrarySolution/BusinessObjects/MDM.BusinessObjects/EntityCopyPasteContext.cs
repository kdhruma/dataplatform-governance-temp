using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// Specifies EntityCopyPasteContext which indicates all information is to be needed for CopyPaste Operation
    /// </summary>
    [DataContract]
    public class EntityCopyPasteContext : ObjectBase, IEntityCopyPasteContext
    {
        #region Fields

        /// <summary>
        /// Field specifying source entity id
        /// </summary>
        private Int64 _fromEntityId = -1;

        /// <summary>
        /// Field specifying source container id
        /// </summary>
        private Int32 _fromContainerId = -1;

        /// <summary>
        /// Field specifying target entity ids
        /// </summary>
        private Collection<Int64> _toEntityIds = null;

        /// <summary>
        /// Field specifying target container id
        /// </summary>
        private Int32 _toConatinerId = -1;

        /// <summary>
        /// Field specifying source to target dataLocale mappings
        /// </summary>
        private Dictionary<LocaleEnum, LocaleEnum> _localeMappings = null;

        /// <summary>
        /// Field specifying Collection of attributeIds
        /// </summary>
        private Collection<Int32> _attributeIds = null;

        /// <summary>
        /// Field specifying Collection of relationship Type Ids
        /// </summary>
        private Collection<Int32> _relationshipTypeIds = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public EntityCopyPasteContext() : base()
        {
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public EntityCopyPasteContext(String valuesAsXml)
        {
            LoadEntityCopyPasteContext(valuesAsXml);
        }
        
        #endregion Constructor

        #region Properties

        /// <summary>
        /// Property denoting Source Entity Id
        /// </summary>
        [DataMember]
        public Int64 FromEntityId
        {
            get { return this._fromEntityId; }
            set { this._fromEntityId = value; }
        }

        /// <summary>
        /// Property denoting Source Container Id
        /// </summary>
        [DataMember]
        public Int32 FromContainerId
        {
            get { return this._fromContainerId; }
            set { this._fromContainerId = value; }
        }

        /// <summary>
        /// Property denoting Target Entity Ids
        /// </summary>
        [DataMember]
        public Collection<Int64> ToEntityIds
        {
            get { return this._toEntityIds; }
            set { this._toEntityIds = value; }
        }

        /// <summary>
        /// Property denoting Target Container Id
        /// </summary>
        [DataMember]
        public Int32 ToContainerId
        {
            get { return this._toConatinerId; }
            set { this._toConatinerId = value; }
        }

        /// <summary>
        ///  Property denoting source to target locale mappings
        /// </summary>
        [DataMember]
        public Dictionary<LocaleEnum, LocaleEnum> LocaleMappings
        {
            get { return this._localeMappings; }
            set { this._localeMappings = value; }
        }

        /// <summary>
        /// Property denoting Ids of Attribute for copy-paste
        /// </summary>
        [DataMember]
        public Collection<Int32> AttributeIds
        {
            get { return this._attributeIds; }
            set { this._attributeIds = value; }
        }

        /// <summary>
        /// Property denoting Ids of RelationshipType for copy-paste
        /// </summary>
        [DataMember]
        public Collection<Int32> RelationshipTypeIds
        {
            get { return this._relationshipTypeIds; }
            set { this._relationshipTypeIds = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Represents EntityCopyPasteContext  in Xml format
        /// </summary>
        /// <returns>String representation of current EntityCopyPasteContext object</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            String attributeIds = String.Empty;
            String relationshipTypeIds = String.Empty;
            String toEntityIds = String.Empty;

            if (this.AttributeIds != null)
            {
                attributeIds = ValueTypeHelper.JoinCollection(this.AttributeIds, ",");
            }

            if (this.RelationshipTypeIds != null)
            {
                relationshipTypeIds = ValueTypeHelper.JoinCollection(this.RelationshipTypeIds, ",");
            }

            if (this.ToEntityIds != null)
            {
                toEntityIds = ValueTypeHelper.JoinCollection(this.ToEntityIds, ",");
            }

            xmlWriter.WriteStartElement("EntityCopyPasteContext");

            xmlWriter.WriteAttributeString("FromEntityId", this.FromEntityId.ToString());
            xmlWriter.WriteAttributeString("FromContainerId", this.FromContainerId.ToString());
            xmlWriter.WriteAttributeString("ToEntityIds", toEntityIds);
            xmlWriter.WriteAttributeString("ToContainerId", this.ToContainerId.ToString());
            xmlWriter.WriteAttributeString("AttributeIds", attributeIds);
            xmlWriter.WriteAttributeString("RelationshipTypeIds", relationshipTypeIds);

            xmlWriter.WriteStartElement("LocaleMappings");

            if (this.LocaleMappings != null)
            {
                foreach (KeyValuePair<LocaleEnum, LocaleEnum> mappings in this.LocaleMappings)
                {
                    xmlWriter.WriteStartElement("LocaleMapping");

                    xmlWriter.WriteAttributeString("FromDataLocale", mappings.Key.ToString());
                    xmlWriter.WriteAttributeString("ToDataLocale", mappings.Value.ToString());

                    xmlWriter.WriteEndElement();
                }
            }

            //LocaleMappings
            xmlWriter.WriteEndElement();

            //EntityCopyPasteContext
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            // get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        #endregion 

        #region Private Methods

        /// <summary>
        /// Load EntityCopyPasteContext object from XML.
        /// </summary>
        /// <param name="valuesAsXml">EntityCopyPasteContext as xml</param>
        private void LoadEntityCopyPasteContext(String valuesAsXml)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityCopyPasteContext")
                    {
                        #region Read EntityCopyPasteContext Properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("FromEntityId"))
                            {
                                this.FromEntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("FromContainerId"))
                            {
                                this.FromContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("ToEntityIds"))
                            {
                                this.ToEntityIds = ValueTypeHelper.SplitStringToLongCollection(reader.ReadContentAsString(), ',');
                            }

                            if (reader.MoveToAttribute("ToContainerId"))
                            {
                                this.ToContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("AttributeIds"))
                            {
                                this.AttributeIds = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                            }

                            if (reader.MoveToAttribute("RelationshipTypeIds"))
                            {
                                this.RelationshipTypeIds = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                            }
                        }

                        #endregion
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "LocaleMappings")
                    {
                        #region Read Locale Mappings

                        String localeMappingsXml = reader.ReadOuterXml();
                        if (!String.IsNullOrEmpty(localeMappingsXml))
                        {
                            LoadLocaleMappings(localeMappingsXml);
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

        /// <summary>
        /// Load LocaleMappings object from XML.
        /// </summary>
        /// <param name="valuesAsXml">LocaleMappings as xml</param>
        private void LoadLocaleMappings(String valuesAsXml)
        {
            Dictionary<LocaleEnum, LocaleEnum> localeMappings = new Dictionary<LocaleEnum, LocaleEnum>();

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "LocaleMapping")
                    {
                        LocaleEnum fromDatalocale = LocaleEnum.UnKnown;
                        LocaleEnum toDatalocale = LocaleEnum.UnKnown;

                        if (reader.MoveToAttribute("FromDataLocale"))
                        {
                            String strLocale = reader.ReadContentAsString();
                            Enum.TryParse<LocaleEnum>(strLocale, true, out fromDatalocale);
                        }

                        if (reader.MoveToAttribute("ToDataLocale"))
                        {
                            String strLocale = reader.ReadContentAsString();
                            Enum.TryParse<LocaleEnum>(strLocale, true, out toDatalocale);
                        }

                        localeMappings.Add(fromDatalocale, toDatalocale);
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

            this.LocaleMappings = localeMappings;
        }

        #endregion
    }
}
