namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Text.RegularExpressions;
    using global::Xunit;

    public class RegexRuleExtensionsShould
    {
        [Fact]
        public void MatchAType()
        {
            // Arrange
            var rule = new RegexRule(
                typeof(RegexRuleExtensionsShould).Name + " match rule",
                match: false,
                type: new Regex(@".+\." + typeof(RegexRuleExtensionsShould).Name));

            // Act
            bool actual = rule.MatchType(typeof (RegexRuleExtensionsShould));

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void NotMatchAType()
        {
            // Arrange
            var rule = new RegexRule("Miss rule", match: false, type: new Regex("Miss"));

            // Act
            bool actual = rule.MatchType(typeof(RegexRuleExtensionsShould));

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void ThrowIfNoTypeRuleForTypeMatch()
        {
            // Arrange
            var rule = new RegexRule("Throw rule", match: false);

            // Act/Assert
            string paramName = Assert.Throws<ArgumentException>(() => rule.MatchType(typeof (RegexRuleExtensionsShould))).ParamName;
            Assert.Equal("rule", paramName);
        }
    }
}
