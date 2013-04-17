namespace AutoTest.ArgNullEx
{
    using System;
    using System.Reflection;
    using Ploeh.AutoFixture;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RequiresArgumentNullExceptionAutoMoqAttribute : RequiresArgumentNullExceptionAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequiresArgumentNullExceptionAutoMoqAttribute"/> class.
        /// </summary>
        /// <param name="assemblyUnderTest">A type in the assembly under test.</param>
        public RequiresArgumentNullExceptionAutoMoqAttribute(Type assemblyUnderTest)
            : base(CreateFixture(), GetAssembly(assemblyUnderTest))
        {
        }

        private static IFixture CreateFixture()
        {
            return
                new Fixture().Customize(new AutoFixtureCustomizations())
                             .Customize(new NullTestsCustomization());
        }

        private static Assembly GetAssembly(Type assemblyUnderTest)
        {
            if (assemblyUnderTest == null) throw new ArgumentNullException("assemblyUnderTest");

            return assemblyUnderTest.Assembly;
        }
    }
}
