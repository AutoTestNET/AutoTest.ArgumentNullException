namespace AutoTest.ArgNullEx.Xunit
{
    using System;
    using System.Threading.Tasks;
    using global::Xunit;

    /// <summary>
    /// Extension methods on a <see cref="MethodData"/>.
    /// </summary>
    public static class MethodDataExtensions
    {
        /// <summary>
        /// Executes the <paramref name="method"/> and checks whether it correctly throws a <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="method">The method data.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous execution of the <paramref name="method"/>.</returns>
        public static Task Execute(this MethodData method)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            return
                method.ExecuteAction()
                      .Then(() => IsArgumentNullException())
                      .Catch(catchInfo => catchInfo.CheckException(method.NullParameter));
        }

        /// <summary>
        /// Checks the <paramref name="exception"/> is a <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="exception">The exception to check.</param>
        /// <returns>The name of the null parameter.</returns>
        private static string IsArgumentNullException(Exception exception = null)
        {
            return Assert.Throws<ArgumentNullException>(() => { if (exception != null) throw exception; }).ParamName;
        }

        /// <summary>
        /// Catch continuation that ensures a <see cref="ArgumentNullException"/> is thrown for the <paramref name="nullParameter"/>.
        /// </summary>
        /// <param name="catchInfo">The catch information.</param>
        /// <param name="nullParameter">The name of the null parameter.</param>
        /// <returns>The catch result.</returns>
        private static CatchInfoBase<Task>.CatchResult CheckException(this CatchInfo catchInfo, string nullParameter)
        {
            if (catchInfo == null)
                throw new ArgumentNullException("catchInfo");
            if (string.IsNullOrWhiteSpace(nullParameter))
                throw new ArgumentNullException("nullParameter");

            string actualParamName = IsArgumentNullException(catchInfo.Exception);
            Assert.Equal(nullParameter, actualParamName);
            return catchInfo.Handled();
        }
    }
}
