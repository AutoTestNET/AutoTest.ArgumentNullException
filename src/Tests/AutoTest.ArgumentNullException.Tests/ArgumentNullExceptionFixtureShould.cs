namespace AutoTest.ArgNullEx
{
    using System.Collections.Generic;
    using System.Reflection;
    using AutoTest.ArgNullEx.Filter;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit;
    using global::Xunit;
    using global::Xunit.Extensions;

    public class ArgumentNullExceptionFixtureShould
    {
        [Theory, AutoMock]
        public void InitializeDefaults(
            [Frozen] Assembly expectedAssembly,
            [Modest] ArgumentNullExceptionFixture sut)
        {
            // AAA
            Assert.NotNull(sut.SpecimenProvider);
            Assert.Same(expectedAssembly, sut.AssemblyUnderTest);
            Assert.Equal(ArgumentNullExceptionFixture.DefaultBindingFlags, sut.BindingFlags);
            Assert.NotNull(sut.Filters);
            Assert.NotNull(sut.TypeFilters);
            Assert.NotNull(sut.MethodFilters);
            Assert.NotNull(sut.ParameterFilters);
        }

        [Theory, AutoMock]
        public void InitializeInjectedDefaultsManual(
            [Frozen] Assembly expectedAssembly,
            [Frozen] IFixture expectedFixture)
        {
            // Arrange/Act
            var sut = new ArgumentNullExceptionFixture(expectedAssembly, expectedFixture);

            // Assert
            Assert.Same(expectedFixture, ((SpecimenProvider)sut.SpecimenProvider).Builder);
            Assert.Same(expectedAssembly, sut.AssemblyUnderTest);
            Assert.Equal(ArgumentNullExceptionFixture.DefaultBindingFlags, sut.BindingFlags);
            Assert.NotNull(sut.Filters);
            Assert.NotNull(sut.TypeFilters);
            Assert.NotNull(sut.MethodFilters);
            Assert.NotNull(sut.ParameterFilters);
        }

        [Theory, AutoMock]
        public void InitializeInjectedDefaults(
            [Frozen] Assembly expectedAssembly,
            [Frozen] IFixture expectedFixture,
            [Frozen] List<IFilter> filters,
            [Greedy] ArgumentNullExceptionFixture sut)
        {
            // AAA
            Assert.Same(expectedFixture, ((SpecimenProvider)sut.SpecimenProvider).Builder);
            Assert.Same(expectedAssembly, sut.AssemblyUnderTest);
            Assert.Same(filters, sut.Filters);
            Assert.Equal(ArgumentNullExceptionFixture.DefaultBindingFlags, sut.BindingFlags);
            Assert.NotNull(sut.TypeFilters);
            Assert.NotNull(sut.MethodFilters);
            Assert.NotNull(sut.ParameterFilters);
        }
    }
}
