namespace AutoTest.ArgNullEx.Xunit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AutoTest.ArgNullEx.Filter;
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

        [Fact]
        public void HaveDefaultExclusionTypeOfTypes()
        {
            // Act
            var sut = new ExcludeAllAttribute();

            // Assert
            Assert.Equal(ExclusionType.Types, sut.ExclusionType);
        }

        [Theory, AutoMock]
        public void ReturnSelfCustomization(ExcludeAllAttribute sut, MethodInfo method)
        {
            // Act
            IArgNullExCustomization customization = sut.GetCustomization(method);

            // Assert
            Assert.Same(sut, customization);
        }

        private static void AssertSingleExcludeAllTypesRule(IRegexFilter regexFilter)
        {
            RegexRule regexRule = regexFilter.TypeRules.Single();
            Assert.False(regexRule.Include);
            Assert.True(regexRule.Type.IsMatch(Guid.NewGuid().ToString()));
        }

        private static void AssertSingleExcludeAllMethodsRule(IRegexFilter regexFilter)
        {
            RegexRule regexRule = regexFilter.MethodRules.Single();
            Assert.False(regexRule.Include);
            Assert.Null(regexRule.Type);
            Assert.True(regexRule.Method.IsMatch(Guid.NewGuid().ToString()));
        }

        private static void AssertSingleExcludeAllParametersRule(IRegexFilter regexFilter)
        {
            RegexRule regexRule = regexFilter.ParameterRules.Single();
            Assert.False(regexRule.Include);
            Assert.Null(regexRule.Type);
            Assert.Null(regexRule.Method);
            Assert.True(regexRule.Parameter.IsMatch(Guid.NewGuid().ToString()));
        }

        [Theory, AutoMock]
        public void ExcludeAllTypes(
            ExcludeAllAttribute sut,
            MethodInfo method)
        {
            // Arrange
            sut.ExclusionType = ExclusionType.Types;
            var fixture = new ArgumentNullExceptionFixture(GetType().Assembly);
            IArgNullExCustomization customization = sut.GetCustomization(method);

            // Act
            customization.Customize(fixture);
            IEnumerable<MethodData> data = fixture.GetData();

            // Assert
            Assert.Empty(data);
            IRegexFilter regexFilter = fixture.Filters.OfType<IRegexFilter>().Single();
            Assert.Equal(1, regexFilter.Rules.Count);
            AssertSingleExcludeAllTypesRule(regexFilter);
        }

        [Theory, AutoMock]
        public void ExcludeAllMethods(
            ExcludeAllAttribute sut,
            MethodInfo method)
        {
            // Arrange
            sut.ExclusionType = ExclusionType.Methods;
            var fixture = new ArgumentNullExceptionFixture(GetType().Assembly);
            IArgNullExCustomization customization = sut.GetCustomization(method);

            // Act
            customization.Customize(fixture);
            IEnumerable<MethodData> data = fixture.GetData();

            // Assert
            Assert.Empty(data);
            IRegexFilter regexFilter = fixture.Filters.OfType<IRegexFilter>().Single();
            Assert.Equal(1, regexFilter.Rules.Count);
            AssertSingleExcludeAllMethodsRule(regexFilter);
        }

        [Theory, AutoMock]
        public void ExcludeAllParameters(
            ExcludeAllAttribute sut,
            MethodInfo method)
        {
            // Arrange
            sut.ExclusionType = ExclusionType.Parameters;
            var fixture = new ArgumentNullExceptionFixture(GetType().Assembly);
            IArgNullExCustomization customization = sut.GetCustomization(method);

            // Act
            customization.Customize(fixture);
            IEnumerable<MethodData> data = fixture.GetData();

            // Assert
            Assert.Empty(data);
            IRegexFilter regexFilter = fixture.Filters.OfType<IRegexFilter>().Single();
            Assert.Equal(1, regexFilter.Rules.Count);
            AssertSingleExcludeAllParametersRule(regexFilter);
        }

        [Theory, AutoMock]
        public void ExcludesAll(
            ExcludeAllAttribute sut,
            MethodInfo method)
        {
            // Arrange
            sut.ExclusionType = ExclusionType.All;
            var fixture = new ArgumentNullExceptionFixture(GetType().Assembly);
            IArgNullExCustomization customization = sut.GetCustomization(method);

            // Act
            customization.Customize(fixture);
            IEnumerable<MethodData> data = fixture.GetData();

            // Assert
            Assert.Empty(data);
            IRegexFilter regexFilter = fixture.Filters.OfType<IRegexFilter>().Single();
            Assert.Equal(3, regexFilter.Rules.Count);
            AssertSingleExcludeAllTypesRule(regexFilter);
            AssertSingleExcludeAllMethodsRule(regexFilter);
            AssertSingleExcludeAllParametersRule(regexFilter);
        }

        [Theory, AutoMock]
        public void ExcludeNone(
            ExcludeAllAttribute sut,
            MethodInfo method)
        {
            // Arrange
            sut.ExclusionType = ExclusionType.None;
            var fixture = new ArgumentNullExceptionFixture(GetType().Assembly);
            IArgNullExCustomization customization = sut.GetCustomization(method);

            // Act
            customization.Customize(fixture);

            // Assert
            IRegexFilter regexFilter = fixture.Filters.OfType<IRegexFilter>().Single();
            Assert.Empty(regexFilter.Rules);
        }
    }
}
