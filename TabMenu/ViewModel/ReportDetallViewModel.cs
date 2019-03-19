using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using BancDelTemps.Model.Class;

namespace BancDelTemps.ViewModel
{
    public class ReportDetallViewModel : INotifyPropertyChanged
    {
        #region NotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
        #region RelayCommand
        class RelayCommand : ICommand
        {
            private Action<object> _action;

            public RelayCommand(Action<object> action)
            {
                _action = action;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                if (parameter != null)
                {
                    _action(parameter);
                }
                else
                {
                    _action("Hello world");
                }
            }

            public event EventHandler CanExecuteChanged;
        }
        #endregion

        public Report Report { get; set; }

        public ReportDetallViewModel()
        {
        }

        public ReportDetallViewModel(Report report)
        {
            Report = report;
        }
    }
}