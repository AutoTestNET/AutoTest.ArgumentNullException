namespace AutoTest.ArgNullEx.Xunit
{
    using System;
    using System.Reflection;

    /// <summary>
    /// An attribute that can be applied to methods in an <see cref="RequiresArgumentNullExceptionAttribute"/>-driven
    /// Theory to indicate that all types should be excluded from the test unless otherwise included.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ExcludeAllAttribute : CustomizeAttribute, IArgNullExCustomization
    {
        /// <summary>
        /// Gets a customization for a test method.
        /// </summary>
        /// <param name="method">The method to be customized.</param>
        /// <returns>A customization for a test method.</returns>
        public override IArgNullExCustomization GetCustomization(MethodInfo method)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            return this;
        }

        /// <summary>
        /// Customizes the specified <paramref name="fixture"/> by excluding all types.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        public virtual void Customize(IArgumentNullExceptionFixture fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");

            fixture.ExcludeAllTypes();
        }
    }
}
