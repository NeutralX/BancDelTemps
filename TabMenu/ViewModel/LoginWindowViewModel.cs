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
using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
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
            ProgressVisibility = "HIDDEN";
            Username = Settings.Default.Username;
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

        private bool _IsDialogOpen;
        public bool IsDialogOpen
        {
            get { return _IsDialogOpen; }
            set { _IsDialogOpen = value; NotifyPropertyChanged(); }
        }

        private string _progressVisibility;
        public string ProgressVisibility
        {
            get { return _progressVisibility; }
            set { _progressVisibility = value; NotifyPropertyChanged(); }
        }

        private bool _isCheckedRemember;
        public bool IsCheckedRemember
        {
            get { return _isCheckedRemember; }
            set
            {
                _isCheckedRemember = value; NotifyPropertyChanged();
                
            }
        }

        public async void LoginToApp(object parameter)
        {
            ProgressVisibility = "VISIBLE";
            var passwordBox = parameter as PasswordBox;
            Password = passwordBox.Password;
            if (_username != "" && _password != "")
            {
                Admin login = new Admin(_username,_password);
                if (_isCheckedRemember == true)
                {
                    Settings.Default.Username = _username;
                    Settings.Default.Save();
                }
                else
                {
                    Settings.Default.Username = "";
                    Settings.Default.Save();
                }
                    Admin returnAdmin = await Task.Run(() => loginAdmin(login));
                
                if (returnAdmin != null)
                {
                    MainWindow mw = new MainWindow();
                    mw.Show();
                    var w = Application.Current.Windows[0];
                    w.Close();
                }
                else
                {
                    IsDialogOpen = true;
                }
                ProgressVisibility = "HIDDEN";
            }
        }

        public async Task<Admin> loginAdmin(Admin login)
        {
             Admin returnAdmin = AdminsRepository.LoginAdmin(login);
             return returnAdmin;
        }
    }
}
