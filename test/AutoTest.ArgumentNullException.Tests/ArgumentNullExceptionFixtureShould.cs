namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using AutoFixture;
    using AutoFixture.Xunit2;
    using AutoTest.ArgNullEx.Execution;
    using AutoTest.ArgNullEx.Filter;
    using AutoTest.ArgNullEx.Mapping;
    using Moq;
    using global::Xunit;

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
            Assert.NotNull(sut.Mappings);
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
            [Frozen] List<IMapping> mappings,
            [Greedy] ArgumentNullExceptionFixture sut)
        {
            // AAA
            Assert.Same(expectedFixture, ((SpecimenProvider)sut.SpecimenProvider).Builder);
            Assert.Same(expectedAssembly, sut.AssemblyUnderTest);
            Assert.Same(filters, sut.Filters);
            Assert.Same(mappings, sut.Mappings);
            Assert.Equal(ArgumentNullExceptionFixture.DefaultBindingFlags, sut.BindingFlags);
            Assert.NotNull(sut.TypeFilters);
            Assert.NotNull(sut.MethodFilters);
            Assert.NotNull(sut.ParameterFilters);
        }

        [Theory, AutoMock]
        internal void CreateStaticMethodInvocationData(
            SpecimenProvider specimenProvider,
            RegexFilter filter)
        {
            // Arrange
            filter.IncludeMethod("Compare", typeof (Uri))
                  .Rules.Add(new RegexRule("exclude all", type: new Regex(".*"), method: new Regex(".*")));

            IArgumentNullExceptionFixture sut =
                new ArgumentNullExceptionFixture(typeof(Uri).GetTypeInfo().Assembly,
                                                 specimenProvider,
                                                 new List<IFilter> { filter },
                                                 new List<IMapping>());

            // Act
            List<MethodData> methodDatas = sut.GetData().ToList();

            // Assert
            Assert.Equal(5, methodDatas.Count);
            Assert.Equal(5, methodDatas.Select(m => m.ExecutionSetup).OfType<DefaultExecutionSetup>().Count());
        }

        [Theory, AutoMock]
        internal void CreateInstanceMethodInvocationData(
            SpecimenProvider specimenProvider,
            RegexFilter filter)
        {
            // Arrange
            filter.IncludeMethod("GetComponents", typeof(Uri))
                  .Rules.Add(new RegexRule("exclude all", type: new Regex(".*"), method: new Regex(".*")));

            IArgumentNullExceptionFixture sut =
                new ArgumentNullExceptionFixture(typeof(Uri).GetTypeInfo().Assembly,
                                                 specimenProvider,
                                                 new List<IFilter> { filter },
                                                 new List<IMapping>());

            // Act
            List<MethodData> methodDatas = sut.GetData().ToList();

            // Assert
            Assert.Equal(2, methodDatas.Count);
            Assert.Equal(2, methodDatas.Select(m => m.ExecutionSetup).OfType<DefaultExecutionSetup>().Count());
        }

        [Theory, AutoMock]
        public void CreateErroredMethodInvocationData(
            Mock<ISpecimenProvider> specimenProviderMock,
            FileNotFoundException exception,
            RegexFilter filter)
        {
            // Arrange
            specimenProviderMock.Setup(sp => sp.GetParameterSpecimens(It.IsAny<IList<ParameterInfo>>(), It.IsAny<int>()))
                                .Throws(exception);
            filter.IncludeMethod("Compare", typeof(Uri))
                  .Rules.Add(new RegexRule("exclude all", type: new Regex(".*"), method: new Regex(".*")));

            IArgumentNullExceptionFixture sut =
                new ArgumentNullExceptionFixture(typeof(Uri).GetTypeInfo().Assembly,
                                                 specimenProviderMock.Object,
                                                 new List<IFilter> { filter },
                                                 new List<IMapping>());

            // Act
            List<MethodData> methodDatas = sut.GetData().ToList();

            // Assert
            Assert.Equal(5, methodDatas.Count);
            List<ErroredExecutionSetup> executionSetups = methodDatas.Select(m => m.ExecutionSetup).OfType<ErroredExecutionSetup>().ToList();
            Assert.Equal(5, executionSetups.Count);
            foreach (ErroredExecutionSetup executionSetup in executionSetups)
            {
                CompositionException compositionException = Assert.IsType<CompositionException>(executionSetup.Exception);
                Assert.Same(exception, compositionException.InnerException);
            }
        }

        [Theory, AutoMock]
        internal void ApplyParameterFilters(
            SpecimenProvider specimenProvider,
            RegexFilter filter)
        {
            // Arrange
            filter.IncludeMethod("Compare", typeof (Uri))
                  .ExcludeParameter("uri1", typeof (Uri))
                  .ExcludeParameter("uri2", typeof(Uri), "Compare")
                  .Rules.Add(new RegexRule("exclude all", type: new Regex(".*"), method: new Regex(".*")));

            IArgumentNullExceptionFixture sut =
                new ArgumentNullExceptionFixture(typeof(Uri).GetTypeInfo().Assembly,
                                                 specimenProvider,
                                                 new List<IFilter> { filter },
                                                 new List<IMapping>());

            // Act
            List<MethodData> methodDatas = sut.GetData().ToList();

            // Assert
            Assert.Equal(3, methodDatas.Count);
            Assert.Equal(3, methodDatas.Select(m => m.ExecutionSetup).OfType<DefaultExecutionSetup>().Count());
        }
    }
}
