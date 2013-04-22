namespace AutoTest.ArgNullEx
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading.Tasks;
    using AutoTest.ArgNullEx.Execution;

    /// <summary>
    /// The data representing a single instance of a <see cref="ArgumentNullException"/> use case test.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class MethodData
    {
        /// <summary>
        /// The setup for the <see cref="ExecuteAction"/>.
        /// </summary>
        private readonly IExecutionSetup _executionSetup;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodData"/> class.
        /// </summary>
        /// <param name="classUnderTest">The type of the class under test.</param>
        /// <param name="instanceUnderTest">The instance of the class under test if the <paramref name="methodUnderTest"/> is not static.</param>
        /// <param name="methodUnderTest">The method under test.</param>
        /// <param name="parameters">The parameters to the <paramref name="methodUnderTest"/>.</param>
        /// <param name="nullParameter">The name of the null parameter in the <paramref name="parameters"/>.</param>
        /// <param name="nullIndex">The index of the null parameter in the <paramref name="parameters"/>.</param>
        /// <param name="executionSetup">The setup for the <see cref="ExecuteAction"/>.</param>
        public MethodData(
            Type classUnderTest,
            object instanceUnderTest,
            MethodBase methodUnderTest,
            object[] parameters,
            string nullParameter,
            int nullIndex,
            IExecutionSetup executionSetup)
        {
            if (classUnderTest == null) throw new ArgumentNullException("classUnderTest");
            if (methodUnderTest == null) throw new ArgumentNullException("methodUnderTest");
            if (parameters == null) throw new ArgumentNullException("parameters");
            if (nullParameter == null) throw new ArgumentNullException("nullParameter");
            if (executionSetup == null) throw new ArgumentNullException("executionSetup");

            ClassUnderTest = classUnderTest;
            InstanceUnderTest = instanceUnderTest;
            MethodUnderTest = methodUnderTest;
            Parameters = parameters;
            NullParameter = nullParameter;
            NullIndex = nullIndex;
            _executionSetup = executionSetup;
        }

        /// <summary>
        /// Gets the type of the class under test.
        /// </summary>
        public Type ClassUnderTest { get; private set; }

        /// <summary>
        /// Gets the instance of the class under test if the <see cref="MethodUnderTest"/> is not static.
        /// </summary>
        public object InstanceUnderTest { get; private set; }

        /// <summary>
        /// Gets the method under test.
        /// </summary>
        public MethodBase MethodUnderTest { get; private set; }

        /// <summary>
        /// Gets the parameters to the <see cref="MethodUnderTest"/>.
        /// </summary>
        public object[] Parameters { get; private set; }

        /// <summary>
        /// Gets the name of the null parameter in the <see cref="Parameters"/>.
        /// </summary>
        public string NullParameter { get; private set; }

        /// <summary>
        /// Gets the index of the null parameter in the <see cref="Parameters"/>.
        /// </summary>
        public int NullIndex { get; private set; }

        /// <summary>
        /// Gets the text to display within the debugger.
        /// </summary>
// ReSharper disable UnusedMember.Local
        private string DebuggerDisplay
        {
            get { return "MethodData: " + ToString(); }
        }
// ReSharper restore UnusedMember.Local

        /// <summary>
        /// Executes the action for the <see cref="MethodUnderTest"/>.
        /// </summary>
        /// <returns>The <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task ExecuteAction()
        {
            return _executionSetup.Setup(this)();
        }

        /// <summary>
        /// Returns a human readable representation of the <see cref="MethodData"/>.
        /// </summary>
        /// <returns>A human readable representation of the <see cref="MethodData"/>.</returns>
        public override string ToString()
        {
            return string.Format("{0}.{1} {2}=null", ClassUnderTest.Name, MethodUnderTest.Name, NullParameter);
        }
    }
}
