using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OpenExtensions.MVVM.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class RelayCommandAsync : ICommandAsync
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler CanExecuteChanged;

        private bool _isExecuting;
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;

        /// <summary>
        /// Initialize a new instance of <see cref="RelayCommandAsync"/>
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommandAsync(Func<Task> execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Initialize a new instance of <see cref="RelayCommandAsync"/>
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommandAsync(Func<bool> canExecute, Func<Task> execute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Indicates if the task can execute.
        /// </summary>
        /// <returns></returns>
        public bool CanExecute()
        {
            return !_isExecuting && (_canExecute?.Invoke() ?? true);
        }

        /// <summary>
        /// Executes the task if <see cref="CanExecute"/> returns true.
        /// </summary>
        /// <returns></returns>
        public async Task ExecuteAsync()
        {
            if (CanExecute())
            {
                try
                {
                    _isExecuting = true;
                    await _execute();
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
            return CanExecute();
        }

        async void ICommand.Execute(object parameter)
        {
            await ExecuteAsync();
        }

        /// <summary>
        /// Returns the storage but if its null it initializes a new instance 
        /// of the command, stores it in storage and returns it.
        /// </summary>
        /// <param name="storage">The backing storage of the command</param>
        /// <param name="execute">The execute method</param>
        /// <param name="canExecute">The can execute method</param>
        /// <returns></returns>
        public static RelayCommandAsync New(ref RelayCommandAsync storage, Func<Task> execute, Func<bool> canExecute = null)
        {
            return storage ?? (storage = new RelayCommandAsync(execute, canExecute));
        }

        /// <summary>
        /// Returns the storage but if its null it initializes a new instance 
        /// of the command, stores it in storage and returns it.
        /// </summary>
        /// <param name="storage">The backing storage of the command</param>
        /// <param name="canExecute">The can execute method</param>
        /// <param name="execute">The execute method</param>
        /// <returns></returns>
        public static RelayCommandAsync New(ref RelayCommandAsync storage, Func<bool> canExecute, Func<Task> execute)
        {
            return storage ?? (storage = new RelayCommandAsync(execute, canExecute));
        }
    }
}
