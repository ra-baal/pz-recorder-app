﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using Recorder.Model;

namespace Recorder.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        private KinectRecorder _recorder;

        public ViewModel()
        {
            _recorder = new KinectRecorder();
            _recorder.ActionsOnColorFrameReady.Add(OnRecordedImageChanged);
            if (_recorder.State == RecorderStates.Ready)
                _recorder.Start();
        }

        #region Binded properties.

        //public bool IsRecorderReady
        //{
        //    get
        //    {
        //        //return _recorder.IsReadyOrRecording;
        //        return _recorder.State == RecorderStates.Ready;
        //    }
        //}

        public string RecorderState => _recorder.State.ToString();

        public WriteableBitmap RecordedImage => _recorder.ColorBitmap;

        #endregion

        #region Property changed notification.

        public event PropertyChangedEventHandler PropertyChanged;
        protected void _onPropertyChanged(params string[] propertyNames)
        {
            if (PropertyChanged != null)
            {
                foreach (string name in propertyNames)
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        
        public void OnRecordedImageChanged()
        {
            _onPropertyChanged(nameof(RecordedImage));
        }

        #endregion

        #region Commands.

        private ICommand _startRecording;
        public ICommand StartRecording
        {
            get
            {
                if (_startRecording == null)
                    _startRecording = new RelayCommand(
                        o =>
                        {
                            //_recorder.Start();
                            _recorder.StartRecording();
                            _onPropertyChanged(nameof(RecorderState));
                        },
                        o => _recorder.State == RecorderStates.Ready);

                return _startRecording;
            }

        }

        private ICommand _stopRecording;
        public ICommand StopRecording
        {
            get
            {
                if (_stopRecording == null)
                    _stopRecording = new RelayCommand(
                        o =>
                        {
                            _recorder.Stop();
                            _onPropertyChanged(nameof(RecorderState));
                        },
                        o => _recorder.State == RecorderStates.Recording);

                return _stopRecording;
            }

        }

        #endregion

    }
}
