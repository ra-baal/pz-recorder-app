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
        [DllImport(_dll)] public static extern int RecordingManager_Version();
        [DllImport(_dll)] public static extern int RecordingManager_Test(int a, int b);
        [DllImport(_dll)] private static extern void* RecordingManager_New();
        [DllImport(_dll)] private static extern void RecordingManager_Delete(void* recordingManager);
        [DllImport(_dll)] private static extern int RecordingManager_GetRecordersNumber(void* recordingManager);
        [DllImport(_dll)] private static extern Colors* RecordingManager_GetColorBitmaps(void* recordingManager);
        [DllImport(_dll)] private static extern void RecordingManager_StartRecording(void* recordingManager);
        [DllImport(_dll)] private static extern void RecordingManager_StopRecording(void* recordingManager);
        [DllImport(_dll)] private static extern void RecordingManager_SetDirectory(void* recordingManager, string str);
        #endregion

        #region Handlers.

        private void* _objptr;
        private bool _disposedValue;
        private WriteableBitmap[]? _colorBitmaps;
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
            Colors* colors = RecordingManager_GetColorBitmaps(_objptr);

            // kinect v1
            colorsToBitmap(colors, 0);

            // kinect v2
            colorsToBitmap(colors, 1);

            return _colorBitmaps;
        }

        private void colorsToBitmap(Colors* colorsArray, int recorder_bitmap)
        {
            Colors colors = colorsArray[recorder_bitmap];
            System.Windows.Media.PixelFormat format;
            int bytesPerPixel;

            switch (colors.Format)
            {
                case ColorFormat.UnknownFormat:

                    format = PixelFormats.Rgb24;
                    if (_colorBitmaps[recorder_bitmap] == null)
                        _colorBitmaps[recorder_bitmap] = new WriteableBitmap(
                            colors.Width,
                            colors.Height,
                            _dpi,
                            _dpi,
                            format,
                            null);
                    bytesPerPixel = 3;
                    byte[] pixels = new byte[colors.Width * colors.Height * bytesPerPixel];
                    _colorBitmaps[recorder_bitmap].WritePixels(
                        new Int32Rect(0, 0, _colorBitmaps[recorder_bitmap].PixelWidth, _colorBitmaps[recorder_bitmap].PixelHeight),
                        pixels,
                        _colorBitmaps[recorder_bitmap].PixelWidth * bytesPerPixel,
                        0);
                    return;
                    //throw new ArgumentException("Nieznany format piksela");
                case ColorFormat.Rgb24:
                    format = PixelFormats.Rgb24;
                    bytesPerPixel = 3;
                    break;
                case ColorFormat.Bgr32:
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


            byte[] colorPixels = new byte[colors.Width * colors.Height * bytesPerPixel];

            for (int i = 0; i < colorPixels.Length; i++)
                colorPixels[i] = *(((byte*)colors.Data) + i);

            _colorBitmaps[recorder_bitmap].WritePixels(
                new Int32Rect(0, 0, _colorBitmaps[recorder_bitmap].PixelWidth, _colorBitmaps[recorder_bitmap].PixelHeight),
                colorPixels,
                _colorBitmaps[recorder_bitmap].PixelWidth * bytesPerPixel,
                0);

        }

        public void StartRecording() => RecordingManager_StartRecording(_objptr);

        public void StopRecording() => RecordingManager_StopRecording(_objptr);

        /// <summary>
        /// Nazwa katalogu lub cała ścieżka do katalogu, gdzie będą tworzone katalogi z filmami.
        /// </summary>
        public void SetDirectory(string directory) => RecordingManager_SetDirectory(_objptr, directory);

        // atrapa
        //public RecorderState[] GetStates() => new RecorderState[] { RecorderState.Unknown, RecorderState.Unknown };

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


    }

}
