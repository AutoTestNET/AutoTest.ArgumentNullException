namespace AutoTest.ArgNullEx
{
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    public class AsyncCustomization : ICustomization
    {
        /// <summary>
        /// A completed task to return from a standard asynchronous method.
        /// </summary>
        private static readonly Task CompletedTask = Task.FromResult(0);

        void ICustomization.Customize(IFixture fixture)
        {
            // Customize the MethodData to setup the ExecutingActionAsync to return a completed task.
            fixture.Customize<MethodData>(
                composer => composer.With(m => m.ExecutingActionAsync, () => CompletedTask));
        }
    }

    public class NullTestsCustomization : ICustomization
    {
        private static readonly ParameterInfo ParameterInfo = GetParameterInfo();

        private static ParameterInfo GetParameterInfo(object unused = null)
        {
            return
                typeof(NullTestsCustomization).GetMethod("GetParameterInfo",
                                                          BindingFlags.NonPublic | BindingFlags.Static)
                                              .GetParameters()
                                              .Single();
        }

        void ICustomization.Customize(IFixture fixture)
        {
            var throwingRecursionBehavior = fixture.Behaviors.OfType<ThrowingRecursionBehavior>().SingleOrDefault();
            if (throwingRecursionBehavior != null)
            {
                fixture.Behaviors.Remove(throwingRecursionBehavior);
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            }

            fixture.Inject(ParameterInfo);
        }
    }

    public class AutoFixtureCustomizations : CompositeCustomization
    {
        public AutoFixtureCustomizations()
            : base(new AsyncCustomization(), new NullTestsCustomization(), new AutoMoqCustomization())
        {
        }
    }
}
