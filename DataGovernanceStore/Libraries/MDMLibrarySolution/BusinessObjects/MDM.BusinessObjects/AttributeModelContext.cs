using System;
using System.ComponentModel;
using System.Xml;
using System.Runtime.Serialization;
using System.IO;
using System.Collections.ObjectModel;
using System.Linq;

using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies Attribute Model Context. This context helps in fetching appropriate Attributes and their properties as per the context parameters.
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class AttributeModelContext : ObjectBase, IAttributeModelContext
    {
        #region Fields

        /// <summary>
        /// Field denoting the type of the attribute model
        /// </summary>
        [DataMember]
        [ProtoMember(1), DefaultValue(AttributeModelType.All)]
        private AttributeModelType _attributeModelType = AttributeModelType.All;

        /// <summary>
        /// Field denoting container id for the attribute model context
        /// </summary>
        [DataMember]
        [ProtoMember(2), DefaultValue(0)]
        private Int32 _containerId = 0;

        /// <summary>
        /// Field denoting entity type id for the attribute model context
        /// </summary>
        [DataMember]
        [ProtoMember(3), DefaultValue(0)]
        private Int32 _entityTypeId = 0;

        /// <summary>
        /// Field denoting relationship type id for the attribute model context
        /// </summary>
        [DataMember]
        [ProtoMember(4), DefaultValue(0)]
        private Int32 _relationshipTypeId = 0;

        /// <summary>
        /// Field denoting category id for the attribute model context
        /// </summary>
        [DataMember]
        [ProtoMember(5), DefaultValue(0)]
        private Int64 _categoryId = 0;

        /// <summary>
        /// Field denoting locales in which AttributeModels are to be loaded.
        /// </summary>
        [DataMember]
        [ProtoMember(6)]
        private Collection<LocaleEnum> _locales = new Collection<LocaleEnum>();

        /// <summary>
        /// Field denoting EntityId for current context for AttributeModel.
        /// EntityId is used when we want to evaluate the CustomView for some specific Entity.
        /// E.g. Required empty attributes for current entity.
        /// </summary>
        [DataMember]
        [ProtoMember(7), DefaultValue(0)]
        private Int64 _entityId = 0;

        /// <summary>
        /// Field denoting whether to get only attribute models marked as ShowAtCreation
        /// </summary>
        [DataMember]
        [ProtoMember(8), DefaultValue(false)]
        private Boolean _getOnlyShowAtCreationAttributes = false;

        /// <summary>
        /// Field denoting whether to get only attribute models marked as ShowAtCreation
        /// </summary>
        [DataMember]
        [ProtoMember(9), DefaultValue(false)]
        private Boolean _getOnlyRequiredAttributes = false;

        /// <summary>
        /// Field denoting whether to get complete details or only the main details of attribute model
        /// </summary>
        [DataMember]
        [ProtoMember(10), DefaultValue(true)]
        private Boolean _getCompleteDetailsOfAttribute = true;

        /// <summary>
        /// Field denoting whether to sort attribute model or not.
        /// </summary>
        [DataMember]
        [ProtoMember(11), DefaultValue(true)]
        private Boolean _applySorting = true;

        /// <summary>
        /// Field denoting whether to apply security for attribute model or not.
        /// </summary>
        [DataMember]
        [ProtoMember(12), DefaultValue(true)]
        private Boolean _applySecurity = true;

        /// <summary>
        /// Field denoting whether to apply attribute dependency for attribute model or not.
        /// </summary>
        [DataMember]
        [ProtoMember(13), DefaultValue(true)]
        private Boolean _applyAttributeDependency = true;

        /// <summary>
        /// Field denoting container name for the attribute model context
        /// </summary>
        [DataMember]
        [ProtoMember(14)]
        private String _containerName = String.Empty;

        /// <summary>
        /// Field denoting entity type name for the attribute model context
        /// </summary>
        [DataMember]
        [ProtoMember(15)]
        private String _entityTypeName = String.Empty;

        /// <summary>
        /// Field denoting category name for the attribute model context
        /// </summary>
        [DataMember]
        [ProtoMember(16)]
        private String _categoryPath = String.Empty;

        /// <summary>
        /// Field denoting relationship type name for the attribute model context
        /// </summary>
        [DataMember]
        [ProtoMember(17)]
        private String _relationshipTypeName = String.Empty;

        /// <summary>
        /// Field denoting whether to load permission set for attribute models
        /// </summary>
        [DataMember]
        [ProtoMember(18), DefaultValue(false)]
        private Boolean _loadPermissions = false;

        /// <summary>
        /// Indicates whether to load state validation attributes or not.
        /// </summary>
        [DataMember]
        [ProtoMember(19), DefaultValue(false)]
        private Boolean _loadStateValidationAttributes = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public AttributeModelContext()
        {
        }

        /// <summary>
        /// Constructor with containerId, entityTypeId, categoryId and locale
        /// </summary>
        /// <param name="containerId">ContainerId for AttributeModelContext</param>
        /// <param name="entityTypeId">EntityTypeId for AttributeModelContext</param>
        /// <param name="relationshipTypeId">RelationshipTypeId for AttributeModelContext</param>
        /// <param name="categoryId">CategoryId for AttributeModelContext</param>
        /// <param name="locales">Locales for AttributeModelContext</param>
        /// <param name="entityId">Entity Id for which we want attribute model. Entity Id is mainly used to evaluate BR for custom views</param>
        /// <param name="attributeModelType">Indicate which type of attributes are to be loaded</param>
        /// <param name="getOnlyShowAtCreationAttributes">Indicates that only attributes those are marked as ShowAtCreation = true</param>
        /// <param name="getOnlyRequiredAttributes">Indicates that only required attributes are to be loaded</param>
        /// <param name="getCompleteDetailsOfAttribute">Indicates that all detail about attributes are to be loaded</param>
        /// <returns>AttributeModelContext initialized with given information</returns>
        public AttributeModelContext(Int32 containerId, Int32 entityTypeId, Int32 relationshipTypeId, Int64 categoryId, Collection<LocaleEnum> locales, Int64 entityId, AttributeModelType attributeModelType, Boolean getOnlyShowAtCreationAttributes, Boolean getOnlyRequiredAttributes, Boolean getCompleteDetailsOfAttribute)
        {
            _containerId = containerId;
            _entityTypeId = entityTypeId;
            _relationshipTypeId = relationshipTypeId;
            _categoryId = categoryId;
            _locales = locales;
            _entityId = entityId;
            _attributeModelType = attributeModelType;
            _getOnlyShowAtCreationAttributes = getOnlyShowAtCreationAttributes;
            _getOnlyRequiredAttributes = getOnlyRequiredAttributes;
            _getCompleteDetailsOfAttribute = getCompleteDetailsOfAttribute;
        }

        /// <summary>
        /// Constructor with Values as xml format
        /// </summary>
        /// <param name="valuesAsXml">Xml formatted values with which object will be initalized.</param>
        public AttributeModelContext(String valuesAsXml)
        {
            LoadAttributeModelContext(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting model type of an attribute
        /// </summary>
        public AttributeModelType AttributeModelType
        {
            get
            {
                return _attributeModelType;
            }
            set
            {
                _attributeModelType = value;
            }
        }

        /// <summary>
        /// Property denoting container id for the attribute model context
        /// </summary>
        public Int32 ContainerId
        {
            get { return _containerId; }
            set { _containerId = value; }
        }

        /// <summary>
        /// Property denoting entity type id for the attribute model context
        /// </summary>
        public Int32 EntityTypeId
        {
            get { return _entityTypeId; }
            set { _entityTypeId = value; }
        }

        /// <summary>
        /// Property denoting relation type id for the attribute model context
        /// </summary>
        public Int32 RelationshipTypeId
        {
            get { return _relationshipTypeId; }
            set { _relationshipTypeId = value; }
        }

        /// <summary>
        /// Property denoting category id for the attribute model context
        /// </summary>
        public Int64 CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
        }

        /// <summary>
        /// Property denoting locales for which AttributeModels are to be fetched.
        /// </summary>
        public Collection<LocaleEnum> Locales
        {
            get
            {
                var locales = new Collection<LocaleEnum>();

                if (_locales != null && _locales.Count > 0)
                {
                    foreach (var locale in _locales)
                    {
                        if (locale != LocaleEnum.UnKnown && !locales.Contains(locale))
                        {
                            locales.Add(locale);
                        }
                    }
                }
                
                _locales = locales;
                
                return _locales;
            }
            set { _locales = value; }
        }

        /// <summary>
        /// Property denoting EntityId for current context for AttributeModel.
        /// EntityId is used when we want to evaluate the CustomView for some specific Entity.
        /// E.g. Required empty attributes for current entity.
        /// </summary>
        public Int64 EntityId
        {
            get
            {
                return this._entityId;
            }
            set
            {
                this._entityId = value;
            }
        }

        /// <summary>
        /// Property denoting whether to get only attribute models marked as ShowAtCreation
        /// </summary>
        public Boolean GetOnlyShowAtCreationAttributes
        {
            get { return _getOnlyShowAtCreationAttributes; }
            set { _getOnlyShowAtCreationAttributes = value; }
        }

        /// <summary>
        /// Property denoting whether to get only attribute models marked as Required
        /// </summary>
        public Boolean GetOnlyRequiredAttributes
        {
            get { return _getOnlyRequiredAttributes; }
            set { _getOnlyRequiredAttributes = value; }
        }

        /// <summary>
        /// Property denoting whether to get complete details or only the main details of attribute model
        /// </summary>
        public Boolean GetCompleteDetailsOfAttribute
        {
            get { return _getCompleteDetailsOfAttribute; }
            set { _getCompleteDetailsOfAttribute = value; }
        }

        /// <summary>
        /// Property denoting whether to sort attribute model or not.
        /// </summary>
        public Boolean ApplySorting
        {
            get { return _applySorting; }
            set { _applySorting = value; }
        }

        /// <summary>
        /// Field denoting whether to apply security for attribute model or not.
        /// </summary>
        public Boolean ApplySecurity
        {
            get { return _applySecurity; }
            set { _applySecurity = value; }
        }

        /// <summary>
        /// Property along with the Attribute Dependency Appconfig key denotes whether to apply attribute dependency for attribute model.
        /// If attribute dependency is required, both these values should be true.
        /// </summary>
        public Boolean ApplyAttributeDependency
        {
            get { return _applyAttributeDependency; }
            set { _applyAttributeDependency = value; }
        }

        /// <summary>
        /// Property denoting container name for the attribute model context
        /// </summary>
        public String ContainerName
        {
            get { return _containerName; }
            set { _containerName = value; }
        }

        /// <summary>
        /// Field denoting entity type name for the attribute model context
        /// </summary>
        public String EntityTypeName
        {
            get { return _entityTypeName; }
            set { _entityTypeName = value; }
        }

        /// <summary>
        /// Field denoting category name for the attribute model context
        /// </summary>
        public String CategoryPath
        {
            get { return _categoryPath; }
            set { _categoryPath = value; }
        }

        /// <summary>
        /// Field denoting relationship type name for the attribute model context
        /// </summary>
        public String RelationshipTypeName
        {
            get { return _relationshipTypeName; }
            set { _relationshipTypeName = value; }
        }

        /// <summary>
        /// Field denoting whether to load permission set for attribute models
        /// </summary>
        public Boolean LoadPermissions
        {
            get { return _loadPermissions; }
            set { _loadPermissions = value; }
        }

        /// <summary>
        /// Specifies whether to load state validation attributes or not.
        /// </summary>
        public Boolean LoadStateValidationAttributes
        {
            get { return _loadStateValidationAttributes; }
            set { _loadStateValidationAttributes = value; }
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
            if (obj is AttributeModelContext)
            {
                AttributeModelContext objectToBeCompared = obj as AttributeModelContext;

                if (this.ContainerId != objectToBeCompared.ContainerId)
                {
                    return false;
                }
                if (this.EntityTypeId != objectToBeCompared.EntityTypeId)
                {
                    return false;
                }
                if (this.RelationshipTypeId != objectToBeCompared.RelationshipTypeId)
                {
                    return false;
                }
                if (this.CategoryId != objectToBeCompared.CategoryId)
                {
                    return false;
                }
                if (this.AttributeModelType != objectToBeCompared.AttributeModelType)
                {
                    return false;
                }
                foreach (LocaleEnum locale in this.Locales)
                {
                    if (!objectToBeCompared.Locales.Contains(locale))
                        return false;
                }
                foreach (LocaleEnum locale in objectToBeCompared.Locales)
                {
                    if (!this.Locales.Contains(locale))
                        return false;
                }
                if (this.EntityId != objectToBeCompared.EntityId)
                {
                    return false;
                }
                if (this.GetOnlyShowAtCreationAttributes != objectToBeCompared.GetOnlyShowAtCreationAttributes)
                {
                    return false;
                }
                if (this.GetOnlyRequiredAttributes != objectToBeCompared.GetOnlyRequiredAttributes)
                {
                    return false;
                }
                if (this.GetCompleteDetailsOfAttribute != objectToBeCompared.GetCompleteDetailsOfAttribute)
                {
                    return false;
                }
                if (this.ApplySorting != objectToBeCompared.ApplySorting)
                {
                    return false;
                }
                if (this.ApplySecurity != objectToBeCompared.ApplySecurity)
                {
                    return false;
                }
                if (this.LoadStateValidationAttributes != objectToBeCompared.LoadStateValidationAttributes)
                {
                    return false;
                }
                return true;
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
            hashCode = this.ContainerId.GetHashCode() ^ this.EntityTypeId.GetHashCode() ^ this.RelationshipTypeId.GetHashCode() ^ this.CategoryId.GetHashCode() ^ 
                       this.EntityId.GetHashCode() ^ this.GetOnlyShowAtCreationAttributes.GetHashCode() ^ this.GetOnlyRequiredAttributes.GetHashCode() ^ 
                       this.GetCompleteDetailsOfAttribute.GetHashCode() ^ this.ApplySorting.GetHashCode() ^ this.ApplySecurity.GetHashCode() ^ this.LoadStateValidationAttributes.GetHashCode();

            //Loop over the locale enum collection and add the Int32 value. Do no use the locale hash code
            foreach (LocaleEnum locale in _locales)
            {
                hashCode = hashCode ^ locale.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// 
        /// </summary>
        public Collection<LocaleEnum> GetLocales()
        {
            return _locales;
        }

        /// <summary>
        /// Get Xml representation of Attribute Model Identifier Context
        /// </summary>
        /// <returns>Xml representation of Attribute Model Data Context</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            //xmlWriter.WriteStartDocument();

            //Child AttributeModel node start
            xmlWriter.WriteStartElement("AttributeModelContext");

            #region write Attribute Model Context

            xmlWriter.WriteAttributeString("AttributeModelType", this.AttributeModelType.ToString());
            xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
            xmlWriter.WriteAttributeString("EntityTypeId", this.EntityTypeId.ToString());
            xmlWriter.WriteAttributeString("RelationshipTypeId", this.RelationshipTypeId.ToString());
            xmlWriter.WriteAttributeString("CategoryId", this.CategoryId.ToString());
            xmlWriter.WriteAttributeString("Locales", ValueTypeHelper.JoinCollection(this.Locales, ","));
            xmlWriter.WriteAttributeString("EntityId", this.EntityId.ToString());
            xmlWriter.WriteAttributeString("GetOnlyShowAtCreationAttributes", this.GetOnlyShowAtCreationAttributes.ToString());
            xmlWriter.WriteAttributeString("GetOnlyRequiredAttributes", this.GetOnlyRequiredAttributes.ToString());
            xmlWriter.WriteAttributeString("GetCompleteDetailsOfAttribute", this.GetCompleteDetailsOfAttribute.ToString());
            xmlWriter.WriteAttributeString("ApplySorting", this.ApplySorting.ToString());
            xmlWriter.WriteAttributeString("LoadStateValidationAttributes", this.LoadStateValidationAttributes.ToString());

            #endregion write Attribute Model Context

            //AttributeModelContext node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            returnXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();


            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of Attribute Model Identifier Context based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Attribute Model Data Context</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String returnXml = String.Empty;

            if (objectSerialization == ObjectSerialization.Full)
            {
                returnXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                if (objectSerialization == ObjectSerialization.ProcessingOnly)
                {
                    #region write Attribute Model Context for ProcessingOnly Xml

                    xmlWriter.WriteAttributeString("AttributeModelType", this.AttributeModelType.ToString());
                    xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
                    xmlWriter.WriteAttributeString("EntityTypeId", this.EntityTypeId.ToString());
                    xmlWriter.WriteAttributeString("RelationshipTypeId", this.RelationshipTypeId.ToString());
                    xmlWriter.WriteAttributeString("CategoryId", this.CategoryId.ToString());
                    xmlWriter.WriteAttributeString("Locales", ValueTypeHelper.JoinCollection(this.Locales, ","));
                    xmlWriter.WriteAttributeString("EntityId", this.EntityId.ToString());
                    xmlWriter.WriteAttributeString("GetOnlyShowAtCreationAttributes", this.GetOnlyShowAtCreationAttributes.ToString());
                    xmlWriter.WriteAttributeString("GetOnlyRequiredAttributes", this.GetOnlyRequiredAttributes.ToString());
                    xmlWriter.WriteAttributeString("GetCompleteDetailsOfAttribute", this.GetCompleteDetailsOfAttribute.ToString());

                    #endregion write Attribute Model Context for ProcessingOnly Xml
                }
                else if (objectSerialization == ObjectSerialization.Compact)
                {
                    #region write Attribute Model Context for Compact Xml

                    xmlWriter.WriteAttributeString("AttributeModelType", this.AttributeModelType.ToString());
                    xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
                    xmlWriter.WriteAttributeString("EntityTypeId", this.EntityTypeId.ToString());
                    xmlWriter.WriteAttributeString("RelationshipTypeId", this.RelationshipTypeId.ToString());
                    xmlWriter.WriteAttributeString("CategoryId", this.CategoryId.ToString());
                    xmlWriter.WriteAttributeString("Locales", ValueTypeHelper.JoinCollection(this.Locales, ","));
                    xmlWriter.WriteAttributeString("EntityId", this.EntityId.ToString());
                    xmlWriter.WriteAttributeString("GetOnlyShowAtCreationAttributes", this.GetOnlyShowAtCreationAttributes.ToString());
                    xmlWriter.WriteAttributeString("GetOnlyRequiredAttributes", this.GetOnlyRequiredAttributes.ToString());
                    xmlWriter.WriteAttributeString("GetCompleteDetailsOfAttribute", this.GetCompleteDetailsOfAttribute.ToString());
                    xmlWriter.WriteAttributeString("ApplySorting", this.ApplySorting.ToString());

                    #endregion write Attribute Model Context for Compact Xml
                }

                //AttributeModelContext node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                returnXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            return returnXml;
        }

        /// <summary>
        /// Read the input xml and set the values to attribute model context
        /// </summary>
        /// <param name="valuesAsXml">Attribute Model Context as xml</param>
        private void LoadAttributeModelContext(String valuesAsXml)
        {
            #region Sample Xml

            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeModelContext")
                    {
                        #region Read EntityContext Properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("AttributeModelType"))
                            {
                                AttributeModelType modelType = Core.AttributeModelType.All;
                                Enum.TryParse(reader.ReadContentAsString(), out modelType);
                                this.AttributeModelType = modelType;
                            }

                            if (reader.MoveToAttribute("ContainerId"))
                            {
                                this.ContainerId = reader.ReadContentAsInt();
                            }

                            if (reader.MoveToAttribute("EntityTypeId"))
                            {
                                this.EntityTypeId = reader.ReadContentAsInt();
                            }

                            if (reader.MoveToAttribute("RelationshipTypeId"))
                            {
                                this.RelationshipTypeId = reader.ReadContentAsInt();
                            }

                            if (reader.MoveToAttribute("CategoryId"))
                            {
                                this.CategoryId = reader.ReadContentAsLong();
                            }

                            if (reader.MoveToAttribute("Locales"))
                            {
                                String strLocales = reader.GetAttribute("Locales");
                                this.Locales = ValueTypeHelper.SplitStringToLocaleEnumCollection(strLocales, ',');
                            }

                            if (reader.MoveToAttribute("EntityId"))
                            {
                                this.EntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("GetOnlyRequiredAttributes"))
                            {
                                this.GetOnlyRequiredAttributes = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("GetOnlyShowAtCreationAttributes"))
                            {
                                this.GetOnlyShowAtCreationAttributes = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("GetCompleteDetailsOfAttribute"))
                            {
                                this.GetCompleteDetailsOfAttribute = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("ApplySorting"))
                            {
                                this.ApplySorting = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadStateValidationAttributes"))
                            {
                                this.LoadStateValidationAttributes = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
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

        #endregion
    }
}
