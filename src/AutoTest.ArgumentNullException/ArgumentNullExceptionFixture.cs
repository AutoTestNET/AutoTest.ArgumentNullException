namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AutoTest.ArgNullEx.Execution;
    using AutoTest.ArgNullEx.Filter;
    using AutoTest.ArgNullEx.Framework;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Kernel;

    /// <summary>
    /// A custom fixture to generate the parameter specimens to execute methods to ensure they correctly throw <see cref="ArgumentNullException"/> errors.
    /// </summary>
    public class ArgumentNullExceptionFixture : IArgumentNullExceptionFixture
    {
        /// <summary>
        /// The Default value for the <see cref="BindingFlags"/>.
        /// </summary>
        public const BindingFlags DefaultBindingFlags =
            BindingFlags.Instance
            | BindingFlags.Static
            | BindingFlags.Public
            | BindingFlags.NonPublic
            | BindingFlags.DeclaredOnly;

        /// <summary>
        /// The assembly under test.
        /// </summary>
        private readonly Assembly _assemblyUnderTest;

        /// <summary>
        /// The <see cref="IFixture"/> used to create specimens.
        /// </summary>
        private readonly IFixture _fixture;

        /// <summary>
        /// The list of filters.
        /// </summary>
        private readonly List<IFilter> _filters;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentNullExceptionFixture"/> class.
        /// </summary>
        /// <param name="assemblyUnderTest">A <see cref="Type"/> in the assembly under test.</param>
        public ArgumentNullExceptionFixture(Assembly assemblyUnderTest)
            : this(assemblyUnderTest, new Fixture(), DiscoverFilters())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentNullExceptionFixture" /> class.
        /// </summary>
        /// <param name="assemblyUnderTest">The assembly under test.</param>
        /// <param name="fixture">The fixture.</param>
        public ArgumentNullExceptionFixture(Assembly assemblyUnderTest, IFixture fixture)
            : this(assemblyUnderTest, fixture, DiscoverFilters())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentNullExceptionFixture" /> class.
        /// </summary>
        /// <param name="assemblyUnderTest">The assembly under test.</param>
        /// <param name="fixture">The fixture.</param>
        /// <param name="filters">The list of filters.</param>
        public ArgumentNullExceptionFixture(Assembly assemblyUnderTest, IFixture fixture, List<IFilter> filters)
        {
            if (assemblyUnderTest == null)
                throw new ArgumentNullException("assemblyUnderTest");
            if (fixture == null)
                throw new ArgumentNullException("fixture");
            if (filters == null)
                throw new ArgumentNullException("filters");

            _assemblyUnderTest = assemblyUnderTest;
            _fixture = fixture;
            _filters = filters;
            BindingFlags = DefaultBindingFlags;
        }

        /// <summary>
        /// Gets the assembly under test.
        /// </summary>
        public Assembly AssemblyUnderTest
        {
            get { return _assemblyUnderTest; }
        }

        /// <summary>
        /// Gets the <see cref="IFixture"/> used to create specimens.
        /// </summary>
        public IFixture Fixture
        {
            get { return _fixture; }
        }

        /// <summary>
        /// Gets the list of filters.
        /// </summary>
        public List<IFilter> Filters
        {
            get { return _filters; }
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
        /// Gets or sets the flags that control binding and the way in which the search for members and types is conducted by reflection.
        /// </summary>
        public BindingFlags BindingFlags { get; set; }

        /// <summary>
        /// Returns the data for the methods to test.
        /// </summary>
        /// <returns>The data for the methods to test.</returns>
        public IEnumerable<MethodData> GetData()
        {
            return
                from type in GetTypesInAssembly(_assemblyUnderTest, TypeFilters)
                from method in GetMethodsInType(type, BindingFlags, MethodFilters)
                from data in SetupParameterData(type, method)
                select data;
        }

        /// <summary>
        /// Discovers the list of filters using reflection.
        /// </summary>
        /// <returns>The list of filters.</returns>
        private static List<IFilter> DiscoverFilters()
        {
            var discoverableCollection = new ReflectionDiscoverableCollection<IFilter>();
            discoverableCollection.Discover();
            return discoverableCollection.Items;
        }

        /// <summary>
        /// Executes the <paramref name="filter"/> on the <paramref name="type"/>, logging information if it was excluded.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="filter">The <see cref="Type"/> filter.</param>
        /// <returns>The result of <see cref="ITypeFilter.ExcludeType"/>.</returns>
        private static bool ExcludeType(Type type, ITypeFilter filter)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (filter == null)
                throw new ArgumentNullException("filter");

            bool excludeType = filter.ExcludeType(type);
            if (excludeType)
            {
                System.Diagnostics.Trace.TraceInformation(
                    "The type '{0}' was excluded by the filter '{1}'.",
                    type,
                    filter.Name);
            }

            return excludeType;
        }

        /// <summary>
        /// Gets all the types in the <paramref name="assembly"/> limited by the <paramref name="filters"/>.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> from which to retrieve the types.</param>
        /// <param name="filters">The collection of filters to limit the types.</param>
        /// <returns>All the types in the <paramref name="assembly"/> limited by the <paramref name="filters"/>.</returns>
        private static IEnumerable<Type> GetTypesInAssembly(Assembly assembly, IEnumerable<ITypeFilter> filters)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");
            if (filters == null)
                throw new ArgumentNullException("filters");

            return filters.Aggregate(
                assembly.GetTypes().AsEnumerable(),
                (current, filter) => current.Where(type => !ExcludeType(type, filter))).ToArray();
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
            if (type == null)
                throw new ArgumentNullException("type");
            if (method == null)
                throw new ArgumentNullException("method");
            if (filter == null)
                throw new ArgumentNullException("filter");

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
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="System.Reflection.BindingFlags"/> that specify how the search is conducted.</param>
        /// <param name="filters">The collection of filters to limit the methods.</param>
        /// <returns>All the methods in the <paramref name="type"/> limited by the <paramref name="filters"/>.</returns>
        private static IEnumerable<MethodBase> GetMethodsInType(Type type, BindingFlags bindingAttr, IEnumerable<IMethodFilter> filters)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (filters == null)
                throw new ArgumentNullException("filters");

            return filters.Aggregate(
                type.GetMethods(bindingAttr).Cast<MethodBase>()
                .Union(type.GetConstructors(bindingAttr)),
                (current, filter) => current.Where(method => IncludeMethod(type, method, filter))).ToArray();
        }

        /// <summary>
        /// Executes the <paramref name="filter"/> on the <paramref name="method"/>, logging information if it was excluded.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="filter">The <see cref="Type"/> filter.</param>
        /// <returns>The result of <see cref="IMethodFilter.IncludeMethod"/>.</returns>
        private static bool ExcludeParameter(Type type, MethodBase method, ParameterInfo parameter, IParameterFilter filter)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (method == null)
                throw new ArgumentNullException("method");
            if (parameter == null)
                throw new ArgumentNullException("parameter");
            if (filter == null)
                throw new ArgumentNullException("filter");

            bool excludeParameter = filter.ExcludeParameter(type, method, parameter);
            if (excludeParameter)
            {
                System.Diagnostics.Trace.TraceInformation(
                    "The parameter '{0}.{1}({2})' was excluded by the filter '{3}'.",
                    type.Name,
                    method.Name,
                    parameter.Name,
                    filter.Name);
            }

            return excludeParameter;
        }

        /// <summary>
        /// Sets up the parameter data for the <paramref name="method"/> on the <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> the method belongs to.</param>
        /// <param name="method">The method.</param>
        /// <returns>The parameter data for the <paramref name="method"/> on the <paramref name="type"/>.</returns>
        private IEnumerable<MethodData> SetupParameterData(Type type, MethodBase method)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (method == null)
                throw new ArgumentNullException("method");

            ParameterInfo[] parameterInfos = method.GetParameters();
            var data = new List<MethodData>(parameterInfos.Length);

            for (int parameterIndex = 0; parameterIndex < parameterInfos.Length; ++parameterIndex)
            {
                ParameterInfo parameterInfo = parameterInfos[parameterIndex];

                // Run the filters against the parameter.
                if (ParameterFilters.Any(filter => ExcludeParameter(type, method, parameterInfo, filter)))
                    continue;

                try
                {
                    object[] parameters = GetParameterSpecimens(parameterInfos, parameterIndex);

                    object instanceUnderTest = null;
                    if (!method.IsStatic)
                    {
                        var context = new SpecimenContext(Fixture);
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
                    var compositionEx = new CompositionException(type, method, parameterInfo.Name, ex);
                    data.Add(
                        new MethodData(
                            classUnderTest: type,
                            instanceUnderTest: null,
                            methodUnderTest: method,
                            parameters: new object[] { },
                            nullParameter: parameterInfo.Name,
                            nullIndex: parameterIndex,
                            executionSetup: new ErroredExecutionSetup(compositionEx)));
                }
            }

            return data;
        }

        /// <summary>
        /// Gets the specimens for the <paramref name="parameters"/>.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="nullIndex">The index of the null parameter.</param>
        /// <returns>The specimens for the <paramref name="parameters"/>.</returns>
        private object[] GetParameterSpecimens(IList<ParameterInfo> parameters, int nullIndex)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            if (parameters.Count == 0)
                throw new ArgumentException("There are no parameters", "parameters");
            if (nullIndex >= parameters.Count)
            {
                string error = string.Format(
                    "The nullIndex '{0}' is beyond the range of the parameters '{1}'.",
                    nullIndex,
                    parameters.Count);
                throw new ArgumentException(error, "nullIndex");
            }

            // Simple optimization, if the only parameter is to be null.
            if (parameters.Count == 1)
                return new object[] { null };

            var data = new object[parameters.Count];
            for (int parameterIndex = 0; parameterIndex < parameters.Count; ++parameterIndex)
            {
                if (parameterIndex == nullIndex)
                    continue;

                data[parameterIndex] = Resolve(parameters[parameterIndex]);
            }

            return data;
        }

        /// <summary>
        /// Resolves the <paramref name="parameter"/> specimen.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>The <paramref name="parameter"/> specimen.</returns>
        private object Resolve(ParameterInfo parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException("parameter");

            var context = new SpecimenContext(_fixture);
            return context.Resolve(parameter);
        }
    }
}
