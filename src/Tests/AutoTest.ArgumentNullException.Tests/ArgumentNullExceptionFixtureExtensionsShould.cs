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
            BindingFlags bindingFlag)
        {
            // Arrange.
            var sut = new ArgumentNullExceptionFixture(typeof(ArgumentNullExceptionFixtureExtensionsShould).Assembly);

            sut.ClearBindingFlags(bindingFlag);
            Assert.False(sut.BindingFlags.HasFlag(bindingFlag), "The Binding flag should not be set after having been cleared.");
            sut.SetBindingFlags(bindingFlag);
            Assert.True(sut.BindingFlags.HasFlag(bindingFlag), "The binding flag has not been set.");
        }

        [Theory, PropertyData("AllBindingFlags")]
        public void ClearBindingFlag(
            BindingFlags bindingFlag)
        {
            // Arrange.
            var sut = new ArgumentNullExceptionFixture(typeof(ArgumentNullExceptionFixtureExtensionsShould).Assembly);

            sut.SetBindingFlags(bindingFlag);
            Assert.True(sut.BindingFlags.HasFlag(bindingFlag), "The binding flag has not been set.");
            sut.ClearBindingFlags(bindingFlag);
            Assert.False(sut.BindingFlags.HasFlag(bindingFlag), "The Binding flag should not be set after having been cleared.");
        }
    }
}
