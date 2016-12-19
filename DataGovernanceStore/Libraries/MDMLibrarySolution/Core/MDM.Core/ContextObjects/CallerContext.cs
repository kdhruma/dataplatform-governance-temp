using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Caller Context
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class CallerContext : ObjectBase, ICallerContext, ICloneable
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private EventSource _mdmSource = EventSource.UnKnown;

        /// <summary>
        /// 
        /// </summary>
        private EventSubscriber _mdmSubscriber = EventSubscriber.UnKnown;

        /// <summary>
        /// 
        /// </summary>
        private MDMPublisher _mdmPublisher = MDMPublisher.Unknown;

        /// <summary>
        /// Field denoting MDMCenter Application
        /// </summary>
        private MDMCenterApplication _application = MDMCenterApplication.MDMCenter;

        /// <summary>
        /// Field denoting the MDMCenter Module
        /// </summary>
        private MDMCenterModules _module = MDMCenterModules.Unknown;

        /// <summary>
        /// Field denoting the Program Name
        /// </summary>
        private String _programName = String.Empty;

        /// <summary>
        /// Field denoting the Job Id
        /// </summary>
        private Int32 _jobId = -1;

        /// <summary>
        /// Field denoting the Profile Id
        /// </summary>
        private Int32 _profileId = -1;

        /// <summary>
        /// Field denoting the Profile Name
        /// </summary>
        private String _profileName = String.Empty;

        /// <summary>
        /// Field denoting the server Id
        /// </summary>
        private Int32 _serverId = -1;

        /// <summary>
        /// Field denoting the Server Name
        /// </summary>
        private String _serverName = String.Empty;

        /// <summary>
        /// Field denoting additional properties
        /// </summary>
        private Dictionary<String, Object> _additionalProperties;

        /// <summary>
        /// 
        /// </summary>
        private Guid _operationId = Guid.Empty;

        /// <summary>
        /// 
        /// </summary>
        private TraceSettings _traceSettings = new TraceSettings();

        /// <summary>
        /// 
        /// </summary>
        private Guid _activityId;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public CallerContext() : base()
        {
        }

        /// <summary>
        /// Parameter with application and module of MDMCenter
        /// </summary>
        /// <param name="application">Indicates the application of the MDMCenter</param>
        /// <param name="module">Indicates the module of the MDMCenter</param>
        public CallerContext(MDMCenterApplication application, MDMCenterModules module)
        {
            this._application = application;
            this._module = module;
        }

        /// <summary>
        /// Parameter with application and module of MDMCenter
        /// </summary>
        /// <param name="application">Indicates the application of the MDMCenter</param>
        /// <param name="module">Indicates the module of the MDMCenter</param>
        /// <param name="programName">Indicates the caller program name</param>
        public CallerContext(MDMCenterApplication application, MDMCenterModules module, String programName)
        {
            this._application = application;
            this._module = module;
            this._programName = programName;
        }

        /// <summary>
        /// Parameter with application and module of MDMCenter
        /// </summary>
        /// <param name="application">Indicates the application of the MDMCenter</param>
        /// <param name="module">Indicates the module of the MDMCenter</param>
        /// <param name="programName">Indicates the caller program name</param>
        /// <param name="jobId">Indicates the Job Id</param>
        /// <param name="profileId">Indicates the Profile Id</param>
        /// <param name="profileName">Indicates the Profile Name</param>
        public CallerContext(MDMCenterApplication application, MDMCenterModules module, String programName, Int32 jobId, Int32 profileId, String profileName)
        {
            this._application = application;
            this._module = module;
            this._programName = programName;
            this._jobId = jobId;
            this._profileName = profileName;
            this._profileId = profileId;
        }

        /// <summary>
        ///  Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        public CallerContext(String valuesAsXml)
        {
            LoadCallerContext(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public EventSource MDMSource
        {
            get
            {
                return _mdmSource;
            }
            set
            {
                _mdmSource = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public EventSubscriber MDMSubscriber
        {
            get
            {
                return _mdmSubscriber;
            }
            set
            {
                _mdmSubscriber = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public MDMPublisher MDMPublisher
        {
            get
            {
                return _mdmPublisher;
            }
            set
            {
                _mdmPublisher = value;
            }
        }

        /// <summary>
        /// Property denoting the application of MDMCenter
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        public MDMCenterApplication Application
        {
            get
            {
                return this._application;
            }
            set
            {
                this._application = value;
            }
        }

        /// <summary>
        /// Property denoting the Module of MDMCenter
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        public MDMCenterModules Module
        {
            get
            {
                return this._module;
            }
            set
            {
                this._module = value;
            }
        }

        /// <summary>
        ///  Property denoting the ProgramName
        /// </summary>
        [DataMember]
        [ProtoMember(6)]
        public String ProgramName
        {
            get
            {
                return _programName;
            }
            set
            {
                _programName = value;
            }
        }

        /// <summary>
        /// Property denoting the Job Id
        /// </summary>
        [DataMember]
        [ProtoMember(7)]
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
        /// Property denoting the Profile Id
        /// </summary>
        [DataMember]
        [ProtoMember(8)]
        public Int32 ProfileId
        {
            get
            {
                return this._profileId;
            }
            set
            {
                this._profileId = value;
            }
        }

        /// <summary>
        /// Property denoting the Profile Name
        /// </summary>
        [DataMember]
        [ProtoMember(9)]
        public String ProfileName
        {
            get
            {
                return this._profileName;
            }
            set
            {
                this._profileName = value;
            }
        }

        /// <summary>
        /// Property denoting the Server Id
        /// </summary>
        [DataMember]
        [ProtoMember(10)]
        public Int32 ServerId
        {
            get
            {
                return this._serverId;
            }
            set
            {
                this._serverId = value;
            }
        }

        /// <summary>
        /// Property denoting the Server Name
        /// </summary>
        [DataMember]
        [ProtoMember(11)]
        public String ServerName
        {
            get
            {
                return this._serverName;
            }
            set
            {
                this._serverName = value;
            }
        }

        /// <summary>
        /// Property denoting additional properties
        /// </summary>
        [DataMember]
        [ProtoMember(12)]
        public Dictionary<String, Object> AdditionalProperties
        {
            get
            {
                return _additionalProperties;
            }
            set
            {
                _additionalProperties = value;
            }
        }

        /// <summary>
        /// Property denoting unique identifier for the caller activitiy        
        /// </summary>
        [DataMember]
        [ProtoMember(13)]
        public Guid OperationId
        {
            get
            {
                return _operationId;
            }
            set
            {
                _operationId = value;
            }
        }

        /// <summary>
        /// Property denoting unique identifier for the caller activitiy        
        /// </summary>
        [DataMember]
        [ProtoMember(14)]
        public TraceSettings TraceSettings
        {
            get
            {
                return _traceSettings;
            }
            set
            {
                _traceSettings = value;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(15)]
        public Guid ActivityId
        {
            get { return _activityId; }
            set { _activityId = value; }
        }
        

        #endregion

        #region Methods

        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// Get XML representation of Service Context object
        /// </summary>
        /// <returns>XML representation of Service Context object</returns>
        public string ToXml()
        {
            String callerContextXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Service Context Map node start
            xmlWriter.WriteStartElement("CallerContext");

            #region Write caller Context properties

            xmlWriter.WriteAttributeString("MDMApplication", this.Application.ToString());
            xmlWriter.WriteAttributeString("MDMPublisher", this.MDMPublisher.ToString());
            xmlWriter.WriteAttributeString("MDMService", this.Module.ToString());
            xmlWriter.WriteAttributeString("MDMSource", this.MDMSource.ToString());
            xmlWriter.WriteAttributeString("MDMSubscriber", this.MDMSubscriber.ToString());
            xmlWriter.WriteAttributeString("ProgramName", this.ProgramName);
            xmlWriter.WriteAttributeString("ServerId", this.ServerId.ToString());
            xmlWriter.WriteAttributeString("ServerName", this.ServerName);
            xmlWriter.WriteAttributeString("JobId", this.JobId.ToString());
            xmlWriter.WriteAttributeString("ProfileId", this.ProfileId.ToString());
            xmlWriter.WriteAttributeString("ProfileName", this.ProfileName);
            xmlWriter.WriteAttributeString("OperationId", this.OperationId.ToString());
            xmlWriter.WriteAttributeString("ActivityId", this.ActivityId.ToString());

            #endregion

            //Operation result node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            callerContextXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return callerContextXml;
        }

        #endregion Public methods

        #region Private methods

        private void LoadCallerContext(String valuesAsXml)
        {
            #region Sample Xml

            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "CallerContext")
                    {
                        #region Read CallerContext Properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("MDMApplication"))
                            {
                                MDMCenterApplication application = MDMCenterApplication.PIM;
                                Enum.TryParse<MDMCenterApplication>(reader.ReadContentAsString(), out application);
                                this.Application = application;
                            }

                            if (reader.MoveToAttribute("MDMPublisher"))
                            {
                                MDMPublisher publisher = MDMPublisher.Unknown;
                                Enum.TryParse<MDMPublisher>(reader.ReadContentAsString(), out publisher);
                                this.MDMPublisher = publisher;
                            }

                            if (reader.MoveToAttribute("MDMService"))
                            {
                                MDMCenterModules service = MDMCenterModules.Unknown;
                                Enum.TryParse<MDMCenterModules>(reader.ReadContentAsString(), out service);
                                this.Module = service;
                            }

                            if (reader.MoveToAttribute("MDMSource"))
                            {
                                EventSource eventSource = EventSource.Entity;
                                Enum.TryParse<EventSource>(reader.ReadContentAsString(), out eventSource);
                                this.MDMSource = eventSource;
                            }

                            if (reader.MoveToAttribute("MDMSubscriber"))
                            {
                                //what will be the default EventSubscriber?
                                EventSubscriber eventSubscriber = EventSubscriber.MDMCenter;
                                Enum.TryParse<EventSubscriber>(reader.ReadContentAsString(), out eventSubscriber);
                                this.MDMSubscriber = eventSubscriber;
                            }

                            if (reader.MoveToAttribute("ServerId"))
                            {
                                this.ServerId = ValueTypeHelper.Int16TryParse(reader.ReadContentAsString(), -1);
                            }

                            if (reader.MoveToAttribute("ServerName"))
                            {
                                this.ServerName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("JobId"))
                            {
                                this.JobId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                            }

                            if (reader.MoveToAttribute("ProfileId"))
                            {
                                this.ProfileId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                            }

                            if (reader.MoveToAttribute("ProfileName"))
                            {
                                this.ProfileName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("OperationId"))
                            {
                                this.OperationId = new Guid(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("ActivityId"))
                            {
                                this.ActivityId = new Guid(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("ProgramName"))
                            {
                                this.ProgramName = reader.ReadContentAsString();
                            }
                            else
                            {
                                //Keep on reading the xml until we reach expected node.
                                reader.Read();
                            }
                        }

                        #endregion
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

        #endregion Private methods

        #endregion Methods
    }
}
