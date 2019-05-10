// Copyright (c) 2013 - 2017 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace AutoTest.ArgNullEx.Xunit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using global::Xunit;
    using global::Xunit.Sdk;

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
        /// <exception cref="ArgumentNullException">The <paramref name="fixture"/> parameter is
        /// <see langword="null"/>.</exception>
        protected RequiresArgumentNullExceptionAttribute(
            IArgumentNullExceptionFixture fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException(nameof(fixture));

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
        /// <param name="testMethod">The test method under test.</param>
        /// <returns>The data for the test <see cref="TheoryAttribute"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="testMethod"/> is <see langword="null"/>.
        /// </exception>
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (testMethod == null)
                throw new ArgumentNullException(nameof(testMethod));

            CustomizeFixture(testMethod, _fixture);

            return _fixture.GetData().Select(data => new object[] { data });
        }

        /// <summary>
        /// Get the <see cref="Assembly"/> from the supplied <see cref="Type"/>.
        /// </summary>
        /// <param name="assemblyUnderTest">A <see cref="Type"/> in the assembly under test.</param>
        /// <returns>The <see cref="Assembly"/> from the supplied <see cref="Type"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="assemblyUnderTest"/> parameter is
        /// <see langword="null"/>.</exception>
        private static Assembly GetAssembly(Type assemblyUnderTest)
        {
            if (assemblyUnderTest == null)
                throw new ArgumentNullException(nameof(assemblyUnderTest));

            return assemblyUnderTest.GetTypeInfo().Assembly;
        }

        /// <summary>
        /// Customizes the <see cref="Fixture"/> with any <see cref="CustomizeAttribute"/>.
        /// </summary>
        /// <param name="method">The method that may have been customized.</param>
        /// <param name="fixture">The fixture.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="method"/> or <paramref name="fixture"/>
        /// parameters is <see langword="null"/>.</exception>
        private static void CustomizeFixture(MethodInfo method, IArgumentNullExceptionFixture fixture)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));
            if (fixture == null)
                throw new ArgumentNullException(nameof(fixture));

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
