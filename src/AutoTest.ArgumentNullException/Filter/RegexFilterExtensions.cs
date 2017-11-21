// Copyright (c) 2013 - 2017 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Extension methods on <see cref="IRegexFilter"/>.
    /// </summary>
    public static class RegexFilterExtensions
    {
        /// <summary>
        /// The match all <see cref="Regex"/>.
        /// </summary>
        private static readonly Regex MatchAll = new Regex(".*");

        /// <summary>
        /// Excludes the <paramref name="type"/> from checks for <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <param name="type">The type.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="filter"/> or <paramref name="type"/> parameters
        /// are <see langword="null"/>.</exception>
        public static IRegexFilter ExcludeType(this IRegexFilter filter, Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return filter.ExcludeType(type.FullName);
        }

        /// <summary>
        /// Excludes the <paramref name="typeFullName"/> from checks for <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <param name="typeFullName">The <see cref="Type.FullName"/> of the <see cref="Type"/>.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="filter"/> or <paramref name="typeFullName"/>
        /// parameters are <see langword="null"/>.</exception>
        public static IRegexFilter ExcludeType(this IRegexFilter filter, string typeFullName)
        {
            return filter.AddTypeRule(typeFullName, include: false);
        }

        /// <summary>
        /// Includes the <paramref name="type"/> for checks for <see cref="ArgumentNullException"/>. Overrides any type
        /// rules that may exclude the <paramref name="type"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <param name="type">The type.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="filter"/> or <paramref name="type"/> parameters
        /// are <see langword="null"/>.</exception>
        public static IRegexFilter IncludeType(this IRegexFilter filter, Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return filter.IncludeType(type.FullName);
        }

        /// <summary>
        /// Includes the <paramref name="typeFullName"/> for checks for <see cref="ArgumentNullException"/>. Overrides
        /// any type rules that may exclude the <paramref name="typeFullName"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <param name="typeFullName">The <see cref="Type.FullName"/> of the <see cref="Type"/>.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="filter"/> or <paramref name="typeFullName"/>
        /// parameters are <see langword="null"/>.</exception>
        public static IRegexFilter IncludeType(this IRegexFilter filter, string typeFullName)
        {
            return filter.AddTypeRule(typeFullName, include: true);
        }

        /// <summary>
        /// Excludes the <paramref name="methodName"/> from checks for <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <param name="methodName">The method name.</param>
        /// <param name="type">The type.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="filter"/> or <paramref name="methodName"/>
        /// parameters are <see langword="null"/>.</exception>
        public static IRegexFilter ExcludeMethod(this IRegexFilter filter, string methodName, Type type)
        {
            return filter.ExcludeMethod(methodName, type != null ? type.FullName : null);
        }

        /// <summary>
        /// Excludes the <paramref name="methodName"/> from checks for <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <param name="methodName">The method name.</param>
        /// <param name="typeFullName">The <see cref="Type.FullName"/> of the <see cref="Type"/>.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="filter"/> or <paramref name="methodName"/>
        /// parameters are <see langword="null"/>.</exception>
        public static IRegexFilter ExcludeMethod(
            this IRegexFilter filter,
            string methodName,
            string typeFullName = null)
        {
            return filter.AddMethodRule(methodName, include: false, typeFullName: typeFullName);
        }

        /// <summary>
        /// Includes the <paramref name="methodName"/> for checks for <see cref="ArgumentNullException"/>. Overrides any
        /// method rules that may exclude the <paramref name="methodName"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <param name="methodName">The method name.</param>
        /// <param name="type">The type.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="filter"/> or <paramref name="methodName"/>
        /// parameters are <see langword="null"/>.</exception>
        public static IRegexFilter IncludeMethod(this IRegexFilter filter, string methodName, Type type)
        {
            return filter.IncludeMethod(methodName, type != null ? type.FullName : null);
        }

        /// <summary>
        /// Includes the <paramref name="methodName"/> for checks for <see cref="ArgumentNullException"/>. Overrides any
        /// method rules that may exclude the <paramref name="methodName"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <param name="methodName">The method name.</param>
        /// <param name="typeFullName">The type.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="filter"/> or <paramref name="methodName"/>
        /// parameters are <see langword="null"/>.</exception>
        public static IRegexFilter IncludeMethod(
            this IRegexFilter filter,
            string methodName,
            string typeFullName = null)
        {
            // If the type is specified ensure it is included otherwise the
            // method may never be included.
            if (!string.IsNullOrWhiteSpace(typeFullName))
                filter.IncludeType(typeFullName);

            return filter.AddMethodRule(methodName, include: true, typeFullName: typeFullName);
        }

        /// <summary>
        /// Excludes the <paramref name="parameterName"/> from checks for <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="type">The type.</param>
        /// <param name="methodName">The method name.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="filter"/> or <paramref name="parameterName"/>
        /// parameters are <see langword="null"/>.</exception>
        public static IRegexFilter ExcludeParameter(
            this IRegexFilter filter,
            string parameterName,
            Type type,
            string methodName = null)
        {
            return filter.ExcludeParameter(parameterName, type != null ? type.FullName : null, methodName);
        }

        /// <summary>
        /// Excludes the <paramref name="parameterName"/> from checks for <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="typeFullName">The <see cref="Type.FullName"/> of the <see cref="Type"/>.</param>
        /// <param name="methodName">The method name.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="filter"/> or <paramref name="parameterName"/>
        /// parameters are <see langword="null"/>.</exception>
        public static IRegexFilter ExcludeParameter(
            this IRegexFilter filter,
            string parameterName,
            string typeFullName = null,
            string methodName = null)
        {
            return filter.AddParameterRule(
                parameterName,
                include: false,
                typeFullName: typeFullName,
                methodName: methodName);
        }

        /// <summary>
        /// Includes the <paramref name="parameterName"/> for checks for <see cref="ArgumentNullException"/>. Overrides
        /// any parameter rules that may exclude the <paramref name="parameterName"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="type">The type.</param>
        /// <param name="methodName">The method name.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="filter"/> or <paramref name="parameterName"/>
        /// parameters are <see langword="null"/>.</exception>
        public static IRegexFilter IncludeParameter(
            this IRegexFilter filter,
            string parameterName,
            Type type,
            string methodName = null)
        {
            return filter.IncludeParameter(parameterName, type != null ? type.FullName : null, methodName);
        }

        /// <summary>
        /// Includes the <paramref name="parameterName"/> for checks for <see cref="ArgumentNullException"/>. Overrides
        /// any parameter rules that may exclude the <paramref name="parameterName"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="typeFullName">The <see cref="Type.FullName"/> of the <see cref="Type"/>.</param>
        /// <param name="methodName">The method name.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="filter"/> or <paramref name="parameterName"/>
        /// parameters are <see langword="null"/>.</exception>
        public static IRegexFilter IncludeParameter(
            this IRegexFilter filter,
            string parameterName,
            string typeFullName = null,
            string methodName = null)
        {
            // If the method or type type is specified ensure they are included
            // otherwise the parameter may never be included.
            if (!string.IsNullOrWhiteSpace(methodName))
                filter.IncludeMethod(methodName, typeFullName);
            else if (!string.IsNullOrWhiteSpace(typeFullName))
                filter.IncludeType(typeFullName);

            return filter.AddParameterRule(
                parameterName,
                include: true,
                typeFullName: typeFullName,
                methodName: methodName);
        }

        /// <summary>
        /// Excludes all types.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="filter"/> parameter is
        /// <see langword="null"/>.</exception>
        public static IRegexFilter ExcludeAllTypes(this IRegexFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");

            filter.Rules.Add(new RegexRule(
                                 "Exclude all types",
                                 include: false,
                                 type: MatchAll));

            return filter;
        }

        /// <summary>
        /// Excludes all methods.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="filter"/> parameter is
        /// <see langword="null"/>.</exception>
        public static IRegexFilter ExcludeAllMethods(this IRegexFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");

            filter.Rules.Add(new RegexRule(
                                 "Exclude all methods",
                                 include: false,
                                 method: MatchAll));

            return filter;
        }

        /// <summary>
        /// Excludes all types, methods and parameters.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="filter"/> parameter is
        /// <see langword="null"/>.</exception>
        public static IRegexFilter ExcludeAllParameters(this IRegexFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");

            filter.Rules.Add(new RegexRule(
                                 "Exclude all parameters",
                                 include: false,
                                 parameter: MatchAll));

            return filter;
        }

        /// <summary>
        /// Excludes all types, methods and parameters.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="filter"/> parameter is
        /// <see langword="null"/>.</exception>
        public static IRegexFilter ExcludeAll(this IRegexFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");

            return
                filter.ExcludeAllTypes()
                      .ExcludeAllMethods()
                      .ExcludeAllParameters();
        }

        /// <summary>
        /// Returns the <see cref="Regex"/> that matches on the <paramref name="name"/> if <paramref name="name"/> is
        /// not <see langword="null"/>; otherwise <see langword="null"/>.
        /// </summary>
        /// <param name="name">The method name.</param>
        /// <returns>The <see cref="Regex"/> that matches on the <paramref name="name"/> if <paramref name="name"/> is
        /// not <see langword="null"/>; otherwise <see langword="null"/>.</returns>
        private static Regex GetNameRegex(string name = null)
        {
            return name == null ? null : new Regex(@"\A" + Regex.Escape(name) + @"\z");
        }

        /// <summary>
        /// Adds the rule to include or exclude the <paramref name="typeFullName"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <param name="typeFullName">The <see cref="Type.FullName"/> of the <see cref="Type"/>.</param>
        /// <param name="include">A value indicating whether this is a include or exclude rule.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="filter"/> or <paramref name="typeFullName"/>
        /// parameters are <see langword="null"/>.</exception>
        private static IRegexFilter AddTypeRule(this IRegexFilter filter, string typeFullName, bool include)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");
            if (string.IsNullOrWhiteSpace(typeFullName))
                throw new ArgumentNullException("typeFullName");

            var name = string.Concat(include ? "Include " : "Exclude ", typeFullName);

            filter.Rules.Add(new RegexRule(
                                 name,
                                 include: include,
                                 type: GetNameRegex(typeFullName)));

            return filter;
        }

        /// <summary>
        /// Adds the rule to include or exclude the <paramref name="methodName"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <param name="methodName">The method name.</param>
        /// <param name="include">A value indicating whether this is a include or exclude rule.</param>
        /// <param name="typeFullName">The <see cref="Type.FullName"/> of the <see cref="Type"/>.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="filter"/> or <paramref name="methodName"/>
        /// parameters are <see langword="null"/>.</exception>
        private static IRegexFilter AddMethodRule(
            this IRegexFilter filter,
            string methodName,
            bool include,
            string typeFullName = null)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");
            if (string.IsNullOrWhiteSpace(methodName))
                throw new ArgumentNullException("methodName");

            var name = string.Concat(include ? "Include " : "Exclude ", methodName);

            filter.Rules.Add(new RegexRule(
                                 name,
                                 include: include,
                                 type: GetNameRegex(typeFullName),
                                 method: GetNameRegex(methodName)));

            return filter;
        }

        /// <summary>
        /// Adds the rule to include or exclude the <paramref name="methodName"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Regex"/> filter.</param>
        /// <param name="parameterName">The parameter name</param>
        /// <param name="include">A value indicating whether this is a include or exclude rule.</param>
        /// <param name="typeFullName">The <see cref="Type.FullName"/> of the <see cref="Type"/>.</param>
        /// <param name="methodName">The method name.</param>
        /// <returns>The <paramref name="filter"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="filter"/> or <paramref name="parameterName"/>
        /// parameters are <see langword="null"/>.</exception>
        private static IRegexFilter AddParameterRule(
            this IRegexFilter filter,
            string parameterName,
            bool include,
            string typeFullName = null,
            string methodName = null)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");
            if (string.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentNullException("parameterName");

            var name = string.Concat(include ? "Include " : "Exclude ", parameterName);

            filter.Rules.Add(new RegexRule(
                                 name,
                                 include: include,
                                 type: GetNameRegex(typeFullName),
                                 method: GetNameRegex(methodName),
                                 parameter: GetNameRegex(parameterName)));

            return filter;
        }
    }
}
