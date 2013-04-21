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
        /// <returns><c>true</c> if the type should be included, otherwise <c>false</c>.</returns>
        bool ITypeFilter.IncludeType(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            return type.IsClass || (type.IsValueType && !type.IsEnum);
        }
    }
}
