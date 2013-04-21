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
    }
}
