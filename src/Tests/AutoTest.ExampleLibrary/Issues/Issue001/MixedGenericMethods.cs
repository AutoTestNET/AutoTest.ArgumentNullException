namespace AutoTest.ExampleLibrary.Issues.Issue001
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Test class used to demonstrate issue 1 on generic methods.
    /// https://github.com/AutoTestNET/AutoTest.ArgumentNullException/issues/1
    /// </summary>
    public static class MixedGenericMethods
    {
        /// <summary>
        /// Gets a value indicating if the <see cref="GenericMethod{T, T}"/> classValue parameter has been tested.
        /// </summary>
        public static bool ClassValueTested { get; private set; }

        /// <summary>
        /// Gets a value indicating if the <see cref="GenericMethod{T, T}"/> stringValue parameter has been tested.
        /// </summary>
        public static bool StringValueTested { get; private set; }

        public static void GenericMethod<TClass, TStruct>(TClass classValue, int intValue, TStruct structValue, string stringValue)
            where TClass : class
            where TStruct : struct
        {
            ClassValueTested = StringValueTested = false;

            if (classValue == null)
            {
                ClassValueTested = true;
                throw new ArgumentNullException("classValue");
            }
            if (stringValue == null)
            {
                StringValueTested = true;
                throw new ArgumentNullException("stringValue");
            }

            throw new Exception("Shouldn't ever get here.");
        }
    }
}
