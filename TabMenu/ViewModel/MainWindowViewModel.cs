using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BancDelTemps.Model;
using BancDelTemps.Model.Class;
using BancDelTemps.Properties;
using BancDelTemps.View;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
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
        //GENERAL
        public ICommand ButtonCloseApp { get; set; }
        //USER
        public ICommand ButtonFiltreUserEmail { get; set; }
        public ICommand ButtonFiltreUserNomCognom { get; set; }
        public ICommand ButtonFiltreUserReiniciar { get; set; }
        //POST
        public ICommand ButtonFiltrePostTitol { get; set; }
        public ICommand ButtonFiltrePostCreador { get; set; }
        public ICommand ButtonFiltrePostCategoria { get; set; }
        public ICommand ButtonFiltrePostDataInici { get; set; }
        public ICommand ButtonFiltrePostDataFinal { get; set; }
        public ICommand ButtonFiltrePostReiniciar { get; set; }
        //REPORT
        public ICommand ButtonFiltreReportDescripcio { get; set; }
        public ICommand ButtonFiltreReportReiniciar { get; set; }
        //PACT
        public ICommand ButtonFiltrePactTitol { get; set; }
        public ICommand ButtonFiltrePactDescripcio { get; set; }
        public ICommand ButtonFiltrePactDataInici { get; set; }
        public ICommand ButtonFiltrePactDataFinal { get; set; }
        public ICommand ButtonFiltrePactReiniciar { get; set; }
        public ICommand DobleClickUsers { get; set; }
        public ICommand DobleClickPosts { get; set; }



        private Boolean _firstTime;
        private double _lastLecture;
        private double _trend;

        public MainWindowViewModel()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Settings.Default.Culture);

            //GENERAL
            ButtonCloseApp = new RelayCommand(o => System.Windows.Application.Current.Shutdown());
            //USER
            ButtonFiltreUserEmail = new RelayCommand(o => Users = UsersRepository.GetUsersByEmail(_emailUser));
            ButtonFiltreUserNomCognom = new RelayCommand(o => Users = UsersRepository.GetUsersByName(_nomCognomUser));
            ButtonFiltreUserReiniciar = new RelayCommand(o => UsersPopulate());
            DobleClickUsers = new RelayCommand(o => test());
            //POST
            DataIniciPost = DateTime.Today;
            DataFinalPost = DateTime.Today;
            ButtonFiltrePostTitol = new RelayCommand(o => Posts = PostsRepository.GetPostsByTitle(_titolPost));
            ButtonFiltrePostCreador = new RelayCommand(o => Posts = PostsRepository.GetPostsByUser(_creadorPost));
            ButtonFiltrePostCategoria = new RelayCommand(o => Posts = PostsRepository.GetPostsByCategory(_categoriaPost));
            ButtonFiltrePostDataInici = new RelayCommand(o => Posts = PostsRepository.GetPostsByDateCreated(_dataIniciPost));
            ButtonFiltrePostDataFinal = new RelayCommand(o => Posts = PostsRepository.GetPostsByDateFinished(_dataFinalPost));
            ButtonFiltrePostReiniciar = new RelayCommand(o => PostsPopulate());
            DobleClickPosts = new RelayCommand(o=> obrirDetallPosts());
            //REPORT
            ButtonFiltreReportDescripcio = new RelayCommand(o => Reports = ReportsRepository.GetReportsByDescription(_descripcioReport));
            ButtonFiltreReportReiniciar = new RelayCommand(o => EstatReport = false);
            //PACT
            DataIniciPact = DateTime.Today;
            DataFinalPact = DateTime.Today;
            ButtonFiltrePactTitol = new RelayCommand(o => Pacts = PactsRepository.GetPactsByTitle(_titolPact));
            ButtonFiltrePactDescripcio = new RelayCommand(o => Pacts = PactsRepository.GetPactsByDescription(_descripcioPact));
            ButtonFiltrePactDataInici = new RelayCommand(o => Pacts = PactsRepository.GetPactsByDateCreated(_dataIniciPact));
            ButtonFiltrePactDataFinal = new RelayCommand(o => Pacts = PactsRepository.GetPactsByDateFinished(_dataFinalPact));
            ButtonFiltrePactReiniciar = new RelayCommand(o => PactsPopulate());

            UsersPopulate();
            PostsPopulate();
            ReportsPopulate();
            PactsPopulate();

            _firstTime = true;
            LastHourSeries = new SeriesCollection
                {
                    new LineSeries
                    {
                        AreaLimit = -10,
                        Values = new ChartValues<ObservableValue>
                        {
                            new ObservableValue(3),
                            new ObservableValue(5),
                            new ObservableValue(6),
                            new ObservableValue(7),
                            new ObservableValue(3),
                            new ObservableValue(4),
                            new ObservableValue(2),
                            new ObservableValue(5),
                            new ObservableValue(8),
                            new ObservableValue(3),
                            new ObservableValue(5),
                            new ObservableValue(6),
                            new ObservableValue(7),
                            new ObservableValue(3),
                            new ObservableValue(4),
                            new ObservableValue(2),
                            new ObservableValue(5),
                            new ObservableValue(8)
                        }
                    }
                };
            _trend = 8;

