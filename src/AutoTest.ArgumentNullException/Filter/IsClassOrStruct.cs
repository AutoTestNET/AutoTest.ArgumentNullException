namespace AutoTest.ArgNullEx.Filter
{
    using System;

    /// <summary>
    /// Filters out types that are not classes.
    /// </summary>
    public class IsClassOrStruct : FilterBase, ITypeFilter
    {
        /// <summary>
        /// Filters out types that are not classes.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the <paramref name="type"/> should be excluded, otherwise <c>false</c>.</returns>
        bool ITypeFilter.ExcludeType(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            return !type.IsClass && (!type.IsValueType || type.IsEnum);
        }
    }
}
