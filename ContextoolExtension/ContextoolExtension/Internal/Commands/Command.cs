//------------------------------------------------------------------------------
// <copyright file="Command.cs" company="Christian Gunderman">
//     Copyright (c) Christian Gunderman.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Contextool.Internal.Commands
{
    using System;
    using Contextool.Project;

    /// <summary>
    /// Visual Studio Command Definition corresponding to a command
    /// defined in the package's VSCT file.
    /// </summary>
    internal sealed class Command
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="commandId">The commandId, corresponding to an ID in the VSCT file.</param>
        /// <param name="invocationAction">The action performed when the menu item is clicked.</param>
        public Command(int commandId, Action invocationAction)
        {
            Contract.NotNull(invocationAction, nameof(invocationAction));

            this.CommandId = commandId;
            this.InvocationAction = invocationAction;
        }

        /// <summary>
        /// Gets the ID for this command that is declared in the VSCT file.
        /// </summary>
        public int CommandId
        {
            get;
        }

        /// <summary>
        /// Gets the action that should be performed when this command is clicked.
        /// </summary>
        public Action InvocationAction
        {
            get;
        }
    }
}
