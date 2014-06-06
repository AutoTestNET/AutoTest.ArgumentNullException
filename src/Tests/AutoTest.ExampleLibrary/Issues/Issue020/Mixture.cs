namespace AutoTest.ExampleLibrary.Issues.Issue020
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
