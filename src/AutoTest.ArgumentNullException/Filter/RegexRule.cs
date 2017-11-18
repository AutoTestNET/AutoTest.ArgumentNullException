// Copyright (c) 2013 - 2017 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// The <see cref="Regex"/>s to Include on a filter.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class RegexRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegexRule" /> class.
        /// </summary>
        /// <param name="name">The name of the rule.</param>
        /// <param name="include">A value indicating whether this is a include or exclude rule.</param>
        /// <param name="type">The <see cref="Regex"/> to include or exclude the type.</param>
        /// <param name="method">The <see cref="Regex"/> to include or exclude the method.</param>
        /// <param name="parameter">The <see cref="Regex"/> to include or exclude the parameter.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="name"/> parameter is
        /// <see langword="null"/>.</exception>
        public RegexRule(
            string name,
            bool include = false,
            Regex type = null,
            Regex method = null,
            Regex parameter = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            Name = name;
            Include = include;
            Type = type;
            Method = method;
            Parameter = parameter;
        }

        /// <summary>
        /// Gets the name of the rule.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this is a include or exclude rule.
        /// </summary>
        public bool Include { get; private set; }

        /// <summary>
        /// Gets the <see cref="Regex"/> to include or exclude the type.
        /// </summary>
        public Regex Type { get; private set; }

        /// <summary>
        /// Gets the <see cref="Regex"/> to include or exclude the method.
        /// </summary>
        public Regex Method { get; private set; }

        /// <summary>
        /// Gets the <see cref="Regex"/> to include or exclude the parameter.
        /// </summary>
        public Regex Parameter { get; private set; }

        /// <summary>
        /// Gets the text to display within the debugger.
        /// </summary>
        private string DebuggerDisplay
        {
            get { return "RegexRule: " + Name; }
        }
    }
}
