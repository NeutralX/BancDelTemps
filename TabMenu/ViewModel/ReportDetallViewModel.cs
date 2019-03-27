using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using BancDelTemps.Model;
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

        #region Main
        public ICommand ButtonCloseApp { get; set; }
        public ICommand ButtonDiscardChanges { get; set; }
        public ICommand ButtonSaveChanges { get; set; }
        public ICommand ButtonBanAuthor { get; set; }
        public ICommand ButtonBanNoAuthor { get; set; }
        public Report Report { get; set; }
        private User _userAutor { get; set; }

        public User UserAutor {
            get => _userAutor;
            set { _userAutor = value; NotifyPropertyChanged(); }
        }
        private User _userNoAutor { get; set; }

        public User UserNoAutor {
            get => _userNoAutor;
            set { _userNoAutor = value; NotifyPropertyChanged(); }
        }

        private string _description { get; set; }

        public string Description {
            get => _description;
            set { _description = value; NotifyPropertyChanged(); }
        }
        private bool _revised { get; set; }

        public bool Revised {
            get => _revised;
            set { _revised = value; NotifyPropertyChanged(); }
        }


        List<ObjectInfo> llista = new List<ObjectInfo>();
        private List<ObjectInfo> _objects;
        public List<ObjectInfo> Objects {
            get { return _objects; }
            set { _objects = value; NotifyPropertyChanged(); }
        }

        List<ObjectInfo> llista2 = new List<ObjectInfo>();
        private List<ObjectInfo> _objects2;
        public List<ObjectInfo> Objects2 {
            get { return _objects2; }
            set { _objects2 = value; NotifyPropertyChanged(); }
        }

        public ReportDetallViewModel()
        {
        }

        public ReportDetallViewModel(Report report)
        {
            ButtonCloseApp = new RelayCommand(o => Application.Current.Windows[1].Close());
            ButtonDiscardChanges = new RelayCommand(o => discardChanges());
            ButtonSaveChanges = new RelayCommand(o => SaveChanges());
            ButtonBanAuthor = new RelayCommand(o=> BanAuthor());
            ButtonBanNoAuthor = new RelayCommand(o=> BanNoAuthor());
            Report = report;
            _userAutor = UsersRepository.GetUser(Report.Id_Reported);
            _userNoAutor = UsersRepository.GetUser(Report.Id_Reporter);
            FillDGV1();
            FillDGV2();
            Description = report.description;
            Revised = report.is_revised;


        }

        public void BanAuthor()
        {
            Ban ban = new Ban();
            ban.UserId_User = UserAutor.Id_User;
            ban.ban_date = DateTime.Now.ToString("dd-MM-yyyy");
            ban.ban_finish_date = DateTime.Now.AddDays(3).ToString("dd-MM-yyyy");
            BansRepository.InsertBan(ban);
        }
        public void BanNoAuthor()
        {
            Ban ban = new Ban();
            ban.UserId_User = UserNoAutor.Id_User;
            ban.ban_date = DateTime.Now.ToString("dd-MM-yyyy");
            ban.ban_finish_date = DateTime.Now.AddDays(3).ToString("dd-MM-yyyy");
            BansRepository.InsertBan(ban);
        }

        public void SaveChanges()
        {
            Report rNew = new Report();
            rNew.Id_Report = Report.Id_Report;
            rNew.Id_Reported = Report.Id_Reported;
            rNew.Id_Reporter = Report.Id_Reporter;
            rNew.Post_Id_Post = Report.Post_Id_Post;
            rNew.description = Description;
            rNew.is_revised = Revised;
            ReportsRepository.UpdateReport(rNew);
            Application.Current.Windows[1].Close();
        }

        public void discardChanges()
        {
            MessageBoxResult mbRes = MessageBox.Show("Are you sure you want to discard changes?", "Warning", MessageBoxButton.YesNo);
            if (mbRes == MessageBoxResult.Yes)
            {
                Application.Current.Windows[1].Close();
            }
        }


        public void FillDGV1()
        {
            PropertyInfo[] properties = _userAutor.GetType().GetProperties();
            foreach (var p in properties)
            {
                if (p.PropertyType.ToString().Contains("ICollection")) continue;
                ObjectInfo obi = new ObjectInfo(p.Name, p.GetValue(_userAutor).ToString());
                llista.Add(obi);


            }

            Objects = llista;
        }
        public void FillDGV2()
        {
            PropertyInfo[] properties = _userNoAutor.GetType().GetProperties();
            foreach (var p in properties)
            {
                if (p.PropertyType.ToString().Contains("ICollection")) continue;
                ObjectInfo obi = new ObjectInfo(p.Name, p.GetValue(_userNoAutor).ToString());
                llista2.Add(obi);


            }

            Objects2 = llista2;
        }

        #endregion
    }
}