using System;
using System.Windows.Input;

namespace OpenExtensions.MVVM.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler CanExecuteChanged;

        private bool _isExecuting;
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        /// <summary>
        /// Initialize a new instance of <see cref="RelayCommand"/>
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Initialize a new instance of <see cref="RelayCommand"/>
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommand(Func<bool> canExecute, Action execute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter">Parameter is always ignored</param>
        /// <returns></returns>
        public bool CanExecute(object parameter = null)
        {
            return !_isExecuting && (_canExecute?.Invoke() ?? true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter">Parameter is always ignored</param>
        public void Execute(object parameter = null)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    _isExecuting = true;
                    _execute?.Invoke();
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

        /// <summary>
        /// Returns the storage but if its null it initializes a new instance 
        /// of the command, stores it in storage and returns it.
        /// </summary>
        /// <param name="storage">The backing storage of the command</param>
        /// <param name="execute">The execute method</param>
        /// <param name="canExecute">The can execute method</param>
        /// <returns></returns>
        public static RelayCommand New(ref RelayCommand storage, Action execute, Func<bool> canExecute = null)
        {
            return storage ?? (storage = new RelayCommand(execute, canExecute));
        }

        /// <summary>
        /// Returns the storage but if its null it initializes a new instance 
        /// of the command, stores it in storage and returns it.
        /// </summary>
        /// <param name="storage">The backing storage of the command</param>
        /// <param name="canExecute">The can execute method</param>
        /// <param name="execute">The execute method</param>
        /// <returns></returns>
        public static RelayCommand New(ref RelayCommand storage, Func<bool> canExecute, Action execute)
        {
            return storage ?? (storage = new RelayCommand(execute, canExecute));
        }
    }
}
