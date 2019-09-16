using System.Threading.Tasks;
using System.Windows.Input;

namespace OpenExtensions.Core.Interfaces
{
    /// <summary>
    /// A Generic <see cref="ICommand"/> to use with async
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICommandAsyncGeneric<T> : ICommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        Task ExecuteAsync(T parameter);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        bool CanExecute(T parameter);
    }
}
