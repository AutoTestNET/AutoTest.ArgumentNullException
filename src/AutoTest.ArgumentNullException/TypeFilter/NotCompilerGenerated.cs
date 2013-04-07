namespace AutoTest.ArgNullEx.TypeFilter
{
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;

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

            return !IsCompilerGenerated(type);
        }

        /// <summary>
        /// Determines if a <paramref name="type"/> is compiler generated.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the type was compiler generated; otherwise <c>false</c>.</returns>
        private static bool IsCompilerGenerated(MemberInfo type)
        {
            if (type == null) throw new ArgumentNullException("type");

            if (type.GetCustomAttribute<CompilerGeneratedAttribute>() != null)
                return true;

            return type.DeclaringType != null && IsCompilerGenerated(type.DeclaringType);
        }
    }
}
