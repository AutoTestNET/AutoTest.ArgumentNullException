// Copyright (c) 2013 - 2017 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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
    public static class MethodFiltering
    {
        /// <summary>
        /// Gets all the methods (including constructors) in the <paramref name="type"/> limited by the
        /// <paramref name="filters"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> from which to retrieve the methods.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="System.Reflection.BindingFlags"/>
        /// that specify how the search is conducted.</param>
        /// <param name="filters">The collection of filters to limit the methods.</param>
        /// <returns>All the methods in the <paramref name="type"/> limited by the <paramref name="filters"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="type"/> or <paramref name="filters"/> parameters
        /// are <see langword="null"/>.</exception>
        public static IEnumerable<MethodBase> GetMethods(this Type type, BindingFlags bindingAttr, IEnumerable<IMethodFilter> filters)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (filters == null)
                throw new ArgumentNullException("filters");

            IEnumerable<MethodBase> allMethods =
                type.GetMethods(bindingAttr).Cast<MethodBase>()
                    .Union(type.GetConstructors(bindingAttr));

            IEnumerable<MethodBase> filteredMethods =
                filters.Aggregate(
                    allMethods,
                    (current, filter) => current.Where(method => !method.ApplyFilter(type, filter)));

            return filteredMethods.Select(ConvertGenericMethod).ToList();
        }

        /// <summary>
        /// Converts generic method definitions to methods with specific type definitions.
        /// </summary>
        /// <param name="method">The method that may be a generic method definition.</param>
        /// <returns>The converted generic method definition; otherwise the <paramref name="method"/> if it is non
        /// generic.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="method"/> parameter is <see langword="null"/>.
        /// </exception>
        private static MethodBase ConvertGenericMethod(MethodBase method)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            // If it's not generic return the method.
            if (!method.IsGenericMethodDefinition)
                return method;

            // Only MethodInfo can be generic (at the time of writing).
            var methodInfo = (MethodInfo)method;

            // Get non generic argument types from the generic arguments.
            Type[] genericArguments = methodInfo.GetGenericArguments();
            Type[] nonGenericArguments = genericArguments.Select(GenericTypeConversion.GetNonGenericType).ToArray();

            return methodInfo.MakeGenericMethod(nonGenericArguments);
        }

        /// <summary>
        /// Executes the <paramref name="filter"/> on the <paramref name="method"/>, logging information if it was
        /// excluded.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="type">The type.</param>
        /// <param name="filter">The <see cref="Type"/> filter.</param>
        /// <returns>The result of <see cref="IMethodFilter.ExcludeMethod"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="method"/>, <paramref name="type"/> or
        /// <paramref name="filter"/> parameters are <see langword="null"/>.</exception>
        private static bool ApplyFilter(this MethodBase method, Type type, IMethodFilter filter)
        {
            if (method == null)
                throw new ArgumentNullException("method");
            if (type == null)
                throw new ArgumentNullException("type");
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
