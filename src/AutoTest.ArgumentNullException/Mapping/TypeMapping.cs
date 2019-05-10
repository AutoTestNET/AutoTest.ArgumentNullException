// Copyright (c) 2013 - 2017 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace AutoTest.ArgNullEx.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Helper class for applying mappings on types.
    /// </summary>
    internal static class TypeMapping
    {
        /// <summary>
        /// Maps the the supplied <paramref name="types"/> to potentially different types as determined by the mappings.
        /// </summary>
        /// <param name="types">The original types.</param>
        /// <param name="mappings">The type mappings.</param>
        /// <returns>The mapped types.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="types"/> or <paramref name="mappings"/>
        /// parameters are <see langword="null"/>.</exception>
        public static IEnumerable<Type> MapTypes(this IEnumerable<Type> types, IEnumerable<ITypeMapping> mappings)
        {
            if (types == null)
                throw new ArgumentNullException(nameof(types));
            if (mappings == null)
                throw new ArgumentNullException(nameof(mappings));

            return mappings.Aggregate(
                types,
                (current, mapping) => current.Select(t => t.ApplyMapping(mapping))).ToList();
        }

        /// <summary>
        /// Applied the <paramref name="mapping"/> from the <paramref name="originalType"/> to a potentially new
        /// <see cref="Type"/>.
        /// </summary>
        /// <param name="originalType">The original <see cref="Type"/>.</param>
        /// <param name="mapping">The type mapping.</param>
        /// <returns>A new <see cref="Type"/> is a mapping has been made; otherwise the
        /// <paramref name="originalType"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="originalType"/> or <paramref name="mapping"/>
        /// parameters are <see langword="null"/>.</exception>
        private static Type ApplyMapping(this Type originalType, ITypeMapping mapping)
        {
            if (originalType == null)
                throw new ArgumentNullException(nameof(originalType));
            if (mapping == null)
                throw new ArgumentNullException(nameof(mapping));

            Type newType = mapping.MapTo(originalType);
            if (originalType != newType)
            {
                ////TODO: Look into Tracing.
                ////Trace.TraceInformation(
                ////    "The type '{0}' was mapped to the type '{1}' by the mapping '{2}'.",
                ////    originalType,
                ////    newType,
                ////    mapping.Name);
            }

            return newType;
        }
    }
}
