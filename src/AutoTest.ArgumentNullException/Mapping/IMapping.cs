namespace AutoTest.ArgNullEx.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The base for all mappings.
    /// </summary>
    public interface IMapping
    {
        /// <summary>
        /// Gets the name of the mapping.
        /// </summary>
        string Name { get; }
    }
}
