namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Moq;
    using global::Xunit;
    using global::Xunit.Extensions;

    public class SpecimenProviderShould
    {
        [Theory, AutoMock]
        internal void ThrowWhenNoParameters(SpecimenProvider sut)
        {
            // AAA
            string actualParamName =
                Assert.Throws<ArgumentException>(
                    () => ((ISpecimenProvider) sut).GetParameterSpecimens(new ParameterInfo[] {}, 0))
                      .ParamName;
            Assert.Equal("parameters", actualParamName);
        }

        [Theory, AutoMock]
        internal void ThrowWhenNullIndexOutOfBounds(
            Mock<ParameterInfo>[] parameterMocks,
            SpecimenProvider sut)
        {
            // Arrange
            List<ParameterInfo> parameters = parameterMocks.Select(pm => pm.Object).ToList();

            // Act/Assert
            string actualParamName =
                Assert.Throws<ArgumentException>(
                    () => ((ISpecimenProvider)sut).GetParameterSpecimens(parameters, parameters.Count))
                      .ParamName;
            Assert.Equal("nullIndex", actualParamName);
        }

        [Theory, AutoMock]
        internal void ReturnSingleNullObjectArrayForSingleParameter(SpecimenProvider sut)
        {
            // Act
            object[] actualParameters = ((ISpecimenProvider) sut).GetParameterSpecimens(new ParameterInfo[1], 0);

            Assert.NotNull(actualParameters);
            Assert.Equal(1, actualParameters.Length);
            Assert.Null(actualParameters[0]);
        }
    }
}
