namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AutoTest.ArgNullEx.Filter;
    using Moq;
    using global::Xunit;
    using global::Xunit.Extensions;

    public class ArgumentNullExceptionFixtureExtensionsShould
    {
        #region Clear/SetBindingFlag

        public static IEnumerable<object[]> AllBindingFlags
        {
            get { return GetAllBindingFlags(); }
        }

        private static IEnumerable<object[]> GetAllBindingFlags()
        {
            IEnumerable<BindingFlags> compositeBindingFlags =
                new[]
                    {
                        BindingFlags.CreateInstance | BindingFlags.DeclaredOnly,
                        BindingFlags.ExactBinding | BindingFlags.FlattenHierarchy,
                        BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.IgnoreCase,
                        BindingFlags.IgnoreReturn | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic,
                        BindingFlags.OptionalParamBinding | BindingFlags.Public | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty | BindingFlags.SetField
                    };
            return
                Enum.GetValues(typeof(BindingFlags))
                    .OfType<BindingFlags>()
                    .Union(compositeBindingFlags)
                    .Where(bf => bf != 0) // Remove the zero value enumeration.
                    .Select(bf => new object[] { bf });
        }

        [Theory, PropertyData("AllBindingFlags")]
        public void SetBindingFlag(
            BindingFlags mask)
        {
            // Arrange.
            IArgumentNullExceptionFixture sut = new ArgumentNullExceptionFixture(typeof(ArgumentNullExceptionFixtureExtensionsShould).Assembly);

            // Get the original value which should be preserved.
            BindingFlags original = sut.BindingFlags;

            Assert.Same(sut, sut.ClearBindingFlags(mask));
            Assert.False(sut.BindingFlags.HasFlag(mask), "The Binding flag should not be set after having been cleared.");
            Assert.Equal(original & ~mask, sut.BindingFlags);

            Assert.Same(sut, sut.SetBindingFlags(mask));
            Assert.True(sut.BindingFlags.HasFlag(mask), "The binding flag has not been set.");
            Assert.True(sut.BindingFlags.HasFlag(original), "The original binding flags have not been preserved.");
            Assert.Equal(original | mask, sut.BindingFlags);
        }

        [Theory, PropertyData("AllBindingFlags")]
        public void ClearBindingFlag(
            BindingFlags mask)
        {
            // Arrange.
            IArgumentNullExceptionFixture sut = new ArgumentNullExceptionFixture(typeof(ArgumentNullExceptionFixtureExtensionsShould).Assembly);

            // Get the original value which should be preserved with the exception of the cleared values.
            BindingFlags original = sut.BindingFlags;

            Assert.Same(sut, sut.SetBindingFlags(mask));
            Assert.True(sut.BindingFlags.HasFlag(mask), "The binding flag has not been set.");
            Assert.True(sut.BindingFlags.HasFlag(original), "The original binding flags have not been preserved.");
            Assert.Equal(original | mask, sut.BindingFlags);

            Assert.Same(sut, sut.ClearBindingFlags(mask));
            Assert.False(sut.BindingFlags.HasFlag(mask), "The Binding flag should not be set after having been cleared.");
            Assert.Equal(original & ~mask, sut.BindingFlags);
        }

        #endregion Clear/SetBindingFlag

        #region Add/Remove Filters

        [Theory, AutoMock]
        public void AddFilters(
            List<IFilter> filters,
            IFilter[] filtersToAdd,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            // Arrange
            fixtureMock.SetupGet(f => f.Filters).Returns(filters);
            List<IFilter> expectedFilters = filters.Union(filtersToAdd).ToList();

            // Act
            IArgumentNullExceptionFixture actual = fixtureMock.Object.AddFilters(filtersToAdd);

            // Asserts
            Assert.Same(fixtureMock.Object, actual);
            Assert.Equal(expectedFilters.Count, filters.Count);
            Assert.False(expectedFilters.Except(filters).Any());
        }

        [Theory, AutoMock]
        public void AddOnlyDistinctFilters(
            IFilter[] filtersToAdd,
            List<IFilter> filters,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            // Arrange
            fixtureMock.SetupGet(f => f.Filters).Returns(filters);
            List<IFilter> expectedFilters = filters.Union(filtersToAdd).ToList();

            // Act
            IArgumentNullExceptionFixture actual =
                fixtureMock.Object
                           .AddFilters(filtersToAdd)
                           .AddFilters(filtersToAdd)
                           .AddFilters(filters);

            // Asserts
            Assert.Same(fixtureMock.Object, actual);
            Assert.Equal(expectedFilters.Count, filters.Count);
            Assert.False(expectedFilters.Except(filters).Any());
        }

        [Theory, AutoMock]
        public void RemoveFilters(
            IFilter[] filtersToAdd,
            List<IFilter> filters,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            // Arrange
            fixtureMock.SetupGet(f => f.Filters).Returns(filters);
            IFilter[] originalFilters = filters.ToArray();

            // Act
            IArgumentNullExceptionFixture actual =
                fixtureMock.Object
                           .AddFilters(filtersToAdd)
                           .RemoveFilters(originalFilters);

            // Asserts
            Assert.Same(fixtureMock.Object, actual);
            Assert.Equal(filtersToAdd.Length, filters.Count);
            Assert.False(filtersToAdd.Except(filters).Any());
        }

        [Theory, AutoMock]
        public void OnlyRemovePresentFilters(
            IFilter[] filtersToAdd,
            List<IFilter> filters,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            // Arrange
            fixtureMock.SetupGet(f => f.Filters).Returns(filters);
            IFilter[] originalFilters = filters.ToArray();

            // Act
            IArgumentNullExceptionFixture actual =
                fixtureMock.Object
                           .AddFilters(filtersToAdd)
                           .RemoveFilters(originalFilters)
                           .RemoveFilters(originalFilters);

            // Asserts
            Assert.Same(fixtureMock.Object, actual);
            Assert.Equal(filtersToAdd.Length, filters.Count);
            Assert.False(filtersToAdd.Except(filters).Any());
        }

        [Theory, AutoMock]
        public void RemoveFiltersByType(
            IsClassOrStruct[] filtersToAdd,
            List<IFilter> filters,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            // Arrange
            fixtureMock.SetupGet(f => f.Filters).Returns(filters);
            IFilter[] originalFilters = filters.ToArray();

            // Act
            IArgumentNullExceptionFixture actual =
                fixtureMock.Object
                           .AddFilters(filtersToAdd)
                           .RemoveFilters(typeof(IsClassOrStruct));

            // Asserts
            Assert.Same(fixtureMock.Object, actual);
            Assert.Equal(originalFilters.Length, filters.Count);
            Assert.False(originalFilters.Except(filters).Any());
        }

        #endregion Add/Remove Filters

        #region Exclude/Incude Type

// ReSharper disable UnusedParameter.Local
        private void AssertTypeRule(
            Func<Type, IArgumentNullExceptionFixture> addMethod,
            IList<RegexRule> existingRules,
            IList<RegexRule> regexRules,
            bool expectedInclude)
        {
            IArgumentNullExceptionFixture actual = addMethod(GetType());

            Assert.Same(addMethod.Target, actual);
            Assert.Equal(existingRules.Count + 1, regexRules.Count);
            Assert.False(existingRules.Except(regexRules).Any());
            RegexRule addedRule = regexRules.Except(existingRules).Single();
            Assert.Equal(expectedInclude, addedRule.Include);
            Assert.NotNull(addedRule.Type);
            Assert.Null(addedRule.Method);
            Assert.Null(addedRule.Parameter);
            Assert.True(addedRule.MatchType(GetType()));
        }
// ReSharper restore UnusedParameter.Local

        [Theory, AutoMock]
        public void ExcludeAType(
            List<IFilter> filters,
            List<RegexRule> regexRules,
            Mock<IRegexFilter> regexFilterMock,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            // Arrange
            regexFilterMock.SetupGet(r => r.Rules).Returns(regexRules);
            filters.Add(regexFilterMock.Object);
            fixtureMock.SetupGet(f => f.Filters).Returns(filters);

            // Act/Assert
            AssertTypeRule(fixtureMock.Object.ExcludeType, regexRules.ToArray(), regexRules, expectedInclude: false);
        }

        [Theory, AutoMock]
        public void IncludeAType(
            List<IFilter> filters,
            List<RegexRule> regexRules,
            Mock<IRegexFilter> regexFilterMock,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            // Arrange
            regexFilterMock.SetupGet(r => r.Rules).Returns(regexRules);
            filters.Add(regexFilterMock.Object);
            fixtureMock.SetupGet(f => f.Filters).Returns(filters);

            // Act/Assert
            AssertTypeRule(fixtureMock.Object.IncludeType, regexRules.ToArray(), regexRules, expectedInclude: true);
        }

        [Theory, AutoMock]
        public void ThrowIfNoIRegexFilterWhenExcludingAType(
            List<IFilter> filters,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            // Arrange
            fixtureMock.SetupGet(f => f.Filters).Returns(filters);

            // Act/Assert
            Assert.Throws<InvalidOperationException>(() => fixtureMock.Object.ExcludeType(GetType()));
        }

        [Theory, AutoMock]
        public void ThrowIfMultipleIRegexFiltersWhenExcludingAType(
            List<IFilter> filters,
            List<IRegexFilter> regexFilters,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            // Arrange
            filters.AddRange(regexFilters);
            fixtureMock.SetupGet(f => f.Filters).Returns(filters);

            // Act/Assert
            Assert.Throws<InvalidOperationException>(() => fixtureMock.Object.ExcludeType(GetType()));
        }

        [Theory, AutoMock]
        public void ThrowIfNoIRegexFilterWhenIncludingAType(
            List<IFilter> filters,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            // Arrange
            fixtureMock.SetupGet(f => f.Filters).Returns(filters);

            // Act/Assert
            Assert.Throws<InvalidOperationException>(() => fixtureMock.Object.IncludeType(GetType()));
        }

        [Theory, AutoMock]
        public void ThrowIfMultipleIRegexFiltersWhenIncludingAType(
            List<IFilter> filters,
            List<IRegexFilter> regexFilters,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            // Arrange
            filters.AddRange(regexFilters);
            fixtureMock.SetupGet(f => f.Filters).Returns(filters);

            // Act/Assert
            Assert.Throws<InvalidOperationException>(() => fixtureMock.Object.IncludeType(GetType()));
        }

        #endregion Exclude/Incude Type

        #region Exclude/Incude Method

// ReSharper disable UnusedParameter.Local
        private void AssertMethodRule(
            Func<string, Type, IArgumentNullExceptionFixture> addMethod,
            MethodBase method,
            Type type,
            IList<RegexRule> existingRules,
            IList<RegexRule> regexRules,
            bool expectedInclude)
        {
            IArgumentNullExceptionFixture actual = addMethod(method.Name, type);

            Assert.Same(addMethod.Target, actual);
            Assert.Equal(existingRules.Count + 1, regexRules.Count);
            Assert.False(existingRules.Except(regexRules).Any());
            RegexRule addedRule = regexRules.Except(existingRules).Single();
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
        public void ExcludeAMethodWithType(
            Type type,
            Mock<MethodBase> methodMock,
            List<IFilter> filters,
            List<RegexRule> regexRules,
            Mock<IRegexFilter> regexFilterMock,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            // Arrange
            methodMock.SetupGet(m => m.Name).Returns("name" + Guid.NewGuid());
            regexFilterMock.SetupGet(r => r.Rules).Returns(regexRules);
            filters.Add(regexFilterMock.Object);
            fixtureMock.SetupGet(f => f.Filters).Returns(filters);

            // Act/Assert
            AssertMethodRule(fixtureMock.Object.ExcludeMethod, methodMock.Object, type, regexRules.ToArray(), regexRules, expectedInclude: false);
        }

        [Theory, AutoMock]
        public void ExcludeAMethodWithoutType(
            Mock<MethodBase> methodMock,
            List<IFilter> filters,
            List<RegexRule> regexRules,
            Mock<IRegexFilter> regexFilterMock,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            // Arrange
            methodMock.SetupGet(m => m.Name).Returns("name" + Guid.NewGuid());
            regexFilterMock.SetupGet(r => r.Rules).Returns(regexRules);
            filters.Add(regexFilterMock.Object);
            fixtureMock.SetupGet(f => f.Filters).Returns(filters);

            // Act/Assert
            AssertMethodRule(fixtureMock.Object.ExcludeMethod, methodMock.Object, null, regexRules.ToArray(), regexRules, expectedInclude: false);
        }

        [Theory, AutoMock]
        public void IncludeAMethodWithType(
            Type type,
            Mock<MethodBase> methodMock,
            List<IFilter> filters,
            List<RegexRule> regexRules,
            Mock<IRegexFilter> regexFilterMock,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            // Arrange
            methodMock.SetupGet(m => m.Name).Returns("name" + Guid.NewGuid());
            regexFilterMock.SetupGet(r => r.Rules).Returns(regexRules);
            filters.Add(regexFilterMock.Object);
            fixtureMock.SetupGet(f => f.Filters).Returns(filters);

            // Act/Assert
            AssertMethodRule(fixtureMock.Object.IncludeMethod, methodMock.Object, type, regexRules.ToArray(), regexRules, expectedInclude: true);
        }

        [Theory, AutoMock]
        public void IncludeAMethodWithoutType(
            Mock<MethodBase> methodMock,
            List<IFilter> filters,
            List<RegexRule> regexRules,
            Mock<IRegexFilter> regexFilterMock,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            // Arrange
            methodMock.SetupGet(m => m.Name).Returns("name" + Guid.NewGuid());
            regexFilterMock.SetupGet(r => r.Rules).Returns(regexRules);
            filters.Add(regexFilterMock.Object);
            fixtureMock.SetupGet(f => f.Filters).Returns(filters);

            // Act/Assert
            AssertMethodRule(fixtureMock.Object.IncludeMethod, methodMock.Object, null, regexRules.ToArray(), regexRules, expectedInclude: true);
        }

        [Theory, AutoMock]
        public void ThrowIfNoIRegexFilterWhenExcludingAMethod(
            string methodName,
            List<IFilter> filters,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            // Arrange
            fixtureMock.SetupGet(f => f.Filters).Returns(filters);

            // Act/Assert
            Assert.Throws<InvalidOperationException>(() => fixtureMock.Object.ExcludeMethod(methodName));
        }

        [Theory, AutoMock]
        public void ThrowIfMultipleIRegexFiltersWhenExcludingAMethod(
            string methodName,
            List<IFilter> filters,
            List<IRegexFilter> regexFilters,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            // Arrange
            filters.AddRange(regexFilters);
            fixtureMock.SetupGet(f => f.Filters).Returns(filters);

            // Act/Assert
            Assert.Throws<InvalidOperationException>(() => fixtureMock.Object.ExcludeMethod(methodName));
        }

        [Theory, AutoMock]
        public void ThrowIfNoIRegexFilterWhenIncludingAMethod(
            string methodName,
            List<IFilter> filters,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            // Arrange
            fixtureMock.SetupGet(f => f.Filters).Returns(filters);

            // Act/Assert
            Assert.Throws<InvalidOperationException>(() => fixtureMock.Object.IncludeMethod(methodName));
        }

        [Theory, AutoMock]
        public void ThrowIfMultipleIRegexFiltersWhenIncludingAMethod(
            string methodName,
            List<IFilter> filters,
            List<IRegexFilter> regexFilters,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            // Arrange
            filters.AddRange(regexFilters);
            fixtureMock.SetupGet(f => f.Filters).Returns(filters);

            // Act/Assert
            Assert.Throws<InvalidOperationException>(() => fixtureMock.Object.IncludeMethod(methodName));
        }

        #endregion Exclude/Incude Method
    }
}
