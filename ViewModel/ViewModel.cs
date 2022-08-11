using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Recorder.Model;
using Recorder.Service;

namespace Recorder.ViewModel
{
    public class ViewModel : ViewModelBase
    {
        private readonly List<IModel> _model = new();
        private readonly IWindowService _windowService;
        private readonly IMessageBoxService _messageService;

        // TODO: Może wstrzyknąć modelom Bitmapy zamiast trzymać tablicę w menedżerze
        public ViewModel(IWindowService windowService, IMessageBoxService messageService)
        {
            try
            {
                throw new NotImplementedException();
                _model?.Add(new ModelImpl());
                _model?.ForEach(m => m.SetOnPreviewImageChanged(OnPreviewImageChanged));
            }
            catch (Exception)
            {
                _model?.Add(new FakeModel()); // When without a device.
                _model?.Add(new FakeModel());
                _model?.Add(new FakeModel());
            }

            _windowService = windowService;
            _messageService = messageService;
            //_kinect.ActionsOnPreviewFrameReady.Add(OnPreviewImageChanged);
            //if (_recorder.State == RecorderStates.Ready)
            //    _recorder.StartPreview();
        }

        #region Binded properties.

        public List<List<object>> RecorderData =>
            RecorderState.Zip(RecordedImage, (k, v) => new List<object>{k, v}).ToList();

        public string GeneralState => _model.Select(m => m.State).Distinct().Min().ToString();

        public List<string> RecorderState => _model.Select(m => m.State.ToString()).ToList();

        public List<WriteableBitmap[]> RecordedImage => _model.Select(m => m.ColorBitmaps).ToList();

        #endregion

        #region Property changed notification.

        public void OnPreviewImageChanged()
        {
            _onPropertyChanged(nameof(RecordedImage));
        }

        #endregion

        #region Commands.

        private ICommand? _startRecording;
        public ICommand StartRecording
        {
            get
            {
                return _startRecording ??= new RelayCommand(
                    o =>
                    {
                        _model.ForEach(m => m.RecordingMode());
                        _onPropertyChanged(nameof(RecorderState));
                    },
                    o => _model.All(m => m.State is Model.RecorderState.Ready or Model.RecorderState.Preview));
            }

        }

        private ICommand? _stopRecording;
        public ICommand StopRecording
        {
            get
            {
                return _stopRecording ??= new RelayCommand(
                    o =>
                    {
                        _model.ForEach(m => m.PreviewMode());
                        _onPropertyChanged(nameof(RecorderState));
                    },
                    o => _model.All(m => m.State == Model.RecorderState.Recording));
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
