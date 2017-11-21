namespace AutoTest.ArgNullEx.Xunit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using global::Xunit;

    public class SubstituteAttributeShould
    {
        [Theory, AutoMock]
        public void BeACustomization(SubstituteAttribute sut)
        {
            // Assert
            Assert.IsAssignableFrom<IArgNullExCustomization>(sut);
            Assert.IsAssignableFrom<CustomizeAttribute>(sut);
        }

        [Theory, AutoMock]
        public void ReturnSelfCustomization(SubstituteAttribute sut, MethodInfo method)
        {
            // Act
            IArgNullExCustomization customization = sut.GetCustomization(method);

            // Assert
            Assert.Same(sut, customization);
        }

        [Fact]
        public void InitializeWithTypes()
        {
            // Act
            var sut = new SubstituteAttribute(typeof(Dictionary<,>), typeof(Dictionary<string, string>));

            // Assert
            Assert.Same(typeof(Dictionary<,>), sut.OriginalType);
            Assert.Same(typeof(Dictionary<string, string>), sut.NewType);
        }
    }
}
