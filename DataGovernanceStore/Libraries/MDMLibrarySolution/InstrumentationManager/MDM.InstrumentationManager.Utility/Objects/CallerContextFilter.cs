using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using ProtoBuf;

namespace MDM.BusinessObjects.Diagnostics
{
    using MDM.Core;
    using MDM.Core.Extensions;
    using MDM.Interfaces.Diagnostics;

    /// <summary>
    /// Specifies CallerContext filter
    /// </summary>
    [DataContract]
    [ProtoBuf.ProtoContract]
    [Serializable()]
    public class CallerContextFilter : ICallerContextFilter
    {
        #region Fields

        [DataMember]
        [ProtoMember(1)]
        private Collection<EventSource> _mdmSourceList = new Collection<EventSource>();

        [DataMember]
        [ProtoMember(2)]
        private Collection<EventSubscriber> _mdmSubscriberList = new Collection<EventSubscriber>();

        [DataMember]
        [ProtoMember(3)]
        private Collection<MDMPublisher> _mdmPublisherList = new Collection<MDMPublisher>();

        [DataMember]
        [ProtoMember(4)]
        private Collection<MDMCenterApplication> _applicationList = new Collection<MDMCenterApplication>();

        [DataMember]
        [ProtoMember(5)]
        private Collection<Int32> _serverIdList = new Collection<Int32>();

        [DataMember]
        [ProtoMember(6)]
        private Collection<String> _serverNameList = new Collection<String>();

        [DataMember]
        [ProtoMember(7)]
        private Collection<Int32> _profileIdList = new Collection<Int32>();

        [DataMember]
        [ProtoMember(8)]
        private Collection<String> _profileNameList = new Collection<String>();

        [DataMember]
        [ProtoMember(9)]
        private Collection<MDMCenterModules> _moduleList = new Collection<MDMCenterModules>();

        [DataMember]
        [ProtoMember(10)]
        private Collection<String> _programNameList = new Collection<String>();

        [DataMember]
        [ProtoMember(11)]
        private Collection<Int64> _jobIdList = new Collection<Int64>();

        [DataMember]
        [ProtoMember(12)]
        private Collection<Guid> _activityIdList = new Collection<Guid>();

        [DataMember]
        [ProtoMember(13)]
        private Collection<String> _activityNameList = new Collection<String>();

        [DataMember]
        [ProtoMember(14)]
        private Collection<Guid> _operationIdList = new Collection<Guid>();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs <see cref="CallerContextFilter"/> using specified instance data
        /// </summary>
        public CallerContextFilter(CallerContextFilter source)
        {
            this._mdmSourceList = source._mdmSourceList.CopyCollection();
            this._mdmSubscriberList = source._mdmSubscriberList.CopyCollection();
            this._mdmPublisherList = source._mdmPublisherList.CopyCollection();
            this._applicationList = source._applicationList.CopyCollection();
            this._serverIdList = source._serverIdList.CopyCollection();
            this._serverNameList = source._serverNameList.CloneCollection();
            this._profileIdList = source._profileIdList.CopyCollection();
            this._profileNameList = source._profileNameList.CloneCollection();
            this._moduleList = source._moduleList.CopyCollection();
            this._programNameList = source._programNameList.CloneCollection();
            this._jobIdList = source._jobIdList.CopyCollection();
            this._activityIdList = source._activityIdList.CopyCollection();
            this._activityNameList = source._activityNameList.CloneCollection();
            this._operationIdList = source._operationIdList.CopyCollection();
        }

        /// <summary>
        /// Constructs <see cref="CallerContextFilter"/> using data provided as XML
        /// </summary>
        /// <param name="callerContextFilterAsXml">XML string having data</param>
        public CallerContextFilter(String callerContextFilterAsXml)
        {
            LoadFromXml(callerContextFilterAsXml);
        }

        /// <summary>
        /// Prameterless constructor
        /// </summary>
        public CallerContextFilter()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Specifies MDMSource filter
        /// </summary>
        public Collection<EventSource> MDMSourceList
        {
            get { return _mdmSourceList; }
            set { _mdmSourceList = value; }
        }

        /// <summary>
        /// Specifies MDMSubscriber filter
        /// </summary>
        public Collection<EventSubscriber> MDMSubscriberList
        {
            get { return _mdmSubscriberList; }
            set { _mdmSubscriberList = value; }
        }

