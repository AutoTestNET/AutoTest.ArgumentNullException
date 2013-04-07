namespace AutoTest.ArgNullEx
{
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit;

    public class AutoMockAttribute : AutoDataAttribute
    {
        public AutoMockAttribute()
            : base(new Fixture().Customize(new AutoFixtureCustomizations()))
        {
        }
    }
}
