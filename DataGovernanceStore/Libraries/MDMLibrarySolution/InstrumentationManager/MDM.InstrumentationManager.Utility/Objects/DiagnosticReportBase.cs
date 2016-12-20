using System;
using System.IO;
using System.Xml;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

namespace MDM.BusinessObjects.Diagnostics
{
    using MDM.Core;
    using MDM.Interfaces.Diagnostics;

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    [ProtoBuf.ProtoContract]
    public class DiagnosticReportBase : ObjectBase, IDiagnosticReportBase
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private DateTime _executionDateTime;

        /// <summary>
        /// 
        /// </summary>
        private DiagnosticRecordCollection _diagnosticRecords = new DiagnosticRecordCollection();
        
        /// <summary>
        /// Main activity of the report (only 1 supported)
        /// </summary>
        private DiagnosticActivity _diagnosticActivity = null;

        /// <summary>
        /// 
        /// </summary>
        private string _mainOperation;

        /// <summary>
        /// Sync object
        /// </summary>
        private readonly object _syncObject = new object();
        
        #endregion

        #region Constructors

        /// <summary>
        /// Paramterless Constructor
        /// </summary>
        public DiagnosticReportBase()
        { 
        
        }

        #endregion
        
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(1)]
        public DateTime ExecutionDateTime
        {
            get { return _executionDateTime; }
            set { _executionDateTime = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(2)]
        public DiagnosticRecordCollection DiagnosticRecords
        {
            get { return _diagnosticRecords; }
            set { _diagnosticRecords = value; }
        }

        /// <summary>
        /// Diagnostic Activity
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(3)]
        public DiagnosticActivity Activity
        {
            get { return _diagnosticActivity; }
            set { _diagnosticActivity = value; }
        }

        /// <summary>
        /// Property denoting the main operation that this report is capturing diagnostic report for
        /// </summary>
        [DataMember]
        [ProtoBuf.ProtoMember(4)]
        public String MainOperation 
        {
            get { return _mainOperation; }
            set { _mainOperation = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="referenceId"></param>
        /// <param name="stepNumber"></param>
        /// <param name="parentActivityName"></param>
        /// <param name="activityName"></param>
        /// <param name="messageClass"></param>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        public void AddDiagnosticRecord(Int64 referenceId, String stepNumber, String parentActivityName, String activityName, MessageClassEnum messageClass, String messageCode, String message)
        {
            this.AddDiagnosticRecordInternal(referenceId, stepNumber, parentActivityName, activityName, messageClass, messageCode, message, null,  0, String.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="referenceId"></param>
        /// <param name="stepNumber"></param>
        /// <param name="parentActivityName"></param>
        /// <param name="activityName"></param>
        /// <param name="messageClass"></param>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        /// <param name="durationInMilliseconds"></param>
        public void AddDiagnosticRecord(Int64 referenceId, String stepNumber, String parentActivityName, String activityName, MessageClassEnum messageClass, String messageCode, String message, Double durationInMilliseconds)
        {
            this.AddDiagnosticRecordInternal(referenceId, stepNumber, parentActivityName, activityName, messageClass, messageCode, message, null, durationInMilliseconds, String.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="referenceId"></param>
        /// <param name="stepNumber"></param>
        /// <param name="parentActivityName"></param>
        /// <param name="activityName"></param>
        /// <param name="messageClass"></param>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        /// <param name="messageParameters"></param>
        /// <param name="durationInMilliseconds"></param>
        public void AddDiagnosticRecord(Int64 referenceId, String stepNumber, String parentActivityName, String activityName, MessageClassEnum messageClass, String messageCode, String message, Collection<String> messageParameters, Double durationInMilliseconds)
        {
            this.AddDiagnosticRecordInternal(referenceId, stepNumber, parentActivityName, activityName, messageClass, messageCode, message, messageParameters, durationInMilliseconds, String.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="referenceId"></param>
        /// <param name="stepNumber"></param>
        /// <param name="parentActivityName"></param>
        /// <param name="activityName"></param>
        /// <param name="messageClass"></param>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        /// <param name="messageParameters"></param>
        /// <param name="dataXml"></param>
        public void AddDiagnosticRecord(Int64 referenceId, String stepNumber, String parentActivityName, String activityName, MessageClassEnum messageClass, String messageCode, String message, Collection<String> messageParameters, String dataXml)
        {
            this.AddDiagnosticRecordInternal(referenceId, stepNumber, parentActivityName, activityName, messageClass, messageCode, message, messageParameters, 0, dataXml);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="referenceId"></param>
        /// <param name="stepNumber"></param>
        /// <param name="parentActivityName"></param>
        /// <param name="activityName"></param>
        /// <param name="messageClass"></param>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        /// <param name="durationInMilliseconds"></param>
        /// <param name="threadNumber"></param>
        /// <param name="threadId"></param>
        public void AddDiagnosticRecord(Int64 referenceId, String stepNumber, String parentActivityName, String activityName, MessageClassEnum messageClass, String messageCode, String message, Double durationInMilliseconds, Int32 threadNumber, Int32 threadId)
        {
            this.AddDiagnosticRecordInternal(referenceId, stepNumber, parentActivityName, activityName, messageClass, messageCode, message, null, durationInMilliseconds, String.Empty, threadNumber, threadId);
        }

        /// <summary>
        /// Adds given DiagnoticReport to current report.
        /// </summary>
        /// <param name="report">DiagnosticReport</param>
        public void AddDiagnosticReport(DiagnosticReportBase report)
        {
            if (report == null)
            {
                return;
            }

            foreach (DiagnosticRecord record in report.DiagnosticRecords)
            {
                this.DiagnosticRecords.Add(record);
            }

            // Todo - make this threadsafe
            if (_diagnosticActivity == null)
            {
                _diagnosticActivity = report.GetRootActivity();
            }
            else
            {
                _diagnosticActivity.AddDiagnosticActivity(report.GetRootActivity());
            }
        }

        /// <summary>
        /// Creates and/or returns the root activity of the DiagnosticActivity
        /// </summary>
        /// <param name="activityName"></param>
        /// <param name="executionContext"></param>
        /// <param name="referenceId"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public DiagnosticActivity GetRootActivity([CallerMemberName] String activityName = "", ExecutionContext executionContext = null, Int64 referenceId = 1, [CallerFilePath] String filePath = "")
        {
            // Todo - make this threadsafe
            if (_diagnosticActivity == null)
            {
                _diagnosticActivity = new DiagnosticActivity(executionContext, activityName, referenceId, filePath);
            }

            return _diagnosticActivity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="referenceId"></param>
        /// <param name="stepNumber"></param>
        /// <param name="parentActivityName"></param>
        /// <param name="activityName"></param>
        /// <param name="messageClass"></param>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        /// <param name="messageParameters"></param>
        /// <param name="durationInMilliseconds"></param>
        /// <param name="dataXml"></param>
        /// <param name="threadNumber"></param>
        /// <param name="threadId"></param>
        private void AddDiagnosticRecordInternal(Int64 referenceId, String stepNumber, String parentActivityName, String activityName, MessageClassEnum messageClass, String messageCode, String message, Collection<String> messageParameters, Double durationInMilliseconds, String dataXml, Int32 threadNumber = -1, Int32 threadId = -1)
        {
            lock (_syncObject)
            {
                this._diagnosticRecords.Add(new DiagnosticRecord(referenceId, stepNumber, messageClass, messageCode, message, messageParameters, durationInMilliseconds, dataXml, threadNumber, threadId));
            }
        }

        /// <summary>
        /// Gets Xml representation of Diagnostic Report Base
        /// </summary>
        /// <returns>Xml representation of Diagnostic Report Base</returns>
        public String ToXml()
        {
            String diagnosticReportXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("DiagnosticReport");

            if (this.DiagnosticRecords != null && this.DiagnosticRecords.Count > 0)
            {
                xmlWriter.WriteRaw(this.DiagnosticRecords.ToXml());
            }

            if (this.Activity != null)
            {
                xmlWriter.WriteRaw(this.Activity.ToXml());
            }

            //Diagnostic Report node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            diagnosticReportXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return diagnosticReportXml;
        }

        #endregion
    }
}
