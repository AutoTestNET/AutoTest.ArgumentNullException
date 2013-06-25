namespace AutoTest.ExampleLibrary
{
    using System.Threading.Tasks;
    using AutoTest.ArgNullEx;
    using AutoTest.ArgNullEx.Xunit;
    using Xunit.Extensions;

    public class RequiresArgNullEx
    {
        [Theory, RequiresArgNullExAutoMoq(typeof(Class1))]
        public Task TestAllNullArguments(MethodData method)
        {
            return method.Execute();
        }
    }
}
