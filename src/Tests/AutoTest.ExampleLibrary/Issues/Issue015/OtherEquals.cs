namespace AutoTest.ExampleLibrary.Issues.Issue015
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class OtherEquals
    {
#pragma warning disable 108,114
        public bool Equals(object obj)
#pragma warning restore 108,114
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            return false;
        }

        public bool Equals(OtherEquals obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            return false;
        }

        public bool Equals(string other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            return false;
        }

        public bool Equals(string x, string y)
        {
            if (x == null)
                throw new ArgumentNullException("x");
            if (y == null)
                throw new ArgumentNullException("y");

            return false;
        }
    }
}
