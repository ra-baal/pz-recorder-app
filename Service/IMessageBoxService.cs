using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recorder.Entities;

namespace Recorder.Service
{
    /// <summary>
    /// Defines functionality for the displaying of message boxes
    /// </summary>
    public interface IMessageBoxService
    {
        /// <summary>
        /// Shows a message box, and blocks until it is closed.
        /// </summary>
        /// <param name="message">The message to shown in the message box</param>
        /// <param name="messageboxKind">The kind of message box to display</param>
        /// <param name="title">The title of the message box window</param>
        /// <returns>The button that the user clicked</returns>
        MessageBoxResponse ShowMessagebox(string message, MessageBoxKind messageboxKind, string title = null);
    }
}
