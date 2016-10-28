//------------------------------------------------------------------------------
// <copyright file="PackageModel.cs" company="Christian Gunderman">
//     Copyright (c) Christian Gunderman.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Contextool.Internal
{
    using System;
    using Contextool.Project;
    using Microsoft.VisualStudio.Shell;
    using Commands;    
    
    /// <summary>
    /// The PackageModel is a single instance class that encapsulates the VS specific application
    /// initialization and tear-down routines.
    /// </summary>
    internal sealed class PackageModel : IDisposable
    {
        /// <summary>
        /// Creates a new instance of <see cref="PackageModel"/>. There is exactly one PackageModel
        /// per VS instance.
        /// </summary>
        /// <param name="shimFactory">Shims of VS specific services.</param>
        public PackageModel(IShimFactory shimFactory)
        {
            Contract.NotNull(shimFactory, nameof(shimFactory));

            this.ShimFactory = shimFactory;
            this.Context = new ContextoolContext(this.ShimFactory);
        }

        /// <summary>
        /// Gets an instance of <see cref="IContextoolContext"/> for interacting
        /// with the Contextool project system.
        /// </summary>
        private IContextoolContext Context
        {
            get;
        }

        /// <summary>
        /// Gets an instance of <see cref="IShimFactory"/> that shims away important
        /// IDE features.
        /// </summary>
        private IShimFactory ShimFactory
        {
            get;
        }

        #region Public Members

        /// <summary>
        /// This method begins execution of the package.
        /// </summary>
        public void Start(Package package)
        {
            this.ShimFactory.Logging.LogInfo("Beginning initialization");
            this.ShimFactory.Logging.LogInfo("Registering Visual Studio commands...");

            CommandDefinitions.RegisterAllCommands(this.ShimFactory, package);
        }

        /// <summary>
        /// This method cleans up all package resources.
        /// </summary>
        public void Dispose()
        {
            this.ShimFactory.Dispose();
        }

        #endregion
    }
}
