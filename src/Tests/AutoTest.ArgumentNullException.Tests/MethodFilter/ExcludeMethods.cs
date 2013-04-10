namespace AutoTest.ArgNullEx.MethodFilter
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class ExcludeMethods : FilterBase, IMethodFilter
    {
        public static ISet<MethodInfo> MethodsToExclude = new HashSet<MethodInfo>
            {
                typeof(RequiresArgumentNullExceptionAttribute).GetMethod("Match"),
            };

        bool IMethodFilter.IncludeMethod(Type type, MethodInfo method)
        {
            return !MethodsToExclude.Contains(method);
        }
    }
}
