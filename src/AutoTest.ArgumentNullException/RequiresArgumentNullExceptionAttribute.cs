namespace AutoTest.ArgumentNullException
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
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
        /// Initializes a new instance of the <see cref="RequiresArgumentNullExceptionAttribute"/> class.
        /// </summary>
        /// <param name="typeInAssembly">A type in the assembly under test.</param>
        public RequiresArgumentNullExceptionAttribute(
            Type typeInAssembly)
            : this(new AutoDataAttribute(), typeInAssembly.Assembly)
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
        }

        /// <summary>
        /// Returns the data for the test <see cref="TheoryAttribute"/>.
        /// </summary>
        /// <param name="methodUnderTest">The test method under test.</param>
        /// <param name="parameterTypes">The types of the parameters.</param>
        /// <returns>The data for the test <see cref="TheoryAttribute"/>.</returns>
        public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest, Type[] parameterTypes)
        {
            var data = new List<MethodData>();

            IEnumerable<Type> classesInAut = _assemblyUnderTest.GetTypes().Where(t => t.IsClass && !IsCompilerGenerated(t)).ToArray();
            foreach (Type type in classesInAut)
            {
                MethodInfo[] methods = type.GetMethods();
                foreach (MethodInfo method in methods)
                {
                    if (method.Name == "Equals") continue;

                    ParameterInfo[] parameterInfos = method.GetParameters();

                    // If there are no parameters, or all the parameters are defaulted to null.
                    if (parameterInfos.Length == 0 || parameterInfos.All(pi => pi.HasDefaultValue && pi.DefaultValue == null)) continue;

                    try
                    {
                        Type[] methodParameterTypes = parameterInfos.Select(p => p.ParameterType).ToArray();

                        for (int i = 0; i < parameterInfos.Length; ++i)
                        {
                            ParameterInfo parameterInfo = parameterInfos[i];

                            if (parameterInfo.ParameterType.IsValueType) continue;
                            if (parameterInfo.HasDefaultValue && parameterInfo.DefaultValue == null) continue;

                            object[] arguments = base.GetData(method, methodParameterTypes).Single();
                            arguments[i] = null;

                            var methodData =
                                new MethodData
                                {
                                    ClassUnderTest = type,
                                    MethodUnderTest = method,
                                    Arguments = arguments,
                                    NullArgument = parameterInfo.Name,
                                    NullIndex = 0,
                                };
                            SetupExecutingAction(methodData);

                            data.Add(methodData);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return data.Select(d => new object[] { d });
        }

        /// <summary>
        /// Determines if a <paramref name="type"/> is compiler generated.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the type was compiler generated; otherwise <c>false</c>.</returns>
        private static bool IsCompilerGenerated(MemberInfo type)
        {
            if (type == null) throw new ArgumentNullException("type");

            if (type.GetCustomAttribute<CompilerGeneratedAttribute>() != null)
                return true;

            return type.DeclaringType != null && IsCompilerGenerated(type.DeclaringType);
        }

        /// <summary>
        /// Executes the <paramref name="methodUnderTest"/> synchronously.
        /// </summary>
        /// <param name="methodUnderTest">The method information.</param>
        /// <param name="sut">The system under tests, can be <c>null</c> if the <paramref name="methodUnderTest"/> is static.</param>
        /// <param name="parameters">The parameters to the <paramref name="methodUnderTest"/>.</param>
        /// <returns>The <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        private static async Task ExecuteAsynchronously(MethodBase methodUnderTest, object sut, object[] parameters)
        {
            try
            {
                var result = (Task)methodUnderTest.Invoke(sut, parameters);
                await result;
            }
            catch (TargetInvocationException targetInvocationException)
            {
                throw targetInvocationException.InnerException;
            }
        }

        /// <summary>
        /// Executes the <paramref name="methodUnderTest"/> synchronously.
        /// </summary>
        /// <param name="methodUnderTest">The method information.</param>
        /// <param name="sut">The system under tests, can be <c>null</c> if the <paramref name="methodUnderTest"/> is static.</param>
        /// <param name="parameters">The parameters to the <paramref name="methodUnderTest"/>.</param>
        private static void ExecuteSynchronously(MethodBase methodUnderTest, object sut, object[] parameters)
        {
            try
            {
                methodUnderTest.Invoke(sut, parameters);
            }
            catch (TargetInvocationException targetInvocationException)
            {
                throw targetInvocationException.InnerException;
            }
        }

        /// <summary>
        /// Sets up either the <see cref="MethodData.ExecutingActionSync"/> or the <see cref="MethodData.ExecutingActionAsync"/> for the method invocation.
        /// </summary>
        /// <param name="methodData">The method data.</param>
        private void SetupExecutingAction(MethodData methodData)
        {
            if (methodData == null) throw new ArgumentNullException("methodData");

            object sut = null;
            if (!methodData.MethodUnderTest.IsStatic)
            {
                var context = new SpecimenContext(AutoDataAttribute.Fixture);
                sut = context.Resolve(new SeededRequest(methodData.ClassUnderTest, null));
            }

            if (methodData.MethodUnderTest.ReturnType.IsSubclassOf(typeof(Task)))
            {
                methodData.ExecutingActionAsync = () => ExecuteAsynchronously(methodData.MethodUnderTest, sut, methodData.Arguments);
            }
            else
            {
                methodData.ExecutingActionSync = () => ExecuteSynchronously(methodData.MethodUnderTest, sut, methodData.Arguments);
            }
        }
    }
}
