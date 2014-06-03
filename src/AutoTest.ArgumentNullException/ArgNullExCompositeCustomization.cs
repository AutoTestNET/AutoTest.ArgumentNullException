namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Customizes an <see cref="IArgNullExCustomization"/> by using all contained <see cref="Customizations"/>.
    /// </summary>
    public class ArgNullExCompositeCustomization : IArgNullExCustomization
    {
        /// <summary>
        /// The customizations contained within this instance.
        /// </summary>
        private readonly List<IArgNullExCustomization> _customizations;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgNullExCompositeCustomization"/> class.
        /// </summary>
        /// <param name="customizations">The customizations.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="customizations"/> parameter is <see langword="null"/>.</exception>
        public ArgNullExCompositeCustomization(IEnumerable<IArgNullExCustomization> customizations)
        {
            if (customizations == null)
                throw new ArgumentNullException("customizations");

            _customizations = customizations.ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgNullExCompositeCustomization"/> class.
        /// </summary>
        /// <param name="customizations">The customizations.</param>
        public ArgNullExCompositeCustomization(params IArgNullExCustomization[] customizations)
            : this(customizations.AsEnumerable())
        {
        }

        /// <summary>
        /// Gets the customizations contained within this instance.
        /// </summary>
        public IEnumerable<IArgNullExCustomization> Customizations
        {
            get { return _customizations; }
        }

        /// <summary>
        /// Customizes the specified fixture.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="fixture"/> parameter is <see langword="null"/>.</exception>
        public void Customize(IArgumentNullExceptionFixture fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");

            _customizations.ForEach(customization => customization.Customize(fixture));
        }
    }
}
