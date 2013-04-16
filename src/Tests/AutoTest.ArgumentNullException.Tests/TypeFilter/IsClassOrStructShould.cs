namespace AutoTest.ArgNullEx.TypeFilter
{
    using System;
    using Xunit;
    using Xunit.Extensions;

    public class IsClassOrStructShould
    {
        [Theory, AutoMock]
        public void ReturnName(IsClassOrStruct sut)
        {
            Assert.Equal("IsClassOrStruct", sut.Name);
        }

        [Theory]
        [InlineData(typeof(int), true)]
        [InlineData(typeof(DateTime), true)]
        [InlineData(typeof(int?), true)]
        [InlineData(typeof(DateTime?), true)]
        [InlineData(typeof(string), true)]
        [InlineData(typeof(object), true)]
        [InlineData(typeof(UriFormat), false)]
        [InlineData(typeof(UriComponents), false)]
        public void IncludeOnlyClassesAndStructs(Type type, bool expected)
        {
            ITypeFilter sut = new IsClassOrStruct();

            // Act
            bool actual = sut.IncludeType(type);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
