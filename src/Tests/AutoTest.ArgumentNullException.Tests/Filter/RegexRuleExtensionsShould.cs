namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using Moq;
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
        public void IgnoreTypeRegexIfNull(Type type, Mock<MethodBase> methodMock)
        {
            // Arrange
            methodMock.SetupGet(m => m.Name).Returns("Name" + Guid.NewGuid());
            var rule = new RegexRule(methodMock.Object.Name + " hit rule", method: new Regex(methodMock.Object.Name));

            // Act
            bool actual = rule.MatchMethod(type, methodMock.Object);

            // Assert
            Assert.True(actual);
        }

        [Theory, AutoMock]
        public void ApplyTypeRegexIfProvided(Type type, Mock<MethodBase> methodMock)
        {
            // Arrange
            methodMock.SetupGet(m => m.Name).Returns("Name" + Guid.NewGuid());
            var rule = new RegexRule(
                methodMock.Object.Name + " hit rule",
                type: new Regex(Guid.NewGuid().ToString()),
                method: new Regex(methodMock.Object.Name));

            // Act
            bool actual = rule.MatchMethod(type, methodMock.Object);

            // Assert
            Assert.False(actual);
        }

        [Theory, AutoMock]
        public void NotMatchAMethod(Type type, Mock<MethodBase> methodMock)
        {
            // Arrange
            methodMock.SetupGet(m => m.Name).Returns(Guid.NewGuid().ToString());
            var rule = new RegexRule("Miss rule", method: new Regex("Miss"));

            // Act
            bool actual = rule.MatchMethod(type, methodMock.Object);

            // Assert
            Assert.False(actual);
        }

        [Theory, AutoMock]
        public void ThrowIfNoTypeRuleForMethodMatch(Type type, MethodBase method)
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
