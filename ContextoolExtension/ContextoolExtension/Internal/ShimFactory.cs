//------------------------------------------------------------------------------
// <copyright file="ShimFactory.cs" company="Christian Gunderman">
//     Copyright (c) Christian Gunderman.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Contextool.Internal
{
    using System;
    using Contextool.Project.Shims;
    using Shims;

    /// <summary>
    /// A ShimFactory for obtaining instances of Shims of system specific dependencies.
    /// </summary>
    internal sealed class ShimFactory : IShimFactory
    {
        /// <summary>
        /// A lazy instance of the <see cref="IIDESolution"/> type.
        /// </summary>
        private Lazy<IIDESolution> solution = new Lazy<IIDESolution>(() => new IDESolution());

        /// <summary>
        /// Gets an instance of the <see cref="IIDESolution"/> Shim.
        /// </summary>
        public IIDESolution Solution
        {
            get
            {
                return this.solution.Value;
            }
        }
    }
}
