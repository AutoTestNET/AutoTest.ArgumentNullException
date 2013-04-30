namespace AutoTest.ArgNullEx.Execution
{
    using System;
    using System.Threading.Tasks;
    using Ploeh.AutoFixture.Xunit;
    using global::Xunit;
    using global::Xunit.Extensions;

    public class ErroredExecutionSetupShould
    {
        [Theory, AutoMock]
        public void InitialiseException(
            [Frozen(As = typeof(Exception))] InvalidOperationException expected,
            ErroredExecutionSetup sut)
        {
            Assert.Same(expected, sut.Exception);
        }

        [Theory, AutoMock]
        public async Task SetupThrowingExecution(
            [Frozen(As = typeof(Exception))] ArgumentException expected,
            ErroredExecutionSetup sut,
            MethodData methodData)
        {
            // Act
            Func<Task> execute = ((IExecutionSetup) sut).Setup(methodData);

            // Executing method should not throw but return a faulted task.
            Task task = execute();
            Assert.True(task.IsFaulted);

            try
            {
                await task;
            }
            catch (ArgumentException actual)
            {
                Assert.Same(expected, actual);
                return;
            }

            // To get here is an error.
            Assert.Throws<ArgumentException>(() => { });
        }
    }
}
