namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;
    using Ploeh.AutoFixture.Kernel;

    public class AsyncCustomization : ICustomization
    {
        /// <summary>
        /// <see cref="MethodData"/> has two public constructors with the same number of parameters.
        /// This method query is used to remove any uncertainty in which one AutoFixture selects.
        /// </summary>
        private class MethodDataConstructorSelector : IMethodQuery
        {
            public IEnumerable<IMethod> SelectMethods(Type type)
            {
                if (type == null) throw new ArgumentNullException("type");
                if (type != typeof(MethodData)) throw new ArgumentException(string.Format("The type must be '{0}'.", typeof(MethodData)), "type");

                var types = new[]
                    {
                        typeof (Type),
                        typeof (object),
                        typeof (MethodInfo),
                        typeof (object[]),
                        typeof (string),
                        typeof (int),
                        typeof (Action),
                    };

                return new IMethod[]
                    {
                        new ConstructorMethod(type.GetConstructor(types))
                    };
            }
        }

        /// <summary>
        /// Returns a completed task.
        /// </summary>
        /// <returns>A completed task.</returns>
        private static Task CompletedTask()
        {
            return Task.FromResult(0);
        }

        void ICustomization.Customize(IFixture fixture)
        {
            // Customize the MethodData to construct using the correct constructor
            // and setup the ExecutingActionAsync to return a completed task.
            fixture.Customize<MethodData>(
                composer => composer.FromFactory(new MethodInvoker(new MethodDataConstructorSelector()))
                                    .Do(m => m.ExecutingActionAsync = CompletedTask));
        }
    }

    public class NullTestsCustomization : ICustomization
    {
        private static readonly MethodInfo MethodInfo = GetParameterInfo().Item1;

        private static readonly ParameterInfo ParameterInfo = GetParameterInfo().Item2;

        private static Tuple<MethodInfo, ParameterInfo> GetParameterInfo(object unused = null)
        {
            var method =
                typeof(NullTestsCustomization).GetMethod("GetParameterInfo",
                                                          BindingFlags.NonPublic | BindingFlags.Static);

            return Tuple.Create(method, method.GetParameters().Single());
        }

        void ICustomization.Customize(IFixture fixture)
        {
            var throwingRecursionBehavior = fixture.Behaviors.OfType<ThrowingRecursionBehavior>().SingleOrDefault();
            if (throwingRecursionBehavior != null)
            {
                fixture.Behaviors.Remove(throwingRecursionBehavior);
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            }

            fixture.Inject(MethodInfo);
            fixture.Inject<MethodBase>(MethodInfo);
            fixture.Inject(ParameterInfo);
        }
    }

    public class AutoFixtureCustomizations : CompositeCustomization
    {
        public AutoFixtureCustomizations()
            : base(new AsyncCustomization(), new AutoMoqCustomization())
        {
        }
    }
}
