namespace AutoTest.ExampleLibrary.Issues.Issue015
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Test class used to demonstrate issue 15
    /// https://github.com/AutoTestNET/AutoTest.ArgumentNullException/issues/15
    /// </summary>
    public class OtherEquals
    {
#pragma warning disable 108,114
        public bool Equals(object obj)
#pragma warning restore 108,114
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            return false;
        }

        public bool Equals(OtherEquals obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            return false;
        }

        public bool Equals(string other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return false;
        }

        public bool Equals(string x, string y)
        {
            if (x == null)
                throw new ArgumentNullException(nameof(x));
            if (y == null)
                throw new ArgumentNullException(nameof(y));

            return false;
        }
    }
}
