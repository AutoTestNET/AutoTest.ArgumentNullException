namespace AutoTest.ArgNullEx
{
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines a reflected asynchronous <see cref="MethodBase"/> execution.
    /// </summary>
    public interface IExecution
    {
        /// <summary>
        /// Executes a reflected <see cref="MethodBase"/>.
        /// </summary>
        /// <returns>A task representing the asynchronous execution of a <see cref="MethodBase"/>.</returns>
        Task Execute();
    }
}
