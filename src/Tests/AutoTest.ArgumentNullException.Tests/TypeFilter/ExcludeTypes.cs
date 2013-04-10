namespace AutoTest.ArgNullEx.TypeFilter
{
    using System;
    using System.Collections.Generic;
    using AutoTest.ArgNullEx.Framework;

    public class ExcludeTypes : FilterBase, ITypeFilter
    {
        public static ISet<Type> TypesToExclude = new HashSet<Type>
            {
                typeof(ReflectionDiscoverableCollection<>),
            };

        bool ITypeFilter.IncludeType(Type type)
        {
            return !TypesToExclude.Contains(type);
        }
    }
}
