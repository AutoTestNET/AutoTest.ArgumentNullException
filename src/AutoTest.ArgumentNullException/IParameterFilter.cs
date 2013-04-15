namespace AutoTest.ArgNullEx
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Interface defining a predicate function for filtering on a <see cref="ParameterInfo"/> of a <see cref="MethodInfo"/> on <see cref="Type"/>.
    /// </summary>
    public interface IParameterFilter : IFilter
    {
        /// <summary>
        /// A predicate function for filtering on a <see cref="ParameterInfo"/> of a <see cref="MethodInfo"/> on <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns><c>true</c> if the <paramref name="parameter"/> should be included, otherwise <c>false</c>.</returns>
        bool IncludeParameter(Type type, MethodInfo method, ParameterInfo parameter);
    }
}
