namespace AutoTest.ExampleLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoTest.ArgNullEx;
    using AutoTest.ArgNullEx.Xunit;
    using AutoTest.ExampleLibrary.Issues.Issue009;
    using Xunit.Extensions;

    public class RequiresArgNullEx
    {
        [Theory, RequiresArgNullExAutoMoq(typeof(Class1))]
        [Exclude(Type = typeof(InternalInnerInterface))]
        public Task TestAllNullArguments(MethodData method)
        {
            return method.Execute();
        }
    }
}
