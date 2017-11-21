namespace AutoTest.ExampleLibrary.Issues.Issue006
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoTest.ArgNullEx;
    using AutoTest.ArgNullEx.Xunit;
    using Xunit;

    public class Issue006
    {
        [Theory, RequiresArgNullExAutoMoq(typeof(SpecialCharacters))]
        [Include(Type = typeof(SpecialCharacters.InnerClass))]
        public async Task TestNullArguments(MethodData method)
        {
            await method.Execute();

            Assert.True(SpecialCharacters.Tested);
        }
    }
}
