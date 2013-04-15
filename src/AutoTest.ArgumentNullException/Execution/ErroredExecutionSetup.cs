namespace AutoTest.ArgNullEx.Execution
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// The <see cref="IExecutionSetup"/> used to throw a setup error.
    /// </summary>
    public class ErroredExecutionSetup : IExecutionSetup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErroredExecutionSetup"/> class.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to throw when executed.</param>
        public ErroredExecutionSetup(Exception exception)
        {
            if (exception == null) throw new ArgumentNullException("exception");

            Exception = exception;
        }

        /// <summary>
        /// Gets the <see cref="Exception"/> to throw when executed.
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Sets up an errored execute setup.
        /// </summary>
        /// <param name="methodData">The method data.</param>
        /// <returns>A <see cref="Func{T}"/> returning an errored <see cref="Task"/> setup with the <see cref="Exception"/>.</returns>
        Func<Task> IExecutionSetup.Setup(MethodData methodData)
        {
            if (methodData == null) throw new ArgumentNullException("methodData");

            return Execute;
        }

        /// <summary>
        /// Returns an errored <see cref="Task"/> setup with the <see cref="Exception"/>.
        /// </summary>
        /// <returns>An errored <see cref="Task"/> setup with the <see cref="Exception"/>.</returns>
        private Task Execute()
        {
            var tcs = new TaskCompletionSource<int>();
            tcs.SetException(Exception);
            return tcs.Task;
        }
    }
}
