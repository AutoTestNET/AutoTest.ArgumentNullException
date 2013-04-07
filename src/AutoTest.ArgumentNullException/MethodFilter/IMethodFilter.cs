namespace AutoTest.ArgNullEx.MethodFilter
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Interface defining a predicate on a <see cref="MethodInfo"/> of a <see cref="Type"/>.
    /// </summary>
    public interface IMethodFilter
    {
        /// <summary>
        /// A predicate function for filtering on a <see cref="MethodInfo"/> of a <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        /// <returns><c>true</c> if the <paramref name="method"/> should be included, otherwise <c>false</c>.</returns>
        bool IncludeMethod(Type type, MethodInfo method);
    }
}
