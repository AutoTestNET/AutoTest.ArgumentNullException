namespace AutoTest.ArgNullEx
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RequiresArgumentNullExceptionAutoMoqAttribute : RequiresArgumentNullExceptionAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequiresArgumentNullExceptionAutoMoqAttribute"/> class.
        /// </summary>
        /// <param name="typeInAssembly">A type in the assembly under test.</param>
        public RequiresArgumentNullExceptionAutoMoqAttribute(
            Type typeInAssembly)
            : base(new AutoMockAttribute(), typeInAssembly.Assembly)
        {
        }
    }
}
