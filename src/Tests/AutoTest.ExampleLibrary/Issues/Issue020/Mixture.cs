namespace AutoTest.ExampleLibrary.Issues.Issue020
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Test class used to demonstrate issue 20
    /// https://github.com/AutoTestNET/AutoTest.ArgumentNullException/issues/20
    /// </summary>
    public static class Mixture
    {
        /// <summary>
        /// Gets a value indicating if the <see cref="Mixture"/> has been tested.
        /// </summary>
        public static bool Tested { get; private set; }

        public static string Public(string stringValue)
        {
            Tested = false;
            string returnVal = Private(stringValue);

            if (stringValue != null)
                return returnVal;

            Tested = true;
            throw new ArgumentNullException("stringValue");
        }

        private static string Private(string stringValue)
        {
            return stringValue ?? string.Empty;
        }
    }
}
