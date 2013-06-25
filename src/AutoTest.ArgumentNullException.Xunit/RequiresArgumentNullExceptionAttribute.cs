namespace AutoTest.ArgNullEx.Xunit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using global::Xunit.Extensions;

    /// <summary>
    /// Test Attribute to prove methods correctly throw <see cref="ArgumentNullException"/> errors.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RequiresArgumentNullExceptionAttribute : DataAttribute
    {
        /// <summary>
        /// The fixture.
        /// </summary>
        private readonly IArgumentNullExceptionFixture _fixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiresArgumentNullExceptionAttribute"/> class.
        /// </summary>
        /// <param name="assemblyUnderTest">A <see cref="Type"/> in the assembly under test.</param>
        public RequiresArgumentNullExceptionAttribute(Type assemblyUnderTest)
            : this(new ArgumentNullExceptionFixture(GetAssembly(assemblyUnderTest)))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiresArgumentNullExceptionAttribute"/> class.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        protected RequiresArgumentNullExceptionAttribute(
            IArgumentNullExceptionFixture fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");

            _fixture = fixture;
        }

        /// <summary>
        /// Gets the <see cref="IArgumentNullExceptionFixture"/>.
        /// </summary>
        public IArgumentNullExceptionFixture Fixture
        {
            get { return _fixture; }
        }

        /// <summary>
        /// Returns the data for the test <see cref="TheoryAttribute"/>.
        /// </summary>
        /// <param name="methodUnderTest">The test method under test.</param>
        /// <param name="parameterTypes">The types of the parameters.</param>
        /// <returns>The data for the test <see cref="TheoryAttribute"/>.</returns>
        public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest, Type[] parameterTypes)
        {
            if (methodUnderTest == null)
                throw new ArgumentNullException("methodUnderTest");
            if (parameterTypes == null)
                throw new ArgumentNullException("parameterTypes");

            CustomizeFixture(methodUnderTest, _fixture);

            return _fixture.GetData().Select(data => new object[] { data });
        }

        /// <summary>
        /// Get the <see cref="Assembly"/> from the supplied <see cref="Type"/>.
        /// </summary>
        /// <param name="assemblyUnderTest">A <see cref="Type"/> in the assembly under test.</param>
        /// <returns>The <see cref="Assembly"/> from the supplied <see cref="Type"/>.</returns>
        private static Assembly GetAssembly(Type assemblyUnderTest)
        {
            if (assemblyUnderTest == null)
                throw new ArgumentNullException("assemblyUnderTest");

            return assemblyUnderTest.Assembly;
        }

        /// <summary>
        /// Customizes the <see cref="Fixture"/> with any <see cref="CustomizeAttribute"/>.
        /// </summary>
        /// <param name="method">The method that may have been customized.</param>
        /// <param name="fixture">The fixture.</param>
        private static void CustomizeFixture(MethodInfo method, IArgumentNullExceptionFixture fixture)
        {
            if (method == null)
                throw new ArgumentNullException("method");
            if (fixture == null)
                throw new ArgumentNullException("fixture");

            IEnumerable<CustomizeAttribute> customizeAttributes =
                method.GetCustomAttributes(typeof(CustomizeAttribute), inherit: false)
                      .Cast<CustomizeAttribute>();

            foreach (CustomizeAttribute customizeAttribute in customizeAttributes)
            {
                fixture.Customize(customizeAttribute.GetCustomization(method));
            }
        }
    }
}
