//------------------------------------------------------------------------------
// <copyright file="IIDELogging.cs" company="Christian Gunderman">
//     Copyright (c) Christian Gunderman.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Contextool.Project.Shims
{
    using System;

    /// <summary>
    /// IDE independent host exception and error logging interface.
    /// </summary>
    public interface IIDELogging : IDisposable
    {
        /// <summary>
        /// Logs a string to the IDE logging mechanism.
        /// </summary>
        /// <param name="message">The string to log.</param>
        void LogInfo(string message);

        /// <summary>
        /// Logs a warning to the IDE logging mechanism.
        /// </summary>
        /// <param name="message">The string to log.</param>
        void LogWarning(string message);

        /// <summary>
        /// Logs an error the IDE logging mechanism.
        /// </summary>
        /// <param name="message">The string to log.</param>
        void LogError(string message);

        /// <summary>
        /// Performs the requested action and logs any exceptions with info tag.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="description">The description for the log message.</param>
        /// <returns>True if the action succeeds, and false if an exception was caught.</returns>
        bool TryActionAndLogInfo(Action action, string description = null);

        /// <summary>
        /// Performs the requested action and logs any exceptions with warning tag.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="description">The description for the log message.</param>
        /// <returns>True if the action succeeds, and false if an exception was caught.</returns>
        bool TryActionAndLogWarning(Action action, string description = null);

        /// <summary>
        /// Performs the requested action and logs any exceptions with error tag.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="description">The description for the log message.</param>
        /// <returns>True if the action succeeds, and false if an exception was caught.</returns>
        bool TryActionAndLogError(Action action, string description = null);
    }
}
