namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using Xunit;
    using Xunit.Extensions;

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

        public static IEnumerable<object[]> CompilerGeneratedTypes
        {
            get { return GetCompilerGeneratedTypes(); }
        }

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
        private static void TestNullableParams(
            int unusedInt,
            Guid? unusedNullableGuid,
            ArraySegment<bool> unusedArraySegment,
            IDisposable unusedDisposable)
        {
        }
// ReSharper restore UnusedParameter.Local
// ReSharper restore UnusedMember.Local

        private static IEnumerable<object[]> GetTestNullableParams()
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
            Guid? guidWithDefaultNull = null,
            string stringWithDefault = "default value",
            IDisposable disposableDefaultNull = null)
        {
        }
// ReSharper restore UnusedParameter.Local
// ReSharper restore UnusedMember.Local

        private static IEnumerable<object[]> GetTestNullDefaultParams()
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
                    new object[] {testParams[3], true},
                    new object[] {testParams[4], false},
                    new object[] {testParams[5], true}
                };
        }

        private static IEnumerable<object[]> GetCompilerGeneratedTypes()
        {
            return new[]
                {
                    new object[] {typeof(OuterCg), true},
                    new object[] {typeof(OuterCg.InnerCgOuterCg), true},
                    new object[] {typeof(OuterCg.InnerNoCgOuterCg), true},
                    new object[] {typeof(OuterNoCg), false},
                    new object[] {typeof(OuterNoCg.InnerCgOuterNoCg), true},
                    new object[] {typeof(OuterNoCg.InnerNoCgOuterNoCg), false},
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

        [Theory, PropertyData("CompilerGeneratedTypes")]
        public void IdentifyCompilerGeneratedTypes(Type type, bool expected)
        {
            // Act
            bool actual = type.IsCompilerGenerated();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
