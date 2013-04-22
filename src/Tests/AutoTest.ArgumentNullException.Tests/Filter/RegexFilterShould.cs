namespace AutoTest.ArgNullEx.Filter
{
    using global::Xunit;
    using global::Xunit.Extensions;

    public class RegexFilterShould
    {
        [Theory, AutoMock]
        public void ReturnName(RegexFilter sut)
        {
            Assert.Equal("RegexFilter", sut.Name);
        }
    }
}
