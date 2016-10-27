namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Moq;
    using global::Xunit;

    public class ParameterFilteringShould
    {
        [Theory, AutoMock]
        public void ApplyExclusionFilter(
            Mock<IParameterFilter> filterMock,
            Type type,
            Mock<MethodBase> methodMock,
            Mock<ParameterInfo> parameterMock)
        {
            // Arrange
            parameterMock.Setup(p => p.ToString()).Returns(typeof(ParameterInfo).ToString);

            parameterMock.SetupGet(p => p.Name)
                .Returns(Guid.NewGuid().ToString())
                .Verifiable();
            methodMock.SetupGet(m => m.Name)
                .Returns(Guid.NewGuid().ToString())
                .Verifiable();
            filterMock.Setup(f => f.ExcludeParameter(type, methodMock.Object, parameterMock.Object))
                .Returns(true)
                .Verifiable();
            filterMock.SetupGet(f => f.Name)
                      .Returns("mocked filter")
                      .Verifiable();

            // Act
            bool actual = filterMock.Object.ApplyFilter(type, methodMock.Object, parameterMock.Object);

            // Assert
            filterMock.Verify();
            parameterMock.Verify();
            methodMock.Verify();
            Assert.True(actual);
        }

        [Theory, AutoMock]
        public void ApplyIncludionFilter(
            Mock<IParameterFilter> filterMock,
            Type type,
            MethodBase method,
            Mock<ParameterInfo> parameterMock)
        {
            // Arrange
            parameterMock.Setup(p => p.ToString()).Returns(typeof(ParameterInfo).ToString);
            filterMock.Setup(f => f.ExcludeParameter(type, method, parameterMock.Object))
                .Returns(false)
                .Verifiable();

            // Act
            bool actual = filterMock.Object.ApplyFilter(type, method, parameterMock.Object);

            // Assert
            filterMock.Verify();
            Assert.False(actual);
        }
    }
}
