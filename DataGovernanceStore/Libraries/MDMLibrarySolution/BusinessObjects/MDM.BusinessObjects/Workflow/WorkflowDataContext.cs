using System;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Collections;
using System.Xml;
using System.Text;
using System.Linq;

namespace MDM.BusinessObjects.Workflow
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Workflow DataContext
    /// </summary>
    [DataContract]
    public class WorkflowDataContext : MDMObject, IWorkflowDataContext
    {
        #region Private Fields

        /// <summary>
        /// Represents the collection of Ids and types of the objects which are being passed to the workflow
        /// </summary>
        private WorkflowMDMObjectCollection _mdmObjectCollection = new WorkflowMDMObjectCollection();

        /// <summary>
        /// Short Name of the workflow
        /// </summary>
        private String _workflowName = String.Empty;

        /// <summary>
        /// Long Name of the workflow
        /// </summary>
        private String _workflowLongName = String.Empty;

        /// <summary>
        /// Represents the version of the workflow.
        /// </summary>
        private Int32 _workflowVersionId = 0;

        /// <summary>
        /// Represents the data which user wants to send to workflow runtime which is not predefined.
        /// The data should be in the format of XML.
        /// &lt;ExtendedProperties&gt;&lt;Property Key = "" Value = "" /&gt;&lt;/ExtendedProperties&gt;
        /// </summary>
        private String _extendedProperties = String.Empty;

        /// <summary>
        /// Application name to pass as argument. This is the Enum indicating which application is performing current action 
        /// </summary>
        private MDMCenterApplication _application = MDMCenterApplication.MDMCenter;

        /// <summary>
        /// Module name to pass as argument. This is the Enum indicating which module is performing current action 
        /// </summary>
        private MDMCenterModules _module = MDMCenterModules.Unknown;

        /// <summary>
        /// Indicates the comments entered for the Workflow.
        /// </summary>
        private String _workflowComments = String.Empty;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Property denoting the collection of Ids and types of the objects which are being passed to the workflow
        /// </summary>
        [DataMember]
        public WorkflowMDMObjectCollection MDMObjectCollection
        {
            get
            {
                return this._mdmObjectCollection;
            }
            set
            {
                this._mdmObjectCollection = value;
            }
        }

        /// <summary>
        /// Property denoting the Name of the workflow
        /// </summary>
        [DataMember]
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
        /// Property denoting the Long Name of the workflow
        /// </summary>
        [DataMember]
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
        /// Property denoting the version of the workflow.
        /// </summary>
        [DataMember]
        public Int32 WorkflowVersionId
        {
            get
            {
                return this._workflowVersionId;
            }
            set
            {
                this._workflowVersionId = value;
            }
        }

        /// <summary>
        /// Property denoting the data which user wants to send to workflow runtime which is not predefined.
        /// The data should be in the format of XML:
        /// &lt;ExtendedProperties&gt;&lt;Property Key = "" Value = "" /&gt;&lt;/ExtendedProperties&gt;
        /// </summary>
        [DataMember]
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
        /// Application name to pass as argument. This is the Enum indicating which application is performing current action 
        /// </summary>
        [DataMember]
        public MDMCenterApplication Application
        {
            get
            {
                return _application;
            }
            set
            {
                this._application = value;
            }
        }

        /// <summary>
        /// Module name to pass as argument. This is the Enum indicating which module is performing current action 
        /// </summary>
        [DataMember]
        public MDMCenterModules Module
        {
            get
            {
                return _module;
            }
            set
            {
                this._module = value;
            }
        }
        
        /// <summary>
        /// Property denoting the comments entered for the Workflow.
        /// </summary>
        [DataMember]
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
        
        #endregion Public Properties

        #region Constructors



        #endregion

        #region Methods



        #endregion

        #region IWorkflowDataContext Methods

        /// <summary>
        /// Get WorkflowMDMObjectCollection
        /// </summary>
        /// <returns></returns>
        public IWorkflowMDMObjectCollection GetWorkflowMDMObjectCollection()
        {
            if (this._mdmObjectCollection == null)
            {
                throw new NullReferenceException("WorkflowMDMObjectCollection is null");
            }
            return (IWorkflowMDMObjectCollection)this._mdmObjectCollection;
        }

        /// <summary>
        /// Set WorkflowMDMObjectCollection
        /// </summary>
        /// <param name="iWorkflowMDMObjectCollection">WorkflowMDMObjectCollection which needs to be set</param>
        public void SetWorkflowMDMObjectCollection(IWorkflowMDMObjectCollection iWorkflowMDMObjectCollection)
        {
            if (iWorkflowMDMObjectCollection == null)
                throw new ArgumentNullException("WorkflowMDMObjectCollection");

            this._mdmObjectCollection = (WorkflowMDMObjectCollection)iWorkflowMDMObjectCollection;
        }
        
        #endregion
    }
}
