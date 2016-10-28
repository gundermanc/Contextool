//------------------------------------------------------------------------------
// <copyright file="CommandDefinition.cs" company="Christian Gunderman">
//     Copyright (c) Christian Gunderman.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Contextool.Internal.Commands
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using Contextool.Project;
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    /// The definitions for all Visual Studio Commands and their registration helpers.
    /// </summary>
    internal static class CommandDefinitions
    {
        #region Command Definitions

        /// <summary>
        /// Creates instances of Visual Studio commands for the given <see cref="PackageModel"/> instance.
        /// All <see cref="CommandGroup"/>s and their respective <see cref="Command"/>s should be declared
        /// here.
        /// </summary>
        /// <param name="packageModel">The ShimFactory for this instance of VS.</param>
        /// <returns>A series of CommandGroups.</returns>
        private static IEnumerable<CommandGroup> CreateCommands(IShimFactory shimFactory)
        {
            // Tools Menu Command Group:
            yield return new CommandGroup(
                new Guid("40226fc3-ce64-4646-b5bb-be8db4d4854f"),

                // Add Contextool Project Command.
                new Command(0x0100, () => shimFactory.Logging.LogInfo("Add Contextool Project menu item clicked.")));
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Registers all known Commands with the current Visual Studio instance.
        /// </summary>
        /// <param name="shimFactory">The current ShimFactory.</param>
        /// <param name="serviceProvider">The VS service provider.</param>
        public static void RegisterAllCommands(IShimFactory shimFactory, IServiceProvider serviceProvider)
        {
            Contract.NotNull(shimFactory, nameof(shimFactory));
            Contract.NotNull(serviceProvider, nameof(serviceProvider));

            OleMenuCommandService commandService = serviceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            Contract.NotNull(commandService, nameof(commandService));

            RegisterAllCommands(shimFactory, commandService);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Registers all known Commands with the current Visual Studio instance.
        /// </summary>
        /// <param name="shimFactory">The current ShimFactory.</param>
        /// <param name="commandService">The Visual Studio command service.</param>
        private static void RegisterAllCommands(IShimFactory shimFactory, OleMenuCommandService commandService)
        {
            Contract.NotNull(shimFactory, nameof(shimFactory));
            Contract.NotNull(commandService, nameof(commandService));

            foreach (var group in CreateCommands(shimFactory))
            {
                foreach (var command in group.Commands)
                {
                    RegisterSingleCommand(commandService, command.CommandId, group.GroupGuid, command.InvocationAction);
                }
            }
        }

        /// <summary>
        /// Registers a single command with the VS command service.
        /// </summary>
        /// <param name="commandService">The VS command service.</param>
        /// <param name="commandId">A command ID declared in the package's VSCT file.</param>
        /// <param name="commandSet">A parent CommandGroup ID.</param>
        /// <param name="invocationAction">An action to be performed on invocation.</param>
        private static void RegisterSingleCommand(
            OleMenuCommandService commandService,
            int commandId,
            Guid commandSet,
            Action invocationAction)
        {
            Contract.NotNull(commandService, nameof(commandService));
            Contract.NotNull(commandSet, nameof(commandSet));
            Contract.NotNull(invocationAction, nameof(invocationAction));

            // TODO: do something on invocation throw.
            var menuItem = new MenuCommand(new EventHandler((obj, args) => invocationAction()), new CommandID(commandSet, commandId));
            commandService.AddCommand(menuItem);
        }

        #endregion
    }
}
