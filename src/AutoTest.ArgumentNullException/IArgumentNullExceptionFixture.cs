namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// A custom fixture to generate the parameter specimens to execute methods to ensure they correctly throw <see cref="ArgumentNullException"/> errors.
    /// </summary>
    public interface IArgumentNullExceptionFixture
    {
        /// <summary>
        /// Gets or sets the flags that control binding and the way in which the search for members and types is conducted by reflection.
        /// </summary>
        BindingFlags BindingFlags { get; set; }

        /// <summary>
        /// Gets the list of filters.
        /// </summary>
        List<IFilter> Filters { get; }

        /// <summary>
        /// Returns the data for the methods to test.
        /// </summary>
        /// <returns>The data for the methods to test.</returns>
        IEnumerable<MethodData> GetData();
    }
}
