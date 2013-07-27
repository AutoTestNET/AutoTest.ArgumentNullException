namespace AutoTest.ExampleLibrary.Issues.Issue006
{
    using System.Threading.Tasks;
    using AutoTest.ArgNullEx;
    using AutoTest.ArgNullEx.Xunit;
    using Xunit;
    using Xunit.Extensions;

    public class RequiresArgNullEx
    {
        [Theory, RequiresArgNullExAutoMoq(typeof(SpecialCharacters))]
        [Include(Type = typeof(SpecialCharacters.InnerClass))]
        public async Task TestAllNullArguments(MethodData method)
        {
            await method.Execute();

            Assert.True(SpecialCharacters.Tested);
        }
    }
}
