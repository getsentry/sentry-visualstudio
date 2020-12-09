using System;
using System.Windows.Input;

namespace VSSentry.Shared
{
    public class DelegateCommand : ICommand
    {
        private Action<object> _executeMethod;
        public DelegateCommand(Action<object> executeMethod)
        {
            _executeMethod = executeMethod;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged;
        public void Execute(object parameter)
        {
            _executeMethod.Invoke(parameter);
        }
    }
}
