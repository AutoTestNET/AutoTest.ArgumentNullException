namespace AutoTest.ArgumentNullException
{
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    public class AutoFixtureCustomizations : CompositeCustomization
    {
        public AutoFixtureCustomizations()
            : base(new AutoMoqCustomization())
        {
        }
    }
}
