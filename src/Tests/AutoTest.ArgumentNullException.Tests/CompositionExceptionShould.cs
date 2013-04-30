namespace AutoTest.ArgNullEx
{
    using System;
    using System.Reflection;
    using Moq;
    using Ploeh.AutoFixture.Xunit;
    using global::Xunit;
    using global::Xunit.Extensions;

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
            Mock<MethodBase> methodUnderTestMock,
            string nullParameter,
            Exception innerException)
        {
            // Arrange
            methodUnderTestMock.SetupGet(m => m.Name).Returns("Name" + Guid.NewGuid());

            // Act
            var sut = new CompositionException(classUnderTest, methodUnderTestMock.Object, nullParameter, innerException);

            // Assert
            Assert.Contains(classUnderTest.Name, sut.Message);
            Assert.Contains(methodUnderTestMock.Object.Name, sut.Message);
            Assert.Contains(nullParameter, sut.Message);
            Assert.Contains(innerException.ToString(), sut.Message);
        }
    }
}
