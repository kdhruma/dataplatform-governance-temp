using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Relationship Context
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class RelationshipContext : MDMObject, IRelationshipContext
    {
        #region Fields

        /// <summary>
        /// Field denoting the data locales
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        private Collection<LocaleEnum> _dataLocales = new Collection<LocaleEnum>();

        /// <summary>
        /// Field denoting the container id
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        private Int32 _containerId = 0;

        /// <summary>
        /// Field denoting level upto which relationships are to be fetched.
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        private Int16 _level = 3;

        /// <summary>
        /// Field denoting relationship id.
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        private Int64 _relationshipId = 0;

        /// <summary>
        /// Field denoting relationship parent id.
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        private Int64 _relationshipParentId = 0;

        /// <summary>
        /// Field denoting the relationship types
        /// </summary>
        [DataMember]
        [ProtoMember(6)]
        private Collection<Int32> _relationshipTypeIdList = new Collection<Int32>();

        /// <summary>
        /// Field denoting the relationship types
        /// </summary>
        [DataMember]
        [ProtoMember(7)]
        private Collection<String> _relationshipTypeNames = null;

        /// <summary>
        /// Field denoting whether to load Relationship attributes or not
        /// </summary>
        [DataMember]
        [ProtoMember(8)]
        private Boolean _loadRelationshipAttributes = true;

        /// <summary>
        /// Field denoting whether to return related entity details.
        /// </summary>
        [DataMember]
        [ProtoMember(9)]
        private Boolean _returnRelatedEntityDetails = false;

        /// <summary>
        /// Field denoting whether to load related entities attributes or not
        /// </summary>
        [DataMember]
        [ProtoMember(10)]
        private Boolean _loadRelatedEntitiesAttributes = false;

        /// <summary>
        /// Field denoting which attributes are to be loaded.
        /// To load attributes from attribute ids given in related entities attributeIdList, LoadRelatedEntitiesAttributes should be set to true.
        /// </summary>
        [DataMember]
        [ProtoMember(11)]
        private Collection<Int32> _relatedEntitiesAttributeIdList = null;

        /// <summary>
        /// Field denoting which attributes are to be loaded.
        /// To load attributes from attribute name and group name given in related entities attributeNameGroupNameList, LoadRelatedEntitiesAttributes should be set to true.
        /// </summary>
        [DataMember]
        [ProtoMember(12)]
        private Collection<String> _relatedEntitiesAttributeNames = null;

        /// <summary>
        /// Field denoting whether to load sources or not.
        /// </summary>
        [DataMember]
        [ProtoMember(13)]
        private Boolean _loadSources = false;

        /// <summary>
        /// Field whether to get status of De-normalized relationships or not.
        /// </summary>
        [DataMember]
        [ProtoMember(14)]
        private Boolean _loadDenormalizedRelationshipsStatus = false;

        /// <summary>
        /// Field denoting whether to apply data model security for relationships or not.
        /// </summary>
        [DataMember]
        [ProtoMember(15)]
        private Boolean _applyDMS = true;

        /// <summary>
        /// Field denoting whether to load relationship attributes models or not
        /// </summary>
        [DataMember]
        [ProtoMember(16)]
        private Boolean _loadRelationshipAttributeModels = false;

        /// <summary>
        /// Field denoting whether to load relationship attributes having blank values / no value
        /// When set to true, relationship get will return attribute object instances having blank / no value
        /// </summary>
        [DataMember]
        [ProtoMember(17), DefaultValue(true)]
        private Boolean _loadBlankRelationshipAttributes = true;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public RelationshipContext()
            : base()
        {
        }

        /// <summary>
        /// Initialise Relationship Context 
        /// </summary>
        /// <param name="relationshipTypeIdList">List of relationship ids of relationships to load</param>
        /// <param name="loadRelationshipAttributes">Flag to decide whether to load relationship attributes or not</param>
        /// <param name="level">Level of relationships to load</param>
        /// <param name="locale">Locale for values</param>
        public RelationshipContext(Collection<Int32> relationshipTypeIdList, Boolean loadRelationshipAttributes, LocaleEnum locale, Int16 level = 10)
            : base()
        {
            this._relationshipTypeIdList = relationshipTypeIdList;
            this._loadRelationshipAttributes = loadRelationshipAttributes;
            this.Locale = locale;
            this._level = level;
        }

        /// <summary>
        /// Initialise RelationshipContext using Xml
        /// </summary>
        /// <param name="valuesAsXml">
        /// Zml having values
        /// <para> Sample:</para>
        /// <![CDATA[<RelationshipContext RelationshipTypeIdList="3001,3002" LoadRelationshipAttributes="true" Level="10" Locale="en_WW"/>]]>
        /// </param>
        public RelationshipContext(String valuesAsXml)
            : base()
        {
            LoadRelationshipContext(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Field denoting list of data locales
        /// </summary>
        public Collection<LocaleEnum> DataLocales
        {
            get
            {
                return this._dataLocales;
            }
            set
            {
                this._dataLocales = value;
            }
        }

        /// <summary>
        /// Property denoting the container id
        /// </summary>
        public Int32 ContainerId
        {
            get
            {
                return this._containerId;
            }
            set
            {
                this._containerId = value;
            }
        }

        /// <summary>
        /// Field denoting level up to which relationships are to be fetched.
        /// </summary>
        public Int16 Level
        {
            get
            {
                return this._level;
            }
            set
            {
                this._level = value;
            }
        }

        /// <summary>
        /// Property denoting the relationship id
        /// </summary>
        public Int64 RelationshipId
        {
            get
            {
                return this._relationshipId;
            }
            set
            {
                this._relationshipId = value;
            }
        }

        /// <summary>
        /// Specifies the relationship parent id
        /// </summary>
        public Int64 RelationshipParentId
        {
            get
            {
                return this._relationshipParentId;
            }
            set
            {
                this._relationshipParentId = value;
            }
        }

        /// <summary>
        /// Property denoting the relationship type id list
        /// </summary>
        public Collection<Int32> RelationshipTypeIdList
        {
            get
            {
                if (this._relationshipTypeIdList == null)
                {
                    this._relationshipTypeIdList = new Collection<Int32>();
            }
                return this._relationshipTypeIdList;
            }
            set
            {
                this._relationshipTypeIdList = value;
            }
        }

        /// <summary>
        /// Property denoting the relationship type name list
        /// </summary>
        public Collection<String> RelationshipTypeNames
        {
            set
            {
                this._relationshipTypeNames = value;
            }
            get
            {
                if (this._relationshipTypeNames == null)
                    this._relationshipTypeNames = new Collection<String>();

                return this._relationshipTypeNames;
            }
        }

        /// <summary>
        /// Field denoting whether to load Relationship attributes or not
        /// </summary>
        public Boolean LoadRelationshipAttributes
        {
            get
            {
                return this._loadRelationshipAttributes;
            }
            set
            {
                this._loadRelationshipAttributes = value;
            }
        }

        /// <summary>
        /// Specifies to return related entity details or not.
        /// This flag will set to false if common attributes are not configured.
        /// </summary>
        public Boolean ReturnRelatedEntityDetails
        {
            get
            {
                return _returnRelatedEntityDetails;
            }
            set
            {
                _returnRelatedEntityDetails = value;
            }
        }

        /// <summary>
        /// Field denoting whether to load related entities attributes or not
        /// </summary>
        public Boolean LoadRelatedEntitiesAttributes
        {
            get
            {
                return this._loadRelatedEntitiesAttributes;
            }
            set
            {
                this._loadRelatedEntitiesAttributes = value;
            }
        }

        /// <summary>
        /// Field denoting which attributes are to be loaded.
        /// To load attributes from attribute ids given in related entities attributeIdList, LoadRelatedEntitiesAttributes should be set to true.
        /// </summary>
        public Collection<Int32> RelatedEntitiesAttributeIdList
        {
            get
            {
                return this._relatedEntitiesAttributeIdList;
            }
            set
            {
                this._relatedEntitiesAttributeIdList = value;
            }
        }

        /// <summary>
        /// Field denoting which attributes are to be loaded.
        /// To load attributes from attribute name and group name given in related entities attributeNameGroupNameList, LoadRelatedEntitiesAttributes should be set to true.
        /// </summary>
        public Collection<String> RelatedEntitiesAttributeNames
        {
            get
            {
                if (this._relatedEntitiesAttributeNames == null)
                    this._relatedEntitiesAttributeNames = new Collection<String>();

                return this._relatedEntitiesAttributeNames;
            }
            set
            {
                this._relatedEntitiesAttributeNames = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load Source values or not.
        /// </summary>
        public Boolean LoadSources
        {
            get
            {
                return _loadSources;
            }
            set
            {
                _loadSources = value;
            }
        }

        /// <summary>
        /// Indicates whether to get status of De-normalized relationships or not.
        /// </summary>
        public Boolean LoadDenormalizedRelationshipsStatus
        {
            get
            {
                return _loadDenormalizedRelationshipsStatus;
            }
            set
            {
                _loadDenormalizedRelationshipsStatus = value;
            }
            }

        /// <summary>
        /// Specifies to apply data model security for relationships or not.
        /// </summary>
        public Boolean ApplyDMS
        {
            get { return _applyDMS; }
            set { _applyDMS = value; }
        }

        /// <summary>
        /// Field denoting whether to load relationship attribute models or not
        /// </summary>
        public Boolean LoadRelationshipAttributeModels
        {
            get
            {
                return this._loadRelationshipAttributeModels;
            }
            set
            {
                this._loadRelationshipAttributeModels = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load relationships attributes having blank values / no value
        /// When set to true, relationship get will return attribute object instances having blank / no value
        /// </summary>
        public Boolean LoadBlankRelationshipAttributes
        {
            get
            {
                return this._loadBlankRelationshipAttributes;
            }
            set
            {
                this._loadBlankRelationshipAttributes = value;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is RelationshipContext)
                {
                    RelationshipContext objectToBeCompared = obj as RelationshipContext;

                    if (!this.Locale.Equals(objectToBeCompared.Locale))
                    {
                        return false;
                    }

                    if (!this.ContainerId.Equals(objectToBeCompared.ContainerId))
                    {
                        return false;
                    }

                    if (!this.Level.Equals(objectToBeCompared.Level))
                    {
                        return false;
                    }

                    if (!this.RelationshipId.Equals(objectToBeCompared.RelationshipId))
                    {
                        return false;
                    }

                    if (!this.RelationshipParentId.Equals(objectToBeCompared.RelationshipParentId))
                    {
                        return false;
                    }

                    if (!this.LoadRelationshipAttributes.Equals(objectToBeCompared.LoadRelationshipAttributes))
                    {
                        return false;
                    }

                    if (!this.LoadRelatedEntitiesAttributes.Equals(objectToBeCompared.LoadRelatedEntitiesAttributes))
                    {
                        return false;
                    }

                    if (!this.ApplyDMS.Equals(objectToBeCompared.ApplyDMS))
                    {
                        return false;
                    }

                    if (!this.LoadDenormalizedRelationshipsStatus.Equals(objectToBeCompared.LoadDenormalizedRelationshipsStatus))
                    {
                        return false;
                    }

                    Int32 relatedEntitiesAttributeIdsUnion = this.RelatedEntitiesAttributeIdList.ToList().Union(objectToBeCompared.RelatedEntitiesAttributeIdList.ToList()).Count();
                    Int32 relatedEntitiesAttributeIdsIntersect = this.RelatedEntitiesAttributeIdList.ToList().Intersect(objectToBeCompared.RelatedEntitiesAttributeIdList.ToList()).Count();

                    if (relatedEntitiesAttributeIdsUnion != relatedEntitiesAttributeIdsIntersect)
                    {
                        return false;
                    }

                    Int32 relationshipTypeIdsUnion = this.RelationshipTypeIdList.ToList().Union(objectToBeCompared.RelationshipTypeIdList.ToList()).Count();
                    Int32 relationshipTypeIdsIntersect = this.RelationshipTypeIdList.ToList().Intersect(objectToBeCompared.RelationshipTypeIdList.ToList()).Count();

                    if (relationshipTypeIdsUnion != relationshipTypeIdsIntersect)
                    {
                        return false;
                    }

                    if (this.LoadRelationshipAttributeModels != this.LoadRelationshipAttributeModels)
                    {
                        return false;
                    }

                    if (this.LoadBlankRelationshipAttributes != this.LoadBlankRelationshipAttributes)
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
            hashCode = base.GetHashCode() ^ this.Locale.GetHashCode() ^ this.Level.GetHashCode() ^ this.LoadRelationshipAttributes.GetHashCode()
                        ^ this.ContainerId.GetHashCode() ^ this.LoadRelatedEntitiesAttributes.GetHashCode() ^ this.RelationshipId.GetHashCode()
                        ^ this.RelationshipParentId.GetHashCode() ^ this.ApplyDMS.GetHashCode() ^ this.LoadDenormalizedRelationshipsStatus.GetHashCode()
                        ^ this.LoadRelationshipAttributeModels.GetHashCode() ^ this.LoadBlankRelationshipAttributes.GetHashCode();

            if (this.RelationshipTypeIdList != null)
                hashCode = hashCode ^ this.RelationshipTypeIdList.GetHashCode();

            if (this.RelatedEntitiesAttributeIdList != null)
                hashCode = hashCode ^ this.RelatedEntitiesAttributeIdList.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Represents RelationshipContext  in Xml format
        /// </summary>
        /// <returns>String representation of current RelationshipContext object</returns>
        public override String ToXml()
        {
            String xml = String.Empty;

            String relationshipTypeIds = String.Empty;
            String relatedEntitiesAttributeIds = String.Empty;

            if (this.RelationshipTypeIdList != null && this.RelationshipTypeIdList.Count > 0)
            {
                relationshipTypeIds = ValueTypeHelper.JoinCollection(this.RelationshipTypeIdList, ",");
            }

            if (this.RelatedEntitiesAttributeIdList != null && this.RelatedEntitiesAttributeIdList.Count > 0)
            {
                relatedEntitiesAttributeIds = ValueTypeHelper.JoinCollection(this.RelatedEntitiesAttributeIdList, ",");
            }

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //RelationshipContext node start
            xmlWriter.WriteStartElement("RelationshipContext");

            xmlWriter.WriteAttributeString("DataLocales", ValueTypeHelper.JoinCollection(this.DataLocales, ","));
            xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
            xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
            xmlWriter.WriteAttributeString("Level", this.Level.ToString());
            xmlWriter.WriteAttributeString("RelationshipId", this.RelationshipId.ToString());
            xmlWriter.WriteAttributeString("RelationshipParentId", this.RelationshipParentId.ToString());
            xmlWriter.WriteAttributeString("RelationshipTypeIdList", relationshipTypeIds);
            xmlWriter.WriteAttributeString("LoadRelationshipAttributes", this.LoadRelationshipAttributes.ToString());
            xmlWriter.WriteAttributeString("LoadRelatedEntitiesAttributes", this.LoadRelatedEntitiesAttributes.ToString());
            xmlWriter.WriteAttributeString("RelatedEntitiesAttributeIdList", relatedEntitiesAttributeIds);
            xmlWriter.WriteAttributeString("ApplyDMS", this.ApplyDMS.ToString());
            xmlWriter.WriteAttributeString("LoadSources", this.LoadSources.ToString());
            xmlWriter.WriteAttributeString("LoadRelationshipAttributeModels", this.LoadRelationshipAttributeModels.ToString());
            xmlWriter.WriteAttributeString("LoadBlankRelationshipAttributes", this.LoadBlankRelationshipAttributes.ToString());

            //RelationshipContext end node
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Create a new relationship context object.
        /// </summary>
        /// <returns>New relationship context instance</returns>
        public RelationshipContext Clone()
        {
            RelationshipContext clonedRelationshipContext = new RelationshipContext();

            clonedRelationshipContext._dataLocales = ValueTypeHelper.CloneCollection(this._dataLocales);
            clonedRelationshipContext.Locale = this.Locale;

            clonedRelationshipContext._containerId = this._containerId;
            
            clonedRelationshipContext._level = this._level;
            
            clonedRelationshipContext._relationshipId = this._relationshipId;
            clonedRelationshipContext._relationshipParentId = this._relationshipParentId;

            clonedRelationshipContext._relationshipTypeIdList = ValueTypeHelper.CloneCollection(this._relationshipTypeIdList);
            clonedRelationshipContext._relationshipTypeNames = ValueTypeHelper.CloneCollection(this._relationshipTypeNames);

            clonedRelationshipContext._loadRelationshipAttributes = this._loadRelationshipAttributes;

            clonedRelationshipContext._returnRelatedEntityDetails = this._returnRelatedEntityDetails;
            
            clonedRelationshipContext._loadRelatedEntitiesAttributes = this._loadRelatedEntitiesAttributes;
            clonedRelationshipContext._relatedEntitiesAttributeIdList = ValueTypeHelper.CloneCollection(this._relatedEntitiesAttributeIdList);
            clonedRelationshipContext._relatedEntitiesAttributeNames = ValueTypeHelper.CloneCollection(this._relatedEntitiesAttributeNames);
            
            clonedRelationshipContext._applyDMS = this._applyDMS;
            
            clonedRelationshipContext._loadSources = this._loadSources;
            clonedRelationshipContext._loadDenormalizedRelationshipsStatus = this._loadDenormalizedRelationshipsStatus;
            clonedRelationshipContext._loadRelationshipAttributeModels = this._loadRelationshipAttributeModels;
            clonedRelationshipContext._loadBlankRelationshipAttributes = this._loadBlankRelationshipAttributes;

            return clonedRelationshipContext;
        }

        /// <summary>
        /// Adds attribute unique identifier in RelatedEntitiesAttributeNameGroupNameList
        /// </summary>
        /// <param name="attributeName">Represents Attribute short name</param>
        public void AddRelatedEntitiesAttributeName(String attributeName)
        {
            if (!String.IsNullOrWhiteSpace(attributeName))
            {
                this.RelatedEntitiesAttributeNames.Add(attributeName);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialize relationshioContext from Xml
        /// </summary>
        /// <param name="valuesAsXml">
        /// Xml having values
        /// <para> Sample:</para>
        /// <![CDATA[<RelationshipContext RelationshipTypeIdList="3001,3002" LoadRelationshipAttributes="true" Level="10" Locale="en_WW"/>]]>
        /// </param>
        private void LoadRelationshipContext(String valuesAsXml)
        {
            #region Sample Xml
            /*
             * <RelationshipContext RelationshipTypeIdList="3001,3002" LoadRelationshipAttributes="true" Level="10" Locale="en_WW"/>
             */
            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "RelationshipContext")
                    {
                        #region Read RelationshipContext Properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("Locale"))
                            {
                                String strLocale = reader.GetAttribute("Locale");
                                LocaleEnum locale = LocaleEnum.UnKnown;
                                Enum.TryParse<Core.LocaleEnum>(strLocale, out locale);
                                this.Locale = locale;
                            }

                            if (reader.MoveToAttribute("DataLocales"))
                            {
                                this.DataLocales = ValueTypeHelper.SplitStringToLocaleEnumCollection(reader.ReadContentAsString(), ',');
                            }

                            if (reader.MoveToAttribute("ContainerId"))
                            {
                                this.ContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._containerId);
                            }

                            if (reader.MoveToAttribute("Level"))
                            {
                                this.Level = ValueTypeHelper.Int16TryParse(reader.ReadContentAsString(), this._level);
                            }

                            if (reader.MoveToAttribute("RelationshipId"))
                            {
                                this.RelationshipId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._relationshipId);
                            }

                            if (reader.MoveToAttribute("RelationshipParentId"))
                            {
                                this.RelationshipParentId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._relationshipParentId);
                            }

                            if (reader.MoveToAttribute("RelationshipTypeIdList"))
                            {
                                this.RelationshipTypeIdList = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                            }

                            if (reader.MoveToAttribute("RelationshipTypeNames"))
                            {
                                this.RelationshipTypeNames = ValueTypeHelper.SplitStringToStringCollection(reader.ReadContentAsString(), ',');
                            }

                            if (reader.MoveToAttribute("ReturnRelatedEntityDetails"))
                            {
                                this.ReturnRelatedEntityDetails = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadRelationshipAttributes"))
                            {
                                this.LoadRelationshipAttributes = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadRelatedEntitiesAttributes"))
                            {
                                this.LoadRelatedEntitiesAttributes = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("RelatedEntitiesAttributeIdList"))
                            {
                                this.RelatedEntitiesAttributeIdList = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                            }

                            if (reader.MoveToAttribute("RelatedEntitiesAttributeNames"))
                            {
                                this.RelatedEntitiesAttributeNames = ValueTypeHelper.SplitStringToStringCollection(reader.ReadContentAsString(), ',');
                            }

                            if (reader.MoveToAttribute("ApplyDMS"))
                            {
                                this.ApplyDMS = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadSources"))
                            {
                                this.LoadSources = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadRelationshipAttributeModels"))
                            {
                                this.LoadRelationshipAttributeModels = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("LoadBlankRelationshipAttributes"))
                            {
                                this.LoadBlankRelationshipAttributes = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
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