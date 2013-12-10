namespace AutoTest.ArgNullEx.Xunit
{
    using System;
    using System.Reflection;

    /// <summary>
    /// An attribute that can be applied to methods in an <see cref="RequiresArgumentNullExceptionAttribute"/>-driven
    /// Theory to indicate that the specified <see cref="ExcludeAttribute.Type"/>, <see cref="ExcludeAttribute.Method"/>
    /// and/or <see cref="ExcludeAttribute.Parameter"/> must be excluded from the test.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ExcludeAttribute : CustomizeAttribute, IArgNullExCustomization
    {
        /// <summary>
        /// Gets or sets the <see cref="System.Type"/> to exclude from checks for <see cref="ArgumentNullException"/>.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the full name of the <see cref="System.Type"/> for checks for <see cref="ArgumentNullException"/>.
        /// </summary>
        public string TypeFullName { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Method"/> to exclude from checks for <see cref="ArgumentNullException"/>.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Parameter"/> to exclude from checks for <see cref="ArgumentNullException"/>.
        /// </summary>
        public string Parameter { get; set; }

        /// <summary>
        /// Gets a customization for a test method.
        /// </summary>
        /// <param name="method">The method to be customized.</param>
        /// <returns>A customization for a test method.</returns>
        public override IArgNullExCustomization GetCustomization(MethodInfo method)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            return this;
        }

        /// <summary>
        /// Customizes the specified <paramref name="fixture"/> by excluding all types.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        public virtual void Customize(IArgumentNullExceptionFixture fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");

            if (Type != null && !string.IsNullOrWhiteSpace(TypeFullName))
            {
                throw new InvalidOperationException("Type and TypeFullName cannot both be specified.");
            }

            if (!string.IsNullOrWhiteSpace(Parameter))
            {
                if (!string.IsNullOrWhiteSpace(TypeFullName))
                {
                    fixture.ExcludeParameter(Parameter, TypeFullName, Method);
                    return;
                }

                fixture.ExcludeParameter(Parameter, Type, Method);
                return;
            }

            if (!string.IsNullOrWhiteSpace(Method))
            {
                if (!string.IsNullOrWhiteSpace(TypeFullName))
                {
                    fixture.ExcludeMethod(Method, TypeFullName);
                    return;
                }

                fixture.ExcludeMethod(Method, Type);
                return;
            }

            if (!string.IsNullOrWhiteSpace(TypeFullName))
            {
                fixture.ExcludeType(TypeFullName);
                return;
            }

            if (Type == null)
                throw new InvalidOperationException("Either the Type, Method or Parameter must be specified.");

            fixture.ExcludeType(Type);
        }
    }
}
