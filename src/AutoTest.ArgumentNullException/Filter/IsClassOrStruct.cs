// Copyright (c) 2013 - 2017 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Filters out types that are not classes or structs.
    /// </summary>
    public sealed class IsClassOrStruct : FilterBase, ITypeFilter
    {
        /// <summary>
        /// Filters out types that are not classes.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><see langword="true"/> if the <paramref name="type"/> should be excluded;
        /// otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="type"/> parameter is <see langword="null"/>.
        /// </exception>
        bool ITypeFilter.ExcludeType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            TypeInfo typeInfo = type.GetTypeInfo();
            return !typeInfo.IsClass && (!typeInfo.IsValueType || typeInfo.IsEnum);
        }
    }
}
