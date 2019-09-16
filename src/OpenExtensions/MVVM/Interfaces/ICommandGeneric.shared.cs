﻿using System.Windows.Input;

namespace OpenExtensions.MVVM.Interfaces
{
    /// <summary>
    /// A Generic <see cref="ICommand"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICommandGeneric<T> : ICommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        void Execute(T parameter);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        bool CanExecute(T parameter);
    }
}
