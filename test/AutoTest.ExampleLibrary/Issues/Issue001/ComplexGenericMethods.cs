namespace AutoTest.ExampleLibrary.Issues.Issue001
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Test class used to demonstrate issue 1 on generic methods.
    /// https://github.com/AutoTestNET/AutoTest.ArgumentNullException/issues/1
    /// </summary>
    public static class ComplexGenericMethods
    {
        public interface ITest1
        {
            string String1 { get; set; }
        }

        public interface ITest2
        {
            string String2 { get; set; }
        }

        /// <summary>
        /// Gets a value indicating if the <see cref="GenericClassMethod{T}"/> classValue parameter has been tested.
        /// </summary>
        public static bool ClassValueTested { get; private set; }

        /// <summary>
        /// Gets a value indicating if the <see cref="GenericClassMethod{T}"/> genericClassMethodStringValue parameter has been tested.
        /// </summary>
        public static bool GenericClassMethodStringValueTested { get; private set; }

        public static void GenericClassMethod<TClass>(TClass classValue, string genericClassMethodStringValue)
            where TClass : class, ITest1, ITest2
        {
            if (classValue == null)
            {
                ClassValueTested = true;
                throw new ArgumentNullException(nameof(classValue));
            }
            if (genericClassMethodStringValue == null)
            {
                GenericClassMethodStringValueTested = true;
                throw new ArgumentNullException(nameof(genericClassMethodStringValue));
            }

            throw new Exception("Shouldn't ever get here.");
        }

        /// <summary>
        /// Gets a value indicating if the <see cref="GenericExceptionMethod{T}"/> exceptionValue parameter has been tested.
        /// </summary>
        public static bool ExceptionValueTested { get; private set; }

        /// <summary>
        /// Gets a value indicating if the <see cref="GenericExceptionMethod{T}"/> genericExceptionMethodStringValue parameter has been tested.
        /// </summary>
        public static bool GenericExceptionMethodStringValueTested { get; private set; }

        public static void GenericExceptionMethod<TException>(TException exceptionValue, string genericExceptionMethodStringValue)
            where TException : Exception, new()
        {
            if (exceptionValue == null)
            {
                ExceptionValueTested = true;
                throw new ArgumentNullException(nameof(exceptionValue));
            }
            if (genericExceptionMethodStringValue == null)
            {
                GenericExceptionMethodStringValueTested = true;
                throw new ArgumentNullException(nameof(genericExceptionMethodStringValue));
            }

            throw new Exception("Shouldn't ever get here.");
        }
    }
}
