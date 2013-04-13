namespace AutoTest.ArgNullEx.Execution
{
    using System;
    using System.Reflection;

    /// <summary>
    /// The default <see cref="IExecutionSetup"/>.
    /// </summary>
    public class DefaultExecutionSetup : IExecutionSetup
    {
        /// <summary>
        /// Sets up a reflected asynchronous <see cref="MethodInfo"/> execution.
        /// </summary>
        /// <param name="methodData">The method data.</param>
        /// <returns>A reflected asynchronous <see cref="MethodInfo"/> execution.</returns>
        IExecution IExecutionSetup.Setup(MethodData methodData)
        {
            if (methodData == null) throw new ArgumentNullException("methodData");

            return new DefaultExecution(methodData.MethodUnderTest, methodData.Arguments, methodData.InstanceUnderTest);
        }
    }
}
