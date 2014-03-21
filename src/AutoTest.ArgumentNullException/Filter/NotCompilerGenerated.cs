namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        /// <returns><see langword="true"/> if the <paramref name="type"/> should be excluded;
        /// otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="type"/> parameter is <see langword="null"/>.
        /// </exception>
        bool ITypeFilter.ExcludeType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return type.IsCompilerGenerated();
        }

        /// <summary>
        /// Filters out methods that are compiler generated.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        /// <returns><see langword="true"/> if the <paramref name="method"/> should be excluded;
        /// otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="type"/> or <paramref name="method"/> parameters
        /// are <see langword="null"/>.</exception>
        bool IMethodFilter.ExcludeMethod(Type type, MethodBase method)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (method == null)
                throw new ArgumentNullException("method");

            return method.IsCompilerGenerated();
        }
    }
}
