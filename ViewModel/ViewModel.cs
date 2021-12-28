using System;
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
            _recorder.ActionsWhenColorFrameReady.Add(OnRecordedImageChanged);
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

        public string RecorderState
        {
            get
            {
                return _recorder.State.ToString();
            }
        }

        public WriteableBitmap RecordedImage
        {
            get
            {
                return _recorder.ColorBitmap; 
            }
        }

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
                            _recorder.Start();
                        },
                        o =>
                        {
                            return _recorder.State == RecorderStates.Ready;
                        });

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
                        },
                        o =>
                        {
                            return _recorder.State == RecorderStates.Recording;
                        });

                return _stopRecording;
            }

        }

        #endregion

    }
}
