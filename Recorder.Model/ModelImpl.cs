using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Threading;

namespace Recorder.Model
{
    // Nazwa z sufiksem z powodu kolizji z nazwą przestrzeni nazw.
    public class ModelImpl : RecordingManager, IModel
    {
        private Timer _timer;

        void IModel.SetOnPreviewImageChanged(Action onPreviewImageChanged)
        {
            const int previewRefreshing = 200;

            _timer = new Timer((e) =>
            {
                onPreviewImageChanged();
            },
            null, TimeSpan.Zero, TimeSpan.FromMilliseconds(previewRefreshing));
        }

        RecorderState IModel.State => base.GetStates()[1];

        WriteableBitmap IModel.ColorBitmap => base.GetColorBitmap();

        void IModel.PreviewMode() => base.PreviewMode();

        void IModel.RecordingMode() => base.RecordingMode();
    }
}
