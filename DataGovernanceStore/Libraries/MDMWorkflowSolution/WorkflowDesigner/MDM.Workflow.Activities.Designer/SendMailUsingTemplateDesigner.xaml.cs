using System;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MDM.Workflow.Activities.Designer
{
    /// <summary>
    /// Interaction logic for SendMailUsingTemplateDesigner.xaml
    /// </summary>
    public partial class SendMailUsingTemplateDesigner
    {
        public SendMailUsingTemplateDesigner()
        {
            InitializeComponent();

            //Get the Icon for the designer
            Stream manifestResourceStream = typeof(SendMailDesigner).Module.Assembly.GetManifestResourceStream("MDM.Workflow.Activities.Designer.Images.SendMailUsingTemplate.bmp");

            if (manifestResourceStream != null)
            {
                var bmpframe = BitmapFrame.Create(manifestResourceStream);

                this.Icon = new DrawingBrush
                {
                    Drawing = new ImageDrawing
                    {
                        Rect = new System.Windows.Rect(0, 0, 16, 16),
                        ImageSource = bmpframe
                    }
                };
            }
        }
    }
}
