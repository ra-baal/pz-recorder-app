using System;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Recorder.Model;
using Recorder.Service;

namespace Recorder.ViewModel
{
    public class ViewModel : ViewModelBase
    {
        private readonly IModel _model;
        private readonly IWindowService _windowService;
        private readonly IMessageBoxService _messageService;

        public ViewModel(IWindowService windowService, IMessageBoxService messageService)
        {
            try
            {
                _model = new ModelImpl();
                _model.SetOnPreviewImageChanged(OnPreviewImageChanged);
            }
            catch (Exception)
            {
                _model = new FakeModel(); // When without a device.
            }

            _windowService = windowService;
            _messageService = messageService;
            //_kinect.ActionsOnPreviewFrameReady.Add(OnPreviewImageChanged);
            //if (_recorder.State == RecorderStates.Ready)
            //    _recorder.StartPreview();
        }

        #region Binded properties.

        public string RecorderState => _model.State.ToString();

        public WriteableBitmap RecordedImage => _model.ColorBitmaps[0];

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
                            _model.RecordingMode();
                            _onPropertyChanged(nameof(RecorderState));
                        },
                        o => _model.State == Model.RecorderState.Ready || _model.State == Model.RecorderState.Preview);
                
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
                            _model.PreviewMode(); 
                            _onPropertyChanged(nameof(RecorderState));
                        },
                        o => _model.State == Model.RecorderState.Recording);

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
