// Copyright (c) 2013 - 2017 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Filters out abstract methods.
    /// </summary>
    public sealed class NotAbstractMethod : FilterBase, IMethodFilter
    {
        /// <summary>
        /// Filters out abstract methods.
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
                throw new ArgumentNullException(nameof(type));
            if (method == null)
                throw new ArgumentNullException(nameof(method));

            return method.IsAbstract;
        }
    }
}
