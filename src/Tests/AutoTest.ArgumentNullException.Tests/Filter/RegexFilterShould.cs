namespace AutoTest.ArgNullEx.Filter
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using global::Xunit;
    using global::Xunit.Extensions;

    public class RegexFilterShould
    {
        private static List<RegexFilter.RegexRule> TypeRules
        {
            get
            {
                return new List<RegexFilter.RegexRule>
                    {
                        new RegexFilter.RegexRule("Type rule 1", match: true, type: new Regex(".*")),
                        new RegexFilter.RegexRule("Type rule 2", match: false, type: new Regex(".*")),
                        new RegexFilter.RegexRule("Type rule 3", match: true, type: new Regex(".*")),
                    };
            }
        }

        private static List<RegexFilter.RegexRule> MethodRules
        {
            get
            {
                return new List<RegexFilter.RegexRule>
                    {
                        new RegexFilter.RegexRule("Method rule 1", match: true, type: new Regex(".*"), method: new Regex(".*")),
                        new RegexFilter.RegexRule("Method rule 2", match: false, method: new Regex(".*")),
                        new RegexFilter.RegexRule("Method rule 3", match: true, type: new Regex(".*"), method: new Regex(".*")),
                        new RegexFilter.RegexRule("Method rule 4", match: false, type: new Regex(".*"), method: new Regex(".*")),
                        new RegexFilter.RegexRule("Method rule 5", match: true, method: new Regex(".*")),
                        new RegexFilter.RegexRule("Method rule 6", match: false, type: new Regex(".*"), method: new Regex(".*")),
                    };
            }
        }

        private static List<RegexFilter.RegexRule> ParameterRules
        {
            get
            {
                return new List<RegexFilter.RegexRule>
                    {
                        new RegexFilter.RegexRule("Parameter rule 1", match: true, parameter: new Regex(".*")),
                        new RegexFilter.RegexRule("Parameter rule 2", match: false, type: new Regex(".*"), parameter: new Regex(".*")),
                        new RegexFilter.RegexRule("Parameter rule 3", match: true, method: new Regex(".*"), parameter: new Regex(".*")),
                        new RegexFilter.RegexRule("Parameter rule 4", match: false, type: new Regex(".*"), method: new Regex(".*"), parameter: new Regex(".*")),
                    };
            }
        }


        public static IEnumerable<object[]> AllRuleTypes
        {
            get
            {
                return new[]
                    {
                        new object[] {TypeRules, MethodRules, ParameterRules},
                    };
            }
        }

        [Theory, AutoMock]
        public void ReturnName(RegexFilter sut)
        {
            Assert.Equal("RegexFilter", sut.Name);
        }

        [Theory, PropertyData("AllRuleTypes")]
        public void ReturnTypeRules(
            List<RegexFilter.RegexRule> typeRules,
            List<RegexFilter.RegexRule> methodRules,
            List<RegexFilter.RegexRule> parameterRules)
        {
            var sut = new RegexFilter(parameterRules.Concat(methodRules).Concat(typeRules));

            List<RegexFilter.RegexRule> actualRules = sut.TypeRules.ToList();

            Assert.Equal(typeRules.Count, actualRules.Count);
            Assert.False(typeRules.Except(actualRules).Any());
        }

        [Theory, PropertyData("AllRuleTypes")]
        public void ReturnMethodRules(
            List<RegexFilter.RegexRule> typeRules,
            List<RegexFilter.RegexRule> methodRules,
            List<RegexFilter.RegexRule> parameterRules)
        {
            var sut = new RegexFilter(parameterRules.Concat(methodRules).Concat(typeRules));

            List<RegexFilter.RegexRule> actualRules = sut.MethodRules.ToList();

            Assert.Equal(methodRules.Count, actualRules.Count);
            Assert.False(methodRules.Except(actualRules).Any());
        }

        [Theory, PropertyData("AllRuleTypes")]
        public void ReturnParameterRules(
            List<RegexFilter.RegexRule> typeRules,
            List<RegexFilter.RegexRule> methodRules,
            List<RegexFilter.RegexRule> parameterRules)
        {
            var sut = new RegexFilter(parameterRules.Concat(methodRules).Concat(typeRules));

            List<RegexFilter.RegexRule> actualRules = sut.ParameterRules.ToList();

            Assert.Equal(parameterRules.Count, actualRules.Count);
            Assert.False(parameterRules.Except(actualRules).Any());
        }
    }
}
