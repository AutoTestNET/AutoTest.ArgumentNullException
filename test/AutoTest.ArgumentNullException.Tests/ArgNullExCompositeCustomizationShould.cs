namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Moq;
    using global::Xunit;

    public class ArgNullExArgNullExCompositeCustomizationShould
    {
        [Theory, AutoMock]
        public void BeACustomization(ArgNullExCompositeCustomization sut)
        {
            // AAA
            Assert.IsAssignableFrom<IArgNullExCustomization>(sut);
        }

        [Theory, AutoMock]
        public void InitializeCustomizationsWithArray(
            IArgNullExCustomization[] customizations)
        {
            // Act
            var sut = new ArgNullExCompositeCustomization(customizations);

            // Assert
            Assert.NotNull(sut.Customizations);
            Assert.NotEmpty(sut.Customizations);
            Assert.Equal(customizations.Length, sut.Customizations.Count());
            Assert.False(customizations.Except(sut.Customizations).Any());
        }

        [Theory, AutoMock]
        public void InitializeCustomizationsWithEnumerable(
            IArgNullExCustomization[] customizations)
        {
            // Act
            var sut = new ArgNullExCompositeCustomization(customizations.AsEnumerable());

            // Assert
            Assert.NotNull(sut.Customizations);
            Assert.NotEmpty(sut.Customizations);
            Assert.Equal(customizations.Length, sut.Customizations.Count());
            Assert.False(customizations.Except(sut.Customizations).Any());
        }

        [Theory, AutoMock]
        internal void ApplyAllCustomizations(
            IArgumentNullExceptionFixture fixture,
            List<Mock<IArgNullExCustomization>> customizations)
        {
            // Arrange
            customizations.ForEach(mock => mock.Setup(c => c.Customize(fixture)).Verifiable());
            var sut = new ArgNullExCompositeCustomization(customizations.Select(mock => mock.Object));

            // Act
            ((IArgNullExCustomization)sut).Customize(fixture);

            // Assert
            Assert.NotEmpty(sut.Customizations);
            Assert.Equal(customizations.Count, sut.Customizations.Count());
            customizations.ForEach(mock => mock.Verify());
        }
    }
}
