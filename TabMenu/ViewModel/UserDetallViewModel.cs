using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using BancDelTemps.Model;
using BancDelTemps.Model.Class;

namespace BancDelTemps.ViewModel
{
    public class UserDetallViewModel : INotifyPropertyChanged
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
        public ICommand ButtonSaveChanges { get; set; }
        public ICommand ButtonDiscardChanges { get; set; }
        public User User { get; set; }

        public UserDetallViewModel()
        {
        }

        public UserDetallViewModel(User user)
        {
            ButtonCloseApp = new UserDetallViewModel.RelayCommand(o => Application.Current.Windows[1].Close());
            ButtonSaveChanges = new UserDetallViewModel.RelayCommand(o => saveChanges());
            ButtonDiscardChanges = new UserDetallViewModel.RelayCommand(o => discardChanges());
            User = user;
            IdUser = user.Id_User;
            NameUser = user.name;
            LastNameUser = user.last_name;
            GenderUser = user.gender;
            EmailUser = user.email;
            BirthDateUser = DateTime.Parse(user.date_of_birth);
        }

        public void saveChanges()
        {
            User uNew = new User();
            uNew.Id_User = IdUser;
            uNew.name = NameUser;
            uNew.last_name = LastNameUser;
            if (SelectedIndexGenre == 0)
            {
                GenderUser = "Home";
            }
            else if (SelectedIndexGenre == 1)
            {

                GenderUser = "Dona";
            }
            else
            {
                GenderUser = "Other";
            }
            uNew.gender = GenderUser;
            uNew.email = EmailUser;
            uNew.date_of_birth = BirthDateUser.ToString("dd-MM-yyyy");
            uNew.register_date = User.register_date;
            uNew.picture_path = User.picture_path;
            uNew.time_hours = User.time_hours;
            UsersRepository.UpdateUser(uNew);
            Application.Current.Windows[1].Close();
        }

        public void discardChanges()
        {
            MessageBoxResult mbRes =  MessageBox.Show("Are you sure you want to discard changes?", "Warning", MessageBoxButton.YesNo);
            if (mbRes == MessageBoxResult.Yes)
            {
                Application.Current.Windows[1].Close();
            } 
        }


        private int _idUser;
        public int IdUser
        {
            get { return _idUser; }
            set
            {
                _idUser = value; NotifyPropertyChanged();
            }
        }

        private string _nameUser;
        public string NameUser
        {
            get { return _nameUser; }
            set
            {
                _nameUser = value; NotifyPropertyChanged();
            }
        }

        private string _lastNameUser;
        public string LastNameUser
        {
            get { return _lastNameUser; }
            set
            {
                _lastNameUser = value; NotifyPropertyChanged();
            }
        }

        private string _genderUser;
        public string GenderUser
        {
            get { return _genderUser; }
            set
            {
                _genderUser = value; NotifyPropertyChanged();
                if (_genderUser == "Home")
                {
                    SelectedIndexGenre = 0;
                } else if (_genderUser == "Dona")
                {
                    SelectedIndexGenre = 1;
                }
                else
                {
                    SelectedIndexGenre = 2;
                }
            }
        }

        private string _emailUser;
        public string EmailUser
        {
            get { return _emailUser; }
            set
            {
                _emailUser = value; NotifyPropertyChanged();
            }
        }

        private int _selectedIndexGenre;
        public int SelectedIndexGenre
        {
            get { return _selectedIndexGenre; }
            set
            {
                _selectedIndexGenre = value; NotifyPropertyChanged();
            }
        }

        private DateTime _birthDateUser;
        public DateTime BirthDateUser
        {
            get { return _birthDateUser; }
            set
            {
                _birthDateUser = value; NotifyPropertyChanged();
            }
        }

        #endregion

    }
}