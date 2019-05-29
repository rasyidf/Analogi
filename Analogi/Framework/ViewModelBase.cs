using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Analogi.Core
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
            CanExecuteChanged = null;
        }

        #endregion Constructors

        #region Events

        public event EventHandler CanExecuteChanged;

        #endregion Events

        #region Methods

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

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


}