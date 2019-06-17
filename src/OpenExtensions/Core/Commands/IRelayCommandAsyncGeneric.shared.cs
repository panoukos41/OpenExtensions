﻿using System.Threading.Tasks;
using System.Windows.Input;

namespace OpenExtensions.Core.Commands
{
    /// <summary>
    /// A Generic <see cref="ICommand"/> to use with async
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRelayCommandAsyncGeneric<T> : ICommand
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
