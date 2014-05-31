namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Interface defining a predicate function for filtering on a <see cref="ParameterInfo"/> of a
    /// <see cref="MethodBase"/> on <see cref="Type"/>.
    /// </summary>
    public interface IParameterFilter : IFilter
    {
        /// <summary>
        /// A predicate function for filtering on a <see cref="ParameterInfo"/> of a <see cref="MethodBase"/>
        /// on <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns><see langword="true"/> if the <paramref name="parameter"/> should be excluded;
        /// otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="type"/>, <paramref name="method"/> or
        /// <paramref name="parameter"/> parameters are <see langword="null"/>.</exception>
        bool ExcludeParameter(Type type, MethodBase method, ParameterInfo parameter);
    }
}
