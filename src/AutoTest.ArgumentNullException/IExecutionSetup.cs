namespace AutoTest.ArgNullEx
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

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
        Func<Task> Setup(MethodData methodData);
    }
}
