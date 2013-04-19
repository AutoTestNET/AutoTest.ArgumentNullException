namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using global::Xunit;
    using global::Xunit.Extensions;

    public class ArgumentNullExceptionFixtureExtensionsShould
    {
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
            var sut = new ArgumentNullExceptionFixture(typeof(ArgumentNullExceptionFixtureExtensionsShould).Assembly);

            // Get the original value which should be preserved.
            BindingFlags original = sut.BindingFlags;

            Assert.Equal(sut.BindingFlags, sut.ClearBindingFlags(mask));
            Assert.False(sut.BindingFlags.HasFlag(mask), "The Binding flag should not be set after having been cleared.");
            Assert.Equal(original & ~mask, sut.BindingFlags);

            Assert.Equal(sut.BindingFlags, sut.SetBindingFlags(mask));
            Assert.True(sut.BindingFlags.HasFlag(mask), "The binding flag has not been set.");
            Assert.True(sut.BindingFlags.HasFlag(original), "The original binding flags have not been preserved.");
            Assert.Equal(original | mask, sut.BindingFlags);
        }

        [Theory, PropertyData("AllBindingFlags")]
        public void ClearBindingFlag(
            BindingFlags mask)
        {
            // Arrange.
            var sut = new ArgumentNullExceptionFixture(typeof(ArgumentNullExceptionFixtureExtensionsShould).Assembly);

            // Get the original value which should be preserved with the exception of the cleared values.
            BindingFlags original = sut.BindingFlags;

            Assert.Equal(sut.BindingFlags, sut.SetBindingFlags(mask));
            Assert.True(sut.BindingFlags.HasFlag(mask), "The binding flag has not been set.");
            Assert.True(sut.BindingFlags.HasFlag(original), "The original binding flags have not been preserved.");
            Assert.Equal(original | mask, sut.BindingFlags);

            Assert.Equal(sut.BindingFlags, sut.ClearBindingFlags(mask));
            Assert.False(sut.BindingFlags.HasFlag(mask), "The Binding flag should not be set after having been cleared.");
            Assert.Equal(original & ~mask, sut.BindingFlags);
        }
    }
}
