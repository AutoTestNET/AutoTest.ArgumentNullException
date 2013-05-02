namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Ploeh.AutoFixture.Kernel;

    /// <summary>
    /// Provides parameter and instance specimens for a null parameter invocation of a method.
    /// </summary>
    internal class SpecimenProvider : ISpecimenProvider
    {
        /// <summary>
        /// The specimen builder.
        /// </summary>
        private readonly ISpecimenBuilder _builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecimenProvider"/> class.
        /// </summary>
        /// <param name="builder">The specimen builder.</param>
        public SpecimenProvider(ISpecimenBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException("builder");

            _builder = builder;
        }

        /// <summary>
        /// Gets the <see cref="ISpecimenBuilder"/> used to create specimens.
        /// </summary>
        public ISpecimenBuilder Builder
        {
            get { return _builder; }
        }

        /// <summary>
        /// Gets the specimens for the <paramref name="parameters"/>.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="nullIndex">The index of the null parameter.</param>
        /// <returns>The specimens for the <paramref name="parameters"/>.</returns>
        object[] ISpecimenProvider.GetParameterSpecimens(IList<ParameterInfo> parameters, int nullIndex)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            if (parameters.Count == 0)
                throw new ArgumentException("There are no parameters", "parameters");
            if (nullIndex >= parameters.Count)
            {
                string error = string.Format(
                    "The nullIndex '{0}' is beyond the range of the parameters '{1}'.",
                    nullIndex,
                    parameters.Count);
                throw new ArgumentException(error, "nullIndex");
            }

            // Simple optimization, if the only parameter is to be null.
            if (parameters.Count == 1)
                return new object[] { null };

            var data = new object[parameters.Count];
            for (int parameterIndex = 0; parameterIndex < parameters.Count; ++parameterIndex)
            {
                if (parameterIndex == nullIndex)
                    continue;

                data[parameterIndex] = Resolve(parameters[parameterIndex]);
            }

            return data;
        }

        /// <summary>
        /// Creates an instance of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to create</param>
        /// <returns>The instance of the <paramref name="type"/>.</returns>
        object ISpecimenProvider.CreateInstance(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return Resolve(type);
        }

        /// <summary>
        /// Resolves the <paramref name="request"/> specimen.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <returns>The <paramref name="request"/> specimen.</returns>
        private object Resolve(object request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            return new SpecimenContext(_builder).Resolve(request);
        }
    }
}
