namespace AutoTest.ArgNullEx.Xunit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// An attribute that can be applied to methods in an <see cref="RequiresArgumentNullExceptionAttribute"/>-driven
    /// Theory to indicate that only the specified <see cref="IncludeAttribute.Type"/>,
    /// <see cref="IncludeAttribute.Method"/> and/or <see cref="IncludeAttribute.Parameter"/> must be included in the
    /// test.
    /// </summary>
    public class IncludeAttribute : ExcludeAllAttribute
    {
        /// <summary>
        /// Gets or sets the <see cref="System.Type"/> for checks for <see cref="ArgumentNullException"/>.
        /// Overrides any type rules that may exclude the <see cref="System.Type"/>.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the full name of the <see cref="System.Type"/> for checks for
        /// <see cref="ArgumentNullException"/>.
        /// Overrides any type rules that may exclude the <see cref="System.Type"/>.
        /// </summary>
        public string TypeFullName { get; set; }

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
        /// Customizes the specified <paramref name="fixture"/> by including only specified
        /// <see cref="IncludeAttribute.Type"/>, <see cref="IncludeAttribute.Method"/> and/or
        /// <see cref="IncludeAttribute.Parameter"/>.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="fixture"/> parameter is
        /// <see langword="null"/>.</exception>
        public override void Customize(IArgumentNullExceptionFixture fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");

            if (Type != null && !string.IsNullOrWhiteSpace(TypeFullName))
            {
                throw new InvalidOperationException("Type and TypeFullName cannot both be specified.");
            }

            base.Customize(fixture);

            if (!string.IsNullOrWhiteSpace(Parameter))
            {
                if (!string.IsNullOrWhiteSpace(TypeFullName))
                {
                    fixture.IncludeParameter(Parameter, TypeFullName, Method);
                    return;
                }

                fixture.IncludeParameter(Parameter, Type, Method);
                return;
            }

            if (!string.IsNullOrWhiteSpace(Method))
            {
                if (!string.IsNullOrWhiteSpace(TypeFullName))
                {
                    fixture.IncludeMethod(Method, TypeFullName);
                    return;
                }

                fixture.IncludeMethod(Method, Type);
                return;
            }

            if (!string.IsNullOrWhiteSpace(TypeFullName))
            {
                fixture.IncludeType(TypeFullName);
                return;
            }

            if (Type == null)
                throw new InvalidOperationException("Either the Type, Method or Parameter must be specified.");

            fixture.IncludeType(Type);
        }
    }
}
