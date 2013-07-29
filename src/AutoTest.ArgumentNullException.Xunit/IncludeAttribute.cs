namespace AutoTest.ArgNullEx.Xunit
{
    using System;

    /// <summary>
    /// An attribute that can be applied to methods in an <see cref="RequiresArgumentNullExceptionAttribute"/>-driven
    /// Theory to indicate that only the specified <see cref="IncludeAttribute.Type"/>, <see cref="IncludeAttribute.Method"/>
    /// and/or <see cref="IncludeAttribute.Parameter"/> must be included in the test.
    /// </summary>
    public class IncludeAttribute : ExcludeAllAttribute
    {
        /// <summary>
        /// Gets or sets the <see cref="System.Type"/> for checks for <see cref="ArgumentNullException"/>.
        /// Overrides any type rules that may exclude the <see cref="System.Type"/>.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Method"/> for checks for <see cref="ArgumentNullException"/>.
        /// Overrides any method rules that may exclude the <see cref="Method"/>.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Parameter"/> for checks for <see cref="ArgumentNullException"/>.
        /// Overrides any parameter rules that may exclude the <see cref="Parameter"/>.
        /// </summary>
        public string Parameter { get; set; }

        /// <summary>
        /// Customizes the specified <paramref name="fixture"/> by including only specified <see cref="IncludeAttribute.Type"/>,
        /// <see cref="IncludeAttribute.Method"/> and/or <see cref="IncludeAttribute.Parameter"/>.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        public override void Customize(IArgumentNullExceptionFixture fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");

            base.Customize(fixture);

            if (!string.IsNullOrWhiteSpace(Parameter))
            {
                fixture.IncludeParameter(Parameter, Type, Method);
                return;
            }

            if (!string.IsNullOrWhiteSpace(Method))
            {
                fixture.IncludeMethod(Method, Type);
                return;
            }

            if (Type == null)
                throw new InvalidOperationException("Either the Type, Method or Parameter must be specified.");

            fixture.IncludeType(Type);
        }
    }
}
