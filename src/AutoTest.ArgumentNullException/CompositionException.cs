﻿namespace AutoTest.ArgNullEx
{
    using System;
    using System.Reflection;

    /// <summary>
    /// An <see cref="Exception"/> representing an error composing a method execution.
    /// </summary>
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
        private static string CreateMessage(
            Type classUnderTest,
            MethodBase methodUnderTest,
            string nullParameter,
            Exception innerException)
        {
            if (classUnderTest == null)
                throw new ArgumentNullException("classUnderTest");
            if (methodUnderTest == null)
                throw new ArgumentNullException("methodUnderTest");
            if (nullParameter == null)
                throw new ArgumentNullException("nullParameter");
            if (innerException == null)
                throw new ArgumentNullException("innerException");

            return string.Format(
                "Error in the composition for the test '{0}.{1} {2}=null'.\n{3}",
                classUnderTest.Name,
                methodUnderTest.Name,
                nullParameter,
                innerException);
        }
    }
}