using System;
using System.Windows.Input;

namespace OpenExtensions.MVVM.Commands
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RelayCommand<T> : ICommandGeneric<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler CanExecuteChanged;

        private bool _isExecuting;
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        /// <summary>
        /// Initialize a new instance of <see cref="RelayCommand{T}"/>
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Initialize a new instance of <see cref="RelayCommand{T}"/>
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommand(Func<T, bool> canExecute, Action<T> execute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(T parameter)
        {
            return !_isExecuting && (_canExecute?.Invoke(parameter) ?? true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(T parameter)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    _isExecuting = true;
                    _execute?.Invoke(parameter);
                }
                finally
                {
                    _isExecuting = false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute((T)parameter);
        }

        void ICommand.Execute(object parameter)
        {
            Execute((T)parameter);
        }

        /// <summary>
        /// Returns the storage but if its null it initializes a new instance 
        /// of the command, stores it in storage and returns it.
        /// </summary>
        /// <param name="storage">The backing storage of the command</param>
        /// <param name="execute">The execute method</param>
        /// <param name="canExecute">The can execute method</param>
        /// <returns></returns>
        public static RelayCommand<T> New(ref RelayCommand<T> storage, Action<T> execute, Func<T, bool> canExecute = null)
        {
            return storage ?? (storage = new RelayCommand<T>(execute, canExecute));
        }

        /// <summary>
        /// Returns the storage but if its null it initializes a new instance 
        /// of the command, stores it in storage and returns it.
        /// </summary>
        /// <param name="storage">The backing storage of the command</param>
        /// <param name="canExecute">The can execute method</param>
        /// <param name="execute">The execute method</param>
        /// <returns></returns>
        public static RelayCommand<T> New(ref RelayCommand<T> storage, Func<T, bool> canExecute, Action<T> execute)
        {
            return storage ?? (storage = new RelayCommand<T>(execute, canExecute));
        }
    }
}
