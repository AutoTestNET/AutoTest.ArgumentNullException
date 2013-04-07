namespace AutoTest.ArgumentNullException
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// The data representing a single instance of a <see cref="ArgumentNullException"/> use case test.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class MethodData
    {
        /// <summary>
        /// The type of the class under test.
        /// </summary>
        public Type ClassUnderTest;

        /// <summary>
        /// The instance of the class under test if the <see cref="MethodUnderTest"/> is not static.
        /// </summary>
        public object InstanceUnderTest;

        /// <summary>
        /// The method under test.
        /// </summary>
        public MethodInfo MethodUnderTest;

        /// <summary>
        /// The arguments to the method under test.
        /// </summary>
        public object[] Arguments;

        /// <summary>
        /// The name of the null argument.
        /// </summary>
        public string NullArgument;

        /// <summary>
        /// The index of the null argument in the <see cref="Arguments"/>.
        /// </summary>
        public int NullIndex;

        /// <summary>
        /// The executing action if the <see cref="MethodUnderTest"/> is synchronous; otherwise <c>null</c> if asynchronous.
        /// </summary>
        public Action ExecutingActionSync;

        /// <summary>
        /// The executing action if the <see cref="MethodUnderTest"/> is asynchronous; otherwise <c>null</c> if synchronous.
        /// </summary>
        public Func<Task> ExecutingActionAsync;

        /// <summary>
        /// Gets the test to display within the debugger.
        /// </summary>
        private string DebuggerDisplay
        {
            get { return "MethodData: " + ToString(); }
        }

        /// <summary>
        /// Returns a human readable representation of the <see cref="MethodData"/>.
        /// </summary>
        /// <returns>A human readable representation of the <see cref="MethodData"/>.</returns>
        public override string ToString()
        {
            return string.Format("{0}.{1} {2}", ClassUnderTest.Name, MethodUnderTest.Name, NullArgument);
        }
    }
}
