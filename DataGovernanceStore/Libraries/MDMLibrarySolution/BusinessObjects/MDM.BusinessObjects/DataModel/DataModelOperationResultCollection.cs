using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DataModel
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.Core.Extensions;

    /// <summary>
    /// Represents class for collection of data model operation result
    /// </summary>
    [DataContract]
    public class DataModelOperationResultCollection : ICollection<DataModelOperationResult>, IEnumerable<DataModelOperationResult>, IDataModelOperationResultCollection
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
        /// Field denoting collection of DataModel Operation Result
        /// </summary>
        [DataMember]
        private Collection<DataModelOperationResult> _dataModelOperationResultCollection = new Collection<DataModelOperationResult>();

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
        /// Find dataModel operation result from DataModelOperationResultCollection based on dataModelId
        /// </summary>
        /// <param name="id">dataModelId to search in dataModel Operation Result Collection</param>
        /// <returns>DataModelOperationResult object having given dataModelId</returns>
        public DataModelOperationResult this[int id]
        {
            get
            {
                DataModelOperationResult dataModelOperationResult = GetDataModelOperationResultById(id);
                if (dataModelOperationResult == null)
                    throw new ArgumentException(String.Format("No dataModel operation result found for dataModel id: {0}", id), "id");

                return dataModelOperationResult;
            }
            set
            {
                DataModelOperationResult dataModelOperationResult = GetDataModelOperationResultById(id);
                if (dataModelOperationResult == null)
                    throw new ArgumentException(String.Format("No dataModel operation result found for dataModel id: {0}", id), "id");

                dataModelOperationResult = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the dataModel operation result collection class
        /// </summary>
        public DataModelOperationResultCollection()
        {

        }

        /// <summary>
        /// Initializes a new instance of the datamodel operation result collection class with the IList of dataModelOperationResults
        /// </summary>
        /// <param name="dataModelOperationResults">IList of dataModelOperationResults</param>
        public DataModelOperationResultCollection(IList<DataModelOperationResult> dataModelOperationResults)
        {
            this._dataModelOperationResultCollection = new Collection<DataModelOperationResult>(dataModelOperationResults);
        }

        /// <summary>
        /// Initializes a new instance of the dataModel operation result collection class with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        public DataModelOperationResultCollection(String valuesAsXml)
        {
            /*
             * Sample XML:
              <DataModelOperationResults Status="Failed">
                  <Errors />
                  <Informations />
                  <DataModelOperationResult Id="1234" >
                    <Errors>
                        <Error Code="" Message = "DataModelCreated BR failed"/>
                    </Errors>
                    <Informations>
                        <Information Code="" Message="DataModel process successful : Id = 4376255, Name = 116674, LongName = 116674, SKU = 116674, Path = " />
                    </Informations>
                    <ReturnValues>
                        <ReturnValue>
                            <Entities>
                                <DataModel Id="4376255" ObjectId="318824" Name="116674" LongName="116674" SKU="116674"/>
                            </Entities>
                        </ReturnValue>
                    </ReturnValues>
                  </DataModelOperationResult>
             </DataModelOperationResults>
             */
            XmlTextReader reader = null;

            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "DataModelOperationResults")
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
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "DataModelOperationResult")
                    {
                        String dataModelOperationResultsXML = reader.ReadOuterXml();

                        if (!String.IsNullOrWhiteSpace(dataModelOperationResultsXML))
                        {
                            DataModelOperationResult dataModelOperationResult = new DataModelOperationResult(dataModelOperationResultsXML);

                            if (dataModelOperationResult != null)
                                this._dataModelOperationResultCollection.Add(dataModelOperationResult);
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

        #region Public Methods

        /// <summary>
        /// Check if DataModelOperationResultCollection contains dataModel operation result with given dataModel id
        /// </summary>
        /// <param name="id">Id of the dataModel using which dataModel operation result is to be searched from collection</param>
        /// <returns>
        /// <para>true : If dataModel operation result found in DataModelOperationResultCollection</para>
        /// <para>false : If dataModel operation result found not in DataModelOperationResultCollection</para>
        /// </returns>
        public Boolean Contains(Int32 id)
        {
            if (GetDataModelOperationResultById(id) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove dataModelOperationResult object from DataModelOperationResultCollection
        /// </summary>
        /// <param name="dataModelId">Id of the dataModel using which DataModel Operation Result is to be removed from collection</param>
        /// <returns>true if dataModel operation result is successfully removed; otherwise, false. This method also returns false if dataModel operation result was not found in the original collection</returns>
        public Boolean Remove(Int64 dataModelId)
        {
            //There should not be any way to remove dataModel operation result
            //If any business rule removes any of the dataModelOperationResult from the Operation Result schema, operation gives improper results

            return false;
        }

        /// <summary>
        /// Gets information of Operation Result
        /// </summary>
        /// <returns>Collection of information</returns>
        public Collection<IInformation> GetInformation()
        {
            Collection<IInformation> iInformationCollection = new Collection<IInformation>();

            foreach (Information info in this.Informations)
            {
                iInformationCollection.Add((IInformation)info);
            }

            return iInformationCollection;
        }

        /// <summary>
        /// Gets errors
        /// </summary>
        /// <returns>Collection of errors</returns>
        public Collection<IError> GetErrors()
        {
            Collection<IError> iErrorCollection = new Collection<IError>();

            foreach (Error error in this.Errors)
            {
                iErrorCollection.Add((IError)error);
            }

            return iErrorCollection;
        }

        /// <summary>
        /// Gets warnings
        /// </summary>
        /// <returns>Collection of warnings</returns>
        public IWarningCollection GetWarnings()
        {
            WarningCollection warningCollection = new WarningCollection();

            foreach (Warning warning in this.Warnings)
            {
                warningCollection.Add(warning);
            }

            return (IWarningCollection)warningCollection;
        }


        /// <summary>
        /// Get Xml representation of dataModel operation result collection
        /// </summary>
        /// <returns>Xml representation of dataModel operation result collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Operation result node start
            xmlWriter.WriteStartElement("DataModelOperationResults");

            #region Write dataModel operation result properties

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

            #region Write DataModelOperationResults

            if (this._dataModelOperationResultCollection != null)
            {
                foreach (DataModelOperationResult eor in this._dataModelOperationResultCollection)
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
        /// Get Xml representation of dataModel operation result collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of dataModel operation result collection</returns>
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
                xmlWriter.WriteStartElement("DataModelOperationResults");

                #region Write dataModel operation result properties

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

                #region Write DataModelOperationResults

                foreach (DataModelOperationResult eor in this._dataModelOperationResultCollection)
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

        /// <summary>
        /// Adds operation result
        /// </summary>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>The boolean result saying whether add is successful or not</returns>
        public Boolean AddOperationResult(String resultCode, String resultMessage, OperationResultType operationResultType)
        {
            Boolean addSuccess = false;

            if (operationResultType == OperationResultType.Error)
            {
                Error error = new Error(resultCode, resultMessage);

                if (this.Errors == null)
                    this.Errors = new ErrorCollection();

                this.Errors.Add(error);

                //Update operation result status
                this.OperationResultStatus = OperationResultStatusEnum.Failed;

                addSuccess = true;
            }
            else if (operationResultType == OperationResultType.Information)
            {
                Information information = new Information(resultCode, resultMessage);

                if (this.Informations == null)
                    this.Informations = new InformationCollection();

                this.Informations.Add(information);
                addSuccess = true;
            }
            else if (operationResultType == OperationResultType.Warning)
            {
                Warning warning = new Warning(resultCode, resultMessage);

                if (this.Warnings == null)
                    this.Warnings = new WarningCollection();

                this.Warnings.Add(warning);

                this.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;

                addSuccess = true;
            }

            return addSuccess;
        }

        /// <summary>
        /// Adds dataModel operation result
        /// </summary>
        /// <param name="id">Id of the dataModel for which operation result needs to be added</param>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>The boolean result saying whether add is successful or not</returns>
        public Boolean AddDataModelOperationResult(Int32 id, String resultCode, String resultMessage, OperationResultType operationResultType)
        {
            Boolean addSuccess = false;

            //Get the requested dataModel operation result
            DataModelOperationResult dataModelOperationResult = GetDataModelOperationResultById(id);

            if (dataModelOperationResult != null)
            {
                //Add operation result
                addSuccess = dataModelOperationResult.AddOperationResult(resultCode, resultMessage, operationResultType);

                if (addSuccess)
                    RefreshOperationResultStatus();
            }

            return addSuccess;
        }

        /// <summary>
        /// Calculates the status and updates with new status
        /// </summary>
        public void RefreshOperationResultStatus()
        {
            //Get failed dataModel operation results
            IEnumerable<DataModelOperationResult> failedEORs = this._dataModelOperationResultCollection.Where(eor => eor.OperationResultStatus == OperationResultStatusEnum.Failed ||
                eor.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors);

            //Get succeeded dataModel operation results
            IEnumerable<DataModelOperationResult> succeededEORs = this._dataModelOperationResultCollection.Where(eor => eor.OperationResultStatus == OperationResultStatusEnum.None ||
                eor.OperationResultStatus == OperationResultStatusEnum.Successful);

            //Get warned dataModel operation results
            IEnumerable<DataModelOperationResult> warnedEORs = this._dataModelOperationResultCollection.Where(eor => eor.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings);

            if (failedEORs != null && failedEORs.Count() > 0)
            {
                //if all are failed the overall status will be Failed else completedWithErrors.
                if (failedEORs.Count() == this.Count)
                    this.OperationResultStatus = OperationResultStatusEnum.Failed;
                else
                    this.OperationResultStatus = OperationResultStatusEnum.CompletedWithErrors;
            }
            else if (warnedEORs != null && warnedEORs.Count() > 0)
            {
                //if some are get warned the overall status will be CompletedWithWarnings
                this.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
            }
            else if (succeededEORs != null && succeededEORs.Count() > 0)
            {
                //if there are no errors and warnings the overall status will be successful
                this.OperationResultStatus = OperationResultStatusEnum.Successful;
            }
            else
                this.OperationResultStatus = OperationResultStatusEnum.None;
        }

        /// <summary>
        /// Get data model operation result object based on id
        /// </summary>
        /// <param name="id">Indicates the identifier for which data model operation result to be returned</param>
        /// <returns>Returns data model operation result object based on id</returns>
        public DataModelOperationResult GetDataModelOperationResultById(int id)
        {
            DataModelOperationResult dataModelOperationResult = null;
            if (!this._dataModelOperationResultCollection.IsNullOrEmpty())
            {
                foreach (DataModelOperationResult item in this._dataModelOperationResultCollection)
                {
                    if (item.Id == id)
                    {
                        dataModelOperationResult = item;
                        break;
                    }
                }
            }

            return dataModelOperationResult;
        }

        /// <summary>
        /// Get dataModel operation result by external Id
        /// </summary>
        /// <param name="externalId">Indicates the external id</param>
        /// <returns>Returns the data model operation result</returns>
        public DataModelOperationResult GetDataModelOperationResultByExternalId(string externalId)
        {
            DataModelOperationResult dataModelOperationResult = null;
            if (!this._dataModelOperationResultCollection.IsNullOrEmpty())
            {
                foreach (DataModelOperationResult item in this._dataModelOperationResultCollection)
                {
                    if (String.Compare(item.ExternalId, externalId) == 0)
                    {
                        dataModelOperationResult = item;
                        break;
                    }
                }
            }

            return dataModelOperationResult;
        }

        /// <summary>
        /// Fetch DataModelOperationResult by reference Id
        /// </summary>
        /// <param name="referenceId">Indicates the reference Id of an dataModel</param>
        /// <returns>DataModelOperationResult having given referenceId</returns>
        public IDataModelOperationResult GetByReferenceId(string referenceId)
        {
            DataModelOperationResult dataModelOperationResult = null;
            if (!this._dataModelOperationResultCollection.IsNullOrEmpty())
            {
                foreach (DataModelOperationResult item in this._dataModelOperationResultCollection)
                {
                    if (String.Compare(item.ReferenceId, referenceId) == 0)
                    {
                        dataModelOperationResult = item;
                        break;
                    }
                }
            }

             return dataModelOperationResult;
        }

        /// <summary>
        /// Compare DataModelOperationResultCollection. 
        /// </summary>
        /// <param name="subsetDataModelOperationResultCollection">Expected DataModelOperationResultCollection.</param>
        /// <param name="compareIds">Compare Ids or not.</param>
        /// <returns>If acutual DataModelOperationResultCollection is match then true else false</returns>
        public Boolean IsSuperSetOf(DataModelOperationResultCollection subsetDataModelOperationResultCollection, Boolean compareIds = false)
        {
            if (subsetDataModelOperationResultCollection != null && subsetDataModelOperationResultCollection.Count > 0)
            {
                foreach (DataModelOperationResult subsetDataModelOperationResult in subsetDataModelOperationResultCollection)
                {
                    DataModelOperationResult dataModelOperationResult = this.GetDataModelOperationResultByExternalId(subsetDataModelOperationResult.ExternalId);

                    if (dataModelOperationResult == null)
                    {
                        return false;
                    }
                    else if (!dataModelOperationResult.IsSuperSetOf(subsetDataModelOperationResult, compareIds))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Gets OperationResultCollection representation of DataModel Collection
        /// </summary>
        /// <returns></returns>
        public IOperationResultCollection GetOperationResultCollection()
        {
            OperationResultCollection operationResults = new OperationResultCollection();
            
            foreach (DataModelOperationResult dataModelOperationResult in this._dataModelOperationResultCollection)
            {
                operationResults.Add(dataModelOperationResult);
            }

            return operationResults;
        }

        #endregion

        #region Private Methods

        

        #endregion

        #region ICollection<DataModelOperationResult> Members

        /// <summary>
        /// Add dataModel operation result object in collection
        /// </summary>
        /// <param name="dataModelOperationResult">Indicates data model operation result to add in collection</param>
        public void Add(DataModelOperationResult dataModelOperationResult)
        {
            this._dataModelOperationResultCollection.Add(dataModelOperationResult);
        }

        /// <summary>
        /// Add dataModel operation result objects in collection
        /// </summary>
        /// <param name="items">Indicates data model operation results to add in collection</param>
        public void AddRange(IDataModelOperationResultCollection items)
        {
            foreach (IDataModelOperationResult item in items)
            {
                if (item.HasError)
                {
                    //Update operation result status
                    this.OperationResultStatus = OperationResultStatusEnum.CompletedWithErrors;
                }
                else if (item.HasWarnings)
                {
                    //Update operation result status
                    if (this.OperationResultStatus != OperationResultStatusEnum.CompletedWithErrors)
                        this.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
                }

                this._dataModelOperationResultCollection.Add((DataModelOperationResult)item);
            }
        }

        /// <summary>
        /// Removes all data model operation result from collection
        /// </summary>
        public void Clear()
        {
            this._dataModelOperationResultCollection.Clear();
        }

        /// <summary>
        /// Determines whether the DataModelOperationResultCollection contains a specific dataModel operation result
        /// </summary>
        /// <param name="dataModelOperationResult">The dataModel operation result object to locate in the DataModelOperationResultCollection.</param>
        /// <returns>
        /// <para>true : If dataModel operation result found in DataModelOperationResultCollection</para>
        /// <para>false : If dataModel operation result found not in DataModelOperationResultCollection</para>
        /// </returns>
        public Boolean Contains(DataModelOperationResult dataModelOperationResult)
        {
            return this._dataModelOperationResultCollection.Contains(dataModelOperationResult);
        }

        /// <summary>
        /// Copies the elements of the DataModelOperationResultCollection to a
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from DataModelOperationResultCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(DataModelOperationResult[] array, Int32 arrayIndex)
        {
            this._dataModelOperationResultCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. in DataModelOperationResultCollection
        /// </summary>
        public Int32 Count
        {
            get
            {
                return this._dataModelOperationResultCollection.Count;
            }
        }

        /// <summary>
        /// Check if DataModelOperationResultCollection is read-only.
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific dataModel operation result from the DataModelOperationResultCollection.
        /// </summary>
        /// <param name="dataModelOperationResult">Indicates the dataModel operation result object to remove from the DataModelOperationResultCollection.</param>
        /// <returns>Returns true if dataModel operation result is successfully removed; otherwise false. 
        /// This method also returns false if dataModel operation result was not found in the original collection
        /// </returns>
        public Boolean Remove(DataModelOperationResult dataModelOperationResult)
        {
            //Commenting remove

            //There should not be any way to remove dataModel operation result
            //If any business rule removes any of the dataModelOperationResult from the Operation Result schema, operation gives improper results

            //return this._dataModelOperationResultCollection.Remove(dataModelOperationResult);

            return false;
        }

        #endregion

        #region IEnumerable<DataModelOperationResult> Members

        /// <summary>
        /// Returns an enumerator that iterates through a DataModelOperationResultCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<DataModelOperationResult> GetEnumerator()
        {
            return this._dataModelOperationResultCollection.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a DataModelOperationResultCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._dataModelOperationResultCollection.GetEnumerator();
        }

        #endregion
    }
}
