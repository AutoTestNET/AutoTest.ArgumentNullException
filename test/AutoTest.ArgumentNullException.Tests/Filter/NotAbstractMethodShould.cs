namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using global::Xunit;

    public class NotAbstractMethodShould
    {
        private abstract class TestClass
        {
            protected abstract void AbstractMethod();
            protected virtual void NonAbstractMethod()
            {
            }
        }

        public static IEnumerable<object[]> AbstractMethods
        {
            get { return GetAbstractMethods(); }
        }

        internal static IEnumerable<object[]> GetAbstractMethods()
        {
            Type type = typeof (TestClass);
            MethodInfo abstractMethod = type.GetMethod("AbstractMethod", BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo nonAbstractMethod = type.GetMethod("NonAbstractMethod", BindingFlags.NonPublic | BindingFlags.Instance);

            return new[]
                {
                    new object[] {type, abstractMethod, true},
                    new object[] {type, nonAbstractMethod, false},
                };
        }

        [Theory, AutoMock]
        public void ReturnName(NotAbstractMethod sut)
        {
            Assert.Equal("NotAbstractMethod", sut.Name);
        }

        [Theory, MemberData(nameof(AbstractMethods))]
        public void ExcludeAbstractMethods(Type type, MethodInfo method, bool expected)
        {
            // Arrange
            IMethodFilter sut = new NotAbstractMethod();

            // Act
            bool actual = sut.ExcludeMethod(type, method);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
