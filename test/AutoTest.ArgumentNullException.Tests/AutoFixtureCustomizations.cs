namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AutoFixture;
    using AutoFixture.AutoMoq;
    using AutoFixture.Kernel;

    public class ReflectionCustomization : ICustomization
    {
        private static readonly MethodInfo MethodInfo = GetMethodInfo();

        private static MethodInfo GetMethodInfo()
        {
            MethodInfo method =
                typeof(ReflectionCustomization).GetMethod(
                    "GetMethodInfo",
                    BindingFlags.NonPublic | BindingFlags.Static);
            return method;
        }

        void ICustomization.Customize(IFixture fixture)
        {
            fixture.Inject(MethodInfo);
            fixture.Customize<MethodBase>(
                composer => composer.FromFactory(new TypeRelay(typeof(MethodBase), typeof(MethodInfo))));
            fixture.Inject(GetType().GetTypeInfo().Assembly);
        }
    }

    public class NullTestsCustomization : ICustomization
    {
        private static readonly ParameterInfo ParameterInfo = GetParameterInfo();

        // ReSharper disable once UnusedParameter.Local
        private static ParameterInfo GetParameterInfo(object unused = null)
        {
            MethodInfo method =
                typeof(NullTestsCustomization).GetMethod("GetParameterInfo",
                                                          BindingFlags.NonPublic | BindingFlags.Static);

            return method.GetParameters().Single();
        }

        void ICustomization.Customize(IFixture fixture)
        {
            fixture.Inject(ParameterInfo);
        }
    }

    public class AutoFixtureCustomizations : CompositeCustomization
    {
        public AutoFixtureCustomizations()
            : base(new ReflectionCustomization(), new AutoMoqCustomization())
        {
        }
    }
}
