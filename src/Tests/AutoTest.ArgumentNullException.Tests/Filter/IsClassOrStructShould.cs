namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::Xunit;
    using global::Xunit.Extensions;

    public class IsClassOrStructShould
    {
        [Theory, AutoMock]
        public void ReturnName(IsClassOrStruct sut)
        {
            Assert.Equal("IsClassOrStruct", sut.Name);
        }

        [Theory]
        [InlineData(typeof(int), false)]
        [InlineData(typeof(DateTime), false)]
        [InlineData(typeof(int?), false)]
        [InlineData(typeof(DateTime?), false)]
        [InlineData(typeof(string), false)]
        [InlineData(typeof(object), false)]
        [InlineData(typeof(UriFormat), true)]
        [InlineData(typeof(UriComponents), true)]
        public void IncludeOnlyClassesAndStructs(Type type, bool expected)
        {
            ITypeFilter sut = new IsClassOrStruct();

            // Act
            bool actual = sut.ExcludeType(type);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
