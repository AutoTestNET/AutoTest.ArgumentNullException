namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Moq;
    using global::Xunit;
    using global::Xunit.Extensions;

    public class NotNullDefaultShould
    {
        [Theory, AutoMock]
        public void ReturnName(NotNullDefault sut)
        {
            Assert.Equal("NotNullDefault", sut.Name);
        }

        public static IEnumerable<object[]> NullDefaultParams
        {
            get { return NullExtensionsShould.GetTestNullDefaultParams(); }
        }

        [Theory, PropertyData("NullDefaultParams")]
        public void IdentifyNullDefault(ParameterInfo param, bool expected)
        {
            // Arrange
            IParameterFilter sut = new NotNullDefault();
            var methodBaseMock = new Mock<MethodBase>();

            // Act
            bool actual = sut.ExcludeParameter(GetType(), methodBaseMock.Object, param);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
