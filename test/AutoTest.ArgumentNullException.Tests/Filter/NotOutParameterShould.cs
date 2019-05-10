namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using global::Xunit;

    public class NotOutParameterShould
    {
        private static void SomeOutParametersMethod(
            int intInput,
            string stringInput,
            Guid guidInput,
            ref int intRef,
            ref string stringRef,
            ref Guid guidRef,
            out int intOutput,
            out string stringOutput,
            out Guid guidOutput)
        {
            intOutput = 0;
            stringOutput = string.Empty;
            guidOutput = Guid.Empty;
        }

        public static IEnumerable<object[]> SomeOutParameters => GetSomeOutParameters();

        internal static IEnumerable<object[]> GetSomeOutParameters()
        {
            Type type = typeof(NotOutParameterShould);
            MethodInfo method = type.GetMethod("SomeOutParametersMethod", BindingFlags.NonPublic | BindingFlags.Static);
            ParameterInfo[] someOutParameters = method.GetParameters();

            return someOutParameters.Select(parameter => new object[] { type, method, parameter, parameter.Name.Contains("Output") });
        }

        [Theory, AutoMock]
        public void ReturnName(NotOutParameter sut)
        {
            Assert.Equal("NotOutParameter", sut.Name);
        }

        [Theory, MemberData(nameof(SomeOutParameters))]
        public void ExcludOutParameter(Type type, MethodInfo method, ParameterInfo parameter, bool expected)
        {
            // Arrange
            IParameterFilter sut = new NotOutParameter();

            // Act
            bool actual = sut.ExcludeParameter(type, method, parameter);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
