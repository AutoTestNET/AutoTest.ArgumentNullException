namespace AutoTest.ExampleLibrary.Issues.Issue004
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Test class used to demonstrate issue 3 and 4
    /// https://github.com/AutoTestNET/AutoTest.ArgumentNullException/issues/3
    /// https://github.com/AutoTestNET/AutoTest.ArgumentNullException/issues/4
    /// </summary>
    public class SomeOutParameters
    {
        /// <summary>
        /// Gets a value indicating if the <see cref="SomeOutParametersMethod"/> stringInput parameter has been tested.
        /// </summary>
        public static bool StringInputTested { get; private set; }

        /// <summary>
        /// Gets a value indicating if the <see cref="SomeOutParametersMethod"/> stringRef parameter has been tested.
        /// </summary>
        public static bool StringRefTested { get; private set; }

        private static void SomeOutParametersMethod(
            int intInput,
            string stringInput,
            Guid guidInput,
            ref int intRef,
            ref string stringRef,
            ref Guid guidRef,
            out int intOutput,
            out string stringOutput,
            out Guid guidOutput)
        {
            StringInputTested = StringRefTested = false;

            if (stringInput == null)
            {
                StringInputTested = true;
                throw new ArgumentNullException("stringInput");
            }
            if (stringRef == null)
            {
                StringRefTested = true;
                throw new ArgumentNullException("stringRef");
            }

            throw new Exception("Shouldn't ever get here.");
        }
    }
}
