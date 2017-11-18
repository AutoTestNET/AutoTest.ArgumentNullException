namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Moq;
    using global::Xunit;

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

        [Theory, MemberData(nameof(NullableParams))]
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
