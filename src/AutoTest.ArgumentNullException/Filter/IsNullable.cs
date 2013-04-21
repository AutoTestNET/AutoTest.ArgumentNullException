namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Filters out parameters that are not nullable.
    /// </summary>
    public class IsNullable : FilterBase, IParameterFilter
    {
        /// <summary>
        /// Filters out parameters that are not nullable.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns><c>true</c> if the <paramref name="parameter"/> should be included, otherwise <c>false</c>.</returns>
        bool IParameterFilter.IncludeParameter(Type type, MethodBase method, ParameterInfo parameter)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (method == null) throw new ArgumentNullException("method");
            if (parameter == null) throw new ArgumentNullException("parameter");

            return parameter.IsNullable();
        }
    }
}
