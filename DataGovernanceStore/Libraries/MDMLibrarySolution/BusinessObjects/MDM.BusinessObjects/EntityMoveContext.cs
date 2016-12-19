using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies EntityContext which indicates what all information is to be loaded in Entity object
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class EntityMoveContext : ObjectBase, IEntityMoveContext
    {
        #region Fields

        /// <summary>
        /// Field denoting ReParent Type of Move Context.
        /// </summary>
        private ReParentTypeEnum _reParentType = ReParentTypeEnum.UnKnown;

        /// <summary>
        /// Field denoting target category id to which entity has to be moved
        /// </summary>
        private Int64 _targetCategoryId = 0;

        /// <summary>
        /// Field indicating targetcategory name
        /// </summary>
        private String _targetCategoryName = String.Empty;

        /// <summary>
        /// Field indicationg targetcategory path
        /// </summary>
        private String _targetCategoryPath = String.Empty;

        /// <summary>
        /// Field denoting  from Category Id.
        /// </summary>
        private Int64 _fromCategoryId = 0;

        /// <summary>
        /// Field indicating from category id
        /// </summary>
        private Int64 _targetParentEntityId = 0;

        /// <summary>
        /// Field denoting target parent Extension Entity id.
        /// </summary>
        private Int64 _targetParentExtensionEntityId = 0;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public EntityMoveContext()
            : base()
        {

        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public EntityMoveContext(String valuesAsXml)
        {
            LoadEntityMoveContext(valuesAsXml);
        }

        /// <summary>
        /// Constructor which takes from category id and target category id
        /// </summary>
        /// <param name="fromCategoryId">indicates from category</param>
        /// <param name="targetCategoryId">indicates target category</param>
        public EntityMoveContext(Int64 fromCategoryId, Int64 targetCategoryId)
        {
            this._fromCategoryId = fromCategoryId;
            this._targetCategoryId = targetCategoryId;
        }

        /// <summary>
        /// A constructor which takes fromCategoryId targetCategoryId targetCategoryName and targetCategoryPath
        /// </summary>
        /// <param name="fromCategoryId">Indicates fromCategoryId</param>
        /// <param name="targetCategoryId">Indicates targetCategoryId</param>
        /// <param name="targetCategoryName">Indicates targetCategoryName</param>
        /// <param name="targetCategoryPath">Indicates targetCategoryPath</param>
        public EntityMoveContext(Int64 fromCategoryId, Int64 targetCategoryId, String targetCategoryName, String targetCategoryPath)
        {

            this._fromCategoryId = fromCategoryId;
            this._targetCategoryId = targetCategoryId;
            this._targetCategoryName = targetCategoryName;
            this._targetCategoryPath = targetCategoryPath;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Property indicating ReParentType
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public ReParentTypeEnum ReParentType
        {
            get
            {
                return _reParentType;
            }
            set
            {
                _reParentType = value;
            }
        }

        /// <summary>
        /// Property denoting target category id to which entity has to be moved
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public Int64 TargetCategoryId
        {
            get
            {
                return _targetCategoryId;
            }
            set
            {
                _targetCategoryId = value;
            }
        }

        /// <summary>
        /// Property indicating from Category ( from which entity is being moved)
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public Int64 FromCategoryId
        {
            get
            {
                return _fromCategoryId;
            }
            set
            {
                _fromCategoryId = value;
            }
        }

        /// <summary>
        /// Property indicating target category name
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        public String TargetCategoryName
        {
            get
            {
                return _targetCategoryName;
            }
            set
            {
                _targetCategoryName = value;
            }
        }

        /// <summary>
        /// Property indicating target Category Path
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        public String TargetCategoryPath
        {
            get
            {
                return _targetCategoryPath;
            }
            set
            {
                _targetCategoryPath = value;
            }
        }

        /// <summary>
        /// Property indicating target Parent Entity Id.
        /// </summary>
        [DataMember]
        [ProtoMember(6)]
        public Int64 TargetParentEntityId
        {
            get { return _targetParentEntityId; }
            set { _targetParentEntityId = value; }
        }

        /// <summary>
        /// Property indicating target Parent Extension Entity Id.
        /// </summary>
        [DataMember]
        [ProtoMember(7)]
        public Int64 TargetParentExtensionEntityId
        {
            get { return _targetParentExtensionEntityId; }
            set { _targetParentExtensionEntityId = value; }
        }

        #endregion Properties

        #region Methods

        #region Public methods

        /// <summary>
        /// Loads properties of EntityMoveContext object from xml
        /// </summary>
        /// <param name="reader">Indicates an xml reader used to read xml format data</param>
        internal void LoadEntityMoveContextMetadataFromXml(XmlTextReader reader)
        {
            if (reader != null)
            {
                if (reader.MoveToAttribute("ReParentType"))
                {
                    ReParentTypeEnum reParentType = ReParentTypeEnum.UnKnown;
                    Enum.TryParse<ReParentTypeEnum>(reader.ReadContentAsString(), true, out reParentType);
                    this._reParentType = reParentType;
                }
                if (reader.MoveToAttribute("TargetCategoryId"))
                {
                    this._targetCategoryId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._targetCategoryId);
                }
                if (reader.MoveToAttribute("TargetCategoryPath"))
                {
                    this._targetCategoryPath = reader.ReadContentAsString();
                }
                if (reader.MoveToAttribute("TargetCategoryName"))
                {
                    this._targetCategoryName = reader.ReadContentAsString();
                }
                if (reader.MoveToAttribute("FromCategoryId"))
                {
                    this._fromCategoryId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._fromCategoryId);
                }
                if (reader.MoveToAttribute("TargetParentEntityId"))
                {
                    this._targetParentEntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._targetParentEntityId);
                }
                if (reader.MoveToAttribute("TargetParentExtensionEntityId"))
                {
                    this._targetParentExtensionEntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._targetParentExtensionEntityId);
                }
            }
            else
            {
                throw new ArgumentNullException("reader", "XmlTextReader is null. Can not read EntityMoveContext object.");
            }
        }

        /// <summary>
        /// Represents EntityMoveContext in Xml format
        /// </summary>
        /// <returns>String representation of current EntityContext object</returns>
        public String ToXml()
        {
            String xml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //EntityMoveContext start node
            xmlWriter.WriteStartElement("EntityMoveContext");

            //EntityMoveContext properties
            xmlWriter.WriteAttributeString("ReParentType", this.ReParentType.ToString());
            xmlWriter.WriteAttributeString("FromCategoryId", this.FromCategoryId.ToString());
            xmlWriter.WriteAttributeString("TargetCategoryId", this.TargetCategoryId.ToString());
            xmlWriter.WriteAttributeString("TargetCategoryName", this.TargetCategoryName.ToString());
            xmlWriter.WriteAttributeString("TargetCategoryPath", this.TargetCategoryPath.ToString());
            xmlWriter.WriteAttributeString("TargetParentEntityId", this.TargetParentEntityId.ToString());
            xmlWriter.WriteAttributeString("TargetParentExtensionEntityId", this.TargetParentExtensionEntityId.ToString());

            //EntityContext end node
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }
        
        #endregion Public methods

        #region Private methods

        /// <summary>
        /// Initialize EntityMoveContext from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value of EntityMoveContext</param>
        private void LoadEntityMoveContext(String valuesAsXml)
        {
            #region Sample Xml

            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityMoveContext")
                    {
                        #region Read EntityMoveContext Properties

                        if (reader.HasAttributes)
                        {
                            LoadEntityMoveContextMetadataFromXml(reader);

                            reader.Read();
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
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is EntityMoveContext)
            {
                EntityMoveContext objectToBeCompared = obj as EntityMoveContext;

                if (this.ReParentType != objectToBeCompared.ReParentType)
                    return false;

                if (this.FromCategoryId != objectToBeCompared.FromCategoryId)
                    return false;

                if (this.TargetCategoryId != objectToBeCompared.TargetCategoryId)
                    return false;

                if (this.TargetCategoryName != objectToBeCompared.TargetCategoryName)
                    return false;

                if (this.TargetCategoryPath != objectToBeCompared.TargetCategoryPath)
                    return false;

                if (this.TargetParentEntityId != objectToBeCompared.TargetParentEntityId)
                    return false;

                if (this.TargetParentExtensionEntityId != objectToBeCompared.TargetParentExtensionEntityId)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            Int32 hashCode = base.GetHashCode() ^ this.FromCategoryId.GetHashCode() ^ this.TargetCategoryId.GetHashCode() ^ this.TargetCategoryName.GetHashCode() ^
                this.TargetCategoryPath.GetHashCode() ^ this.TargetParentEntityId.GetHashCode() ^ this.TargetParentExtensionEntityId.GetHashCode() ^ this.ReParentType.GetHashCode();

            return hashCode;
        }

        #endregion Private methods

        #endregion Methods
    }
}
