using ProtoBuf;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Workflow
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Contains the details of a workflow instance
    /// </summary>
    [DataContract]
    [ProtoContract]    
    public class WorkflowState : ObjectBase, IWorkflowState
    {
        #region Fields

        /// <summary>
        /// Represents the Workflow Id
        /// </summary>
        private Int32 _workflowId = -1;

        /// <summary>
        /// Represents the Workflow Short Name 
        /// </summary>
        private String _workflowName = String.Empty;

        /// <summary>
        /// Represents the Workflow Long Name 
        /// </summary>
        private String _workflowLongName = String.Empty;

        /// <summary>
        /// Represents the Workflow Version Id
        /// </summary>
        private Int32 _workflowVersionId = 0;

        /// <summary>
        /// Represent the Workflow Version long Name 
        /// </summary>
        private String _workflowVersionName = String.Empty;

        /// <summary>
        /// Represents the Workflow Instance Id
        /// </summary>
        private Int32 _instanceId = 0;

        /// <summary>
        /// Represents the ID of the activity
        /// </summary>
        private Int32 _activityId = 0;

        /// <summary>
        /// Unique Name of an activity
        /// </summary>
        private String _activityShortName = String.Empty;

        /// <summary>
        /// Name of the activity as displayed in the designer(Display Name)
        /// </summary>
        private String _activityLongName = String.Empty;

        /// <summary>
        /// User to whom the activity has been assigned
        /// </summary>
        private String _assignedUser = String.Empty;

        /// <summary>
        /// Role to whom the activity has been assigned
        /// </summary>
        private String _assignedRole = String.Empty;

        /// <summary>
        /// User Id to whom the activity has been assigned
        /// </summary>
        private Int32 _assignedUserId = -1;

        /// <summary>
        /// Field denoting previous activity short name.
        /// </summary>
        private String _previousActivityShortName = String.Empty;

        /// <summary>
        /// Field denoting previous activity long name.
        /// </summary>
        private String _previousActivityLongName = String.Empty;

        /// <summary>
        /// Field denoting previous activity user id.
        /// </summary>
        private Int32 _previousActivityUserId = 1;

        /// <summary>
        /// Field denoting previous activity user.
        /// </summary>
        private String _previousActivityUser = String.Empty;

        /// <summary>
        /// Field denoting previous activity comments.
        /// </summary>
        private String _previousActivityComments = String.Empty;

        /// <summary>
        /// Represent the PerformedAction by the last Activity
        /// </summary>
        private String _previousActivityAction = String.Empty;

        /// <summary>
        /// Represent the PerformedAction by the last Activity
        /// </summary>
        private String _previousActivityEventDate = String.Empty;

        /// <summary>
        /// The date time at which tracking record has been generated
        /// </summary>
        private String _eventDate = String.Empty;

        /// <summary>
        /// Indicates the comments entered for the Workflow.
        /// </summary>
        private String _workflowComments = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public WorkflowState()
        {

        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public WorkflowState(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadWorkflowState(valuesAsXml, objectSerialization);
        }

        /// <summary>
        /// Initialize Workflow state from tracked activity.
        /// </summary>
        /// <param name="trackedActivityInfo">Tracked activity information</param>
        public WorkflowState(TrackedActivityInfo trackedActivityInfo)
        {
            this._workflowId = trackedActivityInfo.WorkflowId;
            this._workflowName = trackedActivityInfo.WorkflowName;
            this._workflowLongName = trackedActivityInfo.WorkflowLongName;
            this._workflowVersionId = trackedActivityInfo.WorkflowVersionId;
            this._workflowVersionName = trackedActivityInfo.WorkflowVersionName;
            this._instanceId = trackedActivityInfo.Id;
            this._activityId = ValueTypeHelper.Int32TryParse(trackedActivityInfo.WorkflowDefinitionActivityID, 0);
            this._activityLongName = trackedActivityInfo.ActivityLongName;
            this._activityShortName = trackedActivityInfo.ActivityShortName;
            this._assignedUser = trackedActivityInfo.ActingUser;
            this._assignedRole = trackedActivityInfo.AssignedRoles;
            this._assignedUserId = trackedActivityInfo.ActingUserId;
            this._eventDate = trackedActivityInfo.EventDate;
            this._workflowComments = trackedActivityInfo.WorkflowComments;
            this._previousActivityEventDate = trackedActivityInfo.PreviousActivityStartDateTime;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Represents the Workflow Id
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public Int32 WorkflowId
        {
            get { return _workflowId; }
            set { _workflowId = value; }
        }

        /// <summary>
        /// Represents the Workflow Short Name 
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public String WorkflowName
        {
            get { return _workflowName; }
            set { _workflowName = value; }
        }

        /// <summary>
        /// Represents the Workflow Long Name 
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public String WorkflowLongName
        {
            get { return _workflowLongName; }
            set { _workflowLongName = value; }
        }

        /// <summary>
        /// Represents the Workflow Version Id
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        public Int32 WorkflowVersionId
        {
            get { return _workflowVersionId; }
            set { _workflowVersionId = value; }
        }

        /// <summary>
        /// Represent the Workflow Version long Name 
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        public String WorkflowVersionName
        {
            get { return _workflowVersionName; }
            set { _workflowVersionName = value; }
        }

        /// <summary>
        /// Represents the Workflow Instance Id
        /// </summary>
        [DataMember]
        [ProtoMember(6)]
        public Int32 InstanceId
        {
            get { return _instanceId; }
            set { _instanceId = value; }
        }

        /// <summary>
        /// Represents the ID of the activity
        /// </summary>
        [DataMember]
        [ProtoMember(7)]
        public Int32 ActivityId
        {
            get { return _activityId; }
            set { _activityId = value; }
        }

        /// <summary>
        /// Unique Name of an activity
        /// </summary>
        [DataMember]
        [ProtoMember(8)]
        public String ActivityShortName
        {
            get { return _activityShortName; }
            set { _activityShortName = value; }
        }

        /// <summary>
        /// Name of the activity as displayed in the designer(Display Name)
        /// </summary>
        [DataMember]
        [ProtoMember(9)]
        public String ActivityLongName
        {
            get { return _activityLongName; }
            set { _activityLongName = value; }
        }

        /// <summary>
        /// User to whom the activity has been assigned
        /// </summary>
        [DataMember]
        [ProtoMember(10)]
        public String AssignedUser
        {
            get { return _assignedUser; }
            set { _assignedUser = value; }
        }

        /// <summary>
        /// User Id to whome the activity has been assigned
        /// </summary>
        [DataMember]
        [ProtoMember(11)]
        public Int32 AssignedUserId
        {
            get { return _assignedUserId; }
            set { _assignedUserId = value; }
        }

        /// <summary>
        /// Property denoting previous activity short name.
        /// </summary>
        [DataMember]
        [ProtoMember(12)]
        public String PreviousActivityShortName
        {
            get { return _previousActivityShortName; }
            set { _previousActivityShortName = value; }
        }

        /// <summary>
        /// Property denoting previous activity long name.
        /// </summary>
        [DataMember]
        [ProtoMember(13)]
        public String PreviousActivityLongName
        {
            get { return _previousActivityLongName; }
            set { _previousActivityLongName = value; }
        }

        /// <summary>
        /// Property denoting previous activity user id.
        /// </summary>
        [DataMember]
        [ProtoMember(14)]
        public Int32 PreviousActivityUserId
        {
            get { return _previousActivityUserId; }
            set { _previousActivityUserId = value; }
        }

        /// <summary>
        /// Property denoting previous activity user.
        /// </summary>
        [DataMember]
        [ProtoMember(15)]
        public String PreviousActivityUser
        {
            get { return _previousActivityUser; }
            set { _previousActivityUser = value; }
        }

        /// <summary>
        /// Property denoting previous activity comments.
        /// </summary>
        [DataMember]
        [ProtoMember(16)]
        public String PreviousActivityComments
        {
            get { return _previousActivityComments; }
            set { _previousActivityComments = value; }
        }

        /// <summary>
        /// Property denoting the PerformedAction by the Last Activity
        /// </summary>
        [DataMember]
        [ProtoMember(17)]
        public String PreviousActivityAction
        {
            get
            {
                return _previousActivityAction;
            }
            set
            {
                _previousActivityAction = value;
            }
        }

        /// <summary>
        /// Property denoting the date time at which tracking record has been generated
        /// </summary>
        [DataMember]
        [ProtoMember(18)]
        public String EventDate
        {
            get
            {
                return this._eventDate;
            }
            set
            {
                this._eventDate = value;
            }
        }

        /// <summary>
        /// Property denoting the comments entered for the Workflow.
        /// </summary>
        [DataMember]
        [ProtoMember(19)]
        public String WorkflowComments
        {
            get
            {
                return _workflowComments;
            }
            set
            {
                _workflowComments = value;
            }
        }

        /// <summary>
        /// Property denoting the PerformedAction by the Last Activity
        /// </summary>
        [DataMember]
        [ProtoMember(20)]
        public String PreviousActivityEventDate
        {
            get
            {
                return _previousActivityEventDate;
            }
            set
            {
                _previousActivityEventDate = value;
            }
        }

        /// <summary>
        /// Property denoting the PerformedAction by the Last Activity
        /// </summary>
        [DataMember]
        [ProtoMember(21)]
        public String AssignedRole
        {
            get
            {
                return _assignedRole;
            }
            set
            {
                _assignedRole = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Loads the workflowstate with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadWorkflowState(string valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml
            /*
                <WorkflowState WorkflowId="4" WorkflowName="DelayWorkflow" WorkflowVersionId="6" WorkflowVersionName="V.0" InstanceId="12"
                  ActivityId="" ActivityShortName="9e797034-1ae0-4047-8b8f-0ebb7f8a71b4" ActivityLongName="HumanWork" AssignedUser="" AssignedUserId="-1"
                  PreviousActivityShortName="9e797034-1ae0-4047-8b8f-0ebb7f8a71b5" PreviousActivityLongName="" PreviousActivityUserId="20" PreviousActivityUser="cfadmin"
                  PreviousActivityComments="Please fill proper details." />      
            */
            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                if (objectSerialization == ObjectSerialization.DataStorage)
                {
                    LoadWorkflowStateForDataStorage(valuesAsXml);
                }
                else
                {
                    XmlTextReader reader = null;
                    try
                    {
                        reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                        while (!reader.EOF)
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "WorkflowState")
                            {
                                #region Read Workflow State

                                LoadWorkflowStateMetadataFromXml(reader);

                                reader.Read();

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
            }
        }

        /// <summary>
        /// Loads the workflowstate with the XML having values of object when Enum of ObjectSerialization is DataStorage
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        private void LoadWorkflowStateForDataStorage(string valuesAsXml)
        {
            #region Sample Xml
            /*
                <WorkflowState WfId="4" WfName="DelayWorkflow" WfVersionId="6" WfVersionName="V.0" InstId="12"
                  ActivityId="" ActivityName="9e797034-1ae0-4047-8b8f-0ebb7f8a71b4" ActivityLName="HumanWork" AssignedUser="" AssignedUserId="-1"
                  PrevActivityShortName="9e797034-1ae0-4047-8b8f-0ebb7f8a71b5" PrevActivityLongName="" PrevActivityUserId="20" PrevActivityUser="cfadmin"
                  PrevActivityComments="Please fill proper details." />      
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "WorkflowState")
                        {
                            #region Read Workflow State

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("WfId"))
                                {
                                    this.WorkflowId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                                }

                                if (reader.MoveToAttribute("WfName"))
                                {
                                    this.WorkflowName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("WfName"))
                                {
                                    this.WorkflowLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("WfVersionId"))
                                {
                                    this.WorkflowVersionId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("WfVersionName"))
                                {
                                    this.WorkflowVersionName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("InstId"))
                                {
                                    this.InstanceId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("ActivityId"))
                                {
                                    this.ActivityId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("ActivityName"))
                                {
                                    this.ActivityShortName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ActivityLName"))
                                {
                                    this.ActivityLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("AssignedUser"))
                                {
                                    this.AssignedUser = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("AssignedUserId"))
                                {
                                    this.AssignedUserId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                                }

                                if (reader.MoveToAttribute("PrevActivityName"))
                                {
                                    this.PreviousActivityShortName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("PrevActivityLName"))
                                {
                                    this.PreviousActivityLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("PrevActivityUserId"))
                                {
                                    this.PreviousActivityUserId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.PreviousActivityUserId);
                                }

                                if (reader.MoveToAttribute("PrevActivityUser"))
                                {
                                    this.PreviousActivityUser = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("PrevActivityComments"))
                                {
                                    this.PreviousActivityComments = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("PrevActivityAction"))
                                {
                                    this.PreviousActivityAction = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("EventDate"))
                                {
                                    this.EventDate = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("WfComments"))
                                {
                                    this.WorkflowComments = reader.ReadContentAsString();
                                }
                            }
                            reader.Read();

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
        }

        /// <summary>
        /// Loads properties of WorkflowState from xml
        /// </summary>
        /// <param name="reader">Indicates an xml reader used to read xml format data</param>
        internal void LoadWorkflowStateMetadataFromXml(XmlTextReader reader)
        {
            if (reader != null)
            {
                if (reader.HasAttributes)
                {
                    if (reader.MoveToAttribute("WorkflowId"))
                    {
                        this._workflowId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                    }

                    if (reader.MoveToAttribute("WorkflowName"))
                    {
                        this._workflowName = reader.ReadContentAsString();
                    }

                    if (reader.MoveToAttribute("WorkflowLongName"))
                    {
                        this._workflowLongName = reader.ReadContentAsString();
                    }

                    if (reader.MoveToAttribute("WorkflowVersionId"))
                    {
                        this._workflowVersionId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                    }

                    if (reader.MoveToAttribute("WorkflowVersionName"))
                    {
                        this._workflowVersionName = reader.ReadContentAsString();
                    }

                    if (reader.MoveToAttribute("InstanceId"))
                    {
                        this._instanceId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                    }

                    if (reader.MoveToAttribute("ActivityId"))
                    {
                        this._activityId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                    }

                    if (reader.MoveToAttribute("ActivityShortName"))
                    {
                        this._activityShortName = reader.ReadContentAsString();
                    }

                    if (reader.MoveToAttribute("ActivityLongName"))
                    {
                        this._activityLongName = reader.ReadContentAsString();
                    }

                    if (reader.MoveToAttribute("AssignedUser"))
                    {
                        this._assignedUser = reader.ReadContentAsString();
                    }

                    if (reader.MoveToAttribute("AssignedRole"))
                    {
                        this._assignedRole = reader.ReadContentAsString();
                    }

                    if (reader.MoveToAttribute("AssignedUserId"))
                    {
                        this._assignedUserId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                    }

                    if (reader.MoveToAttribute("PreviousActivityShortName"))
                    {
                        this._previousActivityShortName = reader.ReadContentAsString();
                    }

                    if (reader.MoveToAttribute("PreviousActivityLongName"))
                    {
                        this._previousActivityLongName = reader.ReadContentAsString();
                    }

                    if (reader.MoveToAttribute("PreviousActivityUserId"))
                    {
                        this._previousActivityUserId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._previousActivityUserId);
                    }

                    if (reader.MoveToAttribute("PreviousActivityUser"))
                    {
                        this._previousActivityUser = reader.ReadContentAsString();
                    }

                    if (reader.MoveToAttribute("PreviousActivityComments"))
                    {
                        this._previousActivityComments = reader.ReadContentAsString();
                    }

                    if (reader.MoveToAttribute("PreviousActivityAction"))
                    {
                        this._previousActivityAction = reader.ReadContentAsString();
                    }

                    if (reader.MoveToAttribute("PreviousActivityEventDate"))
                    {
                        this._previousActivityEventDate = reader.ReadContentAsString();
                    }

                    if (reader.MoveToAttribute("EventDate"))
                    {
                        this._eventDate = reader.ReadContentAsString();
                    }

                    if (reader.MoveToAttribute("WorkflowComments"))
                    {
                        this._workflowComments = reader.ReadContentAsString();
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("reader", "XmlTextReader is null. Can not read WorkflowState object.");
            }
        }

        /// <summary>
        /// Get XML representation of WorkflowState object
        /// </summary>
        /// <returns>XML representation of WorkflowState object</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            ConvertWorkflowStateToXml(xmlWriter);

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Converts WorkflowState object into xml format.
        /// In future, this method can be enhanced to accept EntityConversionContext object
        /// </summary>
        /// <param name="xmlWriter">Indicates a writer to generate xml reperesentation of WorkflowState object</param>
        internal void ConvertWorkflowStateToXml(XmlTextWriter xmlWriter)
        {
            if (xmlWriter != null)
            {
                //workflow state node start
                xmlWriter.WriteStartElement("WorkflowState");

                #region workflow state information

                xmlWriter.WriteAttributeString("WorkflowId", this._workflowId.ToString());
                xmlWriter.WriteAttributeString("WorkflowName", this._workflowName);
                xmlWriter.WriteAttributeString("WorkflowLongName", this._workflowLongName);
                xmlWriter.WriteAttributeString("WorkflowVersionId", this._workflowVersionId.ToString());
                xmlWriter.WriteAttributeString("WorkflowVersionName", this._workflowVersionName);
                xmlWriter.WriteAttributeString("WorkflowComments", this._workflowComments);
                xmlWriter.WriteAttributeString("InstanceId", this._instanceId.ToString());
                xmlWriter.WriteAttributeString("ActivityId", this._activityId.ToString());
                xmlWriter.WriteAttributeString("ActivityShortName", this._activityShortName);
                xmlWriter.WriteAttributeString("ActivityLongName", this._activityLongName);
                xmlWriter.WriteAttributeString("AssignedUser", this._assignedUser);
                xmlWriter.WriteAttributeString("AssignedRole", this._assignedRole);
                xmlWriter.WriteAttributeString("AssignedUserId", this._assignedUserId.ToString());
                xmlWriter.WriteAttributeString("PreviousActivityShortName", this._previousActivityShortName);
                xmlWriter.WriteAttributeString("PreviousActivityLongName", this._previousActivityLongName);
                xmlWriter.WriteAttributeString("PreviousActivityUserId", this._previousActivityUserId.ToString());
                xmlWriter.WriteAttributeString("PreviousActivityUser", this._previousActivityUser);
                xmlWriter.WriteAttributeString("PreviousActivityComments", this._previousActivityComments);
                xmlWriter.WriteAttributeString("PreviousActivityAction", this._previousActivityAction);
                xmlWriter.WriteAttributeString("PreviousActivityEventDate", this._previousActivityEventDate);
                xmlWriter.WriteAttributeString("EventDate", this._eventDate.ToString());

                #endregion workflow state information

                //workflow state node end
                xmlWriter.WriteEndElement();
            }
            else
            {
                throw new ArgumentNullException("xmlWriter", "XmlTextWriter is null. Can not write WorkflowState object.");
            }
        }

        /// <summary>
        /// Get XML representation of WorkflowState object
        /// </summary>
        /// <param name="serializationOption">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>XML representation of WorkflowState object</returns>
        public String ToXml(ObjectSerialization serializationOption)
        {
            String xml = String.Empty;

            if (serializationOption == ObjectSerialization.Full)
            {
                return this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                xmlWriter.WriteStartElement("WorkflowState");

                if (serializationOption == ObjectSerialization.DataStorage)
                {
                    #region workflow state information

                    xmlWriter.WriteAttributeString("WfId", this.WorkflowId.ToString());
                    xmlWriter.WriteAttributeString("WfName", this.WorkflowName);
                    xmlWriter.WriteAttributeString("WfLName", this.WorkflowLongName);
                    xmlWriter.WriteAttributeString("WfVersionId", this.WorkflowVersionId.ToString());
                    xmlWriter.WriteAttributeString("WfVersionName", this.WorkflowVersionName);
                    xmlWriter.WriteAttributeString("WfComments", this.WorkflowComments);
                    xmlWriter.WriteAttributeString("InstId", this.InstanceId.ToString());
                    xmlWriter.WriteAttributeString("ActivityId", this.ActivityId.ToString());
                    xmlWriter.WriteAttributeString("ActivityName", this.ActivityShortName);
                    xmlWriter.WriteAttributeString("ActivityLName", this.ActivityLongName);
                    xmlWriter.WriteAttributeString("AssignedUser", this.AssignedUser);
                    xmlWriter.WriteAttributeString("AssignedUserId", this.AssignedUserId.ToString());
                    xmlWriter.WriteAttributeString("PrevActivityName", this.PreviousActivityShortName);
                    xmlWriter.WriteAttributeString("PrevActivityLName", this.PreviousActivityLongName);
                    xmlWriter.WriteAttributeString("PrevActivityUserId", this.PreviousActivityUserId.ToString());
                    xmlWriter.WriteAttributeString("PrevActivityUser", this.PreviousActivityUser);
                    xmlWriter.WriteAttributeString("PrevActivityComments", this.PreviousActivityComments);
                    xmlWriter.WriteAttributeString("PrevActivityAction", this.PreviousActivityAction);
                    xmlWriter.WriteAttributeString("PrevActivityEDate", this._previousActivityEventDate);
                    xmlWriter.WriteAttributeString("EventDate", this.EventDate.ToString());

                    #endregion workflow state information
                }

                //workflow state node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                xml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }

            return xml;
        }

        /// <summary>
        /// Clone WorkflowState object
        /// </summary>
        /// <returns>Returns cloned copy of WorkflowState</returns>
        public WorkflowState Clone()
        {
            WorkflowState workflowState = new WorkflowState();

            workflowState._activityId = this.ActivityId;
            workflowState._activityShortName = this.ActivityShortName;
            workflowState._activityLongName = this.ActivityLongName;
            workflowState._assignedUser = this.AssignedUser;
            workflowState._assignedRole = this.AssignedRole;
            workflowState._assignedUserId = this.AssignedUserId;
            workflowState._eventDate = this.EventDate;
            workflowState._instanceId = this.InstanceId;

            workflowState._previousActivityAction = this.PreviousActivityAction;
            workflowState._previousActivityEventDate = this.PreviousActivityEventDate;
            workflowState._previousActivityComments = this.PreviousActivityComments;
            workflowState._previousActivityLongName = this.PreviousActivityLongName;
            workflowState._previousActivityShortName = this.PreviousActivityShortName;
            workflowState._previousActivityUser = this.PreviousActivityUser;
            workflowState._previousActivityUserId = this.PreviousActivityUserId;

            workflowState._workflowComments = this.WorkflowComments;
            workflowState._workflowId = this.WorkflowId;
            workflowState._workflowLongName = this.WorkflowLongName;
            workflowState._workflowName = this.WorkflowName;
            workflowState._workflowVersionId = this.WorkflowVersionId;
            workflowState._workflowVersionName = this._workflowVersionName;

            return workflowState;
        }

        #endregion Methods
    }
}