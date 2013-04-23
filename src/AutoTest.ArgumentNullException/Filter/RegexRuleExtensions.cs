namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Extension methods on <see cref="RegexRule"/>.
    /// </summary>
    public static class RegexRuleExtensions
    {
        /// <summary>
        /// Checks whether the supplied <paramref name="type"/> matches the <paramref name="rule"/>.
        /// </summary>
        /// <param name="rule">The rule to match against the <paramref name="type"/>.</param>
        /// <param name="type">The type to check if it matches the <paramref name="rule"/>.</param>
        /// <returns><c>true</c> if the <paramref name="rule"/> matches against the <paramref name="type"/>; otherwise <c>false</c>.</returns>
        public static bool MatchType(this RegexRule rule, Type type)
        {
            if (rule == null)
                throw new ArgumentNullException("rule");
            if (type == null)
                throw new ArgumentNullException("type");
            if (rule.Type == null)
                throw new ArgumentException("The rule has a null Type regular expression.", "rule");

            return rule.Type.IsMatch(type.FullName);
        }

        /// <summary>
        /// Checks whether the supplied <paramref name="method"/> matches the <paramref name="rule"/>.
        /// </summary>
        /// <param name="rule">The rule to match against the <paramref name="method"/>.</param>
        /// <param name="type">The type to check if it matches the <paramref name="rule"/> if <see cref="RegexRule.Type"/> is not null.</param>
        /// <param name="method">The method to check if it matches the <paramref name="rule"/>.</param>
        /// <returns><c>true</c> if the <paramref name="rule"/> matches against the <paramref name="method"/>; otherwise <c>false</c>.</returns>
        public static bool MatchMethod(this RegexRule rule, Type type, MethodBase method)
        {
            if (rule == null)
                throw new ArgumentNullException("rule");
            if (type == null)
                throw new ArgumentNullException("type");
            if (method == null)
                throw new ArgumentNullException("method");
            if (rule.Method == null)
                throw new ArgumentException("The rule has a null Method regular expression.", "rule");

            // If there is a type regular expression it must match the type.
            if (rule.Type != null && !rule.MatchType(type))
                return false;

            return rule.Method.IsMatch(method.Name);
        }
    }
}
