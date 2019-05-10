// Copyright (c) 2013 - 2017 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace AutoTest.ArgNullEx.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides a mechanism for substituting one <see cref="Type"/> for another.
    /// </summary>
    internal class SubstituteType : TypeMappingBase
    {
        /// <summary>
        /// The the mappings for substituting one <see cref="Type"/> for another.
        /// </summary>
        private readonly Dictionary<Type, Type> _substitutions = new Dictionary<Type, Type>();

        /// <summary>
        /// Adds a substitution mapping from the <paramref name="originalType"/> to the <paramref name="newType"/>.
        /// </summary>
        /// <param name="originalType">The original <see cref="Type"/>.</param>
        /// <param name="newType">The new <see cref="Type"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="originalType"/> or <paramref name="newType"/>
        /// parameters are <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">A substitution for the <paramref name="originalType"/> already
        /// exists.</exception>
        public void Substitute(Type originalType, Type newType)
        {
            if (originalType == null)
                throw new ArgumentNullException(nameof(originalType));
            if (newType == null)
                throw new ArgumentNullException(nameof(newType));

            try
            {
                _substitutions.Add(originalType, newType);
            }
            catch (ArgumentException ex)
            {
                string message =
                    string.Format(
                        "Unable to add a substitute for the type '{0}' to the type '{1}', one may already exist.",
                        originalType,
                        newType);
                throw new InvalidOperationException(message, ex);
            }
        }

        /// <summary>
        /// If specified, a <paramref name="newType"/> is substituted for the <paramref name="originalType"/>.
        /// </summary>
        /// <param name="originalType">The original <see cref="Type"/>.</param>
        /// <param name="newType">The out result of the new <see cref="Type"/>.</param>
        /// <returns><see langword="true"/> if the <paramref name="originalType"/> is to be mapped to a
        /// <paramref name="newType"/>; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="originalType"/> parameter is
        /// <see langword="null"/>.</exception>
        protected override bool MapTo(Type originalType, out Type newType)
        {
            if (originalType == null)
                throw new ArgumentNullException(nameof(originalType));

            return _substitutions.TryGetValue(originalType, out newType);
        }
    }
}
