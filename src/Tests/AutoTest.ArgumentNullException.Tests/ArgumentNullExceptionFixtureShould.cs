namespace AutoTest.ArgNullEx
{
    using System.Reflection;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit;
    using global::Xunit;
    using global::Xunit.Extensions;

    public class ArgumentNullExceptionFixtureShould
    {
        [Theory, AutoMock]
        public void InitializeDefaults(
            [Frozen] Assembly expectedAssembly,
            ArgumentNullExceptionFixture sut)
        {
            // AAA
            Assert.NotNull(sut.Fixture);
            Assert.Same(expectedAssembly, sut.AssemblyUnderTest);
            Assert.Equal(ArgumentNullExceptionFixture.DefaultBindingFlags, sut.BindingFlags);
            Assert.NotNull(sut.TypeFilters);
            Assert.NotNull(sut.MethodFilters);
            Assert.NotNull(sut.ParameterFilters);
        }

        [Theory, AutoMock]
        public void InitializeInjectedDefaults(
            [Frozen] IFixture expectedFixture,
            [Frozen] Assembly expectedAssembly,
            [Greedy] ArgumentNullExceptionFixture sut)
        {
            // AAA
            Assert.Same(expectedFixture, sut.Fixture);
            Assert.Same(expectedAssembly, sut.AssemblyUnderTest);
            Assert.Equal(ArgumentNullExceptionFixture.DefaultBindingFlags, sut.BindingFlags);
            Assert.NotNull(sut.TypeFilters);
            Assert.NotNull(sut.MethodFilters);
            Assert.NotNull(sut.ParameterFilters);
        }
    }
}
