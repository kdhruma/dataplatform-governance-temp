using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Activities;
using System.Activities.Presentation;
using System.Activities.Presentation.Toolbox;
using System.Activities.Presentation.Metadata;
using System.Activities.Statements;
using System.Activities.Core.Presentation;
using System.ServiceModel.Activities;
using System.ServiceModel.Activities.Presentation.Factories;


namespace MDM.Workflow.Designer
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Workflow;
    using MDM.Core;

    /// <summary>
    /// Interaction logic for DefinitionView.xaml
    /// </summary>
    public partial class DefinitionView : Page
    {
        #region Fields

        WorkflowDesigner _workflowDesigner = null;

        DocumentContentPanel _definitionWindow = null;

        DockableContentPanel _propertyWindow = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public DefinitionView()
        {
            InitializeComponent();
            RegisterMetadata();
            BindDefinitionView();
        }

        #endregion

        #region Methods

        private void RegisterMetadata()
        {
            DesignerMetadata metaData = new DesignerMetadata();
            metaData.Register();
        }

        /// <summary>
        /// Gets the Workflow Definition for requested Id and binds to the panel.
        /// </summary>
        private void BindDefinitionView()
        {
            this._workflowDesigner = new WorkflowDesigner();
            this._definitionWindow = new DocumentContentPanel();
            this._propertyWindow = new DockableContentPanel();
            WorkflowWCFUtilities workflowService = new WorkflowWCFUtilities();
            OperationResult operationResult = new OperationResult();
            WorkflowVersion workflowVersion = new WorkflowVersion();
            Collection<TrackedActivityInfo> trackedActivityCollection = new Collection<TrackedActivityInfo>();

            try
            {
                CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.MDMAdvanceWorkflow);

                //Get the Workflow Definition from DB
                workflowService.GetWorkflowViewDetails(App.WorkflowVersionId, "", ref workflowVersion, ref trackedActivityCollection, ref operationResult,callerContext );

                if (workflowVersion != null && !String.IsNullOrEmpty(workflowVersion.WorkflowDefinition))
                {
                    this._workflowDesigner.Text = workflowVersion.WorkflowDefinition;
                    this._workflowDesigner.Load();
                    //this._workflowDesigner.View.IsEnabled = false;
                    this._workflowDesigner.PropertyInspectorView.IsEnabled = false;

                    //Add workflow definition to Dock Manager
                    _definitionWindow.DockManager = dockManager;
                    _definitionWindow.Content = this._workflowDesigner.View;
                    _definitionWindow.Show();

                    // Add the Property Inspector (property window)
                    _propertyWindow.DockManager = dockManager;
                    _propertyWindow.Content = this._workflowDesigner.PropertyInspectorView;
                    _propertyWindow.Show(Dock.Right);
                    _propertyWindow.AutoHide();
                    _propertyWindow.Title = "Properties";
                    Uri uri = new Uri(@"\Icons\props.ico", UriKind.Relative);
                    _propertyWindow.Icon = new BitmapImage(uri);
                    _propertyWindow.IsEnabled = false;
                    _propertyWindow.Focusable = false;
                }
                else
                    throw new Exception("Workflow Definition is not available for this workflow.");
            }
            catch(Exception ex)
            {
                String errorMessage = String.Format("The error occurred while loading definition view.\nError: {0}", ex.Message);
                MessageBox.Show(errorMessage, "Workflow Definition View", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}
