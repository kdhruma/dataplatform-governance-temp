using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;
using ProtoBuf;

namespace MDM.BusinessObjects.Diagnostics
{
    using Core;
    using MDM.Interfaces.Diagnostics;
    using MDM.Core.Extensions;

    /// <summary>
    /// Business Class for Diagnostic Report settings
    /// </summary>
    [DataContract]
    [ProtoContract]
    [Serializable()]
    public class DiagnosticReportSettings : IDiagnosticReportSettings
    {
        #region Fields

        /// <summary>
        /// Field denoting DataRequestType for diagnostic.
        /// <para>
        /// Default value is set to 0 which stands for System Traces load
        /// </para>
        /// </summary>
        private Int32 _dataRequestType = 0;

        /// <summary>
        /// Field denoting level of the diagnostic Activity
        /// </summary>
        private Int32 _level = -1;

        /// <summary>
        /// Field denoting collection of message class
        /// </summary>
        private Collection<MessageClassEnum> _messageClasses = new Collection<MessageClassEnum>();

        /// <summary>
        /// Field denoting duration of an activity
        /// </summary>
        private Int32? _duration = null;
        
        /// <summary>
        /// Field denoting search operator for an Activity duration
        /// </summary>
        private SearchOperator? _durationOperator = null;

        /// <summary>
        /// Field denoting keywords to search in diagnostic activities
        /// </summary>
        private Collection<String> _searchKeywords = new Collection<String>();

        /// <summary>
        /// Field denoting search columns to search in
        /// </summary>
        private Collection<SearchColumn> _searchColumns = new Collection<SearchColumn>();

        /// <summary>
        /// Field denoting maximum record to return 
        /// </summary>
        private Int32? _maxRecordsToReturn = null;

        /// <summary>
        /// Flag denoting to include/not include 'Data' in Diagnostic record 
        /// </summary>
        private Boolean _includeActivityExtendedData = true;

        /// <summary>
        /// Flag denoting to include/not include Data Row in Diagnostic record 
        /// </summary>
        private Boolean? _hasActivityExtendedData = null;

        /// <summary>
        /// Field denoting CallerContext filter
        /// </summary>
        private CallerContextFilter _callerContextFilter = new CallerContextFilter();

        /// <summary>
        /// Field denoting SecurityContext filter
        /// </summary>
        private SecurityContextFilter _securityContextFilter = new SecurityContextFilter();

        /// <summary>
        /// Field denoting CallDataContext filter
        /// </summary>
        private CallDataContext _сallDataContext = new CallDataContext();

        /// <summary>
        /// Field denoting legacy MDM trace source filter
        /// </summary>
        private Collection<MDMTraceSource> _legacyMDMTraceSources = new Collection<MDMTraceSource>();

        /// <summary>
        /// Field denoting FromDateTime filter
        /// </summary>
        private DateTime? _fromDateTime = null;
        
        /// <summary>
        /// Field denoting ToDateTime filter
        /// </summary>
        private DateTime? _toDateTime = null;

        /// <summary>
        /// Field denoting substrings to search in diagnostic records message properties
        /// </summary>
        private Collection<String> _messages = new Collection<String>();

        /// <summary>
        /// Field denoting thread Ids filter
        /// </summary>
        private Collection<Int32> _threadIds = new Collection<Int32>();

        /// <summary>
        /// Field denoting whether to include/not include execution context columns (except of AdditionalContextData column, please use <see cref="IncludeExecutionContextExtendedData"/> property for this column requesting) in diagnostic record
        /// </summary>
        private Boolean _includeContextData = false;

