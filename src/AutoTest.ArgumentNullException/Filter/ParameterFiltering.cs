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
    /// Helper class for applying filters on parameters.
    /// </summary>
    public static class ParameterFiltering
    {
        /// <summary>
        /// Executes the <paramref name="filter"/> on the <paramref name="parameter"/>, logging information if it was
        /// excluded.
        /// </summary>
        /// <param name="filter">The <see cref="Type"/> filter.</param>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>The result of <see cref="IMethodFilter.ExcludeMethod"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="filter"/>, <paramref name="type"/>,
        /// <paramref name="method"/> or <paramref name="parameter"/> parameters are <see langword="null"/>.</exception>
        public static bool ApplyFilter(this IParameterFilter filter, Type type, MethodBase method, ParameterInfo parameter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (method == null)
                throw new ArgumentNullException(nameof(method));
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));

            bool excludeParameter = filter.ExcludeParameter(type, method, parameter);
            if (excludeParameter)
            {
                ////TODO: Look into Tracing.
                ////Trace.TraceInformation(
                ////    "The parameter '{0}.{1}({2})' was excluded by the filter '{3}'.",
                ////    type.Name,
                ////    method.Name,
                ////    parameter.Name,
                ////    filter.Name);
            }

            return excludeParameter;
        }
    }
}
