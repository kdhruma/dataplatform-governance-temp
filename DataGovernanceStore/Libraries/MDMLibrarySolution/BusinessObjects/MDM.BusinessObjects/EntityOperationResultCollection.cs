using System;
using System.Collections;
using System.Collections.Generic;
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
    /// Specifies Entity Operation Result Collection
    /// </summary>
    [DataContract]
    public class EntityOperationResultCollection : ICollection<EntityOperationResult>, IEnumerable<EntityOperationResult>, IEntityOperationResultCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting generic errors for operation
        /// </summary>
        private ErrorCollection _errors = new ErrorCollection();

        /// <summary>
        /// Field denoting generic information about operation
        /// </summary>
        private InformationCollection _informations = new InformationCollection();

        /// <summary>
        /// Field denoting generic warning about operation
        /// </summary>
        private WarningCollection _warnings = new WarningCollection();

        /// <summary>
        /// Indicates status of Operation.
        /// </summary>
        private OperationResultStatusEnum _operationResultStatus = OperationResultStatusEnum.None;

        /// <summary>
        /// Field denoting collection of Entity Operation Result
        /// </summary>
        [DataMember]
        private Collection<EntityOperationResult> _entityOperationResultCollection = new Collection<EntityOperationResult>();

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
        /// Property denoting generic error about operation
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
        /// Find entity operation result from EntityOperationResultCollection based on entityId
        /// </summary>
        /// <param name="entityId">entityId to search in entity Operation Result Collection</param>
        /// <returns>EntityOperationResult object having given entityId</returns>
        public EntityOperationResult this[Int64 entityId]
        {
            get
            {
                EntityOperationResult entityOperationResult = GetEntityOperationResult(entityId);
                if (entityOperationResult == null)
                    throw new ArgumentException(String.Format("No entity operation result found for entity id: {0}", entityId), "entityId");

                return entityOperationResult;
            }
            set
            {
                EntityOperationResult entityOperationResult = GetEntityOperationResult(entityId);
                if (entityOperationResult == null)
                    throw new ArgumentException(String.Format("No entity operation result found for entity id: {0}", entityId), "entityId");

                entityOperationResult = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the entity operation result collection class
        /// </summary>
        public EntityOperationResultCollection()
        {

        }

        /// <summary>
        /// Initializes a new instance of the entity operation result collection class with the IList of entityOperationResults
        /// </summary>
        /// <param name="entityOperationResults">IList of entityOperationResults</param>
        public EntityOperationResultCollection(IList<EntityOperationResult> entityOperationResults)
        {
            this._entityOperationResultCollection = new Collection<EntityOperationResult>(entityOperationResults);
        }

        /// <summary>
        /// Initializes a new instance of the entity operation result collection class with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        public EntityOperationResultCollection(String valuesAsXml)
        {
            /*
             * Sample XML:
              <EntityOperationResults Status="Failed">
              <Errors />
              <Informations />
              <EntityOperationResult Id="1234" >
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
             </EntityOperationResult>
             </EntityOperationResults>
             */
            XmlTextReader reader = null;

            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityOperationResults")
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
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityOperationResult")
                    {
                        String entityOperationResultsXML = reader.ReadOuterXml();

                        if (!String.IsNullOrWhiteSpace(entityOperationResultsXML))
                        {
                            EntityOperationResult entityOperationResult = new EntityOperationResult(entityOperationResultsXML);

                            if (entityOperationResult != null)
                                this._entityOperationResultCollection.Add(entityOperationResult);
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

        /// <summary>
        /// Initializes a new instance of the entity operation result collection for the entities
        /// </summary>
        /// <param name="entities">Entities as per which entity operation result collection is going to be initialized</param>
        public EntityOperationResultCollection(EntityCollection entities)
        {
            PrepareEntityOperationResultsSchema(entities);
        }

        

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Check if EntityOperationResultCollection contains entity operation result with given entity id
        /// </summary>
        /// <param name="entityId">Id of the entity using which entity operation result is to be searched from collection</param>
        /// <returns>
        /// <para>true : If entity operation result found in EntityOperationResultCollection</para>
        /// <para>false : If entity operation result found not in EntityOperationResultCollection</para>
        /// </returns>
        public Boolean Contains(Int64 entityId)
        {
            if (GetEntityOperationResult(entityId) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check if EntityOperationResultCollection contains entity operation result with given entity Name 
        /// </summary>
        /// <param name="externalId">Specifies short name of the entity using which entity operation result is to be searched from collection</param>
        /// <returns>
        /// <para>true : If entity operation result found in EntityOperationResultCollection</para>
        /// <para>false : If entity operation result found not in EntityOperationResultCollection</para>
        /// </returns>
        public Boolean Contains(String externalId)
        {
            if (GetEntityOperationResult(externalId) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove entityOperationResult object from EntityOperationResultCollection
        /// </summary>
        /// <param name="entityId">Id of the entity using which Entity Operation Result is to be removed from collection</param>
        /// <returns>true if entity operation result is successfully removed; otherwise, false. This method also returns false if entity operation result was not found in the original collection</returns>
        public Boolean Remove(Int64 entityId)
        {
            //Commenting remove

            //There should not be any way to remove entity operation result
            //If any business rule removes any of the entityOR from the Operation Result schema, operation gives improper results

            //EntityOperationResult entityOperationResult = GetEntityOperationResult(entityId);

            //if (entityOperationResult == null)
            //    throw new ArgumentException("No entity operation result found for given entity id");
            //else
            //    return this.Remove(entityOperationResult);

            return false;
        }

        /// <summary>
        /// Get Xml representation of entity operation result collection
        /// </summary>
        /// <returns>Xml representation of entity operation result collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Operation result node start
            xmlWriter.WriteStartElement("EntityOperationResults");

            #region Write entity operation result properties

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

            if (this._entityOperationResultCollection != null)
            {
                foreach (EntityOperationResult eor in this._entityOperationResultCollection)
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
        /// Get Xml representation of entity operation result collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of entity operation result collection</returns>
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
                xmlWriter.WriteStartElement("EntityOperationResults");

                #region Write entity operation result properties

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

                foreach (Information information in this.Informations)
                    xmlWriter.WriteRaw(information.ToXml(objectSerialization));

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

                foreach (EntityOperationResult eor in this._entityOperationResultCollection)
                {
                    //If object serialization says external and the operation result status is success do not add the EOR.. Add only failed ones..
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

        #region Without entityId parameter

        /// <summary>
        /// Adds operation result
        /// </summary>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>The boolean result saying whether add is successful or not</returns>
        public Boolean AddOperationResult(String resultCode, String resultMessage, OperationResultType operationResultType)
        {
            return this.AddOperationResult(resultCode, resultMessage, new Collection<Object>(), ReasonType.NotSpecified, -1, -1, operationResultType);
        }

        /// <summary>
        /// Adds operation result
        /// </summary>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="reasonType">Indicates type of the reason.</param>
        /// <param name="ruleMapContextId">Indicates rule map context identifier.</param>
        /// <param name="ruleId">Indicates rule identifier.</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>The boolean result saying whether add is successful or not</returns>
        public Boolean AddOperationResult(String resultCode, String resultMessage, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType)
        {
            return this.AddOperationResult(resultCode, resultMessage, new Collection<Object>(), reasonType, ruleMapContextId, ruleId, operationResultType);
        }

        /// <summary>
        /// Adds operation result
        /// </summary>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="parameters">Indicates the additional parameters that requires for operationResult</param>      
        /// <param name="reasonType">Indicates type of the reason.</param>
        /// <param name="ruleMapContextId">Indicates rule map context identifier.</param>
        /// <param name="ruleId">Indicates rule identifier.</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>The boolean result saying whether add is successful or not</returns>
        public Boolean AddOperationResult(String resultCode, String resultMessage, Collection<Object> parameters, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType)
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

        #endregion Without entityId parameter

        #region With entityId parameter

        /// <summary>
        /// Adds entity operation result
        /// </summary>
        /// <param name="entityId">Id of the entity for which operation result needs to be added</param>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="parameters">Indicates the additional parameters that requires for operationResult</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>The boolean result saying whether add is successful or not</returns>
        public Boolean AddEntityOperationResult(Int64 entityId, String resultCode, String resultMessage, Collection<Object> parameters, OperationResultType operationResultType)
        {
            return AddEntityOperationResult(entityId, resultCode, resultMessage, parameters, ReasonType.NotSpecified, -1,-1, operationResultType);
        }

        /// <summary>
        /// Adds entity operation result
        /// </summary>
        /// <param name="entityId">Id of the entity for which operation result needs to be added</param>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>The boolean result saying whether add is successful or not</returns>
        public Boolean AddEntityOperationResult(Int64 entityId, String resultCode, String resultMessage, OperationResultType operationResultType)
        {
            return AddEntityOperationResult(entityId, resultCode, resultMessage, new Collection<Object>(), ReasonType.NotSpecified, -1, -1, operationResultType);
        }

        /// <summary>
        /// Adds entity operation result
        /// </summary>
        /// <param name="entityId">Id of the entity for which operation result needs to be added</param>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="reasonType">Indicates type of the reason.</param>
        /// <param name="ruleMapContextId">Indicates rule map context identifier.</param>
        /// <param name="ruleId">Indicates rule identifier.</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>The boolean result saying whether add is successful or not</returns>
        public Boolean AddEntityOperationResult(Int64 entityId, String resultCode, String resultMessage, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType)
        {
            return AddEntityOperationResult(entityId, resultCode, resultMessage, new Collection<Object>(), reasonType, ruleMapContextId, ruleId, operationResultType);
        }

        /// <summary>
        /// Adds entity operation result
        /// </summary>
        /// <param name="entityId">Id of the entity for which operation result needs to be added</param>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>  
        /// <param name="parameters">Indicates the additional parameters that requires for operationResult</param>      
        /// <param name="reasonType">Indicates type of the reason.</param>
        /// <param name="ruleMapContextId">Indicates rule map context identifier.</param>
        /// <param name="ruleId">Indicates rule identifier.</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>The boolean result saying whether add is successful or not</returns>
        public Boolean AddEntityOperationResult(Int64 entityId, String resultCode, String resultMessage, Collection<Object> parameters, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType)
        {
            Boolean addSuccess = false;

            //Get the requested entity operation result
            EntityOperationResult entityOperationResult = GetEntityOperationResult(entityId);

            if (entityOperationResult == null)
            {
                entityOperationResult = new EntityOperationResult() { EntityId = entityId };
                this._entityOperationResultCollection.Add(entityOperationResult);
            }

            //Add operation result
            addSuccess = entityOperationResult.AddOperationResult(resultCode, resultMessage, parameters, reasonType, ruleMapContextId, ruleId, operationResultType);

            if (addSuccess)
            {
                RefreshOperationResultStatus();
            }

            return addSuccess;
        }

        /// <summary>
        /// Adds entity attribute operation result
        /// </summary>
        /// <param name="entityId">Id of the entity for which operation result needs to be added</param>
        /// <param name="attributeId">Id of the entity attribute for which operation result needs to be added</param>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>The boolean result saying whether add is successful or not</returns>
        public Boolean AddAttributeOperationResult(Int64 entityId, Int32 attributeId, String resultCode, String resultMessage, OperationResultType operationResultType)
        {
            Boolean addSuccess = false;

            //Get the requested entity operation result
            EntityOperationResult entityOperationResult = GetEntityOperationResult(entityId);

            if (entityOperationResult != null)
            {
                //Add operation result
                addSuccess = entityOperationResult.AddAttributeOperationResult(attributeId, resultCode, resultMessage, operationResultType);

                if (addSuccess)
                    RefreshOperationResultStatus();
            }

            return addSuccess;
        }

        #endregion With entityId parameter

        #endregion AddOperationResult Overload Methods

        /// <summary>
        /// Calculates the status and updates with new status
        /// </summary>
        public void RefreshOperationResultStatus()
        {
            Int32 failedEORCount = 0;

            //Get failed entity operation results
            IEnumerable<EntityOperationResult> failedEORs = this._entityOperationResultCollection.Where(eor => eor.OperationResultStatus == OperationResultStatusEnum.Failed ||
                eor.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors);

            //Get succeeded entity operation results
            IEnumerable<EntityOperationResult> succeededEORs = this._entityOperationResultCollection.Where(eor => eor.OperationResultStatus == OperationResultStatusEnum.None ||
                eor.OperationResultStatus == OperationResultStatusEnum.Successful);

            //Get warned entity operation results
            IEnumerable<EntityOperationResult> warnedEORs = this._entityOperationResultCollection.Where(eor => eor.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings);

            if (failedEORs != null)
            {
                failedEORCount = failedEORs.Count();
            }

            //if child level don't have any errors but parent level errors are there then based on that overall status needs to count
            if (failedEORCount > 0 || (this._errors != null && this._errors.Count > 0))
            {
                //if all childs are failed the overall status will be Failed else completedWithErrors.
                //if child level errors are not there and only parent level errors are there then status will be failed
                // if all childs not failed and parent level error is there then status will be completedWithErrors
                if (failedEORCount == this.Count)
                {
                    this.OperationResultStatus = OperationResultStatusEnum.Failed;
                }
                else
                {
                    this.OperationResultStatus = OperationResultStatusEnum.CompletedWithErrors;
                }
            }
            //if child level don't have any warnings but parent level warnings are there then also status will be CompletedWithWarnings
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
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public EntityOperationResult GetEntityOperationResult(Int64 entityId)
        {
            var filteredEntityOperationResults = from entityOperationResult in this._entityOperationResultCollection
                                                 where entityOperationResult.EntityId == entityId
                                                 select entityOperationResult;

            if (filteredEntityOperationResults.Any())
                return filteredEntityOperationResults.First();
            else
                return null;
        }

        /// <summary>
        /// Get entity operation result by external id.
        /// </summary>
        /// <param name="externalId">Indicates the external id.</param>
        /// <returns>Entity operation result</returns>
        public EntityOperationResult GetEntityOperationResult(string externalId)
        {
            var filteredEntityOperationResults = from entityOperationResult in this._entityOperationResultCollection
                                                 where entityOperationResult.ExternalId == externalId
                                                 select entityOperationResult;

            if (filteredEntityOperationResults.Any())
            {
                return filteredEntityOperationResults.First();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Fetch EntityOperationResult by reference Id
        /// </summary>
        /// <param name="referenceId">Indicates the reference Id of an entity</param>
        /// <returns>EntityOperationResult having given referenceId</returns>
        public IEntityOperationResult GetByReferenceId(Int64 referenceId)
        {
            var filteredEntityOperationResults = from entityOperationResult in this._entityOperationResultCollection
                                                 where entityOperationResult.ReferenceId == referenceId
                                                 select entityOperationResult;

            if (filteredEntityOperationResults.Any())
                return filteredEntityOperationResults.First();
            else
                return null;
        }

        /// <summary>
        /// Fetch EntityOperationResult by entity Id
        /// </summary>
        /// <param name="entityId">Indicates the entity Id of an entity</param>
        /// <returns>EntityOperationResult having given entityId</returns>
        public IEntityOperationResult GetByEntityId(Int64 entityId)
        {
            IEntityOperationResult filteredEntityOperationResult = null;

            foreach (EntityOperationResult eor in this._entityOperationResultCollection)
            {
                if (eor.EntityId == entityId)
                {
                    filteredEntityOperationResult = eor;
                    break;
                }
            }

            return filteredEntityOperationResult;
        }

        /// <summary>
        /// Refresh and update missing entity operation results for the given entities
        /// </summary>
        /// <param name="entities">Entities as per which entity operation result collection is going to be initialized</param>
        public void RefreshOperationResultsSchema(EntityCollection entities)
        {
            PrepareEntityOperationResultsSchema(entities, RefreshType.Delta);
        }

        /// <summary>
        /// Prepares Entity Operation Results schema as per the entities 
        /// </summary>
        /// <param name="entities">Entities for which operation results schema has to be prepared</param>
        /// <param name="refreshType"></param>
        public void PrepareEntityOperationResultsSchema(EntityCollection entities, RefreshType refreshType = RefreshType.Full)
        {
            if (entities != null && entities.Count > 0)
            {
                if (refreshType == RefreshType.Full && this._entityOperationResultCollection.Count > 0)
                {
                    this._entityOperationResultCollection.Clear();
                }

                Int64 entityIdToBeCreated = -1;

                foreach (Entity entity in entities)
                {
                    EntityOperationResult entityOperationResult = null;

                    if (refreshType == RefreshType.Delta)
                    {
                        entityOperationResult = _entityOperationResultCollection.FirstOrDefault(eor => eor.EntityId == entity.Id);
                    }

                    if (entity.Id < 1)
                    {
                        entity.Id = entityIdToBeCreated;
                        entityIdToBeCreated--;
                    }

                    if(entityOperationResult == null)
                    {
                        entityOperationResult = new EntityOperationResult(entity.Id, entity.ReferenceId, entity.LongName, entity.ExternalId);
                        this._entityOperationResultCollection.Add(entityOperationResult);
                    }
                    else
                    {
                        entityOperationResult.EntityId = entity.Id;
                    }

                    if (entity.Attributes != null && entity.Attributes.Count > 0)
                    {
                        if (refreshType == RefreshType.Full)
                        {
                            entityOperationResult.AttributeOperationResultCollection = new AttributeOperationResultCollection(entity.Attributes);
                        }
                        else
                        {
                            entityOperationResult.AttributeOperationResultCollection.RefreshOperationResultsSchema(entity.Attributes);
                        }
                    }

                    if (entity.Relationships != null && entity.Relationships.Count > 0)
                    {
                        if (refreshType == RefreshType.Full)
                        {
                            entityOperationResult.RelationshipOperationResultCollection = new RelationshipOperationResultCollection(entity.Relationships);
                        }
                        else
                        {
                            entityOperationResult.RelationshipOperationResultCollection.RefreshOperationResultsSchema(entity.Relationships);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Compare EntityOperationResultCollection. 
        /// </summary>
        /// <param name="subsetEntityOperationResultCollection">Expected EntityOperationResultCollection.</param>
        /// <param name="compareIds">Compare Ids or not.</param>
        /// <returns>If acutual EntityOperationResultCollection is match then true else false</returns>
        public Boolean IsSuperSetOf(EntityOperationResultCollection subsetEntityOperationResultCollection, Boolean compareIds = false)
        {
            if (subsetEntityOperationResultCollection != null)
            {
                // compare parent level EntityOperationResultCollection status, errors, warning and informations with expected entityOperationResultCollection
                if (!(this.OperationResultStatus == subsetEntityOperationResultCollection.OperationResultStatus &&
                    this.Errors.IsSuperSetOf(subsetEntityOperationResultCollection.Errors) &&
                    this.Warnings.IsSuperSetOf(subsetEntityOperationResultCollection.Warnings) &&
                    this.Informations.IsSuperSetOf(subsetEntityOperationResultCollection.Informations)))
                {
                    return false;
                }

                if (subsetEntityOperationResultCollection.Count > 0)
                {
                    foreach (EntityOperationResult subsetEntityOperationResult in subsetEntityOperationResultCollection)
                    {
                        EntityOperationResult entityOperationResult = this.GetEntityOperationResult(subsetEntityOperationResult.ExternalId);
                        if (entityOperationResult == null)
                        {
                            return false;
                        }
                        else if (!entityOperationResult.IsSuperSetOf(subsetEntityOperationResult, compareIds))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// This method copies entire entity operation result collection.
        /// </summary>
        /// <param name="sourceEntityOperationResults">source entity operation result collection to copy into target.</param>
        /// <param name="copyEntitiesMetadata">Boolean flag which indicates if copying entity meta data is required or not.</param>
        public void CopyEntityOperationResults(EntityOperationResultCollection sourceEntityOperationResults, Boolean copyEntitiesMetadata)
        {
            if (sourceEntityOperationResults != null && sourceEntityOperationResults.Count > 0)
            {
                foreach (EntityOperationResult sourceEntityOperationResult in sourceEntityOperationResults)
                {
                    EntityOperationResult targetEntityOperationResult = this.GetByEntityId(sourceEntityOperationResult.EntityId) as EntityOperationResult;

                    if (targetEntityOperationResult == null)
                    {
                        this._entityOperationResultCollection.Add(sourceEntityOperationResult);
                    }
                    else
                    {
                        targetEntityOperationResult.CopyEntityOperationResult(sourceEntityOperationResult, copyEntitiesMetadata);
                    }
                }

                this.Errors.AddRange(sourceEntityOperationResults.Errors);
                this.Informations.AddRange(sourceEntityOperationResults.Informations);
                this.Warnings.AddRange(sourceEntityOperationResults.Warnings);

                this.RefreshOperationResultStatus();
            }
        }

        /// <summary>
        /// Copies entire operation results and adds to entity operation results
        /// </summary>
        /// <param name="operationResults">Indicates operation result collection</param>
        public void CopyOperationResults(OperationResultCollection operationResults)
        {
            if (operationResults != null && operationResults.Count > 0)
            {
                foreach (OperationResult operationResult in operationResults)
                {
                    this.Errors.AddRange(operationResult.Errors);
                    this.Warnings.AddRange(operationResult.Warnings);
                    this.Informations.AddRange(operationResult.Informations);
                }

                this.RefreshOperationResultStatus();
            }
        }

        /// <summary>
        /// Determine whether any system error exists in error collection
        /// </summary>
        /// <returns>Returns true if there us any system error exists in error collection; otherwise false.</returns>
        public Boolean HasAnySystemError()
        {
            Boolean hasAnySystemError = false;

            if (this.Errors != null && this.Errors.Count > 0)
            {
                hasAnySystemError = this.Errors.HasAnySystemError();
            }

            if (!hasAnySystemError)
            {
                foreach (EntityOperationResult entityOperationResult in this._entityOperationResultCollection)
                {
                    if (entityOperationResult.Errors != null && entityOperationResult.Errors.Count > 0)
                    {
                        hasAnySystemError = entityOperationResult.Errors.HasAnySystemError();

                        if (hasAnySystemError)
                        {
                            break;
                        }
                    }
                }
            }

            return hasAnySystemError;
        }

        #endregion

        #region Private Methods



        #endregion

        #region ICollection<EntityOperationResult> Members

        /// <summary>
        /// Add entity operation result object in collection
        /// </summary>
        /// <param name="entityOperationResult">Attribute operation result to add in collection</param>
        public void Add(EntityOperationResult entityOperationResult)
        {
            this._entityOperationResultCollection.Add(entityOperationResult);
        }

        /// <summary>
        /// Add entity operation result object in collection
        /// </summary>
        /// <param name="iEntityOperationResult">Indicates entity operation result to add in collection</param>
        public void Add(IEntityOperationResult iEntityOperationResult)
        {
            this.Add((EntityOperationResult)iEntityOperationResult);
        }

        /// <summary>
        /// Adds entity operation results object in collection
        /// </summary>
        /// <param name="entityOperationResults">Indicates entity operation results to add in collection</param>
        public void AddRange(EntityOperationResultCollection entityOperationResults)
        {
            if (entityOperationResults != null && entityOperationResults.Count > 0)
            {
                foreach (EntityOperationResult entityOperationReuslt in entityOperationResults)
                {
                    this._entityOperationResultCollection.Add(entityOperationReuslt);
                }
            }
        }

        /// <summary>
        /// Removes all relationships from collection
        /// </summary>
        public void Clear()
        {
            this._entityOperationResultCollection.Clear();
        }

        /// <summary>
        /// Determines whether the EntityOperationResultCollection contains a specific entity operation result
        /// </summary>
        /// <param name="entityOperationResult">The entity operation result object to locate in the EntityOperationResultCollection.</param>
        /// <returns>
        /// <para>true : If entity operation result found in EntityOperationResultCollection</para>
        /// <para>false : If entity operation result found not in EntityOperationResultCollection</para>
        /// </returns>
        public Boolean Contains(EntityOperationResult entityOperationResult)
        {
            return this._entityOperationResultCollection.Contains(entityOperationResult);
        }

        /// <summary>
        /// Copies the elements of the EntityOperationResultCollection to a
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from EntityOperationResultCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(EntityOperationResult[] array, Int32 arrayIndex)
        {
            this._entityOperationResultCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of relationship in EntityOperationResultCollection
        /// </summary>
        public Int32 Count
        {
            get
            {
                return this._entityOperationResultCollection.Count;
            }
        }

        /// <summary>
        /// Check if EntityOperationResultCollection is read-only.
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific entity operation result from the EntityOperationResultCollection.
        /// </summary>
        /// <param name="entityOperationResult">The entity operation result object to remove from the EntityOperationResultCollection.</param>
        /// <returns>true if entity operation result is successfully removed; otherwise, false. This method also returns false if entity operation result was not found in the original collection</returns>
        public Boolean Remove(EntityOperationResult entityOperationResult)
        {
            //Commenting remove

            //There should not be any way to remove entity operation result
            //If any business rule removes any of the entityOR from the Operation Result schema, operation gives improper results

            //return this._entityOperationResultCollection.Remove(entityOperationResult);

            return false;
        }

        #endregion

        #region IEnumerable<EntityOperationResult> Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityOperationResultCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<EntityOperationResult> GetEnumerator()
        {
            return this._entityOperationResultCollection.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a EntityOperationResultCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._entityOperationResultCollection.GetEnumerator();
        }

        #endregion

        #endregion Methods
    }
}