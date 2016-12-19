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
    ///  Specifies the Workflow action context
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class WorkflowActionContext : MDMObject, IWorkflowActionContext
    {
        #region Private Fields

        /// <summary>
        /// Field denoting Workflow short name 
        /// </summary>
        private String _workflowName = String.Empty;

        /// <summary>
        /// Field denoting Workflow Long name 
        /// </summary>
        private String _workflowLongName = String.Empty;

        /// <summary>
        /// Currently executing activity.
        /// This is used to generate bookmark name
        /// </summary>
        private String _currentActivityName = String.Empty;

        /// <summary>
        /// Name of the activity as displayed in the designer(Display Name)
        /// </summary>
        private String _currentActivityLongName = String.Empty;

        /// <summary>
        /// Represents what action is being performed on the entity and based on the action, what action is required on the workflow.
        /// </summary>
        private String _userAction = String.Empty;

        /// <summary>
        /// Represents which user is working on the entity and thus indirectly on the workflow(login name).
        /// </summary>
        private String _actingUserName = String.Empty;

        /// <summary>
        /// Represents which user(id) is working on the entity and thus indirectly on the workflow.
        /// </summary>
        private Int32 _actingUserId = 0;

        /// <summary>
        /// Comment for the particular action on the workflow.
        /// </summary>
        private String _comments = String.Empty;

        /// <summary>
        /// Represents the data which user wants to send to workflow runtime which is not predefined.
        /// The data should be in the format of XML.
        /// <!--<ExtendedProperties><Property Key = "" Value = "" /></ExtendedProperties>-->
        /// </summary>
        private String _extendedProperties = String.Empty;

        /// <summary>
        /// Indicates Id of the Previous Assigned User.
        /// </summary>
        private Int32 _previousAssignedUserId = 0;

        /// <summary>
        /// Field denoting previous Assigned User name(login).
        /// </summary>
        private String _previousAssignedUserName = String.Empty;

        /// <summary>
        /// Indicates Id of the Newly Assigned User.
        /// </summary>
        private Int32 _newlyAssignedUserId = 0;

        /// <summary>
        /// Field denoting newly Assigned User name(login).
        /// </summary>
        private String _newlyAssignedUserName = String.Empty;

        /// <summary>
        /// Captures the error message on failure.
        /// </summary>
        private String _faultMessage = String.Empty;

        #endregion Private Fields

        #region Public properties

        /// <summary>
        /// Property denoting the Workflow short Name 
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public String WorkflowName
        {
            get
            {
                return this._workflowName;
            }
            set
            {
                this._workflowName = value;
            }
        }

        /// <summary>
        /// Property denoting the Workflow Long Name 
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public String WorkflowLongName
        {
            get
            {
                return this._workflowLongName;
            }
            set
            {
                this._workflowLongName = value;
            }
        }

        /// <summary>
        ///Property denoting the currently executing activity.
        /// This is used to generate bookmark name
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public String CurrentActivityName
        {
            get
            {
                return this._currentActivityName;
            }
            set
            {
                this._currentActivityName = value;
            }
        }

        /// <summary>
        ///Property denoting the currently executing activity Long Name.
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        public String CurrentActivityLongName
        {
            get
            {
                return this._currentActivityLongName;
            }
            set
            {
                this._currentActivityLongName = value;
            }
        }

        /// <summary>
        /// Property denoting what action is being performed on the entity and based on the action, what action is required on the workflow.
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        public String UserAction
        {
            get
            {
                return this._userAction;
            }
            set
            {
                this._userAction = value;
            }
        }

        /// <summary>
        /// Property denoting which user is working on the entity and thus indirectly on the workflow(login name).
        /// </summary>
        [DataMember]
        [ProtoMember(6)]
        public String ActingUserName
        {
            get
            {
                return this._actingUserName;
            }
            set
            {
                this._actingUserName = value;

            }
        }

        /// <summary>
        /// Property denoting which user(id) is working on the entity and thus indirectly on the workflow.
        /// </summary>
        [DataMember]
        [ProtoMember(7)]
        public Int32 ActingUserId
        {
            get
            {
                return this._actingUserId;
            }
            set
            {
                this._actingUserId = value;

            }
        }

        /// <summary>
        ///Property denoting the Comment for the particular action on the workflow.
        /// </summary>
        [DataMember]
        [ProtoMember(8)]
        public String Comments
        {
            get
            {
                return this._comments;
            }
            set
            {
                this._comments = value;
            }
        }

        /// <summary>
        /// Property denoting the data which user wants to send to workflow runtime which is not predefined.
        /// The data should be in the format of XML.
        /// <!--<ExtendedProperties><Property Key = "" Value = "" /></ExtendedProperties>-->
        /// </summary>
        [DataMember]
        [ProtoMember(9)]
        public new String ExtendedProperties
        {
            get
            {
                return this._extendedProperties;
            }
            set
            {
                this._extendedProperties = value;
            }
        }

        /// <summary>
        /// Property denoting Previous assigned user(id) who was working on the entity and thus indirectly on the workflow.
        /// </summary>
        [DataMember]
        [ProtoMember(10)]
        public Int32 PreviousAssignedUserId
        {
            get
            {
                return this._previousAssignedUserId;
            }
            set
            {
                this._previousAssignedUserId = value;

            }
        }

        /// <summary>
        /// Property denoting Previous assigned user name who was working on the entity and thus indirectly on the workflow(login name).
        /// </summary>
        [DataMember]
        [ProtoMember(11)]
        public String PreviousAssignedUserName
        {
            get
            {
                return this._previousAssignedUserName;
            }
            set
            {
                this._previousAssignedUserName = value;

            }
        }

        /// <summary>
        /// Property denoting Newly assigned user(id) who is working on the entity and thus indirectly on the workflow.
        /// </summary>
        [DataMember]
        [ProtoMember(12)]
        public Int32 NewlyAssignedUserId
        {
            get
            {
                return this._newlyAssignedUserId;
            }
            set
            {
                this._newlyAssignedUserId = value;

            }
        }

        /// <summary>
        /// Property denoting Newly assigned user name who is working on the entity and thus indirectly on the workflow(login name).
        /// </summary>
        [DataMember]
        [ProtoMember(13)]
        public String NewlyAssignedUserName
        {
            get
            {
                return this._newlyAssignedUserName;
            }
            set
            {
                this._newlyAssignedUserName = value;

            }
        }

        /// <summary>
        /// Failure Message on failure
        /// </summary>
        [DataMember]
        [ProtoMember(14)]
        public String FaultMessage
        {
            get
            {
                return this._faultMessage;
            }
            set
            {
                this._faultMessage = value;
            }
        }
        #endregion Public properties

        #region Constructors

        /// <summary>
        /// parameterless constructor
        /// </summary>
        public WorkflowActionContext()
            : base()
        {
        }

        /// <summary>
        /// Parameterized constructors with Values as XMl
        /// </summary>
        /// <param name="valueAsXml">values as a XMl format which needs to be initialized</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public WorkflowActionContext(String valueAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadWorkflowActionContext(valueAsXml, objectSerialization);
        }

        #endregion

        #region Methods

        private void LoadWorkflowActionContext(string valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml
            /*
                <WorkflowActionContext CurrentActivityName="" UserAction="" ActingUserName="" ActingUserId="0" Comments="" ExtendedProperties="" />
            */
            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                if (objectSerialization == ObjectSerialization.DataStorage)
                {
                    LoadWorkflowActionContextForDataStorage(valuesAsXml);
                }
                else
                {
                    XmlTextReader reader = null;
                    try
                    {
                        reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                        while (!reader.EOF)
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "WorkflowActionContext")
                            {
                                //Read entity metadata
                                #region Read Entity Attributes

                                    LoadWorkflowActionContextMetadataFromXml(reader);
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
        /// Populate current WorkflowActionContext object from XML when Enum of ObjectSerialization is DataStorage
        /// </summary>
        /// <example>
        /// Sample XML
        /// <para>
        /// <![CDATA[
        ///    <WorkflowActionContext CurrActivityName="" UserAction="" ActingUserName="" ActingUserId="0" Comments="" ExtendedProperties="" />
        /// ]]>
        /// </para>
        /// </example>
        /// <param name="valuesAsXml">String representing XML from which Value is to be populated.</param>
        private void LoadWorkflowActionContextForDataStorage(string valuesAsXml)
        {
            #region Sample Xml
            /*
                <WorkflowActionContext CURRActivityName="" UserAction="" ActingUserName="" ActingUserId="0" Comments="" ExtendedProperties="" />
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "WorkflowActionContext")
                        {
                            //Read WorkflowActionContext metadata
                            #region Read Entity Attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("CurrActivityName"))
                                {
                                    this.CurrentActivityName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("UserAction"))
                                {
                                    this.UserAction = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ActingUserName"))
                                {
                                    this.ActingUserName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ActingUserId"))
                                {
                                    this.ActingUserId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                                }

                                if (reader.MoveToAttribute("Comments"))
                                {
                                    this.Comments = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ExtendedProperties"))
                                {
                                    this.ExtendedProperties = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("WfName"))
                                {
                                    this.WorkflowName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("WfLName"))
                                {
                                    this.WorkflowLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("CurrActivityLName"))
                                {
                                    this.CurrentActivityLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("PrevAssignedUserId"))
                                {
                                    this.PreviousAssignedUserId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                                }

                                if (reader.MoveToAttribute("PrevAssignedUserName"))
                                {
                                    this.PreviousAssignedUserName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("NewlyAssignedUserId"))
                                {
                                    this.NewlyAssignedUserId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                                }

                                if (reader.MoveToAttribute("NewlyAssignedUserName"))
                                {
                                    this.NewlyAssignedUserName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("FaultMessage"))
                                {
                                    this.FaultMessage = reader.ReadContentAsString();
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
        /// Loads properties of WorkflowActionContext from xml
        /// </summary>
        /// <param name="reader">Indicates an xml reader used to read xml format data</param>
        internal void LoadWorkflowActionContextMetadataFromXml(XmlTextReader reader)
        {
            if (reader != null)
            {
                if (reader.HasAttributes)
                {
                    if (reader.MoveToAttribute("CurrentActivityName"))
                    {
                        this._currentActivityName = reader.ReadContentAsString();
                    }

                    if (reader.MoveToAttribute("UserAction"))
                    {
                        this._userAction = reader.ReadContentAsString();
                    }

                    if (reader.MoveToAttribute("ActingUserName"))
                    {
                        this._actingUserName = reader.ReadContentAsString();
                    }

                    if (reader.MoveToAttribute("ActingUserId"))
                    {
                        this._actingUserId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                    }

                    if (reader.MoveToAttribute("Comments"))
                    {
                        this._comments = reader.ReadContentAsString();
                    }

                    if (reader.MoveToAttribute("ExtendedProperties"))
                    {
                        this._extendedProperties = reader.ReadContentAsString();
                    }

                    if (reader.MoveToAttribute("WorkflowShortName"))
                    {
                        this._workflowName = reader.ReadContentAsString();
                    }

                    if (reader.MoveToAttribute("WorkflowLongName"))
                    {
                        this._workflowLongName = reader.ReadContentAsString();
                    }

                    if (reader.MoveToAttribute("CurrentActivityLongName"))
                    {
                        this._currentActivityLongName = reader.ReadContentAsString();
                    }

                    if (reader.MoveToAttribute("PreviousAssignedUserId"))
                    {
                        this._previousAssignedUserId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                    }

                    if (reader.MoveToAttribute("PreviousAssignedUserName"))
                    {
                        this._previousAssignedUserName = reader.ReadContentAsString();
                    }

                    if (reader.MoveToAttribute("NewlyAssignedUserId"))
                    {
                        this._newlyAssignedUserId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                    }

                    if (reader.MoveToAttribute("NewlyAssignedUserName"))
                    {
                        this._newlyAssignedUserName = reader.ReadContentAsString();
                    }

                    if (reader.MoveToAttribute("FaultMessage"))
                    {
                        this.FaultMessage = reader.ReadContentAsString();
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("reader", "XmlTextReader is null. Can not read WorkflowActionContext object.");
            }
        }

        /// <summary>
        /// Get XML representation of WorkflowActionContext  object
        /// </summary>
        /// <returns>XML representation of WorkflowActionContext  object</returns>
        public override String ToXml()
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            ConvertWorkflowActionContextToXml(xmlWriter);

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Get XML representation of WorkflowActionContext object
        /// </summary>
        /// <param name="serializationOption">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>XML representation of WorkflowActionContext  object</returns>
        public override String ToXml(ObjectSerialization serializationOption)
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

                xmlWriter.WriteStartElement("WorkflowActionContext");

                if (serializationOption == ObjectSerialization.DataStorage)
                {
                    #region workflow state information

                    xmlWriter.WriteAttributeString("CurrActivityName", this.CurrentActivityName.ToString());
                    xmlWriter.WriteAttributeString("UserAction", this.UserAction);
                    xmlWriter.WriteAttributeString("ActingUserName", this.ActingUserName.ToString());
                    xmlWriter.WriteAttributeString("ActingUserId", this.ActingUserId.ToString());
                    xmlWriter.WriteAttributeString("Comments", this.Comments);
                    xmlWriter.WriteAttributeString("ExtendedProperties", this.ExtendedProperties);
                    xmlWriter.WriteAttributeString("WfName", this.WorkflowName);
                    xmlWriter.WriteAttributeString("WfLName", this.WorkflowLongName);
                    xmlWriter.WriteAttributeString("CurrActivityLName", this.CurrentActivityLongName);
                    xmlWriter.WriteAttributeString("PrevAssignedUserId", this.PreviousAssignedUserId.ToString());
                    xmlWriter.WriteAttributeString("PrevAssignedUserName", this.PreviousAssignedUserName);
                    xmlWriter.WriteAttributeString("NewlyAssignedUserId", this.NewlyAssignedUserId.ToString());
                    xmlWriter.WriteAttributeString("NewlyAssignedUserName", this.NewlyAssignedUserName);
                    xmlWriter.WriteAttributeString("FaultMessage", this.FaultMessage);
                    #endregion workflow state information
                }

                //WorkflowActionContext node end
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
        /// Converts WorkflowActionContext object into xml format.
        /// In future, this method can be enhanced to accept EntityConversionContext object
        /// </summary>
        /// <param name="xmlWriter">Indicates a writer to generate xml reperesentation of WorkflowActionContext object</param>
        internal void ConvertWorkflowActionContextToXml(XmlTextWriter xmlWriter)
        {
            if (xmlWriter != null)
            {
                //WorkflowActionContext node start
                xmlWriter.WriteStartElement("WorkflowActionContext");

                xmlWriter.WriteAttributeString("CurrentActivityName", this._currentActivityName.ToString());
                xmlWriter.WriteAttributeString("UserAction", this._userAction);
                xmlWriter.WriteAttributeString("ActingUserName", this._actingUserName.ToString());
                xmlWriter.WriteAttributeString("ActingUserId", this._actingUserId.ToString());
                xmlWriter.WriteAttributeString("Comments", this._comments);
                xmlWriter.WriteAttributeString("ExtendedProperties", this._extendedProperties);
                xmlWriter.WriteAttributeString("WorkflowShortName", this._workflowName);
                xmlWriter.WriteAttributeString("WorkflowLongName", this._workflowLongName);
                xmlWriter.WriteAttributeString("CurrentActivityLongName", this._currentActivityLongName);
                xmlWriter.WriteAttributeString("PreviousAssignedUserId", this._previousAssignedUserId.ToString());
                xmlWriter.WriteAttributeString("PreviousAssignedUserName", this._previousAssignedUserName);
                xmlWriter.WriteAttributeString("NewlyAssignedUserId", this._newlyAssignedUserId.ToString());
                xmlWriter.WriteAttributeString("NewlyAssignedUserName", this._newlyAssignedUserName);
                xmlWriter.WriteAttributeString("FaultMessage", this.FaultMessage);
                //WorkflowActionContext node end
                xmlWriter.WriteEndElement();
            }
            else
            {
                throw new ArgumentNullException("xmlWriter", "XmlTextWriter is null. Can not write WorkflowActionContext object.");
            }
        }

        /// <summary>
        /// Clone WorkflowActionContext object
        /// </summary>
        /// <returns>Returns cloned copy of WorkflowActionContext</returns>
        public WorkflowActionContext Clone()
        {
            WorkflowActionContext workFlowActionContext = new WorkflowActionContext();

            workFlowActionContext._actingUserId = this.ActingUserId;
            workFlowActionContext._actingUserName = this.ActingUserName;
            workFlowActionContext._comments = this.Comments;
            workFlowActionContext._currentActivityLongName = this.CurrentActivityLongName;
            workFlowActionContext._currentActivityName = this.CurrentActivityName;

            workFlowActionContext._newlyAssignedUserId = this.NewlyAssignedUserId;
            workFlowActionContext._newlyAssignedUserName = this.NewlyAssignedUserName;
            workFlowActionContext._previousAssignedUserId = this.PreviousAssignedUserId;
            workFlowActionContext._previousAssignedUserName = this.PreviousAssignedUserName;

            workFlowActionContext._userAction = this.UserAction;
            workFlowActionContext._workflowLongName = this.WorkflowLongName;
            workFlowActionContext._workflowName = this.WorkflowName;
            workFlowActionContext._faultMessage = this.FaultMessage;
            return workFlowActionContext;
        }

        #endregion
    }
}
