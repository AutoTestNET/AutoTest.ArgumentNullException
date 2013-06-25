namespace AutoTest.ArgNullEx.Xunit
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Base class for test methods decorated with
    /// <see cref="RequiresArgumentNullExceptionAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class CustomizeAttribute : Attribute
    {
        /// <summary>
        /// Gets a customization for a test method.
        /// </summary>
        /// <param name="method">The method to be customized.</param>
        /// <returns>A customization for a test method.</returns>
        public abstract IArgNullExCustomization GetCustomization(MethodInfo method);
    }
}
