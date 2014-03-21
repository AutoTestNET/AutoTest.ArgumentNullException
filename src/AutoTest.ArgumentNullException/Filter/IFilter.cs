namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The base for all filters.
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// Gets the name of the filter.
        /// </summary>
        string Name { get; }
    }
}
