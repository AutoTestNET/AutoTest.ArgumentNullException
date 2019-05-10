namespace AutoTest.ExampleLibrary.Issues.Issue020
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoTest.ArgNullEx;
    using AutoTest.ArgNullEx.Xunit;
    using Xunit;

    public class ExcludePrivateShould
    {
        [Theory, RequiresArgNullExAutoMoq(typeof(Mixture)), ExcludePrivate]
        [Include(Type = typeof(Mixture))]
        public async Task OnlyTestPublic(MethodData method)
        {
            await method.Execute().ConfigureAwait(false);
            Assert.True(Mixture.Tested);
        }
    }
}
