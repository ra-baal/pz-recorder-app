using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Recorder.Model
{
    public unsafe class RecordingManager : IRecordingManager
    {
        #region DllImport.
        private const string _dll = "RecorderDll.dll";
        [DllImport(_dll)] public static extern int RecordingManager_Test(int a, int b);
        [DllImport(_dll)] private static extern void* RecordingManager_New();
        [DllImport(_dll)] private static extern void RecordingManager_Delete(void* recordingManager);
        [DllImport(_dll)] private static extern int RecordingManager_GetRecordersNumber(void* recordingManager);
        [DllImport(_dll)] private static extern Colors RecordingManager_GetColorBitmap(void* recordingManager);
        [DllImport(_dll)] private static extern void RecordingManager_RecordingMode(void* recordingManager);
        [DllImport(_dll)] private static extern void RecordingManager_PreviewMode(void* recordingManager);
        #endregion

        #region Handlers.

        private void* _objptr;

        public RecordingManager()
        {
            _objptr = RecordingManager_New();

            if (_objptr == null)
                throw new ExternalException();
        }

        public int GetRecordersNumber() => RecordingManager_GetRecordersNumber(_objptr);

        //public (byte b, byte g, byte r)[] GetColorBitmap()
        //{
        //    Colors colors = RecordingManager_GetColorBitmap(_objptr);

        //    // Ten kod też działa.
        //    (byte b, byte g, byte r)[] bitmap = new (byte r, byte g, byte b)[colors.Heigth * colors.Width];

        //    RGBQUAD* rgbquadPtr = colors.Data;
        //    for (int i = 0; i < colors.Width * colors.Heigth; i++)
        //    {
        //        byte* bytePtr = (byte*)rgbquadPtr; // RGBQUAD.rgbBlue
        //        bitmap[i].b = *bytePtr;

        //        bytePtr++; // RGBQUAD.rgbGreen
        //        bitmap[i].g = *bytePtr;

        //        bytePtr++; // RGBQUAD.rgbRed
        //        bitmap[i].r = *bytePtr;

        //        bytePtr++; // RGBQUAD.rgbReserved

        //        rgbquadPtr++; // Następne 4 bajty
        //    }

        //    return bitmap;
        //}

        public WriteableBitmap GetColorBitmap()
        {
            Colors colors = RecordingManager_GetColorBitmap(_objptr);

            double dpi = 96.0; // Wartość z ColorBasics-WPF.
            // - Bitmapa powinna być polem.
            WriteableBitmap colorBitmap = new WriteableBitmap(
                colors.Width,
                colors.Heigth,
                dpi,
                dpi,
                PixelFormats.Bgr32, // chyba to samo co RGBQUAD
                null);

            // (v1) 
            // nie działa - Jak by tu zrobić bez pośredniej tablicy bajtów?
            //colorBitmap.WritePixels(
            //            new Int32Rect(0, 0, colorBitmap.PixelWidth, colorBitmap.PixelHeight),
            //            (IntPtr)colors.Data,
            //            colorBitmap.PixelWidth * sizeof(int),
            //            0);

            // (v2) 
            // - Tutaj można by pewnie użyć jakiejś funkcji kopijącej cały ciąg pamięci.
            // - Tablica jeśli już musi być, to powinna być polem.
            byte[] colorPixels = new byte[colors.Width*colors.Heigth*4];
            for (int i = 0; i < colorPixels.Length; i++)
                colorPixels[i] = *(((byte*)colors.Data)+i);
            
            colorBitmap.WritePixels(
                        new Int32Rect(0, 0, colorBitmap.PixelWidth, colorBitmap.PixelHeight),
                        colorPixels,
                        colorBitmap.PixelWidth * sizeof(int),
                        0);

            return colorBitmap;
        }

        public void RecordingMode() => RecordingManager_PreviewMode(_objptr);

        public void PreviewMode() => RecordingManager_PreviewMode(_objptr);

        // atrapa
        public RecorderState[] GetStates() => new RecorderState[] { RecorderState.NoSensor, RecorderState.Ready };

        #endregion


    }
}
