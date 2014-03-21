namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        public void IgnoreTypeRegexIfNullWhenMatchMethod(Type type, Mock<MethodBase> methodMock)
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
        public void ApplyTypeRegexIfProvidedWhenMatchMethod(Type type, Mock<MethodBase> methodMock)
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
            methodMock.SetupGet(m => m.Name).Returns("Name" + Guid.NewGuid());
            var rule = new RegexRule("Miss rule", method: new Regex("Miss"));

            // Act
            bool actual = rule.MatchMethod(type, methodMock.Object);

            // Assert
            Assert.False(actual);
        }

        [Theory, AutoMock]
        public void ThrowIfNoMethodRuleForMatchMethod(Type type, MethodBase method)
        {
            // Arrange
            var rule = new RegexRule("Throw rule");

            // Act/Assert
            string paramName = Assert.Throws<ArgumentException>(() => rule.MatchMethod(type, method)).ParamName;
            Assert.Equal("rule", paramName);
        }

        #endregion MatchMethod

        #region MatchParameter

        [Theory, AutoMock]
        public void IgnoreTypeAndMethodRegexIfNullWhenMatchParameter(
            Type type,
            Mock<MethodBase> methodMock,
            Mock<ParameterInfo> parameterMock)
        {
            // Arrange
            parameterMock.SetupGet(p => p.Name).Returns("Name" + Guid.NewGuid());
            var rule = new RegexRule(parameterMock.Object.Name + " hit rule", parameter: new Regex(parameterMock.Object.Name));

            // Act
            bool actual = rule.MatchParameter(type, methodMock.Object, parameterMock.Object);

            // Assert
            Assert.True(actual);
        }

        [Theory, AutoMock]
        public void ApplyTypeRegexIfProvidedWhenMatchParameter(
            Type type,
            Mock<MethodBase> methodMock,
            Mock<ParameterInfo> parameterMock)
        {
            // Arrange
            parameterMock.SetupGet(p => p.Name).Returns("Name" + Guid.NewGuid());
            var rule = new RegexRule(
                methodMock.Object.Name + " hit rule",
                type: new Regex(Guid.NewGuid().ToString()),
                parameter: new Regex(parameterMock.Object.Name));

            // Act
            bool actual = rule.MatchParameter(type, methodMock.Object, parameterMock.Object);

            // Assert
            Assert.False(actual);
        }

        [Theory, AutoMock]
        public void ApplyMethodRegexIfProvidedWhenMatchParameter(
            Type type,
            Mock<MethodBase> methodMock,
            Mock<ParameterInfo> parameterMock)
        {
            // Arrange
            parameterMock.SetupGet(p => p.Name).Returns("Name" + Guid.NewGuid());
            methodMock.SetupGet(p => p.Name).Returns("Name" + Guid.NewGuid());
            var rule = new RegexRule(
                methodMock.Object.Name + " hit rule",
                method: new Regex(Guid.NewGuid().ToString()),
                parameter: new Regex(parameterMock.Object.Name));

            // Act
            bool actual = rule.MatchParameter(type, methodMock.Object, parameterMock.Object);

            // Assert
            Assert.False(actual);
        }

        [Theory, AutoMock]
        public void NotMatchAParameter(
            Type type,
            Mock<MethodBase> methodMock,
            Mock<ParameterInfo> parameterMock)
        {
            // Arrange
            parameterMock.SetupGet(m => m.Name).Returns("Name" + Guid.NewGuid());
            var rule = new RegexRule("Miss rule", parameter: new Regex("Miss"));

            // Act
            bool actual = rule.MatchParameter(type, methodMock.Object, parameterMock.Object);

            // Assert
            Assert.False(actual);
        }

        [Theory, AutoMock]
        public void ThrowIfNoParameterRuleForMatchParameter(
            Type type,
            MethodBase method,
            Mock<ParameterInfo> parameterMock)
        {
            // Arrange
            var rule = new RegexRule("Throw rule");

            // Act/Assert
            string paramName = Assert.Throws<ArgumentException>(() => rule.MatchParameter(type, method, parameterMock.Object)).ParamName;
            Assert.Equal("rule", paramName);
        }

        #endregion MatchParameter
    }
}
