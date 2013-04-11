namespace AutoTest.ArgNullEx
{
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Extension methods around null types and parameters.
    /// </summary>
    public static class NullExtensions
    {
        /// <summary>
        /// Returns <c>true</c> if the <paramref name="type"/> can have a null value; otherwise <c>false</c>.
        /// </summary>
        /// <param name="type">The member.</param>
        /// <returns><c>true</c> if the <paramref name="type"/> can have a null value; otherwise <c>false</c>.</returns>
        public static bool IsNullable(this Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            return !type.IsValueType || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// Returns <c>true</c> if the <paramref name="parameter"/> can have a null value; otherwise <c>false</c>.
        /// </summary>
        /// <param name="parameter">The parameter info.</param>
        /// <returns><c>true</c> if the <paramref name="parameter"/> can have a null value; otherwise <c>false</c>.</returns>
        public static bool IsNullable(this ParameterInfo parameter)
        {
            if (parameter == null) throw new ArgumentNullException("parameter");

            return parameter.ParameterType.IsNullable();
        }

        /// <summary>
        /// Returns <c>true</c> if the <paramref name="parameter"/> has a <c>null</c> default value; otherwise <c>false</c>.
        /// </summary>
        /// <param name="parameter">The information about the parameter.</param>
        /// <returns><c>true</c> if the <paramref name="parameter"/> has a <c>null</c> default value; otherwise <c>false</c>.</returns>
        public static bool HasNullDefault(this ParameterInfo parameter)
        {
            if (parameter == null) throw new ArgumentNullException("parameter");

            return parameter.HasDefaultValue && parameter.DefaultValue == null;
        }

        /// <summary>
        /// Returns <c>true</c> if the <paramref name="member"/> was compiler generated; otherwise <c>false</c>.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns><c>true</c> if the <paramref name="member"/> was compiler generated; otherwise <c>false</c>.</returns>
        public static bool IsCompilerGenerated(this MemberInfo member)
        {
            if (member == null) throw new ArgumentNullException("member");

            if (member.GetCustomAttribute<CompilerGeneratedAttribute>() != null)
                return true;

            return member.DeclaringType != null && IsCompilerGenerated(member.DeclaringType);
        }
    }
}
