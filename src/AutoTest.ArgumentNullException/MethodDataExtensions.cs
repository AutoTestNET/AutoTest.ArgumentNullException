namespace AutoTest.ArgNullEx
{
    using System;
    using System.Threading.Tasks;
    using Xunit;

    /// <summary>
    /// Extension methods on a <see cref="MethodData"/>.
    /// </summary>
    public static class MethodDataExtensions
    {
        /// <summary>
        /// Executes the <paramref name="method"/>,
        /// </summary>
        /// <param name="method">The method data.</param>
        /// <returns>A task representing the asynchronous execution of the <paramref name="method"/>.</returns>
        public static async Task Execute(this MethodData method)
        {
            if (method == null) throw new ArgumentNullException("method");

            if (method.ExecutingActionSync != null)
            {
                string actualParamName = Assert.Throws<ArgumentNullException>(() => method.ExecutingActionSync()).ParamName;
                Assert.Equal(method.NullArgument, actualParamName);
                return;
            }

            if (method.ExecutingActionAsync != null)
            {
                Exception ex = null;
                try
                {
                    await method.ExecutingActionAsync();
                }
                catch (Exception innerEx)
                {
                    ex = innerEx;
                }
                string actualParamName = Assert.Throws<ArgumentNullException>(() => { if (ex != null) throw ex; }).ParamName;
                Assert.Equal(method.NullArgument, actualParamName);
                return;
            }

            throw new ArgumentException("The method data does not have an executing action.", "method");
        }
    }
}
