// Copyright (c) 2013 - 2017 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace AutoTest.ArgNullEx.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Interface defining a mapping from one <see cref="Type"/> to another.
    /// </summary>
    public interface ITypeMapping : IMapping
    {
        /// <summary>
        /// Returns the <see cref="Type"/> to use instead of the supplied <paramref name="type"/>; otherwise the same
        /// <paramref name="type"/> if no mapping is needed.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The <see cref="Type"/> to use instead of the supplied <paramref name="type"/>; otherwise the same
        /// <paramref name="type"/> if no mapping is needed.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="type"/> parameter is <see langword="null"/>.
        /// </exception>
        Type MapTo(Type type);
    }
}
