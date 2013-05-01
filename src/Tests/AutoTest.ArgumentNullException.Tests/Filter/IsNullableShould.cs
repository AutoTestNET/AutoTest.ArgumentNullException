namespace AutoTest.ArgNullEx.Filter
{
    using System.Collections.Generic;
    using System.Reflection;
    using Moq;
    using global::Xunit;
    using global::Xunit.Extensions;

    public class IsNullableShould
    {
        [Theory, AutoMock]
        public void ReturnName(IsNullable sut)
        {
            Assert.Equal("IsNullable", sut.Name);
        }

        public static IEnumerable<object[]> NullableParams
        {
            get { return NullExtensionsShould.GetTestNullableParams(); }
        }

        [Theory, PropertyData("NullableParams")]
        public void IncludeNullableParameters(ParameterInfo param, bool include)
        {
            // Arrange
            IParameterFilter sut = new IsNullable();
            var methodBaseMock = new Mock<MethodBase>();

            // Act
            bool actualExclude = sut.ExcludeParameter(GetType(), methodBaseMock.Object, param);

            // Assert
            Assert.Equal(!include, actualExclude);
        }
    }
}
