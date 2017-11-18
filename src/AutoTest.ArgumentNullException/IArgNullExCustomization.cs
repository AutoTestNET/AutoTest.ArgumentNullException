// Copyright (c) 2013 - 2017 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Encapsulates a customization of an <see cref="IArgumentNullExceptionFixture"/>.
    /// </summary>
    public interface IArgNullExCustomization
    {
        /// <summary>
        /// Customizes the specified <paramref name="fixture"/>.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="fixture"/> parameter is <see langword="null"/>.</exception>
        void Customize(IArgumentNullExceptionFixture fixture);
    }
}
