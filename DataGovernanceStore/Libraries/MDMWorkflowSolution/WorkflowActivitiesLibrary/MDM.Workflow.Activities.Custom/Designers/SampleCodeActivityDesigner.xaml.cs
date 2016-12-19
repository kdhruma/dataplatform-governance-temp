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

namespace MDM.Workflow.Activities.Custom.Designers
{
    // Interaction logic for SampleCodeActivityDesigner.xaml
    public partial class SampleCodeActivityDesigner
    {
        /// <summary>
        /// Constructors
        /// </summary>
        public SampleCodeActivityDesigner()
        {
            InitializeComponent();

            //TODO::Change the Image name with the actual name here.. 

            //Format for specifying resource Name - <Namespace>.<ImageFolderName>.<ImageNameWithExtension>

            //Get the Icon for the designer
            Stream manifestResourceStream = typeof(SampleCodeActivityDesigner).Module.Assembly.GetManifestResourceStream("MDM.Workflow.Activities.Custom.Designers.Images.SampleActivityImage.bmp");

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
