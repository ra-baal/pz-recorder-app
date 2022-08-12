using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Recorder.Model
{
    public class FakeRecordingManager : IRecordingManager
    {
        public WriteableBitmap[] GetColorBitmaps()
        {
            return new WriteableBitmap[]
            {
                new WriteableBitmap(640, 480, 96, 96, PixelFormats.Bgr32, null),
                new WriteableBitmap(640, 480, 96, 96, PixelFormats.Bgr32, null),
                new WriteableBitmap(640, 480, 96, 96, PixelFormats.Bgr32, null)
            };
        }

        public int GetRecordersNumber()
        {
            return 3;
        }

        public void SetDirectory(string directory)
        {
            // pass
        }

        public void StartRecording()
        {
            // pass
        }

        public void StopRecording()
        {
            // pass
        }
    }
}