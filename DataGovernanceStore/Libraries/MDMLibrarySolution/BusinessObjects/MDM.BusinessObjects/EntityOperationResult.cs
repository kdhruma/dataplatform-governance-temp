using MDM.BusinessObjects.DQM;
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
    /// Specifies results for Entity operations
    /// </summary>
    [DataContract]
    [KnownType(typeof(AttributeOperationResultCollection))]
    [KnownType(typeof(RelationshipOperationResultCollection))]
    public class EntityOperationResult : OperationResult, IEntityOperationResult
    {
        #region Fields

        /// <summary>
        /// Field denoting the id of the entity for which results are created
        /// </summary>
        private Int64 _entityId = 0;

        /// <summary>
        /// Field denoting external id of the entity for which results are created
        /// </summary>
        private String _externalId = String.Empty;

        /// <summary>
        /// Field denoting referece id of the entity for which results are created
        /// </summary>
        private Int64 _referenceId = -1;

        /// <summary>
        /// Field denoting the long name of the entity for which results are created
        /// </summary>
        private String _entityLongName = String.Empty;

        /// <summary>
        /// Field denoting the operation result collection for the attributes of the entity in the context 
        /// </summary>
        private AttributeOperationResultCollection _attributeOperationResultCollection = new AttributeOperationResultCollection();

        /// <summary>
        /// Field denoting the operation result collection for the relationships of the entity in the context 
        /// </summary>
        private RelationshipOperationResultCollection _relationshipOperationResultCollection = new RelationshipOperationResultCollection();

        /// <summary>
        /// Field denoting the DataQualityIndicator value collection of the entity in the context 
        /// </summary>
        private Collection<NamedDataQualityIndicatorValue> _dataQualityIndicatorValueCollection = new Collection<NamedDataQualityIndicatorValue>();

        /// <summary>
        /// Field denoting the normalization results value collection of the entity in the context 
        /// </summary>
        private NormalizationResultsCollection normalizationResults = new NormalizationResultsCollection();

        /// <summary>
        /// Holds passed business condition id list
        /// </summary>
        private Collection<Int32> _passedBusinessConditionIdList = null;

        /// <summary>
        /// Holds failed business condition id list
        /// </summary>
        private Collection<Int32> _failedBusinessConditionIdList = null;

        /// <summary>
        /// Field denoting the GUID of the entity for which results are created
        /// </summary>
        private String _entityGUID = String.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Property defining the type of the Operation Result
        /// </summary>
        public new String ObjectType
        {
            get
            {
                return "EntityOperationResult";
            }
        }

        /// <summary>
        /// Property denoting the id of the entity for which results are created
        /// </summary>
        [DataMember]
        public Int64 EntityId
        {
            get
            {
                return _entityId;
            }
            set
            {
                _entityId = value;
            }
        }

        /// <summary>
        /// Property denoting the external id of the entity for which results are created
        /// </summary>
        [DataMember]
        public String ExternalId
        {
            get
            {
                return this._externalId;
            }
            set
            {
                this._externalId = value;
            }
        }

        /// <summary>
        /// Property denoting the reference id of the entity for which results are created
        /// </summary>
        [DataMember]
        new public Int64 ReferenceId
        {
            get
            {
                return this._referenceId;
            }
            set
            {
                this._referenceId = value;
            }
        }

        /// <summary>
        /// Property denoting the long name of the entity for which results are created
        /// </summary>
        [DataMember]
        public String EntityLongName
        {
            get
            {
                return _entityLongName;
            }
            set
            {
                _entityLongName = value;
            }
        }

        /// <summary>
        /// Property denoting the operation result collection for the attributes of the entity in the context 
        /// </summary>
        [DataMember]
        public AttributeOperationResultCollection AttributeOperationResultCollection
        {
            get
            {
                return _attributeOperationResultCollection;
            }
            set
            {
                _attributeOperationResultCollection = value;
            }
        }

        /// <summary>
        /// Property denoting the operation result collection for relationships of the entity in the context 
        /// </summary>
        [DataMember]
        public RelationshipOperationResultCollection RelationshipOperationResultCollection
        {
            get
            {
                return _relationshipOperationResultCollection;
            }
            set
            {
                _relationshipOperationResultCollection = value;
            }
        }

        /// <summary>
        /// Property denoting the DataQualityIndicator values collection of the entity for which results are created
        /// </summary>
        [DataMember]
        public Collection<NamedDataQualityIndicatorValue> NamedDataQualityIndicatorValues
        {
            get
            {
                return _dataQualityIndicatorValueCollection;
            }
            set
            {
                _dataQualityIndicatorValueCollection = value;
            }
        }

        /// <summary>
        /// Property denoting the normalization results value collection of the entity in the context 
        /// </summary>
        [DataMember]
        public NormalizationResultsCollection NormalizationResults
        {
            get
            {
                return normalizationResults;
            }
            set
            {
                normalizationResults = value;
            }
        }

        /// <summary>
        /// Holds passed business condition id list
        /// </summary>
        [DataMember]
        public Collection<Int32> PassedBusinessConditionIdList
        {
            get
            {
                if (this._passedBusinessConditionIdList == null)
                {
                    this._passedBusinessConditionIdList = new Collection<Int32>();
                }

                return this._passedBusinessConditionIdList;
            }
            set
            {
                this._passedBusinessConditionIdList = value;
            }
        }

        /// <summary>
        /// Holds failed business condition id list
        /// </summary>
        [DataMember]
        public Collection<Int32> FailedBusinessConditionIdList
        {
            get
            {
                if (this._failedBusinessConditionIdList == null)
                {
                    this._failedBusinessConditionIdList = new Collection<Int32>();
                }

                return this._failedBusinessConditionIdList;
            }
            set
            {
                this._failedBusinessConditionIdList = value;
            }
        }

        

        /// <summary>
        /// Indicates entity GUID
        /// </summary>
        public String EntityGUID
        {
            get 
            { 
                return this._entityGUID; 
            }
            set 
            { 
               this. _entityGUID = value; 
            }
        }
        

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes new instance of entity operation result
        /// </summary>
        public EntityOperationResult()
        {

        }

        /// <summary>
        /// Initializes new instance of entity operation result with the provided entity id
        /// </summary>
        /// <param name="entityId">Id of the entity</param>
        /// <param name="entityLongName">Long name of the entity</param>
        public EntityOperationResult(Int64 entityId, String entityLongName)
        {
            this.EntityId = entityId;
            this.EntityLongName = entityLongName;
        }

        /// <summary>
        /// Initializes new instance of entity operation result with the provided entity id
        /// </summary>
        /// <param name="entityId">Id of the entity</param>
        /// <param name="externalId">External id of the entity</param>
        /// <param name="entityLongName">Long name of the entity</param>
        public EntityOperationResult(Int64 entityId, String externalId, String entityLongName)
        {
            this.EntityId = entityId;
            this.ExternalId = externalId;
            this.EntityLongName = entityLongName;
        }

        /// <summary>
        /// Initializes new instance of entity operation result with the provided entity id
        /// </summary>
        /// <param name="entityId">Id of the entity</param>
        /// <param name="referenceId">Reference id of the entity</param>
        /// <param name="entityLongName">Long name of the entity</param>
        public EntityOperationResult(Int64 entityId, Int64 referenceId, String entityLongName)
        {
            this.EntityId = entityId;
            this.ReferenceId = referenceId;
            this.EntityLongName = entityLongName;
        }

        /// <summary>
        /// Initializes new instance of entity operation result with the provided input fields
        /// </summary>
        /// <param name="entityId">Indicates Id of the entity if available</param>
        /// <param name="referenceId">Indicates Reference id of the entity</param>
        /// <param name="entityLongName">Indicates Long name of the entity</param>
        /// <param name="externalId">Indicates External Id</param>
        public EntityOperationResult(Int64 entityId, Int64 referenceId, String entityLongName, String externalId)
        {
            this.EntityId = entityId;
            this.ReferenceId = referenceId;
            this.EntityLongName = entityLongName;
            this.ExternalId = externalId;
        }

        /// <summary>
        /// Initialize new instance of entity operation result from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for entity operation result</param>
        public EntityOperationResult(String valuesAsXml)
        {
            LoadOperationResult(valuesAsXml);
        }

        /// <summary>
        /// Initializes a new instance of Entity Operation Result for the requested entity
        /// </summary>
        /// <param name="entity">Entity as per which entity operation result is going to be initialized</param>
        public EntityOperationResult(Entity entity)
        {
            PrepareEntityOperationResultSchema(entity);
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Initialize current object with Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for current OperationResult
        /// <para>
        /// Sample Xml:
        /// <![CDATA[
        /// <EntityOperationResult Id="1234" Name="P2198" Status="Failed">
        ///     <Errors>
        ///         <Error Code="" Message = "EntityCreated BR failed"/>
        ///     </Errors>
        ///     <Informations>
        ///         <Information Code="" Message="Entity process successful : Id = 4376255, Name = 116674, LongName = 116674, SKU = 116674, Path = " />
        ///     </Informations>
        ///     <ReturnValues>
        ///         <ReturnValue>
        ///             <Entities>
        ///                 <Entity Id="4376255" ObjectId="318824" Name="116674" LongName="116674" SKU="116674"/>
        ///             </Entities>
        ///         </ReturnValue>
        ///     </ReturnValues>
        ///     <AttributeOperationResults Status="Failed">
        ///         <AttributeOperationResult Id="9812" LongName="Description" Status="Failed">
        ///             <Errors>
        ///                 <Error Code="" Message = "Required Attributes are not filled"/>
        ///             </Errors>
        ///         </AttributeOperationResult>
        ///     </AttributeOpeartionResults>
        /// </EntityOperationResult>
        /// ]]>
        /// </para>
        /// </param>
        public new void LoadOperationResult(String valuesAsXml)
        {
            #region Sample Xml
            /*
             <EntityOperationResult Id="1234" Name="P2198" Status="Failed">
                <Errors>
                    <Error Code="" Message = "EntityCreated BR failed"/>
                </Errors>
                <Informations>
                    <Information Code="" Message="Entity process successful : Id = 4376255, Name = 116674, LongName = 116674, SKU = 116674, Path = " />
                </Informations>
                <ReturnValues>
                    <ReturnValue>
                        <Entities>
                            <Entity Id="4376255" ObjectId="318824" Name="116674" LongName="116674" SKU="116674"/>
                        </Entities>
                    </ReturnValue>
                </ReturnValues>
                <AttributeOperationResults Status="Failed">
                    <AttributeOperationResult Id="9812" LongName="Description" Status="Failed">
                        <Errors>
                            <Error Code="" Message = "Required Attributes are not filled"/>
                        </Errors>
                    </AttributeOperationResult>
                </AttributeOpeartionResults>
             </EntityOperationResult>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityOperationResult")
                        {
                            #region Read EntityOperationResult attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.EntityId = reader.ReadContentAsLong();
                                }

                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.EntityLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ExternalId"))
                                {
                                    this.ExternalId = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ReferenceId"))
                                {
                                    this.ReferenceId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("Status"))
                                {
                                    OperationResultStatusEnum operationResultStatus = OperationResultStatusEnum.None;
                                    String strOperationResultStatus = reader.ReadContentAsString();

                                    if (!String.IsNullOrWhiteSpace(strOperationResultStatus))
                                        Enum.TryParse<OperationResultStatusEnum>(strOperationResultStatus, out operationResultStatus);

                                    this.OperationResultStatus = operationResultStatus;
                                }

                                if (reader.MoveToAttribute("PerformedAction"))
                                {
                                    ObjectAction performedAction = ObjectAction.Unknown;
                                    Enum.TryParse(reader.ReadContentAsString(), out performedAction);
                                    this.PerformedAction = performedAction;
                                }
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Error")
                        {
                            //Read error
                            #region Read error

                            String errorXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(errorXml))
                            {
                                Error error = new Error(errorXml);

                                if (error != null)
                                {
                                    if (this.Errors == null)
                                        this.Errors = new ErrorCollection();

                                    this.Errors.Add(error);
                                }
                            }

                            #endregion Read error
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Information")
                        {
                            //Read Information
                            #region Read Information

                            String infoXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(infoXml))
                            {
                                Information info = new Information(infoXml);

                                if (info != null)
                                {
                                    if (this.Informations == null)
                                        this.Informations = new InformationCollection();

                                    this.Informations.Add(info);
                                }
                            }

                            #endregion Read Information
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Warning")
                        {
                            //Read Warning
                            #region Read Warning

                            String warningXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(warningXml))
                            {
                                Warning warning = new Warning(warningXml);

                                if (warning != null)
                                {
                                    if (this.Warnings == null)
                                        this.Warnings = new WarningCollection();

                                    this.Warnings.Add(warning);
                                }
                            }

                            #endregion Read Warning
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ReturnValue")
                        {
                            //Read ReturnValues
                            #region Read ReturnValues

                            if (this.ReturnValues == null)
                                this.ReturnValues = new Collection<Object>();

                            this.ReturnValues.Add(reader.ReadInnerXml());

                            #endregion Read ReturnValues
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeOperationResults")
                        {
                            //Read Attribute Operation Results
                            #region Read Attribute Operation Results

                            String aorXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(aorXml))
                            {
                                AttributeOperationResultCollection aorCollection = new AttributeOperationResultCollection(aorXml);

                                if (aorCollection != null)
                                {
                                    this._attributeOperationResultCollection = aorCollection;
                                }
                            }

                            #endregion Read Attribute Operation Results
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "RelationshipOperationResults")
                        {
                            //Read Relationship Operation Results
                            #region Read Relationship Operation Results

                            String rorXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(rorXml))
                            {
                                RelationshipOperationResultCollection rorCollection = new RelationshipOperationResultCollection(rorXml);

                                if (rorCollection != null)
                                {
                                    this._relationshipOperationResultCollection = rorCollection;
                                }
                            }

                            #endregion Read Relationship Operation Results
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

        /// <summary>
        /// Get Xml representation of Entity Operation Result
        /// </summary>
        /// <returns>Xml representation of Entity Operation Result object</returns>
        public new String ToXml()
        {
            String entityOperationResultXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Operation result node start
            xmlWriter.WriteStartElement("EntityOperationResult");

            #region Write entity operation result properties

            xmlWriter.WriteAttributeString("Id", this.EntityId.ToString());
            xmlWriter.WriteAttributeString("Name", this.EntityLongName.ToString());
            xmlWriter.WriteAttributeString("ExternalId", this.ExternalId.ToString());
            xmlWriter.WriteAttributeString("ReferenceId", this.ReferenceId.ToString());
            xmlWriter.WriteAttributeString("Status", this.OperationResultStatus.ToString());
            xmlWriter.WriteAttributeString("PerformedAction", this.PerformedAction.ToString());

            #endregion

            #region Write Errors

            //Add Error Nodes
            xmlWriter.WriteStartElement("Errors");

            foreach (Error error in this.Errors)
            {
                xmlWriter.WriteRaw(error.ToXml());
            }

            //Error nodes end
            xmlWriter.WriteEndElement();

            #endregion Write Errors

            #region Write Information

            //Add Information Nodes
            xmlWriter.WriteStartElement("Informations");

            foreach (Information information in this.Informations)
            {
                xmlWriter.WriteRaw(information.ToXml());
            }

            //Information node end
            xmlWriter.WriteEndElement();

            #endregion Write Information

            #region Write Warning

            //Add Warning Nodes
            xmlWriter.WriteStartElement("Warnings");

            foreach (Warning warning in this.Warnings)
            {
                xmlWriter.WriteRaw(warning.ToXml());
            }

            //Warning node end
            xmlWriter.WriteEndElement();

            #endregion Write Warning

            #region Write return values

            //Add Return Values Nodes
            xmlWriter.WriteStartElement("ReturnValues");

            foreach (Object returnValue in this.ReturnValues)
            {
                xmlWriter.WriteStartElement("ReturnValue");
                xmlWriter.WriteRaw(returnValue.ToString());
                xmlWriter.WriteEndElement();
            }

            //Return values node end
            xmlWriter.WriteEndElement();

            #endregion Write return values

            #region Write attribute operation results

            String attributeOperationResultsXml = this.AttributeOperationResultCollection.ToXml();
            xmlWriter.WriteRaw(attributeOperationResultsXml);

            #endregion Write attribute operation results

            #region Write relationship operation results

            if (this.RelationshipOperationResultCollection != null)
            {
                String relationshipOperationResultsXml = this.RelationshipOperationResultCollection.ToXml();
                xmlWriter.WriteRaw(relationshipOperationResultsXml);
            }

            #endregion Write attribute operation results

            //Operation result node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            entityOperationResultXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return entityOperationResultXml;
        }

        /// <summary>
        /// Get Xml representation of Entity operation result based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Entity operation result</returns>
        public new String ToXml(ObjectSerialization objectSerialization)
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

                //Operation result node start
                xmlWriter.WriteStartElement("EntityOperationResult");

                #region Write entity operation result properties

                xmlWriter.WriteAttributeString("Id", this.EntityId.ToString());
                xmlWriter.WriteAttributeString("Name", this.EntityLongName.ToString());
                xmlWriter.WriteAttributeString("ExternalId", this.ExternalId.ToString());
                xmlWriter.WriteAttributeString("ReferenceId", this.ReferenceId.ToString());
                xmlWriter.WriteAttributeString("Status", this.OperationResultStatus.ToString());
                xmlWriter.WriteAttributeString("PerformedAction", this.PerformedAction.ToString());

                #endregion

                #region Write Errors

                //Add Error Nodes
                xmlWriter.WriteStartElement("Errors");

                foreach (Error error in this.Errors)
                {
                    xmlWriter.WriteRaw(error.ToXml(objectSerialization));
                }

                //Error nodes end
                xmlWriter.WriteEndElement();

                #endregion Write Errors

                #region Write Information

                //Add Information Nodes
                xmlWriter.WriteStartElement("Informations");

                foreach (Information information in this.Informations)
                {
                    xmlWriter.WriteRaw(information.ToXml(objectSerialization));
                }

                //Information node end
                xmlWriter.WriteEndElement();

                #endregion Write Information

                #region Write Warning

                //Add Warning Nodes
                xmlWriter.WriteStartElement("Warnings");

                foreach (Warning warning in this.Warnings)
                {
                    xmlWriter.WriteRaw(warning.ToXml(objectSerialization));
                }

                //Warning node end
                xmlWriter.WriteEndElement();

                #endregion Write Warning

                #region Write return values

                //Add Return Values Nodes
                xmlWriter.WriteStartElement("ReturnValues");

                foreach (Object returnValue in this.ReturnValues)
                {
                    xmlWriter.WriteStartElement("ReturnValue");
                    xmlWriter.WriteRaw(returnValue.ToString());
                    xmlWriter.WriteEndElement();
                }

                //Return values node end
                xmlWriter.WriteEndElement();

                #endregion Write return values

                #region Write attribute operation results

                String attributeOperationResultsXml = this.AttributeOperationResultCollection.ToXml(objectSerialization);
                xmlWriter.WriteRaw(attributeOperationResultsXml);

                #endregion Write attribute operation results

                #region Write relationship operation results

                if (this.RelationshipOperationResultCollection != null)
                {
                    String relationshipOperationResultsXml = this.RelationshipOperationResultCollection.ToXml(objectSerialization);
                    xmlWriter.WriteRaw(relationshipOperationResultsXml);
                }

                #endregion Write attribute operation results

                //Operation result node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //Get the actual XML
                returnXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }

            return returnXml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeOperationResultCollection"></param>
        public void SetAttributeOperationResults(AttributeOperationResultCollection attributeOperationResultCollection)
        {
            attributeOperationResultCollection.RefreshOperationResultStatus();

            this._attributeOperationResultCollection = attributeOperationResultCollection;

            if (attributeOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.Failed
                           || attributeOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
            {
                this.OperationResultStatus = OperationResultStatusEnum.CompletedWithErrors;
            }
            else if (attributeOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
            {
                this.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipOperationResultCollection"></param>
        public void SetRelationshipOperationResults(RelationshipOperationResultCollection relationshipOperationResultCollection)
        {
            relationshipOperationResultCollection.RefreshOperationResultStatus();

            this._relationshipOperationResultCollection = relationshipOperationResultCollection;

            if (relationshipOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.Failed
                           || relationshipOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
            {
                this.OperationResultStatus = OperationResultStatusEnum.CompletedWithErrors;
            }
            else if (relationshipOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
            {
                this.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
            }
        }

        /// <summary>
        /// Adds attribute operation result
        /// </summary>
        /// <param name="attributeId">Id of the attribute for which result needs to be added</param>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        public Boolean AddAttributeOperationResult(Int32 attributeId, String resultCode, String resultMessage, OperationResultType operationResultType)
        {
            Boolean addSuccess = false;

            if (this._attributeOperationResultCollection != null && this._attributeOperationResultCollection.Count > 0)
            {
                addSuccess = this._attributeOperationResultCollection.AddAttributeOperationResult(attributeId, resultCode, resultMessage, operationResultType);

                //If the Attribute Collection overall operation result is Failed or Completed With Errors, the entity operation result status should be Failed
                if (this._attributeOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.Failed || this._attributeOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
                    this.OperationResultStatus = OperationResultStatusEnum.Failed;
                else if (this._attributeOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
                    this.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
            }

            return addSuccess;
        }

        /// <summary>
        /// Adds relationship operation result
        /// </summary>
        /// <param name="relationshipId">Indicates Id of the relationship for which result needs to be added</param>
        /// <param name="resultCode">Indicates Result Code</param>
        /// <param name="resultMessage">Indicates Result Message</param>
        /// <param name="operationResultType">Indicates The type of the result which needs to be added</param>
        /// <returns>Returns Boolean result saying whether add is successful or not</returns>
        public Boolean AddRelationshipOperationResult(Int32 relationshipId, String resultCode, String resultMessage, OperationResultType operationResultType)
        {
            Boolean addSuccess = false;

            if (this._relationshipOperationResultCollection != null && this._relationshipOperationResultCollection.Count > 0)
            {
                addSuccess = this._relationshipOperationResultCollection.AddRelationshipOperationResult(relationshipId, resultCode, resultMessage, operationResultType);

                //If the Attribute Collection overall operation result is Failed or Completed With Errors, the entity operation result status should be Failed
                if (this._relationshipOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.Failed || this._relationshipOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
                    this.OperationResultStatus = OperationResultStatusEnum.Failed;
                else if (this._relationshipOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
                    this.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
            }

            return addSuccess;
        }

        /// <summary>
        /// Adds relationship attribute operation result
        /// </summary>
        /// <param name="relationshipId">Id of the relationship having attribute</param>
        /// <param name="attributeId">Id of the attribute for which result needs to be added</param>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        public Boolean AddRelationshipAttributeOperationResult(Int32 relationshipId, Int32 attributeId, String resultCode, String resultMessage, OperationResultType operationResultType)
        {
            Boolean addSuccess = false;

            addSuccess = this._relationshipOperationResultCollection.AddAttributeOperationResult(relationshipId, attributeId, resultCode, resultMessage, operationResultType);

            //If the Attribute Collection overall operation result is Failed or Completed With Errors, the entity operation result status should be Failed
            if (this._relationshipOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.Failed || this._relationshipOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
                this.OperationResultStatus = OperationResultStatusEnum.Failed;
            else if (this._relationshipOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
                this.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;

            return addSuccess;
        }

        /// <summary>
        /// Adds relationship attribute operation result
        /// </summary>
        /// <param name="relationshipTypeName">Name of the relationship type having attribute</param>
        /// <param name="attributeName">Name of the attribute for which result needs to be added</param>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="parameters">Message format parameters</param>
        /// <param name="reasonType">Reason type</param>
        /// <param name="ruleMapContextId">Rule map context id</param>
        /// <param name="ruleId">Rule id</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <param name="ignoreError">Indicates whether to show the errors on UI or not. Default value is false</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        public Boolean AddRelationshipAttributeOperationResult(String relationshipTypeName, String attributeName, String resultCode, String resultMessage, Collection<Object> parameters, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType, Boolean ignoreError = false)
        {
            Boolean addSuccess = false;

            addSuccess = this._relationshipOperationResultCollection.AddAttributeOperationResult(relationshipTypeName, attributeName, resultCode, resultMessage, parameters, reasonType, ruleMapContextId, ruleId, operationResultType, ignoreError);

            //If the Attribute Collection overall operation result is Failed or Completed With Errors, the entity operation result status should be Failed
            if (this._relationshipOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.Failed || this._relationshipOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
            {
                this.OperationResultStatus = OperationResultStatusEnum.Failed;
            }
            else if (this._relationshipOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
            {
                this.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
            }

            return addSuccess;
        }

        /// <summary>
        /// Adds relationship operation result
        /// </summary>
        /// <param name="relationshipTypeName">Indicates Type Name of the relationship for which result needs to be added</param>
        /// <param name="resultCode">Indicates Result Code</param>
        /// <param name="resultMessage">Indicates Result Message</param>
        /// <param name="parameters">Indicates message format parameters</param>
        /// <param name="reasonType">Reason type</param>
        /// <param name="ruleMapContextId">Rule map context id</param>
        /// <param name="ruleId">Rule id</param>
        /// <param name="operationResultType">Indicates The type of the result which needs to be added</param>
        /// <param name="ignoreError">Indicates whether to show the errors on UI or not. Default value is false</param>
        /// <returns>Returns Boolean result saying whether add is successful or not</returns>
        public Boolean AddRelationshipOperationResult(String relationshipTypeName, String resultCode, String resultMessage, Collection<Object> parameters, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType, Boolean ignoreError = false)
        {
            Boolean addSuccess = false;

            if (this._relationshipOperationResultCollection != null && this._relationshipOperationResultCollection.Count > 0)
            {
                addSuccess = this._relationshipOperationResultCollection.AddRelationshipOperationResult(relationshipTypeName, resultCode, resultMessage, parameters, reasonType, ruleMapContextId, ruleId, operationResultType, ignoreError);

                //If the Attribute Collection overall operation result is Failed or Completed With Errors, the entity operation result status should be Failed
                if (this._relationshipOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.Failed || this._relationshipOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
                {
                    this.OperationResultStatus = OperationResultStatusEnum.Failed;
                }
                else if (this._relationshipOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
                {
                    this.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
                }
            }

            return addSuccess;
        }

        /// <summary>
        /// This method copies entire EntityOperationResult
        /// </summary>
        /// <param name="entityOperationResult">source entity operation result</param>
        /// <param name="copyEntityMetadata">Boolean flag which indicates if copying entity metadata is required or not.</param>
        public void CopyEntityOperationResult(EntityOperationResult entityOperationResult, Boolean copyEntityMetadata)
        {
            #region Parameter Validation

            if (entityOperationResult == null)
            {
                throw new ArgumentNullException("Entity OperationResult");
            }

            #endregion Parameter Validation

            if (copyEntityMetadata)
            {
                this.EntityId = entityOperationResult.EntityId;
                this.EntityLongName = entityOperationResult.EntityLongName;
                this.ExternalId = entityOperationResult.ExternalId;
                this.ReferenceId = entityOperationResult.ReferenceId;
                this.PerformedAction = entityOperationResult.PerformedAction;
            }

            //Copy OperationResultStatus from Source to Target if OperationResultStatus of Source is Failed/ CompletedWithErrors. 
            //If None/ Successful, do not copy.
            if (entityOperationResult.OperationResultStatus == OperationResultStatusEnum.Failed || entityOperationResult.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
            {
                this.OperationResultStatus = entityOperationResult.OperationResultStatus;
            }
            else if (entityOperationResult.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
            {
                this.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
            }

            #region Copy Errors from Source to Target

            if (entityOperationResult.Errors != null && entityOperationResult.Errors.Count > 0)
            {
                if (this.Errors == null)
                {
                    this.Errors = new ErrorCollection();
                }

                this.Errors.AddRange(entityOperationResult.Errors);
            }

            #endregion Copy Errors from Source to Target

            #region Copy Informations from Source to Target

            if (entityOperationResult.Informations != null && entityOperationResult.Informations.Count > 0)
            {
                if (this.Informations == null)
                {
                    this.Informations = new InformationCollection();
                }

                this.Informations.AddRange(entityOperationResult.Informations);
            }

            #endregion Copy Informations from Source to Target

            #region Copy Warnings from Source to Target

            if (entityOperationResult.Warnings != null && entityOperationResult.Warnings.Count > 0)
            {
                if (this.Warnings == null)
                {
                    this.Warnings = new WarningCollection();
                }

                this.Warnings.AddRange(entityOperationResult.Warnings);
            }

            #endregion Copy Warnings from Source to Target

            #region Copy Return Values from Source to Target

            if (entityOperationResult.ReturnValues != null)
            {
                if (this.ReturnValues == null)
                {
                    this.ReturnValues = new Collection<Object>();
                }

                foreach (Object val in entityOperationResult.ReturnValues)
                {
                    this.ReturnValues.Add(val);
                }
            }

            #endregion Copy Return Values from Source to Target

            #region Copy Attribute Operation Result Collection from Source to Target

            CopyAttributeOperationResults(entityOperationResult.AttributeOperationResultCollection, this.AttributeOperationResultCollection);

            #endregion Copy Attribute Operation Result Collection from Source to Target

            #region Copy Relationship Operation Result Collection from Source to Target

            if (entityOperationResult.RelationshipOperationResultCollection != null)
            {
                this.RelationshipOperationResultCollection.OperationResultStatus = entityOperationResult.RelationshipOperationResultCollection.OperationResultStatus;

                foreach (RelationshipOperationResult relationshipOperationResult in entityOperationResult.RelationshipOperationResultCollection)
                {
                    RelationshipOperationResult targetRelationshipOperationResult = this.RelationshipOperationResultCollection.GetRelationshipOperationResult(relationshipOperationResult.RelationshipId);

                    if (targetRelationshipOperationResult == null)
                    {
                        this.RelationshipOperationResultCollection.Add(relationshipOperationResult);
                    }
                    else
                    {
                        CopyAttributeOperationResults(relationshipOperationResult.AttributeOperationResultCollection, targetRelationshipOperationResult.AttributeOperationResultCollection);
                    }
                }

                //copy any Relationship errors at entity level 
                if (entityOperationResult.RelationshipOperationResultCollection.Errors!=null && entityOperationResult.RelationshipOperationResultCollection.Errors.Count > 0)
                {
                    if (this.RelationshipOperationResultCollection.Errors == null)
                    {
                        this.RelationshipOperationResultCollection.Errors = entityOperationResult.RelationshipOperationResultCollection.Errors;
                    }
                    else
                    {
                        foreach (Error error in entityOperationResult.RelationshipOperationResultCollection.Errors)
                        {
                            this.RelationshipOperationResultCollection.Errors.Add(error);
                        }
                    }
                }

                this.RelationshipOperationResultCollection.RefreshOperationResultStatus();
            }

            #endregion Copy Relationship Operation Result Collection from Source to Target
        }

        /// <summary>
        /// This method copies entire EntityOperationResult
        /// </summary>
        /// <param name="iEntityOperationResult">source entity operation result</param>
        /// <param name="copyEntityMetadata">boolean flag which indicates if copying entity metadata is required or not.</param>
        public void CopyEntityOperationResult(IEntityOperationResult iEntityOperationResult, Boolean copyEntityMetadata)
        {
            this.CopyEntityOperationResult(iEntityOperationResult as EntityOperationResult, copyEntityMetadata);
        }

        /// <summary>
        /// Get collection of errors from current EntityOperationResult. This will merge errors from current EntityOR and errors from all Attribute operation result.
        /// </summary>
        /// <returns>Collection of errors.</returns>
        public ErrorCollection GetAllErrors()
        {
            ErrorCollection errors = new ErrorCollection();

            //Add errors form current object
            if (this.Errors != null && this.Errors.Count > 0)
            {
                foreach (Error error in this.Errors)
                {
                    errors.Add(error);
                }
            }

            //Get errors from all Attribute operation result
            if (this.AttributeOperationResultCollection != null && this.AttributeOperationResultCollection.Count > 0)
            {
                foreach (AttributeOperationResult attributeOR in this.AttributeOperationResultCollection)
                {
                    if (attributeOR.Errors != null && attributeOR.Errors.Count > 0)
                    {
                        foreach (Error attributeError in attributeOR.Errors)
                        {
                            errors.Add(attributeError);
                        }
                    }
                }
            }

            //Get errors from all relationship operation result
            if (this.RelationshipOperationResultCollection != null && this.RelationshipOperationResultCollection.Count > 0)
            {
                foreach (RelationshipOperationResult relationshipOR in this.RelationshipOperationResultCollection)
                {
                    AddRelationshipErrors(relationshipOR, errors);
                }
                //Get errors at entity level
                if (this.RelationshipOperationResultCollection.Errors != null && this.RelationshipOperationResultCollection.Errors.Count > 0)
                {
                    foreach (Error error in this.RelationshipOperationResultCollection.Errors)
                    {
                        errors.Add(error);
                    }
                }
            }

            return errors;
        }

        /// <summary>
        /// Get collection of warnings from current EntityOperationResult. This will merge warnings from current EntityOR and warnings from all Attribute operation result.
        /// </summary>
        /// <returns>Collection of warnings.</returns>
        public WarningCollection GetAllWarnings()
        {
            WarningCollection warnings = new WarningCollection();

            //Add errors form current object
            if (this.Warnings != null && this.Warnings.Count > 0)
            {
                foreach (Warning warning in this.Warnings)
                {
                    warnings.Add(warning);
                }
            }

            //Get warnings from all Attribute operation result
            if (this.AttributeOperationResultCollection != null && this.AttributeOperationResultCollection.Count > 0)
            {
                foreach (AttributeOperationResult attributeOR in this.AttributeOperationResultCollection)
                {
                    if (attributeOR.Warnings != null && attributeOR.Warnings.Count > 0)
                    {
                        foreach (Warning attributeWarning in attributeOR.Warnings)
                        {
                            warnings.Add(attributeWarning);
                        }
                    }
                }
            }

            //Get warnings from all relationship operation result
            if (this.RelationshipOperationResultCollection != null && this.RelationshipOperationResultCollection.Count > 0)
            {
                foreach (RelationshipOperationResult relationshipOR in this.RelationshipOperationResultCollection)
                {
                    AddRelationshipWarnings(relationshipOR, warnings);
                }
            }

            return warnings;
        }

        /// <summary>
        /// Get collection of Information from current EntityOperationResult. This will merge information from current EntityOR and informations from all Attribute operation result.
        /// </summary>
        /// <returns>Collection of information.</returns>
        public InformationCollection GetAllInformations()
        {
            InformationCollection informations = new InformationCollection();

            //Add errors form current object
            if (this.Informations != null && this.Informations.Count > 0)
            {
                foreach (Information information in this.Informations)
                {
                    informations.Add(information);
                }
            }

            //Get informations from all Attribute operation result
            if (this.AttributeOperationResultCollection != null && this.AttributeOperationResultCollection.Count > 0)
            {
                foreach (AttributeOperationResult attributeOR in this.AttributeOperationResultCollection)
                {
                    if (attributeOR.Informations != null && attributeOR.Informations.Count > 0)
                    {
                        foreach (Information attributeInformation in attributeOR.Informations)
                        {
                            informations.Add(attributeInformation);
                        }
                    }
                }
            }

            //Get informations from all relationship operation result
            if (this.RelationshipOperationResultCollection != null && this.RelationshipOperationResultCollection.Count > 0)
            {
                foreach (RelationshipOperationResult relationshipOR in this.RelationshipOperationResultCollection)
                {
                    AddRelationshipInformations(relationshipOR, informations);
                }
            }

            return informations;
        }

        /// <summary>
        /// Get collection of AttributeOperation result. Which contains errors and warnings
        /// </summary>
        /// <returns>AttributeOperationResultCollection</returns>
        public IAttributeOperationResultCollection GetAttributeOperationResultCollection()
        {
            IAttributeOperationResultCollection iAttributeOperationResultCollection = null;

            if (this.AttributeOperationResultCollection != null)
            {
                iAttributeOperationResultCollection = (IAttributeOperationResultCollection)this.AttributeOperationResultCollection;
            }
            return iAttributeOperationResultCollection;
        }

        /// <summary>
        ///  Get collection of Relationship Operation result. Which contains errors and warnings
        /// </summary>
        /// <returns>IRelationshipOperationResultCollection</returns>
        public IRelationshipOperationResultCollection GetRelationshipOperationResultCollection()
        {
            IRelationshipOperationResultCollection iRelationshipOperationResultCollection = null;

            if (this.RelationshipOperationResultCollection != null)
            {
                iRelationshipOperationResultCollection = (IRelationshipOperationResultCollection)this.RelationshipOperationResultCollection;
            }

            return iRelationshipOperationResultCollection;
        }

        /// <summary>
        /// Checks if current EntityOperationResult is superset of passed EntityOperationResult
        /// </summary>
        /// <param name="subsetEntityOperationResult">Indicates EntityOperationResult which should be compared</param>
        /// <param name="compareIds">Indicates Ids should have to be compared or not</param>
        /// <returns>True or False based on comparison</returns>
        public Boolean IsSuperSetOf(EntityOperationResult subsetEntityOperationResult, Boolean compareIds = false)
        {
            if (subsetEntityOperationResult == null)
            {
                // validate
            }
            if (compareIds)
            {
                if (this.Id != subsetEntityOperationResult.Id)
                    return false;

                if (this.EntityId != subsetEntityOperationResult.Id)
                    return false;

                if (this.ReferenceId != subsetEntityOperationResult.ReferenceId)
                    return false;
            }

            if (this.EntityLongName != subsetEntityOperationResult.EntityLongName)
                return false;

            if (this.ExternalId != subsetEntityOperationResult.ExternalId)
                return false;

            if (!base.IsSuperSetOf(subsetEntityOperationResult, compareIds))
                return false;

            if (!this.AttributeOperationResultCollection.IsSuperSetOf(subsetEntityOperationResult.AttributeOperationResultCollection, compareIds))
                return false;

            if (!this.RelationshipOperationResultCollection.IsSuperSetOf(subsetEntityOperationResult.RelationshipOperationResultCollection, compareIds))
                return false;

            return true;
        }

        /// <summary>
        /// Prepares Entity Operation Result schema as per the requested entity
        /// </summary>
        /// <param name="entity">Entity for which operation result schema has to be prepared</param>
        public void PrepareEntityOperationResultSchema(Entity entity)
        {
            if (entity != null)
            {
                EntityOperationResult entityOperationResult = new EntityOperationResult(entity.Id, entity.ReferenceId, entity.LongName, entity.ExternalId);

                if (entity.Attributes != null && entity.Attributes.Count > 0)
                {
                    entityOperationResult.AttributeOperationResultCollection = new AttributeOperationResultCollection(entity.Attributes);
                }

                if (entity.Relationships != null && entity.Relationships.Count > 0)
                {
                    entityOperationResult.RelationshipOperationResultCollection = new RelationshipOperationResultCollection(entity.Relationships);
                }
            }
        }

        /// <summary>
        /// Refresh the operation result status based on the errors and warnings
        /// </summary>
        public override void RefreshOperationResultStatus()
        {
            base.RefreshOperationResultStatus();

            if (this.OperationResultStatus == OperationResultStatusEnum.Successful || 
                this.OperationResultStatus == OperationResultStatusEnum.None ||
                this.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
            {
                this.AttributeOperationResultCollection.RefreshOperationResultStatus();
                SetEnityOperationResultStatus(this.AttributeOperationResultCollection.OperationResultStatus);
            }

            if (this.OperationResultStatus == OperationResultStatusEnum.Successful || 
                this.OperationResultStatus == OperationResultStatusEnum.None ||
                this.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
            {
                this.RelationshipOperationResultCollection.RefreshOperationResultStatus();
                SetEnityOperationResultStatus(this.RelationshipOperationResultCollection.OperationResultStatus);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceAttributeOperationResults"></param>
        /// <param name="targetAttributeOerationResults"></param>
        private void CopyAttributeOperationResults(AttributeOperationResultCollection sourceAttributeOperationResults, AttributeOperationResultCollection targetAttributeOerationResults)
        {
            if (sourceAttributeOperationResults != null && targetAttributeOerationResults != null)
            {
                this.AttributeOperationResultCollection.OperationResultStatus = sourceAttributeOperationResults.OperationResultStatus;

                foreach (AttributeOperationResult attributeOperationResult in sourceAttributeOperationResults)
                {
                    AttributeOperationResult targetAttributeOperationResult = targetAttributeOerationResults.GetAttributeOperationResult(attributeOperationResult.AttributeId, attributeOperationResult.Locale);

                    // If attribute does not exist, add it.
                    if (targetAttributeOperationResult == null)
                    {
                        this.AttributeOperationResultCollection.Add(attributeOperationResult);
                    }
                    else // if exists, add source errors, warnings and informations and return values into target RelationshipAttributeOperationResult
                    {
                        #region Copy errors

                        if (attributeOperationResult.Errors.Count > 0)
                        {
                            targetAttributeOperationResult.Errors.AddRange(attributeOperationResult.Errors);
                        }

                        #endregion Copy errors

                        #region Copy warnings

                        if (attributeOperationResult.Warnings.Count > 0)
                        {
                            targetAttributeOperationResult.Warnings.AddRange(attributeOperationResult.Warnings);
                        }

                        #endregion Copy warnings

                        #region Copy informations

                        if (attributeOperationResult.Informations.Count > 0)
                        {
                            targetAttributeOperationResult.Informations.AddRange(attributeOperationResult.Informations);
                        }

                        #endregion Copy informations

                        #region Copy return values

                        if (attributeOperationResult.ReturnValues.Count > 0)
                        {
                            foreach (Object val in attributeOperationResult.ReturnValues)
                            {
                                targetAttributeOperationResult.ReturnValues.Add(val);
                            }
                        }

                        #endregion Copy return values
                    }

                    if (targetAttributeOperationResult != null)
                    {
                        targetAttributeOperationResult.RefreshOperationResultStatus();
                    }
                }

                targetAttributeOerationResults.RefreshOperationResultStatus();
            }
        }

        private void AddRelationshipErrors(RelationshipOperationResult relationshipOR, ErrorCollection errors)
        {
            if (relationshipOR.Errors != null && relationshipOR.Errors.Count > 0)
            {
                foreach (Error relationshipError in relationshipOR.Errors)
                {
                    errors.Add(relationshipError);
                }
            }
            if (relationshipOR.AttributeOperationResultCollection != null && relationshipOR.AttributeOperationResultCollection.Count > 0)
            {
                foreach (AttributeOperationResult attributeOR in relationshipOR.AttributeOperationResultCollection)
                {
                    if (attributeOR.Errors != null && attributeOR.Errors.Count > 0)
                    {
                        foreach (Error attributeError in attributeOR.Errors)
                        {
                            errors.Add(attributeError);
                        }
                    }
                }
            }
            var childRelationshipOperationResult = relationshipOR.GetChildRelationshipOperationResult(relationshipOR.Id);
            if (childRelationshipOperationResult != null)
            {
                AddRelationshipErrors(childRelationshipOperationResult, errors);
            }
            else
            {
                return;
            }
        }

        private void AddRelationshipWarnings(RelationshipOperationResult relationshipOR, WarningCollection warnings)
        {
            if (relationshipOR.Warnings != null && relationshipOR.Warnings.Count > 0)
            {
                foreach (Warning relationshipWarning in relationshipOR.Warnings)
                {
                    warnings.Add(relationshipWarning);
                }
            }
            if (relationshipOR.AttributeOperationResultCollection != null && relationshipOR.AttributeOperationResultCollection.Count > 0)
            {
                foreach (AttributeOperationResult attributeOR in relationshipOR.AttributeOperationResultCollection)
                {
                    if (attributeOR.Warnings != null && attributeOR.Warnings.Count > 0)
                    {
                        foreach (Warning attributeWarning in attributeOR.Warnings)
                        {
                            warnings.Add(attributeWarning);
                        }
                    }
                }
            }
            var childRelationshipOperationResult = relationshipOR.GetChildRelationshipOperationResult(relationshipOR.Id);
            if (childRelationshipOperationResult != null)
            {
                AddRelationshipWarnings(childRelationshipOperationResult, warnings);
            }
            else
            {
                return;
            }
        }

        private void AddRelationshipInformations(RelationshipOperationResult relationshipOR, InformationCollection informations)
        {
            if (relationshipOR.Informations != null && relationshipOR.Informations.Count > 0)
            {
                foreach (Information relationshipInformation in relationshipOR.Informations)
                {
                    informations.Add(relationshipInformation);
                }
            }
            if (relationshipOR.AttributeOperationResultCollection != null && relationshipOR.AttributeOperationResultCollection.Count > 0)
            {
                foreach (AttributeOperationResult attributeOR in relationshipOR.AttributeOperationResultCollection)
                {
                    if (attributeOR.Informations != null && attributeOR.Informations.Count > 0)
                    {
                        foreach (Information attributeInformation in attributeOR.Informations)
                        {
                            informations.Add(attributeInformation);
                        }
                    }
                }
            }
            var childRelationshipOperationResult = relationshipOR.GetChildRelationshipOperationResult(relationshipOR.Id);
            if (childRelationshipOperationResult != null)
            {
                AddRelationshipInformations(childRelationshipOperationResult, informations);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationResultStatus"></param>
        private void SetEnityOperationResultStatus(OperationResultStatusEnum operationResultStatus)
        {
            if (operationResultStatus == OperationResultStatusEnum.Failed || operationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
            {
                this.OperationResultStatus = OperationResultStatusEnum.Failed;
            }
            else if (this.AttributeOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
            {
                this.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
            }
        }

        #endregion

        #endregion
    }
}
