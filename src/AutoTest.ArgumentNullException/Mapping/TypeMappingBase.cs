// Copyright (c) 2013 - 2017 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace AutoTest.ArgNullEx.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Base implementation of <see cref="ITypeMapping"/> providing default behavior.
    /// </summary>
    public abstract class TypeMappingBase : MappingBase, ITypeMapping
    {
        /// <inheritdoc/>
        public virtual Type MapTo(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            Type newType;
            return MapTo(type, out newType) ? newType : type;
        }

        /// <summary>
        /// Implemented in derived types to provide a mapping from the <paramref name="originalType"/> to the
        /// <paramref name="newType"/>.
        /// </summary>
        /// <param name="originalType">The original <see cref="Type"/>.</param>
        /// <param name="newType">The out result of the new <see cref="Type"/>.</param>
        /// <returns><see langword="true"/> if the <paramref name="originalType"/> is to be mapped to a
        /// <paramref name="newType"/>; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="originalType"/> parameter is
        /// <see langword="null"/>.</exception>
        protected abstract bool MapTo(Type originalType, out Type newType);
    }
}
