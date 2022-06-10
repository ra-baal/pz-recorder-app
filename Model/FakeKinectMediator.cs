using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Recorder.Model
{

    public class FakeKinectMediator : IKinectMediator
    {
        public RecorderStates State { get; private set; }
        public WriteableBitmap ColorBitmap { get; private set; }

        public FakeKinectMediator(Action onPreviewFrameReady)
        {

        }

        public void RecordingMode()
        {

        }

        public void PreviewMode()
        {

        }
    }
}
