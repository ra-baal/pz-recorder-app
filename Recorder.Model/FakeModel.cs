using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Recorder.Model
{
    public class FakeModel : IModel
    {
        void IModel.SetOnPreviewImageChanged(Action onPreviewImageChanged) { /*pass*/ }

        RecorderState IModel.State => RecorderState.NoSensor;

        WriteableBitmap IModel.ColorBitmap => new WriteableBitmap(640, 480, 96, 96, PixelFormats.Bgr32, null);

        void IModel.PreviewMode() { /*pass*/ }

        void IModel.RecordingMode() { /*pass*/ }


    }
}
