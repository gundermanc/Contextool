//------------------------------------------------------------------------------
// <copyright file="IIDE.cs" company="Christian Gunderman">
//     Copyright (c) Christian Gunderman.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Contextool.Project.Shims
{
    using System;

    /// <summary>
    /// Main IDE shim layer interface.
    /// </summary>
    public interface IIDE : IDisposable
    {
        /// <summary>
        /// Gets the <see cref="IIDELogging"/> shim.
        /// </summary>
        IIDELogging Logging { get; }

        /// <summary>
        /// Gets the <see cref="IIDESolution"/> shim.
        /// </summary>
        IIDESolution Solution { get; }
    }
}
