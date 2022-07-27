using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recorder.Model;
using Recorder.Service;

namespace Recorder
{
    /// <summary>
    /// Default module for the application
    /// </summary>
    public class DependencyContainer : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IWindowService>().To<WindowService>();
            Bind<IMessageBoxService>().To<MessageBoxService>();
            Bind<IKinectCloudRecorder>().To<KinectCloudRecorder>();
        }
    }
}
