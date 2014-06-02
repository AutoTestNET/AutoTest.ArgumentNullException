namespace AutoTest.ArgNullEx.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Filters out <see cref="IEquatable{T}.Equals(T)"/> implementations,
    /// <see cref="IEqualityComparer{T}.Equals(T, T)"/> implementations, and <see cref="object.Equals(object)"/>
    /// overrides.
    /// </summary>
    internal class NotEqualsMethod : FilterBase, IMethodFilter
    {
        /// <summary>
        /// Filters out <see cref="IEquatable{T}.Equals(T)"/> implementations,
        /// <see cref="IEqualityComparer{T}.Equals(T, T)"/> implementations, and <see cref="object.Equals(object)"/>
        /// overrides.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        /// <returns><see langword="true"/> if the <paramref name="method"/> should be excluded;
        /// otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="type"/> or <paramref name="method"/> parameters
        /// are <see langword="null"/>.</exception>
        bool IMethodFilter.ExcludeMethod(Type type, MethodBase method)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (method == null)
                throw new ArgumentNullException("method");

            // Don't exclude non "Equals" methods.
            if (method.GetMethodName() != "Equals")
                return false;

            // Only methods of type MethodInfo can be Equals.
            var methodInfo = (MethodInfo)method;
            MethodInfo methodDefinition = methodInfo.GetBaseDefinition();

            // All the Equals return boolean, if not "These aren't the methods you're looking for."
            if (methodDefinition.ReturnType != typeof(bool))
                return false;

            // Exclude overrides of Object.Equals.
            if (methodDefinition.DeclaringType == typeof(object))
                return true;

            Type[] interfaces = type.GetInterfaces();
            return interfaces.Where(IsEqualsInterface).Any(t => IsImplementationOfEquals(methodInfo, t));
        }

        /// <summary>
        /// Determines if the <paramref name="type"/> is an implementation of either <see cref="IEquatable{T}"/> or
        /// <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="type">The interface type.</param>
        /// <returns><see langword="true"/> if the <paramref name="type"/> is an implementation of either
        /// <see cref="IEquatable{T}"/> or <see cref="IEqualityComparer{T}"/>; otherwise
        /// <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="type"/> parameter is
        /// <see langword="null"/>.</exception>
        private static bool IsEqualsInterface(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (!type.IsGenericType)
                return false;

            Type definition = type.GetGenericTypeDefinition();
            return definition == typeof(IEquatable<>) || definition == typeof(IEqualityComparer<>);
        }

        /// <summary>
        /// Determines if the <paramref name="method"/> is the Equals implementation of type
        /// <paramref name="interfaceImpl"/>.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="interfaceImpl">The <see cref="Type"/> of the interface implementation.</param>
        /// <returns><see langword="true"/> if the <paramref name="method"/> is the Equals implementation of type
        /// <paramref name="interfaceImpl"/>; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="method"/> or <paramref name="interfaceImpl"/>
        /// parameters are <see langword="null"/>.</exception>
        private static bool IsImplementationOfEquals(MethodBase method, Type interfaceImpl)
        {
            if (method == null)
                throw new ArgumentNullException("method");
            if (interfaceImpl == null)
                throw new ArgumentNullException("interfaceImpl");

            Type definition = interfaceImpl.GetGenericTypeDefinition();
            int equalsParamsCount = definition.GetMethod("Equals").GetParameters().Length;

            ParameterInfo[] prms = method.GetParameters();

            // There must be the correct number of parameters and they must all be the same type.
            if (prms.Length != equalsParamsCount || prms.All(p => p.ParameterType != prms[0].ParameterType))
                return false;

            // Check if this method's parameter is a match for the interface implementation.
            string implName =
                string.Format("{0}[[{1}]]", definition.FullName, prms[0].ParameterType.AssemblyQualifiedName);
            if (implName != interfaceImpl.FullName)
                return false;

            // If the method is public than is must be the implementation of the interface's Equals method.
            if (method.IsPublic)
                return true;

            // Check the method name for the explicit implementation.
            string explicitName =
                string.Format(
                    "{0}<{1}>.Equals",
                    definition.FullName.Replace("`1", string.Empty),
                    prms[0].ParameterType.FullName);
            return method.Name == explicitName;
        }
    }
}
