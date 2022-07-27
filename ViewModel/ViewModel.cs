using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using Recorder.Model;
using Recorder.Service;
using Utilities;

namespace Recorder.ViewModel
{
    public class ViewModel : ViewModelBase
    {
        private readonly IKinectMediator _kinect;
        private readonly IWindowService _windowService;
        private readonly IMessageBoxService _messageService;

        public ViewModel(IWindowService windowService, IMessageBoxService messageService)
        {
            try
            {
                _kinect = new KinectMediator(OnPreviewImageChanged);
            }
            catch (NullReferenceException)
            {
                _kinect = new FakeKinectMediator(OnPreviewImageChanged); // When without a device.
            }
            _windowService = windowService;
            _messageService = messageService;
            //_kinect.ActionsOnPreviewFrameReady.Add(OnPreviewImageChanged);
            //if (_recorder.State == RecorderStates.Ready)
            //    _recorder.StartPreview();
        }

        #region Binded properties.

        public string RecorderState => _kinect.State.ToString();

        public WriteableBitmap RecordedImage => _kinect.ColorBitmap;

        #endregion

        #region Property changed notification.

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
                            _kinect.RecordingMode();
                            _onPropertyChanged(nameof(RecorderState));
                        },
                        o => _kinect.State == RecorderStates.Ready || _kinect.State == RecorderStates.Preview);

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
                            _kinect.PreviewMode(); 
                            _onPropertyChanged(nameof(RecorderState));
                        },
                        o => _kinect.State == RecorderStates.Recording);

                return _stopRecording;
            }

        }

        /// <summary>
        /// Gets the open window command
        /// </summary>
        public ICommand OpenWindowCommand
        {
            get
            {
                return new RelayCommand(
                    o =>
                    {
                        this._windowService.OpenWindow<SettingsViewModel>("SettingsWindow");
                    });
            }
        }

        /// <summary>
        /// Gets the open dialog command
        /// </summary>
        public ICommand OpenDialogCommand
        {
            get
            {
                return new RelayCommand(
                    o =>
                    {
                        this._windowService.OpenDialog<SettingsViewModel>("SettingsWindow");
                    });
            }
        }

        #endregion

    }
}
