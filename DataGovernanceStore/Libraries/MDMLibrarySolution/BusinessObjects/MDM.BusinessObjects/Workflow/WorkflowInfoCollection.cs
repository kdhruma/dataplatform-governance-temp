using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;
    using WFBO = MDM.BusinessObjects.Workflow;

    /// <summary>
    /// Represents the class for workflowinfo collection
    /// </summary>
    [DataContract]
    public class WorkflowInfoCollection : InterfaceContractCollection<IWorkflowInfo, WorkflowInfo>, IWorkflowInfoCollection, ICloneable
    {
        #region Fields

        #endregion Fields

        #region Properties

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public WorkflowInfoCollection()
        {

        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">WorkflowInfo as Xml</param>
        public WorkflowInfoCollection(String valuesAsXml)
        {
            LoadWorkflowInformationFromXml(valuesAsXml);
        }

        #endregion Constructor

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get workflowinfo by workflowname, activityname and activityaction
        /// </summary>
        /// <param name="workflowName">Indicates the workflow name</param>
        /// <param name="workflowActivityName">Indicates the workflow activity name</param>
        /// <param name="workflowActivityAction">Indicates the workflow activity actiion</param>
        /// <returns>WorkflowInfo</returns>
        public IWorkflowInfo GetWorkflowInfo(String workflowName, String workflowActivityName, String workflowActivityAction)
        {
            WorkflowInfo workflowInfoToReturn = null;

            if (!(String.IsNullOrWhiteSpace(workflowName) || String.IsNullOrWhiteSpace(workflowActivityName)))
            {
                if (this._items != null && this._items.Count > 0)
                {
                    foreach (WorkflowInfo workflowInfo in this._items)
                    {
                        //Get the first workflow activity info item which matches with given workflow name and activity(checks workflow action only if provided). 
                        if ((String.Compare(workflowInfo.WorkflowName, workflowName) == 0
                            && (String.Compare(workflowInfo.WorkflowActivityShortName, workflowActivityName) == 0 || String.Compare(workflowInfo.WorkflowActivityLongName, workflowActivityName) == 0))
                            && (String.IsNullOrWhiteSpace(workflowActivityAction) || String.Compare(workflowInfo.WorkflowActivityAction, workflowActivityAction) == 0))
                        {
                            workflowInfoToReturn = workflowInfo;
                            break;
                        }
                    }
                }
            }

            return workflowInfoToReturn;
        }

        /// <summary>
        /// Get all workflows available in the system
        /// </summary>
        /// <returns>Collection of Workflows</returns>
        public Collection<WFBO.Workflow> GetWorkflows()
        {
            Collection<WFBO.Workflow> workflows = new Collection<WFBO.Workflow>();

            foreach (WorkflowInfo workflowInfo in this._items)
            {
                WFBO.Workflow workflow = new WFBO.Workflow();
                workflow.Id = workflowInfo.WorkflowId;
                workflow.Name = workflowInfo.WorkflowName;

                if (workflows.Contains(workflow) == false)
                {
                    workflows.Add(workflow);
                }
            }

            return workflows;
        }

        /// <summary>
        /// Get workflow activities by workflow id
        /// </summary>
        /// <param name="workflowId">Indicates the workflow id</param>
        /// <returns>Collection of workflow activities</returns>
        public Collection<WFBO.WorkflowActivity> GetWorkflowActivitiesByWorkflowId(Int32 workflowId)
        {
            Collection<WFBO.WorkflowActivity> workflowsActivities = new Collection<WFBO.WorkflowActivity>();

            foreach (WorkflowInfo workflowInfo in this._items)
            {
                WFBO.WorkflowActivity workflowActivity = new WFBO.WorkflowActivity();
                workflowActivity.Id = workflowInfo.WorkflowActivityId;
                workflowActivity.Name = workflowInfo.WorkflowActivityLongName;

                if (workflowInfo.WorkflowId == workflowId && workflowsActivities.Contains(workflowActivity) == false)
                {
                    workflowsActivities.Add(workflowActivity);
                }
            }

            return workflowsActivities;
        }

        /// <summary>
        /// Get workflow activity action by workflow id and workflow activity id
        /// </summary>
        /// <param name="workflowId">Indicates the workflow id</param>
        /// <param name="workflowActivityId">Indicates the workflow acticity id</param>
        /// <returns>Collection of workflow activity actions</returns>
        public Collection<WFBO.ActivityAction> GetWorkflowActivityActionsByWorkflowIdAndWorkflowActivityId(Int32 workflowId, Int32 workflowActivityId)
        {
            Collection<WFBO.ActivityAction> workflowsActivityActions = new Collection<WFBO.ActivityAction>();

            foreach (WorkflowInfo workflowInfo in this._items)
            {
                WFBO.ActivityAction workflowActivityAction = new WFBO.ActivityAction();
                workflowActivityAction.Id = workflowInfo.WorkflowActivityActionId; // Change to WorkflowActivityActionId. DB Change is required
                workflowActivityAction.Name = workflowInfo.WorkflowActivityAction;

                if (workflowInfo.WorkflowId == workflowId && workflowInfo.WorkflowActivityId == workflowActivityId && 
                    workflowsActivityActions.Contains(workflowActivityAction) == false)
                {
                    workflowsActivityActions.Add(workflowActivityAction);
                }
            }

            return workflowsActivityActions;
        }

        /// <summary>
        /// Gets a cloned instance of the current WorkflowInfoCollection object
        /// </summary>
        /// <returns>Cloned instance of the current WorkflowInfoCollection object</returns>
        public WorkflowInfoCollection Clone()
        {
            WorkflowInfoCollection clonedWorkflowInformation = new WorkflowInfoCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (WorkflowInfo workflowInfo in this._items)
                {
                    WorkflowInfo clonedWorkflowInfo = workflowInfo.Clone();
                    clonedWorkflowInformation.Add(clonedWorkflowInfo);
                }
            }

            return clonedWorkflowInformation;
        }

        /// <summary>
        /// Gets Xml representation of WorkflowInfo Object
        /// </summary>
        /// <returns>WorkflowInfo Object as Xml</returns>
        public String ToXml()
        {
            String outputXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    // WorkflowInformation node start
                    xmlWriter.WriteStartElement("WorkflowInformation");

                    foreach (WorkflowInfo workflowInfo in this._items)
                    {
                        xmlWriter.WriteRaw(workflowInfo.ToXml());
                    }

                    // WorkflowInformation node end
                    xmlWriter.WriteEndElement();
                }

                // Get the actual XML
                outputXml = sw.ToString();
            }

            return outputXml;
        }

        #region IClonable Members

        /// <summary>
        /// Gets a cloned instance of the current WorkflowInfoCollection object
        /// </summary>
        /// <returns>Cloned instance of the current WorkflowInfoCollection object</returns>
        Object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion IClonable Members

        #endregion Public Methods

        #region Private Methods

        private void LoadWorkflowInformationFromXml(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "WorkflowInformation")
                        {
                            String workflowInfoXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(workflowInfoXml))
                            {
                                WorkflowInfo workflowInfo = new WorkflowInfo(workflowInfoXml);

                                if (workflowInfoXml != null)
                                {
                                    this.Add(workflowInfo);
                                }
                            }
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
