namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Linq;
    using global::Xunit;

    public class RegexFilterExtensionsShould
    {
// ReSharper disable UnusedParameter.Local
        private void AssertTypeRule(Func<Type, IRegexFilter> addMethod, bool expectedInclude)
        {
            IRegexFilter actualFilter = addMethod(GetType());

            Assert.Same(addMethod.Target, actualFilter);
            Assert.Equal(1, actualFilter.Rules.Count);
            RegexRule addedRule = actualFilter.Rules.Single();
            Assert.Equal(expectedInclude, addedRule.Include);
            Assert.NotNull(addedRule.Type);
            Assert.Null(addedRule.Method);
            Assert.Null(addedRule.Parameter);
            Assert.True(addedRule.MatchType(GetType()));
        }
// ReSharper restore UnusedParameter.Local

        [Fact]
        public void AddExcludeTypeRule()
        {
            // AAA
            AssertTypeRule(new RegexFilter().ExcludeType, expectedInclude: false);
        }

        [Fact]
        public void AddIncludeTypeRule()
        {
            // AAA
            AssertTypeRule(new RegexFilter().IncludeType, expectedInclude: true);
        }
    }
}
