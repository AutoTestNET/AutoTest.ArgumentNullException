namespace AutoTest.ArgNullEx.Xunit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// An attribute that can be applied to methods in an <see cref="RequiresArgumentNullExceptionAttribute"/>-driven
    /// Theory to indicate that all types should be excluded from the test unless otherwise included.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ExcludeAllAttribute : CustomizeAttribute, IArgNullExCustomization
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcludeAllAttribute" /> class.
        /// </summary>
        public ExcludeAllAttribute()
        {
            ExclusionType = ExclusionType.Types;
        }

        /// <summary> 
        /// Gets or sets the type of exclusion, the default is <see cref="Xunit.ExclusionType.Types"/>.
        /// </summary>
        public ExclusionType ExclusionType { get; set; }

        /// <summary>
        /// Gets a customization for a test method.
        /// </summary>
        /// <param name="method">The method to be customized.</param>
        /// <returns>A customization for a test method.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="method"/> parameter is
        /// <see langword="null"/>.</exception>
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
        /// <exception cref="ArgumentNullException">The <paramref name="fixture"/> parameter is
        /// <see langword="null"/>.</exception>
        public virtual void Customize(IArgumentNullExceptionFixture fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");

            if (ExclusionType.HasFlag(ExclusionType.Types))
                fixture.ExcludeAllTypes();

            if (ExclusionType.HasFlag(ExclusionType.Methods))
                fixture.ExcludeAllMethods();

            if (ExclusionType.HasFlag(ExclusionType.Parameters))
                fixture.ExcludeAllParameters();
        }
    }
}
