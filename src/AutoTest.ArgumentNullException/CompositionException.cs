// Copyright (c) 2013 - 2017 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// An <see cref="Exception"/> representing an error composing a method execution.
    /// </summary>
#if !NETSTANDARD1_5
    [Serializable]
#endif
    public class CompositionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompositionException"/> class.
        /// </summary>
        /// <param name="classUnderTest">The type of the class under test.</param>
        /// <param name="methodUnderTest">The method under test.</param>
        /// <param name="nullParameter">The name of the null parameter.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public CompositionException(
            Type classUnderTest,
            MethodBase methodUnderTest,
            string nullParameter,
            Exception innerException)
            : base(CreateMessage(classUnderTest, methodUnderTest, nullParameter, innerException), innerException)
        {
        }

        /// <summary>
        /// Creates the error message.
        /// </summary>
        /// <param name="classUnderTest">The type of the class under test.</param>
        /// <param name="methodUnderTest">The method under test.</param>
        /// <param name="nullParameter">The name of the null parameter.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        /// <returns>The error message.</returns>
        /// <exception cref="ArgumentNullException">Any of the parameters are <see langword="null"/>.</exception>
        private static string CreateMessage(
            Type classUnderTest,
            MethodBase methodUnderTest,
            string nullParameter,
            Exception innerException)
        {
            if (classUnderTest == null)
                throw new ArgumentNullException(nameof(classUnderTest));
            if (methodUnderTest == null)
                throw new ArgumentNullException(nameof(methodUnderTest));
            if (nullParameter == null)
                throw new ArgumentNullException(nameof(nullParameter));
            if (innerException == null)
                throw new ArgumentNullException(nameof(innerException));

            return
                $"Error in the composition for the test '{classUnderTest.Name}.{methodUnderTest.Name} " +
                $"{nullParameter}=null'.\n{innerException}";
        }
    }
}
