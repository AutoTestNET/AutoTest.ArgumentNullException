namespace AutoTest.ArgNullEx
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;
    using Ploeh.AutoFixture.Kernel;

    public class ReflectionCustomization : ICustomization
    {
        void ICustomization.Customize(IFixture fixture)
        {
            fixture.Customize<MethodBase>(
                composer => composer.FromFactory(new TypeRelay(typeof (MethodBase), typeof (MethodInfo))));
        }
    }

    public class NullTestsCustomization : ICustomization
    {
        private static readonly ParameterInfo ParameterInfo = GetParameterInfo();

// ReSharper disable UnusedParameter.Local
        private static ParameterInfo GetParameterInfo(object unused = null)
// ReSharper restore UnusedParameter.Local
        {
            var method =
                typeof(NullTestsCustomization).GetMethod("GetParameterInfo",
                                                          BindingFlags.NonPublic | BindingFlags.Static);

            return method.GetParameters().Single();
        }

        void ICustomization.Customize(IFixture fixture)
        {
            var throwingRecursionBehavior = fixture.Behaviors.OfType<ThrowingRecursionBehavior>().SingleOrDefault();
            if (throwingRecursionBehavior != null)
            {
                fixture.Behaviors.Remove(throwingRecursionBehavior);
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            }

            var tcs = new TaskCompletionSource<int>();
            tcs.SetException(new Exception());

            fixture.Inject(ParameterInfo);
            fixture.Inject(new CatchInfo(tcs.Task));
        }
    }

    public class AutoFixtureCustomizations : CompositeCustomization
    {
        public AutoFixtureCustomizations()
            : base(new ReflectionCustomization(), new AutoMoqCustomization())
        {
        }
    }
}
