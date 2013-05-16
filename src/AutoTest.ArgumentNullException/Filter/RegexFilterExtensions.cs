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
            // If the type is specified ensure it is included otherwise the
            // method may never be included.
            if (type != null)
                filter.IncludeType(type);

            return filter.AddMethodRule(methodName, include: true, type: type);
        }

        /// <summary>
        /// Excludes the <paramref name="parameterName"/> from checks for <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="type">The type.</param>
        /// <param name="methodName">The method name.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        public static IRegexFilter ExcludeParameter(this IRegexFilter filter, string parameterName, Type type = null, string methodName = null)
        {
            return filter.AddParameterRule(parameterName, include: false, type: type, methodName: methodName);
        }

        /// <summary>
        /// Includes the <paramref name="parameterName"/> for checks for <see cref="ArgumentNullException"/>. Overrides any parameter rules that may exclude the <paramref name="parameterName"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="type">The type.</param>
        /// <param name="methodName">The method name.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        public static IRegexFilter IncludeParameter(this IRegexFilter filter, string parameterName, Type type = null, string methodName = null)
        {
            // If the method or type type is specified ensure they are included
            // otherwise the parameter may never be included.
            if (!string.IsNullOrWhiteSpace(methodName))
                filter.IncludeMethod(methodName, type);
            else if (type != null)
                filter.IncludeType(type);

            return filter.AddParameterRule(parameterName, include: true, type: type, methodName: methodName);
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
        /// Returns the <see cref="Regex"/> that matches on the <paramref name="name"/> if <paramref name="name"/> is not <c>null</c>; otherwise <c>null</c>.
        /// </summary>
        /// <param name="name">The method name.</param>
        /// <returns>The <see cref="Regex"/> that matches on the <paramref name="name"/> if <paramref name="name"/> is not <c>null</c>; otherwise <c>null</c>.</returns>
        private static Regex GetNameRegex(string name = null)
        {
            return name == null ? null : new Regex(@"\A" + name + @"\z");
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
                                 method: GetNameRegex(methodName)));

            return filter;
        }

        /// <summary>
        /// Adds the rule to include or exclude the <paramref name="methodName"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <param name="parameterName">The parameter name</param>
        /// <param name="include">A value indicating whether this is a include or exclude rule.</param>
        /// <param name="type">The type.</param>
        /// <param name="methodName">The method name.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        private static IRegexFilter AddParameterRule(this IRegexFilter filter, string parameterName, bool include, Type type = null, string methodName = null)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");
            if (string.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentNullException("parameterName");

            var name = string.Concat(include ? "Include " : "Exclude ", parameterName);

            filter.Rules.Add(new RegexRule(
                                 name,
                                 include: include,
                                 type: GetTypeRegex(type),
                                 method: GetNameRegex(methodName),
                                 parameter: GetNameRegex(parameterName)));

            return filter;
        }
    }
}
