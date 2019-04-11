using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Analogi
{
    internal class ViewModelBase  : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyProps(string args) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(args));
    }

    public class DelegateCommand : ICommand
    {

        private readonly Action action;

        public DelegateCommand(Action act) => action = act;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) {
           
            return true;
        }

        public void Execute(object parameter) => action();
    }

}