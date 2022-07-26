using Recorder.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;


namespace Recorder.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        //private IKinectMediator _kinect;
        private IRecordingManager _recordingManager;

        public ViewModel()
        {
            _recordingManager = new RecordingManager();

            //_kinect = new CloudRecordingManager();
            //_kinect = new KinectMediator(OnPreviewImageChanged);
            //_kinect = new FakeKinectMediator(OnPreviewImageChanged); // When without a device.
            //_kinect.ActionsOnPreviewFrameReady.Add(OnPreviewImageChanged);
            //if (_recorder.State == RecorderStates.Ready)
            //_recorder.StartPreview();
        }


        #region Binded properties.

        public string RecorderState => _recordingManager.GetStates()[1].ToString();

        public WriteableBitmap RecordedImage =>_recordingManager.GetColorBitmap();

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
        
        // ToDo: Trzeba wrzucić do środka RecordingManagera, żeby odświeżało widok.
        public void OnPreviewImageChanged()
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
                            OnPreviewImageChanged(); // ToDo: Tymczasowo widok odświeżany przyciskiem start.

                            _recordingManager.RecordingMode();
                            _onPropertyChanged(nameof(RecorderState));

                        },
                        //o => _recordingManager.State == RecorderStates.Ready || _recordingManager.State == RecorderStates.Preview);
                        o => true);

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
                            _recordingManager.PreviewMode();
                            _onPropertyChanged(nameof(RecorderState));
                        },
                        //o => _recordingManager.State == RecorderStates.Recording);
                        o => true);

                return _stopRecording;
            }

        }

        #endregion

    }
}
