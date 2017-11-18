// Copyright (c) 2013 - 2017 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace AutoTest.ArgNullEx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AutoFixture;
    using AutoTest.ArgNullEx.Execution;
    using AutoTest.ArgNullEx.Filter;
    using AutoTest.ArgNullEx.Framework;
    using AutoTest.ArgNullEx.Mapping;

    /// <summary>
    /// A custom builder to generate the parameter specimens to execute methods to ensure they correctly throw
    /// <see cref="ArgumentNullException"/> errors.
    /// </summary>
    public class ArgumentNullExceptionFixture : IArgumentNullExceptionFixture
    {
        /// <summary>
        /// The Default value for the <see cref="BindingFlags"/>.
        /// </summary>
        public const BindingFlags DefaultBindingFlags =
            BindingFlags.Instance
            | BindingFlags.Static
            | BindingFlags.Public
            | BindingFlags.NonPublic
            | BindingFlags.DeclaredOnly;

        /// <summary>
        /// The assembly under test.
        /// </summary>
        private readonly Assembly _assemblyUnderTest;

        /// <summary>
        /// The parameter specimen provider.
        /// </summary>
        private readonly ISpecimenProvider _specimenProvider;

        /// <summary>
        /// The list of filters.
        /// </summary>
        private readonly List<IFilter> _filters;

        /// <summary>
        /// The list of filters.
        /// </summary>
        private readonly List<IMapping> _mappings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentNullExceptionFixture"/> class.
        /// </summary>
        /// <param name="assemblyUnderTest">A <see cref="Type"/> in the assembly under test.</param>
        public ArgumentNullExceptionFixture(Assembly assemblyUnderTest)
            : this(assemblyUnderTest, new Fixture(), DiscoverFilters(), DiscoverMappings())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentNullExceptionFixture" /> class.
        /// </summary>
        /// <param name="assemblyUnderTest">The assembly under test.</param>
        /// <param name="fixture">The specimen fixture.</param>
        public ArgumentNullExceptionFixture(Assembly assemblyUnderTest, IFixture fixture)
            : this(assemblyUnderTest, fixture, DiscoverFilters(), DiscoverMappings())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentNullExceptionFixture" /> class.
        /// </summary>
        /// <param name="assemblyUnderTest">The assembly under test.</param>
        /// <param name="fixture">The specimen fixture.</param>
        /// <param name="filters">The list of filters.</param>
        /// <param name="mappings">The list of mappings.</param>
        public ArgumentNullExceptionFixture(
            Assembly assemblyUnderTest,
            IFixture fixture,
            List<IFilter> filters,
            List<IMapping> mappings)
            : this(assemblyUnderTest, new SpecimenProvider(fixture), filters, mappings)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentNullExceptionFixture" /> class.
        /// </summary>
        /// <param name="assemblyUnderTest">The assembly under test.</param>
        /// <param name="specimenProvider">The specimen provider.</param>
        /// <param name="filters">The list of filters.</param>
        /// <param name="mappings">The list of mappings.</param>
        /// <exception cref="ArgumentNullException">Any of the parameters are <see langword="null"/>.</exception>
        public ArgumentNullExceptionFixture(
            Assembly assemblyUnderTest,
            ISpecimenProvider specimenProvider,
            List<IFilter> filters,
            List<IMapping> mappings)
        {
            if (assemblyUnderTest == null)
                throw new ArgumentNullException("assemblyUnderTest");
            if (specimenProvider == null)
                throw new ArgumentNullException("specimenProvider");
            if (filters == null)
                throw new ArgumentNullException("filters");
            if (mappings == null)
                throw new ArgumentNullException("mappings");

            _assemblyUnderTest = assemblyUnderTest;
            _specimenProvider = specimenProvider;
            _filters = filters;
            _mappings = mappings;
            BindingFlags = DefaultBindingFlags;
        }

        /// <summary>
        /// Gets the assembly under test.
        /// </summary>
        public Assembly AssemblyUnderTest
        {
            get { return _assemblyUnderTest; }
        }

        /// <summary>
        /// Gets the <see cref="ISpecimenProvider"/> used to create parameter specimens.
        /// </summary>
        public ISpecimenProvider SpecimenProvider
        {
            get { return _specimenProvider; }
        }

        /// <summary>
        /// Gets the list of filters.
        /// </summary>
        public List<IFilter> Filters
        {
            get { return _filters; }
        }

        /// <summary>
        /// Gets the list of mappings.
        /// </summary>
        public List<IMapping> Mappings
        {
            get { return _mappings; }
        }

        /// <summary>
        /// Gets the list of <see cref="ITypeFilter"/> objects.
        /// </summary>
        public IEnumerable<ITypeFilter> TypeFilters
        {
            get { return _filters.OfType<ITypeFilter>(); }
        }

        /// <summary>
        /// Gets the list of <see cref="IMethodFilter"/> objects.
        /// </summary>
        public IEnumerable<IMethodFilter> MethodFilters
        {
            get { return _filters.OfType<IMethodFilter>(); }
        }

        /// <summary>
        /// Gets the list of <see cref="IParameterFilter"/> objects.
        /// </summary>
        public IEnumerable<IParameterFilter> ParameterFilters
        {
            get { return _filters.OfType<IParameterFilter>(); }
        }

        /// <summary>
        /// Gets or sets the flags that control binding and the way in which the search for members and types is
        /// conducted by reflection.
        /// </summary>
        public BindingFlags BindingFlags { get; set; }

        /// <summary>
        /// Gets the list of <see cref="ITypeMapping"/> objects.
        /// </summary>
        internal IEnumerable<ITypeMapping> TypeMappings
        {
            get { return _mappings.OfType<ITypeMapping>(); }
        }

        /// <summary>
        /// Returns the data for the methods to test.
        /// </summary>
        /// <returns>The data for the methods to test.</returns>
        public IEnumerable<MethodData> GetData()
        {
            return
                from type in _assemblyUnderTest.GetTypes(TypeFilters).MapTypes(TypeMappings)
                from method in type.GetMethods(BindingFlags, MethodFilters)
                from data in SetupParameterData(type, method)
                select data;
        }

        /// <summary>
        /// Discovers the list of filters using reflection.
        /// </summary>
        /// <returns>The list of filters.</returns>
        private static List<IFilter> DiscoverFilters()
        {
            var discoverableCollection = new ReflectionDiscoverableCollection<IFilter>();
            discoverableCollection.Discover();
            return discoverableCollection.Items;
        }

        /// <summary>
        /// Discovers the list of mappings using reflection.
        /// </summary>
        /// <returns>The list of mappings.</returns>
        private static List<IMapping> DiscoverMappings()
        {
            var discoverableCollection = new ReflectionDiscoverableCollection<IMapping>();
            discoverableCollection.Discover();
            return discoverableCollection.Items;
        }

        /// <summary>
        /// Sets up the parameter data for the <paramref name="method"/> on the <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> the method belongs to.</param>
        /// <param name="method">The method.</param>
        /// <returns>The parameter data for the <paramref name="method"/> on the <paramref name="type"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="type"/> or <paramref name="method"/> parameters
        /// are <see langword="null"/>.</exception>
        private IEnumerable<MethodData> SetupParameterData(Type type, MethodBase method)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (method == null)
                throw new ArgumentNullException("method");

            ParameterInfo[] parameterInfos = method.GetParameters();
            var data = new List<MethodData>(parameterInfos.Length);

            for (int parameterIndex = 0; parameterIndex < parameterInfos.Length; ++parameterIndex)
            {
                ParameterInfo parameterInfo = parameterInfos[parameterIndex];

                // Apply the filters against the parameter.
                if (ParameterFilters.Any(filter => filter.ApplyFilter(type, method, parameterInfo)))
                    continue;

                try
                {
                    object[] parameters = _specimenProvider.GetParameterSpecimens(parameterInfos, parameterIndex);

                    object instanceUnderTest = null;
                    if (!method.IsStatic)
                    {
                        instanceUnderTest = _specimenProvider.CreateInstance(type);
                    }

                    data.Add(
                        new MethodData(
                            classUnderTest: type,
                            instanceUnderTest: instanceUnderTest,
                            methodUnderTest: method,
                            parameters: parameters,
                            nullParameter: parameterInfo.Name,
                            nullIndex: parameterIndex,
                            executionSetup: new DefaultExecutionSetup()));
                }
                catch (Exception ex)
                {
                    var compositionEx = new CompositionException(type, method, parameterInfo.Name, ex);
                    data.Add(
                        new MethodData(
                            classUnderTest: type,
                            instanceUnderTest: null,
                            methodUnderTest: method,
                            parameters: new object[] { },
                            nullParameter: parameterInfo.Name,
                            nullIndex: parameterIndex,
                            executionSetup: new ErroredExecutionSetup(compositionEx)));
                }
            }

            return data;
        }
    }
}
