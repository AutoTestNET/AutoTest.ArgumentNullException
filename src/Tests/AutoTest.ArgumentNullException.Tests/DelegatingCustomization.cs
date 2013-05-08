namespace AutoTest.ArgNullEx
{
    using System;

    internal class DelegatingCustomization : IArgNullExCustomization
    {
        public DelegatingCustomization()
        {
            OnCustomize = f => { };
        }

        public void Customize(IArgumentNullExceptionFixture fixture)
        {
            OnCustomize(fixture);
        }

        internal Action<IArgumentNullExceptionFixture> OnCustomize { get; set; }
    }
}
