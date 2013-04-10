namespace AutoTest.ArgNullEx
{
    using System;
    using Xunit;
    using Xunit.Extensions;

    public class NullExtensionsShould
    {
        [Theory]
        [InlineData(typeof(int), false)]
        [InlineData(typeof(bool), false)]
        [InlineData(typeof(char), false)]
        [InlineData(typeof(Guid), false)]
        [InlineData(typeof(DateTime), false)]
        [InlineData(typeof(int?), true)]
        [InlineData(typeof(bool?), true)]
        [InlineData(typeof(char?), true)]
        [InlineData(typeof(Guid?), true)]
        [InlineData(typeof(DateTime?), true)]
        [InlineData(typeof(ArraySegment<int>), false)]
        [InlineData(typeof(ArraySegment<bool>), false)]
        [InlineData(typeof(ArraySegment<char>), false)]
        [InlineData(typeof(ArraySegment<Guid>), false)]
        [InlineData(typeof(ArraySegment<DateTime>), false)]
        [InlineData(typeof(object), true)]
        [InlineData(typeof(Uri), true)]
        [InlineData(typeof(string), true)]
        [InlineData(typeof(IDisposable), true)]
        [InlineData(typeof(Exception), true)]
        [InlineData(typeof(Tuple<int>), true)]
        public void IdentifyNullableTypes(Type type, bool expected)
        {
            // Act
            bool actual = type.IsNullable();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
