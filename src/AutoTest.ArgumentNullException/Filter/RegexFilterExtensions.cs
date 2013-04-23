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
            if (filter == null)
                throw new ArgumentNullException("filter");
            if (type == null)
                throw new ArgumentNullException("type");

            filter.Rules.Add(new RegexRule("Exclude " + type.Name, include: false, type: new Regex(@"\A" + type.FullName + @"\z")));

            return filter;
        }
    }
}
