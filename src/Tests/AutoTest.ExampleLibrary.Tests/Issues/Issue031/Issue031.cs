namespace AutoTest.ExampleLibrary.Issues.Issue031
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoTest.ArgNullEx;
    using AutoTest.ArgNullEx.Xunit;
    using Xunit;

    public class Issue031
    {
        [Theory, RequiresArgNullExAutoMoq(typeof(SomeNullableValueTypeParameters))]
        [Include(
            ExclusionType = ExclusionType.All,
            Type = typeof(SomeNullableValueTypeParameters),
            Method = "SomeNullableValueTypeParametersMethod",
            Parameter = "stringInput")]
        public async Task TestStringInput(MethodData method)
        {
            await method.Execute();

            Assert.True(SomeNullableValueTypeParameters.StringInputTested);
        }
    }
}
