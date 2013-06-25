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

        [Theory, AutoMock]
        public void ExcludeAllTypesAndIndludeParameter(
            IncludeAttribute sut,
            MethodInfo method,
            List<IFilter> filters,
            List<RegexRule> regexRules,
            Mock<IRegexFilter> regexFilterMock,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
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
            Assert.Equal(existingRules.Count + 4, regexRules.Count);
            Assert.False(existingRules.Except(regexRules).Any());
        }

        [Theory, AutoMock]
        public void ExcludeAllTypesAndIndludeMethod(
            IncludeAttribute sut,
            MethodInfo method,
            List<IFilter> filters,
            List<RegexRule> regexRules,
            Mock<IRegexFilter> regexFilterMock,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            // Arrange
            sut.ExclusionType = ExclusionType.Types;
            regexFilterMock.SetupGet(r => r.Rules).Returns(regexRules);
            filters.Add(regexFilterMock.Object);
            fixtureMock.SetupGet(f => f.Filters).Returns(filters);
            List<RegexRule> existingRules = regexRules.ToList();

            // Act
            sut.Parameter = null;
            IArgNullExCustomization customization = sut.GetCustomization(method);
            customization.Customize(fixtureMock.Object);

            // Assert
            Assert.Equal(existingRules.Count + 3, regexRules.Count);
            Assert.False(existingRules.Except(regexRules).Any());
        }

        [Theory, AutoMock]
        public void ExcludeAllTypesAndIndludeType(
            IncludeAttribute sut,
            MethodInfo method,
            List<IFilter> filters,
            List<RegexRule> regexRules,
            Mock<IRegexFilter> regexFilterMock,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            // Arrange
            sut.ExclusionType = ExclusionType.Types;
            regexFilterMock.SetupGet(r => r.Rules).Returns(regexRules);
            filters.Add(regexFilterMock.Object);
            fixtureMock.SetupGet(f => f.Filters).Returns(filters);
            List<RegexRule> existingRules = regexRules.ToList();

            // Act
            sut.Parameter = null;
            sut.Method = null;
            IArgNullExCustomization customization = sut.GetCustomization(method);
            customization.Customize(fixtureMock.Object);

            // Assert
            Assert.Equal(existingRules.Count + 2, regexRules.Count);
            Assert.False(existingRules.Except(regexRules).Any());
        }

        [Theory, AutoMock]
        public void ThrowsIfNothingSpecified(
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
            IArgNullExCustomization customization = sut.GetCustomization(method);
            Assert.Throws<InvalidOperationException>(() => customization.Customize(fixtureMock.Object));
        }
    }
}
