using System;
using System.Globalization;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using ProtoBuf;

namespace MDM.BusinessObjects.Diagnostics
{
    using Core;
    using MDM.Core.Extensions;
    using MDM.Interfaces.Diagnostics;

    /// <summary>
    /// Specifies Tracing profile
    /// </summary>
    [DataContract]
    [ProtoBuf.ProtoContract]
    [Serializable()]
    public sealed class TracingProfile : MDMObject, ITracingProfile
    {
        #region Fields

        [DataMember]
        [ProtoMember(1)]
        private DateTime _startDateTime = DateTime.MinValue;

        [DataMember]
        [ProtoMember(2)]
        private DateTime _endDateTime = DateTime.MaxValue;

        [DataMember]
        [ProtoMember(3)]
        private CallerContextFilter _callerContextFilter = new CallerContextFilter();

        [DataMember]
        [ProtoMember(4)]
        private TraceSettings _traceSettings = new TraceSettings(false, TracingMode.SelectiveComponentTracing, TracingLevel.Basic);

        [DataMember]
        [ProtoMember(5)]
        private Collection<MessageClassEnum> _messageClasses = new Collection<MessageClassEnum>();

        [DataMember]
        [ProtoMember(6)]
        private static TracingProfile _instance = null;

        [DataMember]
        [ProtoMember(7)]
        private Int32? _durationMinimumThresholdInMilliSeconds = null;

        [DataMember]
        [ProtoMember(8)]
        private Int32? _durationMaximumThresholdInMilliSeconds = null;

        [DataMember]
        [ProtoMember(9)]
        private SecurityContextFilter _securityContextFilter = new SecurityContextFilter();

        [DataMember]
        [ProtoMember(10)]
        private CallDataContext _сallDataContext = new CallDataContext();

        [DataMember]
        [ProtoMember(11)]
        private Collection<MDMTraceSource> _legacyMDMTraceSources = new Collection<MDMTraceSource>();

        [DataMember]
        [ProtoMember(12)]
        private Collection<String> _messages = new Collection<String>();

        private static Object _lockObj = new Object();

        #endregion

        #region Constructors
        
