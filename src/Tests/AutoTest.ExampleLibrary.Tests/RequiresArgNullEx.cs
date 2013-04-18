namespace AutoTest.ExampleLibrary.Tests
{
    using System.Threading.Tasks;
    using AutoTest.ArgNullEx;
    using AutoTest.ArgNullEx.Xunit;
    using Xunit.Extensions;

    public class RequiresArgNullEx
    {
        [Theory, RequiresArgNullExAutoMoq(typeof(Class1))]
        public Task TestAllNullArguments(MethodData methodData)
        {
            return methodData.Execute();
        }
    }
}
