namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using global::Xunit;
    using global::Xunit.Extensions;

    public class RegexRuleExtensionsShould
    {
        #region MatchType

        [Theory, AutoMock]
        public void MatchAType(Type type)
        {
            // Arrange
            var rule = new RegexRule(
                type.Name + " hit rule",
                include: true,
                type: new Regex(@".+\." + type.Name));

            // Act
            bool actual = rule.MatchType(type);

            // Assert
            Assert.True(actual);
        }

        [Theory, AutoMock]
        public void NotMatchAType(Type type)
        {
            // Arrange
            var rule = new RegexRule("Miss rule", type: new Regex("Miss"));

            // Act
            bool actual = rule.MatchType(type);

            // Assert
            Assert.False(actual);
        }

        [Theory, AutoMock]
        public void ThrowIfNoTypeRuleForTypeMatch(Type type)
        {
            // Arrange
            var rule = new RegexRule("Throw rule");

            // Act/Assert
            string paramName = Assert.Throws<ArgumentException>(() => rule.MatchType(type)).ParamName;
            Assert.Equal("rule", paramName);
        }

        #endregion MatchType

        #region MatchMethod

        [Theory, AutoMock]
        public void ThrowIfNoTypeRuleForMethodMatch(Type type, MethodInfo method)
        {
            // Arrange
            var rule = new RegexRule("Throw rule");

            // Act/Assert
            string paramName = Assert.Throws<ArgumentException>(() => rule.MatchMethod(type, method)).ParamName;
            Assert.Equal("rule", paramName);
        }

        #endregion MatchMethod
    }
}
