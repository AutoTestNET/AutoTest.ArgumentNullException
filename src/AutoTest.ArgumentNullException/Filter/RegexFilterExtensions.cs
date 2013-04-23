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
        /// <param name="type">The type to exclude.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        public static IRegexFilter ExcludeType(this IRegexFilter filter, Type type)
        {
            return filter.AddTypeRule(type, include: false);
        }

        /// <summary>
        /// Includes the <paramref name="type"/> for checks for <see cref="ArgumentNullException"/>. Overrides any type rules that may exclude the <paramref name="type"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <param name="type">The type to include.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        public static IRegexFilter IncludeType(this IRegexFilter filter, Type type)
        {
            return filter.AddTypeRule(type, include: true);
        }

        /// <summary>
        /// Adds the rule to include or exclude the <paramref name="type"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <param name="type">The type to include.</param>
        /// <param name="include">A value indicating whether this is a include or exclude rule.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        private static IRegexFilter AddTypeRule(this IRegexFilter filter, Type type, bool include)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");
            if (type == null)
                throw new ArgumentNullException("type");

            var name = string.Concat(include ? "Include " : "Exclude ", type.Name);

            filter.Rules.Add(new RegexRule(name, include: include, type: new Regex(@"\A" + type.FullName + @"\z")));

            return filter;
        }
    }
}
