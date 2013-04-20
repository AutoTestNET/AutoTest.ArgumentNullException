namespace AutoTest.ArgNullEx
{
    using System;
    using System.Reflection;

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
    }
}
