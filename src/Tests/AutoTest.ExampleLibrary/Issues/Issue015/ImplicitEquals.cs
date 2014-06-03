namespace AutoTest.ExampleLibrary.Issues.Issue015
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Dummy interface to ensure checks are made for <see cref="Type.IsGenericType"/> when checking Equals.
    /// </summary>
    public interface IDummy
    {
    }

    public class ImplicitEquals : IEquatable<string>, IEqualityComparer<string>, IDummy
    {
        /// <summary>
        /// Gets a value indicating if the <see cref="ImplicitEquals"/> has been tested.
        /// </summary>
        public static bool Tested { get; private set; }

        public override bool Equals(object obj)
        {
            return RuntimeHelpers.Equals(this, obj);
        }

        public override int GetHashCode()
        {
            return RuntimeHelpers.GetHashCode(this);
        }

        public bool Equals(string other)
        {
            return false;
        }

        public bool Equals(string x, string y)
        {
            return false;
        }

        public int GetHashCode(string obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            return obj.GetHashCode();
        }

        private void Equals(string stringValue, int intValue)
        {
            Tested = false;

            if (stringValue != null)
            {
                throw new Exception("Shouldn't ever get here.");
            }

            Tested = true;
            throw new ArgumentNullException("stringValue");
        }
    }
}
