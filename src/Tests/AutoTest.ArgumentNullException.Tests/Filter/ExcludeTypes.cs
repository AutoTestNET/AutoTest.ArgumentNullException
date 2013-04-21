namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoTest.ArgNullEx.Framework;

    public class ExcludeTypes : FilterBase, ITypeFilter
    {
        private static readonly ISet<Type> TypesToExclude = new HashSet<Type>
            {
                typeof(ReflectionDiscoverableCollection<>),
                typeof(TaskHelpers),
                typeof(TaskHelpersExtensions),
                typeof(CatchInfoBase<>),
                typeof(CatchInfo),
                typeof(CatchInfo<>),
            };

        bool ITypeFilter.IncludeType(Type type)
        {
            return !TypesToExclude.Contains(type);
        }
    }
}
