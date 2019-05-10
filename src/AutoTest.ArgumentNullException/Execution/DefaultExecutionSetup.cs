// Copyright (c) 2013 - 2017 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace AutoTest.ArgNullEx.Execution
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// The default <see cref="IExecutionSetup"/>.
    /// </summary>
    public sealed class DefaultExecutionSetup : IExecutionSetup
    {
        /// <summary>
        /// A singleton completed task.
        /// </summary>
        private static readonly Task CompletedTask = GetCompletedTask();

        /// <summary>
        /// The parameters to the <see cref="MethodUnderTest"/>.
        /// </summary>
        private object[] _parameters;

        /// <summary>
        /// Gets the method information.
        /// </summary>
        public MethodBase MethodUnderTest { get; private set; }

        /// <summary>
        /// Gets the parameters to the <see cref="MethodUnderTest"/>.
        /// </summary>
        public IEnumerable<object> Parameters => _parameters;

        /// <summary>
        /// Gets the system under tests, can be <see langword="null"/> if the <see cref="MethodUnderTest"/> is static.
        /// </summary>
        public object Sut { get; private set; }

        /// <summary>
        /// Sets up a reflected asynchronous <see cref="MethodBase"/> execution.
        /// </summary>
        /// <param name="methodData">The method data.</param>
        /// <returns>A reflected asynchronous <see cref="MethodBase"/> execution.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="methodData"/> parameter is <see langword="null"/>.</exception>
        Func<Task> IExecutionSetup.Setup(MethodData methodData)
        {
            if (methodData == null)
                throw new ArgumentNullException(nameof(methodData));

            MethodUnderTest = methodData.MethodUnderTest;
            _parameters = methodData.Parameters;
            Sut = methodData.InstanceUnderTest;

            return Execute;
        }

        /// <summary>
        /// Returns a completed task.
        /// </summary>
        /// <returns>A completed task.</returns>
        private static Task GetCompletedTask()
        {
            var tcs = new TaskCompletionSource<int>();
            tcs.SetResult(0);
            return tcs.Task;
        }

        /// <summary>
        /// Executes a reflected <see cref="MethodBase"/>.
        /// </summary>
        /// <returns>A task representing the asynchronous execution of a <see cref="MethodBase"/>.</returns>
        private Task Execute()
        {
            try
            {
                var methodInfo = MethodUnderTest as MethodInfo;
                if (methodInfo != null && typeof(Task).GetTypeInfo().IsAssignableFrom(methodInfo.ReturnType))
                {
                    return ExecuteAsynchronously();
                }

                ExecuteSynchronously();

                return CompletedTask;
            }
            catch (Exception ex)
            {
                var tcs = new TaskCompletionSource<int>();
                tcs.SetException(ex);
                return tcs.Task;
            }
        }

        /// <summary>
        /// Executes the <see cref="MethodUnderTest"/> synchronously.
        /// </summary>
        private void ExecuteSynchronously()
        {
            try
            {
                object result = MethodUnderTest.Invoke(Sut, _parameters);

                if (result is IEnumerable enumerable)
                {
                    enumerable.GetEnumerator().MoveNext();
                }
            }
            catch (TargetInvocationException targetInvocationException)
            {
                if (targetInvocationException.InnerException == null)
                    throw;

                throw targetInvocationException.InnerException;
            }
        }

        /// <summary>
        /// Executes the <see cref="MethodUnderTest"/> synchronously.
        /// </summary>
        /// <returns>The <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        private Task ExecuteAsynchronously()
        {
            try
            {
                var result = (Task)MethodUnderTest.Invoke(Sut, _parameters);
                return result;
            }
            catch (TargetInvocationException targetInvocationException)
            {
                if (targetInvocationException.InnerException == null)
                    throw;

                throw targetInvocationException.InnerException;
            }
        }
    }
}
