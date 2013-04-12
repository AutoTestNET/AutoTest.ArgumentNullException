namespace AutoTest.ArgNullEx
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
        /// Initializes a new instance of the <see cref="MethodData"/> class.
        /// </summary>
        /// <param name="classUnderTest">The type of the class under test.</param>
        /// <param name="instanceUnderTest">The instance of the class under test if the <paramref name="methodUnderTest"/> is not static.</param>
        /// <param name="methodUnderTest">The method under test.</param>
        /// <param name="arguments">The arguments to the <paramref name="methodUnderTest"/>.</param>
        /// <param name="nullArgument">The name of the null argument in the <paramref name="arguments"/>.</param>
        /// <param name="nullIndex">The index of the null argument in the <paramref name="arguments"/>.</param>
        /// <param name="executingActionSync">The executing action if the <paramref name="methodUnderTest"/> is synchronous.</param>
        public MethodData(
            Type classUnderTest,
            object instanceUnderTest,
            MethodInfo methodUnderTest,
            object[] arguments,
            string nullArgument,
            int nullIndex,
            Action executingActionSync)
            : this(classUnderTest, instanceUnderTest, methodUnderTest, arguments, nullArgument, nullIndex)
        {
            if (executingActionSync == null) throw new ArgumentNullException("executingActionSync");

            ExecutingActionSync = executingActionSync;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodData"/> class.
        /// </summary>
        /// <param name="classUnderTest">The type of the class under test.</param>
        /// <param name="instanceUnderTest">The instance of the class under test if the <paramref name="methodUnderTest"/> is not static.</param>
        /// <param name="methodUnderTest">The method under test.</param>
        /// <param name="arguments">The arguments to the <paramref name="methodUnderTest"/>.</param>
        /// <param name="nullArgument">The name of the null argument in the <paramref name="arguments"/>.</param>
        /// <param name="nullIndex">The index of the null argument in the <paramref name="arguments"/>.</param>
        /// <param name="executingActionAsync">The executing action if the <paramref name="methodUnderTest"/> is asynchronous.</param>
        public MethodData(
            Type classUnderTest,
            object instanceUnderTest,
            MethodInfo methodUnderTest,
            object[] arguments,
            string nullArgument,
            int nullIndex,
            Func<Task> executingActionAsync)
            : this(classUnderTest, instanceUnderTest, methodUnderTest, arguments, nullArgument, nullIndex)
        {
            if (executingActionAsync == null) throw new ArgumentNullException("executingActionAsync");

            ExecutingActionAsync = executingActionAsync;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodData"/> class.
        /// </summary>
        /// <param name="classUnderTest">The type of the class under test.</param>
        /// <param name="instanceUnderTest">The instance of the class under test if the <paramref name="methodUnderTest"/> is not static.</param>
        /// <param name="methodUnderTest">The method under test.</param>
        /// <param name="arguments">The arguments to the <paramref name="methodUnderTest"/>.</param>
        /// <param name="nullArgument">The name of the null argument in the <paramref name="arguments"/>.</param>
        /// <param name="nullIndex">The index of the null argument in the <paramref name="arguments"/>.</param>
        private MethodData(
            Type classUnderTest,
            object instanceUnderTest,
            MethodInfo methodUnderTest,
            object[] arguments,
            string nullArgument,
            int nullIndex)
        {
            ClassUnderTest = classUnderTest;
            InstanceUnderTest = instanceUnderTest;
            MethodUnderTest = methodUnderTest;
            Arguments = arguments;
            NullArgument = nullArgument;
            NullIndex = nullIndex;
        }

        /// <summary>
        /// Gets or sets the type of the class under test.
        /// </summary>
        public Type ClassUnderTest { get; set; }

        /// <summary>
        /// Gets or sets the instance of the class under test if the <see cref="MethodUnderTest"/> is not static.
        /// </summary>
        public object InstanceUnderTest { get; set; }

        /// <summary>
        /// Gets or sets the method under test.
        /// </summary>
        public MethodInfo MethodUnderTest { get; set; }

        /// <summary>
        /// Gets or sets the arguments to the <see cref="MethodUnderTest"/>.
        /// </summary>
        public object[] Arguments { get; set; }

        /// <summary>
        /// Gets or sets the name of the null argument in the <see cref="Arguments"/>.
        /// </summary>
        public string NullArgument { get; set; }

        /// <summary>
        /// Gets or sets the index of the null argument in the <see cref="Arguments"/>.
        /// </summary>
        public int NullIndex { get; set; }

        /// <summary>
        /// Gets or sets the executing action if the <see cref="MethodUnderTest"/> is synchronous; otherwise <c>null</c> if asynchronous.
        /// </summary>
        public Action ExecutingActionSync { get; set; }

        /// <summary>
        /// Gets or sets the executing action if the <see cref="MethodUnderTest"/> is asynchronous; otherwise <c>null</c> if synchronous.
        /// </summary>
        public Func<Task> ExecutingActionAsync { get; set; }

        /// <summary>
        /// Gets the test to display within the debugger.
        /// </summary>
// ReSharper disable UnusedMember.Local
        private string DebuggerDisplay
        {
            get { return "MethodData: " + ToString(); }
        }
// ReSharper restore UnusedMember.Local

        /// <summary>
        /// Returns a human readable representation of the <see cref="MethodData"/>.
        /// </summary>
        /// <returns>A human readable representation of the <see cref="MethodData"/>.</returns>
        public override string ToString()
        {
            return string.Format("{0}.{1} {2}=null", ClassUnderTest.Name, MethodUnderTest.Name, NullArgument);
        }
    }
}
