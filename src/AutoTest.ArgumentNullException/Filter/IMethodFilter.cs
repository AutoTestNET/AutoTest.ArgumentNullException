namespace AutoTest.ArgNullEx
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Interface defining a predicate on a <see cref="MethodBase"/> of a <see cref="Type"/>.
    /// </summary>
    public interface IMethodFilter : IFilter
    {
        /// <summary>
        /// A predicate function for filtering on a <see cref="MethodBase"/> of a <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        /// <returns><c>true</c> if the <paramref name="method"/> should be included, otherwise <c>false</c>.</returns>
        bool IncludeMethod(Type type, MethodBase method);
    }
}
