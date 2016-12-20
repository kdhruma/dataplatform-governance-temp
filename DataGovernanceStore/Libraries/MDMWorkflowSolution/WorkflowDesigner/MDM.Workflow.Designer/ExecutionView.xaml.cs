using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Activities;
using System.Activities.Debugger;
using System.Activities.Presentation;
using System.Activities.XamlIntegration;
using System.Activities.Presentation.Metadata;
using System.Activities.Statements;
using System.Activities.Core.Presentation;
using System.Activities.Presentation.Debug;
using System.Activities.Presentation.Services;
using System.ServiceModel.Activities;
using System.ServiceModel.Activities.Presentation.Factories;

namespace MDM.Workflow.Designer
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Workflow;

    /// <summary>
    /// Interaction logic for ExecutionView.xaml
    /// </summary>
    public partial class ExecutionView : Page
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private WorkflowDesigner _workflowDesigner = null;

        /// <summary>
        /// 
        /// </summary>
        public IDesignerDebugView _debuggerService = null;

        /// <summary>
        /// 
        /// </summary>
        private DocumentContentPanel _executionViewWindow = null;

        /// <summary>
        /// 
        /// </summary>
        private Collection<TrackedActivityInfo> _trackedActivityCollection = null;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<object, SourceLocation> _wfElementToSourceLocationMap = null;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, Activity> _activityIdToWfElementMap = null;

        /// <summary>
        /// 
        /// </summary>
        private DockableContentPanel _dataGridWindow = null;

        /// <summary>
        /// 
        /// </summary>
        private DataGrid _dgActivityRecordGrid = null;

        #endregion

        #region Properties



        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public ExecutionView()
        {
            try
            {
                InitializeComponent();
                RegisterMetadata();
                LoadExecutionView();
                GenerateDataGrid();
            }
            catch (Exception ex)
            {
                String errorMessage = String.Format("The error occurred while loading execution view.\nError: {0}", ex.Message);
                MessageBox.Show(errorMessage, "Workflow Execution View", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        private void RegisterMetadata()
        {
            DesignerMetadata metaData = new DesignerMetadata();
            metaData.Register();
        }

        /// <summary>
        /// Loads the execution view for the requested Instance ID
        /// </summary>
        private void LoadExecutionView()
        {
            this._workflowDesigner = new WorkflowDesigner();
            this._executionViewWindow = new DocumentContentPanel();

            WorkflowWCFUtilities workflowService = new WorkflowWCFUtilities();

            OperationResult operationResult = new OperationResult();
            WorkflowVersion workflowVersion = new WorkflowVersion();
            _trackedActivityCollection = new Collection<TrackedActivityInfo>();
            CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.MDMAdvanceWorkflow);

            //Get the Workflow Definition from DB
            workflowService.GetWorkflowViewDetails(App.WorkflowVersionId, App.InstanceGuid, ref workflowVersion, ref _trackedActivityCollection, ref operationResult,callerContext);

            if (workflowVersion != null && !String.IsNullOrEmpty(workflowVersion.WorkflowDefinition))
            {
                this._workflowDesigner.Text = workflowVersion.WorkflowDefinition;

                this._debuggerService = this._workflowDesigner.DebugManagerView;

                this._workflowDesigner.Load();

                //Add workflow definition to Dock Manager
                _executionViewWindow.DockManager = dockManager;
                _executionViewWindow.Content = this._workflowDesigner.View;
                _executionViewWindow.Show();

                //Mapping between the Object and Line No.
                _wfElementToSourceLocationMap = UpdateSourceLocationMappingInDebuggerService(workflowVersion.WorkflowDefinition);

                //Mapping between the Object and Activity
                _activityIdToWfElementMap = BuildActivityIdToWfElementMap(_wfElementToSourceLocationMap);
            }
            else
                throw new Exception("Workflow Definition is not available for this workflow.");
        }

        /// <summary>
        /// 
        /// </summary>
        private void GenerateDataGrid()
        {
            DataGridTextColumn gridColumn = null;

            _dgActivityRecordGrid = new DataGrid();
            _dataGridWindow = new DockableContentPanel();
            
            //Set properties
            _dgActivityRecordGrid.AutoGenerateColumns = false;
            _dgActivityRecordGrid.IsEnabled = false;

            //Add columns
            gridColumn = new DataGridTextColumn();
            gridColumn.Header = "Activity Name";
            gridColumn.Binding = new Binding("ActivityLongName");
            _dgActivityRecordGrid.Columns.Add(gridColumn);

            gridColumn = new DataGridTextColumn();
            gridColumn.Header = "Status";
            gridColumn.Binding = new Binding("Status");
            _dgActivityRecordGrid.Columns.Add(gridColumn);

            gridColumn = new DataGridTextColumn();
            gridColumn.Header = "Acted User";
            gridColumn.Binding = new Binding("ActedUser");
            _dgActivityRecordGrid.Columns.Add(gridColumn);

            gridColumn = new DataGridTextColumn();
            gridColumn.Header = "Comments";
            gridColumn.Binding = new Binding("Comments");
            _dgActivityRecordGrid.Columns.Add(gridColumn);

            gridColumn = new DataGridTextColumn();
            gridColumn.Header = "Event Date";
            gridColumn.Binding = new Binding("EventDate");
            _dgActivityRecordGrid.Columns.Add(gridColumn);

            //Assign to dock window
            _dataGridWindow.DockManager = dockManager;
            _dataGridWindow.Content = _dgActivityRecordGrid;
            _dataGridWindow.Show(Dock.Right);
            _dataGridWindow.Title = "Activity Records";
            _dataGridWindow.IsEnabled = false;
            _dataGridWindow.AllowDrop = false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void RunExecutionView()
        {
            foreach (TrackedActivityInfo activityTrackingRecord in _trackedActivityCollection)
            {
                if (_activityIdToWfElementMap.ContainsKey(activityTrackingRecord.WorkflowDefinitionActivityID))
                {
                    Activity activity = _activityIdToWfElementMap[activityTrackingRecord.WorkflowDefinitionActivityID];

                    _dgActivityRecordGrid.Items.Add(activityTrackingRecord);

                    this.Dispatcher.Invoke(DispatcherPriority.Render, (Action)(() =>
                    {
                        this._workflowDesigner.DebugManagerView.CurrentLocation = _wfElementToSourceLocationMap[activity];
                    }));

                    this.Dispatcher.Invoke(DispatcherPriority.SystemIdle, (Action)(() =>
                    {
                        //Add a sleep so that the debug adornments are visible to the user
                        System.Threading.Thread.Sleep(1000);
                    }));
                }
            }

            //Check for the last record status.
            //If status is closed remove the waiting highlighter
            Int32 recordsCount = _trackedActivityCollection.Count;
            if (_trackedActivityCollection[recordsCount - 1].Status == WorkflowActivityState.Closed.ToString())
                this._workflowDesigner.DebugManagerView.CurrentLocation = new SourceLocation("", 1, 1, 1, 10);
        }

        //Required as a reference-Tried differently. Not successful. 

        ///// <summary>
        ///// 
        ///// </summary>
        //private void RunExecutionView()
        //{
        //    foreach (ActivityTracking activityTrackingRecord in _trackedActivityCollection)
        //    {
        //        if (_activityIdToWfElementMap.ContainsKey(activityTrackingRecord.WorkflowDefinitionActivityID))
        //        {
        //            Activity activity = _activityIdToWfElementMap[activityTrackingRecord.WorkflowDefinitionActivityID];

        //            _dgActivityRecordGrid.Items.Add(activityTrackingRecord);

        //            if (activityTrackingRecord.Status == WorkflowActivityState.Executing.ToString())
        //            {
        //                this.Dispatcher.Invoke(DispatcherPriority.Render, (Action)(() =>
        //                {
        //                    this._workflowDesigner.DebugManagerView.CurrentLocation = _wfElementToSourceLocationMap[activity];
        //                    this._workflowDesigner.DebugManagerView.CurrentContext = new SourceLocation("", 1, 1, 1, 10);
        //                }));
        //            }
        //            else
        //            {
        //                this.Dispatcher.Invoke(DispatcherPriority.Render, (Action)(() =>
        //                {
        //                    this._workflowDesigner.DebugManagerView.CurrentContext = _wfElementToSourceLocationMap[activity];
        //                    this._workflowDesigner.DebugManagerView.CurrentLocation = new SourceLocation("", 1 ,1, 1, 10);
        //                }));
        //            }

        //            this.Dispatcher.Invoke(DispatcherPriority.SystemIdle, (Action)(() =>
        //            {
        //                //Add a sleep so that the debug adornments are visible to the user
        //                System.Threading.Thread.Sleep(2000);
        //            }));
        //        }
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Dictionary<object, SourceLocation> UpdateSourceLocationMappingInDebuggerService(String workflowDefinition)
        {
            object rootInstance = GetRootInstance();
            Dictionary<object, SourceLocation> sourceLocationMapping = new Dictionary<object, SourceLocation>();
            Dictionary<object, SourceLocation> designerSourceLocationMapping = new Dictionary<object, SourceLocation>();

            if (rootInstance != null)
            {
                Activity documentRootElement = GetRootWorkflowElement(rootInstance);
                SourceLocationProvider.CollectMapping(GetRootRuntimeWorkflowElement(workflowDefinition), documentRootElement, sourceLocationMapping, "");
                SourceLocationProvider.CollectMapping(documentRootElement, documentRootElement, designerSourceLocationMapping, "");
            }

            // Notify the DebuggerService of the new sourceLocationMapping.
            // When rootInstance == null, it'll just reset the mapping.
            // DebuggerService debuggerService = debuggerService as DebuggerService;
            if (this._debuggerService != null)
            {
                ((DebuggerService)this._debuggerService).UpdateSourceLocations(designerSourceLocationMapping);
            }

            return sourceLocationMapping;
        }

        private Dictionary<string, Activity> BuildActivityIdToWfElementMap(Dictionary<object, SourceLocation> wfElementToSourceLocationMap)
        {
            Dictionary<string, Activity> map = new Dictionary<string, Activity>();

            Activity wfElement;
            foreach (object instance in wfElementToSourceLocationMap.Keys)
            {
                wfElement = instance as Activity;
                if (wfElement != null)
                {
                    map.Add(wfElement.Id, wfElement);
                }
            }

            return map;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private object GetRootInstance()
        {
            ModelService modelService = this._workflowDesigner.Context.Services.GetService<ModelService>();
            if (modelService != null)
            {
                return modelService.Root.GetCurrentValue();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get root WorkflowElement.  Currently only handle when the object is ActivitySchemaType or WorkflowElement.
        /// May return null if it does not know how to get the root activity.
        /// </summary>
        /// <param name="rootModelObject"></param>
        /// <returns></returns>
        private Activity GetRootWorkflowElement(object rootModelObject)
        {
            if (rootModelObject == null)
                throw new Exception("Cannot pass null as rootModelObject.");

            Activity rootWorkflowElement;
            IDebuggableWorkflowTree debuggableWorkflowTree = rootModelObject as IDebuggableWorkflowTree;

            if (debuggableWorkflowTree != null)
            {
                rootWorkflowElement = debuggableWorkflowTree.GetWorkflowRoot();
            }
            else // Loose xaml case.
            {
                rootWorkflowElement = rootModelObject as Activity;
            }

            return rootWorkflowElement;
        }

        private Activity GetRootRuntimeWorkflowElement(String workflowDefinition)
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(workflowDefinition));
            Activity root = ActivityXamlServices.Load(xmlReader);
            WorkflowInspectionServices.CacheMetadata(root);

            IEnumerator<Activity> enumerator1 = WorkflowInspectionServices.GetActivities(root).GetEnumerator();

            //Get the first child of the x:class
            enumerator1.MoveNext();
            root = enumerator1.Current;

            xmlReader.Close();

            return root;
        }

        #endregion

        #region Events

        private void btnReplayExecutionView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Clear Items
                _dgActivityRecordGrid.Items.Clear();

                //Run execution view
                RunExecutionView();
            }
            catch (Exception ex)
            {
                String errorMessage = String.Format("The error occurred while loading execution view. Error: {0}", ex.Message);
                MessageBox.Show(errorMessage, "Workflow Execution View", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}
