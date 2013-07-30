namespace AutoTest.ArgNullEx.Xunit
{
    using System;
    using System.Threading.Tasks;
    using global::Xunit;
    using global::Xunit.Sdk;

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

            bool catchExecuted = false;

            return
                method.ExecuteAction()
                      .Catch(catchInfo =>
                          {
                              catchExecuted = true;
                              return catchInfo.CheckException(method.NullParameter);
                          })
                      .Then(() => ThenNoException(catchExecuted));
        }

        /// <summary>
        /// Throws a <see cref="ThrowsException"/> if <paramref name="catchExecuted"/> is <c>false</c>.
        /// </summary>
        /// <param name="catchExecuted"><c>true</c> of the catch was executed; otherwise <c>false</c>.</param>
        private static void ThenNoException(bool catchExecuted)
        {
            // Don't throw if the catch was executed.
            if (!catchExecuted)
                throw new ThrowsException(typeof(ArgumentNullException));
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

            string actualParamName =
                Assert.Throws<ArgumentNullException>(() => { throw catchInfo.Exception; }).ParamName;
            Assert.Equal(nullParameter, actualParamName);
            return catchInfo.Handled();
        }
    }
}
