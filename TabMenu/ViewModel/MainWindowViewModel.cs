using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using TabMenu;

namespace BancDelTemps.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
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
        #region Main

        public ICommand ButtonCloseApp { get; set; }

        private String culture;
        private Boolean firstTime;
        public MainWindowViewModel()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Settings.Default.Culture);
            ButtonCloseApp = new RelayCommand(o => System.Windows.Application.Current.Shutdown());
            AdminsPopulate();
            UsersPopulate();
            firstTime = true;
        }

        private ComboBoxItem _selectedValueCulture;
        public ComboBoxItem SelectedValueCulture
        {
            get { return _selectedValueCulture; }
            set
            {
                _selectedValueCulture = value; NotifyPropertyChanged();
                if (firstTime == true)
                {
                    firstTime = false;
                }
                else
                {
                    Language_Changer(_selectedValueCulture.Content.ToString());
                }
            }
        }

        public void Language_Changer(string culture)
        {

            switch (culture)
            {
                case "EN":
                    if(Settings.Default.Culture != "en") culture = "en";
                    break;
                case "ES":
                    if(Settings.Default.Culture != "es") culture = "es";
                    break;
                case "CA":
                    if(Settings.Default.Culture != "ca")culture = "ca";
                    break;
            }
            Settings.Default.Culture = culture;
            Settings.Default.Save();
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            App.Current.Shutdown();
        }
        #endregion

        #region Users

        private List<User> _users;
        public List<User> Users
        {
            get { return _users; }
            set { _users = value; NotifyPropertyChanged(); }
        }

        private void UsersPopulate()
        {

            Users = UsersRepository.GetAllUsers();

        }

        private User _selectedUser;
        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value; NotifyPropertyChanged();
            }
        }

        private void InsertUser()
        {
            if (_selectedUser != null)
            {
                User uNew = new User();
                User u2 = UsersRepository.InsertUser(uNew);
                UsersPopulate();
            }
        }

        private void UpdateUser()
        {
            if (_selectedUser != null)
            {
                User uNew = _selectedUser;
                //cNew.nom = NomContacte;
                //cNew.cognoms = CognomContacte;
                User u2 = UsersRepository.UpdateUser(uNew);
                UsersPopulate();
            }
        }

        private void DeleteUser()
        {
            if (_selectedUser != null)
            {
                UsersRepository.DeleteUser(_selectedUser.Id_User);
                UsersPopulate();
            }
        }

        #endregion
        #region Posts

        private List<Post> _posts;
        public List<Post> Posts
        {
            get { return _posts; }
            set { _posts = value; NotifyPropertyChanged(); }
        }

        private void PostsPopulate()
        {

            Posts = PostsRepository.GetAllPosts();

        }

        #endregion
        #region Admins

        private List<Admin> _admins;
        public List<Admin> Admins
        {
            get { return _admins; }
            set { _admins = value; NotifyPropertyChanged(); }
        }

        private void AdminsPopulate()
        {

            Admins = AdminsRepository.GetAllAdmins();

        }

        #endregion
        #region Reports

        private List<Report> _reports;
        public List<Report> Reports
        {
            get { return _reports; }
            set { _reports = value; NotifyPropertyChanged(); }
        }

        private void ReportsPopulate()
        {

            //Reports = ReportsRepository.GetAllPosts();

        }

        #endregion
        #region Bans

        private List<Ban> _bans;
        public List<Ban> Bans
        {
            get { return _bans; }
            set { _bans = value; NotifyPropertyChanged(); }
        }

        private void BansPopulate()
        {

            //Bans = BansRepository.GetAllPosts();

        }

        #endregion
    }
}

