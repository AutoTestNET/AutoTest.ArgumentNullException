namespace AutoTest.ArgNullEx.MethodFilter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Filters out methods that do not have nullable parameterTypes.
    /// </summary>
    public class HasNullableParameters : IMethodFilter
    {
        /// <summary>
        /// Filters out methods that do not have nullable parameterTypes.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        /// <returns><c>true</c> of the <paramref name="method"/> should be included, otherwise <c>false</c>.</returns>
        bool IMethodFilter.IncludeMethod(Type type, MethodInfo method)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (method == null) throw new ArgumentNullException("method");

            ParameterInfo[] parameters = method.GetParameters();

            return parameters.Length != 0 && AnyNullable(parameters.Select(p => p.ParameterType));
        }

        /// <summary>
        /// Predicate to determine if any of the <paramref name="parameterTypes"/> are nullable.
        /// </summary>
        /// <param name="parameterTypes">The parameter types of a method.</param>
        /// <returns><c>true</c> if any of the <paramref name="parameterTypes"/> are nullable; otherwise <c>false</c>.</returns>
        internal static bool AnyNullable(IEnumerable<Type> parameterTypes)
        {
            return
                parameterTypes.Any(
                    t => !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>)));
        }
    }
}
