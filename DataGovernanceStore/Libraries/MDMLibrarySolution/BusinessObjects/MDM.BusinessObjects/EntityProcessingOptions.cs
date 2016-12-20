using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    
    /// <summary>
    /// Specifies EntityProcessingOptions which specifies various flags and indications to entity processing logic
    /// </summary>
    [DataContract]
    public class EntityProcessingOptions : ObjectBase, IEntityProcessingOptions
    {
        #region Fields

        /// <summary>
        /// Field indicates if validation process to be executed as part of entity processing
        /// </summary>
        private Boolean _validateEntities = true;

        /// <summary>
        /// Field indicates Data Quality validation
        /// </summary>
        private Boolean _dataQualityValidation;

        /// <summary>
        /// Field indicates which Data Quality validation profile use
        /// </summary>
        private Int32? _validationProfileId;

        /// <summary>
        /// Field indicates if events to be published as part of entity processing
        /// </summary>
        private Boolean _publishEvents = true;

        /// <summary>
        /// Field indicates to process only entities without attributes and relationships
        /// </summary>
        private Boolean _processOnlyEntities = false;

        /// <summary>
        /// Field indicates if default values need to be processed
        /// </summary>
        private Boolean _processDefaultValues = true;

        /// <summary>
        /// Field indicates if sources need to be processed
        /// </summary>
        private Boolean _processSources = false;

        /// <summary>
        /// Field indicates collection processing type
        /// </summary>
        private CollectionProcessingType _collectionProcessingType = CollectionProcessingType.Replace;

        /// <summary>
        /// Field indicates if we need to fail an entity on any error in entity
        /// </summary>
        private Boolean _failEntityOnAnyError = false;

        /// <summary>
        /// Field indicates if we can add entities during processing
        /// </summary>
        private Boolean _canAddEntities = true;

        /// <summary>
        /// Field indicates if we can update entities during processing
        /// </summary>
        private Boolean _canUpdateEntities = true;

        /// <summary>
        /// Field indicates if we can delete entities during processing
        /// </summary>
        private Boolean _canDeleteEntities = true;

        /// <summary>
        /// Field indicates if we can reclassify entities during processing
        /// </summary>
        private Boolean _canReclassifyEntities = false;

        /// <summary>
        /// Field indicates if inherited values have to be denormed
        /// </summary>
        private Boolean _initialLoadProcessInheritedValues = false;

        /// <summary>
        /// Field indicating the import mode
        /// </summary>
        private ImportMode _importMode = ImportMode.UnKnown;
        
        /// <summary>
        /// Field denoting whether the attribute level partial Processing is Enable or not
        /// </summary>
        private Boolean _isPartialAttributeprocessingEnabled = false;

        /// <summary>
        /// Field denoting whether the Relationship type level partial processing is Enable or not
        /// </summary>
        private Boolean _isPartialRelationshipTypeProcessingEnabled = false;

        /// <summary>
        /// Field denoting whether the Relationship attribute level processing is Enable or not
        /// </summary>
        private Boolean _isPartialRelationshipAttributeProcessingEnabled = false;

        /// <summary>
        /// Field denoting list of the attribute Ids which needs to be validate
        /// </summary>
        private Collection<Int32> _validationMustOnAttributeIdList = new Collection<Int32>();

        /// <summary>
        /// Field denoting list of the relationship attribute Ids which needs to be validate
        /// </summary>
        private Collection<Int32> _validationMustOnRelationshipAttributeIdList = new Collection<Int32>();

        /// <summary>
        /// Field denoting list of the Relationship type Ids which needs to be validate
        /// </summary>
        private Collection<Int32> _validationMustOnRelationshipTypeIdList  = new Collection<Int32>();

        /// <summary>
        /// Field denoting entity processing mode. (Mode will be Async, when entity process is called from processor)
        /// </summary>
        private ProcessingMode _processingMode = ProcessingMode.Sync;

        /// <summary>
        /// Indicates if invalid data are allowed in the system. If this is set to true, even if AttributeModel validation fails for attribute, it will be allowed to set.
        /// </summary>
        private Boolean _allowInvalidData = true;

        /// <summary>
        /// Field denoting whether to apply attribute value security while processing entity 
        /// </summary>
        private Boolean _applyAVS = true;

        /// <summary>
        /// Field denoting whether to apply data model security while processing entity
        /// </summary>
        private Boolean _applyDMS = false;

        /// <summary>
        /// 
        /// </summary>
        private AttributeProcessingOptionsCollection _attributeProcessingOptionCollection;

        /// <summary>
        /// 
        /// </summary>
        private Boolean _ignoreCase = false;

        /// <summary>
        /// Specifies the procession options on whether matching is required or not.
        /// </summary>
        private MatchProcessingOptions _matchProcessingOptions = new MatchProcessingOptions();

        /// <summary>
        /// Indicates if the match store needs to be updated for this entity. If the value is true and if the attributes that got changed belong to a store, then a job will be queued for the 
        /// match store load processor to process this delta change.
        /// </summary>
        private Boolean _loadMatchStore = true;

        /// <summary>
        /// Specifies if diagnostic report should be populated or not.
        /// </summary>
        private Boolean _populateDiagnosticsReport = false;

        /// <summary>
        /// Indicates if only relationships are to be processed w/o RelationshipAttributes. Used for relationship initial load.
        /// </summary>
        private Boolean _processOnlyRelationships = false;

        /// <summary>
        /// Specifies if system need to fill missing ids in entities or not
        /// </summary>
        private Boolean _resolveIdsByNames = false;

        /// <summary>
        /// Property defining the behavior of the attribute compare and merge. This behavior is applicable for common, technical and relationship attributes
        /// </summary>
        private AttributeCompareAndMergeBehavior _attributeCompareAndMergeBehavior = AttributeCompareAndMergeBehavior.CompareOverriddenValuesOnly;

        /// <summary>
        /// Field denoting whether to validate leaf category or not
        /// </summary>
        private Boolean _validateLeafCategory = true;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public EntityProcessingOptions()
            : base()
        { }

        /// <summary>
        /// 
        /// </summary>
        public EntityProcessingOptions(Boolean validateEntities, Boolean publishEvents, Boolean processOnlyEntities, Boolean processDefaultValues)
            : base()
        {
            this._validateEntities = validateEntities;
            this._publishEvents = publishEvents;
            this._processOnlyEntities = processOnlyEntities;
            this._processDefaultValues = processDefaultValues;
        }

        /// <summary>
        /// EntityProcessingOptions with populateDiagnosticsReport overload
        /// </summary>
        /// <param name="validateEntities"></param>
        /// <param name="publishEvents"></param>
        /// <param name="processOnlyEntities"></param>
        /// <param name="processDefaultValues"></param>
        /// <param name="populateDiagnosticsReport"></param>
        public EntityProcessingOptions(Boolean validateEntities, Boolean publishEvents, Boolean processOnlyEntities, Boolean processDefaultValues, Boolean populateDiagnosticsReport)
            : base()
        {
            this._validateEntities = validateEntities;
            this._publishEvents = publishEvents;
            this._processOnlyEntities = processOnlyEntities;
            this._processDefaultValues = processDefaultValues;
            this._populateDiagnosticsReport = populateDiagnosticsReport;
        }

        /// <summary>
        /// Constructor with XML having values of object. 
        /// </summary>
        /// <param name="valuesAsXml">Indicates XML having xml value</param>
        public EntityProcessingOptions(String valuesAsXml)
        {
            LoadEntityProcessingOptions(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Property indicates if validation process to be executed as part of entity processing
        /// </summary>
        [DataMember]
        public Boolean ValidateEntities
        {
            get
            {
                return _validateEntities;
            }
            set
            {
                _validateEntities = value;
            }
        }

        /// <summary>
        /// Property indicates Data Quality Validation
        /// </summary>
        [DataMember]
        public Boolean DataQualityValidation
        {
            get
            {
                return _dataQualityValidation;
            }
            set
            {
                _dataQualityValidation = value;
            }
        }

        /// <summary>
        /// Field indicates which Data Quality validation profile use
        /// </summary>
        [DataMember]
        public Int32? ValidationProfileId
        {
            get
            {
                return _validationProfileId;
            }
            set
            {
                _validationProfileId = value;
            }
        }

        /// <summary>
        /// Property indicates if events to be published as part of entity processing
        /// </summary>
        [DataMember]
        public Boolean PublishEvents
        {
            get
            {
                return _publishEvents;
            }
            set
            {
                _publishEvents = value;
            }
        }

        /// <summary>
        /// Property indicates to process only entities without attributes and relationships
        /// </summary>
        [DataMember]
        public Boolean ProcessOnlyEntities
        {
            get
            {
                return _processOnlyEntities;
            }
            set
            {
                _processOnlyEntities = value;
            }
        }

        /// <summary>
        /// Property indicates if default values need to be processed
        /// </summary>
        [DataMember]
        public Boolean ProcessDefaultValues
        {
            get
            {
                return _processDefaultValues;
            }
            set
            {
                _processDefaultValues = value;
            }
        }

        /// <summary>
        /// Property indicates if source values need to be processed
        /// </summary>
        [DataMember]
        public Boolean ProcessSources
        {
            get
            {
                return _processSources;
            }
            set
            {
                _processSources = value;
            }
        }

        /// <summary>
        /// Property indicates collection processing type
        /// </summary>
        [DataMember]
        public CollectionProcessingType CollectionProcessingType
        {
            get
            {
                return _collectionProcessingType;
            }
            set
            {
                _collectionProcessingType = value;
            }
        }

        /// <summary>
        /// Property indicates if we need to fail an entity on any error in entity
        /// </summary>
        [DataMember]
        public Boolean FailEntityOnAnyError
        {
            get
            {
                return _failEntityOnAnyError;
            }
            set
            {
                _failEntityOnAnyError = value;
            }
        }

        /// <summary>
        /// Property indicates if we can add entities during processing
        /// </summary>
        [DataMember]
        public Boolean CanAddEntities
        {
            get
            {
                return _canAddEntities;
            }
            set
            {
                _canAddEntities = value;
            }
        }

        /// <summary>
        /// Property indicates if we can update entities during processing
        /// </summary>
        [DataMember]
        public Boolean CanUpdateEntities
        {
            get
            {
                return _canUpdateEntities;
            }
            set
            {
                _canUpdateEntities = value;
            }
        }

        /// <summary>
        /// Property indicates if we can delete entities during processing
        /// </summary>
        [DataMember]
        public Boolean CanDeleteEntities
        {
            get
            {
                return _canDeleteEntities;
            }
            set
            {
                _canDeleteEntities = value;
            }
        }

        /// <summary>
        /// Property indicates if we can Reclassify entities during processing
        /// </summary>
        [DataMember]
        public Boolean CanReclassifyEntities
        {
            get
            {
                return _canReclassifyEntities;
            }
            set
            {
                _canReclassifyEntities = value;
            }
        }

        /// <summary>
        /// Property indicates if inherited values have to be denormed
        /// </summary>
        [DataMember]
        public Boolean InitialLoadProcessInheritedValues
        {
            get
            {
                return _initialLoadProcessInheritedValues;
            }
            set
            {
                _initialLoadProcessInheritedValues = value;
            }
        }

        /// <summary>
        /// Property denoting the import mode.
        /// </summary>
        [DataMember]
        public ImportMode ImportMode
        {
            get
            {
                return _importMode;
            }
            set
            {
                _importMode = value;
            }
        }
        
        /// <summary>
        /// Property denoting whether the attribute level partial Processing is Enable or not.
        /// </summary>
        [DataMember]
        public Boolean IsPartialAttributeprocessingEnabled
        {
            get
            {
                return _isPartialAttributeprocessingEnabled;
            }
            set
            {
                _isPartialAttributeprocessingEnabled = value;
            }
        }

        /// <summary>
        /// Property denoting whether the relationship attribute level partial Processing is Enable or not.
        /// </summary>
        [DataMember]
        public Boolean IsPartialRelationshipAttributeProcessingEnabled
        {
            get
            {
                return _isPartialRelationshipAttributeProcessingEnabled;
            }
            set
            {
                _isPartialRelationshipAttributeProcessingEnabled = value;
            }
        }

        /// <summary>
        /// Property denoting whether the relationship type level partial Processing is Enable or not.
        /// </summary>
        [DataMember]
        public Boolean IsPartialRelationshipTypeProcessingEnabled
        {
            get
            {
                return _isPartialRelationshipTypeProcessingEnabled;
            }
            set
            {
                _isPartialRelationshipTypeProcessingEnabled = value;
            }
        }

        /// <summary>
        /// Property denoting list of Attribute Ids needs to be Validate.
        /// </summary>
        [DataMember]
        public Collection<Int32> ValidationMustOnAttributeIdList
        {
            get
            {
                return _validationMustOnAttributeIdList;
            }
            set
            {
                _validationMustOnAttributeIdList = value;
            }
        }

        /// <summary>
        /// Property denoting list of Relationship Attribute Ids needs to be Validate.
        /// </summary>
        [DataMember]
        public Collection<Int32> ValidationMustOnRelationshipAttributeIdList
        {
            get
            {
                return _validationMustOnRelationshipAttributeIdList;
            }
            set
            {
                _validationMustOnRelationshipAttributeIdList = value;
            }
        }

        /// <summary>
        /// Property denoting list of RelationshipType Ids needs be Validate. 
        /// </summary>
        [DataMember]
        public Collection<Int32> ValidationMustOnRelationshipTypeIdList
        {
            get
            {
                return _validationMustOnRelationshipTypeIdList;
            }
            set
            {
                _validationMustOnRelationshipTypeIdList = value;
            }
        }

        /// <summary>
        /// Property denoting entity processing mode. (Mode will be Async, when entity process is called from processor)
        /// </summary>
        [DataMember]
        public ProcessingMode ProcessingMode
        {
            get
            {
                return _processingMode;
            }
            set
            {
                this._processingMode = value;
            }
        }

        /// <summary>
        /// Indicates if invalid data are allowed in the system. If this is set to true, even if AttributeModel validation fails for attribute, it will be allowed to set.
        /// </summary>
        [DataMember]
        public Boolean AllowInvalidData
        {
            get { return _allowInvalidData; }
            set { _allowInvalidData = value; }
        }

        /// <summary>
        /// Property denoting whether to apply attribute value security while processing entity 
        /// </summary>
        [DataMember]
        public Boolean ApplyAVS
        {
            get
            {
                return _applyAVS;
            }
            set
            {
                _applyAVS = value;
            }
        }

        /// <summary>
        /// Property denoting whether to apply data model security while processing entity 
        /// </summary>
        [DataMember]
        public Boolean ApplyDMS
        {
            get
            {
                return _applyDMS;
            }
            set
            {
                _applyDMS = value;
            }
        }


        /// <summary>
        /// Property denoting list of attributes with ActionFlags mentioned in Import profile
        /// </summary>
        [DataMember]
        public AttributeProcessingOptionsCollection AttributeProcessingOptionCollection
        {
            get { return _attributeProcessingOptionCollection; }
            set { _attributeProcessingOptionCollection = value; }
        }

        /// <summary>
        /// Property indicating if matching is required or not and the options for it.
        /// </summary>
        [DataMember]
        public MatchProcessingOptions MatchProcessingOptions
        {
            get { return _matchProcessingOptions; }
            set { _matchProcessingOptions = value; }
        }
        /// <summary>
        /// Property indicates if system need to ignore case sensitive comparision while identifing changes
        /// </summary>
        [DataMember]
        public Boolean IgnoreCase
        {
            get
            {
                return _ignoreCase;
            }
            set
            {
                _ignoreCase = value;
            }
        }

        /// <summary>
        /// Indicates if the match store needs to be updated for this entity. If the value is true and if the attributes that got changed belong to a store, then a job will be queued for the 
        /// match store load processor to process this delta change.
        /// </summary>
        [DataMember]
        public Boolean LoadMatchStore
        {
            get
            {
                return _loadMatchStore;
            }
            set
            {
                _loadMatchStore = value;
            }
        }

        /// <summary>
        /// Specifies if diagnostic report should be populated or not.
        /// Note that this property is not available from IEntityProcessingOptions as this has internal scope.
        /// </summary>
        [DataMember]
        public Boolean PopulateDiagnosticsReport
        {
            get { return _populateDiagnosticsReport; }
            set { _populateDiagnosticsReport = value; }
        }

        /// <summary>
        /// Indicates if only relationships are to be processed w/o RelationshipAttributes. Used for relationship initial load.
        /// </summary>
        [DataMember]
        public Boolean ProcessOnlyRelationships
        {
            get { return _processOnlyRelationships; }
            set { _processOnlyRelationships = value; }
        }

        /// <summary>
        /// Specifies if system need to fill missing ids in entities or not
        /// </summary>
        [DataMember]
        public Boolean ResolveIdsByNames
        {
            get { return this._resolveIdsByNames; }
            set { this._resolveIdsByNames = value; }
        }

        /// <summary>
        /// Property defining the behavior of the attribute compare and merge. This behavior is applicable for common, technical and relationship attributes
        /// </summary>
        [DataMember]
        public AttributeCompareAndMergeBehavior AttributeCompareAndMergeBehavior
        {
            get
            {
                return _attributeCompareAndMergeBehavior;
            }
            set
            {
                this._attributeCompareAndMergeBehavior = value;
            }
        }

        /// <summary>
        /// Indicates whether to validate leaf category or not
        /// </summary>
        [DataMember]
        public Boolean ValidateLeafCategory
        {
            get
            {
                return this._validateLeafCategory;
            }
            set
            {
                this._validateLeafCategory = value;
            }
        }

        #endregion Properties

        #region Methods
         
        #region Public methods

        /// <summary>
        /// Represents EntityProcessingOptions in Xml format
        /// </summary>
        /// <returns>String representation of current EntityProcessingOptions object instance</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            String validationMustOnAttributeIdList = String.Empty;
            String validationMustOnRelationshipTypeIdList = String.Empty;
            String validationMustOnRelationshipAttributeIdList = String.Empty;

            if (this.ValidationMustOnAttributeIdList != null && this.ValidationMustOnAttributeIdList.Count > 0)
            {
                validationMustOnAttributeIdList = ValueTypeHelper.JoinCollection(this.ValidationMustOnAttributeIdList, ",");
            }
            if (this.ValidationMustOnRelationshipTypeIdList != null && this.ValidationMustOnRelationshipTypeIdList.Count > 0)
            {
                validationMustOnRelationshipTypeIdList = ValueTypeHelper.JoinCollection(this.ValidationMustOnRelationshipTypeIdList, ",");
            }
            if (this.ValidationMustOnRelationshipAttributeIdList != null && this.ValidationMustOnRelationshipAttributeIdList.Count > 0)
            {
                validationMustOnRelationshipAttributeIdList = ValueTypeHelper.JoinCollection(this.ValidationMustOnRelationshipAttributeIdList, ",");
            }

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Attribute node start
            xmlWriter.WriteStartElement("EntityProcessingOptions");

            xmlWriter.WriteAttributeString("ValidateEntities", this.ValidateEntities.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("PublishEvents", this.PublishEvents.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("ProcessOnlyEntities", this.ProcessOnlyEntities.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("ProcessDefaultValues", this.ProcessDefaultValues.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("ProcessSources", this.ProcessSources.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("CollectionProcessingType", this.CollectionProcessingType.ToString());
            xmlWriter.WriteAttributeString("FailEntityOnAnyError", this.FailEntityOnAnyError.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("CanAddEntities", this.CanAddEntities.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("CanUpdateEntities", this.CanUpdateEntities.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("CanDeleteEntities", this.CanDeleteEntities.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("CanReclassifyEntities", this.CanReclassifyEntities.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("InitialLoadProcessInheritedValues", this.InitialLoadProcessInheritedValues.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("ImportMode", this.ImportMode.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("IsPartialAttributeprocessingEnabled", this.IsPartialAttributeprocessingEnabled.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("IsPartialRelationshipAttributeProcessingEnabled", this.IsPartialRelationshipAttributeProcessingEnabled.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("IsPartialRelationshipTypeProcessingEnabled", this.IsPartialRelationshipTypeProcessingEnabled.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("ValidationMustOnAttributeIdList", validationMustOnAttributeIdList);
            xmlWriter.WriteAttributeString("ValidationMustOnRelationshipTypeIdList", validationMustOnRelationshipTypeIdList);
            xmlWriter.WriteAttributeString("ValidationMustOnRelationshipAttributeIdList", validationMustOnRelationshipAttributeIdList);
            xmlWriter.WriteAttributeString("ProcessingMode", this.ProcessingMode.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("DataQualityValidation", this.DataQualityValidation.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("AllowInvalidData", this.AllowInvalidData.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("IgnoreCase", this.IgnoreCase.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("LoadMatchStore", this.LoadMatchStore.ToString());
            // xmlWriter.WriteAttributeString("PopulateDiagnosticsReport", this.PopulateDiagnosticsReport.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("ProcessOnlyRelationships", this.ProcessOnlyRelationships.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("AttributeCompareAndMergeBehavior", this.AttributeCompareAndMergeBehavior.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("ApplyDMS", this.ApplyDMS.ToString());
            xmlWriter.WriteAttributeString("ValidateLeafCategory", this.ValidateLeafCategory.ToString().ToLowerInvariant());

            if (this.ValidationProfileId.HasValue)
            {
                xmlWriter.WriteAttributeString("ValidationProfileId", this.ValidationProfileId.GetValueOrDefault().ToString(CultureInfo.InvariantCulture).ToLowerInvariant());
            }

            #region Write MatchProcessingOptions
            if (this.MatchProcessingOptions != null)
            {
                xmlWriter.WriteRaw(this.MatchProcessingOptions.ToXml());
            }

            #endregion

            //EntityProcessingOptions end node
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Represents EntityProcessingOptions in Xml format
        /// </summary>
        /// <returns>String representation of current EntityProcessingOptions object instance</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String xml = String.Empty;
           
            if (serialization == ObjectSerialization.Full)
            {
                xml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //Attribute node start
                xmlWriter.WriteStartElement("EntityProcessingOptions");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    xmlWriter.WriteAttributeString("ValidateEntities", this.ValidateEntities.ToString().ToLower());
                    xmlWriter.WriteAttributeString("PublishEvents", this.PublishEvents.ToString().ToLower());
                    xmlWriter.WriteAttributeString("ProcessOnlyEntities", this.ProcessOnlyEntities.ToString().ToLower());
                    xmlWriter.WriteAttributeString("ProcessDefaultValues", this.ProcessDefaultValues.ToString().ToLower());
                    xmlWriter.WriteAttributeString("ProcessSources", this.ProcessSources.ToString().ToLower());
                    xmlWriter.WriteAttributeString("CollectionProcessingType", this.CollectionProcessingType.ToString());
                    xmlWriter.WriteAttributeString("FailEntityOnAnyError", this.FailEntityOnAnyError.ToString().ToLower());
                    xmlWriter.WriteAttributeString("CanAddEntities", this.CanAddEntities.ToString().ToLower());
                    xmlWriter.WriteAttributeString("CanUpdateEntities", this.CanUpdateEntities.ToString().ToLower());
                    xmlWriter.WriteAttributeString("CanDeleteEntities", this.CanDeleteEntities.ToString().ToLower());
                    xmlWriter.WriteAttributeString("CanReclassifyEntities", this.CanReclassifyEntities.ToString().ToLower());
                    xmlWriter.WriteAttributeString("InitialLoadProcessInheritedValues", this.InitialLoadProcessInheritedValues.ToString().ToLower());
                    xmlWriter.WriteAttributeString("ImportMode", this.ImportMode.ToString().ToLower());
                    xmlWriter.WriteAttributeString("ProcessingMode", this.ProcessingMode.ToString().ToLower());
                    xmlWriter.WriteAttributeString("DataQualityValidation", this.DataQualityValidation.ToString().ToLowerInvariant());
                    xmlWriter.WriteAttributeString("IgnoreCase", this.IgnoreCase.ToString().ToLowerInvariant());
                    xmlWriter.WriteAttributeString("LoadMatchStore", this.LoadMatchStore.ToString());
                    xmlWriter.WriteAttributeString("ResolveIdsByNames", this.ResolveIdsByNames.ToString());
                    // xmlWriter.WriteAttributeString("PopulateDiagnosticsReport", this.PopulateDiagnosticsReport.ToString().ToLowerInvariant());
                    xmlWriter.WriteAttributeString("AttributeCompareAndMergeBehavior", this.AttributeCompareAndMergeBehavior.ToString().ToLowerInvariant());
                    xmlWriter.WriteAttributeString("ApplyDMS", this.ApplyDMS.ToString());

                    if (this.ValidationProfileId.HasValue)
                    {
                        xmlWriter.WriteAttributeString("ValidationProfileId", this.ValidationProfileId.GetValueOrDefault().ToString(CultureInfo.InvariantCulture).ToLowerInvariant());
                    }
                }

                if (serialization == ObjectSerialization.UIRender)
                {
                    xmlWriter.WriteAttributeString("ValidateEntities", this.ValidateEntities.ToString().ToLower());
                    xmlWriter.WriteAttributeString("PublishEvents", this.PublishEvents.ToString().ToLower());
                    xmlWriter.WriteAttributeString("ProcessOnlyEntities", this.ProcessOnlyEntities.ToString().ToLower());
                    xmlWriter.WriteAttributeString("ProcessDefaultValues", this.ProcessDefaultValues.ToString().ToLower());
                    xmlWriter.WriteAttributeString("CollectionProcessingType", this.CollectionProcessingType.ToString());
                    xmlWriter.WriteAttributeString("FailEntityOnAnyError", this.FailEntityOnAnyError.ToString().ToLower());
                    xmlWriter.WriteAttributeString("CanAddEntities", this.CanAddEntities.ToString().ToLower());
                    xmlWriter.WriteAttributeString("CanUpdateEntities", this.CanUpdateEntities.ToString().ToLower());
                    xmlWriter.WriteAttributeString("CanDeleteEntities", this.CanDeleteEntities.ToString().ToLower());
                    xmlWriter.WriteAttributeString("CanReclassifyEntities", this.CanReclassifyEntities.ToString().ToLower());
                    xmlWriter.WriteAttributeString("InitialLoadProcessInheritedValues", this.InitialLoadProcessInheritedValues.ToString().ToLower());
                    xmlWriter.WriteAttributeString("ImportMode", this.ImportMode.ToString().ToLower());
                    xmlWriter.WriteAttributeString("ProcessingMode", this.ProcessingMode.ToString().ToLower());
                    xmlWriter.WriteAttributeString("DataQualityValidation", this.DataQualityValidation.ToString().ToLowerInvariant());
                    xmlWriter.WriteAttributeString("IgnoreCase", this.IgnoreCase.ToString().ToLowerInvariant());
                    xmlWriter.WriteAttributeString("LoadMatchStore", this.LoadMatchStore.ToString());
                    xmlWriter.WriteAttributeString("ResolveIdsByNames", this.ResolveIdsByNames.ToString());
                    // xmlWriter.WriteAttributeString("PopulateDiagnosticsReport", this.PopulateDiagnosticsReport.ToString().ToLowerInvariant());
                    xmlWriter.WriteAttributeString("AttributeCompareAndMergeBehavior", this.AttributeCompareAndMergeBehavior.ToString().ToLowerInvariant());
                    xmlWriter.WriteAttributeString("ApplyDMS", this.ApplyDMS.ToString());

                    if (this.ValidationProfileId.HasValue)
                    {
                        xmlWriter.WriteAttributeString("ValidationProfileId", this.ValidationProfileId.GetValueOrDefault().ToString(CultureInfo.InvariantCulture).ToLowerInvariant());
                    }
                }

                #region Write MatchProcessingOptions
                if (this.MatchProcessingOptions != null)
                {
                    xmlWriter.WriteRaw(this.MatchProcessingOptions.ToXml());
                }

                #endregion

                //EntityProcessingOptions end node
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();
                //get the actual XML
                xml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            return xml;
        }

        #endregion Public methods

        #region Private methods

        private void LoadEntityProcessingOptions(String valuesAsXml)
        { 
            #region Sample Xml
            //<EntityProcessingOptions ValidateEntities="true" PublishEvents="true" ProcessOnlyEntities="false" ProcessDefaultValues="true" CollectionProcessingType="Replace" PopulateDiagnosticsReport="true" ResolveIdsByNames="true"/>
            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityProcessingOptions")
                    {
                        #region Read EntityProcessingOptions Properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("ValidateEntities"))
                            {
                                this.ValidateEntities = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._validateEntities);
                            }

                            if (reader.MoveToAttribute("DataQualityValidation"))
                            {
                                this.DataQualityValidation = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._dataQualityValidation);
                            }
                            if (reader.MoveToAttribute("ValidationProfileId"))
                            {
                                this.ValidationProfileId = ValueTypeHelper.ConvertToNullableInt32(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("PublishEvents"))
                            {
                                this.PublishEvents = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._publishEvents);
                            }

                            if (reader.MoveToAttribute("ProcessOnlyEntities"))
                            {
                                this.ProcessOnlyEntities = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._processOnlyEntities);
                            }

                            if (reader.MoveToAttribute("ProcessDefaultValues"))
                            {
                                this.ProcessDefaultValues = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._processDefaultValues);
                            }

                            if (reader.MoveToAttribute("ProcessSources"))
                            {
                                this.ProcessSources = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._processSources);
                            }

                            if (reader.MoveToAttribute("CollectionProcessingType"))
                            {
                                String strCollectionProcessingType = reader.ReadContentAsString();
                                CollectionProcessingType collectionProcessingType = MDM.Core.CollectionProcessingType.Unknown;
                                Enum.TryParse<CollectionProcessingType>(strCollectionProcessingType, true, out collectionProcessingType);
                                this.CollectionProcessingType = collectionProcessingType;
                            }

                            if (reader.MoveToAttribute("FailEntityOnAnyError"))
                            {
                                this.FailEntityOnAnyError = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._failEntityOnAnyError);
                            }

                            if (reader.MoveToAttribute("CanAddEntities"))
                            {
                                this.CanAddEntities = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._canAddEntities);
                            }

                            if (reader.MoveToAttribute("CanUpdateEntities"))
                            {
                                this.CanUpdateEntities = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._canUpdateEntities);
                            }

                            if (reader.MoveToAttribute("CanDeleteEntities"))
                            {
                                this.CanDeleteEntities = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._canDeleteEntities);
                            }

                            if (reader.MoveToAttribute("CanReclassifyEntities"))
                            {
                                this.CanReclassifyEntities = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._canReclassifyEntities);
                            }

                            if (reader.MoveToAttribute("InitialLoadProcessInheritedValues"))
                            {
                                this.InitialLoadProcessInheritedValues = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._initialLoadProcessInheritedValues);
                            }

                            if (reader.MoveToAttribute("ImportMode"))
                            {
                                String importModeString = reader.ReadContentAsString();
                                ImportMode importMode = ImportMode.UnKnown;
                                Enum.TryParse<ImportMode>(importModeString, true, out importMode);
                                this.ImportMode = importMode;
                            }
                            
                            if (reader.MoveToAttribute("IsPartialAttributeprocessingEnabled"))
                            {
                                this.IsPartialAttributeprocessingEnabled = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._isPartialAttributeprocessingEnabled);
                            }

                            if (reader.MoveToAttribute("IsPartialRelationshipAttributeProcessingEnabled"))
                            {
                                this.IsPartialRelationshipAttributeProcessingEnabled = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._isPartialRelationshipAttributeProcessingEnabled);
                            }

                            if (reader.MoveToAttribute("IsPartialRelationshipTypeProcessingEnabled"))
                            {
                                this.IsPartialRelationshipTypeProcessingEnabled = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._isPartialRelationshipTypeProcessingEnabled);
                            }

                            if (reader.MoveToAttribute("ValidationMustOnAttributeIdList"))
                            {
                                this.ValidationMustOnAttributeIdList = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                            }

                            if (reader.MoveToAttribute("ValidationMustOnRelationshipTypeIdList"))
                            {
                                this.ValidationMustOnRelationshipTypeIdList = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                            }

                            if (reader.MoveToAttribute("ValidationMustOnRelationshipAttributeIdList"))
                            {
                                this.ValidationMustOnRelationshipAttributeIdList = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                            }
                            
                            if (reader.MoveToAttribute("AllowInvalidData"))
                            {
                                this.AllowInvalidData = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                            }

                            if (reader.MoveToAttribute("ProcessingMode"))
                            {
                                String strProcessingMode = reader.ReadContentAsString();
                                ProcessingMode processingMode = MDM.Core.ProcessingMode.Sync;
                                Enum.TryParse<ProcessingMode>(strProcessingMode, true, out processingMode);
                                this.ProcessingMode = processingMode;
                            }

                            if (reader.MoveToAttribute("IgnoreCase"))
                            {
                                this.IgnoreCase = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._ignoreCase);
                            }

                            if (reader.MoveToAttribute("LoadMatchStore"))
                            {
                                this.LoadMatchStore = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("PopulateDiagnosticsReport"))
                            {
                                this.PopulateDiagnosticsReport = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._populateDiagnosticsReport);
                            }

                            if (reader.MoveToAttribute("ProcessOnlyRelationships"))
                            {
                                this.ProcessOnlyRelationships = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._processOnlyRelationships);
                            }

                            if (reader.MoveToAttribute("ResolveIdsByNames"))
                            {
                                this.ResolveIdsByNames = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._resolveIdsByNames);
                            }

                            if (reader.MoveToAttribute("AttributeCompareAndMergeBehavior"))
                            {
                                String strCompareBehavior = reader.ReadContentAsString();
                                AttributeCompareAndMergeBehavior attributeCompareAndMergeBehavior = Core.AttributeCompareAndMergeBehavior.CompareOverriddenValuesOnly;
                                Enum.TryParse<AttributeCompareAndMergeBehavior>(strCompareBehavior, true, out attributeCompareAndMergeBehavior);
                                this.AttributeCompareAndMergeBehavior = attributeCompareAndMergeBehavior;
                            }
                            if (reader.MoveToAttribute("ApplyDMS"))
                            {
                                this.ApplyDMS = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._applyDMS);
                            }

                            if (reader.MoveToAttribute("ValidateLeafCategory"))
                            {
                                this.ValidateLeafCategory = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._validateLeafCategory);
                            }
                        }

                        #endregion
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "MatchProcessingOptions")
                    {
                        #region Read MatchProcessingOptions

                        String matchProcessingOptionsXml = reader.ReadOuterXml();
                        if (!String.IsNullOrEmpty(matchProcessingOptionsXml))
                        {
                            MatchProcessingOptions matchProcessingOptions = new MatchProcessingOptions(matchProcessingOptionsXml);
                            this.MatchProcessingOptions = matchProcessingOptions;
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

        #endregion Private methods

        #endregion Methods
    }
}