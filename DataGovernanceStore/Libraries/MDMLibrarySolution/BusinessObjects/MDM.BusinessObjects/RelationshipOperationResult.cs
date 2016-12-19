using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies results for Relationship operations
    /// </summary>
    [DataContract]
    [KnownType(typeof(AttributeOperationResultCollection))]
    public class RelationshipOperationResult : OperationResult, IRelationshipOperationResult
    {
        #region Fields

        /// <summary>
        /// Field denoting the id of the relationship for which results are created
        /// </summary>
        private Int64 _relationshipId = 0;

        /// <summary>
        /// Field denoting the relationship external Id
        /// </summary>
        private String _relationshipExternalId = String.Empty;

        /// <summary>
        /// Field denoting reference id of the relationship for which results are created
        /// </summary>
        private Int64 _referenceId = -1;

        /// <summary>
        /// Field denoting the external Id of the related entity
        /// </summary>
        private String _toExternalId = String.Empty;

        /// <summary>
        /// Field denoting the type of the relationship
        /// </summary>
        private String _relationshipTypeName = String.Empty;

        /// <summary>
        /// Field denoting the operation result collection for the attributes of the relationship in the context 
        /// </summary>
        private AttributeOperationResultCollection _attributeOperationResultCollection = new AttributeOperationResultCollection();

        /// <summary>
        /// Field denoting the operation result collection for the child relationships of the relationship in the context 
        /// </summary>
        private RelationshipOperationResultCollection _relationshipOperationResultCollection = new RelationshipOperationResultCollection();

        #endregion

        #region Properties

        /// <summary>
        /// Property defining the type of the Operation Result
        /// </summary>
        public new String ObjectType
        {
            get
            {
                return "RelationshipOperationResult";
            }
        }

        /// <summary>
        /// Property denoting the id of the relationship for which results are created
        /// </summary>
        [DataMember]
        public Int64 RelationshipId
        {
            get
            {
                return _relationshipId;
            }
            set
            {
                _relationshipId = value;
            }
        }

        /// <summary>
        /// Property denoting the relationship external id
        /// </summary>
        [DataMember]
        public String RelationshipExternalId
        {
            get
            {
                return _relationshipExternalId;
            }
            set
            {
                _relationshipExternalId = value;
            }
        }

        /// <summary>
        /// Property denoting the reference id of the relationship for which results are created
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
        /// Property denoting the external Id of the related entity
        /// </summary>
        [DataMember]
        public String ToExternalId
        {
            get
            {
                return _toExternalId;
            }
            set
            {
                _toExternalId = value;
            }
        }

        /// <summary>
        /// Property denoting the type of the relationship
        /// </summary>
        [DataMember]
        public String RelationshipTypeName
        {
            get
            {
                return _relationshipTypeName;
            }
            set
            {
                _relationshipTypeName = value;
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
        /// Property denoting the operation result collection for the relationships of the entity in the context 
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

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes new instance of entity operation result
        /// </summary>
        public RelationshipOperationResult()
        {

        }

        /// <summary>
        /// Initializes new instance of relationship operation result with the provided relationship id
        /// </summary>
        /// <param name="relationshipId">Id of the relationship</param>
        /// <param name="relationshipExternalId">Relationship's external Id</param>
        /// <param name="toExternalId">Related entity's external Id</param>
        /// <param name="relationshipTypeName">Relationship Type Name</param>
        public RelationshipOperationResult(Int64 relationshipId, String relationshipExternalId, String toExternalId, String relationshipTypeName)
            : this(relationshipId, relationshipExternalId, toExternalId, relationshipTypeName, -1)
        {
        }

        /// <summary>
        /// Initializes new instance of relationship operation result with the provided relationship id
        /// </summary>
        /// <param name="relationshipId">Id of the relationship</param>
        /// <param name="relationshipExternalId">Relationship's external Id</param>
        /// <param name="toExternalId">Related entity's external Id</param>
        /// <param name="relationshipTypeName">Relationship Type Name</param>
        /// <param name="referenceId">Indicates reference id of the relationship</param>
        public RelationshipOperationResult(Int64 relationshipId, String relationshipExternalId, String toExternalId, String relationshipTypeName,Int64 referenceId)
        {
            this.RelationshipId = relationshipId;
            this.RelationshipExternalId = relationshipExternalId;
            this.ToExternalId = toExternalId;
            this.RelationshipTypeName = relationshipTypeName;
            this.ReferenceId = referenceId;
        }

        /// <summary>
        /// Initializes new instance of relationship operation result for the provided relationship object
        /// </summary>
        /// <param name="relationship">Relationship Object</param>
        public RelationshipOperationResult(Relationship relationship)
        {
            this.RelationshipId = relationship.Id;
            this.RelationshipExternalId = relationship.RelationshipExternalId;
            this.ToExternalId = relationship.ToExternalId;
            this.RelationshipTypeName = relationship.RelationshipTypeName;
            this.ReferenceId = relationship.ReferenceId;
            
            if (relationship.RelationshipAttributes != null && relationship.RelationshipAttributes.Count > 0)
            {
                foreach (Attribute attr in relationship.RelationshipAttributes)
                {
                    AttributeOperationResult attributeOperationResult = new AttributeOperationResult(attr.Id, attr.Name, attr.LongName, attr.AttributeModelType, attr.Locale);
                    this.AttributeOperationResultCollection.Add(attributeOperationResult);
                }
            }

            if (relationship.RelationshipCollection != null && relationship.RelationshipCollection.Count > 0)
            {
                RelationshipOperationResultCollection relOprResultCollection = new RelationshipOperationResultCollection(relationship.RelationshipCollection);
                this._relationshipOperationResultCollection = relOprResultCollection;
            }
        }

        /// <summary>
        /// Initialize new instance of relationship operation result from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for relationship operation result</param>
        public RelationshipOperationResult(String valuesAsXml)
        {
            LoadOperationResult(valuesAsXml);
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
        /// <RelationshipOperationResult Id="1234" Name="P2198" Status="Failed">
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
        /// </RelationshipOperationResult>
        /// ]]>
        /// </para>
        /// </param>
        public new void LoadOperationResult(String valuesAsXml)
        {
            #region Sample Xml
            /*
             <RelationshipOperationResult Id="1234" Name="P2198" Status="Failed">
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
             </RelationshipOperationResult>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "RelationshipOperationResult")
                        {
                            #region Read EntityOperationResult attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.RelationshipId = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("RelationshipExternalId"))
                                {
                                    this.RelationshipExternalId = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ReferenceId"))
                                {
                                    this.ReferenceId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("ToExternalId"))
                                {
                                    this.ToExternalId = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("RelationshipTypeName"))
                                {
                                    this.RelationshipTypeName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Status"))
                                {
                                    OperationResultStatusEnum operationResultStatus = OperationResultStatusEnum.None;
                                    String strOperationResultStatus = reader.ReadContentAsString();

                                    if (!String.IsNullOrWhiteSpace(strOperationResultStatus))
                                        Enum.TryParse<OperationResultStatusEnum>(strOperationResultStatus, out operationResultStatus);

                                    this.OperationResultStatus = operationResultStatus;
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
                                Warning warn = new Warning(warningXml);

                                if (warn != null)
                                {
                                    if (this.Warnings == null)
                                        this.Warnings = new WarningCollection();

                                    this.Warnings.Add(warn);
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
        /// Get Xml representation of Relationship Operation Result
        /// </summary>
        /// <returns>Xml representation of Relationship Operation Result object</returns>
        public new String ToXml()
        {
            String relationshipOperationResultXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Operation result node start
            xmlWriter.WriteStartElement("RelationshipOperationResult");

            #region Write relationship operation result properties

            xmlWriter.WriteAttributeString("Id", this.RelationshipId.ToString());
            xmlWriter.WriteAttributeString("RelationshipExternalId", this.RelationshipExternalId);
            xmlWriter.WriteAttributeString("ReferenceId", this.ReferenceId.ToString());
            xmlWriter.WriteAttributeString("ToExternalId", this.ToExternalId);
            xmlWriter.WriteAttributeString("RelationshipTypeName", this.RelationshipTypeName);
            xmlWriter.WriteAttributeString("Status", this.OperationResultStatus.ToString());

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

            String relationshipOperationResultsXml = this.RelationshipOperationResultCollection.ToXml();
            xmlWriter.WriteRaw(relationshipOperationResultsXml);

            #endregion Write relationship operation results

            //Operation result node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            relationshipOperationResultXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return relationshipOperationResultXml;
        }

        /// <summary>
        /// Get Xml representation of Relationship operation result based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Relationship operation result</returns>
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
                xmlWriter.WriteStartElement("RelationshipOperationResult");

                #region Write relationshio operation result properties

                xmlWriter.WriteAttributeString("Id", this.RelationshipId.ToString());
                xmlWriter.WriteAttributeString("RelationshipExternalId", this.RelationshipExternalId);
                xmlWriter.WriteAttributeString("ReferenceId", this.ReferenceId.ToString());
                xmlWriter.WriteAttributeString("ToExternalId", this.ToExternalId);
                xmlWriter.WriteAttributeString("RelationshipTypeName", this.RelationshipTypeName);
                xmlWriter.WriteAttributeString("Status", this.OperationResultStatus.ToString());

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

                String relationshipOperationResultsXml = this.RelationshipOperationResultCollection.ToXml(objectSerialization);
                xmlWriter.WriteRaw(relationshipOperationResultsXml);

                #endregion Write relationship operation results

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

            addSuccess = this._attributeOperationResultCollection.AddAttributeOperationResult(attributeId, resultCode, resultMessage, operationResultType);

            //If the Attribute Collection overall operation result is Failed or Completed With Errors, the entity operation result status should be Failed
            if (this._attributeOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.Failed || this._attributeOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
                this.OperationResultStatus = OperationResultStatusEnum.Failed;
            else if (this._attributeOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
                this.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;         

            return addSuccess;
        }

        /// <summary>
        /// Adds attribute operation result
        /// </summary>
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
        public Boolean AddAttributeOperationResult(String attributeName, String resultCode, String resultMessage, Collection<Object> parameters, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType, Boolean ignoreError = false)
        {
            Boolean addSuccess = false;

            addSuccess = this._attributeOperationResultCollection.AddAttributeOperationResult(attributeName, resultCode, parameters, reasonType, ruleMapContextId, ruleId, operationResultType, ignoreError);

            //If the Attribute Collection overall operation result is Failed or Completed With Errors, the entity operation result status should be Failed
            if (this._attributeOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.Failed || this._attributeOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
            {
                this.OperationResultStatus = OperationResultStatusEnum.Failed;
            }
            else if (this._attributeOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
            {
                this.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
            }

            return addSuccess;
        }

        /// <summary>
        /// Adds operation result for child relationships
        /// </summary>
        /// <param name="relationshipId">Id of the relationships</param>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        public Boolean AddChildRelationshipOperationResult(Int32 relationshipId, String resultCode, String resultMessage, OperationResultType operationResultType)
        {
            Boolean addSuccess = false;

            RelationshipOperationResult childRelationshipOperationResult = GetChildRelationshipOperationResult(relationshipId);

            if (childRelationshipOperationResult != null)
            {
                addSuccess = childRelationshipOperationResult.AddOperationResult(resultCode, resultMessage, operationResultType);

                //Currently the logic is not in place to set the status at each child relationship level..
                //So, if add is success, status will be set to failed for the relationship operation result collection and hence for the current relationship operation result also
                if (addSuccess && operationResultType == OperationResultType.Error)
                {
                    this.RelationshipOperationResultCollection.OperationResultStatus = OperationResultStatusEnum.Failed;
                    this.OperationResultStatus = OperationResultStatusEnum.Failed;
                }
                else if (addSuccess && operationResultType == OperationResultType.Warning)
                {
                    this.RelationshipOperationResultCollection.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
                    this.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
                }
            }

            return addSuccess;
        }

        /// <summary>
        /// Adds operation result for child relationships
        /// </summary>
        /// <param name="relationshipId">Id of the relationships</param>
        /// <param name="attributeId">Id of the attribute</param>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        public Boolean AddChildRelationshipAttributeOperationResult(Int32 relationshipId, Int32 attributeId, String resultCode, String resultMessage, OperationResultType operationResultType)
        {
            Boolean addSuccess = false;

            RelationshipOperationResult childRelationshipOperationResult = GetChildRelationshipOperationResult(relationshipId);

            if (childRelationshipOperationResult != null)
            {
                addSuccess = childRelationshipOperationResult.AddAttributeOperationResult(attributeId, resultCode, resultMessage, operationResultType);

                //Currently the logic is not in place to set the status at each child relationship level..
                //So, if add is success, status will be set to failed for the relationship operation result collection and hence for the current relationship operation result also
                if (addSuccess && operationResultType == OperationResultType.Error)
                {
                    this.RelationshipOperationResultCollection.OperationResultStatus = OperationResultStatusEnum.Failed;
                    this.OperationResultStatus = OperationResultStatusEnum.Failed;
                }
                else if (addSuccess && operationResultType == OperationResultType.Warning)
                {
                    this.RelationshipOperationResultCollection.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
                    this.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
                }
            }

            return addSuccess;
        }

        /// <summary>
        /// Adds operation result for child relationships
        /// </summary>
        /// <param name="relationshipTypeName">Name of the relationship type</param>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="parameters">Message format parameters</param>
        /// <param name="reasonType">Reason type</param>
        /// <param name="ruleMapContextId">Rule map context id</param>
        /// <param name="ruleId">Rule id</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <param name="ignoreError">Indicates whether to show the errors on UI or not. Default value is false</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        public Boolean AddChildRelationshipOperationResult(String relationshipTypeName, String resultCode, String resultMessage, Collection<Object> parameters, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType, Boolean ignoreError = false)
        {
            Boolean addSuccess = false;

            RelationshipOperationResult childRelationshipOperationResult = GetChildRelationshipOperationResult(relationshipTypeName);

            if (childRelationshipOperationResult != null)
            {
                addSuccess = childRelationshipOperationResult.AddOperationResult(resultCode, resultMessage, parameters, reasonType, ruleMapContextId, ruleId, operationResultType, ignoreError);

                //Currently the logic is not in place to set the status at each child relationship level..
                //So, if add is success, status will be set to failed for the relationship operation result collection and hence for the current relationship operation result also
                if (addSuccess && operationResultType == OperationResultType.Error)
                {
                    this.RelationshipOperationResultCollection.OperationResultStatus = OperationResultStatusEnum.Failed;
                    this.OperationResultStatus = OperationResultStatusEnum.Failed;
                }
                else if (addSuccess && operationResultType == OperationResultType.Warning)
                {
                    this.RelationshipOperationResultCollection.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
                    this.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
                }
            }

            return addSuccess;
        }

        /// <summary>
        /// Adds operation result for child relationships
        /// </summary>
        /// <param name="relationshipTypeName">Name of the relationship type</param>
        /// <param name="attributeName">Name of the attribute</param>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="parameters">Message format parameters</param>
        /// <param name="reasonType">Reason type</param>
        /// <param name="ruleMapContextId">Rule map context id</param>
        /// <param name="ruleId">Rule id</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <param name="ignoreError">Indicates whether to show the errors on UI or not. Default value is false</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        public Boolean AddChildRelationshipAttributeOperationResult(String relationshipTypeName, String attributeName, String resultCode, String resultMessage, Collection<Object> parameters, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType, Boolean ignoreError = false)
        {
            Boolean addSuccess = false;

            RelationshipOperationResult childRelationshipOperationResult = GetChildRelationshipOperationResult(relationshipTypeName);

            if (childRelationshipOperationResult != null)
            {
                addSuccess = childRelationshipOperationResult.AddAttributeOperationResult(attributeName, resultCode, resultMessage, parameters, reasonType, ruleMapContextId, ruleId, operationResultType, ignoreError);

                //Currently the logic is not in place to set the status at each child relationship level..
                //So, if add is success, status will be set to failed for the relationship operation result collection and hence for the current relationship operation result also
                if (addSuccess && operationResultType == OperationResultType.Error)
                {
                    this.RelationshipOperationResultCollection.OperationResultStatus = OperationResultStatusEnum.Failed;
                    this.OperationResultStatus = OperationResultStatusEnum.Failed;
                }
                else if (addSuccess && operationResultType == OperationResultType.Warning)
                {
                    this.RelationshipOperationResultCollection.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
                    this.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
                }
            }

            return addSuccess;
        }

        /// <summary>
        /// Gets child relationship operation result present at any level
        /// </summary>
        /// <param name="relationshipId">Id of the relationship</param>
        /// <returns>Child relationship operation result</returns>
        public RelationshipOperationResult GetChildRelationshipOperationResult(Int32 relationshipId)
        {
            RelationshipOperationResult relOprResult = null;

            if (this.RelationshipOperationResultCollection != null && this.RelationshipOperationResultCollection.Count > 0)
            {
                try
                {
                    relOprResult = this.RelationshipOperationResultCollection.SingleOrDefault(r => r.RelationshipId == relationshipId);
                }
                catch
                {
                    throw new Exception("Duplicate operation results found for the requested relationship Id.");
                }

                if (relOprResult == null)
                {
                    foreach (RelationshipOperationResult rOprRes in this.RelationshipOperationResultCollection)
                    {
                        relOprResult = rOprRes.GetChildRelationshipOperationResult(relationshipId);

                        if (relOprResult != null)
                            break;
                    }
                }
            }

            return relOprResult;
        }

        /// <summary>
        /// Gets child relationship operation result present at any level
        /// </summary>
        /// <param name="relationshipTypeName">Name of the relationship type</param>
        /// <returns>Child relationship operation result</returns>
        public RelationshipOperationResult GetChildRelationshipOperationResult(String relationshipTypeName)
        {
            RelationshipOperationResult relOprResult = null;

            if (this.RelationshipOperationResultCollection != null && this.RelationshipOperationResultCollection.Count > 0)
            {
                try
                {
                    relOprResult = this.RelationshipOperationResultCollection.SingleOrDefault(r => r.RelationshipTypeName == relationshipTypeName);
                }
                catch
                {
                    throw new Exception("Duplicate operation results found for the requested relationship type name.");
                }

                if (relOprResult == null)
                {
                    foreach (RelationshipOperationResult rOprRes in this.RelationshipOperationResultCollection)
                    {
                        relOprResult = rOprRes.GetChildRelationshipOperationResult(relationshipTypeName);

                        if (relOprResult != null)
                        {
                            break;
                        }
                    }
                }
            }

            return relOprResult;
        }

        /// <summary>
        /// Set attribute operation result collection
        /// </summary>
        /// <param name="attributeOperationResultCollection"></param>
        public void SetAttributeOperationResult(AttributeOperationResultCollection attributeOperationResultCollection)
        {
            attributeOperationResultCollection.RefreshOperationResultStatus();

            this._attributeOperationResultCollection = attributeOperationResultCollection;

            if (attributeOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.Failed
                           || attributeOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
            {
                this.OperationResultStatus = OperationResultStatusEnum.CompletedWithErrors;
            }
            else if (attributeOperationResultCollection.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
                this.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
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
        /// Checks if current RelationshipOperationResult is superset of passed RelationshipOperationResult
        /// </summary>
        /// <param name="subsetRelationshipOperationResult">Indicates RelationshipOperationResult which should be compared</param>
        /// <param name="compareIds">Indicates Ids should have to be compared or not</param>
        /// <returns>True or False based on comparison</returns>
        public Boolean IsSuperSetOf(RelationshipOperationResult subsetRelationshipOperationResult, Boolean compareIds = false)
        {
            if (compareIds)
            {
                 if (this.Id != subsetRelationshipOperationResult.Id)
                    return false;

                 if (this.RelationshipId != subsetRelationshipOperationResult.RelationshipId)
                    return false;

                 if (this.ReferenceId != subsetRelationshipOperationResult.ReferenceId)
                     return false;
            }

            if (this.RelationshipExternalId != subsetRelationshipOperationResult.RelationshipExternalId)
                return false;

            if (this.ToExternalId != subsetRelationshipOperationResult.ToExternalId)
                return false;

            if (this.RelationshipTypeName != subsetRelationshipOperationResult.RelationshipTypeName)
                return false;

            if (!this.AttributeOperationResultCollection.IsSuperSetOf(subsetRelationshipOperationResult.AttributeOperationResultCollection, compareIds))
                return false;

            if (!this.RelationshipOperationResultCollection.IsSuperSetOf(subsetRelationshipOperationResult.RelationshipOperationResultCollection, compareIds))
                return false;

            return true;
        }

        /// <summary>
        /// Get collection of AttributeOperation result. Which contains errors and warnings
        /// </summary>
        /// <returns>AttributeOperationResultCollection</returns>
        public IAttributeOperationResultCollection GetAttributeOperationResults()
        {
            IAttributeOperationResultCollection iAttributeOperationResultCollection = null;

            if (this.AttributeOperationResultCollection != null)
            {
                iAttributeOperationResultCollection = (IAttributeOperationResultCollection)this.AttributeOperationResultCollection;
            }
            return iAttributeOperationResultCollection;
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}
