﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Threading;

namespace Recorder.Model
{
    // Nazwa z sufiksem z powodu kolizji z nazwą przestrzeni nazw.
    public class ModelImpl : IModel
    {
        private readonly IRecordingManager _manager;
        public List<RecorderState> States { get; private set; }

        public ModelImpl()
        {
            try
            {
                //throw new Exception("");
                _manager = new RecordingManager();
                // Ogólnie to po stronie biblioteki obiekt managera powinien się tworzyć
                // nawet w przypadku braku kinectów, więc ten wyjątek tutaj pewnie się nie zdarzy.
            }
            catch (Exception)
            {
                _manager = new FakeRecordingManager();
            }

            States = new List<RecorderState>();
            for (int i = 0; i < _manager.GetRecordersNumber(); i++)
                States.Add(RecorderState.Ready);
        }

        private Timer _timer;

        //private RecorderState _state = RecorderState.Ready;

        void IModel.SetOnPreviewImageChanged(Action onPreviewImageChanged)
        {
            const int previewRefreshing = 200;

            _timer = new Timer((e) =>
            {
                onPreviewImageChanged();
            },
            null, TimeSpan.Zero, TimeSpan.FromMilliseconds(previewRefreshing));
        }


        WriteableBitmap[] IModel.ColorBitmaps => _manager.GetColorBitmaps();

        public int RecorderNumber => _manager.GetRecordersNumber();

        void IModel.PreviewMode()
        {
            //_state = RecorderState.Preview;
            for (int i = 0; i < States.Count; i++)
                States[i] = RecorderState.Preview;

            _manager.StopRecording();
        }

        void IModel.RecordingMode()
        {
            //_state = RecorderState.Recording;
            for (int i = 0; i < States.Count; i++)
                States[i] = RecorderState.Recording;

            _manager.StartRecording();
        }

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
