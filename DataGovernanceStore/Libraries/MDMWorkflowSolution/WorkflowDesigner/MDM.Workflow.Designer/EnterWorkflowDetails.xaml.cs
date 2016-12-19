using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Interaction logic for EnterWorkflowDetails.xaml
    /// </summary>
    public partial class EnterWorkflowDetails : Window
    {
        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public EnterWorkflowDetails()
        {
            InitializeComponent();
        }

        #region Events

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tbWorkflowName.Text) || String.IsNullOrEmpty(tbWorkflowLongName.Text) || String.IsNullOrEmpty(tbWorkflowVersionName.Text))
                btnOK.IsEnabled = false;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void workflowDetails_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(tbWorkflowName.Text) && !String.IsNullOrEmpty(tbWorkflowLongName.Text) && !String.IsNullOrEmpty(tbWorkflowVersionName.Text))
                btnOK.IsEnabled = true;
            else
                btnOK.IsEnabled = false;
        }

        #endregion
    }
}
