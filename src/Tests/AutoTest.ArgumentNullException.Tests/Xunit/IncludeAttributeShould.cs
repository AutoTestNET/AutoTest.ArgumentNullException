namespace AutoTest.ArgNullEx.Xunit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AutoTest.ArgNullEx.Filter;
    using Moq;
    using global::Xunit;
    using global::Xunit.Extensions;

    public class IncludeAttributeShould
    {
        [Theory, AutoMock]
        public void BeACustomization(IncludeAttribute sut)
        {
            // Assert
            Assert.IsAssignableFrom<IArgNullExCustomization>(sut);
            Assert.IsAssignableFrom<CustomizeAttribute>(sut);
        }

        [Theory, AutoMock]
        public void ReturnSelfCustomization(IncludeAttribute sut, MethodInfo method)
        {
            // Act
            IArgNullExCustomization customization = sut.GetCustomization(method);

            // Assert
            Assert.Same(sut, customization);
        }

        private static void Execute(
            IncludeAttribute sut,
            MethodInfo method,
            List<IFilter> filters,
            List<RegexRule> regexRules,
            Mock<IRegexFilter> regexFilterMock,
            Mock<IArgumentNullExceptionFixture> fixtureMock,
            int additionalRules)
        {
            // Arrange
            sut.ExclusionType = ExclusionType.Types;
            regexFilterMock.SetupGet(r => r.Rules).Returns(regexRules);
            filters.Add(regexFilterMock.Object);
            fixtureMock.SetupGet(f => f.Filters).Returns(filters);
            List<RegexRule> existingRules = regexRules.ToList();

            // Act
            IArgNullExCustomization customization = sut.GetCustomization(method);
            customization.Customize(fixtureMock.Object);

            // Assert
            Assert.Equal(existingRules.Count + additionalRules, regexRules.Count);
            Assert.False(existingRules.Except(regexRules).Any());
        }

        [Theory, AutoMock]
        public void ExcludeAllTypesAndIncludeParameter(
            IncludeAttribute sut,
            MethodInfo method,
            List<IFilter> filters,
            List<RegexRule> regexRules,
            Mock<IRegexFilter> regexFilterMock,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            sut.TypeFullName = null;
            Execute(sut, method, filters, regexRules, regexFilterMock, fixtureMock, additionalRules: 4);
        }

        [Theory, AutoMock]
        public void ExcludeAllTypesAndIncludeParameterWithTypeByName(
            IncludeAttribute sut,
            MethodInfo method,
            List<IFilter> filters,
            List<RegexRule> regexRules,
            Mock<IRegexFilter> regexFilterMock,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            sut.Type = null;
            Execute(sut, method, filters, regexRules, regexFilterMock, fixtureMock, additionalRules: 4);
        }

        [Theory, AutoMock]
        public void ExcludeAllTypesAndIncludeMethod(
            IncludeAttribute sut,
            MethodInfo method,
            List<IFilter> filters,
            List<RegexRule> regexRules,
            Mock<IRegexFilter> regexFilterMock,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            sut.Parameter = null;
            sut.TypeFullName = null;
            Execute(sut, method, filters, regexRules, regexFilterMock, fixtureMock, additionalRules: 3);
        }

        [Theory, AutoMock]
        public void ExcludeAllTypesAndIncludeMethodWithTypeByName(
            IncludeAttribute sut,
            MethodInfo method,
            List<IFilter> filters,
            List<RegexRule> regexRules,
            Mock<IRegexFilter> regexFilterMock,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            sut.Parameter = null;
            sut.Type = null;
            Execute(sut, method, filters, regexRules, regexFilterMock, fixtureMock, additionalRules: 3);
        }

        [Theory, AutoMock]
        public void ExcludeAllTypesAndIncludeType(
            IncludeAttribute sut,
            MethodInfo method,
            List<IFilter> filters,
            List<RegexRule> regexRules,
            Mock<IRegexFilter> regexFilterMock,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            sut.Parameter = null;
            sut.Method = null;
            sut.TypeFullName = null;
            Execute(sut, method, filters, regexRules, regexFilterMock, fixtureMock, additionalRules: 2);
        }

        [Theory, AutoMock]
        public void ExcludeAllTypesAndIncludeTypeByName(
            IncludeAttribute sut,
            MethodInfo method,
            List<IFilter> filters,
            List<RegexRule> regexRules,
            Mock<IRegexFilter> regexFilterMock,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            sut.Parameter = null;
            sut.Method = null;
            sut.Type = null;
            Execute(sut, method, filters, regexRules, regexFilterMock, fixtureMock, additionalRules: 2);
        }

        [Theory, AutoMock]
        public void ThrowIfBothTypeAndTypeFullNameSpecified(
            IncludeAttribute sut,
            MethodInfo method,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            // AAA
            IArgNullExCustomization customization = sut.GetCustomization(method);
            Assert.Throws<InvalidOperationException>(() => customization.Customize(fixtureMock.Object));
        }

        [Theory, AutoMock]
        public void ThrowIfNothingSpecified(
            IncludeAttribute sut,
            MethodInfo method,
            List<IFilter> filters,
            List<RegexRule> regexRules,
            Mock<IRegexFilter> regexFilterMock,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            // Arrange
            regexFilterMock.SetupGet(r => r.Rules).Returns(regexRules);
            filters.Add(regexFilterMock.Object);
            fixtureMock.SetupGet(f => f.Filters).Returns(filters);

            // Act
            sut.Parameter = null;
            sut.Method = null;
            sut.Type = null;
            sut.TypeFullName = null;
            IArgNullExCustomization customization = sut.GetCustomization(method);
            Assert.Throws<InvalidOperationException>(() => customization.Customize(fixtureMock.Object));
        }
    }
}
