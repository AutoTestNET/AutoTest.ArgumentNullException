namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using global::Xunit;

    public class RegexRuleShould
    {
        [Theory, AutoMock]
        public void InitialiseProperties(string name, bool include, Regex type, Regex method, Regex parameter)
        {
            // Act
            var sut = new RegexRule(name, include, type, method, parameter);

            // Assert
            Assert.Same(name, sut.Name);
            Assert.Equal(include, sut.Include);
            Assert.Same(type, sut.Type);
            Assert.Same(method, sut.Method);
            Assert.Same(parameter, sut.Parameter);
        }

        [Theory, AutoMock]
        public void ProvideDebuggerDisplay(RegexRule sut)
        {
            // Assert as we go
            var debuggerDisplay = sut.GetType().GetCustomAttribute<DebuggerDisplayAttribute>(inherit: false);
            Assert.NotNull(debuggerDisplay);
            Assert.Contains("DebuggerDisplay", debuggerDisplay.Value);

            PropertyInfo propertyInfo = sut.GetType().GetProperty("DebuggerDisplay", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(propertyInfo);

            MethodInfo getMethod = propertyInfo.GetGetMethod(true);
            Assert.NotNull(getMethod);

            var display = Assert.IsType<string>(getMethod.Invoke(sut, new object[] {}));
            Assert.Contains(sut.Name, display);
        }
    }
}
