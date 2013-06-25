namespace AutoTest.ExampleLibrary
{
    using System;
    using System.Linq;
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

            var throwingRecursionBehavior = fixture.Behaviors.OfType<ThrowingRecursionBehavior>().SingleOrDefault();
            if (throwingRecursionBehavior != null)
            {
                fixture.Behaviors.Remove(throwingRecursionBehavior);
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            }

            // Don't need to create complex graphs, just need objects.
            fixture.OmitAutoProperties = true;

            return new ArgumentNullExceptionFixture(assemblyUnderTest, fixture);
        }
    }
}
