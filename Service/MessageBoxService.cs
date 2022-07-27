using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Recorder.Entities;

namespace Recorder.Service
{
    /// <summary>
    /// Concrete implementation of the <see cref="IMessageBoxService"/> for WPF.
    /// </summary>
    public class MessageBoxService : IMessageBoxService
    {
        /// <inheritdoc />
        public MessageBoxResponse ShowMessagebox(string message, MessageBoxKind messageboxKind, string title = null)
        {
            var result = MessageBox.Show(message, title, GetButtonFromMessageBoxKind(messageboxKind));
            return GetMessageBoxResponseFromResult(result);
        }

        /// <summary>
        /// Converts the <see cref="MessageBoxResult"/> enum to its equivalent <see cref="MessageBoxResponse"/> value.
        /// </summary>
        /// <param name="messageBoxResult">
        /// The native message box result
        /// </param>
        /// <returns>
        /// The message box response
        /// </returns>
        private static MessageBoxResponse GetMessageBoxResponseFromResult(MessageBoxResult messageBoxResult)
        {
            return messageBoxResult switch
            {
                MessageBoxResult.Cancel => MessageBoxResponse.Cancel,
                MessageBoxResult.No => MessageBoxResponse.No,
                MessageBoxResult.None => MessageBoxResponse.None,
                MessageBoxResult.OK => MessageBoxResponse.Ok,
                MessageBoxResult.Yes => MessageBoxResponse.Yes,
                _ => throw new ArgumentException($"Unsupported message box result {messageBoxResult}")
            };
        }

        /// <summary>
        /// Converts the <see cref="MessageBoxKind"/> enum into its equivalent <see cref="MessageBoxButton"/> value.
        /// </summary>
        /// <param name="messageboxKind">The message box kind</param>
        /// <returns>
        /// The equivalent <see cref="MessageBoxButton"/>
        /// </returns>
        private static MessageBoxButton GetButtonFromMessageBoxKind(MessageBoxKind messageboxKind)
        {
            return messageboxKind switch
            {
                MessageBoxKind.Ok => MessageBoxButton.OK,
                MessageBoxKind.OkCancel => MessageBoxButton.OKCancel,
                MessageBoxKind.YesNo => MessageBoxButton.YesNo,
                MessageBoxKind.YesNoCancel => MessageBoxButton.YesNoCancel,
                _ => throw new ArgumentException($"Unsupported message box kind {messageboxKind}")
            };
        }
    }
}
