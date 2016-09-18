namespace AutoTest.ArgNullEx.Xunit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using global::Xunit;
    using global::Xunit.Sdk;

    /// <summary>
    /// Extension methods on a <see cref="MethodData"/>.
    /// </summary>
    public static class MethodDataExtensions
    {
        /// <summary>
        /// Executes the <paramref name="method"/> and checks whether it correctly throws a
        /// <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="method">The method data.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous execution of the
        /// <paramref name="method"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="method"/> parameter is
        /// <see langword="null"/>.</exception>
        public static async Task Execute(this MethodData method)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            bool catchExecuted = false;

            try
            {
                await method.ExecuteAction();
            }
            catch (Exception ex)
            {
                catchExecuted = true;
                string actualParamName = Assert.Throws<ArgumentNullException>(() => { throw ex; }).ParamName;
                Assert.Equal(method.NullParameter, actualParamName);
            }

            // Don't throw if the catch was executed.
            if (!catchExecuted)
                throw new ThrowsException(typeof(ArgumentNullException));
        }
    }
}
