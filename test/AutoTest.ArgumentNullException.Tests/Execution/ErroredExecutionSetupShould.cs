﻿namespace AutoTest.ArgNullEx.Execution
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture.Xunit2;
    using global::Xunit;

    public class ErroredExecutionSetupShould
    {
        public class MyException : Exception
        {
        }

        [Theory, AutoMock]
        public void InitialiseException(
            [Frozen(Matching.DirectBaseType)] MyException expected,
            ErroredExecutionSetup sut)
        {
            Assert.Same(expected, sut.Exception);
        }

        [Theory, AutoMock]
        public async Task SetupThrowingExecution(
            [Frozen(Matching.DirectBaseType)] MyException expected,
            ErroredExecutionSetup sut,
            MethodData methodData)
        {
            // Act
            Func<Task> execute = ((IExecutionSetup) sut).Setup(methodData);

            // Executing method should not throw but return a faulted task.
            Task task = execute();

            // Assert
            Assert.True(task.IsFaulted);
            MyException actual = await Assert.ThrowsAsync<MyException>(() => task).ConfigureAwait(false);
            Assert.Same(expected, actual);
        }
    }
}
