namespace AutoTest.ArgNullEx.TypeFilter
{
    using System;

    /// <summary>
    /// Filters out types that are compiler generated.
    /// </summary>
    public class NotCompilerGenerated : FilterBase, ITypeFilter
    {
        /// <summary>
        /// Filters out types that are compiler generated.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the type should be included, otherwise <c>false</c>.</returns>
        bool ITypeFilter.IncludeType(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            return !type.IsCompilerGenerated();
        }
    }
}
