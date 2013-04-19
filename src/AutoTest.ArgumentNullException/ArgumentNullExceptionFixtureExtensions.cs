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
        /// <returns>The value of the <see cref="ArgumentNullExceptionFixture.BindingFlags"/> before being cleared.</returns>
        public static BindingFlags ClearBindingFlags(this ArgumentNullExceptionFixture fixture, BindingFlags mask)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");

            BindingFlags oldValue = fixture.BindingFlags;

            fixture.BindingFlags = oldValue & ~mask;

            return oldValue;
        }

        /// <summary>
        /// Sets the <paramref name="mask"/> of <see cref="BindingFlags"/> on the <paramref name="fixture"/>.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        /// <param name="mask">The mask of <see cref="BindingFlags"/>.</param>
        /// <returns>The value of the <see cref="ArgumentNullExceptionFixture.BindingFlags"/> before being set.</returns>
        public static BindingFlags SetBindingFlags(this ArgumentNullExceptionFixture fixture, BindingFlags mask)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");

            BindingFlags oldValue = fixture.BindingFlags;

            fixture.BindingFlags = oldValue | mask;

            return oldValue;
        }
    }
}
