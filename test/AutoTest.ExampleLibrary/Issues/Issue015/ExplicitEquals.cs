namespace AutoTest.ExampleLibrary.Issues.Issue015
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Test class used to demonstrate issue 15
    /// https://github.com/AutoTestNET/AutoTest.ArgumentNullException/issues/15
    /// </summary>
    public class ExplicitEquals : IEquatable<string>, IEqualityComparer<string>, IEqualityComparer<object>, IDummy
    {
        /// <summary>
        /// Gets a value indicating if the <see cref="ExplicitEquals"/> has been tested.
        /// </summary>
        public static bool TestedStringValue1 { get; private set; }

        /// <summary>
        /// Gets a value indicating if the <see cref="ExplicitEquals"/> has been tested.
        /// </summary>
        public static bool TestedStringValue2 { get; private set; }

        /// <summary>
        /// Gets a value indicating if the <see cref="ExplicitEquals"/> has been tested.
        /// </summary>
        public static bool TestedStringValue3 { get; private set; }

        bool IEquatable<string>.Equals(string other)
        {
            return false;
        }

        bool IEqualityComparer<string>.Equals(string x, string y)
        {
            return false;
        }

        int IEqualityComparer<string>.GetHashCode(string obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            return obj.GetHashCode();
        }

        bool IEqualityComparer<object>.Equals(object x, object y)
        {
            return false;
        }

        int IEqualityComparer<object>.GetHashCode(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            return obj.GetHashCode();
        }

        private bool Equals(string stringValue1, string stringValue2)
        {
            TestedStringValue1 = TestedStringValue2 = false;

            if (stringValue1 == null)
            {
                TestedStringValue1 = true;
                throw new ArgumentNullException("stringValue1");
            }
            if (stringValue2 == null)
            {
                TestedStringValue2 = true;
                throw new ArgumentNullException("stringValue2");
            }

            throw new Exception("Shouldn't ever get here.");
        }

        private bool Equals(string stringValue3)
        {
            TestedStringValue3 = false;

            if (stringValue3 != null)
            {
                throw new Exception("Shouldn't ever get here.");
            }

            TestedStringValue3 = true;
            throw new ArgumentNullException("stringValue3");
        }
    }
}
