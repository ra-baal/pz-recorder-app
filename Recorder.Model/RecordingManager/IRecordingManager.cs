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
        //RecorderState[] GetStates();
        WriteableBitmap[] GetColorBitmaps();
        public void StartRecording();
        public void StopRecording();
        public void SetDirectory(string directory);
    }
}
