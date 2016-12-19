using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Linq;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.Interfaces.DataModel;
    using MDM.BusinessObjects.DataModel;
    using System.Collections;

    /// <summary>
    /// Represent class for data model operation result summary. This object can be used to communicate the result from 1 layer to another.
    /// </summary>
    [DataContract]
    public class DataModelOperationResultSummary : ObjectBase, IDataModelOperationResultSummary
    {
        #region Fields

        /// <summary>
        /// Object Type ("Organization" "EntityType" etc)
        /// </summary>
        String _objectType = String.Empty;

        /// <summary>
        /// Object Name ("S1 - Organization" "S5 - Entity Type" etc)
        /// </summary>

        String _summaryObjectName = String.Empty;

        /// <summary>
        /// Exteranl Id ("Import Summary" or RowId)
        /// </summary>
        String _externalId = String.Empty;

        /// <summary>
        /// Internal Id
        /// </summary>
        Int64 _internalId = -1;

       /// <summary>
        /// Property denoting total object count
        /// </summary>
        Int32 _totalCount = 0;

        /// <summary>
        /// Property denoting Pending object count
        /// </summary>
        Int32 _pendingCount = 0;

        /// <summary>
        /// Property denoting Success object count
        /// </summary>
        Int32 _successCount = 0;

        /// <summary>
        /// Property denoting Failed object count
        /// </summary>
        Int32 _failedCount = 0;

        /// <summary>
        /// Property denoting CompletedWithErrors object count
        /// </summary>
        Int32 _completedWithErrorsCount = 0;
        
        /// <summary>
        /// Property denoting CompletedWithWarnings object count
        /// </summary>
        Int32 _completedWithWarningsCount = 0;

        /// <summary>
        /// Status
        /// </summary>
        OperationResultStatusEnum _summaryStatus = OperationResultStatusEnum.None;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public DataModelOperationResultSummary()
        {
        }

        /// <summary>
        /// Initialize operation result from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for Operation result</param>
        public DataModelOperationResultSummary(String valuesAsXml)
        {
            #region Sample Xml
            /*
             <DataModelOperationResultSummary 
	            ObjectType="Taxonomy" 
	            ExternalId="ImportSummary" 
	            InternalId="-1001"
	            TotalCount="1" 
	            PendingCount="0" 
	            SuccessCount="1" 
	            FailedCount="0" 
	            CompletedWithErrorsCount="0" 
	            CompletedWithWarningsCount="0" />
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DataModelOperationResultSummary")
                        {
                            #region Read EntityOperationResult attributes

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

                                if (reader.MoveToAttribute("InternalId"))
                                {
                                    this.InternalId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
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

                            #endregion
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

        #region Properties

        /// <summary>
        /// OperationResultStatus
        /// </summary>
        public OperationResultStatusEnum SummaryStatus
        {
            get { return _summaryStatus; }
            set { _summaryStatus = value; }
        }


        /// <summary>
        /// Object Type ("Organization" "EntityType" etc)
        /// </summary>
        [DataMember]
        new public String ObjectType
        {
            get { return _objectType; }
            set { _objectType = value; }
        }


        /// <summary>
        /// Object Name ("S1 - Organization" "S5 - EntityType" etc)
        /// </summary>
        [DataMember]
        public String SummaryObjectName
        {
            get { return _summaryObjectName; }
            set { _summaryObjectName = value; }
        }
        
        /// 
        /// <summary>
        /// Exteranl Id ("Import Summary" or RowId)
        /// </summary>
        [DataMember]
        public String ExternalId
        {
            get { return _externalId; }
            set { _externalId = value; }
        }

        /// <summary>
        /// Internal Id
        /// </summary>
        public Int64  InternalId
        {
            get { return _internalId; }
            set { _internalId = value; }
        }

        /// <summary>
        /// Property denoting total object count
        /// </summary>
        [DataMember]
        public  Int32 ToatalCount
        {
            get { return _totalCount; }
            set { _totalCount = value; }
        }

        /// <summary>
        /// Property denoting Pending object count
        /// </summary>
        [DataMember]
        public  Int32 PendingCount
        {
            get { return _pendingCount; }
            set { _pendingCount = value; }
        }

        /// <summary>
        /// Property denoting Success object count
        /// </summary>
        [DataMember]
        public  Int32 SuccessCount
        {
            get { return _successCount; }
            set { _successCount = value; }
        }

        /// <summary>
        /// Property denoting Failed object count
        /// </summary>
        [DataMember]
        public  Int32 FailedCount
        {
            get { return _failedCount; }
            set { _failedCount = value; }
        }

        /// <summary>
        /// Property denoting CompletedWithErrors object count
        /// </summary>
        [DataMember]
        public  Int32 CompletedWithErrorsCount
        {
            get { return _completedWithErrorsCount; }
            set { _completedWithErrorsCount = value; }
        }

        /// <summary>
        /// Property denoting CompletedWithWarnings object count
        /// </summary>
        [DataMember]
        public  Int32 CompletedWithWarningsCount
        {
            get { return _completedWithWarningsCount; }
            set { _completedWithWarningsCount = value; }
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Get Xml representation of Operation Result
        /// </summary>
        /// <returns>Xml representation of Operation Result object</returns>
        public String ToXml()
        {
            String operationResultXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Operation result node start
            xmlWriter.WriteStartElement("DataModelOperationResultSummary");

            xmlWriter.WriteAttributeString("ObjectType", _objectType);
            xmlWriter.WriteAttributeString("SummaryObjectName", _summaryObjectName);
            xmlWriter.WriteAttributeString("ExternalId", _externalId);
            xmlWriter.WriteAttributeString("InternalId", _internalId.ToString());

            xmlWriter.WriteAttributeString("TotalCount", ToatalCount.ToString());
            xmlWriter.WriteAttributeString("PendingCount", _pendingCount.ToString());
            xmlWriter.WriteAttributeString("SuccessCount", SuccessCount.ToString());
            xmlWriter.WriteAttributeString("FailedCount", FailedCount.ToString());
            xmlWriter.WriteAttributeString("CompletedWithErrorsCount", CompletedWithErrorsCount.ToString());
            xmlWriter.WriteAttributeString("CompletedWithWarningsCount", CompletedWithWarningsCount.ToString());

            //Operation result node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            operationResultXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return operationResultXml;
        }

        /// <summary>
        /// Get Xml representation of DataModel operation result based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of DataModel operation result</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            return ToXml();
        }

        /// <summary>
        /// Adds operation summary information to object
        /// </summary>
        /// <param name="objectType">Indicates type of object type</param>
        /// <param name="externalId">Indicates external identifier</param>
        /// <param name="internalId">Indicates internal identifier</param>
        public void AddSummaryInformation(String objectType, String externalId, Int64 internalId)
        {
            this._objectType = objectType;
            this._externalId = externalId;
            this._internalId = internalId;
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
        public void UpdateSummaryCounts(
                                    Int32 total,
                                    Int32 pending,
                                    Int32 succeeded,
                                    Int32 failed,
                                    Int32 completedWithWarnings,
                                    Int32 completedWithErrors
                                 )
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
            if (PendingCount == 0)
            {
                SummaryStatus = OperationResultStatusEnum.Successful;

                if (CompletedWithErrorsCount > 1 || FailedCount > 1)
                {
                    SummaryStatus = OperationResultStatusEnum.CompletedWithErrors;
                }
                else if (CompletedWithWarningsCount > 1)
                {
                    SummaryStatus = OperationResultStatusEnum.CompletedWithWarnings;
                }
            }
            else
            {
                SummaryStatus = OperationResultStatusEnum.Pending;
            }

        }
        #endregion
    }
}