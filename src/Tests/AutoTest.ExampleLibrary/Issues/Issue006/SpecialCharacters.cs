namespace AutoTest.ExampleLibrary.Issues.Issue006
{
    using System;

    /// <summary>
    /// Test class used to demonstrate issue 6
    /// https://github.com/AutoTestNET/AutoTest.ArgumentNullException/issues/6
    /// </summary>
    public class SpecialCharacters
    {
        public class InnerClass
        {
            static void AMethod(object input)
            {
                if (input == null)
                    throw new ArgumentNullException("input");
            }
        }
    }
}
