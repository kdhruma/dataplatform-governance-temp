using ProtoBuf;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.BusinessObjects.Workflow;
    using MDM.Core;
    using MDM.Interfaces;
    /// <summary>
    /// Specifies the workflow change context of an entity.
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class WorkflowChangeContext : ObjectBase, IWorkflowChangeContext
    {
        #region Fields

        /// <summary>
        /// Indicates workflow activity name of workflow change context
        /// </summary>
        private String _activityName = String.Empty;

        /// <summary>
        /// Indicates workflow version of workflow change context
        /// </summary>
        private Int64 _workflowVersion = -1;

        /// <summary>
        /// Indicates workflow runtime instance id of workflow change context
        /// </summary>
        private String _workflowRuntimeInstanceId = String.Empty;

        /// <summary>
        /// Indicates workflow action context for change context
        /// </summary>
        private WorkflowActionContext _workflowActionContext = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Parameter-less Constructor
        /// </summary>
        public WorkflowChangeContext()
            : base()
        {
        }

        /// <summary>
        /// Converts XML into current instance
        /// </summary>
        /// <param name="valuesAsXml">Specifies xml representation of instance</param>
        public WorkflowChangeContext(String valuesAsXml)
        {
            LoadWorkflowChangeContext(valuesAsXml);
        }

        /// <summary>
        /// Constructor with activity name, workflow version , workflow runtime instance id and workflow action context of change context as input parameters
        /// </summary>
        /// <param name="activityName">Indicates workflow activity name of workflow change context</param>
        /// <param name="workflowVersion">Indicates workflow version of workflow change context</param>
        /// <param name="workflowRuntimeInstanceId">Indicates workflow runtime instance id of workflow change context</param>
        /// <param name="workflowActionContext">Indicates workflow action context for change context</param>
        public WorkflowChangeContext(String activityName, Int64 workflowVersion, String workflowRuntimeInstanceId, WorkflowActionContext workflowActionContext)
        {
            this._activityName = activityName;
            this._workflowVersion = workflowVersion;
            this._workflowRuntimeInstanceId = workflowRuntimeInstanceId;
            this._workflowActionContext = workflowActionContext;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Specifies workflow activity name of workflow change context
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public String ActivityName
        {
            get
            {
                return this._activityName;
            }
            set
            {
                this._activityName = value;
            }
        }

        /// <summary>
        /// Specifies workflow version of workflow change context
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public Int64 WorkflowVersion
        {
            get
            {
                return this._workflowVersion;
            }
            set
            {
                this._workflowVersion = value;
            }
        }

        /// <summary>
        /// Specifies workflow runtime instance id of workflow change context
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public String WorkflowRuntimeInstanceId
        {
            get
            {
                return this._workflowRuntimeInstanceId;
            }
            set
            {
                this._workflowRuntimeInstanceId = value;
            }
        }

        /// <summary>
        /// Specifies workflow action context for change context
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        public WorkflowActionContext WorkflowActionContext
        {
            get
            {
                if (this._workflowActionContext == null)
                {
                    this._workflowActionContext = new WorkflowActionContext();
                }

                return this._workflowActionContext;
            }
            set
            {
                this._workflowActionContext = value;
            }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        #region ToXml Methods

        /// <summary>
        /// Gives xml representation of an instance
        /// </summary>
        /// <returns>String xml representation</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    //WorkflowChangeContext node start
                    xmlWriter.WriteStartElement("WorkflowChangeContext");
                    xmlWriter.WriteAttributeString("ActivityName", this.ActivityName);
                    xmlWriter.WriteAttributeString("WorkflowVersion", this.WorkflowVersion.ToString());
                    xmlWriter.WriteAttributeString("WorkflowRuntimeInstanceId", this.WorkflowRuntimeInstanceId);

                    #region write workflow action context

                    if (this._workflowActionContext != null)
                    {
                        xmlWriter.WriteRaw(this.WorkflowActionContext.ToXml());
                    }

                    #endregion write workflow action context

                    //WorkflowChangeContext node end
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        #endregion

        #region Workflow Action Context related methods

        /// <summary>
        /// Gets the action context of workflow change context
        /// </summary>
        /// <returns>Gets the action context of workflow change context</returns>
        public IWorkflowActionContext GetWorkflowActionContext()
        {
            if (this._workflowActionContext == null)
            {
                return null;
            }

            return (IWorkflowActionContext)this.WorkflowActionContext;
        }

        /// <summary>
        /// Sets the action context of workflow change context
        /// </summary>
        /// <param name="iWorkflowActionContext">Indicates the workflow action context to be set</param>
        public void SetWorkflowActionContext(IWorkflowActionContext iWorkflowActionContext)
        {
            this.WorkflowActionContext = (WorkflowActionContext)iWorkflowActionContext;
        }

        #endregion

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is WorkflowChangeContext)
            {
                WorkflowChangeContext objectToBeCompared = obj as WorkflowChangeContext;

                if (this.ActivityName != objectToBeCompared.ActivityName)
                {
                    return false;
                }
                if (this.WorkflowVersion != objectToBeCompared.WorkflowVersion)
                {
                    return false;
                }
                if (this.WorkflowRuntimeInstanceId != objectToBeCompared.WorkflowRuntimeInstanceId)
                {
                    return false;
                }
                if (!this.WorkflowActionContext.Equals(objectToBeCompared.WorkflowActionContext))
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            Int32 hashCode = 0;

            hashCode = this.ActivityName.GetHashCode() ^ this.WorkflowVersion.GetHashCode() ^ this.WorkflowRuntimeInstanceId.GetHashCode() ^
                       this.WorkflowActionContext.GetHashCode();

            return hashCode;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadWorkflowChangeContext(String valuesAsXml)
        {
            XmlTextReader reader = null;

            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "WorkflowChangeContext")
                    {
                        #region Read WorkflowChangeContext

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("ActivityName"))
                            {
                                this._activityName = reader.ReadContentAsString();
                            }
                            if (reader.MoveToAttribute("WorkflowVersion"))
                            {
                                this._workflowVersion = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._workflowVersion);
                            }
                            if (reader.MoveToAttribute("WorkflowRuntimeInstanceId"))
                            {
                                this._workflowRuntimeInstanceId = reader.ReadContentAsString();
                            }

                            reader.Read();
                        }

                        #endregion Read WorkflowChangeContext
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "WorkflowActionContext")
                    {
                        #region Read WorkflowActionContext

                        String workflowActionContextXml = reader.ReadOuterXml();

                        if (!String.IsNullOrEmpty(workflowActionContextXml))
                        {
                            WorkflowActionContext workflowActionContext = new WorkflowActionContext(workflowActionContextXml);

                            if (workflowActionContext != null)
                            {
                                this.WorkflowActionContext = workflowActionContext;
                            }
                        }

                        #endregion Read WorkflowActionContext
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

        #endregion Private Methods

        #endregion Methods
    }
}