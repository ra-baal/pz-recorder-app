using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Security.Tokens;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Ninject;
using Ninject.Modules;
using Recorder.Service;

namespace Recorder
{
    /// <summary>
    /// Runtime Information about the current application
    /// </summary>
    public class RecorderApplication
    {
        /// <summary>
        /// The current application instance
        /// </summary>
        private static RecorderApplication _current;

        /// <summary>
        /// Indicates whether application has been bootstrapped
        /// </summary>
        private bool hasBootstapped = false;

        /// <summary>
        /// Gets the current application instance
        /// </summary>
        public static RecorderApplication Current => _current ??= new RecorderApplication();

        /// <summary>
        /// Prevents creation of RecorderApplication instances
        /// </summary>
        private RecorderApplication()
        {

        }

        /// <summary>
        /// Gets the dependency container
        /// </summary>
        public IKernel  Container { get; private set; }

        /// <summary>
        /// Bootstraps the application and prepares the dependency container
        /// </summary>
        public void Bootstrap()
        {
            if (this.hasBootstapped)
                throw new InvalidOperationException("Illegal attempt to bootstrap application twice.");

            // Container Configuration
            this.Container = new StandardKernel(new DependencyContainer());

            // Bootstrap completed
            this.hasBootstapped = true;
        }
    }
}
