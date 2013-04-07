//-----------------------------------------------------------------------
// <copyright file="ReflectionDiscoverableCollection.cs" company="The glimpse dudes">
//  Shamelessly lifted from Glimpse see: http://getglimpse.com/.
// </copyright>
//-----------------------------------------------------------------------

namespace AutoTest.ArgNullEx.Framework
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// An implementation of <see cref="IDiscoverableCollection{T}"/> which uses .NET reflection to find and load types.
    /// </summary>
    /// <typeparam name="T">The type to find and load.</typeparam>
    internal class ReflectionDiscoverableCollection<T> : IDiscoverableCollection<T>
    {
        /// <summary>
        /// The location for discovery.
        /// </summary>
        private string discoveryLocation;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectionDiscoverableCollection{T}" /> class.
        /// </summary>
        public ReflectionDiscoverableCollection()
        {
            Items = new List<T>();
            IgnoredTypes = new List<Type>();
            AutoDiscover = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not auto discover.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [auto discover]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoDiscover { get; set; }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
        public int Count
        {
            get
            {
                return Items.Count;
            }
        }

        /// <summary>
        /// Gets or sets the file path to the discovery location.
        /// </summary>
        /// <value>
        /// The discovery location.
        /// </value>
        /// <exception cref="System.IO.DirectoryNotFoundException">Throws an exception if the directory does not exist.</exception>
        public string DiscoveryLocation
        {
            get
            {
                return discoveryLocation ?? (discoveryLocation = BaseDirectory);
            }

            set
            {
                // If this isn't an absolute path then root it with the AppDomain's base directory
                var result = Path.IsPathRooted(value) ? value : Path.Combine(BaseDirectory, value);

                if (!Directory.Exists(result))
                {
                    throw new DirectoryNotFoundException(string.Format("The input value '{0}' creates path '{1}', which does not exist.", value, result));
                }

                discoveryLocation = result;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, false.</returns>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the items of the collection.
        /// </summary>
        internal List<T> Items { get; set; }

        /// <summary>
        /// Gets or sets the list of ignored types.
        /// </summary>
        internal List<Type> IgnoredTypes { get; set; }

        /// <summary>
        /// Gets the directory of the application, if the AppDomain is shadow copied, use the shadow directory
        /// </summary>
        private static string BaseDirectory
        {
            get
            {
                var setupInfo = AppDomain.CurrentDomain.SetupInformation;
                return string.Equals(setupInfo.ShadowCopyFiles, "true", StringComparison.OrdinalIgnoreCase)
                           ? Path.Combine(setupInfo.CachePath, setupInfo.ApplicationName)
                           : AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(T item)
        {
            Items.Add(item);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            Items.Clear();
        }

        /// <summary>
        /// Determines whether the collection contains the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if the collection contains the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(T item)
        {
            return Items.Contains(item);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> is the item was removed.</returns>
        public bool Remove(T item)
        {
            var result = Items.Remove(item);

            return result;
        }

        /// <summary>
        /// Ignores the type.
        /// </summary>
        /// <param name="type">The type.</param>
        public void IgnoreType(Type type)
        {
            IgnoredTypes.Add(type);
        }

        /// <summary>
        /// Discovers this all instanced of <typeparamref name="T"/> within the discovery location.
        /// </summary>
        public void Discover()
        {
            var results = new List<T>();

            foreach (var file in Directory.GetFiles(DiscoveryLocation, "*.dll", SearchOption.AllDirectories))
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(file);
                    Type[] allTypes;

                    // GetTypes potentially throws and exception. Defensive coding as per http://haacked.com/archive/2012/07/23/get-all-types-in-an-assembly.aspx
                    try
                    {
                        allTypes = assembly.GetTypes();
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        allTypes = ex.Types.Where(t => t != null).ToArray();
                    }

                    var concreteTypes = allTypes.Where(type => typeof(T).IsAssignableFrom(type) &&
                                                                          !type.IsInterface &&
                                                                          !type.IsAbstract &&
                                                                          !IgnoredTypes.Contains(type));
                    foreach (var type in concreteTypes)
                    {
                        try
                        {
                            var instance = (T)Activator.CreateInstance(type);
                            results.Add(instance);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                catch (Exception)
                {
                }
            }

            if (results.Count > 0)
            {
                Items.Clear();

                Items.AddRange(results);
            }
        }
    }
}
