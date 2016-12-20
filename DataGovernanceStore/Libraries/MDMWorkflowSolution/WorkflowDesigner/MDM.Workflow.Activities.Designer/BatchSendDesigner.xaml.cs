using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MDM.Workflow.Activities.Designer
{
    /// <summary>
    ///Interaction logic for BatchSendDesigner.xaml 
    /// </summary>
    public partial class BatchSendDesigner
    {
        /// <summary>
        /// 
        /// </summary>
        public BatchSendDesigner()
        {
            InitializeComponent();

            Stream manifestResourceStream = typeof(BatchSendDesigner).Module.Assembly.GetManifestResourceStream("MDM.Workflow.Activities.Designer.Images.batchSend.bmp");

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