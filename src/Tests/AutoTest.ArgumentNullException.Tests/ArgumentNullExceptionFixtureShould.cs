namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using AutoTest.ArgNullEx.Execution;
    using AutoTest.ArgNullEx.Filter;
    using Moq;
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
            IFixture expectedFixture)
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
            IFixture expectedFixture,
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

        [Theory, AutoMock]
        internal void CreateMethodInvocationData(
            SpecimenProvider specimenProvider,
            RegexFilter filter)
        {
            // Arrange
            filter.IncludeMethod("Compare", typeof (Uri))
                  .Rules.Add(new RegexRule("exclude all", type: new Regex(".*"), method: new Regex(".*")));

            IArgumentNullExceptionFixture sut =
                new ArgumentNullExceptionFixture(typeof(Uri).Assembly,
                                                 specimenProvider,
                                                 new List<IFilter> { filter });

            // Act
            List<MethodData> methodDatas = sut.GetData().ToList();

            // Assert
            Assert.Equal(5, methodDatas.Count);
            Assert.Equal(5, methodDatas.Select(m => m.ExecutionSetup).OfType<DefaultExecutionSetup>().Count());
        }

        [Theory, AutoMock]
        public void CreateErroredMethodInvocationData(
            Mock<ISpecimenProvider> specimenProviderMock,
            RegexFilter filter)
        {
            // Arrange
            specimenProviderMock.Setup(sp => sp.GetParameterSpecimens(It.IsAny<IList<ParameterInfo>>(), It.IsAny<int>()))
                                .Throws(new Exception());
            filter.IncludeMethod("Compare", typeof(Uri))
                  .Rules.Add(new RegexRule("exclude all", type: new Regex(".*"), method: new Regex(".*")));

            IArgumentNullExceptionFixture sut =
                new ArgumentNullExceptionFixture(typeof(Uri).Assembly,
                                                 specimenProviderMock.Object,
                                                 new List<IFilter> { filter });

            // Act
            List<MethodData> methodDatas = sut.GetData().ToList();

            // Assert
            Assert.Equal(5, methodDatas.Count);
            Assert.Equal(5, methodDatas.Select(m => m.ExecutionSetup).OfType<ErroredExecutionSetup>().Count());
        }
    }
}
