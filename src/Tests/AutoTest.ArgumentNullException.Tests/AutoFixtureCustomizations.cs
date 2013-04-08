namespace AutoTest.ArgNullEx
{
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

    public class AutoFixtureCustomizations : CompositeCustomization
    {
        public AutoFixtureCustomizations()
            : base(new AsyncCustomization(), new AutoMoqCustomization())
        {
        }
    }
}
