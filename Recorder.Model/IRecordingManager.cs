using System.Windows.Media.Imaging;

namespace Recorder.Model
{
    /// <summary>
    /// Wymagania modelu wobec API biblioteki dll?
    /// Być może zbędne.
    /// </summary>
    public interface IRecordingManager
    {
        int GetRecordersNumber();
        RecorderState[] GetStates();
        WriteableBitmap[] GetColorBitmaps();
        //(byte b, byte g, byte r)[] GetColorBitmap();
        void RecordingMode();
        void PreviewMode();
    }
}
