namespace AutoTest.ArgNullEx
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RequiresArgumentNullExceptionAutoMoqAttribute : RequiresArgumentNullExceptionAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequiresArgumentNullExceptionAutoMoqAttribute"/> class.
        /// </summary>
        /// <param name="assemblyUnderTest">A type in the assembly under test.</param>
        public RequiresArgumentNullExceptionAutoMoqAttribute(Type assemblyUnderTest)
            : base(new AutoMockAttribute(), assemblyUnderTest != null ? assemblyUnderTest.Assembly : null)
        {
        }
    }
}
