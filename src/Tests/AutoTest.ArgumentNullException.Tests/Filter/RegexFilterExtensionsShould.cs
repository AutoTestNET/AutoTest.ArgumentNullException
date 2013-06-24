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

            int expectedRules = 1;
            if (expectedInclude && type != null) expectedRules++;

            Assert.Same(addMethod.Target, actualFilter);
            Assert.Equal(expectedRules, actualFilter.Rules.Count);
            RegexRule addedRule = actualFilter.MethodRules.Single();
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

        #region Exclude/Include Parameter

// ReSharper disable UnusedParameter.Local
        private void AssertParameterRule(Func<string, Type, string, IRegexFilter> addMethod, ParameterInfo parameter, Type type, MethodBase method, bool expectedInclude)
        {
            IRegexFilter actualFilter = addMethod(parameter.Name, type, method == null ? null : method.Name);

            int expectedRules = 1;
            if (expectedInclude && method != null) expectedRules++;
            if (expectedInclude && type != null) expectedRules++;

            Assert.Same(addMethod.Target, actualFilter);
            Assert.Equal(expectedRules, actualFilter.Rules.Count);
            RegexRule addedRule = actualFilter.ParameterRules.Single();
            Assert.Equal(expectedInclude, addedRule.Include);
            Assert.NotNull(addedRule.Parameter);
            Assert.True(addedRule.MatchParameter(type ?? GetType(), method ?? new Mock<MethodBase>().Object, parameter));

            if (type == null)
            {
                Assert.Null(addedRule.Type);
            }
            else
            {
                Assert.NotNull(addedRule.Type);
                Assert.True(addedRule.MatchType(type));
            }

            if (method == null)
            {
                Assert.Null(addedRule.Method);
            }
            else
            {
                Assert.NotNull(addedRule.Method);
                Assert.True(addedRule.MatchMethod(type ?? GetType(), method));
            }
        }
