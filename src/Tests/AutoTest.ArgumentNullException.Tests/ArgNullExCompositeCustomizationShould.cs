namespace AutoTest.ArgNullEx
{
    using System.Collections.Generic;
    using System.Linq;
    using global::Xunit;
    using global::Xunit.Extensions;

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
            List<DelegatingCustomization> customizations)
        {
            // Arrange
            var verifications = new List<bool>();
            customizations.ForEach(c => c.OnCustomize = f => verifications.Add(Equals(fixture, f)));
            var sut = new ArgNullExCompositeCustomization(customizations);

            // Act
            ((IArgNullExCustomization)sut).Customize(fixture);

            // Verify outcome
            Assert.NotEmpty(verifications);
            Assert.Equal(customizations.Count, verifications.Count);
            Assert.True(verifications.All(b => b));
        }
    }
}
