// Copyright (c) 2013 - 2017 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace AutoTest.ArgNullEx.Xunit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using global::Xunit;

    /// <summary>
    /// Extension methods on a <see cref="MethodData"/>.
    /// </summary>
    public static class MethodDataExtensions
    {
        /// <summary>
        /// Executes the <paramref name="method"/> and checks whether it correctly throws a
        /// <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="method">The method data.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous execution of the
        /// <paramref name="method"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="method"/> parameter is
        /// <see langword="null"/>.</exception>
        public static Task Execute(this MethodData method)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            return Assert.ThrowsAsync<ArgumentNullException>(method.NullParameter, method.ExecuteAction);
        }
    }
}
