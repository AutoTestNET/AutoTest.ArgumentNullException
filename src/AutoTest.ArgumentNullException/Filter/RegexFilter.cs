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
        /// Gets the list of rules.
        /// </summary>
        public List<RegexRule> Rules
        {
            get { return _rules; }
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
        /// Gets all the <see cref="Regex"/> rules for types.
        /// </summary>
        public IEnumerable<RegexRule> IncludeTypeRules
        {
            get { return TypeRules.Where(r => r.Include); }
        }

        /// <summary>
        /// Gets all the <see cref="Regex"/> rules for types.
        /// </summary>
        public IEnumerable<RegexRule> ExcludeTypeRules
        {
            get { return TypeRules.Where(r => !r.Include); }
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
        /// Gets all the <see cref="Regex"/> rules for Methods.
        /// </summary>
        public IEnumerable<RegexRule> IncludeMethodRules
        {
            get { return MethodRules.Where(r => r.Include); }
        }

        /// <summary>
        /// Gets all the <see cref="Regex"/> rules for Methods.
        /// </summary>
        public IEnumerable<RegexRule> ExcludeMethodRules
        {
            get { return MethodRules.Where(r => !r.Include); }
        }

        /// <summary>
        /// Gets all the <see cref="Regex"/> rules for parameter.
        /// </summary>
        public IEnumerable<RegexRule> ParameterRules
        {
            get
            {
                return _rules.Except(TypeRules.Concat(MethodRules));
            }
        }

        /// <summary>
        /// Gets all the <see cref="Regex"/> rules for Parameters.
        /// </summary>
        public IEnumerable<RegexRule> IncludeParameterRules
        {
            get { return ParameterRules.Where(r => r.Include); }
        }

        /// <summary>
        /// Gets all the <see cref="Regex"/> rules for Parameters.
        /// </summary>
        public IEnumerable<RegexRule> ExcludeParameterRules
        {
            get { return ParameterRules.Where(r => !r.Include); }
        }

        /// <summary>
        /// Filters out types based on the <see cref="Regex"/> rules.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the <paramref name="type"/> should be excluded, otherwise <c>false</c>.</returns>
        bool ITypeFilter.ExcludeType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            // Include the type if it matches any of the include rules
            // or if it matches none on the exclude rules.
            return !IncludeTypeRules.Any(r => r.MatchType(type))
                   && ExcludeTypeRules.Any(r => r.MatchType(type));
        }

        /// <summary>
        /// Filters out methods based on the <see cref="Regex"/> rules.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        /// <returns><c>true</c> if the <paramref name="method"/> should be excluded, otherwise <c>false</c>.</returns>
        bool IMethodFilter.ExcludeMethod(Type type, MethodBase method)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (method == null)
                throw new ArgumentNullException("method");

            // Include the method if it matches any of the include rules
            // or if it matches none on the exclude rules.
            return !IncludeMethodRules.Any(r => r.MatchMethod(type, method))
                   && ExcludeMethodRules.Any(r => r.MatchMethod(type, method));
        }

        /// <summary>
        /// Filters out parameter based on the <see cref="Regex"/> rules.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns><c>true</c> if the <paramref name="parameter"/> should be excluded, otherwise <c>false</c>.</returns>
        bool IParameterFilter.ExcludeParameter(Type type, MethodBase method, ParameterInfo parameter)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (method == null)
                throw new ArgumentNullException("method");
            if (parameter == null)
                throw new ArgumentNullException("parameter");

            // Include the parameter if it matches any of the include rules
            // or if it matches none on the exclude rules.
            return !IncludeParameterRules.Any(r => r.MatchParameter(type, method, parameter))
                   && ExcludeParameterRules.Any(r => r.MatchParameter(type, method, parameter));
        }
    }
}
