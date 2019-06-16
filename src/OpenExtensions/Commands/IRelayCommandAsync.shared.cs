﻿using System.Threading.Tasks;
using System.Windows.Input;

namespace OpenExtensions.Commands
{
    /// <summary>
    /// An <see cref="ICommand"/> to use with async
    /// </summary>
    public interface IRelayCommandAsync : ICommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task ExecuteAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool CanExecute();
    }
}
