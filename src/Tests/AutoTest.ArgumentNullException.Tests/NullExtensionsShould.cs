namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using global::Xunit;
    using global::Xunit.Extensions;

    public class NullExtensionsShould
    {
        [CompilerGenerated]
        public class OuterCg
        {
            [CompilerGenerated]
            public class InnerCgOuterCg
            {}

            public class InnerNoCgOuterCg
            { }
        }

        public class OuterNoCg
        {
            [CompilerGenerated]
            public class InnerCgOuterNoCg
            { }

            public class InnerNoCgOuterNoCg
            { }
        }

        public static IEnumerable<object[]> NullableParams
        {
            get { return GetTestNullableParams(); }
        }

        public static IEnumerable<object[]> NullDefaultParams
        {
            get { return GetTestNullDefaultParams(); }
        }

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
        private static void TestNullableParams(
            int nonNullableInt,
            Guid? nullableGuid,
            ArraySegment<bool> nonNullableArraySegment,
            IDisposable nullableDisposable)
        {
        }
// ReSharper restore UnusedParameter.Local
// ReSharper restore UnusedMember.Local

        internal static IEnumerable<object[]> GetTestNullableParams()
        {
            ParameterInfo[] testParams =
                typeof(NullExtensionsShould).GetMethod("TestNullableParams",
                                                        BindingFlags.NonPublic | BindingFlags.Static)
                                            .GetParameters();

            return new []
                {
                    new object[] {testParams[0], false},
                    new object[] {testParams[1], true},
                    new object[] {testParams[2], false},
                    new object[] {testParams[3], true}
                };
        }

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
        private static void TestNullDefaultParams(
            int intWithoutDefault,
            string stringWithoutDefault,
            int intWithDefault = 10,
            int? intWithNonNullDefault = 10,
            Guid? guidWithDefaultNull = null,
            string stringWithDefault = "default value",
            IDisposable disposableDefaultNull = null)
        {
        }
// ReSharper restore UnusedParameter.Local
// ReSharper restore UnusedMember.Local

        internal static IEnumerable<object[]> GetTestNullDefaultParams()
        {
            ParameterInfo[] testParams =
                typeof(NullExtensionsShould).GetMethod("TestNullDefaultParams",
                                                        BindingFlags.NonPublic | BindingFlags.Static)
                                            .GetParameters();

            return new[]
                {
                    new object[] {testParams[0], false},
                    new object[] {testParams[1], false},
                    new object[] {testParams[2], false},
                    new object[] {testParams[3], false},
                    new object[] {testParams[4], true},
                    new object[] {testParams[5], false},
                    new object[] {testParams[6], true}
                };
        }

        [Theory]
        [InlineData(typeof(int), false)]
        [InlineData(typeof(bool), false)]
        [InlineData(typeof(char), false)]
        [InlineData(typeof(Guid), false)]
        [InlineData(typeof(DateTime), false)]
        [InlineData(typeof(int?), true)]
        [InlineData(typeof(bool?), true)]
        [InlineData(typeof(char?), true)]
        [InlineData(typeof(Guid?), true)]
        [InlineData(typeof(DateTime?), true)]
        [InlineData(typeof(ArraySegment<int>), false)]
        [InlineData(typeof(ArraySegment<bool>), false)]
        [InlineData(typeof(ArraySegment<char>), false)]
        [InlineData(typeof(ArraySegment<Guid>), false)]
        [InlineData(typeof(ArraySegment<DateTime>), false)]
        [InlineData(typeof(object), true)]
        [InlineData(typeof(Uri), true)]
        [InlineData(typeof(string), true)]
        [InlineData(typeof(IDisposable), true)]
        [InlineData(typeof(Exception), true)]
        [InlineData(typeof(Tuple<int>), true)]
        public void IdentifyNullableTypes(Type type, bool expected)
        {
            // Act
            bool actual = type.IsNullable();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory, PropertyData("NullableParams")]
        public void IdentifyNullableParameters(ParameterInfo param, bool expected)
        {
            // Act
            bool actual = param.IsNullable();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory, PropertyData("NullDefaultParams")]
        public void IdentifyNullDefault(ParameterInfo param, bool expected)
        {
            // Act
            bool actual = param.HasNullDefault();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(typeof(OuterCg), true)]
        [InlineData(typeof(OuterCg.InnerCgOuterCg), true)]
        [InlineData(typeof(OuterCg.InnerNoCgOuterCg), true)]
        [InlineData(typeof(OuterNoCg), false)]
        [InlineData(typeof(OuterNoCg.InnerCgOuterNoCg), true)]
        [InlineData(typeof(OuterNoCg.InnerNoCgOuterNoCg), false)]
        public void IdentifyCompilerGeneratedTypes(Type type, bool expected)
        {
            // Act
            bool actual = type.IsCompilerGenerated();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
