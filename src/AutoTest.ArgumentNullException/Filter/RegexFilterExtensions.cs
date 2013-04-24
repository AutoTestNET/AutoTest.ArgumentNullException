namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Extension methods on <see cref="IRegexFilter"/>.
    /// </summary>
    public static class RegexFilterExtensions
    {
        /// <summary>
        /// Excludes the <paramref name="type"/> from checks for <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <param name="type">The type.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        public static IRegexFilter ExcludeType(this IRegexFilter filter, Type type)
        {
            return filter.AddTypeRule(type, include: false);
        }

        /// <summary>
        /// Includes the <paramref name="type"/> for checks for <see cref="ArgumentNullException"/>. Overrides any type rules that may exclude the <paramref name="type"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <param name="type">The type.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        public static IRegexFilter IncludeType(this IRegexFilter filter, Type type)
        {
            return filter.AddTypeRule(type, include: true);
        }

        /// <summary>
        /// Excludes the <paramref name="methodName"/> from checks for <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <param name="methodName">The method name.</param>
        /// <param name="type">The type.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        public static IRegexFilter ExcludeMethod(this IRegexFilter filter, string methodName, Type type = null)
        {
            return filter.AddMethodRule(methodName, include: false, type: type);
        }

        /// <summary>
        /// Includes the <paramref name="methodName"/> for checks for <see cref="ArgumentNullException"/>. Overrides any method rules that may exclude the <paramref name="methodName"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <param name="methodName">The method name.</param>
        /// <param name="type">The type.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        public static IRegexFilter IncludeMethod(this IRegexFilter filter, string methodName, Type type = null)
        {
            return filter.AddMethodRule(methodName, include: true, type: type);
        }

        /// <summary>
        /// Returns the <see cref="Regex"/> that matches on the <paramref name="type"/> if <paramref name="type"/> is not <c>null</c>; otherwise <c>null</c>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The <see cref="Regex"/> that matches on the <paramref name="type"/> if <paramref name="type"/> is not <c>null</c>; otherwise <c>null</c>.</returns>
        private static Regex GetTypeRegex(Type type = null)
        {
            return type == null ? null : new Regex(@"\A" + type.FullName + @"\z");
        }

        /// <summary>
        /// Returns the <see cref="Regex"/> that matches on the <paramref name="methodName"/> if <paramref name="methodName"/> is not <c>null</c>; otherwise <c>null</c>.
        /// </summary>
        /// <param name="methodName">The method name.</param>
        /// <returns>The <see cref="Regex"/> that matches on the <paramref name="methodName"/> if <paramref name="methodName"/> is not <c>null</c>; otherwise <c>null</c>.</returns>
        private static Regex GetMethodRegex(string methodName = null)
        {
            return methodName == null ? null : new Regex(@"\A" + methodName + @"\z");
        }

        /// <summary>
        /// Adds the rule to include or exclude the <paramref name="type"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <param name="type">The type.</param>
        /// <param name="include">A value indicating whether this is a include or exclude rule.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        private static IRegexFilter AddTypeRule(this IRegexFilter filter, Type type, bool include)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");
            if (type == null)
                throw new ArgumentNullException("type");

            var name = string.Concat(include ? "Include " : "Exclude ", type.Name);

            filter.Rules.Add(new RegexRule(
                                 name,
                                 include: include,
                                 type: GetTypeRegex(type)));

            return filter;
        }

        /// <summary>
        /// Adds the rule to include or exclude the <paramref name="methodName"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <param name="methodName">The method name.</param>
        /// <param name="include">A value indicating whether this is a include or exclude rule.</param>
        /// <param name="type">The type.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        private static IRegexFilter AddMethodRule(this IRegexFilter filter, string methodName, bool include, Type type = null)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");
            if (string.IsNullOrWhiteSpace(methodName))
                throw new ArgumentNullException("methodName");

            var name = string.Concat(include ? "Include " : "Exclude ", methodName);

            filter.Rules.Add(new RegexRule(
                                 name,
                                 include: include,
                                 type: GetTypeRegex(type),
                                 method: GetMethodRegex(methodName)));

            return filter;
        }
    }
}
