namespace AutoTest.ExampleLibrary.Issues.Issue009
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AutoTest.ArgNullEx.Filter;
    using Xunit;

    public class Issue009
    {
        [Fact]
        public void ShouldNotThrow()
        {
            var filter = new RegexFilter();
            filter.ExcludeAllTypes().IncludeType(typeof (InternalInnerInterface));
            List<MethodBase> methods =
                typeof(InternalInnerInterface).GetMethods(
                    BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly,
                    new[] { filter }).ToList();

            Assert.NotNull(methods);
            Assert.NotEmpty(methods);
        }
    }
}
