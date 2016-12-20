using System;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies Relationship Operation Result Collection
    /// </summary>
    [DataContract]
    public class RelationshipOperationResultCollection : ICollection<RelationshipOperationResult>, IEnumerable<RelationshipOperationResult>, IRelationshipOperationResultCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting generic errors for operation
        /// </summary>
        private ErrorCollection _errors = null;

        /// <summary>
        /// Field denoting generic information about operation
        /// </summary>
        private InformationCollection _informations = null;

        /// <summary>
        /// Field denoting generic warnings about operation
        /// </summary>
        private WarningCollection _warnings = null;

        /// <summary>
        /// Indicates status of Operation.
        /// </summary>
        private OperationResultStatusEnum _operationResultStatus = OperationResultStatusEnum.None;

        /// <summary>
        /// Field denoting collection of Relationship Operation Result
        /// </summary>
        [DataMember]
        private Collection<RelationshipOperationResult> _relationshipOperationResultCollection = new Collection<RelationshipOperationResult>();

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting generic errors for operation
        /// </summary>
        [DataMember]
        public ErrorCollection Errors
        {
            get
            {
                return _errors;
            }
            set
            {
                _errors = value;
            }
        }

        /// <summary>
        /// Property denoting generic information about operation
        /// </summary>
        [DataMember]
        public InformationCollection Informations
        {
            get
            {
                return _informations;
            }
            set
            {
                _informations = value;
            }
        }

        /// <summary>
        /// Property denoting generic warnings about operation
        /// </summary>
        [DataMember]
        public WarningCollection Warnings
        {
            get
            {
                return _warnings;
            }
            set
            {
                _warnings = value;
            }
        }

        /// <summary>
        /// Indicates overall status of Operation result
        /// </summary>
        [DataMember]
        public OperationResultStatusEnum OperationResultStatus
        {
            get
            {
                return _operationResultStatus;
            }
            set
            {
                _operationResultStatus = value;
            }
        }

        /// <summary>
        /// Find relationship operation result from RelationshipOperationResultCollection based on relationshipId
        /// </summary>
        /// <param name="relationshipId">relationshipId to search in entity Operation Result Collection</param>
        /// <returns>RelationshipOperationResult object having given relationshipId</returns>
        public RelationshipOperationResult this[Int64 relationshipId]
        {
            get
            {
                RelationshipOperationResult relationshipOperationResult = GetRelationshipOperationResult(relationshipId);
                if (relationshipOperationResult == null)
                    throw new ArgumentException(String.Format("No relationship operation result found for entity id: {0}", relationshipId), "relationshipId");

                return relationshipOperationResult;
            }
            set
            {
                RelationshipOperationResult relationshipOperationResult = GetRelationshipOperationResult(relationshipId);
                if (relationshipOperationResult == null)
                    throw new ArgumentException(String.Format("No relationship operation result found for entity id: {0}", relationshipId), "relationshipId");

                relationshipOperationResult = value;
            }
        }

        /// <summary>
        /// Property denotes whether there are any error messages
        /// </summary>
        public Boolean HasError
        {
            get
            {
                return (this.Errors != null && this.Errors.Count > 0) ? true : false;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the relationship operation result collection class
        /// </summary>
        public RelationshipOperationResultCollection()
        {

        }

        /// <summary>
        /// Initializes a new instance of the relationship operation result collection for the provided relationship collection
        /// </summary>
        /// <param name="relationshipCollection">Relationships as per which relationship operation result collection is going to be initialized</param>
        public RelationshipOperationResultCollection(RelationshipCollection relationshipCollection)
        {
            PrepareRelationshipOperationResultsSchema(relationshipCollection);
        }

        /// <summary>
        /// Initializes a new instance of the relationship operation result collection class with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        public RelationshipOperationResultCollection(String valuesAsXml)
        {
            /*
             * Sample XML:
              <RelationshipOperationResults Status="Failed">
              <Errors />
              <Informations />
              <RelationshipOperationResult Id="1234" >
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
                <AttributeOperationResults>
                    <AttributeOperationResult Id="9812">
                        <Errors>
                            <Error Code="" Message = "Required Attributes are not filled"/>
                        </Errors>
                    </AttributeOperationResult>
                </AttributeOpeartionResults>
             </RelationshipOperationResult>
             </RelationshipOperationResults>
             */
            XmlTextReader reader = null;

            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "RelationshipOperationResults")
                    {
                        if (reader.MoveToAttribute("Status"))
                        {
                            OperationResultStatusEnum operationResultStatus = OperationResultStatusEnum.None;
                            String strOperationResultStatus = reader.ReadContentAsString();

                            if (!String.IsNullOrWhiteSpace(strOperationResultStatus))
                                Enum.TryParse<OperationResultStatusEnum>(strOperationResultStatus, out operationResultStatus);

                            this.OperationResultStatus = operationResultStatus;
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Error")
                    {
                        String errorXml = reader.ReadOuterXml();

                        if (!String.IsNullOrEmpty(errorXml))
                        {
                            Error error = new Error(errorXml);

                            if (error != null)
                            {
                                if (this.Errors == null)
                                    this.Errors = new ErrorCollection();

                                this.Errors.Add(error);
                            }
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Information")
                    {
                        String infoXml = reader.ReadOuterXml();

                        if (!String.IsNullOrEmpty(infoXml))
                        {
                            Information info = new Information(infoXml);

                            if (info != null)
                            {
                                if (this.Informations == null)
                                    this.Informations = new InformationCollection();

                                this.Informations.Add(info);
                            }
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Warning")
                    {
                        String warningXml = reader.ReadOuterXml();

                        if (!String.IsNullOrEmpty(warningXml))
                        {
                            Warning warning = new Warning(warningXml);

                            if (warning != null)
                            {
                                if (this.Warnings == null)
                                    this.Warnings = new WarningCollection();

                                this.Warnings.Add(warning);
                            }
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "RelationshipOperationResult")
                    {
                        String relationshipOperationResultsXML = reader.ReadOuterXml();

                        if (!String.IsNullOrWhiteSpace(relationshipOperationResultsXML))
                        {
                            RelationshipOperationResult relationshipOperationResult = new RelationshipOperationResult(relationshipOperationResultsXML);

                            if (relationshipOperationResult != null)
                                this._relationshipOperationResultCollection.Add(relationshipOperationResult);
                        }
                    }
                    else
                    {
                        reader.Read();
                    }
                }

                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipOperationResultCollection"></param>
        /// <returns></returns>
        public void AddRange(RelationshipOperationResultCollection relationshipOperationResultCollection)
        {
            foreach (RelationshipOperationResult relationshipOperationResult in relationshipOperationResultCollection)
                this.Add(relationshipOperationResult);
        }

        /// <summary>
        /// Check if RelationshipOperationResultCollection contains relationship operation result with given entity id
        /// </summary>
        /// <param name="relationshipId">Id of the entity using which relationship operation result is to be searched from collection</param>
        /// <returns>
        /// <para>true : If relationship operation result found in RelationshipOperationResultCollection</para>
        /// <para>false : If relationship operation result found not in RelationshipOperationResultCollection</para>
        /// </returns>
        public Boolean Contains(Int64 relationshipId)
        {
            if (GetRelationshipOperationResult(relationshipId) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check if RelationshipOperationResultCollection contains relationship operation result 
        /// </summary>
        /// <param name="relationshipExternalId">Specifies externalId of the entity</param>
        /// <param name="toExternalId">Specifies ToEntity externalId</param>
        /// <param name="relationshipTypeName">Specifies RelationshipType</param>
        /// <returns>
        /// <para>true : If relationship operation result found in RelationshipOperationResultCollection</para>
        /// <para>false : If relationship operation result found not in RelationshipOperationResultCollection</para>
        /// </returns>
        public Boolean Contains(String relationshipExternalId, String toExternalId, String relationshipTypeName)
        {
            if (GetRelationshipOperationResult(relationshipExternalId, toExternalId, relationshipTypeName) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove relationshipOperationResult object from RelationshipOperationResultCollection
        /// </summary>
        /// <param name="relationshipId">Id of the entity using which Relationship Operation Result is to be removed from collection</param>
        /// <returns>true if relationship operation result is successfully removed; otherwise, false. This method also returns false if relationship operation result was not found in the original collection</returns>
        public Boolean Remove(Int32 relationshipId)
        {
            //Commenting remove

            //There should not be any way to remove relationship operation result
            //If any business rule removes any of the entityOR from the Operation Result schema, operation gives improper results

            //RelationshipOperationResult relationshipOperationResult = GetEntityOperationResult(relationshipId);

            //if (relationshipOperationResult == null)
            //    throw new ArgumentException("No relationship operation result found for given entity id");
            //else
            //    return this.Remove(relationshipOperationResult);

            return false;
        }

        /// <summary>
        /// Get Xml representation of relationship operation result collection
        /// </summary>
        /// <returns>Xml representation of relationship operation result collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Operation result node start
            xmlWriter.WriteStartElement("RelationshipOperationResults");

            #region Write relationship operation result properties

            xmlWriter.WriteAttributeString("Status", this.OperationResultStatus.ToString());

            #endregion

            #region Write Errors

            //Add Error Nodes
            xmlWriter.WriteStartElement("Errors");

            if (this.Errors != null)
            {
                foreach (Error error in this.Errors)
                    xmlWriter.WriteRaw(error.ToXml());
            }

            //Error nodes end
            xmlWriter.WriteEndElement();

            #endregion Write Errors

            #region Write Information

            //Add Information Nodes
            xmlWriter.WriteStartElement("Informations");

            if (this.Informations != null)
            {
                foreach (Information information in this.Informations)
                    xmlWriter.WriteRaw(information.ToXml());
            }

            //Information node end
            xmlWriter.WriteEndElement();

            #endregion Write Information

            #region Write Warning

            //Add warning Nodes
            xmlWriter.WriteStartElement("Warnings");

            if (this.Warnings != null)
            {
                foreach (Warning warning in this.Warnings)
                    xmlWriter.WriteRaw(warning.ToXml());
            }

            //warning node end
            xmlWriter.WriteEndElement();

            #endregion Write warning

            #region Write EntityOperationResults

            if (this._relationshipOperationResultCollection != null)
            {
                foreach (RelationshipOperationResult eor in this._relationshipOperationResultCollection)
                {
                    xmlWriter.WriteRaw(eor.ToXml());
                }
            }

            #endregion

            //Operation result node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            returnXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of relationship operation result collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of relationship operation result collection</returns>
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

                //Operation result node start
                xmlWriter.WriteStartElement("RelationshipOperationResults");

                #region Write relationship operation result properties

                xmlWriter.WriteAttributeString("Status", this.OperationResultStatus.ToString());

                #endregion

                #region Write Errors

                //Add Error Nodes
                xmlWriter.WriteStartElement("Errors");

                if (this.Errors != null)
                {
                    foreach (Error error in this.Errors)
                        xmlWriter.WriteRaw(error.ToXml(objectSerialization));
                }

                //Error nodes end
                xmlWriter.WriteEndElement();

                #endregion Write Errors

                #region Write Information

                //Add Information Nodes
                xmlWriter.WriteStartElement("Informations");

                if (this.Informations != null)
                {
                    foreach (Information information in this.Informations)
                        xmlWriter.WriteRaw(information.ToXml(objectSerialization));
                }

                //Information node end
                xmlWriter.WriteEndElement();

                #endregion Write Information

                #region Write Warning

                //Add warning Nodes
                xmlWriter.WriteStartElement("Warnings");

                if (this.Warnings != null)
                {
                    foreach (Warning warning in this.Warnings)
                        xmlWriter.WriteRaw(warning.ToXml(objectSerialization));
                }

                //warning node end
                xmlWriter.WriteEndElement();

                #endregion Write warning

                #region Write EntityOperationResults

                foreach (RelationshipOperationResult eor in this._relationshipOperationResultCollection)
                {
                    //If object serialization says external and the operation result status is success do not add the ROR.. Add only failed ones..
                    if (objectSerialization == ObjectSerialization.External && eor.OperationResultStatus == OperationResultStatusEnum.Successful)
                        continue;

                    xmlWriter.WriteRaw(eor.ToXml(objectSerialization));
                }

                #endregion

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

        #region AddOperationResult Overload Methods

        #region Without relationshipId parameter

        /// <summary>
        /// Adds operation result
        /// </summary>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        public Boolean AddOperationResult(String resultCode, String resultMessage, OperationResultType operationResultType)
        {
            return AddOperationResult(resultCode, null, resultMessage, ReasonType.NotSpecified, -1, -1, operationResultType);
        }

        /// <summary>
        /// Adds operation result
        /// </summary>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="parameters">Specifies collection of parameters</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        public Boolean AddOperationResult(String resultCode, Collection<Object> parameters, String resultMessage, OperationResultType operationResultType)
        {
            return AddOperationResult(resultCode, parameters, resultMessage, ReasonType.NotSpecified, -1, -1, operationResultType);
        }

        /// <summary>
        /// Adds operation result
        /// </summary>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="parameters">Specifies collection of parameters</param>
        /// <param name="reasonType">Indicates type of the reason.</param>
        /// <param name="ruleMapContextId">Indicates rule map context identifier.</param>
        /// <param name="ruleId">Indicates rule identifier.</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        public Boolean AddOperationResult(String resultCode, Collection<Object> parameters, String resultMessage, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType)
        {
            Boolean addSuccess = false;

            if (operationResultType == OperationResultType.Error)
            {
                Error error = new Error(resultCode, resultMessage, parameters, reasonType, ruleMapContextId, ruleId);

                if (this.Errors == null)
                    this.Errors = new ErrorCollection();

                this.Errors.Add(error);

                //Update operation result status
                this.OperationResultStatus = OperationResultStatusEnum.Failed;

                addSuccess = true;
            }
            else if (operationResultType == OperationResultType.Information)
            {
                Information information = new Information(resultCode, resultMessage, parameters, reasonType, ruleMapContextId, ruleId);

                if (this.Informations == null)
                    this.Informations = new InformationCollection();

                this.Informations.Add(information);
                addSuccess = true;
            }
            else if (operationResultType == OperationResultType.Warning)
            {
                Warning warning = new Warning(resultCode, resultMessage, parameters, reasonType, ruleMapContextId, ruleId);

                if (this.Warnings == null)
                    this.Warnings = new WarningCollection();

                this.Warnings.Add(warning);

                this.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;

                addSuccess = true;
            }

            return addSuccess;
        }

        #endregion Without relationshipId parameter

        #region With relationshipId parameter

        /// <summary>
        /// Adds relationship operation result
        /// </summary>
        /// <param name="relationshipId">Id of the entity for which operation result needs to be added</param>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        public Boolean AddRelationshipOperationResult(Int32 relationshipId, String resultCode, String resultMessage, OperationResultType operationResultType)
        {
            Boolean addSuccess = false;

            if (this._relationshipOperationResultCollection != null && this._relationshipOperationResultCollection.Count > 0)
            {
                //Get the requested relationship operation result
                RelationshipOperationResult relationshipOperationResult = GetRelationshipOperationResult(relationshipId);

                if (relationshipOperationResult != null)
                {
                    //Add operation result
                    addSuccess = relationshipOperationResult.AddOperationResult(resultCode, resultMessage, operationResultType);
                }
                else
                {
                    //Requesting relationship Id does not exist in this relationship operation result collection..
                    //Check whether child collection are having.. if found, add the operation result
                    foreach (RelationshipOperationResult relOprRes in this._relationshipOperationResultCollection)
                    {
                        addSuccess = relOprRes.AddChildRelationshipOperationResult(relationshipId, resultCode, resultMessage, operationResultType);

                        if (addSuccess)
                            break;
                    }
                }

                if (addSuccess)
                    RefreshOperationResultStatus();
            }

            return addSuccess;
        }

        /// <summary>
        /// Adds relationship operation result
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
        public Boolean AddRelationshipOperationResult(String relationshipTypeName, String resultCode, String resultMessage, Collection<Object> parameters, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType, Boolean ignoreError = false)
        {
            Boolean addSuccess = false;

            if (this._relationshipOperationResultCollection != null && this._relationshipOperationResultCollection.Count > 0)
            {
                //Get the requested relationship operation result
                RelationshipOperationResult relationshipOperationResult = GetRelationshipOperationResult(relationshipTypeName);

                if (relationshipOperationResult != null)
                {
                    //Add operation result
                    addSuccess = relationshipOperationResult.AddOperationResult(resultCode, resultMessage, parameters, reasonType, ruleMapContextId, ruleId, operationResultType, ignoreError);
                }
                else
                {
                    foreach (RelationshipOperationResult relOprRes in this._relationshipOperationResultCollection)
                    {
                        addSuccess = relOprRes.AddChildRelationshipOperationResult(relationshipTypeName, resultCode, resultMessage, parameters, reasonType, ruleMapContextId, ruleId, operationResultType, ignoreError);

                        if (addSuccess)
                        {
                            break;
                        }
                    }
                }

                if (addSuccess)
                {
                    RefreshOperationResultStatus();
                }
            }

            return addSuccess;
        }

        /// <summary>
        /// Adds entity attribute operation result
        /// </summary>
        /// <param name="relationshipId">Id of the entity for which operation result needs to be added</param>
        /// <param name="attributeId">Id of the entity attribute for which operation result needs to be added</param>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        public Boolean AddAttributeOperationResult(Int32 relationshipId, Int32 attributeId, String resultCode, String resultMessage, OperationResultType operationResultType)
        {
            Boolean addSuccess = false;

            if (this._relationshipOperationResultCollection != null && this._relationshipOperationResultCollection.Count > 0)
            {
                //Get the requested relationship operation result
                RelationshipOperationResult relationshipOperationResult = GetRelationshipOperationResult(relationshipId);

                if (relationshipOperationResult != null)
                {
                    //Add operation result
                    addSuccess = relationshipOperationResult.AddAttributeOperationResult(attributeId, resultCode, resultMessage, operationResultType);
                }
                else
                {
                    //Requesting relationship Id does not exist in this relationship operation result collection..
                    //Check whether child collection are having.. if found, add the operation result
                    foreach (RelationshipOperationResult relOprRes in this._relationshipOperationResultCollection)
                    {
                        addSuccess = relOprRes.AddChildRelationshipAttributeOperationResult(relationshipId, attributeId, resultCode, resultMessage, operationResultType);

                        if (addSuccess)
                            break;
                    }
                }

                if (addSuccess)
                    RefreshOperationResultStatus();
            }

            return addSuccess;
        }

        /// <summary>
        /// Adds entity attribute operation result
        /// </summary>
        /// <param name="relationshipTypeName">Name of the relationship type for which operation result needs to be added</param>
        /// <param name="attributeName">Name of the entity attribute for which operation result needs to be added</param>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="parameters">Message format parameters</param>
        /// <param name="reasonType">Reason type</param>
        /// <param name="ruleMapContextId">Rule map context id</param>
        /// <param name="ruleId">Rule id</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <param name="ignoreError">Indicates whether to show the errors on UI or not. Default value is false</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        public Boolean AddAttributeOperationResult(String relationshipTypeName, String attributeName, String resultCode, String resultMessage, Collection<Object> parameters, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType, Boolean ignoreError = false)
        {
            Boolean addSuccess = false;

            if (this._relationshipOperationResultCollection != null && this._relationshipOperationResultCollection.Count > 0)
            {
                //Get the requested relationship operation result
                RelationshipOperationResult relationshipOperationResult = GetRelationshipOperationResult(relationshipTypeName);

                if (relationshipOperationResult != null)
                {
                    //Add operation result
                    addSuccess = relationshipOperationResult.AddAttributeOperationResult(attributeName, resultCode, resultMessage, parameters, reasonType, ruleMapContextId, ruleId, operationResultType, ignoreError);
                }
                else
                {
                    foreach (RelationshipOperationResult relOprRes in this._relationshipOperationResultCollection)
                    {
                        addSuccess = relOprRes.AddChildRelationshipAttributeOperationResult(relationshipTypeName, attributeName, resultCode, resultMessage, parameters, reasonType, ruleMapContextId, ruleId, operationResultType, ignoreError);

                        if (addSuccess)
                        {
                            break;
                        }
                    }
                }

                if (addSuccess)
                {
                    RefreshOperationResultStatus();
                }
            }

            return addSuccess;
        }

        #endregion With relationshipId parameter

        #endregion AddOperationResult Overload Methods

        /// <summary>
        /// Calculates the status and updates with new status
        /// </summary>
        public void RefreshOperationResultStatus()
        {
            //Get failed entity operation results
            IEnumerable<RelationshipOperationResult> failedEORs = this._relationshipOperationResultCollection.Where(eor => eor.OperationResultStatus == OperationResultStatusEnum.Failed ||
                eor.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors);

            //Get succeeded entity operation results
            IEnumerable<RelationshipOperationResult> succeededEORs = this._relationshipOperationResultCollection.Where(eor => eor.OperationResultStatus == OperationResultStatusEnum.None ||
                eor.OperationResultStatus == OperationResultStatusEnum.Successful);

            //Get warned entity operation results
            IEnumerable<RelationshipOperationResult> warnedEORs = this._relationshipOperationResultCollection.Where(eor => eor.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings);

            if ((failedEORs != null && failedEORs.Count() > 0) || (this._errors != null && this._errors.Count > 0))
            {
                //if all are failed the overall status will be Failed else completedWithErrors.
                if (failedEORs.Count() == this.Count)
                    this.OperationResultStatus = OperationResultStatusEnum.Failed;
                else
                    this.OperationResultStatus = OperationResultStatusEnum.CompletedWithErrors;
            }
            else if ((warnedEORs != null && warnedEORs.Count() > 0) || (this._warnings != null && this._warnings.Count > 0))
            {
                //if some are get warned the overall status will be CompletedWithWarnings
                this.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
            }
            else if ((succeededEORs != null && succeededEORs.Count() > 0) || (this._informations != null && this._informations.Count > 0))
            {
                //if there are no errors and warnings the overall status will be successful
                this.OperationResultStatus = OperationResultStatusEnum.Successful;
            }
            else
                this.OperationResultStatus = OperationResultStatusEnum.None;
        }

        /// <summary>
        /// Gets relationship operation result
        /// </summary>
        /// <param name="relationshipId">Id of the relationship</param>
        /// <returns>Relationship Operation Result object</returns>
        public RelationshipOperationResult GetRelationshipOperationResult(Int64 relationshipId)
        {
            RelationshipOperationResult resultedROR = null;

            if (this._relationshipOperationResultCollection != null && this._relationshipOperationResultCollection.Count > 0)
            {
                foreach (RelationshipOperationResult ror in this._relationshipOperationResultCollection)
                {
                    if (ror.RelationshipId == relationshipId)
                    {
                        resultedROR = ror;
                        break;
                    }

                    resultedROR = ror.RelationshipOperationResultCollection.GetRelationshipOperationResult(relationshipId);

                    if (resultedROR != null)
                    {
                        break;
                    }
                }
            }

            return resultedROR;
        }

        /// <summary>
        /// Gets relationship operation result
        /// </summary>
        /// <param name="relationshipTypeName">Name of the relationship type</param>
        /// <returns>Relationship Operation Result object</returns>
        public RelationshipOperationResult GetRelationshipOperationResult(String relationshipTypeName)
        {
            RelationshipOperationResult resultedROR = null;

            if (this._relationshipOperationResultCollection != null && this._relationshipOperationResultCollection.Count > 0)
            {
                foreach (RelationshipOperationResult ror in this._relationshipOperationResultCollection)
                {
                    if (String.Compare(ror.RelationshipTypeName, relationshipTypeName, true) == 0)
                    {
                        resultedROR = ror;
                        break;
                    }

                    resultedROR = ror.RelationshipOperationResultCollection.GetRelationshipOperationResult(relationshipTypeName);

                    if (resultedROR != null)
                    {
                        break;
                    }
                }
            }

            return resultedROR;
        }

        /// <summary>
        /// Refresh and update missing relationship operation results for the given entities
        /// </summary>
        /// <param name="relationships">Relationships for which relationship operation result collection is going to be refreshed</param>
        /// <param name="relationshipIdToBeCreated"></param>
        public void RefreshOperationResultsSchema(RelationshipCollection relationships, Int64 relationshipIdToBeCreated = -100000)
        {
            PrepareRelationshipOperationResultsSchema(relationships, relationshipIdToBeCreated, RefreshType.Delta);
        }

        /// <summary>
        /// Prepares Relationship Operation Results schema as per the relationships 
        /// </summary>
        /// <param name="relationships">Relationships for which operation results schema has to be prepared</param>
        /// <param name="relationshipIdToBeCreated"></param>
        /// <param name="refreshType"></param>
        public void PrepareRelationshipOperationResultsSchema(RelationshipCollection relationships, Int64 relationshipIdToBeCreated = -1, RefreshType refreshType = RefreshType.Full)
        {
            if (relationships != null && relationships.Count > 0)
            {
                if (refreshType == RefreshType.Full && this._relationshipOperationResultCollection.Count > 0)
                {
                    this._relationshipOperationResultCollection.Clear();
                }

                foreach (Relationship relationship in relationships)
                {
                    RelationshipOperationResult relationshipOperationResult = null;

                    if (refreshType == RefreshType.Delta)
                    {
                        relationshipOperationResult = _relationshipOperationResultCollection.FirstOrDefault(ror => ror.RelationshipId == relationship.Id);
                    }

                    if (relationship.Id < 1)
                    {
                        relationship.Id = relationshipIdToBeCreated;
                        relationshipIdToBeCreated--;
                    }

                    if (relationshipOperationResult == null)
                    {
                        relationshipOperationResult = new RelationshipOperationResult(relationship.Id, relationship.RelationshipExternalId, relationship.ToExternalId, relationship.RelationshipTypeName, relationship.ReferenceId);
                        this._relationshipOperationResultCollection.Add(relationshipOperationResult);
                    }
                    else
                    {
                        relationshipOperationResult.RelationshipId = relationship.Id;
                        relationshipOperationResult.ToExternalId = relationship.ToExternalId;
                    }

                    relationshipOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;

                    if (relationship.RelationshipAttributes != null && relationship.RelationshipAttributes.Count > 0)
                    {
                        if (refreshType == RefreshType.Full)
                        {
                            relationshipOperationResult.AttributeOperationResultCollection = new AttributeOperationResultCollection(relationship.RelationshipAttributes);
                        }
                        else
                        {
                            relationshipOperationResult.AttributeOperationResultCollection.RefreshOperationResultsSchema(relationship.RelationshipAttributes);
                        }
                    }

                    if (relationship.RelationshipCollection != null && relationship.RelationshipCollection.Count > 0)
                    {
                        if (refreshType == RefreshType.Full)
                        {
                            RelationshipOperationResultCollection childRelOprResults = new RelationshipOperationResultCollection();
                            childRelOprResults.PrepareRelationshipOperationResultsSchema(relationship.RelationshipCollection, relationshipIdToBeCreated, refreshType);
                            relationshipOperationResult.RelationshipOperationResultCollection = childRelOprResults;
                        }
                        else
                        {
                            relationshipOperationResult.RelationshipOperationResultCollection.RefreshOperationResultsSchema(relationship.RelationshipCollection, relationshipIdToBeCreated);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Fetch RelationshipOperationResult by reference Id
        /// </summary>
        /// <param name="referenceId">Indicates the reference id of relationship</param>
        /// <returns>RelationshipOperationResult having given referenceId</returns>
        public IRelationshipOperationResult GetByReferenceId(Int64 referenceId)
        {
            var filteredRelationshipOperationResults = from relationshipOperationResult in this._relationshipOperationResultCollection
                                                       where relationshipOperationResult.ReferenceId == referenceId
                                                       select relationshipOperationResult;

            if (filteredRelationshipOperationResults.Any())
            {
                return filteredRelationshipOperationResults.First();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subsetRelationshipOperationResultCollection"></param>
        /// <param name="compareIds"></param>
        /// <returns></returns>
        public Boolean IsSuperSetOf(RelationshipOperationResultCollection subsetRelationshipOperationResultCollection, Boolean compareIds = false)
        {
            if (this.OperationResultStatus != subsetRelationshipOperationResultCollection.OperationResultStatus)
                return false;

            foreach (RelationshipOperationResult relationshipOperationResult in subsetRelationshipOperationResultCollection)
            {
                RelationshipOperationResult subsetRelationshipOperationResult = this.Where(ror => ror.RelationshipExternalId == relationshipOperationResult.RelationshipExternalId
                                                                                    && ror.RelationshipExternalId == relationshipOperationResult.RelationshipExternalId
                                                                                    && ror.ToExternalId == relationshipOperationResult.ToExternalId
                                                                                    && ror.RelationshipTypeName == relationshipOperationResult.RelationshipTypeName).ToList<RelationshipOperationResult>().FirstOrDefault();

                if (subsetRelationshipOperationResult == null)
                {
                    return false;
                }

                if (!relationshipOperationResult.IsSuperSetOf(subsetRelationshipOperationResult, compareIds))
                {
                    return false;
                }

            }

            return true;
        }

        #endregion

        #region Private Methods

        private RelationshipOperationResult GetRelationshipOperationResult(String relationshipExternalId, String toExternalId, String relationshipTypeName)
        {
            RelationshipOperationResult resultedROR = null;

            if (this._relationshipOperationResultCollection != null && this._relationshipOperationResultCollection.Count > 0)
            {
                foreach (RelationshipOperationResult ror in this._relationshipOperationResultCollection)
                {
                    if (ror.RelationshipExternalId == relationshipExternalId && ror.ToExternalId == toExternalId && ror.RelationshipTypeName == relationshipTypeName)
                    {
                        resultedROR = ror;
                        break;
                    }

                    resultedROR = ror.RelationshipOperationResultCollection.GetRelationshipOperationResult(relationshipExternalId, toExternalId, relationshipTypeName);

                    if (resultedROR != null)
                    {
                        break;
                    }
                }
            }

            return resultedROR;
        }

        #endregion

        #region ICollection<RelationshipOperationResult> Members

        /// <summary>
        /// Add relationship operation result object in collection
        /// </summary>
        /// <param name="relationshipOperationResult">Attribute operation result to add in collection</param>
        public void Add(RelationshipOperationResult relationshipOperationResult)
        {
            this._relationshipOperationResultCollection.Add(relationshipOperationResult);
        }

        /// <summary>
        /// Removes all relationships from collection
        /// </summary>
        public void Clear()
        {
            this._relationshipOperationResultCollection.Clear();
        }

        /// <summary>
        /// Determines whether the RelationshipOperationResultCollection contains a specific relationship operation result
        /// </summary>
        /// <param name="relationshipOperationResult">The relationship operation result object to locate in the RelationshipOperationResultCollection.</param>
        /// <returns>
        /// <para>true : If relationship operation result found in RelationshipOperationResultCollection</para>
        /// <para>false : If relationship operation result found not in RelationshipOperationResultCollection</para>
        /// </returns>
        public Boolean Contains(RelationshipOperationResult relationshipOperationResult)
        {
            return this._relationshipOperationResultCollection.Contains(relationshipOperationResult);
        }

        /// <summary>
        /// Copies the elements of the RelationshipOperationResultCollection to a
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from RelationshipOperationResultCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(RelationshipOperationResult[] array, Int32 arrayIndex)
        {
            this._relationshipOperationResultCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of relationship in RelationshipOperationResultCollection
        /// </summary>
        public Int32 Count
        {
            get
            {
                return this._relationshipOperationResultCollection.Count;
            }
        }

        /// <summary>
        /// Check if RelationshipOperationResultCollection is read-only.
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific relationship operation result from the RelationshipOperationResultCollection.
        /// </summary>
        /// <param name="relationshipOperationResult">The relationship operation result object to remove from the RelationshipOperationResultCollection.</param>
        /// <returns>true if relationship operation result is successfully removed; otherwise, false. This method also returns false if relationship operation result was not found in the original collection</returns>
        public Boolean Remove(RelationshipOperationResult relationshipOperationResult)
        {
            //Commenting remove

            //There should not be any way to remove relationship operation result
            //If any business rule removes any of the entityOR from the Operation Result schema, operation gives improper results

            //return this._relationshipOperationResultCollection.Remove(relationshipOperationResult);

            return false;
        }

        #endregion

        #region IEnumerable<RelationshipOperationResult> Members

        /// <summary>
        /// Returns an enumerator that iterates through a RelationshipOperationResultCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<RelationshipOperationResult> GetEnumerator()
        {
            return this._relationshipOperationResultCollection.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a RelationshipOperationResultCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._relationshipOperationResultCollection.GetEnumerator();
        }

        #endregion

        #endregion Methods
    }
}