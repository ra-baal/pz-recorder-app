using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Recorder.Model;
using Recorder.Service;

namespace Recorder.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly IModel _model;
        private readonly IWindowService _windowService;
        private readonly IMessageBoxService _messageService;

        public SettingsViewModel(ModelImpl model, IWindowService windowService, IMessageBoxService messageService)
        {
            _model = model;
            _windowService = windowService;
            _messageService = messageService;
        }

        private string? _tmpDirectoryName;
        private string? _tmpFilePrefix;
        private string? _tmpPath;

        public string DirectoryName
        {
            get => _tmpDirectoryName ??= _model.DirectoryPrefix;
            set => _tmpDirectoryName = value;
        }

        public string FilePrefix
        {
            get => _tmpFilePrefix ??= _model.FilePrefix;
            set => _tmpFilePrefix = value;
        }

        public string Path
        {
            get => _tmpPath ??= _model.Path;
            set => _tmpPath = value;
        }

        private ICommand? _openFolderDialog;
        private ICommand? _save;
        private ICommand? _close;

        public ICommand OpenFolderDialog
        {
            get
            {
                return _openFolderDialog ??= new RelayCommand(o =>
                {
                    FolderBrowserDialog dialog = new FolderBrowserDialog();
                    dialog.RootFolder = Environment.SpecialFolder.MyComputer;
                    dialog.SelectedPath = Path;
                    if (dialog.ShowDialog() != DialogResult.OK) return;
                    Path = dialog.SelectedPath;
                    _onPropertyChanged(nameof(Path));
                });
            }
        }

        public ICommand Save
        {
            get
            {
                return _save ??= new RelayCommand(o =>
                {
                    if (_tmpDirectoryName != null) _model.DirectoryPrefix = _tmpDirectoryName;
                    if (_tmpFilePrefix != null) _model.FilePrefix = _tmpFilePrefix;
                    if (_tmpPath != null) _model.Path = _tmpPath;
                    _onPropertyChanged(nameof(DirectoryName), nameof(FilePrefix), nameof(Path));
                    if (o is Window w) w.Close();
                });
            }
        }

        public ICommand Close
        {
            get
            {
                return _close ??= new RelayCommand(o =>
                {
                    _tmpDirectoryName = null;
                    _tmpFilePrefix = null;
                    _tmpPath = null;
                    if (o is Window w) w.Close();
                });
            }
        }
    }
}
