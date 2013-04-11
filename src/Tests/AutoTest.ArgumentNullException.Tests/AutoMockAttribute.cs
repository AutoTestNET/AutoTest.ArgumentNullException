namespace AutoTest.ArgNullEx
{
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit;

    public class AutoMockAttribute : AutoDataAttribute
    {
        public AutoMockAttribute()
            : base(CreateFixture(null))
        {
        }

        public AutoMockAttribute(ICustomization customization)
            : base(CreateFixture(customization))
        {
        }

        private static IFixture CreateFixture(ICustomization customization)
        {
            IFixture fixture = new Fixture().Customize(new AutoFixtureCustomizations());
            return customization == null ? fixture : fixture.Customize(customization);
        }
    }
}
