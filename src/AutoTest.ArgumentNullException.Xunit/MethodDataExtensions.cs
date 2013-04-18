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
                      .Then((Action)NoException)
                      .Catch(catchInfo => catchInfo.CheckException(method.NullParameter));
        }

        /// <summary>
        /// Continuation for when the method completes successfully to asserts and throw because a <see cref="ArgumentNullException"/> was not thrown.
        /// </summary>
        private static void NoException()
        {
            Assert.Throws<ArgumentNullException>(() => { });
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

            string actualParamName = Assert.Throws<ArgumentNullException>(() => { throw catchInfo.Exception; }).ParamName;
            Assert.Equal(nullParameter, actualParamName);
            return catchInfo.Handled();
        }
    }
}