#if NET40
                Task.Factory.StartNew(() =>
                {
                    var r = new Random();
     
                    Action action = delegate
                    {
                        LastHourSeries[0].Values.Add(new ObservableValue(_trend));
                        LastHourSeries[0].Values.RemoveAt(0);
                        SetLecture();
                    };
     
                    while (true)
                    {
                        Thread.Sleep(500);
                        _trend += (r.NextDouble() > 0.3 ? 1 : -1) * r.Next(0, 5);
                        Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, action);
                    }
                });
#endif

            Task.Run(() =>
            {
                var r = new Random();
                while (true)
                {
                    Thread.Sleep(500);
                    _trend += (r.NextDouble() > 0.3 ? 1 : -1) * r.Next(0, 5);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        LastHourSeries[0].Values.Add(new ObservableValue(_trend));
                        LastHourSeries[0].Values.RemoveAt(0);
                        SetLecture();
                    });
                }
            });


            //DataContext = this;
        }

        //private ComboBoxItem _selectedValueCulture;
        //public ComboBoxItem SelectedValueCulture {
        //    get { return _selectedValueCulture; }
        //    set {
        //        _selectedValueCulture = value; NotifyPropertyChanged();
        //        if (_firstTime == true)
        //        {
        //            _firstTime = false;
        //        }
        //        else
        //        {
        //            Language_Changer(_selectedValueCulture.Content.ToString());
        //        }
        //    }
        //}

        //public void Language_Changer(string culture)
        //{

        //    switch (culture)
        //    {
        //        case "EN":
        //            if (Settings.Default.Culture != "en") culture = "en";
        //            break;
        //        case "ES":
        //            if (Settings.Default.Culture != "es") culture = "es";
        //            break;
        //        case "CA":
        //            if (Settings.Default.Culture != "ca") culture = "ca";
        //            break;
        //    }
        //    Settings.Default.Culture = culture;
        //    Settings.Default.Save();
        //    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
        //    App.Current.Shutdown();
        //}
        #endregion

        #region Users

        public void test()
        {
            UserDetall userDetall = new UserDetall(User);
            userDetall.ShowDialog();
        }

        private List<User> _users;
        public List<User> Users {
            get { return _users; }
            set { _users = value; NotifyPropertyChanged(); }
        }

        private User _user;
        public User User {
            get => _user;
            set { _user = value; NotifyPropertyChanged(); }
        }

        private void UsersPopulate()
        {
            EmailUser = "";
            NomCognomUser = "";
            Users = UsersRepository.GetAllUsers();

        }

        private User _selectedUser;
        public User SelectedUser {
            get { return _selectedUser; }
            set {
                _selectedUser = value; NotifyPropertyChanged();
            }
        }

        private string _emailUser;
        public string EmailUser {
            get { return _emailUser; }
            set {
                _emailUser = value; NotifyPropertyChanged();
            }
        }

        private string _nomCognomUser;
        public string NomCognomUser {
            get { return _nomCognomUser; }
            set {
                _nomCognomUser = value; NotifyPropertyChanged();
            }
        }

        private string _filtreEmail;
        public string FiltreEmail {
            get { return _filtreEmail; }
            set {
                _filtreEmail = value; NotifyPropertyChanged();
                Users = UsersRepository.GetUsersByEmail(_emailUser);
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

        public void obrirDetallPosts()
        {
            PostDetall postDetall = new PostDetall(Post);
            postDetall.ShowDialog();
        }

        private Post _post;

        public Post Post {
            get => _post;
            set { _post = value; NotifyPropertyChanged(); }
        }

        private List<Post> _posts;
        public List<Post> Posts {
            get { return _posts; }
            set { _posts = value; NotifyPropertyChanged(); }
        }

        private void PostsPopulate()
        {
            TitolPost = "";
            CreadorPost = "";
            CategoriaPost = 0;
            CreadorPost = "";
            DataIniciPost = DateTime.Today;
            DataFinalPost = DateTime.Today;
            Posts = PostsRepository.GetAllPosts();

        }

        private string _titolPost;
        public string TitolPost {
            get { return _titolPost; }
            set {
                _titolPost = value; NotifyPropertyChanged();
            }
        }

        private string _creadorPost;
        public string CreadorPost {
            get { return _creadorPost; }
            set {
                _creadorPost = value; NotifyPropertyChanged();
            }
        }

        private int _categoriaPost;
        public int CategoriaPost {
            get { return _categoriaPost; }
            set {
                _categoriaPost = value; NotifyPropertyChanged();
            }
        }

        private DateTime _dataIniciPost;
        public DateTime DataIniciPost {
            get { return _dataIniciPost; }
            set {
                _dataIniciPost = value; NotifyPropertyChanged();
            }
        }

        private DateTime _dataFinalPost;
        public DateTime DataFinalPost {
            get { return _dataFinalPost; }
            set {
                _dataFinalPost = value; NotifyPropertyChanged();
            }
        }

        #endregion
        #region Admins

        private List<Admin> _admins;
        public List<Admin> Admins {
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
        public List<Report> Reports {
            get { return _reports; }
            set { _reports = value; NotifyPropertyChanged(); }
        }

        private void ReportsPopulate()
        {
            //Reports = ReportsRepository.GetAllReports();
            Reports = ReportsRepository.GetReportsByState(false);
        }

        private bool _estatReport;
        public bool EstatReport {
            get { return _estatReport; }
            set {
                _estatReport = value; NotifyPropertyChanged();
                if (_estatReport == true)
                {
                    Reports = ReportsRepository.GetReportsByState(true);
                    DescripcioReport = "";
                }
                else
                {
                    Reports = ReportsRepository.GetReportsByState(false);
                    DescripcioReport = "";
                }

            }
        }

        private string _descripcioReport;
        public string DescripcioReport {
            get { return _descripcioReport; }
            set {
                _descripcioReport = value; NotifyPropertyChanged();
            }
        }

        #endregion
        #region Pacts

        private List<Pact> _pacts;
        public List<Pact> Pacts {
            get { return _pacts; }
            set { _pacts = value; NotifyPropertyChanged(); }
        }

        private void PactsPopulate()
        {
            TitolPact = "";
            DescripcioPact = "";
            Pacts = PactsRepository.GetAllPacts();

        }

        private string _titolPact;
        public string TitolPact {
            get { return _titolPact; }
            set {
                _titolPact = value; NotifyPropertyChanged();
            }
        }

        private string _descripcioPact;
        public string DescripcioPact {
            get { return _descripcioPact; }
            set {
                _descripcioPact = value; NotifyPropertyChanged();
            }
        }

        private DateTime _dataIniciPact;
        public DateTime DataIniciPact {
            get { return _dataIniciPact; }
            set {
                _dataIniciPact = value; NotifyPropertyChanged();
            }
        }

        private DateTime _dataFinalPact;
        public DateTime DataFinalPact {
            get { return _dataFinalPact; }
            set {
                _dataFinalPact = value; NotifyPropertyChanged();
            }
        }

        #endregion
        #region Bans

        private List<Ban> _bans;
        public List<Ban> Bans {
            get { return _bans; }
            set { _bans = value; NotifyPropertyChanged(); }
        }

        private void BansPopulate()
        {

            Bans = BansRepository.GetAllBans();

        }

        #endregion
        #region Categories



        #endregion
        #region CardHome
        public SeriesCollection LastHourSeries { get; set; }

        public double LastLecture {
            get { return _lastLecture; }
            set {
                _lastLecture = value;
                NotifyPropertyChanged("LastLecture");
            }
        }

        private void SetLecture()
        {
            var target = ((ChartValues<ObservableValue>)LastHourSeries[0].Values).Last().Value;
            var step = (target - _lastLecture) / 4;
#if NET40
                Task.Factory.StartNew(() =>
                {
                    for (var i = 0; i < 4; i++)
                    {
                        Thread.Sleep(100);
                        LastLecture += step;
                    }
                    LastLecture = target;
                });
#endif

            Task.Run(() =>
            {
                for (var i = 0; i < 4; i++)
                {
                    Thread.Sleep(100);
                    LastLecture += step;
                }
                LastLecture = target;
            });

        }

        private void UpdateOnclick(object sender, RoutedEventArgs e)
        {
            //TimePowerChart.Update(true);
        }
        #endregion


    }

}

