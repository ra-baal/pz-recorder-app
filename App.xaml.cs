using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Ninject;
using Recorder.Service;

namespace Recorder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            RecorderApplication.Current.Bootstrap();
            RecorderApplication.Current.Container.Get<IWindowService>().OpenWindow<ViewModel.ViewModel>("MainWindow");
        }
    }
}
