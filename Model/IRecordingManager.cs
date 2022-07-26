

using System.Windows.Media.Imaging;

namespace Recorder.Model
{
    public interface IRecordingManager
    {
        int GetRecordersNumber();
        RecorderState[] GetStates();
        WriteableBitmap GetColorBitmap();
        //(byte b, byte g, byte r)[] GetColorBitmap();
        void RecordingMode();
        void PreviewMode();

    }
}
