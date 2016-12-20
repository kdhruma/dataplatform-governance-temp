using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities.Presentation.PropertyEditing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MDM.Workflow.Activities.Designer
{
    public class ActivityActionsEditor : DialogPropertyValueEditor
    {
        public ActivityActionsEditor()
        {
            this.InlineEditorTemplate = new DataTemplate();

            FrameworkElementFactory stack = new FrameworkElementFactory(typeof(StackPanel));
            stack.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            FrameworkElementFactory label = new FrameworkElementFactory(typeof(Label));
            Binding labelBinding = new Binding("Value");
            label.SetValue(Label.ContentProperty, labelBinding);
            label.SetValue(Label.MaxWidthProperty, 90.0);

            stack.AppendChild(label);

            FrameworkElementFactory editModeSwitch = new FrameworkElementFactory(typeof(EditModeSwitchButton));

            editModeSwitch.SetValue(EditModeSwitchButton.TargetEditModeProperty, PropertyContainerEditMode.Dialog);

            stack.AppendChild(editModeSwitch);

            this.InlineEditorTemplate.VisualTree = stack;
        }

        public override void ShowDialog(PropertyValue propertyValue, IInputElement commandSource)
        {
            String actionDetails = String.Empty;

            if (propertyValue != null && propertyValue.Value != null)
                actionDetails = propertyValue.Value.ToString();

            ActivityActionsEditorWindow activityActionsEditorWindow = new ActivityActionsEditorWindow(actionDetails);

            activityActionsEditorWindow.Owner = Application.Current.MainWindow;
            if (activityActionsEditorWindow.ShowDialog() == true)
            {
                propertyValue.Value = activityActionsEditorWindow.ActionsDetails;
            }
        }
    }
}
