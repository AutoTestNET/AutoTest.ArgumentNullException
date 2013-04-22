namespace AutoTest.ArgNullEx.Filter
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <summary>
    /// A filter to include or exclude using <see cref="Regex"/> matching.
    /// </summary>
    public interface IRegexFilter
    {
        /// <summary>
        /// Gets the list of rules.
        /// </summary>
        List<RegexRule> Rules { get; }
    }
}
