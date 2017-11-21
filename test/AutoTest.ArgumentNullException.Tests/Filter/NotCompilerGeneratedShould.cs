namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using global::Xunit;

    public class NotCompilerGeneratedShould
    {
        [Theory, AutoMock]
        public void ReturnName(NotCompilerGenerated sut)
        {
            Assert.Equal("NotCompilerGenerated", sut.Name);
        }

        [Theory]
        [InlineData(typeof(NullExtensionsShould.OuterCg), true)]
        [InlineData(typeof(NullExtensionsShould.OuterCg.InnerCgOuterCg), true)]
        [InlineData(typeof(NullExtensionsShould.OuterCg.InnerNoCgOuterCg), true)]
        [InlineData(typeof(NullExtensionsShould.OuterNoCg), false)]
        [InlineData(typeof(NullExtensionsShould.OuterNoCg.InnerCgOuterNoCg), true)]
        [InlineData(typeof(NullExtensionsShould.OuterNoCg.InnerNoCgOuterNoCg), false)]
        public void ExcludeCompilerGeneratedTypes(Type type, bool expected)
        {
            ITypeFilter sut = new NotCompilerGenerated();

            // Act
            bool actual = sut.ExcludeType(type);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(typeof(NullExtensionsShould.OuterCg), true)]
        [InlineData(typeof(NullExtensionsShould.OuterCg.InnerCgOuterCg), true)]
        [InlineData(typeof(NullExtensionsShould.OuterCg.InnerNoCgOuterCg), true)]
        [InlineData(typeof(NullExtensionsShould.OuterNoCg), false)]
        [InlineData(typeof(NullExtensionsShould.OuterNoCg.InnerCgOuterNoCg), true)]
        [InlineData(typeof(NullExtensionsShould.OuterNoCg.InnerNoCgOuterNoCg), false)]
        public void ExcludeCompilerGeneratedMethods(Type type, bool expected)
        {
            // Arrange
            ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
            IMethodFilter sut = new NotCompilerGenerated();

            // Act
            bool actual = sut.ExcludeMethod(type, constructor);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
