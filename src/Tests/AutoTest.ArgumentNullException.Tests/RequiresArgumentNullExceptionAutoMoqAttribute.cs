namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AutoTest.ArgNullEx.Filter;
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
            if (assemblyUnderTest == null)
                throw new ArgumentNullException("assemblyUnderTest");

            return assemblyUnderTest.Assembly;
        }

        private static IArgumentNullExceptionFixture CreateFixture(Assembly assemblyUnderTest)
        {
            var fixture =
                new Fixture().Customize(new AutoFixtureCustomizations())
                             .Customize(new NullTestsCustomization());

            return
                new ArgumentNullExceptionFixture(assemblyUnderTest, fixture)
                    .ExcludeType("AutoTest.ArgNullEx.Framework.ReflectionDiscoverableCollection`1")
                    .ExcludeType("AutoTest.ArgNullEx.Framework.ReflectionBlackList")
                    .ExcludeType("AutoTest.ArgNullEx.Framework.AssemblyTypesResolver")
                    .ExcludeType("System.Threading.Tasks.TaskHelpers")
                    .ExcludeType("System.Threading.Tasks.TaskHelpersExtensions")
                    .ExcludeType("System.Threading.Tasks.CatchInfoBase`1")
                    .ExcludeType("System.Threading.Tasks.CatchInfo")
                    .ExcludeType("System.Threading.Tasks.CatchInfo`1")
                    .ExcludeParameter("type", typeof(RegexFilterExtensions), "ExcludeMethod")
                    .ExcludeParameter("type", typeof(RegexFilterExtensions), "ExcludeParameter")
                    .ExcludeParameter("type", typeof(RegexFilterExtensions), "IncludeMethod")
                    .ExcludeParameter("type", typeof(RegexFilterExtensions), "IncludeParameter")
                    .ExcludeParameter("type", typeof(ArgumentNullExceptionFixtureExtensions), "ExcludeMethod")
                    .ExcludeParameter("type", typeof(ArgumentNullExceptionFixtureExtensions), "ExcludeParameter")
                    .ExcludeParameter("type", typeof(ArgumentNullExceptionFixtureExtensions), "IncludeMethod")
                    .ExcludeParameter("type", typeof(ArgumentNullExceptionFixtureExtensions), "IncludeParameter")
                    .ExcludeParameter("instanceUnderTest", typeof(MethodData), ".ctor")
                    .ExcludeParameter("nullParameter", typeof(MethodDataExtensions), "CheckException");
        }
    }
}
