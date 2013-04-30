namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Reflection;
    using global::Xunit;
    using global::Xunit.Extensions;

    public class HasNullableParametersWithoutNullDefaultsShould
    {
        [Theory, AutoMock]
        public void ReturnName(HasNullableParametersWithoutNullDefaults sut)
        {
            Assert.Equal("HasNullableParametersWithoutNullDefaults", sut.Name);
        }

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
        private static void NoParameters()
        {
        }

        private static void NoNullableParameters(int intParam, Guid guidParam, bool boolParam)
        {
        }

        private static void NullDefaultParameters(int intParam, string stringParam = null, bool? nulableBoolParam = null)
        {
        }

        private static void SomeNullableParameters(int intParam, string stringParam, bool boolParam = false)
        {
        }

        private static void NonNullDefaultParameters(int intParam, string stringParam = null, bool? nulableBoolParam = true)
        {
        }
// ReSharper restore UnusedParameter.Local
// ReSharper restore UnusedMember.Local

        [Theory]
        [InlineData("NoParameters", true)]
        [InlineData("NoNullableParameters", true)]
        [InlineData("NullDefaultParameters", true)]
        [InlineData("SomeNullableParameters", false)]
        [InlineData("NonNullDefaultParameters", false)]
        public void CorrectlyExcludeMethods(string methodName, bool include)
        {
            // Arrange
            IMethodFilter sut = new HasNullableParametersWithoutNullDefaults();
            Type type = GetType();
            MethodInfo method = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);

            // Act
            bool actual = sut.ExcludeMethod(type, method);

            // Assert
            Assert.Equal(include, actual);
        }
    }
}
