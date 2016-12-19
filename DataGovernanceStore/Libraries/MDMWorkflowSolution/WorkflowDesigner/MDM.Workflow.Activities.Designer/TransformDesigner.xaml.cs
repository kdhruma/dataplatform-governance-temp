using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MDM.Workflow.Activities.Designer
{
    /// <summary>
    /// Interaction logic for TransformDesigner.xaml
    /// </summary>
    public partial class TransformDesigner
    {
        /// <summary>
        /// 
        /// </summary>
        public TransformDesigner()
        {
            InitializeComponent();

            Stream manifestResourceStream = typeof(TransformDesigner).Module.Assembly.GetManifestResourceStream("MDM.Workflow.Activities.Designer.Images.tranform.bmp");

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