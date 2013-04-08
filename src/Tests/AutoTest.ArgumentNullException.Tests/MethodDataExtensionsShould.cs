namespace AutoTest.ArgNullEx
{
    using System;
    using System.Threading.Tasks;
    using Moq;
    using Xunit;
    using Xunit.Extensions;
    using Xunit.Sdk;

    /// <summary>
    /// Interface to provide a way of mocking the executing actions of <see cref="MethodData"/>.
    /// </summary>
    public interface IExecuteMethods
    {
        /// <summary>
        /// The executing action if the <see cref="MethodData.MethodUnderTest"/> is synchronous; otherwise <c>null</c> if asynchronous.
        /// </summary>
        void ExecutingActionSync();

        /// <summary>
        /// The executing action if the <see cref="MethodData.MethodUnderTest"/> is asynchronous; otherwise <c>null</c> if synchronous.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task ExecutingActionAsync();
    }

    public class MethodDataExtensionsShould
    {
        [Theory, AutoMock]
        public void ThrowsWhenExecutingActionAreNull(
            MethodData method)
        {
            // Arrange
            method.ExecutingActionSync = null;
            method.ExecutingActionAsync = null;

            // Act/Assert
            Exception innerException = Assert.Throws<AggregateException>(() => method.Execute().Wait()).InnerException;
            string actualParamName = Assert.IsType<ArgumentException>(innerException).ParamName;
            Assert.Equal("method", actualParamName);
        }
    }

    public class MethodDataExtensionsSynchronousShould
    {
        [Theory, AutoMock]
        public Task DoNothingWhenCorrectArgumentNullExceptionThrown(
            MethodData method,
            Mock<IExecuteMethods> executeMock)
        {
            // Arrange
            executeMock.Setup(e => e.ExecutingActionSync()).Throws(new ArgumentNullException(method.NullArgument));
            method.ExecutingActionSync = executeMock.Object.ExecutingActionSync;
            method.ExecutingActionAsync = null;

            // Act
            return method.Execute();
        }

        [Theory, AutoMock]
        public void ThrowIfNoExceptionThrown(
            MethodData method)
        {
            // Arrange
            method.ExecutingActionAsync = null;

            // Act/Assert
            Exception innerException = Assert.Throws<AggregateException>(() => method.Execute().Wait()).InnerException;
            Assert.IsType<ThrowsException>(innerException);
        }

        [Theory, AutoMock]
        public void ThrowIfWrongExceptionThrown(
            MethodData method,
            Mock<IExecuteMethods> executeMock)
        {
            // Arrange
            executeMock.Setup(e => e.ExecutingActionSync()).Throws(new Exception(method.ToString()));
            method.ExecutingActionSync = executeMock.Object.ExecutingActionSync;
            method.ExecutingActionAsync = null;

            // Act/Assert
            Exception innerException = Assert.Throws<AggregateException>(() => method.Execute().Wait()).InnerException;
            Assert.IsType<ThrowsException>(innerException);
        }

        [Theory, AutoMock]
        public void ThrowIfWrongArgumentNullExceptionThrown(
            MethodData method,
            Mock<IExecuteMethods> executeMock)
        {
            // Arrange
            executeMock.Setup(e => e.ExecutingActionSync()).Throws(new ArgumentNullException(Guid.NewGuid().ToString()));
            method.ExecutingActionSync = executeMock.Object.ExecutingActionSync;
            method.ExecutingActionAsync = null;

            // Act/Assert
            Exception innerException = Assert.Throws<AggregateException>(() => method.Execute().Wait()).InnerException;
            Assert.IsType<EqualException>(innerException);
        }
    }

    public class MethodDataExtensionsAsynchronousShould
    {
        [Theory, AutoMock]
        public Task DoNothingWhenCorrectArgumentNullExceptionThrown(
            MethodData method,
            Mock<IExecuteMethods> executeMock)
        {
            // Arrange
            var tcs = new TaskCompletionSource<int>();
            tcs.SetException(new ArgumentNullException(method.NullArgument));
            executeMock.Setup(e => e.ExecutingActionAsync()).Returns(tcs.Task);
            method.ExecutingActionAsync = executeMock.Object.ExecutingActionAsync;
            method.ExecutingActionSync = null;

            // Act
            return method.Execute();
        }

        [Theory, AutoMock]
        public void ThrowIfNoExceptionThrown(
            MethodData method)
        {
            // Arrange
            method.ExecutingActionSync = null;

            // Act/Assert
            Exception innerException = Assert.Throws<AggregateException>(() => method.Execute().Wait()).InnerException;
            Assert.IsType<ThrowsException>(innerException);
        }

        [Theory, AutoMock]
        public void ThrowIfWrongExceptionThrown(
            MethodData method,
            Mock<IExecuteMethods> executeMock)
        {
            // Arrange
            var tcs = new TaskCompletionSource<int>();
            tcs.SetException(new Exception(method.ToString()));
            executeMock.Setup(e => e.ExecutingActionAsync()).Returns(tcs.Task);
            method.ExecutingActionAsync = executeMock.Object.ExecutingActionAsync;
            method.ExecutingActionSync = null;

            // Act/Assert
            Exception innerException = Assert.Throws<AggregateException>(() => method.Execute().Wait()).InnerException;
            Assert.IsType<ThrowsException>(innerException);
        }

        [Theory, AutoMock]
        public void ThrowIfWrongArgumentNullExceptionThrown(
            MethodData method,
            Mock<IExecuteMethods> executeMock)
        {
            // Arrange
            var tcs = new TaskCompletionSource<int>();
            tcs.SetException(new ArgumentNullException(Guid.NewGuid().ToString()));
            executeMock.Setup(e => e.ExecutingActionAsync()).Returns(tcs.Task);
            method.ExecutingActionAsync = executeMock.Object.ExecutingActionAsync;
            method.ExecutingActionSync = null;

            // Act/Assert
            Exception innerException = Assert.Throws<AggregateException>(() => method.Execute().Wait()).InnerException;
            Assert.IsType<EqualException>(innerException);
        }
    }
}
