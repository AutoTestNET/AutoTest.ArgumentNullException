namespace AutoTest.ArgNullEx.TypeFilter
{
    using System;
    using Xunit;
    using Xunit.Extensions;

    public class NotCompilerGeneratedShould
    {
        [Theory, AutoMock]
        public void ReturnName(NotCompilerGenerated sut)
        {
            Assert.Equal("NotCompilerGenerated", sut.Name);
        }

        [Theory]
        [InlineData(typeof(NullExtensionsShould.OuterCg), false)]
        [InlineData(typeof(NullExtensionsShould.OuterCg.InnerCgOuterCg), false)]
        [InlineData(typeof(NullExtensionsShould.OuterCg.InnerNoCgOuterCg), false)]
        [InlineData(typeof(NullExtensionsShould.OuterNoCg), true)]
        [InlineData(typeof(NullExtensionsShould.OuterNoCg.InnerCgOuterNoCg), false)]
        [InlineData(typeof(NullExtensionsShould.OuterNoCg.InnerNoCgOuterNoCg), true)]
        public void ExcludeCompilerGeneratedTypes(Type type, bool expected)
        {
            ITypeFilter sut = new NotCompilerGenerated();

            // Act
            bool actual = sut.IncludeType(type);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