        /// <summary>
        /// Field denoting whether to include/not include AdditionalContextData column in diagnostic record
        /// </summary>
        private Boolean _includeExecutionContextExtendedData = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DiagnosticReportSettings()
        {

        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DiagnosticReportSettings(String valueAsXml)
        {
            LoadDiagnosticReportSettings(valueAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting DataRequestType for diagnostic
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public Int32 DataRequestType
        {
            get { return this._dataRequestType; }
            set { this._dataRequestType = value; }
        }

        /// <summary>
        /// Property denoting level of the diagnostic Activity
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public Int32 Level
        {
            get { return this._level; }
            set { this._level = value; }
        }

        /// <summary>
        /// Property denoting collection of message class
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public Collection<MessageClassEnum> MessageClasses
        {
            get { return this._messageClasses; }
            set { this._messageClasses = value; }
        }

        /// <summary>
        /// Property denoting duration of an activity
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        public Int32? Duration
        {
            get { return this._duration; }
            set { this._duration = value; }
        }

        /// <summary>
        /// Property denoting search operator for an Activity duration
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        public SearchOperator? DurationOperator
        {
            get { return this._durationOperator; }
            set { this._durationOperator = value; }
        }

        /// <summary>
        /// Property denoting keywords to search in diagnostic activities. Please also set <see cref="DiagnosticReportSettings.SearchColumns"/> list as scope specification for this kind of search (keyword based).
        /// </summary>
        [DataMember]
        [ProtoMember(6)]
        public Collection<String> SearchKeywords
        {
            get { return this._searchKeywords; }
            set { this._searchKeywords = value; }
        }

        /// <summary>
        /// Property denoting search columns to search in during keyword based search. Please also set <see cref="DiagnosticReportSettings.SearchKeywords"/>.
        /// </summary>
        [DataMember]
        [ProtoMember(7)]
        public Collection<SearchColumn> SearchColumns
        {
            get { return this._searchColumns; }
            set { this._searchColumns = value; }
        }

        /// <summary>
        /// Property denoting maximum record to return 
        /// </summary>
        [DataMember]
        [ProtoMember(8)]
        public Int32? MaxRecordsToReturn
        {
            get { return this._maxRecordsToReturn; }
            set { this._maxRecordsToReturn = value; }
        }

        /// <summary>
        /// Property denoting whether to include/not include extended Data column in diagnostic record 
        /// </summary>
        [DataMember]
        [ProtoMember(9)]
        public Boolean IncludeActivityExtendedData
        {
            get { return this._includeActivityExtendedData; }
            set { this._includeActivityExtendedData = value; }
        }
        
        /// <summary>
        /// Property denoting filter based on Activity Extended Data existance. Please set to <c>True</c> if you want to see only items with extended data.
        /// Please set to <c>False</c> if you want to see only items without extended data.
        /// Please set to <c>null</c> if you want to ignore extended data existance.
        /// </summary>
        [DataMember]
        [ProtoMember(10)]
        public Boolean? HasActivityExtendedData
        {
            get { return this._hasActivityExtendedData; }
            set { this._hasActivityExtendedData = value; }
        }

        /// <summary>
        /// Property denoting CallerContext filter
        /// </summary>
        [DataMember]
        [ProtoMember(11)]
        public CallerContextFilter CallerContextFilter
        {
            get { return _callerContextFilter; }
            set { _callerContextFilter = value; }
        }

        /// <summary>
        /// Property denoting SecurityContext filter
        /// </summary>
        [DataMember]
        [ProtoMember(12)]
        public SecurityContextFilter SecurityContextFilter
        {
            get { return _securityContextFilter; }
            set { _securityContextFilter = value; }
        }

        /// <summary>
        /// Property denoting CallDataContext filter
        /// </summary>
        [DataMember]
        [ProtoMember(13)]
        public CallDataContext CallDataContext
        {
            get { return _сallDataContext; }
            set { _сallDataContext = value; }
        }

        /// <summary>
        /// Property denoting legacy MDM trace source filter
        /// </summary>
        [DataMember]
        [ProtoMember(14)]
        public Collection<MDMTraceSource> LegacyMDMTraceSources
        {
            get { return _legacyMDMTraceSources; }
            set { _legacyMDMTraceSources = value; }
        }

        /// <summary>
        /// Property denoting FromDateTime filter
        /// </summary>
        [DataMember]
        [ProtoMember(15)]
        public DateTime? FromDateTime
        {
            get { return _fromDateTime; }
            set { _fromDateTime = value; }
        }

        /// <summary>
        /// Property denoting ToDateTime filter
        /// </summary>
        [DataMember]
        [ProtoMember(16)]
        public DateTime? ToDateTime
        {
            get { return _toDateTime; }
            set { _toDateTime = value; }
        }

        /// <summary>
        /// Property denoting substrings to search in diagnostic record message properties
        /// </summary>
        [DataMember]
        [ProtoMember(17)]
        public Collection<String> Messages
        {
            get { return this._messages; }
            set { this._messages = value; }
        }

        /// <summary>
        /// Property denoting thread Ids filter
        /// </summary>
        [DataMember]
        [ProtoMember(18)]
        public Collection<Int32> ThreadIds
        {
            get { return this._threadIds; }
            set { this._threadIds = value; }
        }

        /// <summary>
        /// Property denoting whether to include/not include execution context columns (except of AdditionalContextData column, please use <see cref="IncludeExecutionContextExtendedData"/> property for this column requesting) in diagnostic record
        /// </summary>
        [DataMember]
        [ProtoMember(19)]
        public Boolean IncludeContextData
        {
            get { return this._includeContextData; }
            set { this._includeContextData = value; }
        }

        /// <summary>
        /// Property denoting whether to include/not include AdditionalContextData column in diagnostic record
        /// </summary>
        [DataMember]
        [ProtoMember(20)]
        public Boolean IncludeExecutionContextExtendedData
        {
            get { return this._includeExecutionContextExtendedData; }
            set { this._includeExecutionContextExtendedData = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get XML representation of DiagnosticReportSettings
        /// </summary>
        /// <returns>XML representation of DiagnosticReportSettings</returns>
        public String ToXml()
        {
            String diagnosticReportSettingsXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //DiagnosticReportSettings Map node start
            xmlWriter.WriteStartElement("DiagnosticReportSettings");

            xmlWriter.WriteAttributeString("DataRequestType", this.DataRequestType.ToString());
            xmlWriter.WriteAttributeString("Duration", this.Duration.HasValue ? this.Duration.Value.ToString() : "");
            xmlWriter.WriteAttributeString("DurationOperator", this.DurationOperator.HasValue ? this.DurationOperator.ToString() : "");
            xmlWriter.WriteAttributeString("Level", this.Level.ToString());
            xmlWriter.WriteAttributeString("MaxRecordsToReturn", this.MaxRecordsToReturn.HasValue ? this.MaxRecordsToReturn.Value.ToString() : "");
            xmlWriter.WriteAttributeString("IncludeActivityExtendedData", this.IncludeActivityExtendedData.ToString());
            xmlWriter.WriteAttributeString("IncludeContextData", this.IncludeContextData.ToString());
            xmlWriter.WriteAttributeString("IncludeExecutionContextExtendedData", this.IncludeExecutionContextExtendedData.ToString());
            xmlWriter.WriteAttributeString("HasActivityExtendedData", this.HasActivityExtendedData.HasValue ? this.HasActivityExtendedData.Value.ToString() : "");
            xmlWriter.WriteAttributeString("MessageClasses", ValueTypeHelper.JoinCollection(this.MessageClasses, "|"));
            
            xmlWriter.WriteStartElement("SearchColumns");
            if (this.SearchColumns != null && this.SearchColumns.Count > 0)
            {
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection<SearchColumn>(this.SearchColumns, "|"));
            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("SearchKeywords");
            if (this.SearchKeywords != null)
            {
                foreach (String keyword in this.SearchKeywords)
                {
                    xmlWriter.WriteStartElement("Keyword");
                    xmlWriter.WriteRaw(keyword.WrapWithCDataBlock());
                    xmlWriter.WriteEndElement();
                }
            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Messages");
            if (this.Messages != null)
            {
                foreach (String message in this.Messages)
                {
                    xmlWriter.WriteStartElement("Message");
                    xmlWriter.WriteRaw(message.WrapWithCDataBlock());
                    xmlWriter.WriteEndElement();
                }
            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteRaw(this.CallDataContext.ToXml());

            xmlWriter.WriteRaw(this.CallerContextFilter.ToXml());

            xmlWriter.WriteRaw(this.SecurityContextFilter.ToXml());

            xmlWriter.WriteStartElement("LegacyMDMTraceSources");
            if (this.LegacyMDMTraceSources != null && this.LegacyMDMTraceSources.Count > 0)
            {
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection<MDMTraceSource>(this.LegacyMDMTraceSources, "|"));
            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ThreadIds");
            if (this.ThreadIds != null && this.ThreadIds.Count > 0)
            {
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.ThreadIds, "|"));
            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("FromDateTime");
            if (this.FromDateTime.HasValue)
            {
                xmlWriter.WriteRaw(this.FromDateTime.Value.ToLocalTime().ToString(Constants.DATETIME_IN_24_HOURS_FORMAT, CultureInfo.InvariantCulture));
            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ToDateTime");
            if (this.ToDateTime.HasValue)
            {
                xmlWriter.WriteRaw(this.ToDateTime.Value.ToLocalTime().ToString(Constants.DATETIME_IN_24_HOURS_FORMAT, CultureInfo.InvariantCulture));
            }
            xmlWriter.WriteEndElement();

            //node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            diagnosticReportSettingsXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return diagnosticReportSettingsXml;
        }

        #endregion

        #region Private Methods

        private void LoadDiagnosticReportSettings(String diagnosticReportSettingsAsXml)
        {
            #region Sample XML

            #endregion Sample XML

            if (!String.IsNullOrWhiteSpace(diagnosticReportSettingsAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(diagnosticReportSettingsAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType != XmlNodeType.Element)
                        {
                            reader.Read();
                            continue;
                        }
                        if (reader.Name == "DiagnosticReportSettings")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("DataRequestType"))
                                {
                                    this.DataRequestType = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.DataRequestType);
                                }

                                if (reader.MoveToAttribute("Duration"))
                                {
                                    this.Duration = ValueTypeHelper.ConvertToNullableInt32(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("DurationOperator"))
                                {
                                    SearchOperator searchOperator = SearchOperator.Contains;
                                    if (Enum.TryParse<SearchOperator>(reader.ReadContentAsString(), true, out searchOperator))
                                    {
                                        this.DurationOperator = searchOperator;
                                    }
                                }

                                if (reader.MoveToAttribute("Level"))
                                {
                                    this.Level = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.Level);
                                }

                                if (reader.MoveToAttribute("MaxRecordsToReturn"))
                                {
                                    this.MaxRecordsToReturn = ValueTypeHelper.ConvertToNullableInt32(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("IncludeActivityExtendedData"))
                                {
                                    this.IncludeActivityExtendedData = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this.IncludeActivityExtendedData);
                                }

                                if (reader.MoveToAttribute("IncludeContextData"))
                                {
                                    this.IncludeContextData = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this.IncludeContextData);
                                }
                                
                                if (reader.MoveToAttribute("IncludeExecutionContextExtendedData"))
                                {
                                    this.IncludeExecutionContextExtendedData = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this.IncludeExecutionContextExtendedData);
                                }

                                if (reader.MoveToAttribute("HasActivityExtendedData"))
                                {
                                    this.HasActivityExtendedData = ValueTypeHelper.ConvertToNullableBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("MessageClasses"))
                                {
                                    String messageClasses = reader.ReadContentAsString();
                                    this.MessageClasses = new Collection<MessageClassEnum>();
                                    if (!String.IsNullOrWhiteSpace(messageClasses))
                                    {
                                        String[] strOutput = messageClasses.Split('|');
                                        foreach (String s in strOutput)
                                        {
                                            if (String.IsNullOrWhiteSpace(s) || s == "StaticText")
                                            {
                                                // Deprecated "StaticText" item should be skipped
                                                continue;
                                            }
                                            MessageClassEnum result = default(MessageClassEnum);
                                            if (Enum.TryParse(s, out result))
                                            {
                                                this.MessageClasses.Add(result);
                                            }
                                        }
                                    }
                                }
                            }
                            reader.Read();
                        }
                        else if (reader.Name == "SearchColumns")
                        {
                            String value = reader.ReadElementContentAsString();
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                this.SearchColumns =
                                    ValueTypeHelper.SplitStringToEnumCollection<SearchColumn>(value, '|');
                            }
                        }
                        else if (reader.Name == "SearchKeywords")
                        {
                            reader.Read();
                        }
                        else if (reader.Name == "SearchKeyword")
                        {
                            String value = reader.ReadElementContentAsString();
                            if (!this.SearchKeywords.Contains(value))
                            {
                                this.SearchKeywords.Add(value);
                            }
                        }
                        else if (reader.Name == "Messages")
                        {
                            reader.Read();
                        }
                        else if (reader.Name == "Message")
                        {
                            String value = reader.ReadElementContentAsString();
                            if (!this.Messages.Contains(value))
                            {
                                this.Messages.Add(value);
                            }
                        }
                        else if (reader.Name == "FromDateTime")
                        {
                            String fromDateTimeAsString = reader.ReadElementContentAsString();
                            DateTime date;
                            if (DateTime.TryParseExact(fromDateTimeAsString, Constants.DATETIME_IN_24_HOURS_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal, out date))
                            {
                                this.FromDateTime = date.ToLocalTime();
                            }
                            else
                            {
                                this.FromDateTime = null;
                            }
                        }
                        else if (reader.Name == "ToDateTime")
                        {
                            String toDateTimeAsString = reader.ReadElementContentAsString();
                            DateTime date;
                            if (DateTime.TryParseExact(toDateTimeAsString, Constants.DATETIME_IN_24_HOURS_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal, out date))
                            {
                                this.ToDateTime = date.ToLocalTime();
                            }
                            else
                            {
                                this.ToDateTime = DateTime.MaxValue;
                            }
                        }
                        else if (reader.Name == "CallDataContext")
                        {
                            this.CallDataContext = new CallDataContext(reader.ReadOuterXml());
                        }
                        else if (reader.Name == "CallerContextFilter")
                        {
                            this.CallerContextFilter = new CallerContextFilter(reader.ReadOuterXml());
                        }
                        else if (reader.Name == "SecurityContextFilter")
                        {
                            this.SecurityContextFilter = new SecurityContextFilter(reader.ReadOuterXml());
                        }
                        else if (reader.Name == "LegacyMDMTraceSources")
                        {
                            String legacyTraceSources = reader.ReadElementContentAsString();
                            if (!String.IsNullOrWhiteSpace(legacyTraceSources))
                            {
                                this.LegacyMDMTraceSources =
                                    ValueTypeHelper.SplitStringToEnumCollection<MDMTraceSource>(legacyTraceSources, '|');
                            }
                        }
                        else if (reader.Name == "ThreadIds")
                        {
                            String threadIds = reader.ReadElementContentAsString();
                            if (!String.IsNullOrWhiteSpace(threadIds))
                            {
                                this.ThreadIds =
                                    ValueTypeHelper.SplitStringToIntCollection(threadIds, '|');
                            }
                        }
                        else
                        {
                            //Keep on reading the XML until we reach expected node.
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
    }

    /// <summary>
    /// Specifies SearchColumns for Diagnostic Activity searching functionality related to Diagnostic Report generating
    /// </summary>
    public enum SearchColumn
    {
        // Warning! Please do not rename enum members (because they represents values used in report's stored procdure)

        /// <summary>
        /// Specifies Search in All columns
        /// </summary>
        [EnumMember]
        [Description("All columns")]
        All = 0,

        /// <summary>
        /// Specifies Message column
        /// </summary>
        [EnumMember]
        [Description("Message")]
        Message = 1,

        /// <summary>
        /// Specifies ThreadId column
        /// </summary>
        [EnumMember]
        [Description("Thread Id")]
        ThreadId = 2,

        /// <summary>
        /// Specifies MessageClass column
        /// </summary>
        [EnumMember]
        [Description("Message Class")]
        MessageClass = 3,

        /// <summary>
        /// Specifies LegacyTraceSource column
        /// </summary>
        [EnumMember]
        [Description("Legacy Trace Source")]
        LegacyTraceSource = 4,

        /// <summary>
        /// Specifies UserId column
        /// </summary>
        [EnumMember]
        [Description("User Id")]
        UserId = 5,

        /// <summary>
        /// Specifies UserName column
        /// </summary>
        [EnumMember]
        [Description("User Name")]
        UserName = 6,

        /// <summary>
        /// Specifies UserRoleId column
        /// </summary>
        [EnumMember]
        [Description("User Role Id")]
        UserRoleId = 7,

        /// <summary>
        /// Specifies UserRoleName column
        /// </summary>
        [EnumMember]
        [Description("User Role Name")]
        UserRoleName = 8,

        /// <summary>
        /// Specifies OrganizationId column
        /// </summary>
        [EnumMember]
        [Description("Organization Id")]
        OrganizationId = 9,

        /// <summary>
        /// Specifies ContainerId column
        /// </summary>
        [EnumMember]
        [Description("Container Id")]
        ContainerId = 10,

        /// <summary>
        /// Specifies EntityTypeId column
        /// </summary>
        [EnumMember]
        [Description("Entity Type Id")]
        EntityTypeId = 11,

        /// <summary>
        /// Specifies RelationshipTypeId column
        /// </summary>
        [EnumMember]
        [Description("Relationship Type Id")]
        RelationshipTypeId = 12,

        /// <summary>
        /// Specifies CategoryId column
        /// </summary>
        [EnumMember]
        [Description("Category Id")]
        CategoryId = 13,

        /// <summary>
        /// Specifies AttributeId column
        /// </summary>
        [EnumMember]
        [Description("Attribute Id")]
        AttributeId = 14,

        /// <summary>
        /// Specifies EntityId column
        /// </summary>
        [EnumMember]
        [Description("Entity Id")]
        EntityId = 15,

        /// <summary>
        /// Specifies Locale column
        /// </summary>
        [EnumMember]
        [Description("Locale")]
        Locale = 17,

        /// <summary>
        /// Specifies LookupTableName column
        /// </summary>
        [EnumMember]
        [Description("Lookup Table Name")]
        LookupTableName = 18,

        /// <summary>
        /// Specifies MDMSourceId column
        /// </summary>
        [EnumMember]
        [Description("MDM Source Id")]
        MDMSourceId = 19,

        /// <summary>
        /// Specifies MDMSubscriberId column
        /// </summary>
        [EnumMember]
        [Description("MDM Subscriber Id")]
        MDMSubscriberId = 20,

        /// <summary>
        /// Specifies MDMPublisherId column
        /// </summary>
        [EnumMember]
        [Description("MDM Publisher Id")]
        MDMPublisherId = 21,

        /// <summary>
        /// Specifies ApplicationId column
        /// </summary>
        [EnumMember]
        [Description("Application Id")]
        ApplicationId = 22,

        /// <summary>
        /// Specifies ModuleId column
        /// </summary>
        [EnumMember]
        [Description("Module Id")]
        ModuleId = 23,

        /// <summary>
        /// Specifies ServerId column
        /// </summary>
        [EnumMember]
        [Description("Server Id")]
        ServerId = 24,

        /// <summary>
        /// Specifies ServerName column
        /// </summary>
        [EnumMember]
        [Description("Server Name")]
        ServerName = 25,

        /// <summary>
        /// Specifies ProfileId column
        /// </summary>
        [EnumMember]
        [Description("Profile Id")]
        ProfileId = 26,

        /// <summary>
        /// Specifies ProfileName column
        /// </summary>
        [EnumMember]
        [Description("Profile Name")]
        ProfileName = 27,

        /// <summary>
        /// Specifies ProgramName column
        /// </summary>
        [EnumMember]
        [Description("Program Name")]
        ProgramName = 28,

        /// <summary>
        /// Specifies JobId column
        /// </summary>
        [EnumMember]
        [Description("Job Id")]
        JobId = 29,

        /// <summary>
        /// Specifies ActivityName column
        /// </summary>
        [EnumMember]
        [Description("Activity Name")]
        ActivityName = 30
    }
}