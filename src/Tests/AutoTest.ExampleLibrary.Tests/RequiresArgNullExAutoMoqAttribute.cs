namespace AutoTest.ExampleLibrary.Tests
{
    using System;
    using System.Reflection;
    using AutoTest.ArgNullEx;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    public class RequiresArgNullExAutoMoqAttribute : RequiresArgumentNullExceptionAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequiresArgNullExAutoMoqAttribute"/> class.
        /// </summary>
        /// <param name="assemblyUnderTest">A type in the assembly under test.</param>
        public RequiresArgNullExAutoMoqAttribute(Type assemblyUnderTest)
            : base(CreateFixture(), GetAssembly(assemblyUnderTest))
        {
        }

        private static IFixture CreateFixture()
        {
            return new Fixture().Customize(new AutoMoqCustomization());
        }

        private static Assembly GetAssembly(Type assemblyUnderTest)
        {
            if (assemblyUnderTest == null) throw new ArgumentNullException("assemblyUnderTest");

            return assemblyUnderTest.Assembly;
        }
    }
}
