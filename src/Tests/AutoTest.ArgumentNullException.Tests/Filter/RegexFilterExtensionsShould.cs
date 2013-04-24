namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Moq;
    using global::Xunit;
    using global::Xunit.Extensions;

    public class RegexFilterExtensionsShould
    {
        #region Exclude/Include Type

// ReSharper disable UnusedParameter.Local
        private void AssertTypeRule(Func<Type, IRegexFilter> addMethod, bool expectedInclude)
        {
            IRegexFilter actualFilter = addMethod(GetType());

            Assert.Same(addMethod.Target, actualFilter);
            Assert.Equal(1, actualFilter.Rules.Count);
            RegexRule addedRule = actualFilter.Rules.Single();
            Assert.Equal(expectedInclude, addedRule.Include);
            Assert.NotNull(addedRule.Type);
            Assert.Null(addedRule.Method);
            Assert.Null(addedRule.Parameter);
            Assert.True(addedRule.MatchType(GetType()));
        }
// ReSharper restore UnusedParameter.Local

        [Fact]
        public void AddExcludeTypeRule()
        {
            // AAA
            AssertTypeRule(new RegexFilter().ExcludeType, expectedInclude: false);
        }

        [Fact]
        public void AddIncludeTypeRule()
        {
            // AAA
            AssertTypeRule(new RegexFilter().IncludeType, expectedInclude: true);
        }

        #endregion Exclude/Include Type

        #region Exclude/Include Method

// ReSharper disable UnusedParameter.Local
        private void AssertMethodRule(Func<string, Type, IRegexFilter> addMethod, MethodBase method, Type type, bool expectedInclude)
        {
            IRegexFilter actualFilter = addMethod(method.Name, type);

            Assert.Same(addMethod.Target, actualFilter);
            Assert.Equal(1, actualFilter.Rules.Count);
            RegexRule addedRule = actualFilter.Rules.Single();
            Assert.Equal(expectedInclude, addedRule.Include);
            Assert.NotNull(addedRule.Method);
            Assert.True(addedRule.MatchMethod(type ?? GetType(), method));
            Assert.Null(addedRule.Parameter);

            if (type == null)
            {
                Assert.Null(addedRule.Type);
            }
            else
            {
                Assert.NotNull(addedRule.Type);
                Assert.True(addedRule.MatchType(type));
            }
        }
// ReSharper restore UnusedParameter.Local

        [Theory, AutoMock]
        public void AddExcludeMethodRuleWithType(
            Type type,
            Mock<MethodBase> methodMock)
        {
            // Arrange
            methodMock.SetupGet(m => m.Name).Returns("name" + Guid.NewGuid());

            // Act/Assert
            AssertMethodRule(new RegexFilter().ExcludeMethod, methodMock.Object, type, expectedInclude: false);
        }

        [Theory, AutoMock]
        public void AddExcludeMethodRuleWithoutType(
            Mock<MethodBase> methodMock)
        {
            // Arrange
            methodMock.SetupGet(m => m.Name).Returns("name" + Guid.NewGuid());

            // Act/Assert
            AssertMethodRule(new RegexFilter().ExcludeMethod, methodMock.Object, type: null, expectedInclude: false);
        }

        [Theory, AutoMock]
        public void AddIncludeMethodRuleWithType(
            Type type,
            Mock<MethodBase> methodMock)
        {
            // Arrange
            methodMock.SetupGet(m => m.Name).Returns("name" + Guid.NewGuid());

            // Act/Assert
            AssertMethodRule(new RegexFilter().IncludeMethod, methodMock.Object, type, expectedInclude: true);
        }

        [Theory, AutoMock]
        public void AddIncludeMethodRuleWithoutType(
            Mock<MethodBase> methodMock)
        {
            // Arrange
            methodMock.SetupGet(m => m.Name).Returns("name" + Guid.NewGuid());

            // Act/Assert
            AssertMethodRule(new RegexFilter().IncludeMethod, methodMock.Object, type: null, expectedInclude: true);
        }

        #endregion Exclude/Include Method
    }
}
