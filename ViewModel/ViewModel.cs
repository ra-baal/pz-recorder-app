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
        //private readonly List<IModel> _model = new();
        private readonly IModel _model;

        private readonly IWindowService _windowService;
        private readonly IMessageBoxService _messageService;

        // TODO: Może wstrzyknąć modelom Bitmapy zamiast trzymać tablicę w menedżerze
        public ViewModel(
            IWindowService windowService, 
            IMessageBoxService messageService,
            IModel model)
        {
            _model = model;

            _windowService = windowService;
            _messageService = messageService;

            _model.SetOnPreviewImageChanged(OnPreviewImageChanged);
        }

        #region Binded properties.

        //public List<List<object>> RecorderData =>
        //    RecorderState.Zip(RecordedImage, (k, v) => new List<object> { k, v }).ToList();
        public List<List<object>> RecorderData
        {
            get
            {
                WriteableBitmap[] images = _model.ColorBitmaps;

                var objs = new List<List<object>>();

                foreach (var image in images)
                    objs.Add(new List<object> { "", image });

                return objs;
            }
        }

        //public string GeneralState => _model.Select(m => m.State).Distinct().Min().ToString();
        public string GeneralState => _model.State.ToString();

        //public List<string> RecorderState => _model.Select(m => m.State.ToString()).ToList();
        public List<string> RecorderState => new List<string> { $"Recorders: {_model.RecorderNumber}" , "test"};

        //public List<WriteableBitmap[]> RecordedImage => _model.Select(m => m.ColorBitmaps).ToList();
        public WriteableBitmap[] RecordedImage => _model.ColorBitmaps ;

        #endregion

        #region Property changed notification.

        public void OnPreviewImageChanged()
        {
            //_onPropertyChanged(nameof(RecordedImage));
            _onPropertyChanged(nameof(RecorderData));
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
                        //_model.ForEach(m => m.RecordingMode());
                        _model.RecordingMode();
                        _onPropertyChanged(nameof(RecorderState));
                    },
                    //o => _model.All(m => m.State is Model.RecorderState.Ready or Model.RecorderState.Preview));
                    o => _model.State == Model.RecorderState.Ready || _model.State == Model.RecorderState.Preview);
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
                        //_model.ForEach(m => m.PreviewMode());
                        _model.PreviewMode();
                        _onPropertyChanged(nameof(RecorderState));
                    },
                    //o => _model.All(m => m.State == Model.RecorderState.Recording));
                    o => _model.State == Model.RecorderState.Recording);
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
