namespace AutoTest.ExampleLibrary.Issues.Issue031
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Test class used to demonstrate pull request 31
    /// https://github.com/AutoTestNET/AutoTest.ArgumentNullException/pull/31
    /// </summary>
    public class SomeNullableValueTypeParameters
    {
        /// <summary>
        /// Gets a value indicating if the <see cref="SomeNullableValueTypeParametersMethod"/> stringInput parameter
        /// has been tested.
        /// </summary>
        public static bool StringInputTested { get; private set; }

        private static void SomeNullableValueTypeParametersMethod(
            int intInput,
            string stringInput,
            Guid guidInput,
            int? intNullable,
            Guid? guidNullable)
        {
            StringInputTested = false;

            if (stringInput == null)
            {
                StringInputTested = true;
                throw new ArgumentNullException(nameof(stringInput));
            }

            throw new Exception("Shouldn't ever get here.");
        }
    }
}
