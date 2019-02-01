using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BancDelTemps.Model;
using BancDelTemps.Model.Class;

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

        public MainWindowViewModel()
        {
            ButtonCloseApp = new RelayCommand(o => System.Windows.Application.Current.Shutdown());
            UsersPopulate();
        }
        #endregion

        #region Users

        private List<User> _users;
        public List<User> Users {
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
    }
}

