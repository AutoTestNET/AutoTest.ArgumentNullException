// Copyright (c) 2013 - 2017 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Base implementation of <see cref="IFilter"/> providing default behavior.
    /// </summary>
    public abstract class FilterBase : IFilter
    {
        /// <summary>
        /// Gets the name of the filter. The default is to use <see cref="MemberInfo.Name"/>.
        /// </summary>
        public virtual string Name
        {
            get { return GetType().Name; }
        }
    }
}