        /// <summary>
        /// Specifies MDMPublisher filter
        /// </summary>
        public Collection<MDMPublisher> MDMPublisherList
        {
            get { return _mdmPublisherList; }
            set { _mdmPublisherList = value; }
        }

        /// <summary>
        /// Specifies Application filter
        /// </summary>
        public Collection<MDMCenterApplication> ApplicationList
        {
            get { return _applicationList; }
            set { _applicationList = value; }
        }

        /// <summary>
        /// Specifies Server Id filter
        /// </summary>
        public Collection<Int32> ServerIdList
        {
            get { return _serverIdList; }
            set { _serverIdList = value; }
        }

        /// <summary>
        /// Specifies Server Name filter
        /// </summary>
        public Collection<String> ServerNameList
        {
            get { return _serverNameList; }
            set { _serverNameList = value; }
        }

        /// <summary>
        /// Specifies Profile Id filter
        /// </summary>
        public Collection<Int32> ProfileIdList
        {
            get { return _profileIdList; }
            set { _profileIdList = value; }
        }

        /// <summary>
        /// Specifies Profile Name filter
        /// </summary>
        public Collection<String> ProfileNameList
        {
            get { return _profileNameList; }
            set { _profileNameList = value; }
        }

        /// <summary>
        /// Specifies Module filter
        /// </summary>
        public Collection<MDMCenterModules> ModuleList
        {
            get { return _moduleList; }
            set { _moduleList = value; }
        }

        /// <summary>
        /// Specifies Program Name filter
        /// </summary>
        public Collection<String> ProgramNameList
        {
            get { return _programNameList; }
            set { _programNameList = value; }
        }

        /// <summary>
        /// Specifies Job Id filter
        /// </summary>
        public Collection<Int64> JobIdList
        {
            get { return _jobIdList; }
            set { _jobIdList = value; }
        }

        /// <summary>
        /// Specifies Activity Id filter
        /// </summary>
        public Collection<Guid> ActivityIdList
        {
            get { return _activityIdList; }
            set { _activityIdList = value; }
        }

        /// <summary>
        /// Specifies Activity Name filter
        /// </summary>
        public Collection<String> ActivityNameList
        {
            get { return _activityNameList; }
            set { _activityNameList = value; }
        }

