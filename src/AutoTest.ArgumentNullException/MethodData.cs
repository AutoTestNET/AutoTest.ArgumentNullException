﻿// Copyright (c) 2013 - 2017 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using AutoTest.ArgNullEx.Execution;

    /// <summary>
    /// The data representing a single instance of a <see cref="ArgumentNullException"/> use case test.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class MethodData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodData"/> class.
        /// </summary>
        /// <param name="classUnderTest">The type of the class under test.</param>
        /// <param name="instanceUnderTest">The instance of the class under test if the
        /// <paramref name="methodUnderTest"/> is not static.</param>
        /// <param name="methodUnderTest">The method under test.</param>
        /// <param name="parameters">The parameters to the <paramref name="methodUnderTest"/>.</param>
        /// <param name="nullParameter">The name of the null parameter in the <paramref name="parameters"/>.</param>
        /// <param name="nullIndex">The index of the null parameter in the <paramref name="parameters"/>.</param>
        /// <param name="executionSetup">The setup for the <see cref="ExecuteAction"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="classUnderTest"/>,
        /// <paramref name="methodUnderTest"/>, <paramref name="parameters"/>, <paramref name="nullParameter"/> or
        /// <paramref name="executionSetup"/> parameters are <see langword="null"/>.</exception>
        public MethodData(
            Type classUnderTest,
            object instanceUnderTest,
            MethodBase methodUnderTest,
            object[] parameters,
            string nullParameter,
            int nullIndex,
            IExecutionSetup executionSetup)
        {
            if (classUnderTest == null)
                throw new ArgumentNullException(nameof(classUnderTest));
            if (methodUnderTest == null)
                throw new ArgumentNullException(nameof(methodUnderTest));
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (nullParameter == null)
                throw new ArgumentNullException(nameof(nullParameter));
            if (executionSetup == null)
                throw new ArgumentNullException(nameof(executionSetup));

            ClassUnderTest = classUnderTest;
            InstanceUnderTest = instanceUnderTest;
            MethodUnderTest = methodUnderTest;
            Parameters = parameters;
            NullParameter = nullParameter;
            NullIndex = nullIndex;
            ExecutionSetup = executionSetup;
        }

        /// <summary>
        /// Gets the type of the class under test.
        /// </summary>
        public Type ClassUnderTest { get; }

        /// <summary>
        /// Gets the instance of the class under test if the <see cref="MethodUnderTest"/> is not static.
        /// </summary>
        public object InstanceUnderTest { get; }

        /// <summary>
        /// Gets the method under test.
        /// </summary>
        public MethodBase MethodUnderTest { get; }

        /// <summary>
        /// Gets the parameters to the <see cref="MethodUnderTest"/>.
        /// </summary>
        public object[] Parameters { get; }

        /// <summary>
        /// Gets the name of the null parameter in the <see cref="Parameters"/>.
        /// </summary>
        public string NullParameter { get; }

        /// <summary>
        /// Gets the index of the null parameter in the <see cref="Parameters"/>.
        /// </summary>
        public int NullIndex { get; }

        /// <summary>
        /// Gets the setup for <see cref="ExecuteAction"/>.
        /// </summary>
        public IExecutionSetup ExecutionSetup { get; }

        /// <summary>
        /// Gets the text to display within the debugger.
        /// </summary>
        private string DebuggerDisplay => nameof(MethodData) + ": " + ToString();

        /// <summary>
        /// Executes the action for the <see cref="MethodUnderTest"/>.
        /// </summary>
        /// <returns>The <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task ExecuteAction()
        {
            return ExecutionSetup.Setup(this)();
        }

        /// <summary>
        /// Returns a human readable representation of the <see cref="MethodData"/>.
        /// </summary>
        /// <returns>A human readable representation of the <see cref="MethodData"/>.</returns>
        public override string ToString()
        {
            return $"{ClassUnderTest.Name}.{MethodUnderTest.Name} {NullParameter}=null";
        }
    }
}
