namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Interface defining a predicate on a <see cref="Type"/>.
    /// </summary>
    public interface ITypeFilter : IFilter
    {
        /// <summary>
        /// A predicate function for filtering on a <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><see langword="true"/> if the <paramref name="type"/> should be excluded;
        /// otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="type"/> parameter is <see langword="null"/>.
        /// </exception>
        bool ExcludeType(Type type);
    }
}
