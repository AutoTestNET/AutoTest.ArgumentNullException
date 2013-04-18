namespace AutoTest.ArgNullEx
{
    using System.Threading.Tasks;
    using AutoTest.ArgNullEx.Xunit;
    using global::Xunit.Extensions;

    public class TestNullArguments
    {
        [Theory, RequiresArgumentNullExceptionAutoMoq(typeof(MethodData))]
        public Task TestAllNullArguments(MethodData methodData)
        {
            return methodData.Execute();
        }

        [Theory, RequiresArgumentNullExceptionAutoMoq(typeof(MethodDataExtensions))]
        public Task TestAllNullArgumentsXunit(MethodData methodData)
        {
            return methodData.Execute();
        }
    }
}
