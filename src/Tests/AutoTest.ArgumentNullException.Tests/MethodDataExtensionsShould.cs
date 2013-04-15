﻿namespace AutoTest.ArgNullEx
{
    using System;
    using System.Threading.Tasks;
    using Moq;
    using Ploeh.AutoFixture.Xunit;
    using Xunit;
    using Xunit.Extensions;
    using Xunit.Sdk;

    public class MethodDataExtensionsShould
    {
        /// <summary>
        /// Returns a completed task.
        /// </summary>
        /// <returns>A completed task.</returns>
        private static Task CompletedTask()
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// Returns a completed task.
        /// </summary>
        /// <returns>A completed task.</returns>
        private static Task ExceptionTask(Exception exception)
        {
            if (exception == null) throw new ArgumentNullException("exception");

            var tcs = new TaskCompletionSource<int>();
            tcs.SetException(exception);
            return tcs.Task;
        }

        [Theory, AutoMock]
        public Task DoNothingWhenCorrectArgumentNullExceptionThrown(
            [Frozen] Mock<IExecutionSetup> executionSetupMock,
            MethodData method)
        {
            // Arrange
            executionSetupMock
                .Setup(es => es.Setup(method))
                .Returns(() => ExceptionTask(new ArgumentNullException(method.NullArgument)));

            // Act
            return method.Execute();
        }

        [Theory, AutoMock]
        public void ThrowIfNoExceptionThrown(
            [Frozen] Mock<IExecutionSetup> executionSetupMock,
            MethodData method)
        {
            // Arrange
            executionSetupMock
                .Setup(es => es.Setup(method))
                .Returns(() => CompletedTask());

            // Act/Assert
            Exception innerException = Assert.Throws<AggregateException>(() => method.Execute().Wait()).InnerException;
            Assert.IsType<ThrowsException>(innerException);
        }

        [Theory, AutoMock]
        public void ThrowIfWrongExceptionThrown(
            [Frozen] Mock<IExecutionSetup> executionSetupMock,
            MethodData method)
        {
            // Arrange
            executionSetupMock
                .Setup(es => es.Setup(method))
                .Returns(() => ExceptionTask(new Exception(method.ToString())));

            // Act/Assert
            Exception innerException = Assert.Throws<AggregateException>(() => method.Execute().Wait()).InnerException;
            Assert.IsType<ThrowsException>(innerException);
        }

        [Theory, AutoMock]
        public void ThrowIfWrongArgumentNullExceptionThrown(
            [Frozen] Mock<IExecutionSetup> executionSetupMock,
            MethodData method)
        {
            // Arrange
            executionSetupMock
                .Setup(es => es.Setup(method))
                .Returns(() => ExceptionTask(new ArgumentNullException(Guid.NewGuid().ToString())));

            // Act/Assert
            Exception innerException = Assert.Throws<AggregateException>(() => method.Execute().Wait()).InnerException;
            Assert.IsType<EqualException>(innerException);
        }
    }
}
