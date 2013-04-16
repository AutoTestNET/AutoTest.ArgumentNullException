namespace AutoTest.ExampleLibrary.Tests
{
    using System;
    using AutoTest.ArgNullEx;

    public class RequiresArgNullExAutoMoqAttribute : RequiresArgumentNullExceptionAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequiresArgNullExAutoMoqAttribute"/> class.
        /// </summary>
        /// <param name="assemblyUnderTest">A type in the assembly under test.</param>
        public RequiresArgNullExAutoMoqAttribute(Type assemblyUnderTest)
            : base(new AutoMockAttribute(), assemblyUnderTest != null ? assemblyUnderTest.Assembly : null)
        {
        }
    }
}
