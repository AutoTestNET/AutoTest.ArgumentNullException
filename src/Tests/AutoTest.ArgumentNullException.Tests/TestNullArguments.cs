namespace AutoTest.ArgNullEx
{
    using System.Threading.Tasks;
    using Xunit.Extensions;

    public class TestNullArguments
    {
        [Theory, RequiresArgumentNullExceptionAutoMoq(typeof(MethodData))]
        public Task TestAllNullArguments(MethodData methodData)
        {
            return methodData.Execute();
        }
    }
}
