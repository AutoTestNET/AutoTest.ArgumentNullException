namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    /// <summary>
    /// A filter to include or exclude using <see cref="Regex"/> matching.
    /// </summary>
    public class RegexFilter : FilterBase, ITypeFilter, IMethodFilter, IParameterFilter, IRegexFilter
    {
        /// <summary>
        /// The list of rules.
        /// </summary>
        private readonly List<RegexRule> _rules = new List<RegexRule>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RegexFilter" /> class.
        /// </summary>
        /// <param name="rules">The initial rules.</param>
        public RegexFilter(IEnumerable<RegexRule> rules)
        {
            if (rules == null)
                throw new ArgumentNullException("rules");

            _rules = rules.ToList();
        }

        /// <summary>
        /// Gets all the <see cref="Regex"/> rules for types.
        /// </summary>
        public IEnumerable<RegexRule> TypeRules
        {
            get
            {
                return _rules.Where(r => r.Type != null && r.Method == null && r.Parameter == null);
            }
        }

        /// <summary>
        /// Gets all the <see cref="Regex"/> rules for methods.
        /// </summary>
        public IEnumerable<RegexRule> MethodRules
        {
            get
            {
                return _rules.Where(r => r.Method != null && r.Parameter == null);
            }
        }

        /// <summary>
        /// Gets all the <see cref="Regex"/> rules for parameter.
        /// </summary>
        public IEnumerable<RegexRule> ParameterRules
        {
            get
            {
                return _rules.Except(TypeRules.Union(MethodRules));
            }
        }

        /// <summary>
        /// Filters out types based on the <see cref="Regex"/> rules.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the type should be included, otherwise <c>false</c>.</returns>
        bool ITypeFilter.IncludeType(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            return true;
        }

        /// <summary>
        /// Filters out methods based on the <see cref="Regex"/> rules.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        /// <returns><c>true</c> if the <paramref name="method"/> should be included, otherwise <c>false</c>.</returns>
        bool IMethodFilter.IncludeMethod(Type type, MethodBase method)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (method == null) throw new ArgumentNullException("method");

            return true;
        }

        /// <summary>
        /// Filters out parameter based on the <see cref="Regex"/> rules.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns><c>true</c> if the <paramref name="parameter"/> should be included, otherwise <c>false</c>.</returns>
        bool IParameterFilter.IncludeParameter(Type type, MethodBase method, ParameterInfo parameter)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (method == null) throw new ArgumentNullException("method");
            if (parameter == null) throw new ArgumentNullException("parameter");

            return true;
        }
    }
}
