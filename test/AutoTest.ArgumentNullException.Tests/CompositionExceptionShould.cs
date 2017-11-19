namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AutoFixture.Xunit2;
    using Moq;
    using global::Xunit;

    public class CompositionExceptionShould
    {
        [Theory, AutoMock]
        public void RetainInnerException(
            [Frozen]Exception expected,
            CompositionException sut)
        {
            Assert.Same(expected, sut.InnerException);
        }

        [Theory, AutoMock]
        public void ComposeMessageFromParameters(
            Type classUnderTest,
            MethodBase methodUnderTest,
            string nullParameter,
            Exception innerException)
        {
            // Act
            var sut = new CompositionException(classUnderTest, methodUnderTest, nullParameter, innerException);

            // Assert
            Assert.Contains(classUnderTest.Name, sut.Message);
            Assert.Contains(methodUnderTest.Name, sut.Message);
            Assert.Contains(nullParameter, sut.Message);
            Assert.Contains(innerException.ToString(), sut.Message);
        }
    }
}
