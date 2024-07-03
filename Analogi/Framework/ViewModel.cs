using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Analogi.Framework
{

    internal class ViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private readonly Dictionary<string, IList<string>> _validationErrors = [];

        public string this[string propertyName] => string.IsNullOrEmpty(propertyName)
                    ? Error
                    : _validationErrors.TryGetValue(propertyName, out IList<string> value) ? string.Join(Environment.NewLine, value) : string.Empty;

        public string Error => string.Join(Environment.NewLine, GetAllErrors());

        private IEnumerable<string> GetAllErrors()
        {
            return _validationErrors.SelectMany(kvp => kvp.Value).Where(e => !string.IsNullOrEmpty(e));
        }

        public void AddValidationError(string propertyName, string errorMessage)
        {
            if (!_validationErrors.TryGetValue(propertyName, out IList<string> value))
            {
                _validationErrors.Add(propertyName, value);
            }

            value.Add(errorMessage);
        }

        public void ClearValidationErrors(string propertyName)
        {
            if (!_validationErrors.ContainsKey(propertyName))
            {
                return;
            }

            _ = _validationErrors.Remove(propertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
