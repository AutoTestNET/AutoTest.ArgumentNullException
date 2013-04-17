﻿namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Xunit.Extensions;

    /// <summary>
    /// Test Attribute to prove methods correctly throw <see cref="ArgumentNullException"/> errors.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RequiresArgumentNullExceptionAttribute : DataAttribute
    {
        /// <summary>
        /// The fixture.
        /// </summary>
        private readonly ArgumentNullExceptionFixture _fixture;

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
            ArgumentNullExceptionFixture fixture)
        {
            _fixture = fixture;
        }

        /// <summary>
        /// Returns the data for the test <see cref="TheoryAttribute"/>.
        /// </summary>
        /// <param name="methodUnderTest">The test method under test.</param>
        /// <param name="parameterTypes">The types of the parameters.</param>
        /// <returns>The data for the test <see cref="TheoryAttribute"/>.</returns>
        public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest, Type[] parameterTypes)
        {
            if (methodUnderTest == null) throw new ArgumentNullException("methodUnderTest");
            if (parameterTypes == null) throw new ArgumentNullException("parameterTypes");

            return _fixture.GetData().Select(data => new object[] { data });
        }

        /// <summary>
        /// Get the <see cref="Assembly"/> from the supplied <see cref="Type"/>.
        /// </summary>
        /// <param name="assemblyUnderTest">A <see cref="Type"/> in the assembly under test.</param>
        /// <returns>The <see cref="Assembly"/> from the supplied <see cref="Type"/>.</returns>
        private static Assembly GetAssembly(Type assemblyUnderTest)
        {
            if (assemblyUnderTest == null) throw new ArgumentNullException("assemblyUnderTest");

            return assemblyUnderTest.Assembly;
        }
    }
}
