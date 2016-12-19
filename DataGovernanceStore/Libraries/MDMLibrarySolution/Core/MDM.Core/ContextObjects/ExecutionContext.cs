using System;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Core.Extensions;
    using MDM.BusinessObjects;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies ExecutionContext which indicates all information required during entity execution
    /// </summary>
    [DataContract]
    [KnownType(typeof(SecurityContext))]
    [KnownType(typeof(CallerContext))]
    [KnownType(typeof(CallDataContext))]
    [KnownType(typeof(RuleExecutionInfo))]
    [ProtoContract]
    public class ExecutionContext : ObjectBase, IExecutionContext
    {
        #region Fields

        /// <summary>
        /// Field denoting the CallerContext
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        private CallerContext _callerContext = new CallerContext();

        /// <summary>
        /// Field denoting the callDataContext
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        private CallDataContext _callDataContext = new CallDataContext();

        /// <summary>
        /// Field denoting the SecurityContext
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        private SecurityContext _securityContext = new SecurityContext();

        /// <summary>
        /// Field denoting the additional data as part of execution context
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        private String _additionalContextData = String.Empty;

        /// <summary>
        /// Legacy Trace Source collection
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        private Collection<MDMTraceSource> _legacyMDMTraceSources = new Collection<MDMTraceSource>() {MDMTraceSource.UnKnown};

        /// <summary>
        /// Field denoting the RuleExecutionInfo
        /// </summary>
        [DataMember]
        [ProtoMember(6)]
        private RuleExecutionInfo _ruleexecutionInfo = new RuleExecutionInfo();

        #endregion Fields

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public ExecutionContext()
            : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="legacyMDMTraceSource"></param>
        public ExecutionContext(MDMTraceSource legacyMDMTraceSource)
            : base()
        {
            _legacyMDMTraceSources = new Collection<MDMTraceSource> {legacyMDMTraceSource};
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="legacyMDMTraceSources"></param>
        public ExecutionContext(Collection<MDMTraceSource> legacyMDMTraceSources)
            : base()
        {
            _legacyMDMTraceSources = legacyMDMTraceSources;
        }

        /// <summary>
        /// 
        /// </summary>
        public ExecutionContext(CallerContext callerContext, CallDataContext callDataContext, SecurityContext securityContext, String additionalContextData)
            : base()
        {
            _callerContext = callerContext;
            _callDataContext = callDataContext;
            _securityContext = securityContext;
            _additionalContextData = additionalContextData;
        }

        /// <summary>
        ///
        /// </summary>
        public ExecutionContext(CallerContext callerContext, CallDataContext callDataContext, SecurityContext securityContext, String additionalContextData, Collection<MDMTraceSource> legacyMDMTraceSources)
            : this(callerContext, callDataContext, securityContext, additionalContextData)
        {
            _legacyMDMTraceSources = legacyMDMTraceSources;
        }

        /// <summary>
        ///  Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        public ExecutionContext(String valuesAsXml)
        {
            LoadExecutionContext(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        ///  Property denoting the CallerContext
        /// </summary>
        public CallerContext CallerContext
        {
            get
            {
                return _callerContext;
            }
            set
            {
                _callerContext = value;
            }
        }

        /// <summary>
        ///  Property denoting the SecurityContext
        /// </summary>
        public SecurityContext SecurityContext
        {
            get
            {
                return _securityContext;
            }
            set
            {
                _securityContext = value;
            }
        }

        /// <summary>
        ///  Property denoting the callDataContext
        /// </summary>
        public CallDataContext CallDataContext
        {
            get
            {
                return _callDataContext;
            }
            set
            {
                _callDataContext = value;
            }
        }

        /// <summary>
        ///  Property denoting the ExecutionRuleInfo
        /// </summary>
        public RuleExecutionInfo RuleExecutionInfo
        {
            get
            {
                return _ruleexecutionInfo;
            }
            set
            {
                _ruleexecutionInfo = value;
            }
        }

        /// <summary>
        ///  Property denoting the callDataContext
        /// </summary>
        public String AdditionalContextData
        {
            get
            {
                return _additionalContextData;
            }
            set
            {
                _additionalContextData = value;
            }
        }

        /// <summary>
        ///  Property denoting the legacy mdm trace source values
        /// </summary>
        public Collection<MDMTraceSource> LegacyMDMTraceSources
        {
            get
            {
                return _legacyMDMTraceSources;
            }
            set
            {
                _legacyMDMTraceSources = value;
            }
        }

        #endregion Properties

        #region Methods

        #region Public methods

        /// <summary>
        /// Get XML representation of Entity Execution Context object
        /// </summary>
        /// <returns>XML representation of Entity Execution Context object</returns>
        public string ToXml()
        {
            String executionContext = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Entity Execution Context Map node start
            xmlWriter.WriteStartElement("ExecutionContext");

            #region Write Caller Context Informations

            if (this.CallerContext != null)
            {
                xmlWriter.WriteRaw(this.CallerContext.ToXml());
            }

            #endregion

            #region Write Call data Context Informations

            if (this.CallDataContext != null)
            {
                xmlWriter.WriteRaw(this.CallDataContext.ToXml());
            }

            #endregion

            #region Write Security Context Informations

            if (this.SecurityContext != null)
            {
                xmlWriter.WriteRaw(this.SecurityContext.ToXml());
            }

            #endregion

            #region Write Additional context Informations

            xmlWriter.WriteStartElement("AdditionalContextData");
            
            if (!String.IsNullOrWhiteSpace(this.AdditionalContextData))
            {
                xmlWriter.WriteRaw(AdditionalContextData);
            }

            xmlWriter.WriteEndElement();
            
            #endregion

            #region Write Legacy sources Informations

            xmlWriter.WriteStartElement("LegacyMDMTraceSources");

            if (this.LegacyMDMTraceSources != null && this.LegacyMDMTraceSources.Count > 0)
            {
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection<MDMTraceSource>(this.LegacyMDMTraceSources, "|"));
            }

            xmlWriter.WriteEndElement();

            #endregion

            //Operation result node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            executionContext = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return executionContext;
        }

        

        /// <summary>
        /// Get the current execution Rule information.
        /// </summary>
        /// <returns>Returns the IRuleExecutionInfo</returns>
        public IRuleExecutionInfo GetRuleExecutionInfo()
        {
            return (IRuleExecutionInfo)this.RuleExecutionInfo;
        }

        /// <summary>
        /// Gets the CallerContext
        /// </summary>
        /// <returns>ICallerContext</returns>
        public ICallerContext GetCallerContext()
        {
            return (ICallerContext)this.CallerContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ICallDataContext GetCallDataContext()
        {
            return (ICallDataContext)this.CallDataContext;
        }

        /// <summary>
        /// Gets the SecurityContext
        /// </summary>
        /// <returns>ISecurityContext</returns>
        public ISecurityContext GetSecurityContext()
        {
            return (ISecurityContext)this.SecurityContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentExecutionContext"></param>
        public void Merge(ExecutionContext parentExecutionContext)
        {
            if (parentExecutionContext.CallerContext != null)
            {
                MergeCallerContext(parentExecutionContext.CallerContext);
            }

            if (parentExecutionContext.CallDataContext != null)
            {
                MergeCallDataContext(parentExecutionContext.CallDataContext);
            }

            if (parentExecutionContext.SecurityContext != null)
            {
                MergeSecurityContext(parentExecutionContext.SecurityContext);
            }

            if (String.IsNullOrWhiteSpace(this._additionalContextData))
                this._additionalContextData = parentExecutionContext.AdditionalContextData;

            if (parentExecutionContext.LegacyMDMTraceSources.Count > 0)
            {
                foreach (var traceSource in parentExecutionContext.LegacyMDMTraceSources)
                {
                    if (!_legacyMDMTraceSources.Contains(traceSource))
                    { 
                        _legacyMDMTraceSources.Add((MDMTraceSource)((int)traceSource));
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ExecutionContext Clone()
        {
            var clonedExecutionContext = new ExecutionContext();

            if (_callerContext != null)
            {
                clonedExecutionContext.CallerContext = (CallerContext)_callerContext.Clone();
            }

            if (_callDataContext != null)
            {
                clonedExecutionContext.CallDataContext = _callDataContext.Clone();
            }

            if (_securityContext != null)
            {
                clonedExecutionContext.SecurityContext = _securityContext.Clone();
            }

            clonedExecutionContext.AdditionalContextData = _additionalContextData;

            if (_legacyMDMTraceSources != null && _legacyMDMTraceSources.Count > 0)
            {
                clonedExecutionContext.LegacyMDMTraceSources = _legacyMDMTraceSources.CopyCollection(true);
            }

            return clonedExecutionContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        public Boolean Compare(ExecutionContext executionContext)
        {
            var currentExecutionContext = this;

            if (currentExecutionContext.LegacyMDMTraceSources.Count > 0)
            {
                if (!(currentExecutionContext.LegacyMDMTraceSources.Count == 1 && currentExecutionContext.LegacyMDMTraceSources.ElementAt(0) == MDMTraceSource.UnKnown))
                {
                    if (!currentExecutionContext.LegacyMDMTraceSources.ContainsAny(executionContext.LegacyMDMTraceSources))
                        return false;
                }
            }

            if (!CompareCallerContext(executionContext.CallerContext))
            {
                return false;
            }

            if (!CompareCallDataContext(executionContext.CallDataContext))
            {
                return false;
            }

            if (!CompareSecurityContext(executionContext.SecurityContext))
            {
                return false;
            }

            return true;
        }

        #endregion Public methods

        #region Private methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadExecutionContext(String valuesAsXml)
        {
            #region Sample Xml

            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExecutionContext")
                    {
                        #region Read ExecutionContext Properties

                        reader.Read();

                        #endregion
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "CallerContext")
                    {
                        #region Read Caller Context

                        String callerContext = reader.ReadOuterXml();

                        if (!String.IsNullOrEmpty(callerContext))
                        {
                            this.CallerContext = new CallerContext(callerContext);
                        }

                        #endregion
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "CallDataContext")
                    {
                        #region Read Call Data Context

                        String callDataContextString = reader.ReadOuterXml();

                        if (!String.IsNullOrEmpty(callDataContextString))
                        {
                            this.CallDataContext = new CallDataContext(callDataContextString);
                        }

                        #endregion
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "SecurityContext")
                    {
                        #region Read Security Context

                        String securityContext = reader.ReadOuterXml();

                        if (!String.IsNullOrEmpty(securityContext))
                        {
                            this.SecurityContext = new SecurityContext(securityContext);
                        }

                        #endregion
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "AdditionalContextData")
                    {
                        this.AdditionalContextData = reader.ReadInnerXml();
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "LegacyMDMTraceSources")
                    {
                        String legacyTraceSources = reader.ReadElementContentAsString();
                        if (!String.IsNullOrWhiteSpace(legacyTraceSources))
                        {
                            this.LegacyMDMTraceSources = ValueTypeHelper.SplitStringToEnumCollection<MDMTraceSource>(legacyTraceSources, '|');
                        }
                    }
                    else
                    {
                        //Keep on reading the xml until we reach expected node.
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentCallerContext"></param>
        private void MergeCallerContext(CallerContext parentCallerContext)
        {
            var currentCallerContext = this.CallerContext;

            if (currentCallerContext.JobId < 1)
                currentCallerContext.JobId = parentCallerContext.JobId;

            if (currentCallerContext.ProfileId < 1)
                currentCallerContext.ProfileId = parentCallerContext.ProfileId;

            if (String.IsNullOrWhiteSpace(currentCallerContext.ProfileName))
                currentCallerContext.ProfileName = parentCallerContext.ProfileName;

            if (String.IsNullOrWhiteSpace(currentCallerContext.ProgramName))
                currentCallerContext.ProgramName = parentCallerContext.ProgramName;

            //currentCallerContext.Application = parentCallerContext.Application;

            if (currentCallerContext.Module == MDMCenterModules.Unknown)
                currentCallerContext.Module = parentCallerContext.Module;

            if (currentCallerContext.MDMSource == EventSource.UnKnown)
                currentCallerContext.MDMSource = parentCallerContext.MDMSource;

            if (currentCallerContext.MDMSubscriber == EventSubscriber.UnKnown)
                currentCallerContext.MDMSubscriber = parentCallerContext.MDMSubscriber;

            if (currentCallerContext.AdditionalProperties == null || currentCallerContext.AdditionalProperties.Count < 1)
                currentCallerContext.AdditionalProperties = parentCallerContext.AdditionalProperties;

            if (currentCallerContext.MDMPublisher == MDMPublisher.Unknown)
                currentCallerContext.MDMPublisher = parentCallerContext.MDMPublisher;

            if (currentCallerContext.ServerId < 1)
                currentCallerContext.ServerId = parentCallerContext.ServerId;

            if (String.IsNullOrWhiteSpace(currentCallerContext.ServerName))
                currentCallerContext.ServerName = parentCallerContext.ServerName;

            if (currentCallerContext.OperationId == Guid.Empty)
                currentCallerContext.OperationId = parentCallerContext.OperationId;

            if (currentCallerContext.ActivityId == Guid.Empty)
                currentCallerContext.ActivityId = parentCallerContext.ActivityId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentCallDataContext"></param>
        private void MergeCallDataContext(CallDataContext parentCallDataContext)
        {
            var currentCallDataContext = this.CallDataContext;

            if (currentCallDataContext.OrganizationIdList.Count < 1 && parentCallDataContext.OrganizationIdList.Count > 0)
                currentCallDataContext.OrganizationIdList = parentCallDataContext.OrganizationIdList;

            if (currentCallDataContext.ContainerIdList.Count < 1 && parentCallDataContext.ContainerIdList.Count > 0)
                currentCallDataContext.ContainerIdList = parentCallDataContext.ContainerIdList;

            if (currentCallDataContext.EntityTypeIdList.Count < 1 && parentCallDataContext.EntityTypeIdList.Count > 0)
                currentCallDataContext.EntityTypeIdList = parentCallDataContext.EntityTypeIdList;

            if (currentCallDataContext.RelationshipTypeIdList.Count < 1 && parentCallDataContext.RelationshipTypeIdList.Count > 0)
                currentCallDataContext.RelationshipTypeIdList = parentCallDataContext.RelationshipTypeIdList;

            if (currentCallDataContext.CategoryIdList.Count < 1 && parentCallDataContext.CategoryIdList.Count > 0)
                currentCallDataContext.CategoryIdList = parentCallDataContext.CategoryIdList;

            if (currentCallDataContext.AttributeIdList.Count < 1 && parentCallDataContext.AttributeIdList.Count > 0)
                currentCallDataContext.AttributeIdList = parentCallDataContext.AttributeIdList;

            if (currentCallDataContext.LocaleList.Count < 1 && parentCallDataContext.LocaleList.Count > 0)
                currentCallDataContext.LocaleList = parentCallDataContext.LocaleList;

            if (currentCallDataContext.LookupTableNameList.Count < 1 && parentCallDataContext.LookupTableNameList.Count > 0)
                currentCallDataContext.LookupTableNameList = parentCallDataContext.LookupTableNameList;

            if (currentCallDataContext.EntityIdList.Count < 1 && parentCallDataContext.EntityIdList.Count > 0)
                currentCallDataContext.EntityIdList = parentCallDataContext.EntityIdList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentSecurityContext"></param>
        private void MergeSecurityContext(SecurityContext parentSecurityContext)
        {
            if(_securityContext.UserId < 1 && parentSecurityContext.UserId > 0)
            {
                _securityContext = parentSecurityContext;
            }                
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callerContext"></param>
        private Boolean CompareCallerContext(CallerContext callerContext)
        {
            var currentCallerContext = this.CallerContext;

            if (currentCallerContext.OperationId.CompareTo(callerContext.OperationId) != 0)
            {
                return false;
            }

            if (currentCallerContext.ActivityId.CompareTo(callerContext.ActivityId) != 0)
            {
                return false;
            }

            if (!String.IsNullOrWhiteSpace(currentCallerContext.ProgramName) && !String.IsNullOrWhiteSpace(callerContext.ProgramName))
            {
                if (String.Compare(callerContext.ProgramName, 0, currentCallerContext.ProgramName, 0, callerContext.ProgramName.Length, StringComparison.InvariantCultureIgnoreCase) >= 0)
                    return false;
            }

            if(currentCallerContext.MDMSource != EventSource.UnKnown)
            {
                if (!currentCallerContext.MDMSource.Equals(callerContext.MDMSource))
                    return false;
            }

            if (currentCallerContext.MDMSubscriber != EventSubscriber.UnKnown)
            {
                if (!currentCallerContext.MDMSubscriber.Equals(callerContext.MDMSubscriber))
                    return false;
            }

            if (currentCallerContext.MDMPublisher != MDMPublisher.Unknown)
            {
                if (!currentCallerContext.MDMPublisher.Equals(callerContext.MDMPublisher))
                    return false;
            }

            if (currentCallerContext.Application != MDMCenterApplication.MDMCenter)
            {
                if (!currentCallerContext.Application.Equals(callerContext.Application))
                    return false;
            }

            if (currentCallerContext.Module != MDMCenterModules.Unknown)
            {
                if (!currentCallerContext.Module.Equals(callerContext.Module))
                    return false;
            }

            if (currentCallerContext.ServerId != -1)
            {
                if (currentCallerContext.ServerId != callerContext.ServerId)
                    return false;
            }

            if (!String.IsNullOrWhiteSpace(currentCallerContext.ServerName))
            {
                if (!currentCallerContext.ServerName.Equals(callerContext.ServerName, StringComparison.InvariantCultureIgnoreCase))
                    return false;
            }

            if (currentCallerContext.JobId != -1)
            {
                if (currentCallerContext.JobId != callerContext.JobId)
                    return false;
            }

            if (currentCallerContext.ProfileId != -1)
            {
                if (currentCallerContext.ProfileId != callerContext.ProfileId)
                    return false;
            }

            if (!String.IsNullOrWhiteSpace(currentCallerContext.ProfileName))
            {
                if (!currentCallerContext.ProfileName.Equals(callerContext.ProfileName, StringComparison.InvariantCultureIgnoreCase))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callDataContext"></param>
        /// <returns></returns>
        private Boolean CompareCallDataContext(CallDataContext callDataContext)
        {
            var currentCallDataContext = this.CallDataContext;

            if(currentCallDataContext.OrganizationIdList.Count > 0)
            {
                if (!currentCallDataContext.OrganizationIdList.ContainsAny(callDataContext.OrganizationIdList))
                    return false;
            }

            if (currentCallDataContext.ContainerIdList.Count > 0)
            {
                if (!currentCallDataContext.ContainerIdList.ContainsAny(callDataContext.ContainerIdList))
                    return false;
            }

            if (currentCallDataContext.EntityTypeIdList.Count > 0)
            {
                if (!currentCallDataContext.EntityTypeIdList.ContainsAny(callDataContext.EntityTypeIdList))
                    return false;
            }

            if (currentCallDataContext.RelationshipTypeIdList.Count > 0)
            {
                if (!currentCallDataContext.RelationshipTypeIdList.ContainsAny(callDataContext.RelationshipTypeIdList))
                    return false;
            }

            if (currentCallDataContext.CategoryIdList.Count > 0)
            {
                if (!currentCallDataContext.CategoryIdList.ContainsAny(callDataContext.CategoryIdList))
                    return false;
            }

            if (currentCallDataContext.AttributeIdList.Count > 0)
            {
                if (!currentCallDataContext.AttributeIdList.ContainsAny(callDataContext.AttributeIdList))
                    return false;
            }

            if (currentCallDataContext.LocaleList.Count > 0)
            {
                if (!currentCallDataContext.LocaleList.ContainsAny(callDataContext.LocaleList))
                    return false;
            }

            if (currentCallDataContext.LookupTableNameList.Count > 0)
            {
                if (!currentCallDataContext.LookupTableNameList.ContainsAny(callDataContext.LookupTableNameList))
                    return false;
            }

            if (currentCallDataContext.EntityIdList.Count > 0)
            {
                if (!currentCallDataContext.EntityIdList.ContainsAny(callDataContext.EntityIdList))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityContext"></param>
        /// <returns></returns>
        private Boolean CompareSecurityContext(SecurityContext securityContext)
        {
            var currentSecurityContext = this.SecurityContext;

            if(currentSecurityContext.UserId > 0)
            {
                if (!currentSecurityContext.UserId.Equals(securityContext.UserId))
                    return false;
            }

            if (!String.IsNullOrWhiteSpace(currentSecurityContext.UserLoginName))
            {
                if (!currentSecurityContext.UserLoginName.Equals(securityContext.UserLoginName, StringComparison.InvariantCultureIgnoreCase))
                    return false;
            }

            if (currentSecurityContext.UserRoleId > 0)
            {
                if (!currentSecurityContext.UserRoleId.Equals(securityContext.UserRoleId))
                    return false;
            }

            if (!String.IsNullOrWhiteSpace(currentSecurityContext.UserRoleName))
            {
                if (!currentSecurityContext.UserRoleName.Equals(securityContext.UserRoleName, StringComparison.InvariantCultureIgnoreCase))
                    return false;
            }

            return true;
        }

        #endregion Private methods

        #endregion Methods
    }
}
