namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
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
        /// The auto discovered list of type filters.
        /// </summary>
        private readonly IDiscoverableCollection<ITypeFilter> _typeFilters;

        /// <summary>
        /// The auto discovered list of type filters.
        /// </summary>
        private readonly IDiscoverableCollection<IMethodFilter> _methodFilters;

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
            _methodFilters = new ReflectionDiscoverableCollection<IMethodFilter>();
            _typeFilters = new ReflectionDiscoverableCollection<ITypeFilter>();
        }

        /// <summary>
        /// Gets the list of type filters.
        /// </summary>
        public IEnumerable<ITypeFilter> TypeFilters
        {
            get { return _typeFilters; }
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

            _methodFilters.Discover();
            _typeFilters.Discover();

            return
                from type in GetTypesInAssembly(_assemblyUnderTest, _typeFilters)
                from method in GetMethodsInType(type, _methodFilters)
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
            if (includeType == false)
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
        private static bool IncludeMethod(Type type, MethodInfo method, IMethodFilter filter)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (method == null) throw new ArgumentNullException("method");
            if (filter == null) throw new ArgumentNullException("filter");

            bool includeMethod = filter.IncludeMethod(type, method);
            if (includeMethod == false)
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
        /// Executes the <paramref name="methodUnderTest"/> synchronously.
        /// </summary>
        /// <param name="methodUnderTest">The method information.</param>
        /// <param name="parameters">The parameters to the <paramref name="methodUnderTest"/>.</param>
        /// <param name="sut">The system under tests, can be <c>null</c> if the <paramref name="methodUnderTest"/> is static.</param>
        /// <returns>The <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        private static async Task ExecuteAsynchronously(MethodBase methodUnderTest, object[] parameters, object sut = null)
        {
            if (methodUnderTest == null) throw new ArgumentNullException("methodUnderTest");
            if (parameters == null) throw new ArgumentNullException("parameters");

            try
            {
                var result = (Task)methodUnderTest.Invoke(sut, parameters);
                await result;
            }
            catch (TargetInvocationException targetInvocationException)
            {
                if (targetInvocationException.InnerException == null) throw;

                throw targetInvocationException.InnerException;
            }
        }

        /// <summary>
        /// Executes the <paramref name="methodUnderTest"/> synchronously.
        /// </summary>
        /// <param name="methodUnderTest">The method information.</param>
        /// <param name="parameters">The parameters to the <paramref name="methodUnderTest"/>.</param>
        /// <param name="sut">The system under tests, can be <c>null</c> if the <paramref name="methodUnderTest"/> is static.</param>
        private static void ExecuteSynchronously(MethodBase methodUnderTest, object[] parameters, object sut = null)
        {
            if (methodUnderTest == null) throw new ArgumentNullException("methodUnderTest");
            if (parameters == null) throw new ArgumentNullException("parameters");

            try
            {
                methodUnderTest.Invoke(sut, parameters);
            }
            catch (TargetInvocationException targetInvocationException)
            {
                if (targetInvocationException.InnerException == null) throw;

                throw targetInvocationException.InnerException;
            }
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

            for (int i = 0; i < parameterInfos.Length; ++i)
            {
                ParameterInfo parameterInfo = parameterInfos[i];

                // Do nothing if the parameter is not nullable, or is defaulted to null.
                if (!parameterInfo.IsNullable() || parameterInfo.HasNullDefault()) continue;

                try
                {
                    object[] arguments = base.GetData(method, methodParameterTypes).Single();
                    arguments[i] = null;

                    Tuple<object, Action, Func<Task>> executingAction = GetExecutingAction(method, type, arguments);
                    if (executingAction.Item2 != null)
                    {
                        data.Add(
                            new MethodData(
                                classUnderTest: type,
                                instanceUnderTest: executingAction.Item1,
                                methodUnderTest: method,
                                arguments: arguments,
                                nullArgument: parameterInfo.Name,
                                nullIndex: i,
                                executingActionSync: executingAction.Item2));
                    }
                    else
                    {
                        data.Add(
                            new MethodData(
                                classUnderTest: type,
                                instanceUnderTest: executingAction.Item1,
                                methodUnderTest: method,
                                arguments: arguments,
                                nullArgument: parameterInfo.Name,
                                nullIndex: i,
                                executingActionAsync: executingAction.Item3));
                    }
                }
                catch (Exception ex)
                {
                    data.Add(
                        new MethodData(
                            classUnderTest: type,
                            instanceUnderTest: null,
                            methodUnderTest: method,
                            arguments: new object[] { },
                            nullArgument: parameterInfo.Name,
                            nullIndex: 0,
                            executingActionSync: () => { throw ex; }));
                }
            }

            return data;
        }

        /// <summary>
        /// Returns either the <see cref="MethodData.ExecutingActionSync"/> or the <see cref="MethodData.ExecutingActionAsync"/> for the method invocation.
        /// </summary>
        /// <param name="methodUnderTest">The method under test.</param>
        /// <param name="classUnderTest">The type of the call under test.</param>
        /// <param name="arguments">The arguments to the <paramref name="methodUnderTest"/>.</param>
        /// <returns>Either the <see cref="MethodData.ExecutingActionSync"/> or the <see cref="MethodData.ExecutingActionAsync"/> for the method invocation.</returns>
        private Tuple<object, Action, Func<Task>> GetExecutingAction(
            MethodInfo methodUnderTest,
            Type classUnderTest,
            object[] arguments)
        {
            if (methodUnderTest == null) throw new ArgumentNullException("methodUnderTest");
            if (classUnderTest == null) throw new ArgumentNullException("classUnderTest");
            if (arguments == null) throw new ArgumentNullException("arguments");

            object sut = null;
            if (!methodUnderTest.IsStatic)
            {
                var context = new SpecimenContext(AutoDataAttribute.Fixture);
                sut = context.Resolve(new SeededRequest(classUnderTest, null));
            }

            if (typeof(Task).IsAssignableFrom(methodUnderTest.ReturnType))
            {
                return Tuple.Create<object, Action, Func<Task>>(
                    sut,
                    null,
                    () => ExecuteAsynchronously(methodUnderTest, arguments, sut));
            }

            return Tuple.Create<object, Action, Func<Task>>(
                sut,
                () => ExecuteSynchronously(methodUnderTest, arguments, sut),
                null);
        }
    }
}
