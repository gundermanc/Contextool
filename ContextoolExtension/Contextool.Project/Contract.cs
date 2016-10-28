//------------------------------------------------------------------------------
// <copyright file="Contract.cs" company="Christian Gunderman">
//     Copyright (c) Christian Gunderman.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Contextool.Project
{
    using System;

    /// <summary>
    /// Contract class for asserting runtime conditions.
    /// </summary>
    public static class Contract
    {
        /// <summary>
        /// Throws if the parameter is null.
        /// </summary>
        /// <typeparam name="T">The parameter type.</typeparam>
        /// <param name="param">The parameter value.</param>
        /// <param name="paramName">The parameter name.</param>
        /// <exception cref="ArgumentNullException">Thrown if the parameter is null.</exception>
        public static void NotNull<T>(T param, string paramName)
        {
            if (param == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// Throws if the given value is false.
        /// </summary>
        /// <param name="value">A boolean value to check.</param>
        /// <exception cref="InvalidOperationException">Thrown if assertion failed.</exception>
        public static void IsTrue(bool value)
        {
            if (!value)
            {
                throw new InvalidOperationException("IsTrue assertion failed");
            }
        }

        /// <summary>
        /// Throws if the given value is true.
        /// </summary>
        /// <param name="value">A boolean value to check.</param>
        /// <exception cref="InvalidOperationException">Thrown if assertion failed.</exception>
        public static void IsFalse(bool value)
        {
            if (value)
            {
                throw new InvalidOperationException("IsTrue assertion failed");
            }
        }
    }
}
