//------------------------------------------------------------------------------
// <copyright file="IShimFactory.cs" company="Christian Gunderman">
//     Copyright (c) Christian Gunderman.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Contextool.Internal
{
    using Contextool.Project.Shims;

    /// <summary>
    /// Defines an interface for the ShimFactory which provides system specific
    /// dependencies.
    /// </summary>
    internal interface IShimFactory
    {
        /// <summary>
        /// Gets the <see cref="IIDESolution"/> Shim.
        /// </summary>
        IIDESolution Solution { get; }
    }
}
