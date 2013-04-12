namespace AutoTest.ArgNullEx.TypeFilter
{
    using Xunit;
    using Xunit.Extensions;

    public class IsClassShould
    {
        [Theory, AutoMock]
        public void ReturnName(IsClass sut)
        {
            Assert.Equal("IsClass", sut.Name);
        }
    }
}
