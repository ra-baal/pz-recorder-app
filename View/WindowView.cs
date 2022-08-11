using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Recorder.ViewModel;

namespace Recorder.View
{
    /// <summary>
    /// Base class for a window displayed in the bugView application
    /// </summary>
    public abstract class WindowView : Window
    {
        /// <summary>
        /// Initializes the data context of the window.
        /// </summary>
        /// <param name="viewModel">
        /// The view model
        /// </param>
        public void Initialize(ViewModelBase viewModel)
        {
            DataContext = viewModel;
        }

        /// <summary>
        /// Cleans up the bound view model when the window is closed. 
        /// Override Cleanup() in the view model if you need to dispose custom objects.
        /// </summary>
        /// <param name="e">
        /// The closed event args.
        /// </param>
        protected override void OnClosed(EventArgs e)
        {
            if (this.DataContext is ViewModelBase viewModel)
            {
                viewModel.Dispose();
            }

            base.OnClosed(e);
        }
    }
}
