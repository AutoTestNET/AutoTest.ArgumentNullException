namespace AutoTest.ArgNullEx.Xunit
{
    using System;

    /// <summary>
    /// The type of exclusion for the <see cref="ExcludeAllAttribute"/> attribute.
    /// </summary>
    [Flags]
    public enum ExclusionType
    {
        /// <summary>
        /// Nothing is excluded.
        /// </summary>
        None = 0,

        /// <summary>
        /// All types are to be excluded.
        /// </summary>
        Types = 0x1,

        /// <summary>
        /// All methods are to be excluded.
        /// </summary>
        Methods = 0x2,

        /// <summary>
        /// All parameters are to be excluded.
        /// </summary>
        Parameters = 0x4,

        /// <summary>
        /// All types, methods and parameters are to be excluded.
        /// </summary>
        All = Types | Methods | Parameters,
    }
}
