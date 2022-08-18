using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Recorder.Model
{
    /// <summary>
    /// Wymagania modelu widoku wobec modelu.
    /// </summary>
    public interface IModel
    {
        void SetOnPreviewImageChanged(Action onPreviewImageChanged);
        List<RecorderState> State { get; }
        WriteableBitmap[] ColorBitmaps { get; }
        void RecordingMode();
        void PreviewMode();
        int RecorderNumber { get; }

        string DirectoryPrefix { get; set; }
        string FilePrefix { get; set; }
        string Path { get; set; }
    }
}
