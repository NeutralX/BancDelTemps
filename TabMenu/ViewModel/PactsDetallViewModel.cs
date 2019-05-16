using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BancDelTemps.Model;
using BancDelTemps.Model.Class;
using BancDelTemps.View;

namespace BancDelTemps.ViewModel
{
    class PactsDetallViewModel : INotifyPropertyChanged
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


        public ICommand ButtonCloseApp { get; set; }
        public ICommand ButtonSaveChanges { get; set; }
        public ICommand ButtonDiscardChanges { get; set; }
        public ICommand ButtonShowAuthor { get; set; }
        public ICommand ButtonShowPost { get; set; }
        public ICommand ButtonShowParticipant { get; set; }
        public Pact Pact { get; set; }

        public PactsDetallViewModel()
        {
        }

        public PactsDetallViewModel(Pact pact)
        {
            Pact = pact;
            ButtonCloseApp = new PactsDetallViewModel.RelayCommand(o => Application.Current.Windows[1].Close());
            ButtonSaveChanges = new PactsDetallViewModel.RelayCommand(o => saveChanges());
            ButtonDiscardChanges = new PactsDetallViewModel.RelayCommand(o => discardChanges());
            ButtonShowAuthor = new PactsDetallViewModel.RelayCommand(o => authorDetails());
            ButtonShowPost = new PactsDetallViewModel.RelayCommand(o => postDetails());
            ButtonShowParticipant = new PactsDetallViewModel.RelayCommand(o => participantDetails());

            TitlePact = pact.title;
            DescriptionPact = pact.description;
            DateCreatedPact = DateTime.Parse(pact.date_created);
            DateFinishedPact = DateTime.Parse(pact.date_finished);
        }

        public void saveChanges()
        {
            Pact pNew = new Pact();
            pNew.title = TitlePact;
            pNew.description = DescriptionPact;
            pNew.date_created = DateCreatedPact.ToString("dd-MM-yyyy");
            pNew.date_finished = DateFinishedPact.ToString("dd-MM-yyyy");
            pNew.Id_Author = Pact.Id_Author;
            pNew.Id_NoAuthor = Pact.Id_NoAuthor;
            pNew.Id_Pact = Pact.Id_Pact;
            PactsRepository.UpdatePact(pNew);
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

        public void authorDetails()
        {
            Informacio i = new Informacio(UsersRepository.GetUser(Pact.Id_Author));
            i.ShowDialog();
        }

        public void postDetails()
        {
            Informacio i = new Informacio(PostsRepository.GetPostById(Pact.Posts_Id_Post));
            i.ShowDialog();
        }

        public void participantDetails()
        {
            Informacio i = new Informacio(UsersRepository.GetUser(Pact.Id_NoAuthor));
            i.ShowDialog();
        }

        private string _titlePact;
        public string TitlePact
        {
            get { return _titlePact; }
            set
            {
                _titlePact = value; NotifyPropertyChanged();
            }
        }

        private string _descriptionPact;
        public string DescriptionPact
        {
            get { return _descriptionPact; }
            set
            {
                _descriptionPact = value; NotifyPropertyChanged();
            }
        }

        private DateTime _dateCreatedPact;
        public DateTime DateCreatedPact
        {
            get { return _dateCreatedPact; }
            set
            {
                _dateCreatedPact = value; NotifyPropertyChanged();
            }
        }

        private DateTime _dateFinishedPact;
        public DateTime DateFinishedPact
        {
            get { return _dateFinishedPact; }
            set
            {
                _dateFinishedPact = value; NotifyPropertyChanged();
            }
        }
    }
}
