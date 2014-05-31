namespace AutoTest.ExampleLibrary.Issues.Issue012
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class YieldExample
    {
        /// <summary>
        /// Gets a value indicating if the <see cref="YieldExample"/> has been tested.
        /// </summary>
        public static bool Tested { get; private set; }

        public static IEnumerable<string> YieldResult(string stringValue)
        {
            Tested = false;

            if (stringValue != null)
            {
                yield return stringValue;
                throw new Exception("Shouldn't ever get here.");
            }

            Tested = true;
            throw new ArgumentNullException("stringValue");
        }
    }
}
