namespace AutoTest.ExampleLibrary.Issues.Issue001
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Test class used to demonstrate issue 1 on generic methods.
    /// https://github.com/AutoTestNET/AutoTest.ArgumentNullException/issues/1
    /// </summary>
    public static class SimpleGenericMethods
    {
        /// <summary>
        /// Gets a value indicating if the <see cref="GenericMethod2{T}"/> method has been tested.
        /// </summary>
        public static bool GenericMethod2Tested { get; private set; }

        /// <summary>
        /// Gets a value indicating if the <see cref="GenericExceptionMethod{T}"/> method has been tested.
        /// </summary>
        public static bool GenericExceptionMethodTested { get; private set; }

        public static void GenericMethod1<TStruct>(TStruct value)
            where TStruct : struct
        {
            throw new Exception("Shouldn't ever get here.");
        }

        public static void GenericMethod2<TClass>(TClass value)
            where TClass : class
        {
            GenericMethod2Tested = false;

            if (value == null)
            {
                GenericMethod2Tested = true;
                throw new ArgumentNullException("value");
            }

            throw new Exception("Shouldn't ever get here.");
        }

        public static void GenericExceptionMethod<TClass>(TClass value)
            where TClass : Exception
        {
            GenericExceptionMethodTested = false;

            if (value == null)
            {
                GenericExceptionMethodTested = true;
                throw new ArgumentNullException("value");
            }

            throw new Exception("Shouldn't ever get here.");
        }
    }
}