        /// <summary>
        /// 
        /// </summary>
        public TracingProfile()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tracingProfileAsXml"></param>
        public TracingProfile(String tracingProfileAsXml)
        {
            LoadData(tracingProfileAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Specifies Start DateTime filter
        /// </summary>
        public DateTime StartDateTime
        {
            get { return _startDateTime; }
            set { _startDateTime = value; }
        }

        /// <summary>
        /// Specifies End DateTime filter
        /// </summary>
        public DateTime EndDateTime
        {
            get { return _endDateTime; }
            set { _endDateTime = value; }
        }

        /// <summary>
        /// Specifies minimum threshold for diagnostic activities duration in milliseconds
        /// </summary>
        public Int32? DurationMinimumThresholdInMilliSeconds
        {
            get { return _durationMinimumThresholdInMilliSeconds; }
            set { _durationMinimumThresholdInMilliSeconds = value; }
        }

        /// <summary>
        /// Specifies maximum threshold for diagnostic activities duration in milliseconds
        /// </summary>
        public Int32? DurationMaximumThresholdInMilliSeconds
        {
            get { return _durationMaximumThresholdInMilliSeconds; }
            set { _durationMaximumThresholdInMilliSeconds = value; }
        }

        /// <summary>
        /// Specifies CallerContext filter
        /// </summary>
        public CallerContextFilter CallerContextFilter
        {
            get { return _callerContextFilter; }
            set { _callerContextFilter = value; }
        }

        /// <summary>
        /// Specifies SecurityContext filter
        /// </summary>
        public SecurityContextFilter SecurityContextFilter
        {
            get { return _securityContextFilter; }
            set { _securityContextFilter = value; }
        }

        /// <summary>
        /// Specifies CallDataContext filter
        /// </summary>
        public CallDataContext CallDataContext
        {
            get { return _сallDataContext; }
            set { _сallDataContext = value; }
        }

        /// <summary>
        /// Specifies the legacy MDM trace source values
        /// </summary>
        public Collection<MDMTraceSource> LegacyMDMTraceSources
        {
            get { return _legacyMDMTraceSources; }
            set { _legacyMDMTraceSources = value; }
        }

        /// <summary>
        /// Specifies TraceSettings
        /// </summary>
        public TraceSettings TraceSettings
        {
            get { return _traceSettings; }
            set { _traceSettings = value; }
        }

        /// <summary>
        /// Specifies MessageClasses filter
        /// </summary>
        public Collection<MessageClassEnum> MessageClasses
        {
            get { return _messageClasses; }
            set { _messageClasses = value; }
        }

        /// <summary>
        /// Property denoting substrings to filter in diagnostic record message properties
        /// </summary>
        public Collection<String> Messages
        {
            get { return this._messages; }
            set { this._messages = value; }
        }

        #endregion

        #region Static Load and Get Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static TracingProfile GetCurrent()
        {
            return _instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tracingProfile"></param>
        public static void LoadCurrent(TracingProfile tracingProfile)
        {
            lock(_lockObj)
            {
                _instance = tracingProfile;

                if (_instance != null)
                {
                    Constants.TRACING_ENABLED = _instance.TraceSettings.IsBasicTracingEnabled;
                    Constants.PERFORMANCE_TRACING_ENABLED = _instance.TraceSettings.IsBasicTracingEnabled;
                }
            }
        }

        /// <summary>
        /// Loads Trace Configuration
        /// </summary>
        /// <param name="tracingProfileAsXml">Trace configuration xml to be loaded</param>
        public static void LoadCurrent(String tracingProfileAsXml)
        {
            if (!String.IsNullOrWhiteSpace(tracingProfileAsXml))
            {
                TracingProfile profile = new TracingProfile(tracingProfileAsXml);
                LoadCurrent(profile);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override String ToXml()
        {
            String tracingProfileXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            // Entity Execution Context Map node start
            xmlWriter.WriteStartElement("TracingProfile");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("LongName", this.LongName);

            xmlWriter.WriteStartElement("StartDateTime");
            xmlWriter.WriteRaw(this.StartDateTime.ToUniversalTime().ToString(Constants.DATETIME_IN_24_HOURS_FORMAT, CultureInfo.InvariantCulture));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("EndDateTime");
            xmlWriter.WriteRaw(this.EndDateTime.ToUniversalTime().ToString(Constants.DATETIME_IN_24_HOURS_FORMAT, CultureInfo.InvariantCulture));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("MessageClasses");
            if (this.MessageClasses != null && this.MessageClasses.Count > 0)
            {
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.MessageClasses, "|"));
            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Messages");
            if (this.Messages != null && this.Messages.Count > 0)
            {
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.Messages, "|"));
            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteRaw(this.TraceSettings.ToXml());

            xmlWriter.WriteRaw(this.CallDataContext.ToXml());

            xmlWriter.WriteRaw(this.CallerContextFilter.ToXml());

            xmlWriter.WriteRaw(this.SecurityContextFilter.ToXml());

            xmlWriter.WriteStartElement("LegacyMDMTraceSources");
            if (this.LegacyMDMTraceSources != null && this.LegacyMDMTraceSources.Count > 0)
            {
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection<MDMTraceSource>(this.LegacyMDMTraceSources, "|"));
            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("DurationMinimumThresholdInMilliSeconds");
            if (this.DurationMinimumThresholdInMilliSeconds.HasValue)
            {
                xmlWriter.WriteRaw(this.DurationMinimumThresholdInMilliSeconds.Value.ToString(CultureInfo.InvariantCulture));
            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("DurationMaximumThresholdInMilliSeconds");
            if (this.DurationMaximumThresholdInMilliSeconds.HasValue)
            {
                xmlWriter.WriteRaw(this.DurationMaximumThresholdInMilliSeconds.Value.ToString(CultureInfo.InvariantCulture));
            }
            xmlWriter.WriteEndElement();

            // Node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            // Get the actual XML
            tracingProfileXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return tracingProfileXml;
        }

        /// <summary>
        /// Check specified activity by profile filters
        /// </summary>
        /// <param name="diagnosticActivity">Activity to check</param>
        public Boolean CheckActivity(DiagnosticActivity diagnosticActivity)
        {
            if (DurationMinimumThresholdInMilliSeconds.HasValue &&
                diagnosticActivity.DurationInMilliSeconds < DurationMinimumThresholdInMilliSeconds.Value)
            {
                return false;
            }

            if (DurationMaximumThresholdInMilliSeconds.HasValue &&
                diagnosticActivity.DurationInMilliSeconds > DurationMaximumThresholdInMilliSeconds.Value)
            {
                return false;
            }

            if (diagnosticActivity.ExecutionContext != null)
            {
                if (LegacyMDMTraceSources.Count > 0)
                {
                    if (!LegacyMDMTraceSources.ContainsAny(diagnosticActivity.ExecutionContext.LegacyMDMTraceSources))
                    {
                        return false;
                    }
                }

                if (!CheckCallDataContext(diagnosticActivity.ExecutionContext.CallDataContext) ||
                    !CheckSecurityContext(diagnosticActivity.ExecutionContext.SecurityContext) ||
                    !CheckCallerContext(diagnosticActivity.ExecutionContext.CallerContext)
                )
                {
                    return false;
                }
            }

            if (CallerContextFilter.ActivityNameList.Count > 0)
            {
                if (!CallerContextFilter.ActivityNameList.Any(x => diagnosticActivity.ActivityName.IndexOf(x, 0, StringComparison.InvariantCultureIgnoreCase) >= 0))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check specified CallDataContext by profile filters
        /// </summary>
        /// <param name="callDataContext"><see cref="CallDataContext"/> to check</param>
        public Boolean CheckCallDataContext(CallDataContext callDataContext)
        {
            CallDataContext currentCallDataContext = this.CallDataContext;

            if(currentCallDataContext.OrganizationIdList.Count > 0)
            {
                if (!currentCallDataContext.OrganizationIdList.ContainsAny(callDataContext.OrganizationIdList))
                {
                    return false;
                }
            }

            if (currentCallDataContext.ContainerIdList.Count > 0)
            {
                if (!currentCallDataContext.ContainerIdList.ContainsAny(callDataContext.ContainerIdList))
                {
                    return false;
                }
            }

            if (currentCallDataContext.EntityTypeIdList.Count > 0)
            {
                if (!currentCallDataContext.EntityTypeIdList.ContainsAny(callDataContext.EntityTypeIdList))
                {
                    return false;
                }
            }

            if (currentCallDataContext.RelationshipTypeIdList.Count > 0)
            {
                if (!currentCallDataContext.RelationshipTypeIdList.ContainsAny(callDataContext.RelationshipTypeIdList))
                {
                    return false;
                }
            }

            if (currentCallDataContext.CategoryIdList.Count > 0)
            {
                if (!currentCallDataContext.CategoryIdList.ContainsAny(callDataContext.CategoryIdList))
                {
                    return false;
                }
            }

            if (currentCallDataContext.AttributeIdList.Count > 0)
            {
                if (!currentCallDataContext.AttributeIdList.ContainsAny(callDataContext.AttributeIdList))
                {
                    return false;
                }
            }

            if (currentCallDataContext.LocaleList.Count > 0)
            {
                if (!currentCallDataContext.LocaleList.ContainsAny(callDataContext.LocaleList))
                {
                    return false;
                }
            }

            if (currentCallDataContext.LookupTableNameList.Count > 0)
            {
                if (!currentCallDataContext.LookupTableNameList.ContainsAny(callDataContext.LookupTableNameList))
                {
                    return false;
                }
            }

            if (currentCallDataContext.EntityIdList.Count > 0)
            {
                if (!currentCallDataContext.EntityIdList.ContainsAny(callDataContext.EntityIdList))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check specified SecurityContext by profile filters
        /// </summary>
        /// <param name="securityContext"><see cref="SecurityContext"/> to check</param>
        public Boolean CheckSecurityContext(SecurityContext securityContext)
        {
            if(SecurityContextFilter.UserIdList.Count > 0)
            {
                if (!SecurityContextFilter.UserIdList.Contains(securityContext.UserId))
                {
                    return false;
                }
            }

            if (SecurityContextFilter.UserLoginNameList.Count > 0)
            {
                if (!SecurityContextFilter.UserLoginNameList.Any(x => x.Equals(securityContext.UserLoginName, StringComparison.InvariantCultureIgnoreCase)))
                {
                    return false;
                }
            }

            if (SecurityContextFilter.UserRoleIdList.Count > 0)
            {
                if (!SecurityContextFilter.UserRoleIdList.Contains(securityContext.UserRoleId))
                {
                    return false;
                }
            }

            if (SecurityContextFilter.UserRoleNameList.Count > 0)
            {
                if (!SecurityContextFilter.UserRoleNameList.Any(x => x.Equals(securityContext.UserRoleName, StringComparison.InvariantCultureIgnoreCase)))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check specified CallerContext by profile filters
        /// </summary>
        /// <param name="callerContext"><see cref="CallerContext"/> to check</param>
        public Boolean CheckCallerContext(CallerContext callerContext)
        {
            if (CallerContextFilter.OperationIdList.Count > 0)
            {
                if (!CallerContextFilter.OperationIdList.Contains(Constants.ProfileTracingOperationId)) // All operations will be passed if profile tracing is active
                {
                    if (!CallerContextFilter.OperationIdList.Contains(callerContext.OperationId))
                    {
                        return false;
                    }
                }
            }

            if (CallerContextFilter.ActivityIdList.Count > 0 && !CallerContextFilter.ActivityIdList.Contains(callerContext.ActivityId))
            {
                return false;
            }

            if (CallerContextFilter.ProgramNameList.Count > 0)
            {
                if (!CallerContextFilter.ProgramNameList.Any(x => callerContext.ProgramName.IndexOf(x, 0, StringComparison.InvariantCultureIgnoreCase) >= 0))
                {
                    return false;
                }
            }

            if(CallerContextFilter.MDMSourceList.Count > 0)
            {
                if (!CallerContextFilter.MDMSourceList.Contains(callerContext.MDMSource))
                {
                    return false;
                }
            }

            if (CallerContextFilter.MDMSubscriberList.Count > 0)
            {
                if (!CallerContextFilter.MDMSubscriberList.Contains(callerContext.MDMSubscriber))
                {
                    return false;
                }
            }

            if (CallerContextFilter.MDMPublisherList.Count > 0)
            {
                if (!CallerContextFilter.MDMPublisherList.Contains(callerContext.MDMPublisher))
                {
                    return false;
                }
            }

            if (CallerContextFilter.ApplicationList.Count > 0)
            {
                if (!CallerContextFilter.ApplicationList.Contains(callerContext.Application))
                {
                    return false;
                }
            }

            if (CallerContextFilter.ModuleList.Count > 0)
            {
                if (!CallerContextFilter.ModuleList.Contains(callerContext.Module))
                {
                    return false;
                }
            }

            if (CallerContextFilter.ServerIdList.Count > 0)
            {
                if (!CallerContextFilter.ServerIdList.Contains(callerContext.ServerId))
                {
                    return false;
                }
            }

            if (CallerContextFilter.ServerNameList.Count > 0)
            {
                if (!CallerContextFilter.ServerNameList.Any(x => x.Equals(callerContext.ServerName, StringComparison.InvariantCultureIgnoreCase)))
                {
                    return false;
                }
            }

            if (CallerContextFilter.JobIdList.Count > 0)
            {
                if (!CallerContextFilter.JobIdList.Contains(callerContext.JobId))
                {
                    return false;
                }
            }

            if (CallerContextFilter.ProfileIdList.Count > 0)
            {
                if (!CallerContextFilter.ProfileIdList.Contains(callerContext.ProfileId))
                {
                    return false;
                }
            }

            if (CallerContextFilter.ProfileNameList.Count > 0)
            {
                if (!CallerContextFilter.ProfileNameList.Any(x => x.Equals(callerContext.ProfileName, StringComparison.InvariantCultureIgnoreCase)))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Private Methods

        private void LoadData(String tracingProfileAsXml)
        {
            #region Sample Xml

            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(tracingProfileAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(tracingProfileAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType != XmlNodeType.Element)
                        {
                            reader.Read();
                            continue;
                        }

                        if (reader.Name == "TracingProfile")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.Id);
                                }

                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("LongName"))
                                {
                                    this.LongName = reader.ReadContentAsString();
                                }
                            }
                            reader.Read();
                        }
                        else if (reader.Name == "StartDateTime")
                        {
                            String startDateTimeAsString = reader.ReadElementContentAsString();
                            DateTime dateUTC;
                            if (DateTime.TryParseExact(startDateTimeAsString, Constants.DATETIME_IN_24_HOURS_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeUniversal, out dateUTC))
                            {
                                this.StartDateTime = dateUTC.ToLocalTime();
                            }
                            else
                            {
                                this.StartDateTime = DateTime.MinValue;
                            }
                        }
                        else if (reader.Name == "EndDateTime")
                        {
                            String endDateTimeAsString = reader.ReadElementContentAsString();
                            DateTime dateUTC;
                            if (DateTime.TryParseExact(endDateTimeAsString, Constants.DATETIME_IN_24_HOURS_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeUniversal, out dateUTC))
                            {
                                this.EndDateTime = dateUTC.ToLocalTime();
                            }
                            else
                            {
                                this.EndDateTime = DateTime.MaxValue;
                            }
                        }
                        else if (reader.Name == "MessageClasses")
                        {
                            String messageClasses = reader.ReadElementContentAsString();
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
                        else if (reader.Name == "Messages")
                        {
                            String messages = reader.ReadElementContentAsString();
                            if (!String.IsNullOrWhiteSpace(messages))
                            {
                                this.Messages = ValueTypeHelper.SplitStringToStringCollection(messages, '|');
                            }
                        }
                        else if (reader.Name == "TraceSettings")
                        {
                            this.TraceSettings = new TraceSettings(reader.ReadOuterXml());
                        }
                        else if (reader.Name == "ExecutionContext")
                        {
                            // This code for backward compatibility only
                            ExecutionContext executionContext = new ExecutionContext(reader.ReadOuterXml());
                            this.CallDataContext = executionContext.CallDataContext;
                            this.LegacyMDMTraceSources = executionContext.LegacyMDMTraceSources;
                            this.SecurityContextFilter.AddSecurityContextData(executionContext.SecurityContext);
                            this.CallerContextFilter.AddCallerContextData(executionContext.CallerContext);
                            
                            // CallerContext's non-nullable properties default values processing
                            if (CallerContextFilter.ApplicationList.Count == 1 && CallerContextFilter.ApplicationList.ElementAt(0) == MDMCenterApplication.MDMCenter)
                            {
                                CallerContextFilter.ApplicationList.Clear();
                            }
                            if (CallerContextFilter.ModuleList.Count == 1 && CallerContextFilter.ModuleList.ElementAt(0) == MDMCenterModules.Unknown)
                            {
                                CallerContextFilter.ModuleList.Clear();
                            }
                            if (CallerContextFilter.MDMSourceList.Count == 1 && CallerContextFilter.MDMSourceList.ElementAt(0) == EventSource.UnKnown)
                            {
                                CallerContextFilter.MDMSourceList.Clear();
                            }
                            if (CallerContextFilter.MDMSubscriberList.Count == 1 && CallerContextFilter.MDMSubscriberList.ElementAt(0) == EventSubscriber.UnKnown)
                            {
                                CallerContextFilter.MDMSubscriberList.Clear();
                            }
                            if (CallerContextFilter.MDMPublisherList.Count == 1 && CallerContextFilter.MDMPublisherList.ElementAt(0) == MDMPublisher.Unknown)
                            {
                                CallerContextFilter.MDMPublisherList.Clear();
                            }
                        }
                        else if (reader.Name == "DurationMinimumThresholdInMilliSeconds")
                        {
                            Int32 value;
                            if (Int32.TryParse(reader.ReadElementContentAsString(), out value))
                            {
                                this.DurationMinimumThresholdInMilliSeconds = value;
                            }
                        }
                        else if (reader.Name == "DurationMaximumThresholdInMilliSeconds")
                        {
                            Int32 value;
                            if (Int32.TryParse(reader.ReadElementContentAsString(), out value))
                            {
                                this.DurationMaximumThresholdInMilliSeconds = value;
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
                                this.LegacyMDMTraceSources = ValueTypeHelper.SplitStringToEnumCollection<MDMTraceSource>(legacyTraceSources, '|');
                            }
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
    }
}