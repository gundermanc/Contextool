//------------------------------------------------------------------------------
// <copyright file="CommandGroup.cs" company="Christian Gunderman">
//     Copyright (c) Christian Gunderman.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Contextool.Internal.Commands
{
    using System;
    using System.Collections.Generic;
    using Contextool.Project;

    /// <summary>
    /// Defines a Visual Studio command group containing other commands.
    /// Command group locations and attributes are defined in the package
    /// VSCT file.
    /// </summary>
    internal sealed class CommandGroup
    {
        /// <summary>
        /// Creates a new instance of the <see cref="CommandGroup"/> class.
        /// </summary>
        /// <param name="groupGuid">A guid corresponding to a VSCT group.</param>
        /// <param name="commands">A series of commands contained in this group.</param>
        public CommandGroup(Guid groupGuid, params Command[] commands)
        {
            Contract.NotNull(groupGuid, nameof(groupGuid));
            Contract.NotNull(commands, nameof(commands));
            Contract.IsFalse(commands.Length == 0);

            this.GroupGuid = groupGuid;
            this.Commands = commands;
        }

        public Guid GroupGuid
        {
            get;
        }

        public IEnumerable<Command> Commands
        {
            get;
        }
    }
}
