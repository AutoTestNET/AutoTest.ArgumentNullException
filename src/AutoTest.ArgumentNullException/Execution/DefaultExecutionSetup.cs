namespace AutoTest.ArgNullEx.Execution
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// The default <see cref="IExecutionSetup"/>.
    /// </summary>
    public class DefaultExecutionSetup : IExecutionSetup
    {
        /// <summary>
        /// A singleton completed task.
        /// </summary>
        private static readonly Task CompletedTask = Task.FromResult(0);

        /// <summary>
        /// The method information.
        /// </summary>
        private MethodInfo _methodUnderTest;

        /// <summary>
        /// The parameters to the <see cref="_methodUnderTest"/>.
        /// </summary>
        private object[] _parameters;

        /// <summary>
        /// The system under tests, can be <c>null</c> if the <see cref="_methodUnderTest"/> is static.
        /// </summary>
        private object _sut;

        /// <summary>
        /// Gets the method information.
        /// </summary>
        public MethodInfo MethodUnderTest
        {
            get { return _methodUnderTest; }
        }

        /// <summary>
        /// Gets the parameters to the <see cref="MethodUnderTest"/>.
        /// </summary>
        public IEnumerable<object> Parameters
        {
            get { return _parameters; }
        }

        /// <summary>
        /// Gets the system under tests, can be <c>null</c> if the <see cref="MethodUnderTest"/> is static.
        /// </summary>
        public object Sut
        {
            get { return _sut; }
        }

        /// <summary>
        /// Sets up a reflected asynchronous <see cref="MethodInfo"/> execution.
        /// </summary>
        /// <param name="methodData">The method data.</param>
        /// <returns>A reflected asynchronous <see cref="MethodInfo"/> execution.</returns>
        Func<Task> IExecutionSetup.Setup(MethodData methodData)
        {
            if (methodData == null) throw new ArgumentNullException("methodData");
            if (methodData.MethodUnderTest == null) throw new ArgumentException("The methodData.MethodUnderTest is null.", "methodData");
            if (methodData.Arguments == null) throw new ArgumentException("The methodData.Arguments is null.", "methodData");

            _methodUnderTest = methodData.MethodUnderTest;
            _parameters = methodData.Arguments;
            _sut = methodData.InstanceUnderTest;

            return Execute;
        }

        /// <summary>
        /// Executes a reflected <see cref="MethodBase"/>.
        /// </summary>
        /// <returns>A task representing the asynchronous execution of a <see cref="MethodBase"/>.</returns>
        private Task Execute()
        {
            if (typeof(Task).IsAssignableFrom(_methodUnderTest.ReturnType))
            {
                return ExecuteAsynchronously();
            }

            ExecuteSynchronously();
            return CompletedTask;
        }

        /// <summary>
        /// Executes the <see cref="_methodUnderTest"/> synchronously.
        /// </summary>
        private void ExecuteSynchronously()
        {
            try
            {
                _methodUnderTest.Invoke(_sut, _parameters);
            }
            catch (TargetInvocationException targetInvocationException)
            {
                if (targetInvocationException.InnerException == null) throw;

                throw targetInvocationException.InnerException;
            }
        }

        /// <summary>
        /// Executes the <see cref="_methodUnderTest"/> synchronously.
        /// </summary>
        /// <returns>The <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        private async Task ExecuteAsynchronously()
        {
            try
            {
                var result = (Task)_methodUnderTest.Invoke(_sut, _parameters);
                await result;
            }
            catch (TargetInvocationException targetInvocationException)
            {
                if (targetInvocationException.InnerException == null) throw;

                throw targetInvocationException.InnerException;
            }
        }
    }
}
