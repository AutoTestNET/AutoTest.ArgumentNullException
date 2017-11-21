namespace AutoTest.ArgNullEx.Xunit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AutoTest.ArgNullEx.Filter;
    using Moq;
    using global::Xunit;

    public class ExcludeAttributeShould
    {
        [Theory, AutoMock]
        public void BeACustomization(ExcludeAttribute sut)
        {
            // Assert
            Assert.IsAssignableFrom<IArgNullExCustomization>(sut);
            Assert.IsAssignableFrom<CustomizeAttribute>(sut);
        }

        [Theory, AutoMock]
        public void ReturnSelfCustomization(ExcludeAttribute sut, MethodInfo method)
        {
            // Act
            IArgNullExCustomization customization = sut.GetCustomization(method);

            // Assert
            Assert.Same(sut, customization);
        }

        private static void Execute(
            ExcludeAttribute sut,
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
            List<RegexRule> existingRules = regexRules.ToList();

            // Act
            IArgNullExCustomization customization = sut.GetCustomization(method);
            customization.Customize(fixtureMock.Object);

            // Assert
            Assert.Equal(existingRules.Count + 1, regexRules.Count);
            Assert.False(existingRules.Except(regexRules).Any());
        }

        [Theory, AutoMock]
        public void ExcludeParameter(
            ExcludeAttribute sut,
            MethodInfo method,
            List<IFilter> filters,
            List<RegexRule> regexRules,
            Mock<IRegexFilter> regexFilterMock,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            sut.TypeFullName = null;
            Execute(sut, method, filters, regexRules, regexFilterMock, fixtureMock);
        }

        [Theory, AutoMock]
        public void ExcludeParameterWithTypeByName(
            ExcludeAttribute sut,
            MethodInfo method,
            List<IFilter> filters,
            List<RegexRule> regexRules,
            Mock<IRegexFilter> regexFilterMock,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            sut.Type = null;
            Execute(sut, method, filters, regexRules, regexFilterMock, fixtureMock);
        }

        [Theory, AutoMock]
        public void ExcludeMethod(
            ExcludeAttribute sut,
            MethodInfo method,
            List<IFilter> filters,
            List<RegexRule> regexRules,
            Mock<IRegexFilter> regexFilterMock,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            sut.Parameter = null;
            sut.TypeFullName = null;
            Execute(sut, method, filters, regexRules, regexFilterMock, fixtureMock);
        }

        [Theory, AutoMock]
        public void ExcludeMethodWithTypeByName(
            ExcludeAttribute sut,
            MethodInfo method,
            List<IFilter> filters,
            List<RegexRule> regexRules,
            Mock<IRegexFilter> regexFilterMock,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            sut.Parameter = null;
            sut.Type = null;
            Execute(sut, method, filters, regexRules, regexFilterMock, fixtureMock);
        }

        [Theory, AutoMock]
        public void ExcludeType(
            ExcludeAttribute sut,
            MethodInfo method,
            List<IFilter> filters,
            List<RegexRule> regexRules,
            Mock<IRegexFilter> regexFilterMock,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            sut.Parameter = null;
            sut.Method = null;
            sut.TypeFullName = null;
            Execute(sut, method, filters, regexRules, regexFilterMock, fixtureMock);
        }

        [Theory, AutoMock]
        public void ExcludeTypeByName(
            ExcludeAttribute sut,
            MethodInfo method,
            List<IFilter> filters,
            List<RegexRule> regexRules,
            Mock<IRegexFilter> regexFilterMock,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            sut.Parameter = null;
            sut.Method = null;
            sut.Type = null;
            Execute(sut, method, filters, regexRules, regexFilterMock, fixtureMock);
        }

        [Theory, AutoMock]
        public void ThrowIfBothTypeAndTypeFullNameSpecified(
            ExcludeAttribute sut,
            MethodInfo method,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            // AAA
            IArgNullExCustomization customization = sut.GetCustomization(method);
            Assert.Throws<InvalidOperationException>(() => customization.Customize(fixtureMock.Object));
        }

        [Theory, AutoMock]
        public void ThrowIfNothingSpecified(
            ExcludeAttribute sut,
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
