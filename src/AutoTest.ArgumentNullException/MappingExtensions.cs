namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoTest.ArgNullEx.Mapping;

    /// <summary>
    /// Extension methods on <see cref="ArgumentNullExceptionFixture"/> for mapping.
    /// </summary>
    public static class MappingExtensions
    {
        /// <summary>
        /// Adds the mapping to substitute the <paramref name="originalType"/> with the <paramref name="newType"/>.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        /// <param name="originalType">The original <see cref="Type"/>.</param>
        /// <param name="newType">The new <see cref="Type"/>.</param>
        /// <returns>The <paramref name="fixture"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="fixture"/>, <paramref name="originalType"/>, or
        /// <paramref name="newType"/> parameters are <see langword="null"/>.</exception>
        public static IArgumentNullExceptionFixture SubstituteType(
            this IArgumentNullExceptionFixture fixture,
            Type originalType,
            Type newType)
        {
            if (originalType == null)
                throw new ArgumentNullException("originalType");
            if (newType == null)
                throw new ArgumentNullException("newType");

            fixture.GetSubstituteTypeMapping().Substitute(originalType, newType);

            return fixture;
        }

        /// <summary>
        /// Gets the single <see cref="SubstituteType"/> from the <see cref="IArgumentNullExceptionFixture.Mappings"/>.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        /// <returns>The single <see cref="SubstituteType"/> from the
        /// <see cref="IArgumentNullExceptionFixture.Mappings"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="fixture"/> parameter is
        /// <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">There are zero of more than one <see cref="SubstituteType"/>
        /// objects in the <see cref="IArgumentNullExceptionFixture.Filters"/>.</exception>
        private static SubstituteType GetSubstituteTypeMapping(this IArgumentNullExceptionFixture fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");

            var substituteType =
                fixture.Mappings
                       .OfType<SubstituteType>()
                       .SingleOrDefault();

            if (substituteType == null)
                throw new InvalidOperationException("There is no SubstituteType in the mappings.");

            return substituteType;
        }
    }
}
