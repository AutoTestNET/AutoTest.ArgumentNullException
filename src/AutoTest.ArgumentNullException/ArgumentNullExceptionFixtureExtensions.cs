namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AutoTest.ArgNullEx.Filter;

    /// <summary>
    /// Extension methods on <see cref="ArgumentNullExceptionFixture"/>.
    /// </summary>
    public static class ArgumentNullExceptionFixtureExtensions
    {
        /// <summary>
        /// Clears the <paramref name="mask"/> of <see cref="BindingFlags"/> on the <paramref name="fixture"/>.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        /// <param name="mask">The mask of <see cref="BindingFlags"/>.</param>
        /// <returns>The <paramref name="fixture"/>.</returns>
        public static IArgumentNullExceptionFixture ClearBindingFlags(this IArgumentNullExceptionFixture fixture, BindingFlags mask)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");

            fixture.BindingFlags = fixture.BindingFlags & ~mask;

            return fixture;
        }

        /// <summary>
        /// Sets the <paramref name="mask"/> of <see cref="BindingFlags"/> on the <paramref name="fixture"/>.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        /// <param name="mask">The mask of <see cref="BindingFlags"/>.</param>
        /// <returns>The <paramref name="fixture"/>.</returns>
        public static IArgumentNullExceptionFixture SetBindingFlags(this IArgumentNullExceptionFixture fixture, BindingFlags mask)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");

            fixture.BindingFlags = fixture.BindingFlags | mask;

            return fixture;
        }

        /// <summary>
        /// Removes all the filters that are instances of the <paramref name="filterType"/>.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        /// <param name="filterType">The type of filter.</param>
        /// <returns>The <paramref name="fixture"/>.</returns>
        public static IArgumentNullExceptionFixture RemoveFilters(this IArgumentNullExceptionFixture fixture, Type filterType)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");
            if (filterType == null)
                throw new ArgumentNullException("filterType");

            fixture.Filters.RemoveAll(filterType.IsInstanceOfType);

            return fixture;
        }

        /// <summary>
        /// Removes all the supplied <paramref name="filtersToRemove"/> from the <paramref name="fixture"/> collection of <see cref="IArgumentNullExceptionFixture.Filters"/>.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        /// <param name="filtersToRemove">The filters to remove.</param>
        /// <returns>The <paramref name="fixture"/>.</returns>
        public static IArgumentNullExceptionFixture RemoveFilters(this IArgumentNullExceptionFixture fixture, IEnumerable<IFilter> filtersToRemove)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");
            if (filtersToRemove == null)
                throw new ArgumentNullException("filtersToRemove");

            foreach (IFilter filter in filtersToRemove)
            {
                fixture.Filters.Remove(filter);
            }
 
            return fixture;
        }

        /// <summary>
        /// Adds the <paramref name="filters"/> to the <paramref name="fixture"/> collection of <see cref="IArgumentNullExceptionFixture.Filters"/> if they are not already in he collection.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        /// <param name="filters">The filters to add.</param>
        /// <returns>The <paramref name="fixture"/>.</returns>
        public static IArgumentNullExceptionFixture AddFilters(this IArgumentNullExceptionFixture fixture, IEnumerable<IFilter> filters)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");
            if (filters == null)
                throw new ArgumentNullException("filters");

            foreach (IFilter filter in filters.Where(filter => !fixture.Filters.Contains(filter)))
            {
                fixture.Filters.Add(filter);
            }

            return fixture;
        }

        /// <summary>
        /// Excludes the <paramref name="type"/> from checks for <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        /// <param name="type">The type.</param>
        /// <returns>The <paramref name="fixture"/>.</returns>
        public static IArgumentNullExceptionFixture ExcludeType(this IArgumentNullExceptionFixture fixture, Type type)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");
            if (type == null)
                throw new ArgumentNullException("type");

            IRegexFilter regexFilter = fixture.GetRegexFilter();

            regexFilter.ExcludeType(type);

            return fixture;
        }

        /// <summary>
        /// Includes the <paramref name="type"/> for checks for <see cref="ArgumentNullException"/>. Overrides any type rules that may exclude the <paramref name="type"/>.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        /// <param name="type">The type.</param>
        /// <returns>The <paramref name="fixture"/>.</returns>
        public static IArgumentNullExceptionFixture IncludeType(this IArgumentNullExceptionFixture fixture, Type type)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");
            if (type == null)
                throw new ArgumentNullException("type");

            IRegexFilter regexFilter = fixture.GetRegexFilter();

            regexFilter.IncludeType(type);

            return fixture;
        }

        /// <summary>
        /// Excludes the <paramref name="methodName"/> from checks for <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        /// <param name="methodName">The method name.</param>
        /// <param name="type">The type.</param>
        /// <returns>The <paramref name="fixture"/>.</returns>
        public static IArgumentNullExceptionFixture ExcludeMethod(this IArgumentNullExceptionFixture fixture, string methodName, Type type = null)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");
            if (string.IsNullOrWhiteSpace(methodName))
                throw new ArgumentNullException("methodName");

            IRegexFilter regexFilter = fixture.GetRegexFilter();

            regexFilter.ExcludeMethod(methodName, type);

            return fixture;
        }

        /// <summary>
        /// Includes the <paramref name="methodName"/> for checks for <see cref="ArgumentNullException"/>. Overrides any method rules that may exclude the <paramref name="methodName"/>.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        /// <param name="methodName">The method name.</param>
        /// <param name="type">The type.</param>
        /// <returns>The <paramref name="fixture"/>.</returns>
        public static IArgumentNullExceptionFixture IncludeMethod(this IArgumentNullExceptionFixture fixture, string methodName, Type type = null)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");
            if (string.IsNullOrWhiteSpace(methodName))
                throw new ArgumentNullException("methodName");

            IRegexFilter regexFilter = fixture.GetRegexFilter();

            regexFilter.IncludeMethod(methodName, type);

            return fixture;
        }

        /// <summary>
        /// Excludes the <paramref name="parameterName"/> from checks for <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="type">The type.</param>
        /// <param name="methodName">The method name.</param>
        /// <returns>The <paramref name="fixture"/>.</returns>
        public static IArgumentNullExceptionFixture ExcludeParameter(this IArgumentNullExceptionFixture fixture, string parameterName, Type type = null, string methodName = null)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");
            if (string.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentNullException("parameterName");

            IRegexFilter regexFilter = fixture.GetRegexFilter();

            regexFilter.ExcludeParameter(parameterName, type, methodName);

            return fixture;
        }

        /// <summary>
        /// Includes the <paramref name="parameterName"/> for checks for <see cref="ArgumentNullException"/>. Overrides any parameter rules that may exclude the <paramref name="parameterName"/>.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="type">The type.</param>
        /// <param name="methodName">The method name.</param>
        /// <returns>The <paramref name="fixture"/>.</returns>
        public static IArgumentNullExceptionFixture IncludeParameter(this IArgumentNullExceptionFixture fixture, string parameterName, Type type = null, string methodName = null)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");
            if (string.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentNullException("parameterName");

            IRegexFilter regexFilter = fixture.GetRegexFilter();

            regexFilter.IncludeParameter(parameterName, type, methodName);

            return fixture;
        }

        /// <summary>
        /// Excludes all types.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        /// <returns>The <paramref name="fixture"/>.</returns>
        public static IArgumentNullExceptionFixture ExcludeAllTypes(this IArgumentNullExceptionFixture fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");

            IRegexFilter regexFilter = fixture.GetRegexFilter();

            regexFilter.ExcludeAllTypes();

            return fixture;
        }

        /// <summary>
        /// Excludes all methods.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        /// <returns>The <paramref name="fixture"/>.</returns>
        public static IArgumentNullExceptionFixture ExcludeAllMethods(this IArgumentNullExceptionFixture fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");

            IRegexFilter regexFilter = fixture.GetRegexFilter();

            regexFilter.ExcludeAllMethods();

            return fixture;
        }

        /// <summary>
        /// Excludes all parameters.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        /// <returns>The <paramref name="fixture"/>.</returns>
        public static IArgumentNullExceptionFixture ExcludeAllParameters(this IArgumentNullExceptionFixture fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");

            IRegexFilter regexFilter = fixture.GetRegexFilter();

            regexFilter.ExcludeAllParameters();

            return fixture;
        }

        /// <summary>
        /// Excludes all types, methods and parameters.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        /// <returns>The <paramref name="fixture"/>.</returns>
        public static IArgumentNullExceptionFixture ExcludeAll(this IArgumentNullExceptionFixture fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");

            IRegexFilter regexFilter = fixture.GetRegexFilter();

            regexFilter.ExcludeAll();

            return fixture;
        }

        /// <summary>
        /// Applies the <paramref name="customization"/>.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        /// <param name="customization">The customization to apply.</param>
        /// <returns>The <paramref name="fixture"/>.</returns>
        public static IArgumentNullExceptionFixture Customize(this IArgumentNullExceptionFixture fixture, IArgNullExCustomization customization)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");
            if (customization == null)
                throw new ArgumentNullException("customization");

            customization.Customize(fixture);
            return fixture;
        }

        /// <summary>
        /// Gets the single <see cref="IRegexFilter"/> from the <see cref="IArgumentNullExceptionFixture.Filters"/>.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        /// <returns>The single <see cref="IRegexFilter"/> from the <see cref="IArgumentNullExceptionFixture.Filters"/>.</returns>
        /// <exception cref="InvalidOperationException">There are zero of more than one <see cref="IRegexFilter"/> objects in the <see cref="IArgumentNullExceptionFixture.Filters"/>.</exception>
        private static IRegexFilter GetRegexFilter(this IArgumentNullExceptionFixture fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");

            var regexFilter =
                fixture.Filters
                       .OfType<IRegexFilter>()
                       .SingleOrDefault();

            if (regexFilter == null)
                throw new InvalidOperationException("There is no IRegexFilter in the filters.");

            return regexFilter;
        }
    }
}
