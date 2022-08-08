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
        private WriteableBitmap _colorBitmap0;
        private WriteableBitmap _colorBitmap1;

        public RecordingManager()
        {
            _objptr = RecordingManager_New();

            if (_objptr == null)
                throw new ExternalException();

            _disposedValue = false;

            _colorBitmap0 = null;
            _colorBitmap1 = null;

        }

        public int GetRecordersNumber() => RecordingManager_GetRecordersNumber(_objptr);

        public WriteableBitmap[] GetColorBitmaps()
        {

            Colors* colors = RecordingManager_GetColorBitmaps(_objptr);

            double dpi = 96.0; // Wartość z ColorBasics-WPF.

            /// 0 ///
            if (_colorBitmap0 == null)
                _colorBitmap0 = new WriteableBitmap(
                    colors[0].Width,
                    colors[0].Heigth,
                    dpi,
                    dpi,
                    PixelFormats.Bgr32, // to samo co RGBQUAD
                    null);

            // (v1) 
            // nie działa - Jak by tu zrobić bez pośredniej tablicy bajtów?
            //colorBitmap.WritePixels(
            //            new Int32Rect(0, 0, colorBitmap.PixelWidth, colorBitmap.PixelHeight),
            //            (IntPtr)colors.Data,
            //            colorBitmap.PixelWidth * sizeof(int),
            //            0);

            // (v2) 
            // - Tutaj można by pewnie użyć jakiejś funkcji kopiującej cały ciąg pamięci.
            // - Tablica jeśli już musi być, to powinna być polem.
            byte[] colorPixels0 = new byte[colors[0].Width * colors[0].Heigth * 4];


            try
            {
                for (int i = 0; i < colorPixels0.Length; i++)
                {
                    colorPixels0[i] = *(((byte*)colors[0].Data) + i);
                }
            }
            catch (Exception e)
            {
                // pass
            }

            _colorBitmap0.WritePixels(
                        new Int32Rect(0, 0, _colorBitmap0.PixelWidth, _colorBitmap0.PixelHeight),
                        colorPixels0,
                        _colorBitmap0.PixelWidth * sizeof(int),
                        0);


            /// 1 ///
            if (_colorBitmap1 == null)
                _colorBitmap1 = new WriteableBitmap(
                    colors[1].Width,
                    colors[1].Heigth,
                    dpi,
                    dpi,
                    PixelFormats.Bgr32, // to samo co RGBQUAD
                    null);

            byte[] colorPixels1 = new byte[colors[1].Width * colors[1].Heigth * 4];

            try
            {
                for (int i = 0; i < colorPixels1.Length; i++)
                {
                    colorPixels1[i] = *(((byte*)colors[1].Data) + i);
                }
            }
            catch (Exception e)
            {
                // pass
            }

            _colorBitmap1.WritePixels(
                        new Int32Rect(0, 0, _colorBitmap1.PixelWidth, _colorBitmap1.PixelHeight),
                        colorPixels1,
                        _colorBitmap1.PixelWidth * sizeof(int),
                        0);



            return new WriteableBitmap[]
            {
                _colorBitmap0,
                _colorBitmap1
            };
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
                _colorBitmap0 = null;
                _colorBitmap1 = null;

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
