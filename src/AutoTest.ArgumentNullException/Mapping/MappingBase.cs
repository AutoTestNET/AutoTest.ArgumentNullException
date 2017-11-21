// Copyright (c) 2013 - 2017 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace AutoTest.ArgNullEx.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Base implementation of <see cref="IMapping"/> providing default behavior.
    /// </summary>
    public abstract class MappingBase : IMapping
    {
        /// <summary>
        /// Gets the name of the Mapping. The default is to use <see cref="MemberInfo.Name"/>.
        /// </summary>
        public virtual string Name
        {
            get { return GetType().Name; }
        }
    }
}
