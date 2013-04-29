namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Filters out parameters that have a <c>null</c> default value.
    /// </summary>
    public class NotNullDefault : FilterBase, IParameterFilter
    {
        /// <summary>
        /// Filters out parameters that have a <c>null</c> default value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns><c>true</c> if the <paramref name="parameter"/> should be excluded, otherwise <c>false</c>.</returns>
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
