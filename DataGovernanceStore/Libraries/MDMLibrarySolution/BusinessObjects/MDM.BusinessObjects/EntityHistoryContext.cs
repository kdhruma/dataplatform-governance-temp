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
    /// Class to define Entity level history context. 
    /// </summary>
    [DataContract]
    public class EntityHistoryContext : IEntityHistoryContext
    {
        #region Fields

        /// <summary>
        /// Field which denotes list of attribute ids for which history is requested
        /// </summary>
        private Collection<Int32> _attributeIdList = null;

        /// <summary>
        /// Field which denotes list of relationship type ids for which history is requested
        /// </summary>
        private Collection<Int32> _relationshipTypeIdList = null;

        /// <summary>
        /// Field which denotes relationship id for which history is requested
        /// </summary>
        private Int64 _relationshipId = 0;

        /// <summary>
        /// Field which denotes container id of an entity for which history is requested
        /// </summary>
        private Int32 _containerId = -1;

        /// <summary>
        /// Field which denotes starting(from) date time 
        /// </summary>
        private DateTime? _fromDateTime = null;

        /// <summary>
        /// Field which denotes end(to) date time
        /// </summary>
        private DateTime? _toDateTime = null;

        /// <summary>
        /// Field which denotes whether to load metadata version details 
        /// </summary>
        private Boolean _loadMetadataVersionDetails = true;

        /// <summary>
        /// Field which denotes whether to load attribute version details 
        /// </summary>
        private Boolean _loadAttributesVersionDetails = true;

        /// <summary>
        /// Field which denotes whether to load relationship attribute version details 
        /// </summary>
        private Boolean _loadRelationshipsVersionDetails = true;

        /// <summary>
        /// Field which denotes whether to load extension relationship version details 
        /// </summary>
        private Boolean _loadExtensionRelationshipsVersionDetails = true;

        /// <summary>
        /// Field which denotes whether to load hierarchy relationship version details 
        /// </summary>
        private Boolean _loadHierarchyRelationshipsVersionDetails = true;

        /// <summary>
        /// Field which denotes whether to load workflow version details 
        /// </summary>
        private Boolean _loadWorkflowVersionDetails = true;

        /// <summary>
        /// Field which denotes whether requested history is for entity or category.
        /// </summary>
        private Boolean _isHistoryForCategory = false;

        /// <summary>
        /// Field denoting current data locale in which data model has to be displayed
        /// </summary>
        private LocaleEnum _currentDataLocale = LocaleEnum.en_WW;

        /// <summary>
        /// Field denoting current UI locale
        /// </summary>
        private LocaleEnum _currentUILocale = LocaleEnum.en_WW;

        /// <summary>
        /// Field which denotes number of records to return in result
        /// </summary>
        private Int32 _maxRecordsToReturn = -1;

        /// <summary>
        /// Field denoting whether to load promote version details
        /// </summary>
        private bool _loadPromoteVersionDetails = true;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public EntityHistoryContext()
        { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public EntityHistoryContext(String valuesAsXml)
        {
            LoadEntityHistoryContext(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting attribute id list for which item level history is requested
        /// </summary>
        [DataMember]
        public Collection<Int32> AttributeIdList
        {
            get { return _attributeIdList; }
            set { _attributeIdList = value; }
        }

        /// <summary>
        /// Property denoting relationship type id list for which item level history is requested
        /// </summary>
        [DataMember]
        public Collection<Int32> RelationshipTypeIdList
        {
            get { return _relationshipTypeIdList; }
            set { _relationshipTypeIdList = value; }
        }

        /// <summary>
        /// Specifies relationship id for which item level history is requested
        /// </summary>
        [DataMember]
        public Int64 RelationshipId
        {
            get { return _relationshipId; }
            set { _relationshipId = value; }
        }

        /// <summary>
        /// Property denoting container id of an entity for which history is requested
        /// </summary>
        [DataMember]
        public Int32 ContainerId
        {
            get { return _containerId; }
            set { _containerId = value; }
        }

        /// <summary>
        /// Property denoting history's start(from) date time
        /// </summary>
        [DataMember]
        public DateTime? FromDateTime
        {
            get { return _fromDateTime; }
            set { _fromDateTime = value; }
        }

        /// <summary>
        /// Property to denote history's end(to) date time
        /// </summary>
        [DataMember]
        public DateTime? ToDateTime
        {
            get { return _toDateTime; }
            set { _toDateTime = value; }
        }

        /// <summary>
        /// Property denoting whether to load metadata version details 
        /// </summary>
        [DataMember]
        public Boolean LoadMetadataVersionDetails
        {
            get { return _loadMetadataVersionDetails; }
            set { _loadMetadataVersionDetails = value; }
        }

        /// <summary>
        /// Property denoting whether to load attribute version details 
        /// </summary>
        [DataMember]
        public Boolean LoadAttributesVersionDetails
        {
            get { return _loadAttributesVersionDetails; }
            set { _loadAttributesVersionDetails = value; }
        }

        /// <summary>
        /// Property denoting whether to load relationship attribute version details 
        /// </summary>
        [DataMember]
        public Boolean LoadRelationshipsVersionDetails
        {
            get { return _loadRelationshipsVersionDetails; }
            set { _loadRelationshipsVersionDetails = value; }
        }

        /// <summary>
        /// Property denoting whether to load extension relationship version details 
        /// </summary>
        [DataMember]
        public Boolean LoadExtensionRelationshipsVersionDetails
        {
            get { return _loadExtensionRelationshipsVersionDetails; }
            set { _loadExtensionRelationshipsVersionDetails = value; }
        }

        /// <summary>
        /// Property denoting whether to load hierarchy relationship version details 
        /// </summary>
        [DataMember]
        public Boolean LoadHierarchyRelationshipsVersionDetails
        {
            get { return _loadHierarchyRelationshipsVersionDetails; }
            set { _loadHierarchyRelationshipsVersionDetails = value; }
        }

        /// <summary>
        /// Property denoting whether to load workflow details or not 
        /// </summary>
        [DataMember]
        public Boolean LoadWorkflowVersionDetails
        {
            get { return _loadWorkflowVersionDetails; }
            set { _loadWorkflowVersionDetails = value; }
        }

        /// <summary>
        /// Property denoting whether to load promote version details or not.
        /// </summary>
        [DataMember]
        public Boolean LoadPromoteVersionDetails
        {
            get { return _loadPromoteVersionDetails; }
            set { _loadPromoteVersionDetails = value; }
        }

        /// <summary>
        /// Property denoting whether requested history is for entity or category.
        /// </summary>
        [DataMember]
        public Boolean IsHistoryForCategory
        {
            get { return _isHistoryForCategory; }
            set { _isHistoryForCategory = value; }
        }

        /// <summary>
        /// Property denoting current data locale in which data model has to be displayed
        /// </summary>
        [DataMember]
        public LocaleEnum CurrentDataLocale
        {
            get { return _currentDataLocale; }
            set { _currentDataLocale = value; }
        }

        /// <summary>
        /// Property denoting current UI locale
        /// </summary>
        [DataMember]
        public LocaleEnum CurrentUILocale
        {
            get { return _currentUILocale; }
            set { _currentUILocale = value; }
        }

        /// <summary>
        /// Property denoting number of records to return in result
        /// </summary>
        [DataMember]
        public Int32 MaxRecordsToReturn
        {
            get { return _maxRecordsToReturn; }
            set { _maxRecordsToReturn = value; }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Represents EntityHistoryContext  in Xml format
        /// </summary>
        /// <returns>String representation of current EntityHistoryContext object</returns>
        public String ToXml()
        {
            String entityHistoryContextxml = String.Empty;
            StringWriter stringWriter = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);

            String attributeIds = String.Empty;
            String relationshipTypeIds = String.Empty;
            String fromDateTime = String.Empty;
            String toDateTime = String.Empty;

            if (this.AttributeIdList != null)
            {
                attributeIds = ValueTypeHelper.JoinCollection(this.AttributeIdList, ",");
            }

            if (this.RelationshipTypeIdList != null)
            {
                relationshipTypeIds = ValueTypeHelper.JoinCollection(this.RelationshipTypeIdList, ",");
            }

            if (this.FromDateTime != null)
            {
                fromDateTime = this.FromDateTime.ToString();
            }

            if (this.ToDateTime != null)
            {
                toDateTime = this.ToDateTime.ToString();
            }

            // Attribute node start
            xmlWriter.WriteStartElement("EntityHistoryContext");

            xmlWriter.WriteAttributeString("AttributeIdList", attributeIds);
            xmlWriter.WriteAttributeString("RelationshipTypeIdList", relationshipTypeIds);
            xmlWriter.WriteAttributeString("RelationshipId", _relationshipId.ToString());
            xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
            xmlWriter.WriteAttributeString("FromDateTime", fromDateTime);
            xmlWriter.WriteAttributeString("ToDateTime", toDateTime);
            xmlWriter.WriteAttributeString("LoadMetadataVersionDetails", this.LoadMetadataVersionDetails.ToString());
            xmlWriter.WriteAttributeString("LoadAttributesVersionDetails", this.LoadAttributesVersionDetails.ToString());
            xmlWriter.WriteAttributeString("LoadRelationshipsVersionDetails", this.LoadRelationshipsVersionDetails.ToString());
            xmlWriter.WriteAttributeString("LoadExtensionRelationshipsVersionDetails", this.LoadExtensionRelationshipsVersionDetails.ToString());
            xmlWriter.WriteAttributeString("LoadHierarchyRelationshipsVersionDetails", this.LoadHierarchyRelationshipsVersionDetails.ToString());
            xmlWriter.WriteAttributeString("LoadWorkflowVersionDetails", this.LoadWorkflowVersionDetails.ToString());
            xmlWriter.WriteAttributeString("IsHistoryForCategory", this.IsHistoryForCategory.ToString());
            xmlWriter.WriteAttributeString("CurrentDataLocale", this.CurrentDataLocale.ToString());
            xmlWriter.WriteAttributeString("CurrentUILocale", this.CurrentUILocale.ToString());
            xmlWriter.WriteAttributeString("MaxRecordsToReturn", this.MaxRecordsToReturn.ToString());
            xmlWriter.WriteAttributeString("LoadPromoteVersionDetails", this.LoadPromoteVersionDetails.ToString());

            // EntityHistoryContext end node
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            // get the actual XML
            entityHistoryContextxml = stringWriter.ToString();

            xmlWriter.Close();
            stringWriter.Close();

            return entityHistoryContextxml;
        }

        /// <summary>
        /// clones the object to another object
        /// </summary>
        /// <returns></returns>
        public EntityHistoryContext Clone()
        {
            EntityHistoryContext clonedEntityHistoryContext = new EntityHistoryContext();

            clonedEntityHistoryContext._attributeIdList = this._attributeIdList;
            clonedEntityHistoryContext._relationshipTypeIdList = this._relationshipTypeIdList;
            clonedEntityHistoryContext._relationshipId = this._relationshipId;
            clonedEntityHistoryContext._containerId = this._containerId;
            clonedEntityHistoryContext._fromDateTime = this._fromDateTime;
            clonedEntityHistoryContext._toDateTime = this._toDateTime;
            clonedEntityHistoryContext._loadAttributesVersionDetails = this._loadAttributesVersionDetails;
            clonedEntityHistoryContext._loadMetadataVersionDetails = this._loadMetadataVersionDetails;
            clonedEntityHistoryContext._loadRelationshipsVersionDetails = this._loadRelationshipsVersionDetails;
            clonedEntityHistoryContext._loadExtensionRelationshipsVersionDetails = this._loadExtensionRelationshipsVersionDetails;
            clonedEntityHistoryContext._loadHierarchyRelationshipsVersionDetails = this._loadHierarchyRelationshipsVersionDetails;
            clonedEntityHistoryContext._loadWorkflowVersionDetails = this._loadWorkflowVersionDetails;
            clonedEntityHistoryContext._isHistoryForCategory = this._isHistoryForCategory;
            clonedEntityHistoryContext._currentDataLocale = this._currentDataLocale;
            clonedEntityHistoryContext._currentUILocale = this._currentUILocale;
            clonedEntityHistoryContext._maxRecordsToReturn = this._maxRecordsToReturn;
            clonedEntityHistoryContext._loadPromoteVersionDetails = this._loadPromoteVersionDetails;

            return clonedEntityHistoryContext;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            Int32 hashCode = this.AttributeIdList.GetHashCode()
                ^ this.RelationshipTypeIdList.GetHashCode()
                ^ this.RelationshipId.GetHashCode()
                ^ this.ContainerId.GetHashCode()
                ^ this.FromDateTime.GetHashCode()
                ^ this.ToDateTime.GetHashCode()
                ^ this.LoadMetadataVersionDetails.GetHashCode()
                ^ this.LoadAttributesVersionDetails.GetHashCode()
                ^ this.LoadRelationshipsVersionDetails.GetHashCode()
                ^ this.LoadExtensionRelationshipsVersionDetails.GetHashCode()
                ^ this.LoadHierarchyRelationshipsVersionDetails.GetHashCode()
                ^ this.LoadWorkflowVersionDetails.GetHashCode()
                ^ this.IsHistoryForCategory.GetHashCode()
                ^ this.CurrentDataLocale.GetHashCode()
                ^ this.CurrentUILocale.GetHashCode()
                ^ this.MaxRecordsToReturn.GetHashCode()
                ^ this.LoadPromoteVersionDetails.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Compare EntityHistoryContext with current EntityHistoryContext.
        /// </summary>
        /// <param name="entityHistoryContext">EntityHistoryContext object which has to be compared with current EntityHistoryContext</param>
        /// <param name="compareIds">Flag to say whether Id based properties has to be considered in comparison or not.</param>
        /// <returns>True : Is both are same. False : otherwise</returns>
        public Boolean Equals(EntityHistoryContext entityHistoryContext, Boolean compareIds = false)
        {
            if (compareIds)
            {
                if (this.AttributeIdList != entityHistoryContext.AttributeIdList)
                {
                    return false;
                }

                if (this.RelationshipTypeIdList != entityHistoryContext.RelationshipTypeIdList)
                {
                    return false;
                }

                if (this.ContainerId != entityHistoryContext.ContainerId)
                {
                    return false;
                }

                if (this.RelationshipId != entityHistoryContext.RelationshipId)
                {
                    return false;
                }
            }

            if (this.LoadMetadataVersionDetails != entityHistoryContext.LoadMetadataVersionDetails)
            {
                return false;
            }

            if (this.LoadAttributesVersionDetails != entityHistoryContext.LoadAttributesVersionDetails)
            {
                return false;
            }

            if (this.LoadRelationshipsVersionDetails != entityHistoryContext.LoadRelationshipsVersionDetails)
            {
                return false;
            }

            if (this.LoadExtensionRelationshipsVersionDetails != entityHistoryContext.LoadExtensionRelationshipsVersionDetails)
            {
                return false;
            }

            if (this.LoadHierarchyRelationshipsVersionDetails != entityHistoryContext.LoadHierarchyRelationshipsVersionDetails)
            {
                return false;
            }

            if (this.LoadWorkflowVersionDetails != entityHistoryContext.LoadWorkflowVersionDetails)
            {
                return false;
            }

            if (this.IsHistoryForCategory != entityHistoryContext.IsHistoryForCategory)
            {
                return false;
            }

            if (this.CurrentDataLocale != entityHistoryContext.CurrentDataLocale)
            {
                return false;
            }

            if (this.CurrentUILocale != entityHistoryContext.CurrentUILocale)
            {
                return false;
            }

            if (this.MaxRecordsToReturn != entityHistoryContext.MaxRecordsToReturn)
            {
                return false;
            }

            if (this.LoadPromoteVersionDetails != entityHistoryContext.LoadPromoteVersionDetails)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Load EntityHistoryContext object from xml.
        /// </summary>
        /// <param name="valuesAsXml">XMl having values for object to load</param>
        private void LoadEntityHistoryContext(String valuesAsXml)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityHistoryContext")
                    {
                        #region Read EntityHistoryContext Properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("AttributeIdList"))
                            {
                                this.AttributeIdList = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                            }

                            if (reader.MoveToAttribute("RelationshipTypeIdList"))
                            {
                                this.RelationshipTypeIdList = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                            }

                            if (reader.MoveToAttribute("RelationshipId"))
                            {
                                this.RelationshipId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), _relationshipId);
                            }

                            if (reader.MoveToAttribute("ContainerId"))
                            {
                                this.ContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), _containerId);
                            }

                            if (reader.MoveToAttribute("FromDateTime"))
                            {
                                this.FromDateTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("ToDateTime"))
                            {
                                this.ToDateTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadMetadataVersionDetails"))
                            {
                                this.LoadMetadataVersionDetails = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadAttributesVersionDetails"))
                            {
                                this.LoadAttributesVersionDetails = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadRelationshipsVersionDetails"))
                            {
                                this.LoadRelationshipsVersionDetails = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadExtensionRelationshipsVersionDetails"))
                            {
                                this.LoadExtensionRelationshipsVersionDetails = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadHierarchyRelationshipsVersionDetails"))
                            {
                                this.LoadHierarchyRelationshipsVersionDetails = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadWorkflowVersionDetails"))
                            {
                                this.LoadWorkflowVersionDetails = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("IsHistoryForCategory"))
                            {
                                this.IsHistoryForCategory = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("CurrentDataLocale"))
                            {
                                LocaleEnum currentDataLocale = LocaleEnum.UnKnown;
                                Enum.TryParse<LocaleEnum>(reader.ReadContentAsString(), out currentDataLocale);
                                this.CurrentDataLocale = currentDataLocale;
                            }

                            if (reader.MoveToAttribute("CurrentUILocale"))
                            {
                                LocaleEnum currentUILocale = LocaleEnum.UnKnown;
                                Enum.TryParse<LocaleEnum>(reader.ReadContentAsString(), out currentUILocale);
                                this.CurrentUILocale = currentUILocale;
                            }

                            if (reader.MoveToAttribute("MaxRecordsToReturn"))
                            {
                                this.MaxRecordsToReturn = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), _maxRecordsToReturn);
                            }

                            if(reader.MoveToAttribute("LoadPromoteVersionDetails"))
                            {
                                this.LoadPromoteVersionDetails = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        //Keep on reading the XML until we reach expected node.
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