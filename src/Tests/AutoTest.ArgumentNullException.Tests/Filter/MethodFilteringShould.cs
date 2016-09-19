namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Moq;
    using global::Xunit;

    public class MethodFilteringShould
    {
        [Theory, AutoMock]
        public void ApplyFiltersToAllMethods(
            Assembly assembly,
            Mock<IMethodFilter> filterMock)
        {
            // Arrange
            const BindingFlags bindingFlags = ArgumentNullExceptionFixture.DefaultBindingFlags;
            List<MethodBase> expectedMethods = GetAllMethods(assembly, bindingFlags).ToList();

            // Confirm there are some methods in the assembly.
            Assert.NotEmpty(expectedMethods);
            filterMock.Setup(f => f.ExcludeMethod(It.IsAny<Type>(), It.IsAny<MethodBase>())).Returns(false);

            // Act
            List<MethodBase> actualMethods =
                (from type in assembly.GetTypes()
                 from method in type.GetMethods(bindingFlags, new[] { filterMock.Object })
                 select method).ToList();

            // Assert
            filterMock.Verify(f => f.ExcludeMethod(It.IsAny<Type>(), It.IsAny<MethodBase>()),
                              Times.Exactly(expectedMethods.Count));
            Assert.Equal(expectedMethods.Count, actualMethods.Count);
        }

        private static IEnumerable<MethodBase> GetAllMethods(Assembly assembly, BindingFlags bindingFlags)
        {
            return
                from type in assembly.GetTypes()
                from method in type.GetMethods(bindingFlags)
                                   .Cast<MethodBase>()
                                   .Concat(type.GetConstructors(bindingFlags))
                select method;
        }

        [Theory, AutoMock]
        public void ApplyFiltersCorrectly(
            Assembly assembly,
            NotCompilerGenerated filter)
        {
            // Arrange
            const BindingFlags bindingFlags = ArgumentNullExceptionFixture.DefaultBindingFlags;
            List<MethodBase> compilerGeneratedMethods =
                GetAllMethods(assembly, bindingFlags)
                    .Where(m => m.IsCompilerGenerated())
                    .ToList();

            // Confirm there are methods to exclude.
            Assert.NotEmpty(compilerGeneratedMethods);

            // Act
            List<MethodBase> actualMethods =
                (from type in assembly.GetTypes()
                 from method in type.GetMethods(bindingFlags, new[] { filter })
                 select method).ToList();

            // Assert
            Assert.NotEmpty(actualMethods);
            Assert.Equal(actualMethods.Count, actualMethods.Except(compilerGeneratedMethods).Count());
        }
    }
}
