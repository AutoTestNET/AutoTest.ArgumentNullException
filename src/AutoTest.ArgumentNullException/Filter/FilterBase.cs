namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Base implementation of <see cref="IFilter"/> providing default behaviour.
    /// </summary>
    public abstract class FilterBase : IFilter
    {
        /// <summary>
        /// Gets the name of the filter. The default is to use <see cref="MemberInfo.Name"/>.
        /// </summary>
        public virtual string Name
        {
            get { return GetType().Name; }
        }
    }
}
