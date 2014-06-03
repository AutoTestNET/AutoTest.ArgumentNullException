namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Filters out parameters that have a <see langword="null"/> default value.
    /// </summary>
    public sealed class NotNullDefault : FilterBase, IParameterFilter
    {
        /// <summary>
        /// Filters out parameters that have a <see langword="null"/> default value.
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
                throw new ArgumentNullException("type");
            if (method == null)
                throw new ArgumentNullException("method");
            if (parameter == null)
                throw new ArgumentNullException("parameter");

            return parameter.HasNullDefault();
        }
    }
}
