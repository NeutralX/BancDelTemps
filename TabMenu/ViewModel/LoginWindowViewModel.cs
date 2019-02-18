using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BancDelTemps.Model;
using BancDelTemps.Model.Class;
using BancDelTemps.Properties;
using BancDelTemps.View;
using TabMenu;

namespace BancDelTemps.ViewModel
{
    class LoginWindowViewModel : INotifyPropertyChanged
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

        public ICommand ButtonLogin { get; set; }
        public LoginWindowViewModel()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Settings.Default.Culture);
            ButtonLogin = new RelayCommand(o => LoginToApp(o));

        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set { _username = value; NotifyPropertyChanged(); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; NotifyPropertyChanged(); }
        }

        private string _errorText;
        public string ErrorText
        {
            get { return _errorText; }
            set { _errorText = value; NotifyPropertyChanged(); }
        }

        public void LoginToApp(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            Password = passwordBox.Password;
            if (_username != "" && _password != "")
            {
                Admin login = new Admin(_username,_password);
                Admin returnAdmin = AdminsRepository.LoginAdmin(login);
                if (returnAdmin != null)
                {
                    MainWindow mw = new MainWindow();
                    mw.Show();
                    var w = Application.Current.Windows[0];
                    w.Close();
                }
                else
                {

                }
            }
        }
    }
}
