namespace AutoTest.ArgumentNullException.TypeFilter
{
    using System;

    /// <summary>
    /// Interface defining a filter predicate on a type.
    /// </summary>
    public interface ITypeFilter
    {
        /// <summary>
        /// A predicate function defining a filter on a <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> of the type should be included, otherwise <c>false</c>.</returns>
        bool FilterType(Type type);
    }
}
