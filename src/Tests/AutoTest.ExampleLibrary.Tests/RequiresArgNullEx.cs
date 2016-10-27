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
    using AutoTest.ExampleLibrary.Issues.Issue022;
    using Xunit;

    public class RequiresArgNullEx
    {
        [Theory, RequiresArgNullExAutoMoq(typeof(Class1))]
        [Exclude(Type = typeof(InternalInnerInterface))]
        [Exclude(Type = typeof(Mixture), Method = "Private")]
        [Substitute(typeof(GenericClass<>), typeof(GenericClass<object>))]
        public Task TestAllNullArguments(MethodData method)
        {
            return method.Execute();
        }
    }
}
