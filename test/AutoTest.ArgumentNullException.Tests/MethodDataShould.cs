namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using AutoTest.ArgNullEx.Execution;
    using global::Xunit;

    public class MethodDataShould
    {
        [Theory, AutoMock]
        public void InitialiseProperties(
            Type classUnderTest,
            object instanceUnderTest,
            MethodBase methodUnderTest,
            object[] parameters,
            string nullParameter,
            int nullIndex,
            IExecutionSetup executionSetup)
        {
            // Act
            var sut = new MethodData(
                classUnderTest,
                instanceUnderTest,
                methodUnderTest,
                parameters,
                nullParameter,
                nullIndex,
                executionSetup);

            // Assert
            Assert.Same(classUnderTest, sut.ClassUnderTest);
            Assert.Equal(instanceUnderTest, sut.InstanceUnderTest);
            Assert.Same(methodUnderTest, sut.MethodUnderTest);
            Assert.Same(parameters, sut.Parameters);
            Assert.Same(nullParameter, sut.NullParameter);
            Assert.Equal(nullIndex, sut.NullIndex);
            Assert.Same(executionSetup, sut.ExecutionSetup);
        }

        [Theory, AutoMock]
        public void ProvideDebuggerDisplay(MethodData sut)
        {
            // Assert as we go
            DebuggerDisplayAttribute debuggerDisplay = sut.GetType().GetTypeInfo().GetCustomAttribute<DebuggerDisplayAttribute>(inherit: false);
            Assert.NotNull(debuggerDisplay);
            Assert.Contains("DebuggerDisplay", debuggerDisplay.Value);

            PropertyInfo propertyInfo = sut.GetType().GetProperty("DebuggerDisplay", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(propertyInfo);

            MethodInfo getMethod = propertyInfo.GetGetMethod(true);
            Assert.NotNull(getMethod);

            string display = Assert.IsType<string>(getMethod.Invoke(sut, new object[] { }));
            Assert.Contains(sut.ToString(), display);
        }
    }
}
