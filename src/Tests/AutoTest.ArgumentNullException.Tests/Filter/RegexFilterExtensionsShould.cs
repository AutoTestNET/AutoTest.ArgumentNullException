namespace AutoTest.ArgNullEx.Filter
{
    using System.Linq;
    using global::Xunit;

    public class RegexFilterExtensionsShould
    {
        [Fact]
        public void AddExcludeTypeRule()
        {
            // Arrange
            var filter = new RegexFilter();

            // Act
            IRegexFilter actualFilter = filter.ExcludeType(GetType());

            // Assert
            Assert.Same(filter, actualFilter);
            Assert.Equal(1, filter.Rules.Count);
            RegexRule addedRule = filter.Rules.Single();
            Assert.False(addedRule.Include);
            Assert.NotNull(addedRule.Type);
            Assert.Null(addedRule.Method);
            Assert.Null(addedRule.Parameter);
            Assert.True(addedRule.MatchType(GetType()));
        }
    }
}
