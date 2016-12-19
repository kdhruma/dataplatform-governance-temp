using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies mapping object for RelationshipTypeAttributeMapping
    /// </summary>
    [DataContract]
    public class RelationshipTypeAttributeMapping : MDMObject, IRelationshipTypeAttributeMapping, IDataModelObject
    {
        #region Fields

        /// <summary>
        /// Field denoting the relationshipType id
        /// </summary>
        private Int32 _relationshipTypeId = -1;

        /// <summary>
        /// Field denoting the descriptive short name of relationshipType
        /// </summary>
        private String _relationshipTypeName = String.Empty;

        /// <summary>
        /// Field denoting the descriptive long name of relationshipType
        /// </summary>
        private String _relationshipTypeLongName = String.Empty;

        /// <summary>
        /// Field denoting the attribute id
        /// </summary>
        private Int32 _attributeId = -1;

        /// <summary>
        /// Field denoting the short name of attribute
        /// </summary>
        private String _attributeName = String.Empty;

        /// <summary>
        /// Field denoting the long name of attribute
        /// </summary>
        private String _attributeLongName = String.Empty;

        /// <summary>
        /// Field denoting the attribute parent id
        /// </summary>
        private Int32 _attributeParentId = -1;

        /// <summary>
        /// Field denoting the short name of attribute parent
        /// </summary>
        private String _attributeParentName = String.Empty;

        /// <summary>
        /// Field denoting the long name of attribute parent
        /// </summary>
        private String _attributeParentLongName = String.Empty;

        /// <summary>
        /// Field denoting the required property of mapping
        /// </summary>
        private Boolean _required = false;

        /// <summary>
        /// Field denoting the read only property of mapping
        /// </summary>
        private Boolean _readOnly = false;

        /// <summary>
        /// Field denoting the show at creation property of mapping
        /// </summary>
        private Boolean _showAtCreation = false;

        /// <summary>
        /// Field denoting the sort order property of mapping
        /// </summary>
        private Int32 _sortOrder = 0;

        /// <summary>
        /// Field denoting the ShowInline property of mapping
        /// </summary>
        private Boolean _showInline = false;

        /// <summary>
        /// Field denoting the sort order property of mapping
        /// </summary>
        private RelationshipTypeAttributeMapping _originalRelationshipTypeAttributeMapping = null;

        /// <summary>
        /// Field denoting the sort order property of mapping
        /// </summary>
        private String _externalId = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public RelationshipTypeAttributeMapping()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of a RelationshipType - Entity Type - Attribute Mapping</param>
        public RelationshipTypeAttributeMapping(Int32 id)
            : base(id)
        {

        }

        /// <summary>
        /// Constructor with relationshipType Id and Attribute Id as input parameters
        /// </summary>
        /// <param name="relationshipTypeId">Indicates the relationshipType Id</param>
        /// <param name="attributeId">Indicates the Attribute Id</param>
        public RelationshipTypeAttributeMapping(Int32 relationshipTypeId, Int32 attributeId)
        {
            this._relationshipTypeId = relationshipTypeId;
        }

        /// <summary>
        /// Initialize new instance of RelationshipTypeAttributeMapping from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for RelationshipTypeAttributeMapping</param>
        public RelationshipTypeAttributeMapping(String valuesAsXml)
        {
            LoadRelationshipTypeAttributeMapping(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property defining the type of the MDM object
        /// </summary>
        public new String ObjectType
        {
            get
            {
                return "RelationshipTypeAttributeMapping";
            }
        }

        /// <summary>
        /// Property denoting mapped relationshipType Id
        /// </summary>
        [DataMember]
        public Int32 RelationshipTypeId
        {
            get { return _relationshipTypeId; }
            set { _relationshipTypeId = value; }
        }

        /// <summary>
        /// Property denoting the mapped relationshipType Short Name
        /// </summary>
        [DataMember]
        public String RelationshipTypeName
        {
            get { return _relationshipTypeName; }
            set { _relationshipTypeName = value; }
        }

        /// <summary>
        /// Property denoting the mapped relationshipType Name
        /// </summary>
        [DataMember]
        public String RelationshipTypeLongName
        {
            get { return _relationshipTypeLongName; }
            set { _relationshipTypeLongName = value; }
        }

        /// <summary>
        /// Property denoting mapped Attribute Id
        /// </summary>
        [DataMember]
        public Int32 AttributeId
        {
            get { return _attributeId; }
            set { _attributeId = value; }
        }

        /// <summary>
        /// Property denoting the mapped Attribute Short Name
        /// </summary>
        [DataMember]
        public String AttributeName
        {
            get { return _attributeName; }
            set { _attributeName = value; }
        }

        /// <summary>
        /// Property denoting the mapped Attribute Name
        /// </summary>
        [DataMember]
        public String AttributeLongName
        {
            get { return _attributeLongName; }
            set { _attributeLongName = value; }
        }

        /// <summary>
        /// Property denoting mapped Attribute Parent Id
        /// </summary>
        [DataMember]
        public Int32 AttributeParentId
        {
            get { return _attributeParentId; }
            set { _attributeParentId = value; }
        }

        /// <summary>
        /// Property denoting the mapped Attribute Parent Short Name
        /// </summary>
        [DataMember]
        public String AttributeParentName
        {
            get { return _attributeParentName; }
            set { _attributeParentName = value; }
        }

        /// <summary>
        /// Property denoting the mapped Attribute Parent Name
        /// </summary>
        [DataMember]
        public String AttributeParentLongName
        {
            get { return _attributeParentLongName; }
            set { _attributeParentLongName = value; }
        }

        /// <summary>
        /// Property denoting the Required property for this Mapping
        /// </summary>
        [DataMember]
        public Boolean Required
        {
            get { return _required; }
            set { _required = value; }
        }

        /// <summary>
        /// Property denoting the Read Only property for this Mapping
        /// </summary>
        [DataMember]
        public Boolean ReadOnly
        {
            get { return _readOnly; }
            set { _readOnly = value; }
        }

        /// <summary>
        /// Property denoting the Show At Creation property for this Mapping
        /// </summary>
        [DataMember]
        public Boolean ShowAtCreation
        {
            get { return _showAtCreation; }
            set { _showAtCreation = value; }
        }

        /// <summary>
        /// Property denoting the Sort Order property for this Mapping
        /// </summary>
        [DataMember]
        public Int32 SortOrder
        {
            get { return _sortOrder; }
            set { _sortOrder = value; }
        }

        /// <summary>
        /// Property denoting the ShowInline Property
        /// </summary>
        [DataMember]
        public Boolean ShowInline
        {
            get { return _showInline; }
            set { _showInline = value; }
        }

        /// <summary>
        /// Property denoting the OriginalRelationshipTypeAttributeMapping
        /// </summary>
        public RelationshipTypeAttributeMapping OriginalRelationshipTypeAttributeMapping
        {
            get { return _originalRelationshipTypeAttributeMapping; }
            set { _originalRelationshipTypeAttributeMapping = value; }
        }

        #region IDataModelObject Properties

        /// <summary>
        /// Property denoting the ExternalId property for this Mapping
        /// </summary>
        public string ExternalId
        {
            get { return _externalId; }
            set { _externalId = value; }
        }

        /// <summary>
        /// Property denoting a ObjectType for an dataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return MDM.Core.ObjectType.RelationshipTypeAttributeMapping;
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Object represents itself as XML
        /// </summary>
        /// <returns>string representation of XML of the object</returns>
        public String ToXML()
        {
            String xml = string.Empty;

            xml = "<RelationshipTypeAttribute Id=\"{0}\" RelationshipTypeId = \"{1}\" AttributeId = \"{2}\" FK_InheritanceMode = \"{3}\" ShowAtCreation = \"{4}\" Required = \"{5}\" ReadOnly = \"{6}\"  SortOrder = \"{7}\" ShowInline = \"{8}\" Action=\"{9}\" />    ";

            string retXML = string.Format(xml, this.Id, this.RelationshipTypeId, this.AttributeId, "2", this.ShowAtCreation, ((this.Required) ? 'Y' : 'N'), this.ReadOnly, this.SortOrder, this.ShowInline, this.Action);

            return retXML;
        }

        /// <summary>
        /// Clone RelationshipTypeAttributeMapping object
        /// </summary>
        /// <returns>cloned copy of IRelationshipTypeAttributeMapping object.</returns>
        public IRelationshipTypeAttributeMapping Clone()
        {
            RelationshipTypeAttributeMapping clonedRelationshipTypeAttributeMapping = new RelationshipTypeAttributeMapping();

            clonedRelationshipTypeAttributeMapping.Id = this.Id;
            clonedRelationshipTypeAttributeMapping.Name = this.Name;
            clonedRelationshipTypeAttributeMapping.LongName = this.LongName;
            clonedRelationshipTypeAttributeMapping.Locale = this.Locale;
            clonedRelationshipTypeAttributeMapping.Action = this.Action;
            clonedRelationshipTypeAttributeMapping.AuditRefId = this.AuditRefId;
            clonedRelationshipTypeAttributeMapping.ExtendedProperties = this.ExtendedProperties;

            clonedRelationshipTypeAttributeMapping.RelationshipTypeId = this.RelationshipTypeId;
            clonedRelationshipTypeAttributeMapping.RelationshipTypeName = this.RelationshipTypeName;
            clonedRelationshipTypeAttributeMapping.RelationshipTypeLongName = this.RelationshipTypeLongName;
            clonedRelationshipTypeAttributeMapping.AttributeId = this.AttributeId;
            clonedRelationshipTypeAttributeMapping.AttributeName = this.AttributeName;
            clonedRelationshipTypeAttributeMapping.AttributeLongName = this.AttributeLongName;
            clonedRelationshipTypeAttributeMapping.AttributeParentId = this.AttributeParentId;
            clonedRelationshipTypeAttributeMapping.AttributeParentName = this.AttributeParentName;
            clonedRelationshipTypeAttributeMapping.AttributeParentLongName = this.AttributeParentLongName;
            clonedRelationshipTypeAttributeMapping.Required = this.Required;
            clonedRelationshipTypeAttributeMapping.ReadOnly = this.ReadOnly;
            clonedRelationshipTypeAttributeMapping.ShowAtCreation = this.ShowAtCreation;
            clonedRelationshipTypeAttributeMapping.SortOrder = this.SortOrder;
            clonedRelationshipTypeAttributeMapping.ShowInline = this.ShowInline;

            return clonedRelationshipTypeAttributeMapping;

        }

        /// <summary>
        /// Delta Merge of RelationshipTypeAttributeMapping
        /// </summary>
        /// <param name="deltaRelationshipTypeAttributeMapping">RelationshipTypeAttributeMapping that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged RelationshipTypeAttributeMapping instance</returns>
        public IRelationshipTypeAttributeMapping MergeDelta(IRelationshipTypeAttributeMapping deltaRelationshipTypeAttributeMapping, ICallerContext iCallerContext, bool returnClonedObject = true)
        {
            IRelationshipTypeAttributeMapping mergedRelationshipTypeAttributeMapping = (returnClonedObject == true) ? deltaRelationshipTypeAttributeMapping.Clone() : deltaRelationshipTypeAttributeMapping;
            mergedRelationshipTypeAttributeMapping.Action = (mergedRelationshipTypeAttributeMapping.Equals(this)) ? ObjectAction.Read : ObjectAction.Update;

            return mergedRelationshipTypeAttributeMapping;
        }

        /// <summary>
        /// Compare relationship type attribute mapping with current relationship type attribute mapping .
        /// This method will compare relationship type attribute mapping .
        /// </summary>
        /// <param name="subsetRelationshipTypeAttributeMapping">Relationship type attribute mapping to be compared with current relationship type attribute mapping.</param>
        /// <param name="compareIds">Flag to determine whether id based comparison is true or not</param>
        /// <returns>True : Is both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(RelationshipTypeAttributeMapping subsetRelationshipTypeAttributeMapping, Boolean compareIds = false)
        {
            if (compareIds)
            {
                if (this.Id != subsetRelationshipTypeAttributeMapping.Id)
                    return false;

                if (this.RelationshipTypeId != subsetRelationshipTypeAttributeMapping.RelationshipTypeId)
                    return false;

                if (this.AttributeId != subsetRelationshipTypeAttributeMapping.AttributeId)
                    return false;

                if (this.AttributeParentId != subsetRelationshipTypeAttributeMapping.AttributeParentId)
                    return false;
            }

            if (this.RelationshipTypeName != subsetRelationshipTypeAttributeMapping.RelationshipTypeName)
                return false;

            if (this.AttributeName != subsetRelationshipTypeAttributeMapping.AttributeName)
                return false;

            if (this.AttributeParentName != subsetRelationshipTypeAttributeMapping.AttributeParentName)
                return false;

            if (this.ReadOnly != subsetRelationshipTypeAttributeMapping.ReadOnly)
                return false;

            if (this.Required != subsetRelationshipTypeAttributeMapping.Required)
                return false;

            if (this.Action != subsetRelationshipTypeAttributeMapping.Action)
                return false;

            if (this.ShowInline != subsetRelationshipTypeAttributeMapping.ShowInline)
                return false;

            if (this.SortOrder != subsetRelationshipTypeAttributeMapping.SortOrder)
                return false;

            if (this.ShowAtCreation != subsetRelationshipTypeAttributeMapping.ShowAtCreation)
                return false;

            return true;
        }

        #region Overrides

        /// <summary>
        /// Determines whether specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">RelationshipTypeAttributeMapping object which needs to be compared.</param>
        /// <returns>Result of the comparison in Boolean.</returns>
        public override bool Equals(object obj)
        {
            if (obj is RelationshipTypeAttributeMapping)
            {
                RelationshipTypeAttributeMapping objectToBeCompared = obj as RelationshipTypeAttributeMapping;

                if (!base.Equals(objectToBeCompared))
                    return false;

                if (this.RelationshipTypeId != objectToBeCompared.RelationshipTypeId)
                    return false;

                if (this.RelationshipTypeName != objectToBeCompared.RelationshipTypeName)
                    return false;

                if (this.RelationshipTypeLongName != objectToBeCompared.RelationshipTypeLongName)
                    return false;

                if (this.AttributeId != objectToBeCompared.AttributeId)
                    return false;

                if (this.AttributeName != objectToBeCompared.AttributeName)
                    return false;

                if (this.AttributeLongName != objectToBeCompared.AttributeLongName)
                    return false;

                if (this.AttributeParentId != objectToBeCompared.AttributeParentId)
                    return false;

                if (this.AttributeParentName != objectToBeCompared.AttributeParentName)
                    return false;

                if (this.AttributeParentLongName != objectToBeCompared.AttributeParentLongName)
                    return false;

                if (this.Required != objectToBeCompared.Required)
                    return false;

                if (this.ReadOnly != objectToBeCompared.ReadOnly)
                    return false;

                if (this.ShowAtCreation != objectToBeCompared.ShowAtCreation)
                    return false;

                if (this.SortOrder != objectToBeCompared.SortOrder)
                    return false;

                if (this.ShowInline != objectToBeCompared.ShowInline)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance
        /// </summary>
        /// <returns>Hash code of the instance</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ this.RelationshipTypeId.GetHashCode() ^ this.RelationshipTypeName.GetHashCode() ^ this.RelationshipTypeLongName.GetHashCode() ^ this.AttributeId.GetHashCode() ^ this.AttributeName.GetHashCode() ^ this.AttributeLongName.GetHashCode() ^ this.AttributeParentId.GetHashCode()
                   ^ this.AttributeParentName.GetHashCode() ^ this.AttributeParentLongName.GetHashCode() ^ this.Required.GetHashCode() ^ this.ReadOnly.GetHashCode() ^ this.ShowAtCreation.GetHashCode() ^ this.ShowInline.GetHashCode() ^ this.SortOrder.GetHashCode();
        }

        #endregion

        #region IDataModelObject

        /// <summary>
        /// Get IDataModelObject for current object.
        /// </summary>
        /// <returns>IDataModelObject</returns>
        public IDataModelObject GetDataModelObject()
        {
            return this;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialize current object with Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for current relationship type attribute mapping
        /// <para>
        /// Sample Xml:
        /// <![CDATA[
        ///     <RelationshipTypeAttribute Id="-1" RelationshipTypeId = "1" AttributeId = "4393" FK_InheritanceMode = "2" ShowAtCreation = "False" Required = "N" ReadOnly = "False"  SortOrder = "0" ShowInline = "False" Action="Create" />
        /// ]]>
        /// </para>
        /// </param>
        private void LoadRelationshipTypeAttributeMapping(String valuesAsXml)
        {
            #region Sample Xml
            /*
             <RelationshipTypeAttribute Id="-1" RelationshipTypeId = "1" AttributeId = "4393" FK_InheritanceMode = "2" ShowAtCreation = "False" Required = "N" ReadOnly = "False"  SortOrder = "0" ShowInline = "False" Action="Create" />
            */
            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "RelationshipTypeAttribute")
                        {
                            #region Read relationship type attributes

                            if (reader.HasAttributes)
                            {

                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("RelationshipTypeId"))
                                {
                                    this.RelationshipTypeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("RelationshipTypeName"))
                                {
                                    this.RelationshipTypeName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("AttributeId"))
                                {
                                    this.AttributeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("AttributeName"))
                                {
                                    this.AttributeName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("AttributeParentName"))
                                {
                                    this.AttributeParentName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Action"))
                                {
                                    this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("ReadOnly"))
                                {
                                    this.ReadOnly = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("Required"))
                                {
                                    this.Required = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("SortOrder"))
                                {
                                    this.SortOrder = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("ShowAtCreation"))
                                {
                                    this.ShowAtCreation = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("ShowInline"))
                                {
                                    this.ShowInline = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }
                            }

                            #endregion
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

        #endregion

        #endregion
    }
}
