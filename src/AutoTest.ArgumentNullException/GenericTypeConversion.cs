namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Threading;
    using AutoTest.ArgNullEx.Lock;

    /// <summary>
    /// Helper class to generate dynamic types based on generic types.
    /// </summary>
    internal static class GenericTypeConversion
    {
        /// <summary>
        /// The lock to prevent concurrent write access but allow multiple read access.
        /// </summary>
        private static readonly ReaderWriterLockSlim Lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        /// <summary>
        /// The dictionary of generic types to non generic types.
        /// </summary>
        private static readonly Dictionary<Type, Type> TypesMap = new Dictionary<Type, Type>();

        /// <summary>
        /// The module used for building generic types.
        /// </summary>
        private static ModuleBuilder _moduleBuilder;

        /// <summary>
        /// Gets the <see cref="System.Reflection.Emit.ModuleBuilder"/>.
        /// </summary>
        private static ModuleBuilder ModuleBuilder
        {
            get
            {
                if (null == _moduleBuilder)
                {
                    var assemblyName = new AssemblyName("TypesMapping_" + Guid.NewGuid().ToString("N"))
                    {
                        Version = Assembly.GetExecutingAssembly().GetName().Version,
                    };

                    AssemblyBuilder assemblyBuilder =
                        AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

                    _moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);
                }

                return _moduleBuilder;
            }
        }

        /// <summary>
        /// Gets a non generic type for the specified <paramref name="genericType"/>.
        /// </summary>
        /// <param name="genericType">The generic type.</param>
        /// <returns>A non generic type for the specified <paramref name="genericType"/>.</returns>
        internal static Type GetNonGenericType(Type genericType)
        {
            if (genericType == null)
                throw new ArgumentNullException("genericType");

            using (new ReadLockDisposable(Lock))
            {
                // With the read lock see if the type already exists.
                Type nonGenericType;
                if (TypesMap.TryGetValue(genericType, out nonGenericType))
                    return nonGenericType;
            }

            using (new WriteLockDisposable(Lock))
            {
                // Now with a write lock see of the type has been added by another thread.
                Type nonGenericType;
                if (TypesMap.TryGetValue(genericType, out nonGenericType))
                    return nonGenericType;

                if (TrySimple(genericType, out nonGenericType))
                {
                    TypesMap.Add(genericType, nonGenericType);
                    return nonGenericType;
                }

                // Since the copy type was no found generate it.
                nonGenericType = GenerateNonGenericType(genericType);

                TypesMap.Add(genericType, nonGenericType);

                return nonGenericType;
            }
        }

        /// <summary>
        /// Tries a simple solution to
        /// </summary>
        /// <param name="genericType">The generic type.</param>
        /// <param name="nonGenericType">The non generic type for the <paramref name="genericType"/>.</param>
        /// <returns><c>true</c> if a <paramref name="nonGenericType"/> can be used; otherwise <c>false</c>.</returns>
        private static bool TrySimple(Type genericType, out Type nonGenericType)
        {
            if (genericType == null)
                throw new ArgumentNullException("genericType");

            Type[] constraints = genericType.GetGenericParameterConstraints();
            GenericParameterAttributes attributes = genericType.GenericParameterAttributes;

            // Handle the simple situation where there are no constraints.
            if (constraints.Length == 0)
            {
                if (attributes == GenericParameterAttributes.None
                    || attributes.HasFlag(GenericParameterAttributes.ReferenceTypeConstraint))
                {
                    nonGenericType = typeof(object);
                    return true;
                }
            }

            // Handle the simple situation where the single constraint.
            if (constraints.Length == 1)
            {
                Type constraint = constraints[0];
                if (constraint == typeof(ValueType))
                {
                    nonGenericType = typeof(int);
                    return true;
                }

                // If there is a single constraint with no attributes, just use the constraint as the non generic type.
                if (attributes == GenericParameterAttributes.None)
                {
                    nonGenericType = constraint;
                    return true;
                }
            }

            nonGenericType = null;
            return false;
        }

        /// <summary>
        /// Generates a non generic type for the specified <paramref name="genericType"/>.
        /// </summary>
        /// <param name="genericType">The generic type.</param>
        /// <returns>A non generic type for the specified <paramref name="genericType"/>.</returns>
        private static Type GenerateNonGenericType(Type genericType)
        {
            if (genericType == null)
                throw new ArgumentNullException("genericType");

            Type[] constraints = genericType.GetGenericParameterConstraints();

            // Cannot yet handle mixed constraints, so just return the generic type which will cause a failure later.
            if (constraints.Any(c => !c.IsInterface))
                return genericType;

            TypeBuilder builder =
                ModuleBuilder.DefineType(
                    string.Format("{0}_NonGeneric_{1:N}", genericType.Name, Guid.NewGuid()),
                    TypeAttributes.Interface | TypeAttributes.Abstract | TypeAttributes.Public);

            foreach (Type constraint in constraints)
            {
                builder.AddInterfaceImplementation(constraint);
            }

            return builder.CreateType();
        }
    }
}
