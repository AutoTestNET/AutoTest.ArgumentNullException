namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Diagnostics;
    using System.Text.RegularExpressions;

    /// <summary>
    /// The <see cref="Regex"/>s to match on a filter.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class RegexRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegexRule" /> class.
        /// </summary>
        /// <param name="name">The name of the rule.</param>
        /// <param name="match">A value indicating whether this is a match or not match (miss) rule.</param>
        /// <param name="type">The <see cref="Regex"/> to match on the type.</param>
        /// <param name="method">The <see cref="Regex"/> to match on the method.</param>
        /// <param name="parameter">The <see cref="Regex"/> to match on the parameter.</param>
        public RegexRule(string name, bool match, Regex type = null, Regex method = null, Regex parameter = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            Name = name;
            Match = match;
            Type = type;
            Method = method;
            Parameter = parameter;
        }

        /// <summary>
        /// Gets the name of the rule.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this is a match or not match (miss) rule.
        /// </summary>
        public bool Match { get; private set; }

        /// <summary>
        /// Gets the <see cref="Regex"/> to match on the type.
        /// </summary>
        public Regex Type { get; private set; }

        /// <summary>
        /// Gets the <see cref="Regex"/> to match on the method.
        /// </summary>
        public Regex Method { get; private set; }

        /// <summary>
        /// Gets the <see cref="Regex"/> to match on the parameter.
        /// </summary>
        public Regex Parameter { get; private set; }

        /// <summary>
        /// Gets the text to display within the debugger.
        /// </summary>
// ReSharper disable UnusedMember.Local
        private string DebuggerDisplay
        {
            get { return "RegexRule: " + Name; }
        }
// ReSharper restore UnusedMember.Local
    }
}