// ReSharper restore UnusedParameter.Local

        [Theory, AutoMock]
        public void AddExcludeParameterRuleWithType(
            Type type,
            Mock<ParameterInfo> parameterMock)
        {
            // Arrange
            parameterMock.SetupGet(p => p.Name).Returns("Name" + Guid.NewGuid());

            // Act/Assert
            AssertParameterRule(new RegexFilter().ExcludeParameter, parameterMock.Object, type, method: null, expectedInclude: false);
        }

        [Theory, AutoMock]
        public void AddExcludeParameterRuleWithMethod(
            Mock<MethodBase> methodMock,
            Mock<ParameterInfo> parameterMock)
        {
            // Arrange
            methodMock.SetupGet(m => m.Name).Returns("Name" + Guid.NewGuid());
            parameterMock.SetupGet(p => p.Name).Returns("Name" + Guid.NewGuid());

            // Act/Assert
            AssertParameterRule(
                new RegexFilter().ExcludeParameter,
                parameterMock.Object,
                type: null,
                method: methodMock.Object,
                expectedInclude: false);
        }

        [Theory, AutoMock]
        public void AddExcludeParameterRuleWithTypeAndMethod(
            Type type,
            Mock<MethodBase> methodMock,
            Mock<ParameterInfo> parameterMock)
        {
            // Arrange
            methodMock.SetupGet(m => m.Name).Returns("Name" + Guid.NewGuid());
            parameterMock.SetupGet(p => p.Name).Returns("Name" + Guid.NewGuid());

            // Act/Assert
            AssertParameterRule(
                new RegexFilter().ExcludeParameter,
                parameterMock.Object,
                type: type,
                method: methodMock.Object,
                expectedInclude: false);
        }

        [Theory, AutoMock]
        public void AddExcludeParameterRuleWithoutTypeOrMethod(
            Mock<ParameterInfo> parameterMock)
        {
            // Arrange
            parameterMock.SetupGet(p => p.Name).Returns("Name" + Guid.NewGuid());

            // Act/Assert
            AssertParameterRule(new RegexFilter().ExcludeParameter, parameterMock.Object, type: null, method: null, expectedInclude: false);
        }

        [Theory, AutoMock]
        public void AddIncludeParameterRuleWithType(
            Type type,
            Mock<ParameterInfo> parameterMock)
        {
            // Arrange
            parameterMock.SetupGet(p => p.Name).Returns("Name" + Guid.NewGuid());

            // Act/Assert
            AssertParameterRule(new RegexFilter().IncludeParameter, parameterMock.Object, type, method: null, expectedInclude: true);
        }

        [Theory, AutoMock]
        public void AddIncludeParameterRuleWithMethod(
            Mock<MethodBase> methodMock,
            Mock<ParameterInfo> parameterMock)
        {
            // Arrange
            methodMock.SetupGet(m => m.Name).Returns("Name" + Guid.NewGuid());
            parameterMock.SetupGet(p => p.Name).Returns("Name" + Guid.NewGuid());

            // Act/Assert
            AssertParameterRule(
                new RegexFilter().IncludeParameter,
                parameterMock.Object,
                type: null,
                method: methodMock.Object,
                expectedInclude: true);
        }

        [Theory, AutoMock]
        public void AddIncludeParameterRuleWithTypeAndMethod(
            Type type,
            Mock<MethodBase> methodMock,
            Mock<ParameterInfo> parameterMock)
        {
            // Arrange
            methodMock.SetupGet(m => m.Name).Returns("Name" + Guid.NewGuid());
            parameterMock.SetupGet(p => p.Name).Returns("Name" + Guid.NewGuid());

            // Act/Assert
            AssertParameterRule(
                new RegexFilter().IncludeParameter,
                parameterMock.Object,
                type: type,
                method: methodMock.Object,
                expectedInclude: true);
        }

        [Theory, AutoMock]
        public void AddIncludeParameterRuleWithoutTypeOrMethod(
            Mock<ParameterInfo> parameterMock)
        {
            // Arrange
            parameterMock.SetupGet(p => p.Name).Returns("Name" + Guid.NewGuid());

            // Act/Assert
            AssertParameterRule(new RegexFilter().IncludeParameter, parameterMock.Object, type: null, method: null, expectedInclude: true);
        }

        #endregion Exclude/Include Parameter

        #region ExcludeAll

        [Fact]
        public void ExcludeAllTypes()
        {
            // Arrange
            var sut = new RegexFilter();

            // Act
            IRegexFilter result = sut.ExcludeAllTypes();

            // Assert
            Assert.Same(sut, result);
            Assert.Equal(1, sut.Rules.Count);
            RegexRule typeRule = sut.TypeRules.Single();
            Assert.True(typeRule.Type.IsMatch(Guid.NewGuid().ToString()));
        }

        [Fact]
        public void ExcludeAllMethods()
        {
            // Arrange
            var sut = new RegexFilter();

            // Act
            IRegexFilter result = sut.ExcludeAllMethods();

            // Assert
            Assert.Same(sut, result);
            Assert.Equal(1, sut.Rules.Count);
            RegexRule methodRule = sut.MethodRules.Single();
            Assert.True(methodRule.Method.IsMatch(Guid.NewGuid().ToString()));
        }

        [Fact]
        public void ExcludeAllParameters()
        {
            // Arrange
            var sut = new RegexFilter();

            // Act
            IRegexFilter result = sut.ExcludeAllParameters();

            // Assert
            Assert.Same(sut, result);
            Assert.Equal(1, sut.Rules.Count);
            RegexRule parameterRule = sut.ParameterRules.Single();
            Assert.True(parameterRule.Parameter.IsMatch(Guid.NewGuid().ToString()));
        }

        [Fact]
        public void ExcludeAll()
        {
            // Arrange
            var sut = new RegexFilter();

            // Act
            IRegexFilter result = sut.ExcludeAll();

            // Assert
            Assert.Same(sut, result);
            Assert.Equal(3, sut.Rules.Count);
            RegexRule typeRule = sut.TypeRules.Single();
            Assert.True(typeRule.Type.IsMatch(Guid.NewGuid().ToString()));
            RegexRule methodRule = sut.MethodRules.Single();
            Assert.True(methodRule.Method.IsMatch(Guid.NewGuid().ToString()));
            RegexRule parameterRule = sut.ParameterRules.Single();
            Assert.True(parameterRule.Parameter.IsMatch(Guid.NewGuid().ToString()));
        }

        #endregion ExcludeAll
    }
}
