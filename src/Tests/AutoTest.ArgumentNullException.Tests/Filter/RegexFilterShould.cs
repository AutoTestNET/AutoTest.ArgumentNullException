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

    public class RegexFilterShould
    {
        #region Rule types

        private static List<RegexRule> TypeRules
        {
            get
            {
                return new List<RegexRule>
                    {
                        new RegexRule("Type rule 1", include: true, type: new Regex(".*")),
                        new RegexRule("Type rule 2", include: false, type: new Regex(".*")),
                        new RegexRule("Type rule 3", include: true, type: new Regex(".*")),
                    };
            }
        }

        private static List<RegexRule> MethodRules
        {
            get
            {
                return new List<RegexRule>
                    {
                        new RegexRule("Method rule 1", include: true, type: new Regex(".*"), method: new Regex(".*")),
                        new RegexRule("Method rule 2", include: false, method: new Regex(".*")),
                        new RegexRule("Method rule 3", include: true, type: new Regex(".*"), method: new Regex(".*")),
                        new RegexRule("Method rule 4", include: false, type: new Regex(".*"), method: new Regex(".*")),
                        new RegexRule("Method rule 5", include: true, method: new Regex(".*")),
                        new RegexRule("Method rule 6", include: false, type: new Regex(".*"), method: new Regex(".*")),
                    };
            }
        }

        private static List<RegexRule> ParameterRules
        {
            get
            {
                return new List<RegexRule>
                    {
                        new RegexRule("Parameter rule 1", include: true, parameter: new Regex(".*")),
                        new RegexRule("Parameter rule 2", include: false, type: new Regex(".*"), parameter: new Regex(".*")),
                        new RegexRule("Parameter rule 3", include: true, method: new Regex(".*"), parameter: new Regex(".*")),
                        new RegexRule("Parameter rule 4", include: false, type: new Regex(".*"), method: new Regex(".*"), parameter: new Regex(".*")),
                    };
            }
        }


        public static IEnumerable<object[]> AllRuleTypes
        {
            get
            {
                return new[]
                    {
                        new object[] {TypeRules, MethodRules, ParameterRules}
                    };
            }
        }

        [Theory, AutoMock]
        public void ReturnName(RegexFilter sut)
        {
            Assert.Equal("RegexFilter", sut.Name);
        }

        [Theory, PropertyData("AllRuleTypes")]
        public void ReturnTypeRules(
            List<RegexRule> typeRules,
            List<RegexRule> methodRules,
            List<RegexRule> parameterRules)
        {
            // Arrange
            var sut = new RegexFilter();
            sut.Rules.AddRange(parameterRules.Concat(methodRules).Concat(typeRules));

            // Act
            List<RegexRule> actualTypeRules = sut.TypeRules.ToList();
            List<RegexRule> actualIncludeTypeRules = sut.IncludeTypeRules.ToList();
            List<RegexRule> actualExcludeTypeRules = sut.ExcludeTypeRules.ToList();

            // Assert
            Assert.Equal(typeRules.Count, actualTypeRules.Count);
            Assert.Equal(typeRules.Count(r => r.Include), actualIncludeTypeRules.Count);
            Assert.Equal(typeRules.Count(r => !r.Include), actualExcludeTypeRules.Count);
            Assert.False(typeRules.Except(actualTypeRules).Any());
            Assert.False(typeRules.Where(r => r.Include).Except(actualIncludeTypeRules).Any());
            Assert.False(typeRules.Where(r => !r.Include).Except(actualExcludeTypeRules).Any());
        }

        [Theory, PropertyData("AllRuleTypes")]
        public void ReturnMethodRules(
            List<RegexRule> typeRules,
            List<RegexRule> methodRules,
            List<RegexRule> parameterRules)
        {
            // Arrange
            var sut = new RegexFilter();
            sut.Rules.AddRange(parameterRules.Concat(methodRules).Concat(typeRules));

            // Act
            List<RegexRule> actualMethodRules = sut.MethodRules.ToList();
            List<RegexRule> actualIncludeMethodRules = sut.IncludeMethodRules.ToList();
            List<RegexRule> actualExcludeMethodRules = sut.ExcludeMethodRules.ToList();

            // Assert
            Assert.Equal(methodRules.Count, actualMethodRules.Count);
            Assert.Equal(methodRules.Count(r => r.Include), actualIncludeMethodRules.Count);
            Assert.Equal(methodRules.Count(r => !r.Include), actualExcludeMethodRules.Count);
            Assert.False(methodRules.Except(actualMethodRules).Any());
            Assert.False(methodRules.Where(r => r.Include).Except(actualIncludeMethodRules).Any());
            Assert.False(methodRules.Where(r => !r.Include).Except(actualExcludeMethodRules).Any());
        }

        [Theory, PropertyData("AllRuleTypes")]
        public void ReturnParameterRules(
            List<RegexRule> typeRules,
            List<RegexRule> methodRules,
            List<RegexRule> parameterRules)
        {
            // Arrange
            var sut = new RegexFilter();
            sut.Rules.AddRange(parameterRules.Concat(methodRules).Concat(typeRules));

            // Act
            List<RegexRule> actualRules = sut.ParameterRules.ToList();

            // Assert
            Assert.Equal(parameterRules.Count, actualRules.Count);
            Assert.False(parameterRules.Except(actualRules).Any());
        }

        #endregion Rule types

        #region ITypeFilter

        [Theory, AutoMock]
        public void ExcudeType(
            IEnumerable<RegexRule> otherRules,
            RegexFilter sut)
        {
            // Arrange
            sut.Rules.AddRange(otherRules);
            sut.ExcludeType(GetType());

            // Act
            bool actual = ((ITypeFilter) sut).IncludeType(GetType());

            // Assert
            Assert.False(actual);
        }

        [Theory, AutoMock]
        public void EnsureIncludeTypeTakesPrecedenceOverExcudeType(
            IEnumerable<RegexRule> otherRules,
            RegexFilter sut)
        {
            // Arrange
            sut.Rules.AddRange(otherRules);
            sut.ExcludeType(GetType())
               .IncludeType(GetType());

            // Act
            bool actual = ((ITypeFilter)sut).IncludeType(GetType());

            // Assert
            Assert.True(actual);
        }

        [Theory, AutoMock]
        public void IncludeTypeWhenNoTypeRules(RegexFilter sut)
        {
            // Arrange
            Assert.Empty(sut.Rules);

            // Act
            bool actual = ((ITypeFilter)sut).IncludeType(GetType());

            // Assert
            Assert.True(actual);
        }

        #endregion ITypeFilter

        #region IMethodFilter

        [Theory, AutoMock]
        public void ExcudeMethod(
            Type type,
            Mock<MethodBase> methodMock,
            string methodName,
            IEnumerable<RegexRule> otherRules,
            RegexFilter sut)
        {
            // Arrange
            methodMock.SetupGet(m => m.Name).Returns(methodName);
            sut.Rules.AddRange(otherRules);
            sut.ExcludeMethod(methodName, type);

            // Act
            bool actual = ((IMethodFilter)sut).IncludeMethod(type, methodMock.Object);

            // Assert
            Assert.False(actual);
        }

        [Theory, AutoMock]
        public void ExcudeMethodWithNoType(
            Type type,
            Mock<MethodBase> methodMock,
            string methodName,
            IEnumerable<RegexRule> otherRules,
            RegexFilter sut)
        {
            // Arrange
            methodMock.SetupGet(m => m.Name).Returns(methodName);
            sut.Rules.AddRange(otherRules);
            sut.ExcludeMethod(methodName);

            // Act
            bool actual = ((IMethodFilter)sut).IncludeMethod(type, methodMock.Object);

            // Assert
            Assert.False(actual);
        }

        [Theory, AutoMock]
        public void EnsureIncludeMethodTakesPrecedenceOverExcudeMethod(
            Type type,
            Mock<MethodBase> methodMock,
            string methodName,
            IEnumerable<RegexRule> otherRules,
            RegexFilter sut)
        {
            // Arrange
            methodMock.SetupGet(m => m.Name).Returns(methodName);
            sut.Rules.AddRange(otherRules);
            sut.ExcludeMethod(methodName, type)
               .IncludeMethod(methodName);

            // Act
            bool actual = ((IMethodFilter)sut).IncludeMethod(type, methodMock.Object);

            // Assert
            Assert.True(actual);
        }

        [Theory, AutoMock]
        public void IncludeMethodWhenNoMethodRules(
            Type type,
            Mock<MethodBase> methodMock,
            RegexFilter sut)
        {
            // Arrange
            Assert.Empty(sut.Rules);

            // Act
            bool actual = ((IMethodFilter)sut).IncludeMethod(type, methodMock.Object);

            // Assert
            Assert.True(actual);
        }

        #endregion IMethodFilter
    }
}
