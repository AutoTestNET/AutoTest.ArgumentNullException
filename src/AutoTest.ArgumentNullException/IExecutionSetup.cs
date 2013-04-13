namespace AutoTest.ArgNullEx
{
    using System.Reflection;

    /// <summary>
    /// Defined a setup for a reflected asynchronous <see cref="MethodInfo"/> execution.
    /// </summary>
    public interface IExecutionSetup
    {
        /// <summary>
        /// Sets up a reflected asynchronous <see cref="MethodInfo"/> execution.
        /// </summary>
        /// <param name="methodData">The method data.</param>
        /// <returns>A reflected asynchronous <see cref="MethodInfo"/> execution.</returns>
        IExecution Setup(MethodData methodData);
    }
}
