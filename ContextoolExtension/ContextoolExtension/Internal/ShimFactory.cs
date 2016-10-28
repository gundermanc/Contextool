//------------------------------------------------------------------------------
// <copyright file="ShimFactory.cs" company="Christian Gunderman">
//     Copyright (c) Christian Gunderman.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Contextool.Internal
{
    using System;
    using Contextool.Internal.Shims;
    using Contextool.Project;
    using Contextool.Project.Shims;

    /// <summary>
    /// A ShimFactory for obtaining instances of single instance global services and IDE
    /// specific functionality.
    /// </summary>
    internal sealed class ShimFactory : IShimFactory
    {
        #region Private Fields

        /// <summary>
        /// A lazy instance of the <see cref="IIDELogging"/> type.
        /// </summary>
        private Lazy<IIDELogging> logging;

        /// <summary>
        /// A lazy instance of the <see cref="IIDESolution"/> type.
        /// </summary>
        private Lazy<IIDESolution> solution;

        #endregion

        /// <summary>
        /// Creates a new instance of <see cref="ShimFactory"/> class.
        /// </summary>
        /// <param name="serviceProvider">The Visual Studio service provider.</param>
        public ShimFactory(IServiceProvider serviceProvider)
        {
            Contract.NotNull(serviceProvider, nameof(serviceProvider));

            this.ServiceProvider = serviceProvider;

            // Initialize Shims.
            this.logging = new Lazy<IIDELogging>(() => new IDELogging(this.ServiceProvider));
            this.solution = new Lazy<IIDESolution>(() => new IDESolution());
        }

        #region IIDE Members

        /// <summary>
        /// Gets an instance of the <see cref="IIDELogging"/> service.
        /// </summary>
        public IIDELogging Logging
        {
            get
            {
                return this.logging.Value;
            }
        }

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

        /// <summary>
        /// Gets an instance of the <see cref="IContextoolContext"/> Shim.
        /// </summary>
        public IContextoolContext Context
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        /// <summary>
        /// The <see cref="IServiceProvider"/> for obtaining Visual Studio services.
        /// </summary>
        private IServiceProvider ServiceProvider
        {
            get;
        }

        #region IDisposable Members

        /// <summary>
        /// Frees all Shim resources.
        /// </summary>
        public void Dispose()
        {
            this.Logging.Dispose();
        }

        #endregion
    }
}
