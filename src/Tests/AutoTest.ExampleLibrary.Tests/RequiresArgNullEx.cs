namespace AutoTest.ExampleLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoTest.ArgNullEx;
    using AutoTest.ArgNullEx.Xunit;
    using AutoTest.ExampleLibrary.Issues.Issue009;
    using AutoTest.ExampleLibrary.Issues.Issue020;
    using Xunit.Extensions;

    public class RequiresArgNullEx
    {
        [Theory, RequiresArgNullExAutoMoq(typeof(Class1))]
        [Exclude(Type = typeof(InternalInnerInterface))]
        [Exclude(Type = typeof(Mixture), Method = "Private")]
        public Task TestAllNullArguments(MethodData method)
        {
            return method.Execute();
        }
    }
}
