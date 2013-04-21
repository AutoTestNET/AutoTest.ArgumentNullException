﻿namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Filters out methods that are compiler generated.
    /// </summary>
    public class NotCompilerGenerated : FilterBase, ITypeFilter, IMethodFilter
    {
        /// <summary>
        /// Filters out types that are compiler generated.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the type should be included, otherwise <c>false</c>.</returns>
        bool ITypeFilter.IncludeType(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            return !type.IsCompilerGenerated();
        }

        /// <summary>
        /// Filters out methods that are compiler generated.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        /// <returns><c>true</c> if the <paramref name="method"/> should be included, otherwise <c>false</c>.</returns>
        bool IMethodFilter.IncludeMethod(Type type, MethodBase method)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (method == null) throw new ArgumentNullException("method");

            return !method.IsCompilerGenerated();
        }
    }
}