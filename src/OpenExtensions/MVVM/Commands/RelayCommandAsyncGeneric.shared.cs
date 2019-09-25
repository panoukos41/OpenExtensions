using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OpenExtensions.MVVM.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class RelayCommandAsync<T> : ICommandAsyncGeneric<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler CanExecuteChanged;

        private bool _isExecuting;
        private readonly Func<T, Task> _execute;
        private readonly Func<T, bool> _canExecute;

        /// <summary>
        /// Initialize a new instance of <see cref="RelayCommandAsync{T}"/>
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommandAsync(Func<T, Task> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Initialize a new instance of <see cref="RelayCommand{T}"/>
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommandAsync(Func<T, bool> canExecute, Func<T, Task> execute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Indicates if the task can execute.
        /// </summary>
        /// <returns></returns>
        public bool CanExecute(T parameter)
        {
            return !_isExecuting && (_canExecute?.Invoke(parameter) ?? true);
        }

        /// <summary>
        /// Executes the task if <see cref="CanExecute(T)"/> returns true.
        /// </summary>
        /// <returns></returns>
        public async Task ExecuteAsync(T parameter)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    _isExecuting = true;
                    await _execute(parameter);
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

        async void ICommand.Execute(object parameter)
        {
            await ExecuteAsync((T)parameter);
        }

        /// <summary>
        /// Returns the storage but if its null it initializes a new instance 
        /// of the command, stores it in storage and returns it.
        /// </summary>
        /// <param name="storage">The backing storage of the command</param>
        /// <param name="execute">The execute method</param>
        /// <param name="canExecute">The can execute method</param>
        /// <returns></returns>
        public static RelayCommandAsync<T> New(ref RelayCommandAsync<T> storage, Func<T, Task> execute, Func<T, bool> canExecute = null)
        {
            return storage ?? (storage = new RelayCommandAsync<T>(execute, canExecute));
        }

        /// <summary>
        /// Returns the storage but if its null it initializes a new instance 
        /// of the command, stores it in storage and returns it.
        /// </summary>
        /// <param name="storage">The backing storage of the command</param>
        /// <param name="canExecute">The can execute method</param>
        /// <param name="execute">The execute method</param>
        /// <returns></returns>
        public static RelayCommandAsync<T> New(ref RelayCommandAsync<T> storage, Func<T, bool> canExecute, Func<T, Task> execute)
        {
            return storage ?? (storage = new RelayCommandAsync<T>(execute, canExecute));
        }
    }
}
