using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace geocentric
{
    public class CommandHandler : ICommand
    {
        private Action _action;
        private bool _canExecute;
        public CommandHandler(Action action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            bool rememeberCanExceute = _canExecute;
            bool? castedParameter = parameter as bool?;
            if (castedParameter != null)
            {
                if (castedParameter == true)
                    _canExecute = true;
                if (castedParameter == false)
                    _canExecute = false;
            }
            if (rememeberCanExceute != _canExecute)
                CanExecuteChanged(this, EventArgs.Empty);
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action();
        }
    }
}
