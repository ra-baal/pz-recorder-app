using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Recorder.Model
{
    [Obsolete]
    public enum RecorderStates
    {
        NoSensor,
        Ready,
        Preview,
        Recording
    }

    [Obsolete]
    public class KinectMediator : IKinectMediator
    {
        private KinectPreview _preview;
        private KinectCloudRecorder _cloudRecorder;

        public RecorderStates State { get; private set; }
        public WriteableBitmap ColorBitmap { get; private set; }

        private readonly List<Action> ActionsOnPreviewFrameReady;

        public KinectMediator(Action onPreviewFrameReady)
        {
            this.ActionsOnPreviewFrameReady = new List<Action>() { onPreviewFrameReady };

            PreviewMode();
        }

        private void _previewOn()
        {
            _preview = new KinectPreview
            {
                ActionsOnPreviewFrameReady = this.ActionsOnPreviewFrameReady
            };
            this.ColorBitmap = _preview.ColorBitmap;



            bool podglądAleNieNagrywaWielokrotnie = true;
            if (podglądAleNieNagrywaWielokrotnie)
                _preview.Start();


        }

        private void _previewOff()
        {
            _preview?.Stop();
            _preview = null;
        }

        private void _recordingOn()
        {
            _cloudRecorder = new KinectCloudRecorder();
            _cloudRecorder.Start();
        }

        private void _recordingOff()
        {
            _cloudRecorder?.Stop();
            _cloudRecorder = null;
        }

        public void RecordingMode()
        {
            _previewOff();
            _recordingOn();
            this.State = _cloudRecorder.State;
        }

        public void PreviewMode()
        {
            _recordingOff();
            _previewOn();
            this.State = _preview.State;
        }
    }
}
