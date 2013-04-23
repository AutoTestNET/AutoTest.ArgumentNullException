namespace AutoTest.ArgNullEx.Filter
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using global::Xunit;
    using global::Xunit.Extensions;

    public class RegexFilterShould
    {
        private static List<RegexRule> TypeRules
        {
            get
            {
                return new List<RegexRule>
                    {
                        new RegexRule("Type rule 1", include: true, type: new Regex(".*")),
                        new RegexRule("Type rule 2", include: false, type: new Regex(".*")),
                        new RegexRule("Type rule 3", include: true, type: new Regex(".*")),
                    };
            }
        }

        private static List<RegexRule> MethodRules
        {
            get
            {
                return new List<RegexRule>
                    {
                        new RegexRule("Method rule 1", include: true, type: new Regex(".*"), method: new Regex(".*")),
                        new RegexRule("Method rule 2", include: false, method: new Regex(".*")),
                        new RegexRule("Method rule 3", include: true, type: new Regex(".*"), method: new Regex(".*")),
                        new RegexRule("Method rule 4", include: false, type: new Regex(".*"), method: new Regex(".*")),
                        new RegexRule("Method rule 5", include: true, method: new Regex(".*")),
                        new RegexRule("Method rule 6", include: false, type: new Regex(".*"), method: new Regex(".*")),
                    };
            }
        }

        private static List<RegexRule> ParameterRules
        {
            get
            {
                return new List<RegexRule>
                    {
                        new RegexRule("Parameter rule 1", include: true, parameter: new Regex(".*")),
                        new RegexRule("Parameter rule 2", include: false, type: new Regex(".*"), parameter: new Regex(".*")),
                        new RegexRule("Parameter rule 3", include: true, method: new Regex(".*"), parameter: new Regex(".*")),
                        new RegexRule("Parameter rule 4", include: false, type: new Regex(".*"), method: new Regex(".*"), parameter: new Regex(".*")),
                    };
            }
        }


        public static IEnumerable<object[]> AllRuleTypes
        {
            get
            {
                return new[]
                    {
                        new object[] {TypeRules, MethodRules, ParameterRules}
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
            List<RegexRule> typeRules,
            List<RegexRule> methodRules,
            List<RegexRule> parameterRules)
        {
            var sut = new RegexFilter();
            sut.Rules.AddRange(parameterRules.Concat(methodRules).Concat(typeRules));

            List<RegexRule> actualRules = sut.TypeRules.ToList();

            Assert.Equal(typeRules.Count, actualRules.Count);
            Assert.False(typeRules.Except(actualRules).Any());
        }

        [Theory, PropertyData("AllRuleTypes")]
        public void ReturnMethodRules(
            List<RegexRule> typeRules,
            List<RegexRule> methodRules,
            List<RegexRule> parameterRules)
        {
            var sut = new RegexFilter();
            sut.Rules.AddRange(parameterRules.Concat(methodRules).Concat(typeRules));

            List<RegexRule> actualRules = sut.MethodRules.ToList();

            Assert.Equal(methodRules.Count, actualRules.Count);
            Assert.False(methodRules.Except(actualRules).Any());
        }

        [Theory, PropertyData("AllRuleTypes")]
        public void ReturnParameterRules(
            List<RegexRule> typeRules,
            List<RegexRule> methodRules,
            List<RegexRule> parameterRules)
        {
            var sut = new RegexFilter();
            sut.Rules.AddRange(parameterRules.Concat(methodRules).Concat(typeRules));

            List<RegexRule> actualRules = sut.ParameterRules.ToList();

            Assert.Equal(parameterRules.Count, actualRules.Count);
            Assert.False(parameterRules.Except(actualRules).Any());
        }
    }
}
