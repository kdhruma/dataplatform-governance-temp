using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for SendDesigner.xaml
    /// </summary>
    public partial class SendDesigner 
    {
        /// <summary>
        /// 
        /// </summary>
        public SendDesigner()
        {
            InitializeComponent();

            Stream manifestResourceStream = typeof(TransformDesigner).Module.Assembly.GetManifestResourceStream("MDM.Workflow.Activities.Designer.Images.send.bmp");

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
