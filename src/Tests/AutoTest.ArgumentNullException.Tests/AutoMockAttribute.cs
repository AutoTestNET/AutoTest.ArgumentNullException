namespace AutoTest.ArgNullEx
{
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit;

    public class AutoMockAttribute : AutoDataAttribute
    {
        public AutoMockAttribute()
            : base(CreateFixture())
        {
        }

        private static IFixture CreateFixture()
        {
            return new Fixture().Customize(new AutoFixtureCustomizations());
        }
    }
}
