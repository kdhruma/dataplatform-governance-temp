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
using Microsoft.Win32;

using MDM.Core;
using MDM.BusinessObjects.Workflow;

namespace MDM.Workflow.Activities.Designer
{
    /// <summary>
    /// Interaction logic for ActivityActionsEditorWindow.xaml
    /// </summary>
    public partial class ActivityActionsEditorWindow : Window
    {
        #region Fields

        private Int32 _selectedIndex = -1;

        private ActivityAction _selectedItem = null;

        private String _actionsDetails = String.Empty;

        private ObservableCollection<ActivityAction> _activityActionsCollection = null;

        #endregion

        #region Properties

        public String ActionsDetails
        {
            get
            {
                return _actionsDetails;
            }
            set
            {
                _actionsDetails = value;
            }
        }

        #endregion

        #region Constructor

        public ActivityActionsEditorWindow(String actionDetails)
        {
            InitializeComponent();

            this.Focus();
            this.Activate();

            _activityActionsCollection = new ObservableCollection<ActivityAction>();

            try
            {
                if (!String.IsNullOrEmpty(actionDetails))
                {
                    String[] actionRows = actionDetails.Split(";".ToCharArray());

                    foreach (String actionRow in actionRows)
                    {
                        String[] actionColumns = actionRow.Split(",".ToCharArray());

                        ActivityAction activityAction = new ActivityAction();
                        activityAction.Name = actionColumns[0].ToString();

                        CommentsRequired commentsRequired = CommentsRequired.None;
                        Enum.TryParse<CommentsRequired>(actionColumns[1].ToString(), out commentsRequired);
                        activityAction.CommentsRequired = commentsRequired;

                        //Read TransitionMessageCode only if it is available.
                        if (actionColumns.Length > 2)
                        {
                            activityAction.TransitionMessageCode = actionColumns[2].ToString();
                        }
                        _activityActionsCollection.Add(activityAction);
                    }
                }
            }
            catch
            {

            }

            cbAddCommentsRequired.ItemsSource = System.Enum.GetValues(typeof(CommentsRequired));
            cbAddCommentsRequired.SelectedItem = CommentsRequired.None;

            cbShowCommentsRequired.ItemsSource = System.Enum.GetValues(typeof(CommentsRequired));
            cbShowCommentsRequired.SelectedItem = CommentsRequired.None;

            dgActivityActions.ItemsSource = _activityActionsCollection;
        }

        #endregion

        #region Events

        private void dgActivityActions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                _selectedItem = e.AddedItems[0] as ActivityAction;

                if (_selectedItem != null)
                {
                    txtShowActionName.Text = _selectedItem.Name;
                    cbShowCommentsRequired.SelectedItem = _selectedItem.CommentsRequired;
                    txtShowTransitionMessageCode.Text = _selectedItem.TransitionMessageCode;
                }

                _selectedIndex = -1;
                foreach (ActivityAction action in _activityActionsCollection)
                {
                    _selectedIndex++;

                    if (action.Name == _selectedItem.Name && action.CommentsRequired == _selectedItem.CommentsRequired && action.TransitionMessageCode == _selectedItem.TransitionMessageCode)
                        break;
                }
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sbActionDetails = new StringBuilder();

            Int32 count = 0;
            foreach (ActivityAction activityAction in dgActivityActions.Items)
            {
                count++;
                sbActionDetails.Append(activityAction.Name);
                sbActionDetails.Append(",");
                sbActionDetails.Append(activityAction.CommentsRequired.ToString());
                sbActionDetails.Append(",");
                sbActionDetails.Append(activityAction.TransitionMessageCode);

                if (count < dgActivityActions.Items.Count)
                    sbActionDetails.Append(";");
            }

            this.ActionsDetails = sbActionDetails.ToString();

            this.DialogResult = true;
            this.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (txtAddActionName.Text != "" && cbAddCommentsRequired.SelectedItem != null)
            {
                ActivityAction activityAction = new ActivityAction();
                activityAction.Name = txtAddActionName.Text;
                activityAction.CommentsRequired = ( CommentsRequired )cbAddCommentsRequired.SelectedItem;
                activityAction.TransitionMessageCode = txtAddTransitionMessageCode.Text;

                _activityActionsCollection.Add(activityAction);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedIndex > -1)
            {
                //remove the selectedItem from the collection source
                _activityActionsCollection.RemoveAt(_selectedIndex);

                //reset all the selections
                _selectedItem = null;
                _selectedIndex = -1;
                txtShowActionName.Text = String.Empty;
                cbShowCommentsRequired.SelectedItem = CommentsRequired.None;
                txtShowTransitionMessageCode.Text = String.Empty;
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedIndex > -1 && txtShowActionName.Text != "" && cbShowCommentsRequired.SelectedItem != null)
            {
                // get the selected's information first
                ActivityAction activityAction = new ActivityAction();

                activityAction.Name = txtShowActionName.Text;
                activityAction.CommentsRequired = ( CommentsRequired )cbShowCommentsRequired.SelectedItem;
                activityAction.TransitionMessageCode = txtShowTransitionMessageCode.Text;

                // remove the old information
                _activityActionsCollection.RemoveAt(_selectedIndex);

                //add a new action
                _activityActionsCollection.Insert(_selectedIndex, activityAction);

                _selectedItem = activityAction;
            }
        }

        #endregion

        //protected override bool RunDialog(IntPtr hwndOwner)
        //{
        //    return true;
        //}

        //public override void Reset()
        //{
        //}
    }
}
