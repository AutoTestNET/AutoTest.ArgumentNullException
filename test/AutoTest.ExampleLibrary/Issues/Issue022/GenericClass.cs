namespace AutoTest.ExampleLibrary.Issues.Issue022
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Test class used to demonstrate issue 22
    /// https://github.com/AutoTestNET/AutoTest.ArgumentNullException/issues/22
    /// </summary>
    public class GenericClass<T>
        where T: class
    {
        /// <summary>
        /// Gets a value indicating if the <see cref="GenericClass{T}"/> has been tested.
        /// </summary>
        public static bool Tested { get; private set; }

        public T Value { get; private set; }

        public GenericClass(T value)
        {
            if (value == null)
            {
                Tested = true;
                throw new ArgumentNullException("value");
            }

            Value = value;
        }
    }
}
