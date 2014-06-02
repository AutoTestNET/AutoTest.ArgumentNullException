﻿namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Extension methods on <see cref="MethodBase"/>.
    /// </summary>
    internal static class MethodExtensions
    {
        /// <summary>
        /// Gets the name of the method, handles explicit implementations.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>The name of the method.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="method"/> parameter is
        /// <see langword="null"/>.</exception>
        public static string GetMethodName(this MethodBase method)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            string name = method.Name;
            int index = method.Name.LastIndexOf('.') + 1;
            if (index > 0)
                name = method.Name.Substring(index);
            return name;
        }
    }
}
