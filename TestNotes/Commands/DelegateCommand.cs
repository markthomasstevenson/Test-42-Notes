using System;
using System.Windows.Input;

namespace TestNotes.Commands
{
    public class DelegateCommand : ICommand
    {
        private readonly Action _whatToExecute;
        private readonly Func<bool> _whenToExecute;
        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action what, Func<bool> when)
        {
            _whatToExecute = what;
            _whenToExecute = when;
        }

        public bool CanExecute(object parameter)
        {
            return _whenToExecute();
        }

        public void Execute(object parameter)
        {
            _whatToExecute();
        }
    }
}
