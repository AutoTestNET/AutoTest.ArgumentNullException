namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Helper class for applying filters on methods in types.
    /// </summary>
    internal static class TypeMethodFiltering
    {
        /// <summary>
        /// Gets all the methods (including constructors) in the <paramref name="type"/> limited by the <paramref name="filters"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> from which to retrieve the methods.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="System.Reflection.BindingFlags"/> that specify how the search is conducted.</param>
        /// <param name="filters">The collection of filters to limit the methods.</param>
        /// <returns>All the methods in the <paramref name="type"/> limited by the <paramref name="filters"/>.</returns>
        public static IEnumerable<MethodBase> GetMethods(this Type type, BindingFlags bindingAttr, IEnumerable<IMethodFilter> filters)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (filters == null)
                throw new ArgumentNullException("filters");

            return filters.Aggregate(
                type.GetMethods(bindingAttr).Cast<MethodBase>()
                    .Union(type.GetConstructors(bindingAttr)),
                (current, filter) => current.Where(method => !ExcludeMethod(type, method, filter))).ToArray();
        }

        /// <summary>
        /// Executes the <paramref name="filter"/> on the <paramref name="method"/>, logging information if it was excluded.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        /// <param name="filter">The <see cref="Type"/> filter.</param>
        /// <returns>The result of <see cref="IMethodFilter.ExcludeMethod"/>.</returns>
        private static bool ExcludeMethod(Type type, MethodBase method, IMethodFilter filter)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (method == null)
                throw new ArgumentNullException("method");
            if (filter == null)
                throw new ArgumentNullException("filter");

            bool excludeMethod = filter.ExcludeMethod(type, method);
            if (excludeMethod)
            {
                Trace.TraceInformation(
                    "The method '{0}.{1}' was excluded by the filter '{2}'.",
                    type.Name,
                    method.Name,
                    filter.Name);
            }

            return excludeMethod;
        }
    }
}
