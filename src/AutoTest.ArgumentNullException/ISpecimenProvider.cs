// Copyright (c) 2013 - 2017 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Provides parameter and instance specimens for a null parameter invocation of a method.
    /// </summary>
    public interface ISpecimenProvider
    {
        /// <summary>
        /// Gets the specimens for the <paramref name="parameters"/>.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="nullIndex">The index of the null parameter.</param>
        /// <returns>The specimens for the <paramref name="parameters"/>.</returns>
        object[] GetParameterSpecimens(IList<ParameterInfo> parameters, int nullIndex);

        /// <summary>
        /// Creates an instance of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to create.</param>
        /// <returns>The instance of the <paramref name="type"/>.</returns>
        object CreateInstance(Type type);
    }
}
