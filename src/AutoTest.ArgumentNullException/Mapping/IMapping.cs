// Copyright (c) 2013 - 2017 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace AutoTest.ArgNullEx.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The base for all mappings.
    /// </summary>
    public interface IMapping
    {
        /// <summary>
        /// Gets the name of the mapping.
        /// </summary>
        string Name { get; }
    }
}
