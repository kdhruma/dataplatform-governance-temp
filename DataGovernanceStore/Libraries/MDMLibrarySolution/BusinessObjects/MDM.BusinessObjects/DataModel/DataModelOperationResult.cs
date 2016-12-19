using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DataModel
{
    using MDM.Core;
    using MDM.Core.DataModel;
    using MDM.Interfaces;

    /// <summary>
    /// Represents class for data model operation result
    /// </summary>
    [DataContract]
    public class DataModelOperationResult : OperationResult, IDataModelOperationResult
    {
        #region Fields

        /// <summary>
        /// Field for the id of operation result
        /// </summary>
        private Int64 _id = -1;

        /// <summary>
        /// Field denoting external id of the dataModelObject for which results are created
        /// </summary>
        private String _externalId = String.Empty;

        /// <summary>
        /// Field denoting the long name of the dataModelObject for which results are created
        /// </summary>
        private String _dataModelObjectLongName = String.Empty;

        /// <summary>
        /// Field denoting object type
        /// </summary>
        private ObjectType _dataModelObjectType;

        #endregion

        #region Properties

        /// <summary>
        /// Field for the id of data model operation result
        /// </summary>
        public new Int64 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Property defining the type of the Operation Result
        /// </summary>
        [DataMember]
        public ObjectType DataModelObjectType
        {
            get
            {
                return _dataModelObjectType;
            }

            set
            {
                _dataModelObjectType = value;
            }
        }


        /// <summary>
        /// Property denoting the external id of the dataModelObject for which results are created
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
        /// Property denoting the long name of the dataModelObject for which results are created
        /// </summary>
        [DataMember]
        public String LongName
        {
            get
            {
                return _dataModelObjectLongName;
            }
            set
            {
                _dataModelObjectLongName = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes new instance of dataModelObject operation result
        /// </summary>
        public DataModelOperationResult()
        {

        }

        /// <summary>
        /// Initializes new instance of dataModelObject operation result with the provided dataModelObject id
        /// </summary>
        /// <param name="externalId">External id of the dataModelObject</param>
        /// <param name="dataModelObjectLongName">Long name of the dataModelObject</param>
        public DataModelOperationResult(String externalId, String dataModelObjectLongName)
        {
            this.ExternalId = externalId;
            this.LongName = dataModelObjectLongName;
        }

        /// <summary>
        /// Initialize new instance of dataModelObject operation result from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for dataModelObject operation result</param>
        public DataModelOperationResult(String valuesAsXml)
        {
            LoadOperationResult(valuesAsXml);
        }

        /// <summary>
        /// Initialize new instance of data model operation result.
        /// </summary>
        /// <param name="id">Indicates data model operation result identifier</param>
        /// <param name="longName">Indicates the long name</param>
        /// <param name="externalId">Indicates the external identifier</param>
        /// <param name="referenceId">Indicates the source reference identifier</param>
        public DataModelOperationResult(Int64 id, String longName, String externalId, String referenceId)
        {
            this.Id = id;
            this.LongName = longName;
            this.ExternalId = externalId;
            this.ReferenceId = referenceId;
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Xml representation of DataModel Operation Result
        /// </summary>
        /// <returns>Xml representation of DataModel Operation Result object</returns>
        public new String ToXml()
        {
            String dataModelObjectOperationResultXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Operation result node start
            xmlWriter.WriteStartElement("DataModelOperationResult");

            #region Write dataModelObject operation result properties

            xmlWriter.WriteAttributeString("Name", this.LongName.ToString());
            xmlWriter.WriteAttributeString("ExternalId", this.ExternalId.ToString());
            xmlWriter.WriteAttributeString("ReferenceId", this.ReferenceId.ToString());
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

            //Operation result node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            dataModelObjectOperationResultXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return dataModelObjectOperationResultXml;
        }

        /// <summary>
        /// Get Xml representation of DataModel operation result based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of DataModel operation result</returns>
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
                xmlWriter.WriteStartElement("DataModelOperationResult");

                #region Write dataModelObject operation result properties

                xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                xmlWriter.WriteAttributeString("Name", this.LongName.ToString());
                xmlWriter.WriteAttributeString("ExternalId", this.ExternalId.ToString());
                xmlWriter.WriteAttributeString("ReferenceId", this.ReferenceId.ToString());
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
        /// This method copies entire DataModelOperationResult
        /// </summary>
        /// <param name="dataModelObjectOperationResult">source dataModelObject operation result</param>
        /// <param name="copyDataModelMetadata">Boolean flag which indicates if copying dataModelObject metadata is required or not.</param>
        public void CopyDataModelOperationResult(DataModelOperationResult dataModelObjectOperationResult, Boolean copyDataModelMetadata)
        {
            #region Parameter Validation

            if (dataModelObjectOperationResult == null)
            {
                throw new ArgumentNullException("DataModel OperationResult");
            }

            #endregion Parameter Validation

            if (copyDataModelMetadata)
            {
                this.Id = dataModelObjectOperationResult.Id;
                this.LongName = dataModelObjectOperationResult.LongName;
                this.ExternalId = dataModelObjectOperationResult.ExternalId;
                this.ReferenceId = dataModelObjectOperationResult.ReferenceId;
            }

            //Copy OperationResultStatus from Source to Target if OperationResultStatus of Source is Failed/ CompletedWithErrors. 
            //If None/ Successful, do not copy.
            if ((dataModelObjectOperationResult.OperationResultStatus == OperationResultStatusEnum.Failed ||
                    dataModelObjectOperationResult.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors))
            {
                this.OperationResultStatus = dataModelObjectOperationResult.OperationResultStatus;
            }
            else if (dataModelObjectOperationResult.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
                this.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;

            //Copy Errors from Source to Target
            if (dataModelObjectOperationResult.Errors != null)
            {
                if (this.Errors == null)
                {
                    this.Errors = new ErrorCollection();
                }

                foreach (Error err in dataModelObjectOperationResult.Errors)
                {
                    this.Errors.Add(err);
                }
            }

            //Copy Informations from Source to Target
            if (dataModelObjectOperationResult.Informations != null)
            {
                if (this.Informations == null)
                {
                    this.Informations = new InformationCollection();
                }
                foreach (Information info in dataModelObjectOperationResult.Informations)
                {
                    this.Informations.Add(info);
                }
            }

            //Copy Warnings from Source to Target
            if (dataModelObjectOperationResult.Warnings != null)
            {
                if (this.Warnings == null)
                {
                    this.Warnings = new WarningCollection();
                }
                foreach (Warning warning in dataModelObjectOperationResult.Warnings)
                {
                    this.Warnings.Add(warning);
                }
            }

        }

        /// <summary>
        /// This method copies entire DataModelOperationResult
        /// </summary>
        /// <param name="iDataModelOperationResult">source dataModelObject operation result</param>
        /// <param name="copyDataModelMetadata">boolean flag which indicates if copying dataModelObject metadata is required or not.</param>
        public void CopyDataModelOperationResult(IDataModelOperationResult iDataModelOperationResult, Boolean copyDataModelMetadata)
        {
            #region Parameter Valudation

            if (iDataModelOperationResult == null)
            {
                throw new ArgumentNullException("iDataModelOperationResult");
            }

            #endregion Parameter Valudation

            DataModelOperationResult dataModelObjectOperationResult = (DataModelOperationResult)iDataModelOperationResult;
            this.CopyDataModelOperationResult(dataModelObjectOperationResult, copyDataModelMetadata);
        }

        /// <summary>
        /// Get collection of errors from current DataModelOperationResult. This will merge errors from current DataModelOR and errors from all Attribute operation result.
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

            return errors;
        }

        /// <summary>
        /// Compare data model operation result with current operation result.
        /// This method will compare data model operation result.
        /// </summary>
        /// <param name="subsetDataModelOperationResult">Data Model operation result to be compared with current operation result.</param>
        /// <param name="compareIds">Flag to determine whether id based comparison is true or not</param>
        /// <returns>True : Is both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(DataModelOperationResult subsetDataModelOperationResult, Boolean compareIds = false)
        {
            if (compareIds)
            {
                if (this.Id != subsetDataModelOperationResult.Id)
                    return false;

                if (this.Id != subsetDataModelOperationResult.Id)
                    return false;

                if (this.ReferenceId != subsetDataModelOperationResult.ReferenceId)
                    return false;
            }

            if (this.LongName != subsetDataModelOperationResult.LongName)
                return false;

            if (this.ExternalId != subsetDataModelOperationResult.ExternalId)
                return false;

            if (!base.IsSuperSetOf(subsetDataModelOperationResult, compareIds))
                return false;

            return true;
        }

        /// <summary>
        /// Get Reference Name for datamodel object type. 
        /// </summary>
        /// <returns>Reference Name for given ObjectType</returns>
        public String GetReferenceNameByObjectType()
        {
            String referenceName = String.Empty;

            if (DataModelDictionary.ObjectsDictionary.ContainsKey(_dataModelObjectType))
            {
                referenceName = DataModelDictionary.ObjectsDictionary[_dataModelObjectType].ToString();
            }

            return referenceName;
        }

        /// <summary>
        /// Get Reference Name for given ObjectType 
        /// </summary>
        /// <returns>Reference Name for given ObjectType</returns>
        public static String GetReferenceNameByObjectType(ObjectType objectType)
        {
            String referenceName = String.Empty;

            if (DataModelDictionary.ObjectsDictionary.ContainsKey(objectType))
            {
                referenceName = DataModelDictionary.ObjectsDictionary[objectType].ToString();
            }

            return referenceName;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialize current object with Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for current OperationResult
        /// <para>
        /// Sample Xml:
        /// <![CDATA[
        /// <DataModelOperationResult Id="1234" ReferenceId="ExcelRowNum" ExternaId="OrgSNFromExcel" Name="OrgSN" Status="Failed">
        ///     <Errors>
        ///         <Error Code="" Message = "DataModelObject Validate Inputs failed"/>
        ///     </Errors>
        ///     <Informations>
        ///         <Information Code="" Message="DataModel process successful : Id = 4376255, Name = 116674, LongName = 116674, SKU = 116674, Path = " />
        ///     </Informations>
        /// </DataModelOperationResult>
        /// ]]>
        /// </para>
        /// </param>
        private new void LoadOperationResult(String valuesAsXml)
        {
            #region Sample Xml
            /*
             <DataModelOperationResult Id="1234" Name="P2198" Status="Failed">
                <Errors>
                    <Error Code="" Message = "DataModelCreated BR failed"/>
                </Errors>
                <Informations>
                    <Information Code="" Message="DataModel process successful : Id = 4376255, Name = 116674, LongName = 116674, SKU = 116674, Path = " />
                </Informations>
                <AttributeOperationResults Status="Failed">
                    <AttributeOperationResult Id="9812" LongName="Description" Status="Failed">
                        <Errors>
                            <Error Code="" Message = "Required Attributes are not filled"/>
                        </Errors>
                    </AttributeOperationResult>
                </AttributeOpeartionResults>
             </DataModelOperationResult>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DataModelOperationResult")
                        {
                            #region Read DataModelOperationResult attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.LongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ExternalId"))
                                {
                                    this.ExternalId = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ReferenceId"))
                                {
                                    this.ReferenceId = reader.ReadContentAsString();
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
                                    String strPerformedAction = reader.ReadContentAsString();

                                    if (!String.IsNullOrWhiteSpace(strPerformedAction))
                                    {
                                        Enum.TryParse(strPerformedAction, out performedAction);
                                    }

                                    PerformedAction = performedAction;
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