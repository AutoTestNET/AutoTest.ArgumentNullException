// Copyright (c) 2013 - 2017 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Extension methods around null types and parameters.
    /// </summary>
    public static class NullExtensions
    {
        /// <summary>
        /// Returns <see langword="true"/> if the <paramref name="type"/> can have a null value; otherwise
        /// <see langword="false"/>.
        /// </summary>
        /// <param name="type">The member.</param>
        /// <returns><see langword="true"/> if the <paramref name="type"/> can have a null value; otherwise
        /// <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="type"/> parameter is
        /// <see langword="null"/>.</exception>
        public static bool IsNullable(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            TypeInfo typeInfo = type.GetTypeInfo();

            return (!typeInfo.IsValueType && !type.IsByRef)
                   || (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
                   || type.IsNullableByRef();
        }

        /// <summary>
        /// Returns <see langword="true"/> if the <paramref name="parameter"/> can have a null value; otherwise
        /// <see langword="false"/>.
        /// </summary>
        /// <param name="parameter">The parameter info.</param>
        /// <returns><see langword="true"/> if the <paramref name="parameter"/> can have a null value; otherwise
        /// <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="parameter"/> parameter is
        /// <see langword="null"/>.</exception>
        public static bool IsNullable(this ParameterInfo parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));

            return parameter.ParameterType.IsNullable();
        }

        /// <summary>
        /// Returns <see langword="true"/> if the <paramref name="parameter"/> has a <see langword="null"/> default
        /// value; otherwise <see langword="false"/>.
        /// </summary>
        /// <param name="parameter">The information about the parameter.</param>
        /// <returns><see langword="true"/> if the <paramref name="parameter"/> has a <see langword="null"/> default
        /// value; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="parameter"/> parameter is
        /// <see langword="null"/>.</exception>
        public static bool HasNullDefault(this ParameterInfo parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));

            return parameter.RawDefaultValue == null;
        }

        /// <summary>
        /// Returns <see langword="true"/> if the <paramref name="member"/> was compiler generated; otherwise
        /// <see langword="false"/>.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns><see langword="true"/> if the <paramref name="member"/> was compiler generated; otherwise
        /// <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="member"/> parameter is
        /// <see langword="null"/>.</exception>
        public static bool IsCompilerGenerated(this MemberInfo member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            if (member.GetCustomAttribute<CompilerGeneratedAttribute>() != null)
                return true;

            return member.DeclaringType != null && IsCompilerGenerated(member.DeclaringType.GetTypeInfo());
        }

        /// <summary>
        /// Returns <see langword="true"/> if the <paramref name="type"/> is <see cref="Type.IsByRef"/> and the
        /// underlying type is nullable; otherwise <see langword="false"/>.
        /// </summary>
        /// <param name="type">The member.</param>
        /// <returns><see langword="true"/> if the <paramref name="type"/> is <see cref="Type.IsByRef"/> and the
        /// underlying type is nullable; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="type"/> parameter is
        /// <see langword="null"/>.</exception>
        private static bool IsNullableByRef(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type.IsByRef && type.GetElementType().IsNullable();
        }
    }
}
