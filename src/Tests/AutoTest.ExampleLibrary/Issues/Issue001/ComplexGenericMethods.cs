namespace AutoTest.ExampleLibrary.Issues.Issue001
{
    using System;

    public static class ComplexGenericMethods
    {
        /// <summary>
        /// Gets a value indicating if the <see cref="NonGenericMethod"/> value parameter has been tested.
        /// </summary>
        public static bool NonGenericTested { get; private set; }

        public interface ITest1
        {
            string String1 { get; set; }
        }

        public interface ITest2
        {
            string String2 { get; set; }
        }

        public static void NonGenericMethod(string value)
        {
            NonGenericTested = false;
            if (value == null)
            {
                NonGenericTested = true;
                throw new ArgumentNullException("value");
            }

            throw new Exception("Shouldn't ever get here.");
        }

        public static void GenericClassMethod<TClass>(TClass classValue, string stringValue)
            where TClass : class, ITest1, ITest2
        {
            throw new Exception("Shouldn't ever get here.");
        }

        public static void GenericExceptionMethod<TException>(TException classValue, string stringValue)
            where TException : Exception, ITest1, ITest2
        {
            throw new Exception("Shouldn't ever get here.");
        }

        public static void GenericStructMethod<TStruct>(TStruct classValue, string stringValue)
            where TStruct : struct, ITest1, ITest2
        {
            throw new Exception("Shouldn't ever get here.");
        }
    }
}
