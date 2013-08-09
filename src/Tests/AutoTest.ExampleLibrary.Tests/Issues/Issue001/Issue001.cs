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
        [Exclude(Type = typeof(SimpleGenericMethods), Method = "GenericExceptionMethod")]
        [Include(Type = typeof(SimpleGenericMethods))]
        public async Task Simple(MethodData method)
        {
            await method.Execute();

            Assert.True(SimpleGenericMethods.GenericMethod2Tested);
        }

        [Theory, RequiresArgNullExAutoMoq(typeof(SimpleGenericMethods))]
        [Include(
            ExclusionType = ExclusionType.Types | ExclusionType.Methods,
            Type = typeof(SimpleGenericMethods),
            Method = "GenericExceptionMethod")]
        public async Task SimpleException(MethodData method)
        {
            await method.Execute();

            Assert.True(SimpleGenericMethods.GenericExceptionMethodTested);
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

        [Theory, RequiresArgNullExAutoMoq(typeof(InterfaceGenericMethods))]
        [Include(
            ExclusionType = ExclusionType.All,
            Type = typeof(ComplexGenericMethods),
            Method = "GenericClassMethod",
            Parameter = "classValue")]
        public async Task ComplexClassValue(MethodData method)
        {
            await method.Execute();

            Assert.True(ComplexGenericMethods.ClassValueTested);
        }

        [Theory, RequiresArgNullExAutoMoq(typeof(InterfaceGenericMethods))]
        [Include(
            ExclusionType = ExclusionType.All,
            Type = typeof(ComplexGenericMethods),
            Method = "GenericClassMethod",
            Parameter = "genericClassMethodStringValue")]
        public async Task ComplexGenericClassMethodStringValue(MethodData method)
        {
            await method.Execute();

            Assert.True(ComplexGenericMethods.GenericClassMethodStringValueTested);
        }

        [Theory, RequiresArgNullExAutoMoq(typeof(InterfaceGenericMethods))]
        [Include(
            ExclusionType = ExclusionType.All,
            Type = typeof(ComplexGenericMethods),
            Method = "GenericExceptionMethod",
            Parameter = "exceptionValue")]
        public async Task ComplexExceptionValue(MethodData method)
        {
            await method.Execute();

            Assert.True(ComplexGenericMethods.ExceptionValueTested);
        }

        [Theory, RequiresArgNullExAutoMoq(typeof(InterfaceGenericMethods))]
        [Include(
            ExclusionType = ExclusionType.All,
            Type = typeof(ComplexGenericMethods),
            Method = "GenericExceptionMethod",
            Parameter = "genericExceptionMethodStringValue")]
        public async Task ComplexGenericExceptionMethodStringValue(MethodData method)
        {
            await method.Execute();

            Assert.True(ComplexGenericMethods.GenericExceptionMethodStringValueTested);
        }
    }
}
