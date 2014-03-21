namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// A filter to include or exclude using <see cref="Regex"/> matching.
    /// </summary>
    public interface IRegexFilter : IFilter
    {
        /// <summary>
        /// Gets the list of rules.
        /// </summary>
        List<RegexRule> Rules { get; }

        /// <summary>
        /// Gets all the <see cref="Regex"/> rules for types.
        /// </summary>
        IEnumerable<RegexRule> TypeRules { get; }

        /// <summary>
        /// Gets all the <see cref="Regex"/> rules for methods.
        /// </summary>
        IEnumerable<RegexRule> MethodRules { get; }

        /// <summary>
        /// Gets all the <see cref="Regex"/> rules for parameters.
        /// </summary>
        IEnumerable<RegexRule> ParameterRules { get; }
    }
}
