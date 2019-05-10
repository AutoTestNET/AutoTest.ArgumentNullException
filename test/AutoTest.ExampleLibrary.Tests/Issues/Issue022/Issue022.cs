namespace AutoTest.ExampleLibrary.Issues.Issue022
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoTest.ArgNullEx;
    using AutoTest.ArgNullEx.Xunit;
    using Xunit;

    public class Issue022
    {
        [Theory, RequiresArgNullExAutoMoq(typeof(GenericClass<>))]
        [Include(Type = typeof(GenericClass<>))]
        [Substitute(typeof(GenericClass<>), typeof(GenericClass<Version>))]
        public async Task TestGenericClass(MethodData method)
        {
            await method.Execute();
            Assert.True(GenericClassBase.Tested);
        }
    }
}
