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
using System.Windows.Shapes;


namespace MDM.Workflow.Designer
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDMBOW = MDM.BusinessObjects.Workflow;

    /// <summary>
    /// Interaction logic for SelectWorkflow.xaml
    /// </summary>
    public partial class SelectWorkflow : Window
    {
        #region Fields

        /// <summary>
        /// Workflows data
        /// </summary>
        private Collection<MDMBOW.Workflow> _workflowCollection = null;

        /// <summary>
        /// Workflow Versions data
        /// </summary>
        private Collection<MDMBOW.WorkflowVersion> _workflowVersionCollection = null;

        #endregion

        #region Properties



        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public SelectWorkflow()
        {
            try
            {
                InitializeComponent();

                btnOK.IsEnabled = false;
                cbWorkflowVersion.IsEnabled = false;

                //Load workflows and workflow versions
                LoadWorkflows();
            }
            catch (Exception ex)
            {
                String errorMessage = String.Format("The error occurred while loading. Error: {0}", ex.Message);
                MessageBox.Show(errorMessage, "Select Workflow", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Methods

        private void LoadWorkflows()
        {
            WorkflowWCFUtilities workflowService = new WorkflowWCFUtilities();

            OperationResult operationResult = new OperationResult();
            _workflowCollection = new Collection<MDMBOW.Workflow>();
            _workflowVersionCollection = new Collection<MDMBOW.WorkflowVersion>();

            //Not required.. Since we are using the existing procedure, populating activities also
            Collection<MDMBOW.WorkflowActivity> workflowActivityCollection = new Collection<MDMBOW.WorkflowActivity>();

            CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.MDMAdvanceWorkflow);
            //Get all the workflow and workflow versions details from the DB
            workflowService.GetAllWorkflowDetails(ref _workflowCollection, ref _workflowVersionCollection, ref workflowActivityCollection, ref operationResult,callerContext);

            if (operationResult != null && operationResult.HasError)
                throw new Exception(operationResult.Errors[0].ErrorMessage);

            cbWorkflow.ItemsSource = _workflowCollection.Where(p=>p.WorkflowType.ToLower() == "wwf");

            cbWorkflowVersion.ItemsSource = _workflowVersionCollection;
        }

        #endregion

        #region Events

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void cbWorkflow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnOK.IsEnabled = false;

            if (cbWorkflow.SelectedValue != null)
            {
                Int32 selectedWorkflowID = 0;
                Int32.TryParse(cbWorkflow.SelectedValue.ToString(), out selectedWorkflowID);

                if (selectedWorkflowID > 0 && _workflowVersionCollection != null && _workflowVersionCollection.Count > 0)
                {
                    cbWorkflowVersion.ItemsSource = _workflowVersionCollection.Where(p => p.WorkflowId == selectedWorkflowID);
                    cbWorkflowVersion.IsEnabled = true;
                }
            }
        }

        private void cbWorkflowVersion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbWorkflow.SelectedValue != null && cbWorkflowVersion.SelectedValue != null)
            {
                Int32 selectedWorkflowID = 0;
                Int32.TryParse(cbWorkflow.SelectedValue.ToString(), out selectedWorkflowID);

                Int32 selectedWorkflowVersionID = 0;
                Int32.TryParse(cbWorkflowVersion.SelectedValue.ToString(), out selectedWorkflowVersionID);

                if (selectedWorkflowID > 0 && selectedWorkflowVersionID > 0)
                    btnOK.IsEnabled = true;
            }
        }

        #endregion
    }
}
