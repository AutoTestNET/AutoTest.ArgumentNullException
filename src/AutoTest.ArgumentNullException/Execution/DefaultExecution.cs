namespace AutoTest.ArgNullEx.Execution
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// The default <see cref="IExecution"/>.
    /// </summary>
    public class DefaultExecution : IExecution
    {
        /// <summary>
        /// A singleton completed task.
        /// </summary>
        private static readonly Task CompletedTask = Task.FromResult(0);

        /// <summary>
        /// The method information.
        /// </summary>
        private readonly MethodInfo _methodUnderTest;

        /// <summary>
        /// The parameters to the <see cref="_methodUnderTest"/>.
        /// </summary>
        private readonly object[] _parameters;

        /// <summary>
        /// The system under tests, can be <c>null</c> if the <see cref="_methodUnderTest"/> is static.
        /// </summary>
        private readonly object _sut;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultExecution"/> class.
        /// </summary>
        /// <param name="methodUnderTest">The method information.</param>
        /// <param name="parameters">The parameters to the <paramref name="methodUnderTest"/>.</param>
        /// <param name="sut">The system under tests, can be <c>null</c> if the <paramref name="methodUnderTest"/> is static.</param>
        public DefaultExecution(MethodInfo methodUnderTest, object[] parameters, object sut = null)
        {
            if (methodUnderTest == null) throw new ArgumentNullException("methodUnderTest");
            if (parameters == null) throw new ArgumentNullException("parameters");

            _methodUnderTest = methodUnderTest;
            _parameters = parameters;
            _sut = sut;
        }

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
        /// Executes a reflected <see cref="MethodBase"/>.
        /// </summary>
        /// <returns>A task representing the asynchronous execution of a <see cref="MethodBase"/>.</returns>
        Task IExecution.Execute()
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
