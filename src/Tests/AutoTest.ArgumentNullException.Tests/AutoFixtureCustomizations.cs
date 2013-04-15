namespace AutoTest.ArgNullEx
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    public class AsyncCustomization : ICustomization
    {
        void ICustomization.Customize(IFixture fixture)
        {
            // Re-factoring has left nothing to customize.
        }
    }

    public class NullTestsCustomization : ICustomization
    {
        private static readonly MethodInfo MethodInfo = GetParameterInfo().Item1;

        private static readonly ParameterInfo ParameterInfo = GetParameterInfo().Item2;

        private static Tuple<MethodInfo, ParameterInfo> GetParameterInfo(object unused = null)
        {
            var method =
                typeof(NullTestsCustomization).GetMethod("GetParameterInfo",
                                                          BindingFlags.NonPublic | BindingFlags.Static);

            return Tuple.Create(method, method.GetParameters().Single());
        }

        void ICustomization.Customize(IFixture fixture)
        {
            var throwingRecursionBehavior = fixture.Behaviors.OfType<ThrowingRecursionBehavior>().SingleOrDefault();
            if (throwingRecursionBehavior != null)
            {
                fixture.Behaviors.Remove(throwingRecursionBehavior);
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            }

            fixture.Inject(MethodInfo);
            fixture.Inject<MethodBase>(MethodInfo);
            fixture.Inject(ParameterInfo);
        }
    }

    public class AutoFixtureCustomizations : CompositeCustomization
    {
        public AutoFixtureCustomizations()
            : base(new AsyncCustomization(), new AutoMoqCustomization())
        {
        }
    }
}
