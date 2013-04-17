namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AutoTest.ArgNullEx.Execution;
    using AutoTest.ArgNullEx.Framework;
    using Ploeh.AutoFixture.Kernel;
    using Ploeh.AutoFixture.Xunit;
    using Xunit.Extensions;

    /// <summary>
    /// Test Attribute to prove methods correctly throw <see cref="ArgumentNullException"/>s.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RequiresArgumentNullExceptionAttribute : InlineAutoDataAttribute
    {
        /// <summary>
        /// The assembly under test.
        /// </summary>
        private readonly Assembly _assemblyUnderTest;

        /// <summary>
        /// The auto discovered list of filters.
        /// </summary>
        private readonly IDiscoverableCollection<IFilter> _filters;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiresArgumentNullExceptionAttribute"/> class.
        /// </summary>
        /// <param name="assemblyUnderTest">A type in the assembly under test.</param>
        public RequiresArgumentNullExceptionAttribute(Type assemblyUnderTest)
            : this(new AutoDataAttribute(), assemblyUnderTest != null ? assemblyUnderTest.Assembly : null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiresArgumentNullExceptionAttribute"/> class.
        /// </summary>
        /// <param name="autoDataAttribute">An <see cref="AutoDataAttribute"/>.</param>
        /// <param name="assemblyUnderTest">The assembly under test.</param>
        protected RequiresArgumentNullExceptionAttribute(
            AutoDataAttribute autoDataAttribute,
            Assembly assemblyUnderTest)
            : base(autoDataAttribute, new object[] { })
        {
            if (assemblyUnderTest == null) throw new ArgumentNullException("assemblyUnderTest");

            _assemblyUnderTest = assemblyUnderTest;
            _filters = new ReflectionDiscoverableCollection<IFilter>();
        }

        /// <summary>
        /// Gets the list of <see cref="ITypeFilter"/> objects.
        /// </summary>
        public IEnumerable<ITypeFilter> TypeFilters
        {
            get { return _filters.OfType<ITypeFilter>(); }
        }

        /// <summary>
        /// Gets the list of <see cref="IMethodFilter"/> objects.
        /// </summary>
        public IEnumerable<IMethodFilter> MethodFilters
        {
            get { return _filters.OfType<IMethodFilter>(); }
        }

        /// <summary>
        /// Gets the list of <see cref="IParameterFilter"/> objects.
        /// </summary>
        public IEnumerable<IParameterFilter> ParameterFilters
        {
            get { return _filters.OfType<IParameterFilter>(); }
        }

        /// <summary>
        /// Returns the data for the test <see cref="TheoryAttribute"/>.
        /// </summary>
        /// <param name="methodUnderTest">The test method under test.</param>
        /// <param name="parameterTypes">The types of the parameters.</param>
        /// <returns>The data for the test <see cref="TheoryAttribute"/>.</returns>
        public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest, Type[] parameterTypes)
        {
            if (methodUnderTest == null) throw new ArgumentNullException("methodUnderTest");
            if (parameterTypes == null) throw new ArgumentNullException("parameterTypes");

            _filters.Discover();

            return
                from type in GetTypesInAssembly(_assemblyUnderTest, TypeFilters)
                from method in GetMethodsInType(type, MethodFilters)
                from data in SetupParameterData(type, method)
                select new object[] { data };
        }

        /// <summary>
        /// Executes the <paramref name="filter"/> on the <paramref name="type"/>, logging information if it was excluded.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="filter">The <see cref="Type"/> filter.</param>
        /// <returns>The result of <see cref="ITypeFilter.IncludeType"/>.</returns>
        private static bool IncludeType(Type type, ITypeFilter filter)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (filter == null) throw new ArgumentNullException("filter");

            bool includeType = filter.IncludeType(type);
            if (!includeType)
            {
                System.Diagnostics.Trace.TraceInformation(
                    "The type '{0}' was excluded by the filter '{1}'.",
                    type,
                    filter.Name);
            }

            return includeType;
        }

        /// <summary>
        /// Gets all the types in the <paramref name="assembly"/> limited by the <paramref name="filters"/>.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> from which to retrieve the types.</param>
        /// <param name="filters">The collection of filters to limit the types.</param>
        /// <returns>All the types in the <paramref name="assembly"/> limited by the <paramref name="filters"/>.</returns>
        private static IEnumerable<Type> GetTypesInAssembly(Assembly assembly, IEnumerable<ITypeFilter> filters)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (filters == null) throw new ArgumentNullException("filters");

            return filters.Aggregate(
                assembly.GetTypes().AsEnumerable(),
                (current, filter) => current.Where(type => IncludeType(type, filter)));
        }

        /// <summary>
        /// Executes the <paramref name="filter"/> on the <paramref name="method"/>, logging information if it was excluded.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        /// <param name="filter">The <see cref="Type"/> filter.</param>
        /// <returns>The result of <see cref="IMethodFilter.IncludeMethod"/>.</returns>
        private static bool IncludeMethod(Type type, MethodBase method, IMethodFilter filter)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (method == null) throw new ArgumentNullException("method");
            if (filter == null) throw new ArgumentNullException("filter");

            bool includeMethod = filter.IncludeMethod(type, method);
            if (!includeMethod)
            {
                System.Diagnostics.Trace.TraceInformation(
                    "The method '{0}.{1}' was excluded by the filter '{2}'.",
                    type.Name,
                    method.Name,
                    filter.Name);
            }

            return includeMethod;
        }

        /// <summary>
        /// Gets all the methods in the <paramref name="type"/> limited by the <paramref name="filters"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> from which to retrieve the methods.</param>
        /// <param name="filters">The collection of filters to limit the methods.</param>
        /// <returns>All the methods in the <paramref name="type"/> limited by the <paramref name="filters"/>.</returns>
        private static IEnumerable<MethodInfo> GetMethodsInType(Type type, IEnumerable<IMethodFilter> filters)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (filters == null) throw new ArgumentNullException("filters");

            return filters.Aggregate(
                type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                    .AsEnumerable(),
                (current, filter) => current.Where(method => IncludeMethod(type, method, filter)));
        }

        /// <summary>
        /// Executes the <paramref name="filter"/> on the <paramref name="method"/>, logging information if it was excluded.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="filter">The <see cref="Type"/> filter.</param>
        /// <returns>The result of <see cref="IMethodFilter.IncludeMethod"/>.</returns>
        private static bool IncludeParameter(Type type, MethodBase method, ParameterInfo parameter, IParameterFilter filter)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (method == null) throw new ArgumentNullException("method");
            if (parameter == null) throw new ArgumentNullException("parameter");
            if (filter == null) throw new ArgumentNullException("filter");

            bool includeParameter = filter.IncludeParameter(type, method, parameter);
            if (!includeParameter)
            {
                System.Diagnostics.Trace.TraceInformation(
                    "The parameter '{0}.{1}({2})' was excluded by the filter '{3}'.",
                    type.Name,
                    method.Name,
                    parameter.Name,
                    filter.Name);
            }

            return includeParameter;
        }

        /// <summary>
        /// Sets up the parameter data for the <paramref name="method"/> on the <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> the method belongs to.</param>
        /// <param name="method">The method.</param>
        /// <returns>The parameter data for the <paramref name="method"/> on the <paramref name="type"/>.</returns>
        private IEnumerable<MethodData> SetupParameterData(Type type, MethodInfo method)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (method == null) throw new ArgumentNullException("method");

            ParameterInfo[] parameterInfos = method.GetParameters();
            var data = new List<MethodData>(parameterInfos.Length);

            Type[] methodParameterTypes = parameterInfos.Select(p => p.ParameterType).ToArray();

            for (int parameterIndex = 0; parameterIndex < parameterInfos.Length; ++parameterIndex)
            {
                ParameterInfo parameterInfo = parameterInfos[parameterIndex];

                // Run the filters against the parameter.
                if (ParameterFilters.Any(filter => !IncludeParameter(type, method, parameterInfo, filter)))
                    continue;

                try
                {
                    object[] parameters = base.GetData(method, methodParameterTypes).Single();
                    parameters[parameterIndex] = null;

                    object instanceUnderTest = null;
                    if (!method.IsStatic)
                    {
                        var context = new SpecimenContext(AutoDataAttribute.Fixture);
                        instanceUnderTest = context.Resolve(new SeededRequest(type, null));
                    }

                    data.Add(
                        new MethodData(
                            classUnderTest: type,
                            instanceUnderTest: instanceUnderTest,
                            methodUnderTest: method,
                            parameters: parameters,
                            nullParameter: parameterInfo.Name,
                            nullIndex: parameterIndex,
                            executionSetup: new DefaultExecutionSetup()));
                }
                catch (Exception ex)
                {
                    data.Add(
                        new MethodData(
                            classUnderTest: type,
                            instanceUnderTest: null,
                            methodUnderTest: method,
                            parameters: new object[] { },
                            nullParameter: parameterInfo.Name,
                            nullIndex: parameterIndex,
                            executionSetup: new ErroredExecutionSetup(ex)));
                }
            }

            return data;
        }
    }
}
