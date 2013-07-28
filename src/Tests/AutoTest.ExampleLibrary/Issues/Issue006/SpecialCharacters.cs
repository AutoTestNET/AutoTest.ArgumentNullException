namespace AutoTest.ExampleLibrary.Issues.Issue006
{
    using System;

    /// <summary>
    /// Test class used to demonstrate issue 6
    /// https://github.com/AutoTestNET/AutoTest.ArgumentNullException/issues/6
    /// </summary>
    public class SpecialCharacters
    {
        /// <summary>
        /// Gets a value indicating if the <see cref="InnerClass"/> has been tested.
        /// </summary>
        public static bool Tested { get; private set; }

        public class InnerClass
        {
            static void AMethod(object input)
            {
                Tested = false;

                if (input != null)
                    throw new Exception("Shouldn't ever get here.");

                Tested = true;
                throw new ArgumentNullException("input");
            }
        }
    }
}
