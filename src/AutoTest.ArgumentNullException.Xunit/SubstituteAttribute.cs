// Copyright (c) 2013 - 2017 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace AutoTest.ArgNullEx.Xunit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// An attribute that can be applied to methods in an <see cref="RequiresArgumentNullExceptionAttribute"/>-driven
    /// Theory to indicate that the specified <see cref="SubstituteAttribute.OriginalType"/> should be substituted for
    /// the <see cref="SubstituteAttribute.NewType"/> when testing.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class SubstituteAttribute : CustomizeAttribute, IArgNullExCustomization
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubstituteAttribute" /> class.
        /// </summary>
        /// <param name="originalType">The original <see cref="Type"/>.</param>
        /// <param name="newType">The new <see cref="Type"/>.</param>
        public SubstituteAttribute(Type originalType, Type newType)
        {
            if (originalType == null)
                throw new ArgumentNullException("originalType");
            if (newType == null)
                throw new ArgumentNullException("newType");

            OriginalType = originalType;
            NewType = newType;
        }

        /// <summary>
        /// Gets the original <see cref="Type"/> to be substituted by the <see cref="NewType"/>.
        /// </summary>
        public Type OriginalType { get; private set; }

        /// <summary>
        /// Gets the new <see cref="Type"/> to substitute in place of the <see cref="OriginalType"/>.
        /// </summary>
        public Type NewType { get; private set; }

        /// <summary>
        /// Gets a customization for a test method.
        /// </summary>
        /// <param name="method">The method to be customized.</param>
        /// <returns>A customization for a test method.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="method"/> parameter is
        /// <see langword="null"/>.</exception>
        public override IArgNullExCustomization GetCustomization(MethodInfo method)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            return this;
        }

        /// <summary>
        /// Customizes the specified <paramref name="fixture"/> by substituting the <see cref="OriginalType"/> with the
        /// <see cref="NewType"/>.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="fixture"/> parameter is
        /// <see langword="null"/>.</exception>
        public void Customize(IArgumentNullExceptionFixture fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");

            fixture.SubstituteType(OriginalType, NewType);
        }
    }
}
