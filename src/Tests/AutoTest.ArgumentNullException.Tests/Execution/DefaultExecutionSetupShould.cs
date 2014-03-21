namespace AutoTest.ArgNullEx.Execution
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using global::Xunit;
    using global::Xunit.Extensions;

    public class DefaultExecutionSetupShould
    {
        private static MethodData GetMethodData(MethodBase methodUnderTest, IExecutionSetup sut, object instanceUnderTest = null)
        {
            return new MethodData(
                typeof(DefaultExecutionSetupShould),
                instanceUnderTest,
                methodUnderTest,
                new object[] {},
                Guid.NewGuid().ToString(),
                0,
                sut);
        }

        private async Task AssertExecutionThrows<T>(MethodData methodData)
            where T : Exception
        {
            // Act
            // This should never throw but always return a task, either completed or faulted.
            Task executeAction = methodData.ExecuteAction();

            // Assert
            try
            {
                await executeAction;
            }
            catch (T)
            {
                return;
            }

            Assert.Throws<T>(() => { });
        }

        [Theory, AutoMock]
        public void InitializeWhenSetup(
            MethodData methodData,
            DefaultExecutionSetup sut)
        {
            // Act
            Func<Task> execute = ((IExecutionSetup)sut).Setup(methodData);

            // Assert
            Assert.NotNull(execute);
            Assert.Same(methodData.MethodUnderTest, sut.MethodUnderTest);
            Assert.Same(methodData.Parameters, sut.Parameters);
            Assert.Same(methodData.InstanceUnderTest, sut.Sut);
        }

        [Theory, AutoMock]
        public async Task ExecuteContructor(DefaultExecutionSetup sut)
        {
            // Arrange
            MethodData methodData =
                GetMethodData(
                    typeof (DefaultExecutionSetupShould).GetConstructor(Type.EmptyTypes),
                    sut,
                    new DefaultExecutionSetupShould());

            // Act
            await methodData.ExecuteAction();
        }

        [Theory, AutoMock]
        public async Task ExecuteSynchronousMethod(DefaultExecutionSetup sut)
        {
            // Arrange
            Action action = () => { };
            MethodData methodData = GetMethodData(action.Method, sut);

            // Act
            await methodData.ExecuteAction();
        }

        [Theory, AutoMock]
        public async Task ExecuteThrowingSynchronousMethod(DefaultExecutionSetup sut)
        {
            // Arrange
            Action action = () => { throw new FileLoadException("Some random message " + Guid.NewGuid()); };

            // Act/Assert
            await AssertExecutionThrows<FileLoadException>(GetMethodData(action.Method, sut));
        }

        [Theory, AutoMock]
        public async Task ExecuteAsynchronousMethod(DefaultExecutionSetup sut)
        {
            // Arrange
            Func<Task> action = async () => { await Task.Yield(); };
            MethodData methodData = GetMethodData(action.Method, sut);

            // Act
            await methodData.ExecuteAction();
        }

        [Theory, AutoMock]
        public async Task ExecuteAsynchronousThrowingMethod(DefaultExecutionSetup sut)
        {
            // Arrange
            Func<Task> action = async () =>
                {
                    await Task.Yield();
                    throw new FileNotFoundException("Some random message " + Guid.NewGuid());
                };

            // AAA
            await AssertExecutionThrows<FileNotFoundException>(GetMethodData(action.Method, sut));
        }

        [Theory, AutoMock]
        public async Task ExecuteAsynchronousMethodThatThrowsSynchronously(DefaultExecutionSetup sut)
        {
            // Arrange
            Func<Task> action = () => { throw new FieldAccessException("Some random message " + Guid.NewGuid()); };

            // Act/Assert
            await AssertExecutionThrows<FieldAccessException>(GetMethodData(action.Method, sut));
        }
    }
}
