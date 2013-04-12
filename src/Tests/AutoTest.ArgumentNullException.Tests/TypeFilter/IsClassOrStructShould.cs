namespace AutoTest.ArgNullEx.TypeFilter
{
    using Xunit;
    using Xunit.Extensions;

    public class IsClassOrStructShould
    {
        [Theory, AutoMock]
        public void ReturnName(IsClassOrStruct sut)
        {
            Assert.Equal("IsClassOrStruct", sut.Name);
        }
    }
}
