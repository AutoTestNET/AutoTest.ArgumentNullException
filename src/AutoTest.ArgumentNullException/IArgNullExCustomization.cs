namespace AutoTest.ArgNullEx
{
    /// <summary>
    /// Encapsulates a customization of an <see cref="IArgumentNullExceptionFixture"/>.
    /// </summary>
    public interface IArgNullExCustomization
    {
        /// <summary>
        /// Customizes the specified <paramref name="fixture"/>.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        void Customize(IArgumentNullExceptionFixture fixture);
    }
}
