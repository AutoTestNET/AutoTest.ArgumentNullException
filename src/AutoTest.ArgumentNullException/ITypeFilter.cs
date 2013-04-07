namespace AutoTest.ArgNullEx.TypeFilter
{
    using System;

    /// <summary>
    /// Interface defining a predicate on a <see cref="Type"/>.
    /// </summary>
    public interface ITypeFilter
    {
        /// <summary>
        /// A predicate function for filtering on a <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the <paramref name="type"/> should be included, otherwise <c>false</c>.</returns>
        bool IncludeType(Type type);
    }
}
