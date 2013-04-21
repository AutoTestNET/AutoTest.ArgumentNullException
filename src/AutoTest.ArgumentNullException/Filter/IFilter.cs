namespace AutoTest.ArgNullEx.Filter
{
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
