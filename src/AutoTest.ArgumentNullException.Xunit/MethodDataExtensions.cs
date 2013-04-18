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
        /// <returns>A task representing the asynchronous execution of the <paramref name="method"/>.</returns>
        public static Task Execute(this MethodData method)
        {
            if (method == null) throw new ArgumentNullException("method");

            return
                method.ExecuteAction()
                      .Then(() =>
                      {
                          Assert.Throws<ArgumentNullException>(() => { });
                      })
                      .Catch(catchInfo =>
                      {
                          string actualParamName = Assert.Throws<ArgumentNullException>(() => { throw catchInfo.Exception; }).ParamName;
                          Assert.Equal(method.NullParameter, actualParamName);
                          return catchInfo.Handled();
                      });
        }
    }
}
