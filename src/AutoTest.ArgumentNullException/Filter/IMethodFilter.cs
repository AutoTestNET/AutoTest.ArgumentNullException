namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        /// <returns><see langword="true"/> if the <paramref name="method"/> should be excluded;
        /// otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="type"/> or <paramref name="method"/> parameters
        /// are <see langword="null"/>.</exception>
        bool ExcludeMethod(Type type, MethodBase method);
    }
}
