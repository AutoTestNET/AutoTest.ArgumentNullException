namespace AutoTest.ExampleLibrary.Issues.Issue004
{
    using System;

    /// <summary>
    /// Test class used to demonstrate issue 3 and 4
    /// https://github.com/AutoTestNET/AutoTest.ArgumentNullException/issues/3
    /// https://github.com/AutoTestNET/AutoTest.ArgumentNullException/issues/4
    /// </summary>
    class SomeOutParameters
    {
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
            if (stringInput == null)
                throw new ArgumentNullException("stringInput");
            if (stringRef == null)
                throw new ArgumentNullException("stringRef");

            intOutput = 0;
            stringOutput = string.Empty;
            guidOutput = Guid.Empty;
        }
    }
}
