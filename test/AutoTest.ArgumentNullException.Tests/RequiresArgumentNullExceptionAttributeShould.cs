namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AutoFixture.Xunit2;
    using AutoTest.ArgNullEx.Xunit;
    using Moq;
    using global::Xunit;

    public class RequiresArgumentNullExceptionAttributeShould
    {
        private class CustomAttribute : RequiresArgumentNullExceptionAttribute
        {
            public CustomAttribute(IArgumentNullExceptionFixture fixture)
                : base(fixture)
            {
            }
        }

        [Fact]
        public void InitialiseFixtureFromType()
        {
            // Act
            var sut = new RequiresArgumentNullExceptionAttribute(typeof(Uri));

            // Assert
            Assert.NotNull(sut.Fixture);
            ArgumentNullExceptionFixture fixture = Assert.IsType<ArgumentNullExceptionFixture>(sut.Fixture);
            Assert.Same(typeof(Uri).GetTypeInfo().Assembly, fixture.AssemblyUnderTest);
        }

        [Theory, AutoMock]
        private void InitializeWithInjectedFixture(
            [Frozen] IArgumentNullExceptionFixture fixture,
            CustomAttribute sut)
        {
            // Assert
            Assert.Same(fixture, sut.Fixture);
        }

        [Theory, AutoMock]
        private void ReturnsEnumerationOfMethodData(
            [Frozen] Mock<IArgumentNullExceptionFixture> fixtureMock,
            ICollection<MethodData> expectedData,
            MethodInfo unused1,
            CustomAttribute sut)
        {
            // Arrange
            fixtureMock.Setup(f => f.GetData()).Returns(expectedData);

            // Act
            List<MethodData> actualData =
                sut.GetData(unused1)
                   .SelectMany(d => d.OfType<MethodData>())
                   .ToList();

            // Assert
            Assert.Equal(expectedData.Count, actualData.Count);
            Assert.False(expectedData.Except(actualData).Any());
        }
    }
}
