namespace AutoTest.ExampleLibrary.Issues.Issue006
{
    using System.Threading.Tasks;
    using AutoTest.ArgNullEx;
    using AutoTest.ArgNullEx.Xunit;
    using Xunit.Extensions;

    public class RequiresArgNullEx
    {
        [Theory, RequiresArgNullExAutoMoq(typeof(SpecialCharacters))]
        [Include(Type = typeof(SpecialCharacters.InnerClass))]
        public Task TestAllNullArguments(MethodData method)
        {
            return method.Execute();
        }
    }
}
