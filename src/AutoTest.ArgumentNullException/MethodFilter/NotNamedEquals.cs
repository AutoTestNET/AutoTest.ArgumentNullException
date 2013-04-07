namespace AutoTest.ArgNullEx.MethodFilter
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Filters out the <see cref="object.Equals(object)"/> method.
    /// </summary>
    public class NotNamedEquals : FilterBase, IMethodFilter
    {
        /// <summary>
        /// Filters out the <see cref="object.Equals(object)"/> method.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        /// <returns><c>true</c> if the <paramref name="method"/> should be included, otherwise <c>false</c>.</returns>
        bool IMethodFilter.IncludeMethod(Type type, MethodInfo method)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (method == null) throw new ArgumentNullException("method");

            return method.Name != "Equals";
        }
    }
}
