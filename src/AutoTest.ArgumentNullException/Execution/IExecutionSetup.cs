// Copyright (c) 2013 - 2017 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace AutoTest.ArgNullEx.Execution
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// Defined a setup for a reflected asynchronous <see cref="MethodBase"/> execution.
    /// </summary>
    public interface IExecutionSetup
    {
        /// <summary>
        /// Sets up a reflected asynchronous <see cref="MethodBase"/> execution.
        /// </summary>
        /// <param name="methodData">The method data.</param>
        /// <returns>A reflected asynchronous <see cref="MethodBase"/> execution.</returns>
        Func<Task> Setup(MethodData methodData);
    }
}