        /// <summary>
        /// Specifies Operation Id filter
        /// </summary>
        public Collection<Guid> OperationIdList
        {
            get { return _operationIdList; }
            set { _operationIdList = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds <see cref="CallerContext"/> values into filter collections
        /// </summary>
        /// <param name="callerContext"><see cref="CallerContext"/> instance which data should be added into filter collections</param>
        public void AddCallerContextData(CallerContext callerContext)
        {
            if (callerContext != null)
            {
                if (callerContext.MDMSource != EventSource.UnKnown && !MDMSourceList.Contains(callerContext.MDMSource))
                {
                    MDMSourceList.Add(callerContext.MDMSource);
                }
                if (callerContext.MDMSubscriber != EventSubscriber.UnKnown && !MDMSubscriberList.Contains(callerContext.MDMSubscriber))
                {
                    MDMSubscriberList.Add(callerContext.MDMSubscriber);
                }
                if (callerContext.MDMPublisher != MDMPublisher.Unknown && !MDMPublisherList.Contains(callerContext.MDMPublisher))
                {
                    MDMPublisherList.Add(callerContext.MDMPublisher);
                }
                if (!ApplicationList.Contains(callerContext.Application))
                {
                    ApplicationList.Add(callerContext.Application);
                }
                if (callerContext.ServerId != -1 && !ServerIdList.Contains(callerContext.ServerId))
                {
                    ServerIdList.Add(callerContext.ServerId);
                }
                if (!String.IsNullOrWhiteSpace(callerContext.ServerName) && !ServerNameList.Any(x => x.Equals(callerContext.ServerName, StringComparison.InvariantCultureIgnoreCase)))
                {
                    ServerNameList.Add(callerContext.ServerName);
                }
                if (callerContext.ProfileId != -1 && !ProfileIdList.Contains(callerContext.ProfileId))
                {
                    ProfileIdList.Add(callerContext.ProfileId);
                }
                if (!String.IsNullOrWhiteSpace(callerContext.ProfileName) && !ProfileNameList.Any(x => x.Equals(callerContext.ProfileName, StringComparison.InvariantCultureIgnoreCase)))
                {
                    ProfileNameList.Add(callerContext.ProfileName);
                }
                if (callerContext.Module != MDMCenterModules.Unknown && !ModuleList.Contains(callerContext.Module))
                {
                    ModuleList.Add(callerContext.Module);
                }
                if (!String.IsNullOrWhiteSpace(callerContext.ProgramName) && !ProgramNameList.Any(x => x.Equals(callerContext.ProgramName, StringComparison.InvariantCultureIgnoreCase)))
                {
                    ProgramNameList.Add(callerContext.ProgramName);
                }
                if (callerContext.JobId != -1 && !JobIdList.Contains(callerContext.JobId))
                {
                    JobIdList.Add(callerContext.JobId);
                }
                if (callerContext.ActivityId != Guid.Empty && !ActivityIdList.Contains(callerContext.ActivityId))
                {
                    ActivityIdList.Add(callerContext.ActivityId);
                }
                if (callerContext.OperationId != Guid.Empty && !OperationIdList.Contains(callerContext.OperationId))
                {
                    OperationIdList.Add(callerContext.OperationId);
                }
            }
        }

        /// <summary>
        /// Returns <see cref="CallerContextFilter"/> in Xml format
        /// </summary>
        /// <returns>String representation of current <see cref="CallerContextFilter"/></returns>
        public String ToXml()
        {
            String result = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            // CallerContextFilter node start
            xmlWriter.WriteStartElement("CallerContextFilter");

            xmlWriter.WriteStartElement("MDMSourceList");
            if(this.MDMSourceList != null && this.MDMSourceList.Count > 0)
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.MDMSourceList, "|"));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("MDMSubscriberList");
            if (this.MDMSubscriberList != null && this.MDMSubscriberList.Count > 0)
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.MDMSubscriberList, "|"));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("MDMPublisherList");
            if (this.MDMPublisherList != null && this.MDMPublisherList.Count > 0)
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.MDMPublisherList, "|"));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("MDMApplicationList");
            if (this.ApplicationList != null && this.ApplicationList.Count > 0)
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.ApplicationList, "|"));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("MDMServiceList");
            if (this.ModuleList != null && this.ModuleList.Count > 0)
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.ModuleList, "|"));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ServerIdList");
            if (this.ServerIdList != null && this.ServerIdList.Count > 0)
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.ServerIdList, "|"));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ServerNameList");
            if (this.ServerNameList != null && this.ServerNameList.Count > 0)
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.ServerNameList, "|"));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ProfileIdList");
            if (this.ProfileIdList != null && this.ProfileIdList.Count > 0)
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.ProfileIdList, "|"));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ProfileNameList");
            if (this.ProfileNameList != null && this.ProfileNameList.Count > 0)
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.ProfileNameList, "|"));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ProgramNameList");
            if (this.ProgramNameList != null && this.ProgramNameList.Count > 0)
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.ProgramNameList, "|"));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("JobIdList");
            if (this.JobIdList != null && this.JobIdList.Count > 0)
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.JobIdList, "|"));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ActivityIdList");
            if (this.ActivityIdList != null && this.ActivityIdList.Count > 0)
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.ActivityIdList, "|"));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ActivityNameList");
            if (this.ActivityNameList != null && this.ActivityNameList.Count > 0)
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.ActivityNameList, "|"));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("OperationIdList");
            if (this.OperationIdList != null && this.OperationIdList.Count > 0)
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.OperationIdList, "|"));
            xmlWriter.WriteEndElement();

            // CallerContextFilter node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            // Get the actual XML
            result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Clones <see cref="CallerContextFilter"/>
        /// </summary>
        public object Clone()
        {
            return
                new CallerContextFilter(this);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads the <see cref="CallerContextFilter"/> with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        private void LoadFromXml(String valuesAsXml)
        {
            #region Sample Xml

            /*
	        <CallerContextFilter>
	          <MDMApplicationList>MDMCenter|...|...</MDMApplicationList>
	          <MDMPublisherList />
	          <MDMServiceList />
	          <MDMSourceList />
	          <MDMSubscriberList />
	          <ProgramNameList />
	          <ServerIdList />
	          <ServerNameList />
	          <JobIdList />
	          <ProfileIdList />
	          <ProfileNameList />
	          <OperationIdList />
	          <ActivityIdList />
	          <ActivityNameList />
	        </CallerContextFilter>
            */

            #endregion

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType != XmlNodeType.Element)
                    {
                        reader.Read();
                        continue;
                    }

                    String data = String.Empty;

                    if (reader.Name == "MDMSourceList")
                    {
                        data = reader.ReadElementContentAsString();
                        if (!String.IsNullOrWhiteSpace(data))                        
                            this.MDMSourceList = ValueTypeHelper.SplitStringToEnumCollection<EventSource>(data, '|');
                    }
                    else if (reader.Name == "MDMSubscriberList")
                    {
                        data = reader.ReadElementContentAsString();
                        if (!String.IsNullOrWhiteSpace(data))
                            this.MDMSubscriberList = ValueTypeHelper.SplitStringToEnumCollection<EventSubscriber>(data, '|');
                    }
                    else if (reader.Name == "MDMPublisherList")
                    {
                        data = reader.ReadElementContentAsString();
                        if (!String.IsNullOrWhiteSpace(data))
                            this.MDMPublisherList = ValueTypeHelper.SplitStringToEnumCollection<MDMPublisher>(data, '|');
                    }
                    else if (reader.Name == "MDMApplicationList")
                    {
                        data = reader.ReadElementContentAsString();
                        if (!String.IsNullOrWhiteSpace(data))
                            this.ApplicationList = ValueTypeHelper.SplitStringToEnumCollection<MDMCenterApplication>(data, '|');
                    }
                    else if (reader.Name == "MDMServiceList")
                    {
                        data = reader.ReadElementContentAsString();
                        if (!String.IsNullOrWhiteSpace(data))                        
                            this.ModuleList = ValueTypeHelper.SplitStringToEnumCollection<MDMCenterModules>(data, '|');
                    }
                    else if (reader.Name == "ServerIdList")
                    {
                        data = reader.ReadElementContentAsString();
                        if (!String.IsNullOrWhiteSpace(data))
                            this.ServerIdList = ValueTypeHelper.SplitStringToIntCollection(data, '|');
                    }
                    else if (reader.Name == "ServerNameList")
                    {
                        data = reader.ReadElementContentAsString();
                        if (!String.IsNullOrWhiteSpace(data))
                            this.ServerNameList = ValueTypeHelper.SplitStringToStringCollection(data, "|");
                    }
                    else if (reader.Name == "ProfileIdList")
                    {
                        data = reader.ReadElementContentAsString();
                        if (!String.IsNullOrWhiteSpace(data))                        
                            this.ProfileIdList = ValueTypeHelper.SplitStringToIntCollection(data, '|');
                    }
                    else if (reader.Name == "ProfileNameList")
                    {
                        data = reader.ReadElementContentAsString();
                        if (!String.IsNullOrWhiteSpace(data))
                            this.ProfileNameList = ValueTypeHelper.SplitStringToStringCollection(data, "|");
                    }
                    else if (reader.Name == "ProgramNameList")
                    {
                        data = reader.ReadElementContentAsString();
                        if (!String.IsNullOrWhiteSpace(data))
                            this.ProgramNameList = ValueTypeHelper.SplitStringToStringCollection(data, "|");
                    }
                    else if (reader.Name == "JobIdList")
                    {
                        data = reader.ReadElementContentAsString();
                        if (!String.IsNullOrWhiteSpace(data))
                            this.JobIdList = ValueTypeHelper.SplitStringToLongCollection(data, '|');
                    }
                    else if (reader.Name == "ActivityIdList")
                    {
                        data = reader.ReadElementContentAsString();
                        if (!String.IsNullOrWhiteSpace(data))
                            this.ActivityIdList = ValueTypeHelper.SplitStringToGuidCollection(data, '|');
                    }
                    else if (reader.Name == "ActivityNameList")
                    {
                        data = reader.ReadElementContentAsString();
                        if (!String.IsNullOrWhiteSpace(data))
                            this.ActivityNameList = ValueTypeHelper.SplitStringToStringCollection(data, "|");
                    }
                    else if (reader.Name == "OperationIdList")
                    {
                        data = reader.ReadElementContentAsString();
                        if (!String.IsNullOrWhiteSpace(data))
                            this.OperationIdList = ValueTypeHelper.SplitStringToGuidCollection(data, '|');
                    }
                    else
                    {
                        // Keep on reading the xml until we reach expected node
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

        #endregion
    }
}