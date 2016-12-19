using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Core;

    /// <summary>
    /// Represents class to indicate entity get options
    /// </summary>
    [DataContract]
    [ProtoContract]
    public sealed class EntityGetOptions : ObjectBase, MDM.Interfaces.IEntityGetOptions
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private Boolean _publishEvents = true;

        /// <summary>
        /// 
        /// </summary>
        private Boolean _applyAVS = true;

        /// <summary>
        /// 
        /// </summary>
        private Boolean _loadLatestFromDB = false;

        /// <summary>
        /// 
        /// </summary>
        private Boolean _applySecurity = true;

        /// <summary>
        /// 
        /// </summary>
        private Boolean _updateCache = false;

        /// <summary>
        /// Specifies whether to update the cache status during the entity get call .
        /// Modified the default value to false as database update during entity get is not recommended.
        /// </summary>
        private Boolean _updateCacheStatusInDB = false;

        /// <summary>
        /// 
        /// </summary>
        private Int32 _bulkGetBatchSize = 0;

        /// <summary>
        /// 
        /// </summary>
        private EntityFillOptions _fillOptions = new EntityFillOptions();

        /// <summary>
        /// 
        /// </summary>
        private Boolean _populateDiagnosticsReport = false;

        /// <summary>
        /// 
        /// </summary>
        private Boolean _loadAttributePermissions = true;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public EntityGetOptions()
        {
                
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public EntityGetOptions(String valuesAsXml)
        {
            LoadEntityGetOptions(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(1), DefaultValue(true)]
        public Boolean PublishEvents
        {
            get { return _publishEvents; }
            set { _publishEvents = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(2), DefaultValue(true)]
        public Boolean ApplyAVS
        {
            get { return _applyAVS; }
            set { _applyAVS = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(3), DefaultValue(false)]
        public Boolean LoadLatestFromDB
        {
            get { return _loadLatestFromDB; }
            set { _loadLatestFromDB = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(4), DefaultValue(true)]
        public Boolean ApplySecurity
        {
            get { return _applySecurity; }
            set { _applySecurity = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(5), DefaultValue(false)]
        public Boolean UpdateCache
        {
            get { return _updateCache; }
            set { _updateCache = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(6), DefaultValue(false)]
        public Boolean UpdateCacheStatusInDB
        {
            get { return _updateCacheStatusInDB; }
            set { _updateCacheStatusInDB = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(7)]
        public Int32 BulkGetBatchSize
        {
            get { return _bulkGetBatchSize; }
            set { _bulkGetBatchSize = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(8)]
        public EntityFillOptions FillOptions
        {
            get { return _fillOptions; }
            set { _fillOptions = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(9), DefaultValue(false)]
        public Boolean PopulateDiagnosticsReport
        {
            get { return _populateDiagnosticsReport; }
            set { _populateDiagnosticsReport = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(10), DefaultValue(false)]
        public Boolean LoadAttributePermissions
        {
            get { return _loadAttributePermissions; }
            set { _loadAttributePermissions = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("EntityGetOptions: [PublishEvents:{0}, ApplyAVS:{1}, LoadLatestFromDB:{2}, ApplySecurity:{3}, UpdateCache:{4}, UpdateCacheStatusInDB:{5}, BulkGetBatchSize:{6}, PopulateDiagnosticsReport:{7}, {8}]", PublishEvents, ApplyAVS, LoadLatestFromDB, ApplySecurity, UpdateCache, UpdateCacheStatusInDB, BulkGetBatchSize, PopulateDiagnosticsReport, FillOptions.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String ToXml()
        {
            return ToXml(ObjectSerialization.Full);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            var xml = String.Empty;

            var sw = new StringWriter();
            var xmlWriter = new XmlTextWriter(sw);

            // Attribute node start
            xmlWriter.WriteStartElement("EntityGetOptions");

            xmlWriter.WriteAttributeString("PublishEvents", this.PublishEvents.ToString());
            xmlWriter.WriteAttributeString("ApplyAVS", this.ApplyAVS.ToString());
            xmlWriter.WriteAttributeString("ApplySecurity", this.ApplySecurity.ToString());
            xmlWriter.WriteAttributeString("LoadLatestFromDB", this.LoadLatestFromDB.ToString());
            xmlWriter.WriteAttributeString("UpdateCache", this.UpdateCache.ToString());
            xmlWriter.WriteAttributeString("UpdateCacheStatusInDB", this.UpdateCacheStatusInDB.ToString());
            xmlWriter.WriteAttributeString("PopulateDiagnosticsReport", this.PopulateDiagnosticsReport.ToString());
            xmlWriter.WriteAttributeString("BulkGetBatchSize", this.BulkGetBatchSize.ToString());

            xmlWriter.WriteRaw(this.FillOptions.ToXml(objectSerialization));

            // EntityGetOptions end node
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            // get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadEntityGetOptions(String valuesAsXml)
        {
            XmlTextReader reader = null;

            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                var entityFillOptionsXml = String.Empty;

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityGetOptions")
                    {
                        #region Read EntityFillOptions Properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("PublishEvents"))
                            {
                                this._publishEvents = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._publishEvents);
                            }

                            if (reader.MoveToAttribute("ApplyAVS"))
                            {
                                this._applyAVS = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._applyAVS);
                            }

                            if (reader.MoveToAttribute("ApplySecurity"))
                            {
                                this._applySecurity = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._applySecurity);
                            }

                            if (reader.MoveToAttribute("LoadLatestFromDB"))
                            {
                                this._loadLatestFromDB = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._loadLatestFromDB);
                            }

                            if (reader.MoveToAttribute("UpdateCache"))
                            {
                                this._updateCache = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._updateCache);
                            }

                            if (reader.MoveToAttribute("UpdateCacheStatusInDB"))
                            {
                                this._updateCacheStatusInDB = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._updateCacheStatusInDB);
                            }

                            if (reader.MoveToAttribute("PopulateDiagnosticsReport"))
                            {
                                this._populateDiagnosticsReport = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._populateDiagnosticsReport);
                            }

                            if (reader.MoveToAttribute("BulkGetBatchSize"))
                            {
                                this._bulkGetBatchSize = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._bulkGetBatchSize);
                            }
                        }

                        #endregion
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityFillOptions")
                    {
                        entityFillOptionsXml = reader.ReadOuterXml();
                        this._fillOptions = new EntityFillOptions(entityFillOptionsXml);
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

        #region Fill options

        /// <summary>
        /// 
        /// </summary>
        [DataContract]
        [ProtoBuf.ProtoContract]
        public class EntityFillOptions
        {
            #region Fields

            /// <summary>
            /// 
            /// </summary>
            private Boolean _fillContainerAndOrganizationInfo = true;

            /// <summary>
            /// 
            /// </summary>
            private Boolean _fillEntityTypeInfo = true;

            /// <summary>
            /// 
            /// </summary>
            private Boolean _fillCategoryInfo = true;

            /// <summary>
            /// 
            /// </summary>
            private Boolean _fillParentInfo = true;

            /// <summary>
            /// 
            /// </summary>
            private Boolean _fillParentExtensionInfo = true;

            /// <summary>
            /// 
            /// </summary>
            private Boolean _fillLookupDisplayValues = true;

            /// <summary>
            /// 
            /// </summary>
            private Boolean _fillLookupRowWithValues = true;

            /// <summary>
            /// 
            /// </summary>
            private Boolean _fillUOMValues = true;

            /// <summary>
            /// 
            /// </summary>
            private Boolean _fillRelationshipProperties = true;

            /// <summary>
            /// 
            /// </summary>
            private Boolean _fillEntityProperties = true;

            #endregion

            #region Constructors

            /// <summary>
            /// 
            /// </summary>
            public EntityFillOptions()
            {
                
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="valuesAsXml"></param>
            public EntityFillOptions(String valuesAsXml)
            {
                LoadFillOptions(valuesAsXml);
            }

            #endregion
            
            #region Properties

            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            [ProtoBuf.ProtoMember(1), DefaultValue(true)]
            public Boolean FillContainerAndOrganizationInfo
            {
                get { return _fillContainerAndOrganizationInfo; }
                set { _fillContainerAndOrganizationInfo = value; }
            }

            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            [ProtoBuf.ProtoMember(2), DefaultValue(true)]
            public Boolean FillEntityTypeInfo
            {
                get { return _fillEntityTypeInfo; }
                set { _fillEntityTypeInfo = value; }
            }

            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            [ProtoBuf.ProtoMember(3), DefaultValue(true)]
            public Boolean FillCategoryInfo
            {
                get { return _fillCategoryInfo; }
                set { _fillCategoryInfo = value; }
            }

            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            [ProtoBuf.ProtoMember(4), DefaultValue(true)]
            public Boolean FillParentInfo
            {
                get { return _fillParentInfo; }
                set { _fillParentInfo = value; }
            }

            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            [ProtoBuf.ProtoMember(5), DefaultValue(true)]
            public Boolean FillParentExtensionInfo
            {
                get { return _fillParentExtensionInfo; }
                set { _fillParentExtensionInfo = value; }
            }

            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            [ProtoBuf.ProtoMember(6), DefaultValue(true)]
            public Boolean FillLookupDisplayValues
            {
                get { return _fillLookupDisplayValues; }
                set { _fillLookupDisplayValues = value; }
            }

            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            [ProtoBuf.ProtoMember(7), DefaultValue(true)]
            public Boolean FillLookupRowWithValues
            {
                get { return _fillLookupRowWithValues; }
                set { _fillLookupRowWithValues = value; }
            }

            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            [ProtoBuf.ProtoMember(8), DefaultValue(true)]
            public Boolean FillUOMValues
            {
                get { return _fillUOMValues; }
                set { _fillUOMValues = value; }
            }

            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            [ProtoBuf.ProtoMember(9), DefaultValue(true)]
            public Boolean FillRelationshipProperties
            {
                get { return _fillRelationshipProperties; }
                set { _fillRelationshipProperties = value; }
            }

            /// <summary>
            /// 
            /// </summary>
            [DataMember]
            [ProtoBuf.ProtoMember(10), DefaultValue(true)]
            public Boolean FillEntityProperties
            {
                get { return _fillEntityProperties; }
                set { _fillEntityProperties = value; }
            }

            #endregion

            #region Methods

            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            public void SetAllEntityPropertiesLevelFillOptionsTo(Boolean value)
            {
                _fillContainerAndOrganizationInfo = value;
                _fillEntityTypeInfo = value;
                _fillCategoryInfo = value;
                _fillParentInfo = value;
                _fillParentExtensionInfo = value;
            }

            /// <summary>
            /// Get an overridden string format of current entity get option object
            /// </summary>
            /// <returns>Returns an overridden string format of current entity get option object</returns>
            public override string ToString()
            {
                return String.Format("EntityFillOptions: [FillContainerAndOrganizationInfo:{0}, FillEntityTypeInfo:{1}, FillCategoryInfo:{2}, FillParentInfo:{3}, FillParentExtensionInfo:{4}, FillLookupDisplayValues:{5}, FillLookupRowWithValues:{6}, FillUOMValues:{7}, FillRelationshipProperties:{8}, , FillEntityProperties:{8}]", FillContainerAndOrganizationInfo, FillEntityTypeInfo, FillCategoryInfo, FillParentInfo, FillParentExtensionInfo, FillLookupDisplayValues, FillLookupRowWithValues, FillUOMValues, FillRelationshipProperties, FillEntityProperties);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public String ToXml()
            {
                return ToXml(ObjectSerialization.Full);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public String ToXml(ObjectSerialization objectSerialization)
            {
                String xml = String.Empty;

                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);
                // xmlWriter.WriteStartDocument();

                // Attribute node start
                xmlWriter.WriteStartElement("EntityFillOptions");

                xmlWriter.WriteAttributeString("FillContainerAndOrganizationInfo", this.FillContainerAndOrganizationInfo.ToString());
                xmlWriter.WriteAttributeString("FillEntityTypeInfo", this.FillEntityTypeInfo.ToString());
                xmlWriter.WriteAttributeString("FillCategoryInfo", this.FillCategoryInfo.ToString());
                xmlWriter.WriteAttributeString("FillParentInfo", this.FillParentInfo.ToString());
                xmlWriter.WriteAttributeString("FillParentExtensionInfo", this.FillParentExtensionInfo.ToString());
                xmlWriter.WriteAttributeString("FillRelationshipProperties", this.FillRelationshipProperties.ToString());
                xmlWriter.WriteAttributeString("FillLookupDisplayValues", this.FillLookupDisplayValues.ToString());
                xmlWriter.WriteAttributeString("FillLookupRowWithValues", this.FillLookupRowWithValues.ToString());
                xmlWriter.WriteAttributeString("FillUOMValues", this.FillUOMValues.ToString());
                xmlWriter.WriteAttributeString("FillEntityProperties", this.FillEntityProperties.ToString());

                // EntityContext end node
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();
                // get the actual XML
                xml = sw.ToString();

                xmlWriter.Close();
                sw.Close();

                return xml;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="valuesAsXml"></param>
            private void LoadFillOptions(String valuesAsXml)
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityFillOptions")
                        {
                            #region Read EntityFillOptions Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("FillContainerAndOrganizationInfo"))
                                {
                                    this._fillContainerAndOrganizationInfo = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._fillContainerAndOrganizationInfo);
                                }

                                if (reader.MoveToAttribute("FillEntityTypeInfo"))
                                {
                                    this._fillEntityTypeInfo = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._fillEntityTypeInfo);
                                }

                                if (reader.MoveToAttribute("FillCategoryInfo"))
                                {
                                    this._fillCategoryInfo = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._fillCategoryInfo);
                                }

                                if (reader.MoveToAttribute("FillParentInfo"))
                                {
                                    this._fillParentInfo = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._fillParentInfo);
                                }

                                if (reader.MoveToAttribute("FillParentExtensionInfo"))
                                {
                                    this._fillParentExtensionInfo = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._fillParentExtensionInfo);
                                }

                                if (reader.MoveToAttribute("FillRelationshipProperties"))
                                {
                                    this._fillRelationshipProperties = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._fillRelationshipProperties);
                                }

                                if (reader.MoveToAttribute("FillLookupDisplayValues"))
                                {
                                    this._fillLookupDisplayValues = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._fillLookupDisplayValues);
                                }

                                if (reader.MoveToAttribute("FillLookupRowWithValues"))
                                {
                                    this._fillLookupRowWithValues = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._fillLookupRowWithValues);
                                }

                                if (reader.MoveToAttribute("FillUOMValues"))
                                {
                                    this._fillUOMValues = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._fillUOMValues);
                                }

                                if (reader.MoveToAttribute("FillEntityProperties"))
                                {
                                    this._fillEntityProperties = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._fillEntityProperties);
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

        #endregion
    }
}
