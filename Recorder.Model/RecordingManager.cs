using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Recorder.Model
{
    public unsafe class RecordingManager : IRecordingManager, IDisposable
    {
        #region DllImport.
        private const string _dll = "RecorderDll.dll";
        [DllImport(_dll)] public static extern int RecordingManager_Test(int a, int b);
        [DllImport(_dll)] private static extern void* RecordingManager_New();
        [DllImport(_dll)] private static extern void RecordingManager_Delete(void* recordingManager);
        [DllImport(_dll)] private static extern int RecordingManager_GetRecordersNumber(void* recordingManager);
        [DllImport(_dll)] private static extern Colors* RecordingManager_GetColorBitmaps(void* recordingManager);
        [DllImport(_dll)] private static extern void RecordingManager_StartRecording(void* recordingManager);
        [DllImport(_dll)] private static extern void RecordingManager_StopRecording(void* recordingManager);
        #endregion

        #region Handlers.

        private void* _objptr;
        private bool _disposedValue;
        private WriteableBitmap[] _colorBitmaps;
        //private WriteableBitmap _colorBitmap1;

        private const double _dpi = 96.0; // Wartość z projektu Kinect SDK ColorBasics-WPF.


        public RecordingManager()
        {
            _objptr = RecordingManager_New();

            if (_objptr == null)
                throw new ExternalException();

            _disposedValue = false;

            _colorBitmaps = new WriteableBitmap[2]
            {
                null,
                null
            };

        }

        public int GetRecordersNumber() => RecordingManager_GetRecordersNumber(_objptr);

        public WriteableBitmap[] GetColorBitmaps()
        {
            // ToDo: lepsza konwersja/kopiowanie colors -> WriteableBitmap
            // (wer 1) 
            // nie działa - Jak by tu zrobić bez pośredniej tablicy bajtów?
            //colorBitmap.WritePixels(
            //            new Int32Rect(0, 0, colorBitmap.PixelWidth, colorBitmap.PixelHeight),
            //            (IntPtr)colors.Data,
            //            colorBitmap.PixelWidth * sizeof(int),
            //            0);

            // (wer 2) 
            // - Tutaj można by pewnie użyć jakiejś funkcji kopiującej cały ciąg pamięci.
            // - Tablica jeśli już musi być, to powinna być polem.

            Colors* colors = RecordingManager_GetColorBitmaps(_objptr);

            // kinect v1
            colorsToBgrBitmap(colors, 0);

            // kinect v2
            colorsToBgrBitmap(colors, 1);

            return _colorBitmaps;
        }

        private void colorsToBgrBitmap(Colors* colorsArray, int recorder_bitmap)
        {
            Colors colors = colorsArray[recorder_bitmap];
            System.Windows.Media.PixelFormat format;
            int bytesPerPixel;

            switch (colors.Format)
            {
                case PixelFormat.UnknownFormat:
                    throw new ArgumentException("Nieznany format piksela");
                case PixelFormat.RGB_888:
                    format = PixelFormats.Rgb24;
                    bytesPerPixel = 3;
                    break;
                case PixelFormat.BGR32:
                    format = PixelFormats.Bgr32;
                    bytesPerPixel = 4;
                    break;
                default:
                    throw new ArgumentException("Nieobsługiwany format piksela");
            }

            if (_colorBitmaps[recorder_bitmap] == null)
                _colorBitmaps[recorder_bitmap] = new WriteableBitmap(
                    colors.Width,
                    colors.Height,
                    _dpi,
                    _dpi,
                    format,
                    null);

            byte[] colorPixels0 = new byte[colors.Width * colors.Height * bytesPerPixel];

            for (int i = 0; i < colorPixels0.Length; i++)
                colorPixels0[i] = *(((byte*)colors.Data) + i);

            _colorBitmaps[recorder_bitmap].WritePixels(
                new Int32Rect(0, 0, _colorBitmaps[recorder_bitmap].PixelWidth, _colorBitmaps[recorder_bitmap].PixelHeight),
                colorPixels0,
                _colorBitmaps[recorder_bitmap].PixelWidth * bytesPerPixel,
                0);

        }

        public void RecordingMode() => RecordingManager_StartRecording(_objptr);

        public void PreviewMode() => RecordingManager_StopRecording(_objptr);

        // atrapa
        public RecorderState[] GetStates() => new RecorderState[] { RecorderState.NoSensor, RecorderState.Ready };


        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // free unmanaged resources (unmanaged objects) and override finalizer
                RecordingManager_Delete(_objptr);
                _objptr = null;

                // set large fields to null
                _colorBitmaps = null;

                _disposedValue = true;
            }
        }

        // override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~RecordingManager()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
        public string DirectoryPrefix
        {
            get => RecorderSettings._dirprefix;
            set => RecorderSettings._dirprefix = value;
        }

        public string FilePrefix
        {
            get => RecorderSettings._fileprefix;
            set => RecorderSettings._fileprefix = value;
        }

        public string Path
        {
            get => RecorderSettings._path;
            set => RecorderSettings._path = value;
        }
    }

}
