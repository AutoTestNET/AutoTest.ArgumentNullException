namespace AutoTest.ExampleLibrary.Issues.Issue015
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using AutoTest.ArgNullEx;
    using AutoTest.ArgNullEx.Xunit;
    using Xunit;

    public class Issue015
    {
        [Theory, RequiresArgNullExAutoMoq(typeof(ExplicitEquals))]
        [Include(Type = typeof(ExplicitEquals))]
        [Exclude(Method = "System.Collections.Generic.IEqualityComparer<System.String>.GetHashCode")]
        public async Task ExplicitEquals(MethodData method)
        {
            await method.Execute();

            if (method.NullParameter == "stringValue1")
                Assert.True(Issues.Issue015.ExplicitEquals.TestedStringValue1);

            if (method.NullParameter == "stringValue2")
                Assert.True(Issues.Issue015.ExplicitEquals.TestedStringValue2);

            if (method.NullParameter == "stringValue3")
                Assert.True(Issues.Issue015.ExplicitEquals.TestedStringValue3);
        }

        [Theory, RequiresArgNullExAutoMoq(typeof(ImplicitEquals))]
        [Include(
            Type = typeof(ImplicitEquals),
            Method = "Equals",
            ExclusionType = ExclusionType.Types | ExclusionType.Methods)]
        public async Task ImplicitEquals(MethodData method)
        {
            await method.Execute();

            Assert.True(Issues.Issue015.ImplicitEquals.Tested);
        }
    }

    public class IgnoreExplicitEquals
    {
        [Fact]
        public void ShouldOnlyGet5Tests()
        {
            IArgumentNullExceptionFixture sut =
                new ArgumentNullExceptionFixture(typeof(ExplicitEquals).GetTypeInfo().Assembly)
                    .ExcludeAllTypes()
                    .IncludeType(typeof(ExplicitEquals));

            IEnumerable<MethodData> data = sut.GetData();
            Assert.Equal(5, data.Count());
        }
    }

    public class IgnoreImplicitEquals
    {
        [Fact]
        public void ShouldOnlyGet2Tests()
        {
            IArgumentNullExceptionFixture sut =
                new ArgumentNullExceptionFixture(typeof(ImplicitEquals).GetTypeInfo().Assembly)
                    .ExcludeAllTypes()
                    .IncludeType(typeof(ImplicitEquals));

            IEnumerable<MethodData> data = sut.GetData();
            Assert.Equal(2, data.Count());
        }
    }

    public class NotIgnoreOtherEquals
    {
        [Fact]
        public void ShouldGetAll5Tests()
        {
            IArgumentNullExceptionFixture sut =
                new ArgumentNullExceptionFixture(typeof(OtherEquals).GetTypeInfo().Assembly)
                    .ExcludeAllTypes()
                    .IncludeType(typeof(OtherEquals));

            IEnumerable<MethodData> data = sut.GetData();
            Assert.Equal(5, data.Count());
        }
    }
}
