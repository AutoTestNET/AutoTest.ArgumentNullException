namespace AutoTest.ArgNullEx.Xunit
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AutoTest.ArgNullEx.Filter;
    using Moq;
    using global::Xunit;
    using global::Xunit.Extensions;

    public class ExcludeAllAttributeShould
    {
        [Theory, AutoMock]
        public void BeACustomization(ExcludeAllAttribute sut)
        {
            // Assert
            Assert.IsAssignableFrom<IArgNullExCustomization>(sut);
            Assert.IsAssignableFrom<CustomizeAttribute>(sut);
        }

        [Theory, AutoMock]
        public void ReturnSelfCustomization(ExcludeAllAttribute sut, MethodInfo method)
        {
            // Act
            IArgNullExCustomization customization = sut.GetCustomization(method);

            // Assert
            Assert.Same(sut, customization);
        }

        [Theory, AutoMock]
        public void ExcludeAllTypes(
            ExcludeAllAttribute sut,
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
    }
}
