namespace AutoTest.ArgNullEx
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using AutoTest.ArgNullEx.Framework;
    using AutoTest.ArgNullEx.Xunit;
    using Ploeh.AutoFixture;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RequiresArgumentNullExceptionAutoMoqAttribute : RequiresArgumentNullExceptionAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequiresArgumentNullExceptionAutoMoqAttribute"/> class.
        /// </summary>
        /// <param name="assemblyUnderTest">A type in the assembly under test.</param>
        public RequiresArgumentNullExceptionAutoMoqAttribute(Type assemblyUnderTest)
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
            var fixture =
                new Fixture().Customize(new AutoFixtureCustomizations())
                             .Customize(new NullTestsCustomization());

            return
                new ArgumentNullExceptionFixture(assemblyUnderTest, fixture)
                    .ExcludeType(typeof(ReflectionDiscoverableCollection<>))
                    .ExcludeType(typeof(TaskHelpers))
                    .ExcludeType(typeof(TaskHelpersExtensions))
                    .ExcludeType(typeof(CatchInfoBase<>))
                    .ExcludeType(typeof(CatchInfo))
                    .ExcludeType(typeof(CatchInfo<>));
        }
    }
}
