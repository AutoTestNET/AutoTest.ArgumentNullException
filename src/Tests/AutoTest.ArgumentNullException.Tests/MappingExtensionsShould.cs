namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoTest.ArgNullEx.Mapping;
    using Moq;
    using global::Xunit;

    public class MappingExtensionsShould
    {
        [Theory, AutoMock]
        public void ThrowInvalidOperationExceptionIfNoSubstituteTypeMapping(
            Type originalType,
            Type newType,
            List<IMapping> mapings,
            Mock<IArgumentNullExceptionFixture> fixtureMock)
        {
            // Arrange
            fixtureMock.SetupGet(f => f.Mappings).Returns(mapings);

            // Act//Assert
            Assert.Throws<InvalidOperationException>(() => fixtureMock.Object.SubstituteType(originalType, newType));
        }

        [Theory, AutoMock]
        public void ThrowInvalidOperationExceptionIfDuplicateSubstitutions(
            Type originalType,
            Type newType1,
            Type newType2,
            List<IMapping> mapings,
            ArgumentNullExceptionFixture fixture)
        {
            // Arrange
            fixture.SubstituteType(originalType, newType1);

            // Act//Assert
            Assert.Throws<InvalidOperationException>(() => fixture.SubstituteType(originalType, newType2));
        }
    }
}
