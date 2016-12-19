using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Jobs
{
    using MDM.BusinessObjects.DataModel;
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Job Import Result
    /// </summary>
    [DataContract]
    public class JobImportResult : MDMObject, IJobImportResult
    {
        #region Fields

        /// <summary>
        /// field denoting the Job Id
        /// </summary>
        private Int32 _jobId = 0;

        /// <summary>
        /// field denoting the staus of the result.
        /// </summary>
        private String _status = String.Empty;

        ///// <summary>
        ///// field denoting the external id of the entity.
        ///// </summary>
        private String _externalId = String.Empty;

        /// <summary>
        /// field denoting the internal id of the entity ( if applicable)
        /// </summary>
        private Int64 _internalId = 0;

        /// <summary>
        /// field denoting description
        /// </summary>
        private String _operationResultXML;

        /// <summary>
        /// field denoting description
        /// </summary>
        private String _description = String.Empty;

        /// <summary>
        /// field denoting Audit Ref Id
        /// </summary>
        private Int64 _auditRefId = 0;

        /// <summary>
        /// field denoting Job Object Type
        /// </summary>
        private ObjectType _jobObjectType = 0;

        /// <summary>
        /// field denoting Job Object Type
        /// </summary>
        private ObjectAction _performedAction = ObjectAction.Unknown;

        /// <summary>
        /// 
        /// </summary>
        private OperationResult _operationResult;

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public JobImportResult()
            : base()
        {
        }

        /// <summary>
        /// Constructor with job Id ,external and internal id, error code and description.
        /// </summary>
        /// <param name="id">Indicates the base identifier</param>
        /// <param name="jobId">Indicates the job Id</param>
        /// <param name="status">Indicates the status</param>
        /// <param name="externalId">Indicates the external Id</param>
        /// <param name="internalId">Indicates the internal Id</param>
        /// <param name="description">Indicates the description</param>
        /// <param name="operationResultXml">Indicates operation result xml</param>
        /// <param name="auditRefId">Indicates audit reference Id</param>
        /// <param name="jobObjectType">Indicates the job object type</param>
        /// <param name="performedAction">Indicates performed action</param>
        public JobImportResult(Int32 id, Int32 jobId, String status, String externalId, Int64 internalId, String description, String operationResultXml, Int32 auditRefId, ObjectType jobObjectType, ObjectAction performedAction)
            : base(id)
        {
            this._jobId = jobId;
            this._status = status;
            this._externalId = externalId;
            this._internalId = internalId;
            this._description = description;
            this._operationResultXML = operationResultXml;
            this._auditRefId = auditRefId;
            this._jobObjectType=jobObjectType;
            this._performedAction = performedAction;
            LoadOperationResultBasedOnJobType(operationResultXml);
            
        }

        /// <summary>
        /// Constructor with XML
        /// </summary>
        /// <param name="valuesAsxml">XML having xml value</param>
        public JobImportResult(String valuesAsxml)
        {
            LoadJobImportResult(valuesAsxml);
        }
        #endregion

        #region Properties
        
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public OperationResult OperationResult
        {
            get
            {
                return _operationResult;
            }
            set
            {
                _operationResult = value;
            }
        }

        /// <summary>
        ///  Property denoting the Job Id
        /// </summary>
        [DataMember]
        public Int32 JobId
        {
            get
            {
                return this._jobId;
            }
            set
            {
                this._jobId = value;
            }
        }

        /// <summary>
        ///  Property denoting the status of the job result
        /// </summary>
        [DataMember]
        public String Status
        {
            get
            {
                return this._status;
            }
            set
            {
                this._status = value;
            }
        }

        /// <summary>
        ///  Property denoting the external Id of the object type
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
        ///  Property denoting the internal Id of the object type
        /// </summary>
        [DataMember]
        public Int64 InternalId
        {
            get
            {
                return this._internalId;
            }
            set
            {
                this._internalId = value;
            }
        }

        /// <summary>
        ///  Property denoting the description of the Job Import Error
        /// </summary>
        [DataMember]
        public String Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        /// <summary>
        ///  Property denoting the operation result
        /// </summary>
        [DataMember]
        public String OperationResultXML
        {
            get
            {
                return this._operationResultXML;
            }
            set
            {
                this._operationResultXML = value;
            }
        }

        /// <summary>
        ///  Property denoting type of Job
        /// </summary>
        [DataMember]
        public new ObjectType ObjectType
        {
            get
            {
                return this._jobObjectType;
            }
            set
            {
                this._jobObjectType = value;
            }
        }

        /// <summary>
        ///  Property denoting action performed on the object.
        /// </summary>
        [DataMember]
        public ObjectAction PerformedAction
        {
            get
            {
                return this._performedAction;
            }
            set
            {
                this._performedAction = value;
            }
        }
        #endregion

        #region Methods

        #region Load Methods

        ///<summary>
        /// Load Job Import result object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        public void LoadJobImportResult(String valuesAsXml)
        {
            
        }

        private void LoadOperationResultBasedOnJobType(String valuesAsXml)
        {
            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                if (this._jobObjectType == Core.ObjectType.Lookup)
                {
                    this.OperationResult = new OperationResult(valuesAsXml);
                }
                else
                {
                    if (this._jobObjectType == Core.ObjectType.DataModelMetadata)
                    {
                        this.OperationResult = new DataModelOperationResult(valuesAsXml);
                    }
                    else
                    {
                        this.OperationResult = new EntityOperationResult(valuesAsXml);
                    }
                }
            }
        }

        #endregion

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of Job Import Result
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml()
        {
            String jobXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //jobXml node start
            xmlWriter.WriteStartElement("JobImportResult");

            #region Write Properties

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("JobId", this.JobId.ToString());
            xmlWriter.WriteAttributeString("Status", this.Status);
            xmlWriter.WriteAttributeString("ObjectType", this.ObjectType.ToString());
            xmlWriter.WriteAttributeString("ExternalId", this.ExternalId);
            xmlWriter.WriteAttributeString("InternalId", this.InternalId.ToString());
            xmlWriter.WriteAttributeString("Description", this.Description);
            xmlWriter.WriteAttributeString("AuditRefId", this.AuditRefId.ToString());
            xmlWriter.WriteAttributeString("OperationResultXML", this.OperationResultXML.ToString());

            #endregion

            //Job node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            jobXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return jobXml;
        }

        /// <summary>
        /// Get Xml representation of JobData
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml(ObjectSerialization serialization)
        {
            // No serialization implemented for now...
            return this.ToXml();
        }

        #endregion

        #endregion
    }
}
