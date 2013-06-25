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
            if (type == null)
                throw new ArgumentNullException("type");

            return (!type.IsValueType && !type.IsByRef)
                   || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                   || type.IsNullableByRef();
        }

        /// <summary>
        /// Returns <c>true</c> if the <paramref name="parameter"/> can have a null value; otherwise <c>false</c>.
        /// </summary>
        /// <param name="parameter">The parameter info.</param>
        /// <returns><c>true</c> if the <paramref name="parameter"/> can have a null value; otherwise <c>false</c>.</returns>
        public static bool IsNullable(this ParameterInfo parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException("parameter");

            return parameter.ParameterType.IsNullable();
        }

        /// <summary>
        /// Returns <c>true</c> if the <paramref name="parameter"/> has a <c>null</c> default value; otherwise <c>false</c>.
        /// </summary>
        /// <param name="parameter">The information about the parameter.</param>
        /// <returns><c>true</c> if the <paramref name="parameter"/> has a <c>null</c> default value; otherwise <c>false</c>.</returns>
        public static bool HasNullDefault(this ParameterInfo parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException("parameter");

            return parameter.RawDefaultValue == null;
        }

        /// <summary>
        /// Returns <c>true</c> if the <paramref name="member"/> was compiler generated; otherwise <c>false</c>.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns><c>true</c> if the <paramref name="member"/> was compiler generated; otherwise <c>false</c>.</returns>
        public static bool IsCompilerGenerated(this MemberInfo member)
        {
            if (member == null)
                throw new ArgumentNullException("member");

            if (Attribute.GetCustomAttribute(member, typeof(CompilerGeneratedAttribute)) != null)
                return true;

            return member.DeclaringType != null && IsCompilerGenerated(member.DeclaringType);
        }

        /// <summary>
        /// Returns <c>true</c> if the <paramref name="type"/> is <see cref="Type.IsByRef"/> and the underlying type is nullable; otherwise <c>false</c>.
        /// </summary>
        /// <param name="type">The member.</param>
        /// <returns><c>true</c> if the <paramref name="type"/> is <see cref="Type.IsByRef"/> and the underlying type is nullable; otherwise <c>false</c>.</returns>
        private static bool IsNullableByRef(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return type.IsByRef && type.GetElementType().IsNullable();
        }
    }
}
