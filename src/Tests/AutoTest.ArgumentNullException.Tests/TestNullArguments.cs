namespace AutoTest.ArgNullEx
{
    using System;
    using Xunit;
    using Xunit.Extensions;

    public class TestNullArguments
    {
        [Theory, RequiresArgumentNullExceptionAutoMoq(typeof(TestNullArguments))]
        public void TestAllNullArguments(MethodData methodData)
        {
            string actualParamName = Assert.Throws<ArgumentNullException>(() => methodData.ExecutingActionSync()).ParamName;
            Assert.Equal(methodData.NullArgument, actualParamName);
        }
    }
}
