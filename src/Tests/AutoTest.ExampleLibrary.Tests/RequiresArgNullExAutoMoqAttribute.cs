namespace AutoTest.ExampleLibrary.Tests
{
    using System;
    using System.Reflection;
    using AutoTest.ArgNullEx;
    using AutoTest.ArgNullEx.Xunit;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    public class RequiresArgNullExAutoMoqAttribute : RequiresArgumentNullExceptionAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequiresArgNullExAutoMoqAttribute"/> class.
        /// </summary>
        /// <param name="assemblyUnderTest">A type in the assembly under test.</param>
        public RequiresArgNullExAutoMoqAttribute(Type assemblyUnderTest)
            : base(CreateFixture(GetAssembly(assemblyUnderTest)))
        {
        }

        private static Assembly GetAssembly(Type assemblyUnderTest)
        {
            if (assemblyUnderTest == null) throw new ArgumentNullException("assemblyUnderTest");

            return assemblyUnderTest.Assembly;
        }

        private static IArgumentNullExceptionFixture CreateFixture(Assembly assemblyUnderTest)
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            return new ArgumentNullExceptionFixture(assemblyUnderTest, fixture);
        }
    }
}
