namespace AutoTest.ArgNullEx
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Extension methods around null types and parameters.
    /// </summary>
    public static class NullExtensions
    {
        /// <summary>
        /// Returns <c>true</c> if the <paramref name="type"/> can have a null value; otherwise <c>false</c>.
        /// </summary>
        /// <param name="type">The type.</param>
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
            return parameter.HasDefaultValue && parameter.DefaultValue == null;
        }
    }
}
