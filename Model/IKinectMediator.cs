using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Recorder.Model
{
    public interface IKinectMediator
    {
        // Interface requires public properties only.
        RecorderStates State { get; }
        WriteableBitmap ColorBitmap { get; }

        void RecordingMode();

        void PreviewMode();

    }

}
