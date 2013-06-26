namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Filters out property setters.
    /// </summary>
    public class NotPropertySetter : FilterBase, IMethodFilter
    {
        /// <summary>
        /// Filters out property setters.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        /// <returns><c>true</c> if the <paramref name="method"/> should be excluded, otherwise <c>false</c>.</returns>
        bool IMethodFilter.ExcludeMethod(Type type, MethodBase method)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (method == null)
                throw new ArgumentNullException("method");

            // Solution taken from here: http://stackoverflow.com/a/234378
            return method.IsSpecialName && method.Name.StartsWith("set_");

            // Potential alternative solution here: http://stackoverflow.com/a/12216834
        }
    }
}
