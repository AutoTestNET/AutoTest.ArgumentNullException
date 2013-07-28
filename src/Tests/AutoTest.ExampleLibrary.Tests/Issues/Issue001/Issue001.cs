namespace AutoTest.ExampleLibrary.Issues.Issue001
{
    using System.Threading.Tasks;
    using AutoTest.ArgNullEx;
    using AutoTest.ArgNullEx.Xunit;
    using Xunit;
    using Xunit.Extensions;

    public class Issue001
    {
        [Theory, RequiresArgNullExAutoMoq(typeof(SimpleGenericMethods))]
        [Include(Type = typeof(SimpleGenericMethods))]
        public async Task Simple(MethodData method)
        {
            await method.Execute();

            Assert.True(SimpleGenericMethods.GenericMethod2Tested);
        }

        [Theory, RequiresArgNullExAutoMoq(typeof(MixedGenericMethods))]
        [Include(
            ExclusionType = ExclusionType.All,
            Type = typeof(MixedGenericMethods),
            Method = "GenericMethod",
            Parameter = "classValue")]
        public async Task MixedClassValue(MethodData method)
        {
            await method.Execute();

            Assert.True(MixedGenericMethods.ClassValueTested);
        }

        [Theory, RequiresArgNullExAutoMoq(typeof(MixedGenericMethods))]
        [Include(
            ExclusionType = ExclusionType.All,
            Type = typeof(MixedGenericMethods),
            Method = "GenericMethod",
            Parameter = "stringValue")]
        public async Task MixedStringValue(MethodData method)
        {
            await method.Execute();

            Assert.True(MixedGenericMethods.StringValueTested);
        }

        [Theory, RequiresArgNullExAutoMoq(typeof(InterfaceGenericMethods))]
        [Include(Type = typeof(InterfaceGenericMethods))]
        [Include(
            ExclusionType = ExclusionType.All,
            Type = typeof(InterfaceGenericMethods),
            Method = "GenericMethod",
            Parameter = "classValue")]
        public async Task InterfaceClassValue(MethodData method)
        {
            await method.Execute();

            Assert.True(InterfaceGenericMethods.ClassValueTested);
        }

        [Theory, RequiresArgNullExAutoMoq(typeof(InterfaceGenericMethods))]
        [Include(Type = typeof(InterfaceGenericMethods))]
        [Include(
            ExclusionType = ExclusionType.All,
            Type = typeof(InterfaceGenericMethods),
            Method = "GenericMethod",
            Parameter = "stringValue")]
        public async Task InterfaceStringValue(MethodData method)
        {
            await method.Execute();

            Assert.True(InterfaceGenericMethods.StringValueTested);
        }
    }
}
