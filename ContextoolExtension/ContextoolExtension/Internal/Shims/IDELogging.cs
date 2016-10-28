//------------------------------------------------------------------------------
// <copyright file="IIDEExceptionLogging.cs" company="Christian Gunderman">
//     Copyright (c) Christian Gunderman.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Contextool.Internal.Shims
{
    using System;
    using Contextool.Project;
    using ContextoolExtension;
    using Contextool.Project.Shims;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio;

    /// <summary>
    /// Implements text and exception logging mechanisms for Contextool.
    /// </summary>
    internal sealed class IDELogging : IIDELogging
    {
        /// <summary>
        /// A unique identifier for the Contextool Visual Studio Output window pane.
        /// </summary>
        private static readonly Guid ContextoolOutputPaneGuid = new Guid("470AB706-E0EB-4D73-905B-103468123AEF");

        /// <summary>
        /// An instance of the Visual Studio output window control interface.
        /// </summary>
        private readonly IVsOutputWindow outputWindow;

        /// <summary>
        /// The Contextool Visual Studio Output window pane control interface.
        /// </summary>
        private readonly IVsOutputWindowPane outputWindowPane;

        /// <summary>
        /// Creates an instance of the <see cref="IDELogging"/> class.
        /// </summary>
        /// <param name="serviceProvider">The Visual Studio service provider mechanism.</param>
        public IDELogging(IServiceProvider serviceProvider)
        {
            Contract.NotNull(serviceProvider, nameof(serviceProvider));

            this.outputWindow = serviceProvider.GetService(typeof(SVsOutputWindow)) as IVsOutputWindow;

            // This is the basis of our error handling. If this doesn't work, throw!
            Contract.NotNull(this.outputWindow, nameof(this.outputWindow));
            Contract.IsTrue(this.outputWindow.CreatePane(ContextoolOutputPaneGuid, Resources.ContextoolOutputWindowPaneTitle, 1, 0) == VSConstants.S_OK);
            Contract.IsTrue(this.outputWindow.GetPane(ContextoolOutputPaneGuid, out this.outputWindowPane) == VSConstants.S_OK);
        }

        #region IIDELogging Members

        /// <summary>
        /// Logs the given error message to the output window pane.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void LogError(string message)
        {
            this.LogLine(Resources.LogErrorHeader, message, true);
        }

        /// <summary>
        /// Logs the given info message to the output window pane.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void LogInfo(string message)
        {
            this.LogLine(Resources.LogInfoHeader, message, false);
        }

        /// <summary>
        /// Logs the given warning message to the output window pane.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void LogWarning(string message)
        {
            this.LogLine(Resources.LogWarningHeader, message, false);
        }


        /// <summary>
        /// Performs the requested action and logs any exceptions with error tag.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="description">The description for the log message.</param>
        /// <returns>True if the action succeeds, and false if an exception was caught.</returns>
        public bool TryActionAndLogError(Action action, string description = null)
        {
            return this.TryActionAndLogHeader(Resources.LogInfoHeader, action, description, false);
        }

        /// <summary>
        /// Performs the requested action and logs any exceptions with info tag.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="description">The description for the log message.</param>
        /// <returns>True if the action succeeds, and false if an exception was caught.</returns>
        public bool TryActionAndLogInfo(Action action, string description = null)
        {
            return this.TryActionAndLogHeader(Resources.LogInfoHeader, action, description, false);
        }

        /// <summary>
        /// Performs the requested action and logs any exceptions with warning tag.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="description">The description for the log message.</param>
        /// <returns>True if the action succeeds, and false if an exception was caught.</returns>
        public bool TryActionAndLogWarning(Action action, string description = null)
        {
            return this.TryActionAndLogHeader(Resources.LogWarningHeader, action, description, false);
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Cleans up unmanaged resources. Specifically, removes the output pane from VS.
        /// </summary>
        public void Dispose()
        {
            // Don't check result, if VS is shutting down, it may have been removed already.
            this.outputWindow.DeletePane(ContextoolOutputPaneGuid);
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Logs a single line of text to the Visual Studio output window.
        /// </summary>
        /// <param name="header">The message header.</param>
        /// <param name="line">The message text.</param>
        /// <param name="activate">If true, activates the output window and brings to front.</param>
        private void LogLine(string header, string line, bool activate)
        {
            this.outputWindowPane.OutputStringThreadSafe($"{header}{line}\n");

            if (activate)
            {
                this.outputWindowPane.Activate();
            }
        }

        /// <summary>
        /// Attempts an action and logs the given header and text if an exception is thrown.
        /// </summary>
        /// <param name="header">The log output header text.</param>
        /// <param name="action">The action to perform.</param>
        /// <param name="actionDescription">The description of the action.</param>
        /// <param name="activate">If true, brings the output window to the front.</param>
        /// <returns>False if the action throws.</returns>
        private bool TryActionAndLogHeader(string header, Action action, string actionDescription, bool activate)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                string line;

                if (actionDescription == null)
                {
                    line = string.Format(Resources.ExceptionLogMessageFormat, ex.GetType().Name, ex.Message);
                }
                else
                {
                    line = string.Format(
                        Resources.ExceptionInterruptedActionLogMessageFormat,
                        actionDescription,
                        ex.GetType().Name,
                        ex.Message);
                }

                this.LogLine(header, line, activate);
                return false;
            }

            return true;
        }

        #endregion
    }
}
