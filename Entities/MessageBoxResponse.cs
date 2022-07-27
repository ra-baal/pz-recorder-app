using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recorder.Entities
{
    /// <summary>
    /// Represents the user's response to a message box
    /// </summary>
    public enum MessageBoxResponse
    {
        /// <summary>
        /// No button was pressed
        /// </summary>
        None = 0,

        /// <summary>
        /// The OK button was pressed
        /// </summary>
        Ok,

        /// <summary>
        /// The Cancel button was pressed
        /// </summary>
        Cancel,

        /// <summary>
        /// The Yes button was pressed
        /// </summary>
        Yes,

        /// <summary>
        /// The No button was pressed
        /// </summary>
        No,
    }
}
