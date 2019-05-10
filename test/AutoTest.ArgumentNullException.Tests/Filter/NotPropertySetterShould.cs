namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using global::Xunit;

    public class NotPropertySetterShould
    {
        class InnerClass
        {
            private string DoNotTest1
            {
                set
                {
                    throw new NotImplementedException("The setter for DoNotTest1 should not be tested for ArgumentNullException.");
                }
            }

            public string DoNotTest2
            {
                set
                {
                    throw new NotImplementedException("The setter for DoNotTest2 should not be tested for ArgumentNullException.");
                }
            }

            private static string DoNotTest3
            {
                set
                {
                    throw new NotImplementedException("The setter for DoNotTest1 should not be tested for ArgumentNullException.");
                }
            }

            public static string DoNotTest4
            {
                set
                {
                    throw new NotImplementedException("The setter for DoNotTest2 should not be tested for ArgumentNullException.");
                }
            }

            public void Overload()
            {
            }

            private void Overload(string input1)
            {
            }

            public static void Overload(string input1, string input2)
            {
            }

            private static void Overload(string input1, string input2, string input3)
            {
            }
        }

        public static IEnumerable<object[]> SomePropertySetters => GetSomePropertySetters();

        internal static IEnumerable<object[]> GetSomePropertySetters()
        {
            Type type = typeof(InnerClass);
            MethodInfo[] methods = type.GetMethods(ArgumentNullExceptionFixture.DefaultBindingFlags);

            return methods.Select(method => new object[] { type, method, method.Name.Contains("DoNotTest") });
        }

        [Theory, AutoMock]
        public void ReturnName(NotPropertySetter sut)
        {
            Assert.Equal("NotPropertySetter", sut.Name);
        }

        [Theory, MemberData(nameof(SomePropertySetters))]
        public void ExcludPropertySetters(Type type, MethodInfo method, bool expected)
        {
            // Arrange
            IMethodFilter sut = new NotPropertySetter();

            // Act
            bool actual = sut.ExcludeMethod(type, method);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
