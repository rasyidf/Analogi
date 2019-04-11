using System;
using System.ComponentModel;
using System.Windows.Input;

namespace rasyidf.Analogi.Core
{
    public class DelegateCommand : ICommand
    {
        #region Fields

        private readonly Action action;

        #endregion Fields

        #region Constructors

        public DelegateCommand(Action act)
        {
            action = act;
        }

        #endregion Constructors

        #region Events

        public event EventHandler CanExecuteChanged;

        #endregion Events

        #region Methods

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            action();
        }

        #endregion Methods
    }

    internal class ViewModelBase : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Methods

        public void NotifyProps(string args)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(args));
        }

        #endregion Methods
    }
}