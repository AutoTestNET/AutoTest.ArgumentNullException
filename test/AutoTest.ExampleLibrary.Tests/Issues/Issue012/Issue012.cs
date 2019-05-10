namespace AutoTest.ExampleLibrary.Issues.Issue012
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoTest.ArgNullEx;
    using AutoTest.ArgNullEx.Xunit;
    using Xunit;

    public class Issue012
    {
        [Theory, RequiresArgNullExAutoMoq(typeof(YieldExample))]
        [Include(Type = typeof(YieldExample))]
        public async Task TestNullArguments(MethodData method)
        {
            await method.Execute().ConfigureAwait(false);

            Assert.True(YieldExample.Tested);
        }
    }
}
