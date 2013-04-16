namespace AutoTest.ExampleLibrary.Tests
{
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;
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
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            return customization == null ? fixture : fixture.Customize(customization);
        }
    }
}
