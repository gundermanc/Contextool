//------------------------------------------------------------------------------
// <copyright file="ContextoolContext.cs" company="Christian Gunderman">
//     Copyright (c) Christian Gunderman.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Contextool.Project
{
    using Contextool.Project.Shims;

    /// <summary>
    /// The solution context for a Visual Studio solution containing a Contextool
    /// project. There is exactly one per VS instance, whether or not a Contextool
    /// project is present.
    /// </summary>
    public sealed class ContextoolContext : IContextoolContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContextoolContext"/> class for interacting with
        /// Contextool projects in Visual Studio. There is one ContextoolContext per VS instance.
        /// </summary>
        /// <param name="ide">Instance of IDE services.</param>
        public ContextoolContext(IIDE ide)
        {
            Contract.NotNull(ide, nameof(ide));
            this.IDE = ide;
        }

        /// <summary>
        /// Gets IDE services reference.
        /// </summary>
        private IIDE IDE
        {
            get;
        }
    }
}
