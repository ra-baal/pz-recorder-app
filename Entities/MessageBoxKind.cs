using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recorder.Entities
{
    /// <summary>
    /// The kind of message box that should be shown
    /// </summary>
    public enum MessageBoxKind
    {
        /// <summary>
        /// Only an OK button is shown.
        /// </summary>
        Ok = 0,

        /// <summary>
        /// An OK and cancel button are shown.
        /// </summary>
        OkCancel,

        /// <summary>
        /// Yes no and cancel buttons are shown.
        /// </summary>
        YesNoCancel,

        /// <summary>
        /// Yes and no buttons are shown.
        /// </summary>
        YesNo
    }
}
