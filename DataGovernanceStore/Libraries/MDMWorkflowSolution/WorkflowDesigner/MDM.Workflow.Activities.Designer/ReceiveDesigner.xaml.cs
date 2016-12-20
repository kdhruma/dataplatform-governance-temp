using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MDM.Workflow.Activities.Designer
{
    /// <summary>
    ///Interaction logic for ReceiveDesigner.xaml 
    /// </summary>
    public partial class ReceiveDesigner
    {
        public ReceiveDesigner()
        {
            InitializeComponent();

            Stream manifestResourceStream = typeof(ReceiveDesigner).Module.Assembly.GetManifestResourceStream("MDM.Workflow.Activities.Designer.Images.receive.bmp");

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
