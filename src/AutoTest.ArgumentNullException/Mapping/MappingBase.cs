namespace AutoTest.ArgNullEx.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Base implementation of <see cref="IMapping"/> providing default behavior.
    /// </summary>
    public abstract class MappingBase : IMapping
    {
        /// <summary>
        /// Gets the name of the Mapping. The default is to use <see cref="MemberInfo.Name"/>.
        /// </summary>
        public virtual string Name
        {
            get { return GetType().Name; }
        }
    }
}
