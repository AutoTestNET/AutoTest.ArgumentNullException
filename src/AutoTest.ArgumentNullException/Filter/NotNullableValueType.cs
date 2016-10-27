﻿namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Filters out parameters that are <see cref="Nullable{T}"/> value types.
    /// </summary>
    public sealed class NotNullableValueType : FilterBase, IParameterFilter
    {
        /// <summary>
        /// Filters out parameters that are <see cref="Nullable{T}"/> value types.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns><see langword="true"/> if the <paramref name="parameter"/> should be excluded;
        /// otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="type"/>, <paramref name="method"/> or
        /// <paramref name="parameter"/> parameters are <see langword="null"/>.</exception>
        bool IParameterFilter.ExcludeParameter(Type type, MethodBase method, ParameterInfo parameter)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (method == null)
                throw new ArgumentNullException(nameof(method));
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));

            Type parameterType = parameter.ParameterType;
            return parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}
