using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents the class for workflowinfo
    /// </summary>
    [DataContract]
    public class WorkflowInfo : IWorkflowInfo, ICloneable
    {
        #region Fields

        /// <summary>
        /// Field denoting the workflow id
        /// </summary>
        private Int32 _workflowId = -1;

        /// <summary>
        /// Field denoting the workflow name
        /// </summary>
        private String _workflowName = String.Empty;

        /// <summary>
        /// Field denoting the workflow version
        /// </summary>
        private Int32 _workflowVersionNumber = -1;

        /// <summary>
        /// Field denoting the workflow activity id
        /// </summary>
        private Int32 _workflowActivityId = -1;

        /// <summary>
        /// Field denoting the workflow activity shortname
        /// </summary>
        private String _workflowActivityShortName = String.Empty;

        /// <summary>
        /// Field denoting the workflow activity longname
        /// </summary>
        private String _workflowActivityLongName = String.Empty;

        /// <summary>
        /// Field denoting the workflow activity action id
        /// </summary>
        private Int32 _workflowActivityActionId = -1;

        /// <summary>
        /// Field denoting the workflow activity action
        /// </summary>
        private String _workflowActivityAction = String.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denoting the workflow id
        /// </summary>
        [DataMember]
        public Int32 WorkflowId
        {
            get { return _workflowId; }
            set { _workflowId = value; }
        }

        /// <summary>
        /// Property denoting the workflow name
        /// </summary>
        [DataMember]
        public String WorkflowName
        {
            get { return _workflowName; }
            set { _workflowName = value; }
        }

        /// <summary>
        /// Property denoting the workflow version
        /// </summary>
        [DataMember]
        public Int32 WorkflowVersionNumber
        {
            get { return _workflowVersionNumber; }
            set { _workflowVersionNumber = value; }
        }

        /// <summary>
        /// Property denoting the workflow activity id
        /// </summary>
        [DataMember]
        public Int32 WorkflowActivityId
        {
            get { return _workflowActivityId; }
            set { _workflowActivityId = value; }
        }

        /// <summary>
        /// Property denoting the workflow activity shortname
        /// </summary>
        [DataMember]
        public String WorkflowActivityShortName
        {
            get { return _workflowActivityShortName; }
            set { _workflowActivityShortName = value; }
        }

        /// <summary>
        /// Property denoting the workflow activity longname
        /// </summary>
        [DataMember]
        public String WorkflowActivityLongName
        {
            get { return _workflowActivityLongName; }
            set { _workflowActivityLongName = value; }
        }

        /// <summary>
        /// Property denoting the workflow activity action id
        /// </summary>
        [DataMember]
        public Int32 WorkflowActivityActionId
        {
            get { return _workflowActivityActionId; }
            set { _workflowActivityActionId = value; }
        }

        /// <summary>
        /// Property denoting the workflow activity action
        /// </summary>
        [DataMember]
        public String WorkflowActivityAction
        {
            get { return _workflowActivityAction; }
            set { _workflowActivityAction = value; }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public WorkflowInfo()
        {

        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">WorkflowInfo as Xml</param>
        public WorkflowInfo(String valuesAsXml)
        {
            LoadWorkflowInforFromXml(valuesAsXml);
        }

        #endregion Constructor

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets a cloned instance of the current WorkflowInfo object
        /// </summary>
        /// <returns>Cloned instance of the current WorkflowInfo object</returns>
        public WorkflowInfo Clone()
        {
            WorkflowInfo clonedWorkflowInfo = new WorkflowInfo();

            clonedWorkflowInfo.WorkflowId = this.WorkflowId;
            clonedWorkflowInfo.WorkflowName = this.WorkflowName;
            clonedWorkflowInfo.WorkflowVersionNumber = this.WorkflowVersionNumber;
            clonedWorkflowInfo.WorkflowActivityId = this.WorkflowActivityId;
            clonedWorkflowInfo.WorkflowActivityShortName = this.WorkflowActivityShortName;
            clonedWorkflowInfo.WorkflowActivityLongName = this.WorkflowActivityLongName;
            clonedWorkflowInfo.WorkflowActivityActionId = this.WorkflowActivityActionId;
            clonedWorkflowInfo.WorkflowActivityAction = this.WorkflowActivityAction;

            return clonedWorkflowInfo;
        }

        /// <summary>
        /// Gets Xml representation of WorkflowInfo Object
        /// </summary>
        public String ToXml()
        {
            String outputXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    // WorkflowInfo node start
                    xmlWriter.WriteStartElement("WorkflowInfo");

                    xmlWriter.WriteAttributeString("WorkflowId", this.WorkflowId.ToString());
                    xmlWriter.WriteAttributeString("WorkflowName", this.WorkflowName.ToString());
                    xmlWriter.WriteAttributeString("WorkflowVersionNumber", this.WorkflowVersionNumber.ToString());
                    xmlWriter.WriteAttributeString("WorkflowActivityId", this.WorkflowActivityId.ToString());
                    xmlWriter.WriteAttributeString("WorkflowActivityShortName", this.WorkflowActivityShortName.ToString());
                    xmlWriter.WriteAttributeString("WorkflowActivityLongName", this.WorkflowActivityLongName.ToString());
                    xmlWriter.WriteAttributeString("WorkflowActivityActionId", this.WorkflowActivityActionId.ToString());
                    xmlWriter.WriteAttributeString("WorkflowActivityAction", this.WorkflowActivityAction.ToString());

                    // WorkflowInfo node end
                    xmlWriter.WriteEndElement();
                }

                //Get the output XML
                outputXml = sw.ToString();
            }

            return outputXml;
        }

        #region IClonable Members

        /// <summary>
        /// Gets a cloned instance of the current WorkflowInfo object
        /// </summary>
        /// <returns>Cloned instance of the current WorkflowInfo object</returns>
        Object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion IClonable Members

        #endregion Public Methods

        #region Private Methods

        private void LoadWorkflowInforFromXml(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "WorkflowInfo")
                        {
                            #region Read WorkflowInfo Attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("WorkflowId"))
                                {
                                    this.WorkflowId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.WorkflowId);
                                }

                                if (reader.MoveToAttribute("WorkflowName"))
                                {
                                    this.WorkflowName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("WorkflowVersionNumber"))
                                {
                                    this.WorkflowVersionNumber = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.WorkflowVersionNumber);
                                }

                                if (reader.MoveToAttribute("WorkflowActivityId"))
                                {
                                    this.WorkflowActivityId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.WorkflowActivityId);
                                }

                                if (reader.MoveToAttribute("WorkflowActivityShortName"))
                                {
                                    this.WorkflowActivityShortName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("WorkflowActivityLongName"))
                                {
                                    this.WorkflowActivityLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("WorkflowActivityActionId"))
                                {
                                    this.WorkflowActivityActionId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.WorkflowActivityActionId);
                                }

                                if (reader.MoveToAttribute("WorkflowActivityAction"))
                                {
                                    this.WorkflowActivityAction = reader.ReadContentAsString();
                                }

                                reader.Read();
                            }

                            #endregion Read WorkflowInfo Attributes
                        }
                        else
                        {
                            //Keep on reading the xml until we reach expected node.
                            reader.Read();
                        }
                    }
                }
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}
