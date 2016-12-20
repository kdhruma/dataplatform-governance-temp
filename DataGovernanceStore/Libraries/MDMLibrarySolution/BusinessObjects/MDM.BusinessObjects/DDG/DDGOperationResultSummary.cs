using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    using ProtoBuf;

    /// <summary>
    /// Represents the class for DDGOperationResult Summary
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class DDGOperationResultSummary : IDDGOperationResultSummary
    {
        #region Fields

        /// <summary>
        /// Field denoting the DDG object type
        /// </summary>
        private String _objectType = String.Empty;

        /// <summary>
        /// Field denoting the DDG object name
        /// </summary>
        private String _objectName = String.Empty;

        /// <summary>
        /// Field denoting the external Id
        /// </summary>
        private String _externalId = String.Empty;

        /// <summary>
        /// Field denoting total object count
        /// </summary>
        private Int32 _totalCount = 0;

        /// <summary>
        /// Field denoting pending object count
        /// </summary>
        private Int32 _pendingCount = 0;

        /// <summary>
        /// Field denoting success object count
        /// </summary>
        private Int32 _successCount = 0;

        /// <summary>
        /// Field denoting failed object count
        /// </summary>
        private Int32 _failedCount = 0;

        /// <summary>
        /// Field denoting completedwitherrors object count
        /// </summary>
        private Int32 _completedWithErrorsCount = 0;

        /// <summary>
        /// Field denoting completedwithwarnings object count
        /// </summary>
        private Int32 _completedWithWarningsCount = 0;

        /// <summary>
        /// Field denoting overall status of Operation
        /// </summary>
        private OperationResultStatusEnum _summaryStatus = OperationResultStatusEnum.None;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denoting the DDG Object Type
        /// </summary>
        [DataMember]
        public String ObjectType
        {
            get { return _objectType; }
            set { _objectType = value; }
        }

        /// <summary>
        /// Property denoting the DDG Object Name
        /// </summary>
        [DataMember]
        public String ObjectName
        {
            get { return _objectName; }
            set { _objectName = value; }
        }

        /// <summary>
        /// Property denoting the External Id
        /// </summary>
        [DataMember]
        public String ExternalId
        {
            get { return _externalId; }
            set { _externalId = value; }
        }

        /// <summary>
        /// Property denoting the total object count
        /// </summary>
        [DataMember]
        public Int32 ToatalCount
        {
            get { return _totalCount; }
            set { _totalCount = value; }
        }

        /// <summary>
        /// Property denoting the pending object count
        /// </summary>
        [DataMember]
        public Int32 PendingCount
        {
            get { return _pendingCount; }
            set { _pendingCount = value; }
        }

        /// <summary>
        /// Property denoting the success object count
        /// </summary>
        [DataMember]
        public Int32 SuccessCount
        {
            get { return _successCount; }
            set { _successCount = value; }
        }

        /// <summary>
        /// Property denoting the failed object count
        /// </summary>
        [DataMember]
        public Int32 FailedCount
        {
            get { return _failedCount; }
            set { _failedCount = value; }
        }

        /// <summary>
        /// Property denoting the completedwitherrors object count
        /// </summary>
        [DataMember]
        public Int32 CompletedWithErrorsCount
        {
            get { return _completedWithErrorsCount; }
            set { _completedWithErrorsCount = value; }
        }

        /// <summary>
        /// Property denoting the completedwithwarnings object count
        /// </summary>
        [DataMember]
        public Int32 CompletedWithWarningsCount
        {
            get { return _completedWithWarningsCount; }
            set { _completedWithWarningsCount = value; }
        }

        /// <summary>
        /// Property denoting overall status of Operation
        /// </summary>
        [DataMember]
        public OperationResultStatusEnum SummaryStatus
        {
            get { return _summaryStatus; }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DDGOperationResultSummary()
        {

        }

        /// <summary>
        /// Initialize operation result from Xml
        /// </summary>
        /// <param name="valuesAsXml">Object as Xml</param>
        public DDGOperationResultSummary(String valuesAsXml)
        {
            LoadOperationResultSummaryFromXml(valuesAsXml);
        }

        #endregion Constructor

        #region Methods

        #region Public Methods

        /// <summary>
        /// Adds operation summary information to object
        /// </summary>
        /// <param name="objectType">Indicates type of object type</param>
        /// <param name="objectName">Indicates type of object name</param>
        /// <param name="externalId">Indicates external identifier</param>
        /// <param name="internalId">Indicates internal identifier</param>
        public void AddObjectInformation(String objectType, String objectName, String externalId, Int64 internalId)
        {
            this._objectType = objectType;
            this._objectName = objectName;
            this._externalId = externalId;
        }

        /// <summary>
        /// Update summary with current number
        /// </summary>
        /// <param name="total">Indicates total count</param>
        /// <param name="pending">Indicates pending count</param>
        /// <param name="succeeded">Indicates the number of results succeeded</param>
        /// <param name="failed">Indicates the number of results failed</param>
        /// <param name="completedWithWarnings">Indicates the number of results completed with warnings</param>
        /// <param name="completedWithErrors">Indicates the number of results completed with errors</param>
        public void UpdateSummaryCounts(Int32 total, Int32 pending, Int32 succeeded, Int32 failed, Int32 completedWithWarnings, Int32 completedWithErrors)
        {
            this.ToatalCount += total;
            this.PendingCount += pending;
            this.SuccessCount += succeeded;
            this.FailedCount += failed;
            this.CompletedWithErrorsCount += completedWithErrors;
            this.CompletedWithWarningsCount += completedWithWarnings;
        }

        /// <summary>
        /// Update the overall summary status based on the status count
        /// </summary>
        public void UpdateSummaryStatus()
        {
            // Todo ..
        }

        /// <summary>
        /// Get Xml representation of Operation Result
        /// </summary>
        /// <returns>Xml representation of Operation Result object</returns>
        public String ToXml()
        {
            String operationResultXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    //Operation result node start
                    xmlWriter.WriteStartElement("DDGOperationResultSummary");

                    xmlWriter.WriteAttributeString("ObjectType", this._objectType);
                    xmlWriter.WriteAttributeString("SummaryObjectName", this._objectName);
                    xmlWriter.WriteAttributeString("ExternalId", this._externalId);

                    xmlWriter.WriteAttributeString("TotalCount", this._totalCount.ToString());
                    xmlWriter.WriteAttributeString("PendingCount", this._pendingCount.ToString());
                    xmlWriter.WriteAttributeString("SuccessCount", this._successCount.ToString());
                    xmlWriter.WriteAttributeString("FailedCount", this._failedCount.ToString());
                    xmlWriter.WriteAttributeString("CompletedWithErrorsCount", this._completedWithErrorsCount.ToString());
                    xmlWriter.WriteAttributeString("CompletedWithWarningsCount", this._completedWithWarningsCount.ToString());

                    //Operation result node end
                    xmlWriter.WriteEndElement();

                    //Get the actual XML
                    operationResultXml = sw.ToString();
                }
            }

            return operationResultXml;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Load DDG OperationResult Summary from Xml
        /// </summary>
        /// <param name="valuesAsXml">Object as Xml</param>
        private void LoadOperationResultSummaryFromXml(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DDGOperationResultSummary")
                        {
                            #region Read DDGOperationResultSummary attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("ObjectType"))
                                {
                                    this.ObjectType = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ExternalId"))
                                {
                                    this.ExternalId = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("TotalCount"))
                                {
                                    this.ToatalCount = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("PendingCount"))
                                {
                                    this.PendingCount = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("SuccessCount"))
                                {
                                    this.SuccessCount = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("FailedCount"))
                                {
                                    this.FailedCount = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("CompletedWithErrorsCount"))
                                {
                                    this.CompletedWithErrorsCount = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("CompletedWithWarningsCount"))
                                {
                                    this.CompletedWithWarningsCount = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                            }

                            #endregion Read DDGOperationResultSummary attributes
                        }
                        else
                        {
                            //Keep on reading the xml until we reach expected node.
                            reader.Read();
                        }
                    }
                }
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}
