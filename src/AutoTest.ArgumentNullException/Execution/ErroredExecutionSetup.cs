// Copyright (c) 2013 - 2017 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace AutoTest.ArgNullEx.Execution
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// The <see cref="IExecutionSetup"/> used to throw a setup error.
    /// </summary>
    public sealed class ErroredExecutionSetup : IExecutionSetup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErroredExecutionSetup"/> class.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to throw when executed.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="exception"/> parameter is <see langword="null"/>.</exception>
        public ErroredExecutionSetup(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            Exception = exception;
        }

        /// <summary>
        /// Gets the <see cref="Exception"/> to throw when executed.
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Sets up an errored execute setup.
        /// </summary>
        /// <param name="methodData">The method data.</param>
        /// <returns>A <see cref="Func{T}"/> returning an errored <see cref="Task"/> setup with the <see cref="Exception"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="methodData"/> parameter is <see langword="null"/>.</exception>
        Func<Task> IExecutionSetup.Setup(MethodData methodData)
        {
            if (methodData == null)
                throw new ArgumentNullException(nameof(methodData));

            return Execute;
        }

        /// <summary>
        /// Returns an errored <see cref="Task"/> setup with the <see cref="Exception"/>.
        /// </summary>
        /// <returns>An errored <see cref="Task"/> setup with the <see cref="Exception"/>.</returns>
        private Task Execute()
        {
            var tcs = new TaskCompletionSource<int>();
            tcs.SetException(Exception);
            return tcs.Task;
        }
    }
}
