namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Helper class for applying filters on types in assemblies.
    /// </summary>
    public static class TypeFiltering
    {
        /// <summary>
        /// Gets all the types in the <paramref name="assembly"/> limited by the <paramref name="filters"/>.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> from which to retrieve the types.</param>
        /// <param name="filters">The collection of filters to limit the types.</param>
        /// <returns>All the types in the <paramref name="assembly"/> limited by the
        /// <paramref name="filters"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="assembly"/> or <paramref name="filters"/>
        /// parameters are <see langword="null"/>.</exception>
        public static IEnumerable<Type> GetTypes(this Assembly assembly, IEnumerable<ITypeFilter> filters)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");
            if (filters == null)
                throw new ArgumentNullException("filters");

            return filters.Aggregate(
                assembly.GetTypes().AsEnumerable(),
                (current, filter) => current.Where(type => !type.ApplyFilter(filter))).ToList();
        }

        /// <summary>
        /// Executes the <paramref name="filter"/> on the <paramref name="type"/>, logging information if it was
        /// excluded.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="filter">The <see cref="Type"/> filter.</param>
        /// <returns>The result of <see cref="ITypeFilter.ExcludeType"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="type"/> or <paramref name="filter"/> parameters
        /// are <see langword="null"/>.</exception>
        private static bool ApplyFilter(this Type type, ITypeFilter filter)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (filter == null)
                throw new ArgumentNullException("filter");

            bool excludeType = filter.ExcludeType(type);
            if (excludeType)
            {
                Trace.TraceInformation(
                    "The type '{0}' was excluded by the filter '{1}'.",
                    type,
                    filter.Name);
            }

            return excludeType;
        }
    }
}
