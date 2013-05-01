namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Moq;
    using global::Xunit;
    using global::Xunit.Extensions;

    public class AssemblyTypeFilteringShould
    {
        [Theory, AutoMock]
        public void ApplyFiltersToAllTypes(
            Assembly assembly,
            Mock<ITypeFilter> filterMock)
        {
            // Arrange
            Type[] expectedTypes = assembly.GetTypes();

            // Confirm there are some types in the assembly.
            Assert.NotEmpty(expectedTypes);
            filterMock.Setup(f => f.ExcludeType(It.IsAny<Type>())).Returns(false);

            // Act
            List<Type> actualTypes = assembly.GetTypes(new[] {filterMock.Object}).ToList();

            // Assert
            filterMock.Verify(f => f.ExcludeType(It.IsAny<Type>()), Times.Exactly(expectedTypes.Length));
            Assert.Equal(expectedTypes.Length, actualTypes.Count);
            Assert.False(expectedTypes.Except(actualTypes).Any());
        }

        [Theory, AutoMock]
        public void ApplyFiltersCorrectly(
            Assembly assembly,
            IsClassOrStruct filter)
        {
            // Arrange
            List<Type> interfaceTypes = assembly.GetTypes().Where(t => t.IsInterface).ToList();

            // Confirm there are interfaces to exclude.
            Assert.NotEmpty(interfaceTypes);

            // Act
            List<Type> actualTypes = assembly.GetTypes(new[] { filter }).ToList();

            // Assert
            Assert.NotEmpty(actualTypes);
            Assert.Equal(actualTypes.Count, actualTypes.Except(interfaceTypes).Count());
        }
    }
}
