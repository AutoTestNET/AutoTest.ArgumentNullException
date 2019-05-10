namespace AutoTest.ExampleLibrary.Issues.Issue004
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoTest.ArgNullEx;
    using AutoTest.ArgNullEx.Xunit;
    using Xunit;

    public class Issue004
    {
        [Theory, RequiresArgNullExAutoMoq(typeof(SomeOutParameters))]
        [Include(
            ExclusionType = ExclusionType.All,
            Type = typeof(SomeOutParameters),
            Method = "SomeOutParametersMethod",
            Parameter = "stringInput")]
        public async Task TestStringInput(MethodData method)
        {
            await method.Execute().ConfigureAwait(false);

            Assert.True(SomeOutParameters.StringInputTested);
        }

        [Theory, RequiresArgNullExAutoMoq(typeof(SomeOutParameters))]
        [Include(
            ExclusionType = ExclusionType.All,
            Type = typeof(SomeOutParameters),
            Method = "SomeOutParametersMethod",
            Parameter = "stringRef")]
        public async Task TestStringRef(MethodData method)
        {
            await method.Execute().ConfigureAwait(false);

            Assert.True(SomeOutParameters.StringRefTested);
        }
    }
}
