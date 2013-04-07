namespace AutoTest.ArgNullEx
{
    /// <summary>
    /// Base implementation of <see cref="IFilter"/> providing default behaviour.
    /// </summary>
    public abstract class FilterBase : IFilter
    {
        /// <summary>
        /// Gets the name of the filter. The default is to use <see cref="object.ToString"/>.
        /// </summary>
        public virtual string Name
        {
            get { return ToString(); }
        }
    }
}
