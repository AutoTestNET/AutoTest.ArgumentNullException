namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using global::Xunit;

    public class NotNullableValueTypeShould
    {
        private static void SomeNullableParametersMethod(
            int intInput,
            string stringInput,
            Guid guidInput,
            int? intNullable,
            Guid? guidNullable)
        {
        }

        public static IEnumerable<object[]> SomeOutParameters => GetSomeOutParameters();

        internal static IEnumerable<object[]> GetSomeOutParameters()
        {
            Type type = typeof(NotNullableValueTypeShould);

            MethodInfo method = type.GetMethod(
                nameof(SomeNullableParametersMethod),
                BindingFlags.NonPublic | BindingFlags.Static);
            ParameterInfo[] someOutParameters = method.GetParameters();

            return someOutParameters.Select(
                parameter => new object[] { type, method, parameter, parameter.Name.Contains("Nullable") });
        }

        [Theory, AutoMock]
        public void ReturnName(NotNullableValueType sut)
        {
            Assert.Equal(nameof(NotNullableValueType), sut.Name);
        }

        [Theory, MemberData(nameof(SomeOutParameters))]
        public void ExcludeNullableValueTypeParameter(
            Type type,
            MethodInfo method,
            ParameterInfo parameter,
            bool expected)
        {
            // Arrange
            IParameterFilter sut = new NotNullableValueType();

            // Act
            bool actual = sut.ExcludeParameter(type, method, parameter);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
