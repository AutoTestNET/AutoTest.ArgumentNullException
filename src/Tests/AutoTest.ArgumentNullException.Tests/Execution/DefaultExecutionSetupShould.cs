namespace AutoTest.ArgNullEx.Execution
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using global::Xunit;
    using global::Xunit.Extensions;

    public class DefaultExecutionSetupShould
    {
        private static MethodData GetStaticMethodData(string methodName, IExecutionSetup sut)
        {
            Type type = typeof(DefaultExecutionSetupShould);
            MethodInfo methodUnderTest = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);

            return new MethodData(
                type,
                null,
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
        public void ShouldInitializeWhenSetup(
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

        private static MethodData GetContructorMethodData(IExecutionSetup sut)
        {
            Type type = typeof(DefaultExecutionSetupShould);
            ConstructorInfo methodUnderTest = type.GetConstructor(Type.EmptyTypes);

            return new MethodData(
                type,
                new DefaultExecutionSetupShould(),
                methodUnderTest,
                new object[] { },
                Guid.NewGuid().ToString(),
                0,
                sut);
        }

        [Theory, AutoMock]
        public async Task ShouldExecuteContructor(DefaultExecutionSetup sut)
        {
            // Arrange
            MethodData methodData = GetContructorMethodData(sut);

            // Act
            await methodData.ExecuteAction();
        }

        private static void ThrowingSynchronousMethod()
        {
            throw new FileLoadException("Some random message " + Guid.NewGuid());
        }

        [Theory, AutoMock]
        public async Task ShouldExecuteThrowingSynchronousMethod(DefaultExecutionSetup sut)
        {
            // AAA
            await AssertExecutionThrows<FileLoadException>(GetStaticMethodData("ThrowingSynchronousMethod", sut));
        }

        private static async Task ThrowingAsynchronousMethod()
        {
            await Task.Yield();
            throw new FileNotFoundException("Some random message " + Guid.NewGuid());
        }

        [Theory, AutoMock]
        public async Task ExecuteAsynchronousThrowingMethod(DefaultExecutionSetup sut)
        {
            // AAA
            await AssertExecutionThrows<FileNotFoundException>(GetStaticMethodData("ThrowingAsynchronousMethod", sut));
        }

        private static Task ThrowingAsynchronousMethodThrowingSynchronously()
        {
            throw new FieldAccessException("Some random message " + Guid.NewGuid());
        }

        [Theory, AutoMock]
        public async Task ShouldExecuteAsynchronousMethodThatThrowsSynchronously(DefaultExecutionSetup sut)
        {
            // AAA
            await AssertExecutionThrows<FieldAccessException>(GetStaticMethodData("ThrowingAsynchronousMethodThrowingSynchronously", sut));
        }
    }
}
